namespace KLibUnitTests
{
    partial class ToneCloudDemo
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
            this.FinishedButton = new System.Windows.Forms.Button();
            this.zedGraph = new ZedGraph.ZedGraphControl();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.PipDurBox = new System.Windows.Forms.NumericUpDown();
            this.PipRampBox = new System.Windows.Forms.NumericUpDown();
            this.PipRateBox = new System.Windows.Forms.NumericUpDown();
            this.FsigmaBox = new System.Windows.Forms.NumericUpDown();
            this.FmeanBox = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.TBox = new System.Windows.Forms.NumericUpDown();
            this.FsBox = new System.Windows.Forms.NumericUpDown();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.ApplyButton = new System.Windows.Forms.Button();
            this.cloudGraph = new ZedGraph.ZedGraphControl();
            this.BWBox = new System.Windows.Forms.NumericUpDown();
            this.label8 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.PipDurBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PipRampBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PipRateBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.FsigmaBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.FmeanBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.FsBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.BWBox)).BeginInit();
            this.SuspendLayout();
            // 
            // FinishedButton
            // 
            this.FinishedButton.Location = new System.Drawing.Point(81, 367);
            this.FinishedButton.Name = "FinishedButton";
            this.FinishedButton.Size = new System.Drawing.Size(75, 23);
            this.FinishedButton.TabIndex = 0;
            this.FinishedButton.Text = "Finished";
            this.FinishedButton.UseVisualStyleBackColor = true;
            this.FinishedButton.Click += new System.EventHandler(this.FinishedButton_Click);
            // 
            // zedGraph
            // 
            this.zedGraph.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.zedGraph.Location = new System.Drawing.Point(381, 35);
            this.zedGraph.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.zedGraph.Name = "zedGraph";
            this.zedGraph.ScrollGrace = 0D;
            this.zedGraph.ScrollMaxX = 0D;
            this.zedGraph.ScrollMaxY = 0D;
            this.zedGraph.ScrollMaxY2 = 0D;
            this.zedGraph.ScrollMinX = 0D;
            this.zedGraph.ScrollMinY = 0D;
            this.zedGraph.ScrollMinY2 = 0D;
            this.zedGraph.Size = new System.Drawing.Size(289, 196);
            this.zedGraph.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(28, 37);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(88, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Pip duration (ms):";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(28, 63);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(73, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Pip ramp (ms):";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(28, 91);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(68, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "Pip rate (Hz):";
            // 
            // PipDurBox
            // 
            this.PipDurBox.Location = new System.Drawing.Point(122, 35);
            this.PipDurBox.Name = "PipDurBox";
            this.PipDurBox.Size = new System.Drawing.Size(82, 20);
            this.PipDurBox.TabIndex = 8;
            this.PipDurBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // PipRampBox
            // 
            this.PipRampBox.Location = new System.Drawing.Point(122, 61);
            this.PipRampBox.Name = "PipRampBox";
            this.PipRampBox.Size = new System.Drawing.Size(82, 20);
            this.PipRampBox.TabIndex = 9;
            this.PipRampBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // PipRateBox
            // 
            this.PipRateBox.Location = new System.Drawing.Point(122, 89);
            this.PipRateBox.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.PipRateBox.Name = "PipRateBox";
            this.PipRateBox.Size = new System.Drawing.Size(82, 20);
            this.PipRateBox.TabIndex = 10;
            this.PipRateBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // FsigmaBox
            // 
            this.FsigmaBox.DecimalPlaces = 2;
            this.FsigmaBox.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.FsigmaBox.Location = new System.Drawing.Point(122, 189);
            this.FsigmaBox.Maximum = new decimal(new int[] {
            20000,
            0,
            0,
            0});
            this.FsigmaBox.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            -2147483648});
            this.FsigmaBox.Name = "FsigmaBox";
            this.FsigmaBox.Size = new System.Drawing.Size(82, 20);
            this.FsigmaBox.TabIndex = 14;
            this.FsigmaBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.FsigmaBox.ValueChanged += new System.EventHandler(this.FsigmaBox_ValueChanged);
            // 
            // FmeanBox
            // 
            this.FmeanBox.Location = new System.Drawing.Point(122, 133);
            this.FmeanBox.Maximum = new decimal(new int[] {
            20000,
            0,
            0,
            0});
            this.FmeanBox.Name = "FmeanBox";
            this.FmeanBox.Size = new System.Drawing.Size(82, 20);
            this.FmeanBox.TabIndex = 13;
            this.FmeanBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.FmeanBox.ValueChanged += new System.EventHandler(this.FmeanBox_ValueChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(17, 191);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(102, 13);
            this.label4.TabIndex = 12;
            this.label4.Text = "Freq std. dev. (oct.):";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(17, 135);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(80, 13);
            this.label5.TabIndex = 11;
            this.label5.Text = "Mean freq (Hz):";
            // 
            // TBox
            // 
            this.TBox.Location = new System.Drawing.Point(122, 261);
            this.TBox.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.TBox.Name = "TBox";
            this.TBox.Size = new System.Drawing.Size(82, 20);
            this.TBox.TabIndex = 18;
            this.TBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // FsBox
            // 
            this.FsBox.Location = new System.Drawing.Point(122, 233);
            this.FsBox.Maximum = new decimal(new int[] {
            20000,
            0,
            0,
            0});
            this.FsBox.Name = "FsBox";
            this.FsBox.Size = new System.Drawing.Size(82, 20);
            this.FsBox.TabIndex = 17;
            this.FsBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(28, 263);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(82, 13);
            this.label6.TabIndex = 16;
            this.label6.Text = "Frame size (ms):";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(28, 235);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(96, 13);
            this.label7.TabIndex = 15;
            this.label7.Text = "Sampling rate (Hz):";
            // 
            // ApplyButton
            // 
            this.ApplyButton.Location = new System.Drawing.Point(81, 321);
            this.ApplyButton.Name = "ApplyButton";
            this.ApplyButton.Size = new System.Drawing.Size(75, 23);
            this.ApplyButton.TabIndex = 19;
            this.ApplyButton.Text = "Apply";
            this.ApplyButton.UseVisualStyleBackColor = true;
            this.ApplyButton.Click += new System.EventHandler(this.ApplyButton_Click);
            // 
            // cloudGraph
            // 
            this.cloudGraph.Location = new System.Drawing.Point(228, 261);
            this.cloudGraph.Name = "cloudGraph";
            this.cloudGraph.ScrollGrace = 0D;
            this.cloudGraph.ScrollMaxX = 0D;
            this.cloudGraph.ScrollMaxY = 0D;
            this.cloudGraph.ScrollMaxY2 = 0D;
            this.cloudGraph.ScrollMinX = 0D;
            this.cloudGraph.ScrollMinY = 0D;
            this.cloudGraph.ScrollMinY2 = 0D;
            this.cloudGraph.Size = new System.Drawing.Size(593, 290);
            this.cloudGraph.TabIndex = 20;
            // 
            // BWBox
            // 
            this.BWBox.DecimalPlaces = 2;
            this.BWBox.Increment = new decimal(new int[] {
            5,
            0,
            0,
            65536});
            this.BWBox.Location = new System.Drawing.Point(122, 161);
            this.BWBox.Maximum = new decimal(new int[] {
            20000,
            0,
            0,
            0});
            this.BWBox.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            -2147483648});
            this.BWBox.Name = "BWBox";
            this.BWBox.Size = new System.Drawing.Size(82, 20);
            this.BWBox.TabIndex = 22;
            this.BWBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.BWBox.ValueChanged += new System.EventHandler(this.BWBox_ValueChanged);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(17, 163);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(55, 13);
            this.label8.TabIndex = 21;
            this.label8.Text = "BW (oct.):";
            // 
            // ToneCloudDemo
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(844, 574);
            this.Controls.Add(this.BWBox);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.cloudGraph);
            this.Controls.Add(this.ApplyButton);
            this.Controls.Add(this.TBox);
            this.Controls.Add(this.FsBox);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.FsigmaBox);
            this.Controls.Add(this.FmeanBox);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.PipRateBox);
            this.Controls.Add(this.PipRampBox);
            this.Controls.Add(this.PipDurBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.zedGraph);
            this.Controls.Add(this.FinishedButton);
            this.Name = "ToneCloudDemo";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Tone Cloud Demo";
            this.Load += new System.EventHandler(this.ToneCloudDemo_Load);
            ((System.ComponentModel.ISupportInitialize)(this.PipDurBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PipRampBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PipRateBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.FsigmaBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.FmeanBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.FsBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.BWBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button FinishedButton;
        private ZedGraph.ZedGraphControl zedGraph;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown PipDurBox;
        private System.Windows.Forms.NumericUpDown PipRampBox;
        private System.Windows.Forms.NumericUpDown PipRateBox;
        private System.Windows.Forms.NumericUpDown FsigmaBox;
        private System.Windows.Forms.NumericUpDown FmeanBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.NumericUpDown TBox;
        private System.Windows.Forms.NumericUpDown FsBox;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button ApplyButton;
        private ZedGraph.ZedGraphControl cloudGraph;
        private System.Windows.Forms.NumericUpDown BWBox;
        private System.Windows.Forms.Label label8;
    }
}