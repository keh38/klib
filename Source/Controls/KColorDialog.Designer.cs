namespace KLib.Controls
{
    partial class KColorDialog
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
            this.colorBox = new System.Windows.Forms.PictureBox();
            this.cancelButton = new System.Windows.Forms.Button();
            this.okButton = new System.Windows.Forms.Button();
            this.alphaSlider = new KLib.Controls.ValueSlider();
            this.blueSlider = new KLib.Controls.ValueSlider();
            this.greenSlider = new KLib.Controls.ValueSlider();
            this.redSlider = new KLib.Controls.ValueSlider();
            ((System.ComponentModel.ISupportInitialize)(this.colorBox)).BeginInit();
            this.SuspendLayout();
            // 
            // colorBox
            // 
            this.colorBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.colorBox.Location = new System.Drawing.Point(23, 286);
            this.colorBox.Name = "colorBox";
            this.colorBox.Size = new System.Drawing.Size(171, 29);
            this.colorBox.TabIndex = 0;
            this.colorBox.TabStop = false;
            this.colorBox.Click += new System.EventHandler(this.colorBox_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.Location = new System.Drawing.Point(118, 331);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 1;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // okButton
            // 
            this.okButton.Location = new System.Drawing.Point(39, 331);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(75, 23);
            this.okButton.TabIndex = 2;
            this.okButton.Text = "OK";
            this.okButton.UseVisualStyleBackColor = true;
            this.okButton.Click += new System.EventHandler(this.okButton_Click);
            // 
            // alphaSlider
            // 
            this.alphaSlider.IsInteger = true;
            this.alphaSlider.Location = new System.Drawing.Point(12, 214);
            this.alphaSlider.MaxLabel = "";
            this.alphaSlider.MaxValue = 255D;
            this.alphaSlider.MinLabel = "";
            this.alphaSlider.MinValue = 0D;
            this.alphaSlider.Name = "alphaSlider";
            this.alphaSlider.Resolution = 1D;
            this.alphaSlider.Size = new System.Drawing.Size(193, 66);
            this.alphaSlider.TabIndex = 7;
            this.alphaSlider.TickSpacing = 32D;
            this.alphaSlider.Title = "Alpha";
            this.alphaSlider.Value = 0D;
            this.alphaSlider.ValueAsFloat = 0F;
            this.alphaSlider.ValueChanged += new System.EventHandler(this.alphaSlider_ValueChanged);
            // 
            // blueSlider
            // 
            this.blueSlider.IsInteger = true;
            this.blueSlider.Location = new System.Drawing.Point(12, 142);
            this.blueSlider.MaxLabel = "";
            this.blueSlider.MaxValue = 255D;
            this.blueSlider.MinLabel = "";
            this.blueSlider.MinValue = 0D;
            this.blueSlider.Name = "blueSlider";
            this.blueSlider.Resolution = 1D;
            this.blueSlider.Size = new System.Drawing.Size(193, 66);
            this.blueSlider.TabIndex = 6;
            this.blueSlider.TickSpacing = 32D;
            this.blueSlider.Title = "Blue";
            this.blueSlider.Value = 0D;
            this.blueSlider.ValueAsFloat = 0F;
            this.blueSlider.ValueChanged += new System.EventHandler(this.blueSlider_ValueChanged);
            // 
            // greenSlider
            // 
            this.greenSlider.IsInteger = true;
            this.greenSlider.Location = new System.Drawing.Point(12, 70);
            this.greenSlider.MaxLabel = "";
            this.greenSlider.MaxValue = 255D;
            this.greenSlider.MinLabel = "";
            this.greenSlider.MinValue = 0D;
            this.greenSlider.Name = "greenSlider";
            this.greenSlider.Resolution = 1D;
            this.greenSlider.Size = new System.Drawing.Size(193, 66);
            this.greenSlider.TabIndex = 5;
            this.greenSlider.TickSpacing = 32D;
            this.greenSlider.Title = "Green";
            this.greenSlider.Value = 0D;
            this.greenSlider.ValueAsFloat = 0F;
            this.greenSlider.ValueChanged += new System.EventHandler(this.greenSlider_ValueChanged);
            // 
            // redSlider
            // 
            this.redSlider.IsInteger = true;
            this.redSlider.Location = new System.Drawing.Point(12, 12);
            this.redSlider.MaxLabel = "";
            this.redSlider.MaxValue = 255D;
            this.redSlider.MinLabel = "";
            this.redSlider.MinValue = 0D;
            this.redSlider.Name = "redSlider";
            this.redSlider.Resolution = 1D;
            this.redSlider.Size = new System.Drawing.Size(193, 66);
            this.redSlider.TabIndex = 4;
            this.redSlider.TickSpacing = 32D;
            this.redSlider.Title = "Red";
            this.redSlider.Value = 0D;
            this.redSlider.ValueAsFloat = 0F;
            this.redSlider.ValueChanged += new System.EventHandler(this.redSlider_ValueChanged);
            // 
            // KColorDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(217, 371);
            this.Controls.Add(this.alphaSlider);
            this.Controls.Add(this.blueSlider);
            this.Controls.Add(this.greenSlider);
            this.Controls.Add(this.redSlider);
            this.Controls.Add(this.okButton);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.colorBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "KColorDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            ((System.ComponentModel.ISupportInitialize)(this.colorBox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox colorBox;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Button okButton;
        private ValueSlider redSlider;
        private ValueSlider greenSlider;
        private ValueSlider blueSlider;
        private ValueSlider alphaSlider;
    }
}