namespace EncryptionTool
{
    partial class MainWindow
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
            this.EncryptButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.inputBox = new System.Windows.Forms.TextBox();
            this.resultBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.DecryptButton = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.decryptBox = new System.Windows.Forms.TextBox();
            this.CreateKeysButton = new System.Windows.Forms.Button();
            this.containerNameBox = new System.Windows.Forms.TextBox();
            this.ExportPublicButton = new System.Windows.Forms.Button();
            this.ImportKeyButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // EncryptButton
            // 
            this.EncryptButton.Location = new System.Drawing.Point(288, 154);
            this.EncryptButton.Name = "EncryptButton";
            this.EncryptButton.Size = new System.Drawing.Size(75, 23);
            this.EncryptButton.TabIndex = 0;
            this.EncryptButton.Text = "Encrypt";
            this.EncryptButton.UseVisualStyleBackColor = true;
            this.EncryptButton.Click += new System.EventHandler(this.Encrypt_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(27, 140);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(84, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "String to encrypt";
            // 
            // inputBox
            // 
            this.inputBox.Location = new System.Drawing.Point(30, 156);
            this.inputBox.Name = "inputBox";
            this.inputBox.Size = new System.Drawing.Size(252, 20);
            this.inputBox.TabIndex = 2;
            // 
            // resultBox
            // 
            this.resultBox.Location = new System.Drawing.Point(30, 203);
            this.resultBox.Name = "resultBox";
            this.resultBox.Size = new System.Drawing.Size(252, 20);
            this.resultBox.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(27, 187);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(55, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Encrypted";
            // 
            // DecryptButton
            // 
            this.DecryptButton.Location = new System.Drawing.Point(288, 203);
            this.DecryptButton.Name = "DecryptButton";
            this.DecryptButton.Size = new System.Drawing.Size(75, 23);
            this.DecryptButton.TabIndex = 5;
            this.DecryptButton.Text = "Decrypt";
            this.DecryptButton.UseVisualStyleBackColor = true;
            this.DecryptButton.Click += new System.EventHandler(this.DecryptButton_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(27, 231);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(56, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "Decrypted";
            // 
            // decryptBox
            // 
            this.decryptBox.Location = new System.Drawing.Point(30, 247);
            this.decryptBox.Name = "decryptBox";
            this.decryptBox.Size = new System.Drawing.Size(252, 20);
            this.decryptBox.TabIndex = 6;
            // 
            // CreateKeysButton
            // 
            this.CreateKeysButton.Location = new System.Drawing.Point(30, 30);
            this.CreateKeysButton.Name = "CreateKeysButton";
            this.CreateKeysButton.Size = new System.Drawing.Size(140, 23);
            this.CreateKeysButton.TabIndex = 8;
            this.CreateKeysButton.Text = "Create RSA Key Pair";
            this.CreateKeysButton.UseVisualStyleBackColor = true;
            this.CreateKeysButton.Click += new System.EventHandler(this.CreateKeysButton_Click);
            // 
            // containerNameBox
            // 
            this.containerNameBox.Location = new System.Drawing.Point(185, 32);
            this.containerNameBox.Name = "containerNameBox";
            this.containerNameBox.Size = new System.Drawing.Size(178, 20);
            this.containerNameBox.TabIndex = 9;
            // 
            // ExportPublicButton
            // 
            this.ExportPublicButton.Location = new System.Drawing.Point(30, 59);
            this.ExportPublicButton.Name = "ExportPublicButton";
            this.ExportPublicButton.Size = new System.Drawing.Size(140, 23);
            this.ExportPublicButton.TabIndex = 10;
            this.ExportPublicButton.Text = "Export Public Key";
            this.ExportPublicButton.UseVisualStyleBackColor = true;
            this.ExportPublicButton.Click += new System.EventHandler(this.ExportPublicButton_Click);
            // 
            // ImportKeyButton
            // 
            this.ImportKeyButton.Location = new System.Drawing.Point(30, 88);
            this.ImportKeyButton.Name = "ImportKeyButton";
            this.ImportKeyButton.Size = new System.Drawing.Size(140, 23);
            this.ImportKeyButton.TabIndex = 11;
            this.ImportKeyButton.Text = "Import Public Key";
            this.ImportKeyButton.UseVisualStyleBackColor = true;
            this.ImportKeyButton.Click += new System.EventHandler(this.ImportKeyButton_Click);
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(437, 308);
            this.Controls.Add(this.ImportKeyButton);
            this.Controls.Add(this.ExportPublicButton);
            this.Controls.Add(this.containerNameBox);
            this.Controls.Add(this.CreateKeysButton);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.decryptBox);
            this.Controls.Add(this.DecryptButton);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.resultBox);
            this.Controls.Add(this.inputBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.EncryptButton);
            this.Name = "MainWindow";
            this.Text = "Encryption Tool";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button EncryptButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox inputBox;
        private System.Windows.Forms.TextBox resultBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button DecryptButton;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox decryptBox;
        private System.Windows.Forms.Button CreateKeysButton;
        private System.Windows.Forms.TextBox containerNameBox;
        private System.Windows.Forms.Button ExportPublicButton;
        private System.Windows.Forms.Button ImportKeyButton;
    }
}

