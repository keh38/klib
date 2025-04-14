namespace KLibUnitTests
{
    partial class MainForm
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
            this.ExitButton = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.FFTWButton = new System.Windows.Forms.Button();
            this.ToneCloudButton = new System.Windows.Forms.Button();
            this.RippleDemoButton = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.RandomButton = new System.Windows.Forms.Button();
            this.TestControlsButton = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.AudioButton = new System.Windows.Forms.Button();
            this.brightnessNumeric = new KLib.Controls.KNumericBox();
            this.label1 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // ExitButton
            // 
            this.ExitButton.Location = new System.Drawing.Point(494, 192);
            this.ExitButton.Name = "ExitButton";
            this.ExitButton.Size = new System.Drawing.Size(75, 23);
            this.ExitButton.TabIndex = 1;
            this.ExitButton.Text = "Exit";
            this.ExitButton.UseVisualStyleBackColor = true;
            this.ExitButton.Click += new System.EventHandler(this.ExitButton_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.FFTWButton);
            this.groupBox1.Controls.Add(this.ToneCloudButton);
            this.groupBox1.Controls.Add(this.RippleDemoButton);
            this.groupBox1.Location = new System.Drawing.Point(202, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(172, 159);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "KLib.Signals";
            // 
            // FFTWButton
            // 
            this.FFTWButton.Location = new System.Drawing.Point(6, 130);
            this.FFTWButton.Name = "FFTWButton";
            this.FFTWButton.Size = new System.Drawing.Size(160, 23);
            this.FFTWButton.TabIndex = 4;
            this.FFTWButton.Text = "FFTW";
            this.FFTWButton.UseVisualStyleBackColor = true;
            this.FFTWButton.Click += new System.EventHandler(this.FFTWButton_Click);
            // 
            // ToneCloudButton
            // 
            this.ToneCloudButton.Location = new System.Drawing.Point(6, 68);
            this.ToneCloudButton.Name = "ToneCloudButton";
            this.ToneCloudButton.Size = new System.Drawing.Size(160, 23);
            this.ToneCloudButton.TabIndex = 1;
            this.ToneCloudButton.Text = "Tone Cloud";
            this.ToneCloudButton.UseVisualStyleBackColor = true;
            this.ToneCloudButton.Click += new System.EventHandler(this.ToneCloudButton_Click);
            // 
            // RippleDemoButton
            // 
            this.RippleDemoButton.Location = new System.Drawing.Point(6, 34);
            this.RippleDemoButton.Name = "RippleDemoButton";
            this.RippleDemoButton.Size = new System.Drawing.Size(160, 23);
            this.RippleDemoButton.TabIndex = 0;
            this.RippleDemoButton.Text = "Moving Ripple Noise";
            this.RippleDemoButton.UseVisualStyleBackColor = true;
            this.RippleDemoButton.Click += new System.EventHandler(this.RippleDemoButton_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.RandomButton);
            this.groupBox2.Location = new System.Drawing.Point(397, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(172, 159);
            this.groupBox2.TabIndex = 3;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "KLib.Utilities";
            // 
            // RandomButton
            // 
            this.RandomButton.Location = new System.Drawing.Point(6, 34);
            this.RandomButton.Name = "RandomButton";
            this.RandomButton.Size = new System.Drawing.Size(160, 23);
            this.RandomButton.TabIndex = 4;
            this.RandomButton.Text = "Random Number Generators";
            this.RandomButton.UseVisualStyleBackColor = true;
            this.RandomButton.Click += new System.EventHandler(this.RandomButton_Click);
            // 
            // TestControlsButton
            // 
            this.TestControlsButton.Location = new System.Drawing.Point(6, 34);
            this.TestControlsButton.Name = "TestControlsButton";
            this.TestControlsButton.Size = new System.Drawing.Size(160, 23);
            this.TestControlsButton.TabIndex = 4;
            this.TestControlsButton.Text = "Controls";
            this.TestControlsButton.UseVisualStyleBackColor = true;
            this.TestControlsButton.Click += new System.EventHandler(this.TestControlsButton_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.TestControlsButton);
            this.groupBox3.Location = new System.Drawing.Point(12, 12);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(172, 159);
            this.groupBox3.TabIndex = 4;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "KLib.Controls";
            // 
            // AudioButton
            // 
            this.AudioButton.Location = new System.Drawing.Point(18, 192);
            this.AudioButton.Name = "AudioButton";
            this.AudioButton.Size = new System.Drawing.Size(160, 23);
            this.AudioButton.TabIndex = 5;
            this.AudioButton.Text = "Audio";
            this.AudioButton.UseVisualStyleBackColor = true;
            this.AudioButton.Click += new System.EventHandler(this.AudioButton_Click);
            // 
            // brightnessNumeric
            // 
            this.brightnessNumeric.AllowInf = false;
            this.brightnessNumeric.AutoSize = true;
            this.brightnessNumeric.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.brightnessNumeric.ClearOnDisable = false;
            this.brightnessNumeric.FloatValue = 0F;
            this.brightnessNumeric.IntValue = 0;
            this.brightnessNumeric.IsInteger = false;
            this.brightnessNumeric.Location = new System.Drawing.Point(268, 195);
            this.brightnessNumeric.MaxCoerce = false;
            this.brightnessNumeric.MaximumSize = new System.Drawing.Size(20000, 20);
            this.brightnessNumeric.MaxValue = 1.7976931348623157E+308D;
            this.brightnessNumeric.MinCoerce = false;
            this.brightnessNumeric.MinimumSize = new System.Drawing.Size(10, 20);
            this.brightnessNumeric.MinValue = 0D;
            this.brightnessNumeric.Name = "brightnessNumeric";
            this.brightnessNumeric.Size = new System.Drawing.Size(100, 20);
            this.brightnessNumeric.TabIndex = 6;
            this.brightnessNumeric.TextFormat = "K4";
            this.brightnessNumeric.ToolTip = "";
            this.brightnessNumeric.Units = "";
            this.brightnessNumeric.Value = 0D;
            this.brightnessNumeric.ValueChanged += new System.EventHandler(this.brightnessNumeric_ValueChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(265, 179);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(56, 13);
            this.label1.TabIndex = 7;
            this.label1.Text = "Brightness";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(374, 192);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 8;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(584, 235);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.brightnessNumeric);
            this.Controls.Add(this.AudioButton);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.ExitButton);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "KLib Unit Tests";
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button ExitButton;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button RippleDemoButton;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button ToneCloudButton;
        private System.Windows.Forms.Button RandomButton;
        private System.Windows.Forms.Button FFTWButton;
        private System.Windows.Forms.Button TestControlsButton;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button AudioButton;
        private KLib.Controls.KNumericBox brightnessNumeric;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button1;
    }
}

