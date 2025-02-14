namespace KLibUnitTests
{
    partial class AudioForm
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
            this.EnumerateButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // EnumerateButton
            // 
            this.EnumerateButton.Location = new System.Drawing.Point(41, 53);
            this.EnumerateButton.Name = "EnumerateButton";
            this.EnumerateButton.Size = new System.Drawing.Size(107, 33);
            this.EnumerateButton.TabIndex = 0;
            this.EnumerateButton.Text = "Enumerate";
            this.EnumerateButton.UseVisualStyleBackColor = true;
            this.EnumerateButton.Click += new System.EventHandler(this.EnumerateButton_Click);
            // 
            // AudioForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.EnumerateButton);
            this.Name = "AudioForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "AudioForm";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button EnumerateButton;
    }
}