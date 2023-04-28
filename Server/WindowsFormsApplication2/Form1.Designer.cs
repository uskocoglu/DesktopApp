namespace WindowsFormsApplication2
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
            this.port_textBox = new System.Windows.Forms.TextBox();
            this.listen_button = new System.Windows.Forms.Button();
            this.timeline = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(41, 51);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "Port:";
            // 
            // port_textBox
            // 
            this.port_textBox.Location = new System.Drawing.Point(85, 48);
            this.port_textBox.Name = "port_textBox";
            this.port_textBox.Size = new System.Drawing.Size(156, 22);
            this.port_textBox.TabIndex = 1;
            this.port_textBox.TextChanged += new System.EventHandler(this.port_textBox_TextChanged);
            // 
            // listen_button
            // 
            this.listen_button.Location = new System.Drawing.Point(266, 47);
            this.listen_button.Name = "listen_button";
            this.listen_button.Size = new System.Drawing.Size(75, 23);
            this.listen_button.TabIndex = 2;
            this.listen_button.Text = "listen";
            this.listen_button.UseVisualStyleBackColor = true;
            this.listen_button.Click += new System.EventHandler(this.listen_button_Click);
            // 
            // timeline
            // 
            this.timeline.Location = new System.Drawing.Point(429, 52);
            this.timeline.Name = "timeline";
            this.timeline.Size = new System.Drawing.Size(455, 387);
            this.timeline.TabIndex = 3;
            this.timeline.Text = "";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(924, 473);
            this.Controls.Add(this.timeline);
            this.Controls.Add(this.listen_button);
            this.Controls.Add(this.port_textBox);
            this.Controls.Add(this.label1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox port_textBox;
        private System.Windows.Forms.Button listen_button;
        private System.Windows.Forms.RichTextBox timeline;
    }
}

