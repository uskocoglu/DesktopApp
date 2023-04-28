using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication2
{
    public partial class Form1 : Form
    {
        Socket serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        // create a list to save client sockets
        List<Socket> clientSockets = new List<Socket>(); 
        // client names list to make sure once connected, the same username cannot connect again
        List<string> clientNames = new List<string>(); 

        bool terminating = false;
        bool listening = false;
        bool userFound = false;

        // sweet id
        int id;
        // create the sweet db in current folder
        string file_path = "";

        string pathForDB = "";
        string pathForID = "";
        string pathForFL = ""; //follow list
        string pathForUsers = "user-db.txt";
        string pathForBlock = "";
        // append the new sweet data to previous sweet data
        string previousDB = "";
        string previousFL = "";
        string previousFollowers = "";
        string pathForFollowers = "";
        string previousBlock = "";

        public Form1()
        {
            Control.CheckForIllegalCrossThreadCalls = false;  
            this.FormClosing += new FormClosingEventHandler(Form1_FormClosing); 
            InitializeComponent();
        }

        private void listen_button_Click(object sender, EventArgs e)
        {
            int serverPort;

            if (Int32.TryParse(port_textBox.Text, out serverPort))
            {
                IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, serverPort);
                serverSocket.Bind(endPoint);
                serverSocket.Listen(3);

                listening = true;
                listen_button.Enabled = false;                

                // create the sweet database if the server is started for the first time
                pathForDB = file_path + "sweetDB.txt";
                pathForID = file_path + "id.txt";
                pathForFL = file_path + "followlistDB.txt";
                pathForFollowers = file_path + "followerlistDB.txt";
                pathForBlock = file_path + "blocklistDB.txt";
                if (!File.Exists(@pathForDB))
                {
                    File.WriteAllText(@pathForDB, "");
                    File.WriteAllText(@pathForID, "0");
                    File.WriteAllText(@pathForFL, "");
                    File.WriteAllText(@pathForBlock, "");
                    id = 0;
                }
                // if id file already exists, get the new sweet id from the file
                else
                {
                    var lines = File.ReadAllLines(@pathForID);
                    id = Int32.Parse(lines[0]);
                }
                if (!File.Exists(@pathForFL))
                {
                    File.WriteAllText(@pathForFL, "");
                }

                if (!File.Exists(@pathForFollowers))
                {
                    File.WriteAllText(@pathForFollowers, "");
                }
                if (!File.Exists(@pathForBlock))
                {
                    File.WriteAllText(@pathForBlock, "");
                }


                // start accepting clients
                Thread acceptThread = new Thread(AcceptPort);
                acceptThread.IsBackground = true;
                acceptThread.Start();

                timeline.AppendText("Started listening on port: " + serverPort + "\n");
            }
            else
            {
                timeline.AppendText("Please check port number \n");
            }
        }

        private void AcceptPort()
        {
            while (listening)
            {
                try
                {
                    // create a new socket for the new client
                    Socket newClient = serverSocket.Accept(); 

                    // get the name of the user from the client
                    Byte[] name_buffer = new Byte[1024];
                    newClient.Receive(name_buffer); 
                    string receivedName = Encoding.Default.GetString(name_buffer);
                    receivedName = receivedName.Substring(0, receivedName.IndexOf("\0"));

                    // create a new string based on the user status and send this string to the client
                    Byte[] usernameCheckBuffer = new Byte[4000];
                    string usernameCheckMessage = "";

                    // read the whole users file and check if the username entered by the client exists
                    var lines = File.ReadAllLines(@pathForUsers);
                    List<String> names = new List<String>();
                    for (int i = 0; i < lines.Length; i++)
                    {
                        var name = lines[i];
                        names.Add(name);
                    }
                    foreach (var name in names)
                    {
                        if (name == receivedName)
                        {
                            userFound = true;
                            break;
                        }
                    }                    
                    // if the new username already exists in the connected clients' list, act accordingly
                    if (clientNames.Contains(receivedName))
                    {
                        timeline.AppendText("Client named " + receivedName + " is already connected!\n");

                        // send the message to the client that the given user is already connected
                        usernameCheckMessage = "Already exists";
                        userFound = false;
                        usernameCheckBuffer = Encoding.Default.GetBytes(usernameCheckMessage);
                        newClient.Send(usernameCheckBuffer);

                        // close the new client socket
                        newClient.Close();
                    }
                    else if (userFound == false)
                    {
                        timeline.AppendText("A user with username " + receivedName + " does not exist in user database.\n");

                        // send the message to the client that the given user is not found in the users database
                        usernameCheckMessage = "Not found";
                        usernameCheckBuffer = Encoding.Default.GetBytes(usernameCheckMessage);
                        newClient.Send(usernameCheckBuffer);
                    }
                    else
                    {
                        // add client to our clients list and its socket to sockets list
                        clientSockets.Add(newClient);  
                        clientNames.Add(receivedName);

                        timeline.AppendText("Client named " + receivedName + " gets connected now!\n");

                        // send the message to the client indicating that the connection was successful
                        usernameCheckMessage = "OK";
                        usernameCheckBuffer = Encoding.Default.GetBytes(usernameCheckMessage);
                        newClient.Send(usernameCheckBuffer);

                        // now we can start receiving sweets from the client
                        Thread receiveThread = new Thread(() => ReceiveSweet(newClient, receivedName));
                        receiveThread.IsBackground = true;
                        receiveThread.Start();

                        // update the userFound bool so that new connections are evaulated 
                        userFound = false;
                    }
                }
                catch
                {
                    if (terminating)
                    {
                        listening = false;
                    }
                    else
                    {
                        // if server stops working
                        timeline.AppendText("\nSocket stopped working.\n");
                        port_textBox.Enabled = true;
                        listen_button.Enabled = true;
                        listening = false;
                        userFound = false;
                    }
                }
            }
            
        }

        private void ReceiveSweet(Socket thisClient, string clientName)
        {
            bool connected = true;

            while (connected && !terminating)
            {
                try
                {
                    // get sweet data from the client
                    Byte[] sweetBuffer = new Byte[64000000];
                    thisClient.Receive(sweetBuffer);
                    string sweet = Encoding.Default.GetString(sweetBuffer);
                    sweet = sweet.Substring(0, sweet.IndexOf("\0"));

                    // if the sweet is şşşş, disconnect the client from the server
                    if (sweet == "ssss")
                    {
                        if (!terminating)
                        {
                            timeline.AppendText("Client named " + clientName + " has disconnected from the server.\n");
                        }

                        // close the socket and update socket and connected users' lists
                        thisClient.Close();
                        clientSockets.Remove(thisClient);
                        clientNames.Remove(clientName);
                        connected = false;
                    }
                    // if the sweet is ğğğğ, the client gets the sweets posted by other users
                    else if (sweet == "gggg")
                    {
                        timeline.AppendText(clientName + " requested sweets of others\n\n");

                        // read the sweet database and add the sweets of other users to a string called requestedSweets
                        var lines = File.ReadLines(@pathForDB);
                        string requestedSweets = "";
                        bool not_me = false;
                        foreach (var line in lines)
                        {
                            // if the current line is an empty one,continue with the next line
                            if (line == "\n" || line == "")
                            {
                                continue;
                            }
                            // if the current line starts with a # it means this is a sweet header 
                            // and if the username is nt the same as the user requesting sweets, 
                            // we will add the sweet of this user into requestedSweets variable
                            else if (line[0] == '#')
                            {
                                string[] words = line.Split(' ');

                                if (words[1] != clientName)
                                {
                                    not_me = true;
                                    requestedSweets += line + "\n";
                                    continue;
                                }
                            }
                            else
                            {
                                // check if we know from the previous line that the current line contains the sweet of other users
                                if (not_me == true)
                                {
                                    requestedSweets += line + "\n";
                                    not_me = false;
                                }
                            }
                        }

                        Byte[] requestBuffer = new Byte[64000000];

                        // if there are no sweets on the database
                        if (requestedSweets == "")
                        {
                            requestBuffer = Encoding.Default.GetBytes("Other users have not sent any sweet!\n");
                        }
                        // if there are sweets of other users on the database, send them to the requesting client
                        else
                        {
                            requestBuffer = Encoding.Default.GetBytes(requestedSweets);
                        }
                        thisClient.Send(requestBuffer);
                    }
                    // if the sweet is ççğğ, the client gets the sweets posted by other users
                    else if (sweet == "ççgg")
                    {
                        timeline.AppendText(clientName + " requested sweets of the users that " + clientName + " follows\n\n");

                        // read the sweet database and add the sweets of other users to a string called requestedSweets
                        var lines = File.ReadLines(@pathForDB);
                        string requestedSweets = "";
                        bool not_me = false;
                        foreach (var line in lines)
                        {
                            // if the current line is an empty one,continue with the next line
                            if (line == "\n" || line == "")
                            {
                                continue;
                            }
                            // if the current line starts with a # it means this is a sweet header 
                            // and if the username is nt the same as the user requesting sweets, 
                            // we will add the sweet of this user into requestedSweets variable
                            else if (line[0] == '#')
                            {
                                string[] words = line.Split(' ');

                                if (words[1] != clientName)
                                {
                                    var lines2 = File.ReadLines(@pathForFL);
                                    foreach (var line2 in lines2)
                                    {
                                        string[] words2 = line2.Split(' ');
                                        if (clientName == words2[0] && words[1] == words2[3])
                                        {
                                            not_me = true;
                                            requestedSweets += line + "\n";
                                            break;
                                        }
                                    }
                                    continue;
                                }
                            }
                            else
                            {
                                // check if we know from the previous line that the current line contains the sweet of other users
                                if (not_me == true)
                                {
                                    requestedSweets += line + "\n";
                                    not_me = false;
                                }
                            }
                        }

                        Byte[] requestBuffer = new Byte[64000000];

                        // if there are no sweets on the database
                        if (requestedSweets == "")
                        {
                            requestBuffer = Encoding.Default.GetBytes("The users that you follow have not sent any sweet!\n");
                        }
                        // if there are sweets of other users on the database, send them to the requesting client
                        else
                        {
                            requestBuffer = Encoding.Default.GetBytes(requestedSweets);
                        }
                        thisClient.Send(requestBuffer);
                    }
                    else if (sweet == "çççç") //if the sweet is çççç, the client gets the users in the database
                    {
                        timeline.AppendText(clientName + " requested users in the database\n\n");
                        var lines2 = File.ReadAllLines(@pathForUsers);
                        List<String> names2 = new List<String>();
                        string requestedUsernames = "";
                        for (int i = 0; i < lines2.Length; i++)
                        {
                            var name = lines2[i];
                            names2.Add(name);
                            requestedUsernames += name + "\n";
                        }

                        Byte[] requestBuffer2 = new Byte[64000000];
                        requestBuffer2 = Encoding.Default.GetBytes(requestedUsernames);
                        thisClient.Send(requestBuffer2);

                    }
                    else if (sweet[sweet.Length - 1] == 'ç' && sweet[sweet.Length - 2] == 'ü') //if the sweet is ğğüç, the client gets the his/her follower list
                    {
                        var lines5 = File.ReadAllLines(@pathForFL);
                        string flwrlist = "";

                        sweet = sweet.Substring(0, sweet.Length - 5);
                        int pos = sweet.IndexOf(' ');

                        foreach (string line in lines5)
                        {
                            int bosluk1 = line.IndexOf(' ');
                            int bosluk2 = line.IndexOf(' ', bosluk1 + 1);
                            int bosluk3 = line.IndexOf(' ', bosluk2 + 1);
                            string firstUn = line.Substring(0, bosluk1);
                            string secondUn = line.Substring(bosluk3 + 1);

                            if (secondUn == sweet)
                            {
                                flwrlist += firstUn + "\n";
                            }
                        }

                        Byte[] requestBuffer2 = new Byte[64000000];
                        requestBuffer2 = Encoding.Default.GetBytes("Follower List:" + "\n"  + flwrlist);
                        thisClient.Send(requestBuffer2);

                    }

                    else if (sweet[sweet.Length - 1] == 'ç' && sweet[sweet.Length - 2] == 'ö') //if the sweet is ğğöç, the client gets the his/her following list
                    {
                        var lines5 = File.ReadAllLines(@pathForFL);
                        string flwrlist = "";

                        sweet = sweet.Substring(0, sweet.Length - 5);
                        int pos = sweet.IndexOf(' ');

                        foreach (string line in lines5)
                        {
                            int bosluk1 = line.IndexOf(' ');
                            int bosluk2 = line.IndexOf(' ', bosluk1 + 1);
                            int bosluk3 = line.IndexOf(' ', bosluk2 + 1);
                            string firstUn = line.Substring(0, bosluk1);
                            string secondUn = line.Substring(bosluk3 + 1);

                            if (firstUn == sweet)
                            {
                                flwrlist += secondUn + "\n";
                            }
                        }

                        Byte[] requestBuffer2 = new Byte[64000000];
                        requestBuffer2 = Encoding.Default.GetBytes("Following List:" + "\n" + flwrlist);
                        thisClient.Send(requestBuffer2);

                    }
                    //if the sweet is şöşö, server deletes the sweet with that id if its owner is the client that request the delete
                    else if ((sweet[sweet.Length - 1] == 'ö') && (sweet[sweet.Length - 2] == 'ç'))
                    {
                        sweet = sweet.Substring(0, sweet.Length - 5);
                        int pos = sweet.IndexOf(' ');
                        string deleteID = sweet.Substring(0, pos);
                        // get the id of sweet to be deleted => deleteID (bufferdan gelen id)
                        var lines8 = File.ReadAllLines(@pathForDB);
                        List<String> newSweets = new List<String>();
                        int num = 0;
                        bool isFound = false;
                        for (int i = 0; i < lines8.Length; i++)
                        {
                            var sweetLine = lines8[i];
                            

                            if(sweetLine != "")
                            {
                                string first = sweetLine.Substring(0, 1);
                                if (first == "#")
                                {
                                    int boslk1 = sweetLine.IndexOf(' ');
                                    var checkID = sweetLine.Substring(1, boslk1 - 1);
                                    if (checkID == deleteID)
                                    {
                                        isFound = true;
                                        int boslk2 = sweetLine.IndexOf(' ', boslk1 + 1);
                                        string userCheck = sweetLine.Substring(boslk1 + 1, boslk2 - (boslk1 + 1));
                                        if (clientName != userCheck)
                                        {
                                            timeline.AppendText("ERROR! You cannot delete this sweet.\n");
                                            Byte[] requestBuffer7 = new Byte[64000000];
                                            requestBuffer7 = Encoding.Default.GetBytes("ERROR! You cannot delete this sweet.\n");
                                            thisClient.Send(requestBuffer7);
                                            // you cannot delete this sweet
                                            break;
                                        }
                                        else
                                        {
                                            for (int k = 0; k < lines8.Length; k++)
                                            {
                                                var newLine = lines8[k];
                                                
                                                if(newLine != "")
                                                {
                                                    string first2 = newLine.Substring(0, 1);
                                                    if (first2 == "#")
                                                    {
                                                        int boslk3 = newLine.IndexOf(' ');
                                                        var checkID2 = newLine.Substring(1, boslk3 - 1);
                                                        if (checkID2 == deleteID)
                                                        {
                                                            num += 1;
                                                            continue;
                                                        }
                                                        else
                                                        {
                                                            newSweets.Add(newLine);
                                                        }
                                                    }
                                                    else if (num == 1)
                                                    {
                                                        num = 0;
                                                        continue;
                                                    }
                                                    else
                                                    {
                                                        newSweets.Add(newLine);
                                                    }
                                                }                                                
                                            }
                                            int g = 0;
                                            previousDB = "";
                                            foreach (var line in newSweets)
                                            {
                                                
                                                if (g != 0 && g % 2 == 0)
                                                {
                                                    previousDB += "" + "\n";                                                    
                                                }
                                                previousDB += line + "\n";
                                                g++;
                                            }
                                            File.WriteAllText(@pathForDB, previousDB);
                                            timeline.AppendText("Sweet successfully deleted\n");
                                            Byte[] requestBuffer10 = new Byte[64000000];
                                            requestBuffer10 = Encoding.Default.GetBytes("Sweet successfully deleted.\n");
                                            thisClient.Send(requestBuffer10);
                                            break;
                                        }
                                    }
                                    else
                                    {
                                        continue;
                                    }
                                    
                                }
                            }                      
                        }

                        if(isFound == false)
                        {
                            timeline.AppendText("ERROR! There is no sweet with this ID.\n");
                            Byte[] requestBuffer9 = new Byte[64000000];
                            requestBuffer9 = Encoding.Default.GetBytes("ERROR! There is no sweet with this ID.\n");
                            thisClient.Send(requestBuffer9);                           
                        }
                    }
                    //if the sweet is "user ö" the client starts following user if user exists, not blocked by client and not blocked by user
                    else if (sweet[sweet.Length - 1] == 'ö')
                    {
                        bool CanFollow = false;
                        int exist = 0;
                        sweet = sweet.Substring(0, sweet.Length - 2);
                        int pos = sweet.IndexOf(' ');
                        string UnToFollow = sweet.Substring(0, pos);
                        string Un = sweet.Substring(pos + 1);

                        var lines2 = File.ReadAllLines(@pathForUsers);
                        List<String> names2 = new List<String>();
                        for (int i = 0; i < lines2.Length; i++)
                        {
                            var name = lines2[i];
                            names2.Add(name);
                        }
                        foreach (var name in names2)
                        {
                            if (name == UnToFollow && name != Un)
                            {
                                CanFollow = true;
                                break;

                            }
                        }
                        var lines3 = File.ReadLines(@pathForFL);
                        foreach (string line in lines3)
                        {
                            int bosluk1 = line.IndexOf(' ');
                            int bosluk2 = line.IndexOf(' ', bosluk1 + 1);
                            int bosluk3 = line.IndexOf(' ', bosluk2 + 1);
                            string firstUn = line.Substring(0, bosluk1);
                            string secondUn = line.Substring(bosluk3 + 1);

                            if (firstUn == Un && secondUn == UnToFollow)
                            {
                                CanFollow = false;
                                exist = 1;
                                break;
                            }
                        }
                        var lines4 = File.ReadLines(@pathForBlock);
                        foreach (string line in lines4)
                        {
                            int bosluk1 = line.IndexOf(' ');
                            int bosluk2 = line.IndexOf(' ', bosluk1 + 1);
                            int bosluk3 = line.IndexOf(' ', bosluk2 + 1);
                            string firstUn = line.Substring(0, bosluk1);
                            string secondUn = line.Substring(bosluk3 + 1);

                            if (secondUn == Un && firstUn == UnToFollow)
                            {
                                CanFollow = false;
                                exist = 2;
                                break;
                            }
                        }
                        //the client can follow the specified user
                        if (CanFollow == true)
                        {
                            timeline.AppendText(Un + " started following " + UnToFollow + "\n");
                            previousFL = File.ReadAllText(@pathForFL);
                            previousFL += Un + " started following " + UnToFollow + "\n";
                            File.WriteAllText(@pathForFL, previousFL);
                            previousFollowers = File.ReadAllText(@pathForFollowers);

                            Byte[] requestBuffer2 = new Byte[64000000];
                            requestBuffer2 = Encoding.Default.GetBytes(Un + " started following " + UnToFollow + "\n");
                            thisClient.Send(requestBuffer2);

                        }
                        //the client can not follow the user because it does not exist in the users database
                        else if (CanFollow == false && exist == 0 && Un != UnToFollow)
                        {
                            timeline.AppendText("The username that " + Un + " wants to follow does not exist.\n");
                            Byte[] requestBuffer2 = new Byte[64000000];
                            requestBuffer2 = Encoding.Default.GetBytes("The username that " + Un + " wants to follow does not exist.\n");
                            thisClient.Send(requestBuffer2);
                        }
                        //the client can not follow themselves
                        else if (CanFollow == false && exist == 0 && Un == UnToFollow)
                        {
                            timeline.AppendText("ERROR! You cannot follow yourselves.\n");
                            Byte[] requestBuffer2 = new Byte[64000000];
                            requestBuffer2 = Encoding.Default.GetBytes("ERROR! You cannot follow yourselves.\n");
                            thisClient.Send(requestBuffer2);
                        }
                        //the client have already followed the specified user
                        else if (CanFollow == false && exist == 1)
                        {
                            timeline.AppendText(Un + " have already followed " + UnToFollow + "\n");
                            Byte[] requestBuffer2 = new Byte[64000000];
                            requestBuffer2 = Encoding.Default.GetBytes(Un + " have already followed " + UnToFollow + "\n");
                            thisClient.Send(requestBuffer2);
                        }
                        //the client have blocked the specified user
                        else if (CanFollow == false && exist == 2)
                        {
                            timeline.AppendText("ERROR! " +UnToFollow+" has blocked "+Un+" so he/she cannot follow him/her.\n");
                            Byte[] requestBuffer2 = new Byte[64000000];
                            requestBuffer2 = Encoding.Default.GetBytes("ERROR! This person has blocked you so you cannot follow him/her.\n");
                            thisClient.Send(requestBuffer2);
                        }


                    }
                    //if the sweet is "user ü", the client wants to block the specified user
                    else if (sweet[sweet.Length - 1] == 'ü')
                    {
                        bool CanBlock = false;
                        int exist2 = 0;
                        sweet = sweet.Substring(0, sweet.Length - 2);
                        int pos = sweet.IndexOf(' ');
                        string UnToBlock = sweet.Substring(0, pos);
                        string Un = sweet.Substring(pos + 1);
                        //timeline.AppendText(sweet + "\n" + UnToFollow + "\n" + Un + "\n");
                        var lines2 = File.ReadAllLines(@pathForUsers);
                        List<String> names2 = new List<String>();
                        for (int i = 0; i < lines2.Length; i++)
                        {
                            var name = lines2[i];
                            names2.Add(name);
                        }
                        foreach (var name in names2)
                        {
                            if (name == UnToBlock && name != Un)
                            {
                                CanBlock = true;
                                break;

                            }
                        }

                        var lines3 = File.ReadLines(@pathForBlock);
                        foreach (string line in lines3)
                        {
                            int bosluk1 = line.IndexOf(' ');
                            int bosluk2 = line.IndexOf(' ', bosluk1 + 1);
                            int bosluk3 = line.IndexOf(' ', bosluk2 + 1);
                            string firstUn = line.Substring(0, bosluk1);
                            string secondUn = line.Substring(bosluk3 + 1);

                            if (firstUn == Un && secondUn == UnToBlock)
                            {
                                CanBlock = false;
                                exist2 = 1;
                                break;
                            }
                        }

                        //the client can block the specified user
                        if (CanBlock == true)
                        {

                            //removing from the follower list
                            string searchLine = (UnToBlock + " started following " + Un);
                            var lines6 = File.ReadAllLines(@pathForFL);
                            List<String> newLines6 = new List<String>();
                            for (int i = 0; i < lines6.Length; i++)
                            {
                                var newLine = lines6[i];
                                if (newLine != searchLine)
                                {
                                    newLines6.Add(newLine);
                                }
                            }
                            previousFL = "";
                            foreach (var line in newLines6)
                            {
                                previousFL += line + "\n";
                            }
                            File.WriteAllText(@pathForFL, previousFL);
                            


                            timeline.AppendText(Un + " has blocked " + UnToBlock + "\n");
                            previousBlock = File.ReadAllText(@pathForBlock);
                            previousBlock += Un + " has blocked " + UnToBlock + "\n";
                            File.WriteAllText(@pathForBlock, previousBlock);
                            previousFollowers = File.ReadAllText(@pathForBlock);

                            Byte[] requestBuffer2 = new Byte[64000000];
                            requestBuffer2 = Encoding.Default.GetBytes(Un + " has blocked " + UnToBlock + "\n");
                            thisClient.Send(requestBuffer2);

                        }
                        //the specified user that client wants to block doesn't exist
                        else if (CanBlock == false && exist2 == 0 && Un != UnToBlock)
                        {
                            timeline.AppendText("The username that " + Un + " wants to block does not exist.\n");
                            Byte[] requestBuffer2 = new Byte[64000000];
                            requestBuffer2 = Encoding.Default.GetBytes("The username that " + Un + " wants to block does not exist.\n");
                            thisClient.Send(requestBuffer2);
                        }
                        //client tries to block himself/herself
                        else if (CanBlock == false && exist2 == 0 && Un == UnToBlock)
                        {
                            timeline.AppendText("ERROR! You cannot block yourselves.\n");
                            Byte[] requestBuffer2 = new Byte[64000000];
                            requestBuffer2 = Encoding.Default.GetBytes("ERROR! You cannot block yourselves.\n");
                            thisClient.Send(requestBuffer2);
                        }
                        //the specified user has already been blocked
                        else if (CanBlock == false && exist2 == 1)
                        {
                            timeline.AppendText(Un + " have already blocked " + UnToBlock + "\n");
                            Byte[] requestBuffer2 = new Byte[64000000];
                            requestBuffer2 = Encoding.Default.GetBytes(Un + " have already blocked " + UnToBlock + "\n");
                            thisClient.Send(requestBuffer2);
                        }
                    }
                    // if the client is posting a sweet, show it on server's rich text box and add it to the sweet database
                    else
                    {
                        timeline.AppendText("#" + id + " " + sweet + "\n");
                        id++;

                        // read the current sweet database into a string
                        previousDB = File.ReadAllText(@pathForDB);
                        previousDB += "#" + id + " " + sweet + "\n";

                        // append new sweet to previously read sweet database
                        File.WriteAllText(@pathForDB, previousDB);
                        File.WriteAllText(@pathForID, id.ToString());
                    }                    
                }
                catch
                {
                    if (!terminating)
                    {
                        timeline.AppendText("Client named " + clientName + " has disconnected from the server.\n");
                    }
                    // close the socket and update socket and connected users' lists
                    thisClient.Close();
                    clientSockets.Remove(thisClient); 
                    clientNames.Remove(clientName);
                    connected = false;
                }
            }
        }

        private void Form1_FormClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            listening = false;
            terminating = true;                                  
            Environment.Exit(0);
        }

        private void port_textBox_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
