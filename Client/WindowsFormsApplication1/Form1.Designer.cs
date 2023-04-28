namespace WindowsFormsApplication1
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.IP_textBox = new System.Windows.Forms.TextBox();
            this.Port_textBox = new System.Windows.Forms.TextBox();
            this.output_box = new System.Windows.Forms.RichTextBox();
            this.connect_button = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.username_textBox = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.sweet_richTextBox = new System.Windows.Forms.RichTextBox();
            this.post_button = new System.Windows.Forms.Button();
            this.request_button = new System.Windows.Forms.Button();
            this.disconnect_button = new System.Windows.Forms.Button();
            this.request_usernames = new System.Windows.Forms.Button();
            this.follow_textBox = new System.Windows.Forms.TextBox();
            this.follow_button = new System.Windows.Forms.Button();
            this.followed_sweets_button = new System.Windows.Forms.Button();
            this.block_textBox = new System.Windows.Forms.TextBox();
            this.block_button = new System.Windows.Forms.Button();
            this.request_followers = new System.Windows.Forms.Button();
            this.request_following = new System.Windows.Forms.Button();
            this.deleteId_TextBox = new System.Windows.Forms.TextBox();
            this.delete_button = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(41, 57);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(24, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "IP:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(41, 96);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(38, 17);
            this.label2.TabIndex = 1;
            this.label2.Text = "Port:";
            // 
            // IP_textBox
            // 
            this.IP_textBox.Location = new System.Drawing.Point(133, 50);
            this.IP_textBox.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.IP_textBox.Name = "IP_textBox";
            this.IP_textBox.Size = new System.Drawing.Size(241, 22);
            this.IP_textBox.TabIndex = 2;
            // 
            // Port_textBox
            // 
            this.Port_textBox.Location = new System.Drawing.Point(133, 96);
            this.Port_textBox.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Port_textBox.Name = "Port_textBox";
            this.Port_textBox.Size = new System.Drawing.Size(241, 22);
            this.Port_textBox.TabIndex = 3;
            // 
            // output_box
            // 
            this.output_box.Location = new System.Drawing.Point(528, 53);
            this.output_box.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.output_box.Name = "output_box";
            this.output_box.Size = new System.Drawing.Size(480, 390);
            this.output_box.TabIndex = 4;
            this.output_box.Text = "";
            // 
            // connect_button
            // 
            this.connect_button.Location = new System.Drawing.Point(144, 192);
            this.connect_button.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.connect_button.Name = "connect_button";
            this.connect_button.Size = new System.Drawing.Size(75, 23);
            this.connect_button.TabIndex = 5;
            this.connect_button.Text = "connect";
            this.connect_button.UseVisualStyleBackColor = true;
            this.connect_button.Click += new System.EventHandler(this.connect_button_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(41, 134);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(77, 17);
            this.label3.TabIndex = 6;
            this.label3.Text = "Username:";
            // 
            // username_textBox
            // 
            this.username_textBox.Location = new System.Drawing.Point(133, 133);
            this.username_textBox.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.username_textBox.Name = "username_textBox";
            this.username_textBox.Size = new System.Drawing.Size(241, 22);
            this.username_textBox.TabIndex = 7;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(41, 261);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(50, 17);
            this.label4.TabIndex = 8;
            this.label4.Text = "Sweet:";
            // 
            // sweet_richTextBox
            // 
            this.sweet_richTextBox.Location = new System.Drawing.Point(133, 228);
            this.sweet_richTextBox.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.sweet_richTextBox.Name = "sweet_richTextBox";
            this.sweet_richTextBox.Size = new System.Drawing.Size(356, 118);
            this.sweet_richTextBox.TabIndex = 9;
            this.sweet_richTextBox.Text = "";
            // 
            // post_button
            // 
            this.post_button.Enabled = false;
            this.post_button.Location = new System.Drawing.Point(387, 368);
            this.post_button.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.post_button.Name = "post_button";
            this.post_button.Size = new System.Drawing.Size(102, 30);
            this.post_button.TabIndex = 10;
            this.post_button.Text = "post";
            this.post_button.UseVisualStyleBackColor = true;
            this.post_button.Click += new System.EventHandler(this.post_button_Click);
            // 
            // request_button
            // 
            this.request_button.Enabled = false;
            this.request_button.Location = new System.Drawing.Point(133, 411);
            this.request_button.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.request_button.Name = "request_button";
            this.request_button.Size = new System.Drawing.Size(144, 32);
            this.request_button.TabIndex = 11;
            this.request_button.Text = "request sweets";
            this.request_button.UseVisualStyleBackColor = true;
            this.request_button.Click += new System.EventHandler(this.request_button_Click);
            // 
            // disconnect_button
            // 
            this.disconnect_button.Enabled = false;
            this.disconnect_button.Location = new System.Drawing.Point(272, 192);
            this.disconnect_button.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.disconnect_button.Name = "disconnect_button";
            this.disconnect_button.Size = new System.Drawing.Size(103, 23);
            this.disconnect_button.TabIndex = 12;
            this.disconnect_button.Text = "disconnect";
            this.disconnect_button.UseVisualStyleBackColor = true;
            this.disconnect_button.Click += new System.EventHandler(this.disconnect_button_Click);
            // 
            // request_usernames
            // 
            this.request_usernames.Enabled = false;
            this.request_usernames.Location = new System.Drawing.Point(345, 411);
            this.request_usernames.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.request_usernames.Name = "request_usernames";
            this.request_usernames.Size = new System.Drawing.Size(144, 32);
            this.request_usernames.TabIndex = 13;
            this.request_usernames.Text = "request usernames";
            this.request_usernames.UseVisualStyleBackColor = true;
            this.request_usernames.Click += new System.EventHandler(this.request_usernames_Click);
            // 
            // follow_textBox
            // 
            this.follow_textBox.Location = new System.Drawing.Point(15, 305);
            this.follow_textBox.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.follow_textBox.Name = "follow_textBox";
            this.follow_textBox.Size = new System.Drawing.Size(76, 22);
            this.follow_textBox.TabIndex = 14;
            // 
            // follow_button
            // 
            this.follow_button.Enabled = false;
            this.follow_button.Location = new System.Drawing.Point(15, 331);
            this.follow_button.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.follow_button.Name = "follow_button";
            this.follow_button.Size = new System.Drawing.Size(75, 30);
            this.follow_button.TabIndex = 15;
            this.follow_button.Text = "follow";
            this.follow_button.UseVisualStyleBackColor = true;
            this.follow_button.Click += new System.EventHandler(this.follow_button_Click);
            // 
            // followed_sweets_button
            // 
            this.followed_sweets_button.Enabled = false;
            this.followed_sweets_button.Location = new System.Drawing.Point(133, 366);
            this.followed_sweets_button.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.followed_sweets_button.Name = "followed_sweets_button";
            this.followed_sweets_button.Size = new System.Drawing.Size(228, 32);
            this.followed_sweets_button.TabIndex = 16;
            this.followed_sweets_button.Text = "request followed sweets";
            this.followed_sweets_button.UseVisualStyleBackColor = true;
            this.followed_sweets_button.Click += new System.EventHandler(this.followed_sweets_button_Click);
            // 
            // block_textBox
            // 
            this.block_textBox.Location = new System.Drawing.Point(15, 400);
            this.block_textBox.Name = "block_textBox";
            this.block_textBox.Size = new System.Drawing.Size(75, 22);
            this.block_textBox.TabIndex = 17;
            // 
            // block_button
            // 
            this.block_button.Enabled = false;
            this.block_button.Location = new System.Drawing.Point(15, 428);
            this.block_button.Name = "block_button";
            this.block_button.Size = new System.Drawing.Size(73, 33);
            this.block_button.TabIndex = 18;
            this.block_button.Text = "block";
            this.block_button.UseVisualStyleBackColor = true;
            this.block_button.Click += new System.EventHandler(this.block_button_Click);
            // 
            // request_followers
            // 
            this.request_followers.Enabled = false;
            this.request_followers.Location = new System.Drawing.Point(133, 458);
            this.request_followers.Name = "request_followers";
            this.request_followers.Size = new System.Drawing.Size(144, 26);
            this.request_followers.TabIndex = 19;
            this.request_followers.Text = "request followers";
            this.request_followers.UseVisualStyleBackColor = true;
            this.request_followers.Click += new System.EventHandler(this.request_followers_Click);
            // 
            // request_following
            // 
            this.request_following.Enabled = false;
            this.request_following.Location = new System.Drawing.Point(345, 458);
            this.request_following.Name = "request_following";
            this.request_following.Size = new System.Drawing.Size(144, 23);
            this.request_following.TabIndex = 20;
            this.request_following.Text = "request following";
            this.request_following.UseVisualStyleBackColor = true;
            this.request_following.Click += new System.EventHandler(this.request_following_Click);
            // 
            // deleteId_TextBox
            // 
            this.deleteId_TextBox.Location = new System.Drawing.Point(18, 500);
            this.deleteId_TextBox.Name = "deleteId_TextBox";
            this.deleteId_TextBox.Size = new System.Drawing.Size(73, 22);
            this.deleteId_TextBox.TabIndex = 21;
            // 
            // delete_button
            // 
            this.delete_button.Enabled = false;
            this.delete_button.Location = new System.Drawing.Point(114, 494);
            this.delete_button.Name = "delete_button";
            this.delete_button.Size = new System.Drawing.Size(153, 34);
            this.delete_button.TabIndex = 22;
            this.delete_button.Text = "Delete Sweet";
            this.delete_button.UseVisualStyleBackColor = true;
            this.delete_button.Click += new System.EventHandler(this.delete_button_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1040, 546);
            this.Controls.Add(this.delete_button);
            this.Controls.Add(this.deleteId_TextBox);
            this.Controls.Add(this.request_following);
            this.Controls.Add(this.request_followers);
            this.Controls.Add(this.block_button);
            this.Controls.Add(this.block_textBox);
            this.Controls.Add(this.followed_sweets_button);
            this.Controls.Add(this.follow_button);
            this.Controls.Add(this.follow_textBox);
            this.Controls.Add(this.request_usernames);
            this.Controls.Add(this.disconnect_button);
            this.Controls.Add(this.request_button);
            this.Controls.Add(this.post_button);
            this.Controls.Add(this.sweet_richTextBox);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.username_textBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.connect_button);
            this.Controls.Add(this.output_box);
            this.Controls.Add(this.Port_textBox);
            this.Controls.Add(this.IP_textBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox IP_textBox;
        private System.Windows.Forms.TextBox Port_textBox;
        private System.Windows.Forms.RichTextBox output_box;
        private System.Windows.Forms.Button connect_button;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox username_textBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.RichTextBox sweet_richTextBox;
        private System.Windows.Forms.Button post_button;
        private System.Windows.Forms.Button request_button;
        private System.Windows.Forms.Button disconnect_button;
        private System.Windows.Forms.Button request_usernames;
        private System.Windows.Forms.TextBox follow_textBox;
        private System.Windows.Forms.Button follow_button;
        private System.Windows.Forms.Button followed_sweets_button;
        private System.Windows.Forms.TextBox block_textBox;
        private System.Windows.Forms.Button block_button;
        private System.Windows.Forms.Button request_followers;
        private System.Windows.Forms.Button request_following;
        private System.Windows.Forms.TextBox deleteId_TextBox;
        private System.Windows.Forms.Button delete_button;
    }
}

