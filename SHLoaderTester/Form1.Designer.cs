namespace SHLoaderTester
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
            this.ConnectionString = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.SHUserName = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.SHServerName = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.SHPassword = new System.Windows.Forms.TextBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.button3 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.label5 = new System.Windows.Forms.Label();
            this.SHServerPort = new System.Windows.Forms.TextBox();
            this.Output = new System.Windows.Forms.TextBox();
            this.groupRid = new System.Windows.Forms.TextBox();
            this.encode = new System.Windows.Forms.CheckBox();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.SuspendLayout();
            // 
            // ConnectionString
            // 
            this.ConnectionString.Location = new System.Drawing.Point(131, 16);
            this.ConnectionString.Name = "ConnectionString";
            this.ConnectionString.Size = new System.Drawing.Size(148, 20);
            this.ConnectionString.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(112, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "SQL connection string";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 97);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(76, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "SH User name";
            // 
            // SHUserName
            // 
            this.SHUserName.Location = new System.Drawing.Point(131, 94);
            this.SHUserName.Name = "SHUserName";
            this.SHUserName.Size = new System.Drawing.Size(148, 20);
            this.SHUserName.TabIndex = 2;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(13, 45);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(94, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "SH server address";
            // 
            // SHServerName
            // 
            this.SHServerName.Location = new System.Drawing.Point(131, 42);
            this.SHServerName.Name = "SHServerName";
            this.SHServerName.Size = new System.Drawing.Size(148, 20);
            this.SHServerName.TabIndex = 4;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(13, 123);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(70, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "SH password";
            // 
            // SHPassword
            // 
            this.SHPassword.Location = new System.Drawing.Point(131, 120);
            this.SHPassword.Name = "SHPassword";
            this.SHPassword.Size = new System.Drawing.Size(148, 20);
            this.SHPassword.TabIndex = 6;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(285, 16);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(391, 171);
            this.tabControl1.TabIndex = 8;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.groupRid);
            this.tabPage1.Controls.Add(this.button3);
            this.tabPage1.Controls.Add(this.button2);
            this.tabPage1.Controls.Add(this.button1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(383, 145);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Complects";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(6, 78);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(243, 31);
            this.button3.TabIndex = 2;
            this.button3.Text = "LoadHDRComplects";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(6, 41);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(243, 31);
            this.button2.TabIndex = 1;
            this.button2.Text = "LoadBaseComplects";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(6, 6);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(243, 31);
            this.button1.TabIndex = 0;
            this.button1.Text = "LoadComplectsTree";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // tabPage2
            // 
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(255, 145);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "tabPage2";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(13, 71);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(75, 13);
            this.label5.TabIndex = 10;
            this.label5.Text = "SH server port";
            // 
            // SHServerPort
            // 
            this.SHServerPort.Location = new System.Drawing.Point(131, 68);
            this.SHServerPort.Name = "SHServerPort";
            this.SHServerPort.Size = new System.Drawing.Size(148, 20);
            this.SHServerPort.TabIndex = 9;
            // 
            // Output
            // 
            this.Output.Location = new System.Drawing.Point(16, 203);
            this.Output.Multiline = true;
            this.Output.Name = "Output";
            this.Output.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.Output.Size = new System.Drawing.Size(528, 257);
            this.Output.TabIndex = 11;
            // 
            // groupRid
            // 
            this.groupRid.Location = new System.Drawing.Point(255, 47);
            this.groupRid.Name = "groupRid";
            this.groupRid.Size = new System.Drawing.Size(122, 20);
            this.groupRid.TabIndex = 3;
            // 
            // encode
            // 
            this.encode.AutoSize = true;
            this.encode.Location = new System.Drawing.Point(16, 152);
            this.encode.Name = "encode";
            this.encode.Size = new System.Drawing.Size(63, 17);
            this.encode.TabIndex = 12;
            this.encode.Text = "Encode";
            this.encode.UseVisualStyleBackColor = true;
            this.encode.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(688, 472);
            this.Controls.Add(this.encode);
            this.Controls.Add(this.Output);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.SHServerPort);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.SHPassword);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.SHServerName);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.SHUserName);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.ConnectionString);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox ConnectionString;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox SHUserName;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox SHServerName;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox SHPassword;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox SHServerPort;
        private System.Windows.Forms.TextBox Output;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.TextBox groupRid;
        private System.Windows.Forms.CheckBox encode;
    }
}

