using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        bool terminating = false;
        bool connected = false;
        string username;
        Socket clientSocket;

        public Form1()
        {
            Control.CheckForIllegalCrossThreadCalls = false;
            this.FormClosing += new FormClosingEventHandler(Form1_FormClosing);
            InitializeComponent();
        }

        private void connect_button_Click(object sender, EventArgs e)
        {
            clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            int portNum;
            string IP = IP_textBox.Text;
            username = username_textBox.Text;

            // client socket tries to connect to the server
            if ((Int32.TryParse(Port_textBox.Text, out portNum)) && (IP_textBox.Text != "") && (username != ""))
            {
                try
                {
                    // send username to the server to be checked
                    clientSocket.Connect(IP, portNum);
                    Byte[] buffer = new Byte[6400];
                    buffer = Encoding.Default.GetBytes(username);
                    clientSocket.Send(buffer);             
                    
                    // get the status of username from the server (does not exist, already connected etc.)       
                    Byte[] usernameCheckBuffer = new Byte[4000];
                    clientSocket.Receive(usernameCheckBuffer);
                    string usernameCheck = Encoding.Default.GetString(usernameCheckBuffer);
                    usernameCheck = usernameCheck.Substring(0, usernameCheck.IndexOf("\0"));

                    if (usernameCheck == "Already exists")
                    {
                        output_box.AppendText("A user with username " + username + " is already connected. Please try another username.\n");
                    }
                    else if (usernameCheck == "Not found")
                    {
                        output_box.AppendText("A user with username " + username + " does not exist in user database. Please try another username that exist in user database.\n");
                    }
                    // we can connect
                    else 
                    {
                        // update the form 
                        disconnect_button.BackColor = Color.Red;
                        connect_button.BackColor = Color.Gray;
                        Port_textBox.BackColor = Color.LightGreen;
                        IP_textBox.BackColor = Color.LightGreen;
                        username_textBox.BackColor = Color.LightGreen;
                        connect_button.Enabled = false;
                        disconnect_button.Enabled = true;
                        connected = true;
                        request_button.Enabled = true;
                        request_usernames.Enabled = true;
                        post_button.Enabled = true;
                        follow_button.Enabled = true;
                        followed_sweets_button.Enabled = true;
                        Port_textBox.Enabled = false;
                        IP_textBox.Enabled = false;
                        username_textBox.Enabled = false;
                        disconnect_button.Enabled = true;
                        request_followers.Enabled = true;
                        request_following.Enabled = true;
                        block_button.Enabled = true;
                        delete_button.Enabled = true;

                        output_box.AppendText("You are connected to the server.\n");

                        // Thread receiveThread = new Thread(Receive);
                        // receiveThread.Start();
                    }
                }
                catch
                {
                    output_box.AppendText("You could not connect to the server.\n");
                }
            }
            // form inputs are not correct
            else 
            {
                Port_textBox.BackColor = Color.Red;
                IP_textBox.BackColor = Color.Red;
                username_textBox.BackColor = Color.Red;
                output_box.AppendText("Please check the IP address, Port number and username.\n");
            }
        }        

        private void disconnect_button_Click(object sender, EventArgs e)
        {
            // send disconnect string as şşşş to the server
            Byte[] buffer = Encoding.Default.GetBytes("şşşş");
            clientSocket.Send(buffer);

            // update the form
            disconnect_button.BackColor = Color.Gray;
            connect_button.BackColor = Color.Green;
            IP_textBox.BackColor = Color.White;
            username_textBox.BackColor = Color.White;
            Port_textBox.BackColor = Color.White;
            connected = false;
            terminating = true;
            disconnect_button.Enabled = false;
            connect_button.Enabled = true;
            IP_textBox.Enabled = true;
            Port_textBox.Enabled = true;
            username_textBox.Enabled = true;
            post_button.Enabled = false;
            request_button.Enabled = false;
            request_usernames.Enabled = false;
            follow_button.Enabled = false;
            followed_sweets_button.Enabled = false;
            post_button.Enabled = false;

            // close the socket
            clientSocket.Close();

            output_box.AppendText("You are disconnected from server\n");
        }

        private void post_button_Click(object sender, EventArgs e)
        {
            // send the sweet data to the server
            string username = username_textBox.Text;
            string dateAndTime = DateTime.Now.ToString("MM/dd/yyyy h:mm tt");
            string sweet = sweet_richTextBox.Text;

            string whole_text = username + " " + dateAndTime + "\n" + sweet + "\n";                  

            if (sweet != "")
            {
                output_box.AppendText(whole_text);
                Byte[] buffer = Encoding.Default.GetBytes(whole_text);
                clientSocket.Send(buffer);
            }
        }

        private void request_button_Click(object sender, EventArgs e)
        {
            // send ğğğğ to server to request the sweets of others
            Byte[] buffer = Encoding.Default.GetBytes("ğğğğ");
            clientSocket.Send(buffer);

            // get the sweet data the server has sent
            Byte[] sweetsBuffer = new Byte[1000000]; 
            clientSocket.Receive(sweetsBuffer);
            
            string sweets = Encoding.Default.GetString(sweetsBuffer);
            sweets = sweets.Substring(0, sweets.IndexOf("\0"));

            // view the sweets on the rich text box
            output_box.AppendText(sweets);
        }

        private void Form1_FormClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            connected = false;
            terminating = true;
            Environment.Exit(0);
        }

        private void request_usernames_Click(object sender, EventArgs e)
        {
            // send çççç to server to request the usernames 
            Byte[] buffer = Encoding.Default.GetBytes("çççç");
            clientSocket.Send(buffer);

            // get the sweet data the server has sent
            Byte[] usersBuffer = new Byte[1000000];
            clientSocket.Receive(usersBuffer);

            string users = Encoding.Default.GetString(usersBuffer);
            users = users.Substring(0, users.IndexOf("\0"));

            // view the sweets on the rich text box
            output_box.AppendText(users);
        }

        private void follow_button_Click(object sender, EventArgs e)
        {

            string usernameToFollow = follow_textBox.Text;
            usernameToFollow = usernameToFollow + " " + username + " ö";
            Byte[] buffer = new Byte[6400];
            buffer = Encoding.Default.GetBytes(usernameToFollow);
            clientSocket.Send(buffer);

            // get the sweet data the server has sent
            Byte[] followBuffer = new Byte[1000000];
            clientSocket.Receive(followBuffer);

            string fb = Encoding.Default.GetString(followBuffer);
            fb = fb.Substring(0, fb.IndexOf("\0"));

            // view the sweets on the rich text box
            output_box.AppendText(fb);



        }

        private void followed_sweets_button_Click(object sender, EventArgs e)
        {
            // send ççğğ to server to request the sweets of the users that the user follows
            Byte[] buffer = Encoding.Default.GetBytes("ççğğ");
            clientSocket.Send(buffer);

            // get the sweet data the server has sent
            Byte[] sweetsBuffer = new Byte[1000000];
            clientSocket.Receive(sweetsBuffer);

            string sweets = Encoding.Default.GetString(sweetsBuffer);
            sweets = sweets.Substring(0, sweets.IndexOf("\0"));

            // view the sweets on the rich text box
            output_box.AppendText(sweets);
        }
        //block button
        private void block_button_Click(object sender, EventArgs e)
        {
            string usernameToBlock = block_textBox.Text;
            usernameToBlock = usernameToBlock + " " + username + " ü";
            Byte[] buffer = new Byte[6400];
            buffer = Encoding.Default.GetBytes(usernameToBlock);
            clientSocket.Send(buffer);

            // get the sweet data the server has sent
            Byte[] followBuffer = new Byte[1000000];
            clientSocket.Receive(followBuffer);

            string fb = Encoding.Default.GetString(followBuffer);
            fb = fb.Substring(0, fb.IndexOf("\0"));

            // view the sweets on the rich text box
            output_box.AppendText(fb);

        }
        //request followers button
        private void request_followers_Click(object sender, EventArgs e)
        {
            string username = username_textBox.Text;
            username = username + " ğğüç";
            Byte[] buffer = Encoding.Default.GetBytes(username);
            clientSocket.Send(buffer);

            // get the sweet data the server has sent
            Byte[] followerBuffer = new Byte[1000000];
            clientSocket.Receive(followerBuffer);

            string follower = Encoding.Default.GetString(followerBuffer);
            follower = follower.Substring(0, follower.IndexOf("\0"));

            // view the sweets on the rich text box
            output_box.AppendText(follower);
        }
        //request following button
        private void request_following_Click(object sender, EventArgs e)
        {
            string username = username_textBox.Text;
            username = username + " ğğöç";
            Byte[] buffer = Encoding.Default.GetBytes(username);
            clientSocket.Send(buffer);

            // get the sweet data the server has sent
            Byte[] followerBuffer = new Byte[1000000];
            clientSocket.Receive(followerBuffer);

            string follower = Encoding.Default.GetString(followerBuffer);
            follower = follower.Substring(0, follower.IndexOf("\0"));

            // view the sweets on the rich text box
            output_box.AppendText(follower);
        }
        //delete a specified sweet button
        private void delete_button_Click(object sender, EventArgs e)
        {
            string username = username_textBox.Text;
            string id = deleteId_TextBox.Text;
            id = id + " " + username + " şöçö";
            Byte[] buffer = Encoding.Default.GetBytes(id);
            clientSocket.Send(buffer);

            // get the sweet data the server has sent
            Byte[] followerBuffer = new Byte[1000000];
            clientSocket.Receive(followerBuffer);

            string follower = Encoding.Default.GetString(followerBuffer);
            follower = follower.Substring(0, follower.IndexOf("\0"));

            // view the sweets on the rich text box
            output_box.AppendText(follower);
        }

        //private void Receive()
        //{
        //    while (connected)
        //    {
        //        output_box.AppendText("11111111111111111\n");
        //        Byte[] buffer = new Byte[64];
        //        clientSocket.Receive(buffer);

        //        string incomingMessage = Encoding.Default.GetString(buffer);
        //        incomingMessage = incomingMessage.Substring(0, incomingMessage.IndexOf("\0"));
        //        output_box.AppendText("22222222222222222222\n");

        //        if (incomingMessage == "çıkış")
        //        {
        //            disconnect_button.BackColor = Color.Gray;
        //            connect_button.BackColor = Color.MediumSeaGreen;
        //            IP_textBox.BackColor = Color.White;
        //            username_textBox.BackColor = Color.White;
        //            Port_textBox.BackColor = Color.White;
        //            connected = false;
        //            terminating = true;
        //            clientSocket.Close();
        //            disconnect_button.Enabled = false;
        //            connect_button.Enabled = true;
        //            IP_textBox.Enabled = true;
        //            Port_textBox.Enabled = true;
        //            username_textBox.Enabled = true;
        //            post_button.Enabled = false;
        //            request_button.Enabled = false;
        //            post_button.Enabled = false;

        //            output_box.AppendText("You are disconnected from server\n");

        //            clientSocket.Close();
        //            connected = false;
        //        }         

        //    }
        //}
    }
}
