namespace MSGraphDemo
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
            this.signInButton = new System.Windows.Forms.Button();
            this.signOutButton = new System.Windows.Forms.Button();
            this.resultBox = new System.Windows.Forms.TextBox();
            this.signInLabel = new System.Windows.Forms.Label();
            this.opsPanel = new System.Windows.Forms.Panel();
            this.uploadButton = new System.Windows.Forms.Button();
            this.testButton = new System.Windows.Forms.Button();
            this.rootButton = new System.Windows.Forms.Button();
            this.tokenBox = new System.Windows.Forms.TextBox();
            this.downloadButton = new System.Windows.Forms.Button();
            this.opsPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // signInButton
            // 
            this.signInButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.signInButton.Location = new System.Drawing.Point(38, 29);
            this.signInButton.Name = "signInButton";
            this.signInButton.Size = new System.Drawing.Size(153, 63);
            this.signInButton.TabIndex = 0;
            this.signInButton.Text = "Sign In";
            this.signInButton.UseVisualStyleBackColor = true;
            this.signInButton.Click += new System.EventHandler(this.signInButton_Click);
            // 
            // signOutButton
            // 
            this.signOutButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.signOutButton.Location = new System.Drawing.Point(38, 30);
            this.signOutButton.Name = "signOutButton";
            this.signOutButton.Size = new System.Drawing.Size(153, 63);
            this.signOutButton.TabIndex = 1;
            this.signOutButton.Text = "Sign Out";
            this.signOutButton.UseVisualStyleBackColor = true;
            this.signOutButton.Click += new System.EventHandler(this.signOutButton_Click);
            // 
            // resultBox
            // 
            this.resultBox.Location = new System.Drawing.Point(38, 273);
            this.resultBox.Multiline = true;
            this.resultBox.Name = "resultBox";
            this.resultBox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.resultBox.Size = new System.Drawing.Size(413, 219);
            this.resultBox.TabIndex = 2;
            // 
            // signInLabel
            // 
            this.signInLabel.Location = new System.Drawing.Point(207, 35);
            this.signInLabel.Name = "signInLabel";
            this.signInLabel.Size = new System.Drawing.Size(244, 57);
            this.signInLabel.TabIndex = 3;
            this.signInLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // opsPanel
            // 
            this.opsPanel.Controls.Add(this.downloadButton);
            this.opsPanel.Controls.Add(this.uploadButton);
            this.opsPanel.Controls.Add(this.testButton);
            this.opsPanel.Controls.Add(this.rootButton);
            this.opsPanel.Location = new System.Drawing.Point(38, 111);
            this.opsPanel.Name = "opsPanel";
            this.opsPanel.Size = new System.Drawing.Size(412, 141);
            this.opsPanel.TabIndex = 4;
            // 
            // uploadButton
            // 
            this.uploadButton.Location = new System.Drawing.Point(84, 3);
            this.uploadButton.Name = "uploadButton";
            this.uploadButton.Size = new System.Drawing.Size(75, 23);
            this.uploadButton.TabIndex = 2;
            this.uploadButton.Text = "Upload";
            this.uploadButton.UseVisualStyleBackColor = true;
            this.uploadButton.Click += new System.EventHandler(this.uploadButton_Click);
            // 
            // testButton
            // 
            this.testButton.Location = new System.Drawing.Point(3, 32);
            this.testButton.Name = "testButton";
            this.testButton.Size = new System.Drawing.Size(75, 23);
            this.testButton.TabIndex = 1;
            this.testButton.Text = "Test";
            this.testButton.UseVisualStyleBackColor = true;
            this.testButton.Click += new System.EventHandler(this.testButton_Click);
            // 
            // rootButton
            // 
            this.rootButton.Location = new System.Drawing.Point(3, 3);
            this.rootButton.Name = "rootButton";
            this.rootButton.Size = new System.Drawing.Size(75, 23);
            this.rootButton.TabIndex = 0;
            this.rootButton.Text = "Find root";
            this.rootButton.UseVisualStyleBackColor = true;
            this.rootButton.Click += new System.EventHandler(this.rootButton_Click);
            // 
            // tokenBox
            // 
            this.tokenBox.Location = new System.Drawing.Point(38, 509);
            this.tokenBox.Multiline = true;
            this.tokenBox.Name = "tokenBox";
            this.tokenBox.Size = new System.Drawing.Size(412, 37);
            this.tokenBox.TabIndex = 5;
            // 
            // downloadButton
            // 
            this.downloadButton.Location = new System.Drawing.Point(84, 32);
            this.downloadButton.Name = "downloadButton";
            this.downloadButton.Size = new System.Drawing.Size(75, 23);
            this.downloadButton.TabIndex = 3;
            this.downloadButton.Text = "Download";
            this.downloadButton.UseVisualStyleBackColor = true;
            this.downloadButton.Click += new System.EventHandler(this.downloadButton_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(488, 558);
            this.Controls.Add(this.tokenBox);
            this.Controls.Add(this.opsPanel);
            this.Controls.Add(this.signInLabel);
            this.Controls.Add(this.resultBox);
            this.Controls.Add(this.signInButton);
            this.Controls.Add(this.signOutButton);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.opsPanel.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button signInButton;
        private System.Windows.Forms.Button signOutButton;
        private System.Windows.Forms.TextBox resultBox;
        private System.Windows.Forms.Label signInLabel;
        private System.Windows.Forms.Panel opsPanel;
        private System.Windows.Forms.Button rootButton;
        private System.Windows.Forms.Button testButton;
        private System.Windows.Forms.TextBox tokenBox;
        private System.Windows.Forms.Button uploadButton;
        private System.Windows.Forms.Button downloadButton;
    }
}

