namespace KLibUnitTests
{
    partial class RippleDemoForm
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
            this.ApplyButton = new System.Windows.Forms.Button();
            this.ExitButton = new System.Windows.Forms.Button();
            this.zedGraph = new ZedGraph.ZedGraphControl();
            this.InitVelBox = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.FinalVelBox = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.FinalDensBox = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.InitDensBox = new System.Windows.Forms.NumericUpDown();
            this.Full = new System.Windows.Forms.Button();
            this.FreqResButton = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.FinalDepthBox = new System.Windows.Forms.NumericUpDown();
            this.label6 = new System.Windows.Forms.Label();
            this.InitDepthBox = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.InitVelBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.FinalVelBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.FinalDensBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.InitDensBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.FinalDepthBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.InitDepthBox)).BeginInit();
            this.SuspendLayout();
            // 
            // ApplyButton
            // 
            this.ApplyButton.Location = new System.Drawing.Point(77, 416);
            this.ApplyButton.Name = "ApplyButton";
            this.ApplyButton.Size = new System.Drawing.Size(100, 23);
            this.ApplyButton.TabIndex = 0;
            this.ApplyButton.Text = "3 components";
            this.ApplyButton.UseVisualStyleBackColor = true;
            this.ApplyButton.Click += new System.EventHandler(this.ApplyButton_Click);
            // 
            // ExitButton
            // 
            this.ExitButton.Location = new System.Drawing.Point(77, 503);
            this.ExitButton.Name = "ExitButton";
            this.ExitButton.Size = new System.Drawing.Size(100, 23);
            this.ExitButton.TabIndex = 1;
            this.ExitButton.Text = "Finished";
            this.ExitButton.UseVisualStyleBackColor = true;
            this.ExitButton.Click += new System.EventHandler(this.ExitButton_Click);
            // 
            // zedGraph
            // 
            this.zedGraph.Location = new System.Drawing.Point(271, 24);
            this.zedGraph.Name = "zedGraph";
            this.zedGraph.ScrollGrace = 0D;
            this.zedGraph.ScrollMaxX = 0D;
            this.zedGraph.ScrollMaxY = 0D;
            this.zedGraph.ScrollMaxY2 = 0D;
            this.zedGraph.ScrollMinX = 0D;
            this.zedGraph.ScrollMinY = 0D;
            this.zedGraph.ScrollMinY2 = 0D;
            this.zedGraph.Size = new System.Drawing.Size(643, 502);
            this.zedGraph.TabIndex = 4;
            // 
            // InitVelBox
            // 
            this.InitVelBox.DecimalPlaces = 1;
            this.InitVelBox.Increment = new decimal(new int[] {
            5,
            0,
            0,
            65536});
            this.InitVelBox.Location = new System.Drawing.Point(112, 116);
            this.InitVelBox.Name = "InitVelBox";
            this.InitVelBox.Size = new System.Drawing.Size(87, 20);
            this.InitVelBox.TabIndex = 5;
            this.InitVelBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.InitVelBox.Value = new decimal(new int[] {
            17,
            0,
            0,
            0});
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(45, 118);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(61, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "Init Velocity";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(37, 144);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(69, 13);
            this.label2.TabIndex = 8;
            this.label2.Text = "Final Velocity";
            // 
            // FinalVelBox
            // 
            this.FinalVelBox.DecimalPlaces = 1;
            this.FinalVelBox.Increment = new decimal(new int[] {
            5,
            0,
            0,
            65536});
            this.FinalVelBox.Location = new System.Drawing.Point(112, 142);
            this.FinalVelBox.Name = "FinalVelBox";
            this.FinalVelBox.Size = new System.Drawing.Size(87, 20);
            this.FinalVelBox.TabIndex = 7;
            this.FinalVelBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.FinalVelBox.Value = new decimal(new int[] {
            17,
            0,
            0,
            0});
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(37, 220);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(67, 13);
            this.label3.TabIndex = 12;
            this.label3.Text = "Final Density";
            // 
            // FinalDensBox
            // 
            this.FinalDensBox.DecimalPlaces = 5;
            this.FinalDensBox.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.FinalDensBox.Location = new System.Drawing.Point(112, 218);
            this.FinalDensBox.Name = "FinalDensBox";
            this.FinalDensBox.Size = new System.Drawing.Size(87, 20);
            this.FinalDensBox.TabIndex = 11;
            this.FinalDensBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.FinalDensBox.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(45, 194);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(59, 13);
            this.label4.TabIndex = 10;
            this.label4.Text = "Init Density";
            // 
            // InitDensBox
            // 
            this.InitDensBox.DecimalPlaces = 5;
            this.InitDensBox.Increment = new decimal(new int[] {
            5,
            0,
            0,
            65536});
            this.InitDensBox.Location = new System.Drawing.Point(112, 192);
            this.InitDensBox.Name = "InitDensBox";
            this.InitDensBox.Size = new System.Drawing.Size(87, 20);
            this.InitDensBox.TabIndex = 9;
            this.InitDensBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.InitDensBox.Value = new decimal(new int[] {
            431,
            0,
            0,
            131072});
            // 
            // Full
            // 
            this.Full.Location = new System.Drawing.Point(77, 445);
            this.Full.Name = "Full";
            this.Full.Size = new System.Drawing.Size(100, 23);
            this.Full.TabIndex = 13;
            this.Full.Text = "Full";
            this.Full.UseVisualStyleBackColor = true;
            this.Full.Click += new System.EventHandler(this.Full_Click);
            // 
            // FreqResButton
            // 
            this.FreqResButton.Location = new System.Drawing.Point(77, 474);
            this.FreqResButton.Name = "FreqResButton";
            this.FreqResButton.Size = new System.Drawing.Size(100, 23);
            this.FreqResButton.TabIndex = 14;
            this.FreqResButton.Text = "Freq Resolution";
            this.FreqResButton.UseVisualStyleBackColor = true;
            this.FreqResButton.Click += new System.EventHandler(this.FreqResButton_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(37, 295);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(61, 13);
            this.label5.TabIndex = 18;
            this.label5.Text = "Final Depth";
            // 
            // FinalDepthBox
            // 
            this.FinalDepthBox.DecimalPlaces = 5;
            this.FinalDepthBox.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.FinalDepthBox.Location = new System.Drawing.Point(112, 293);
            this.FinalDepthBox.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.FinalDepthBox.Name = "FinalDepthBox";
            this.FinalDepthBox.Size = new System.Drawing.Size(87, 20);
            this.FinalDepthBox.TabIndex = 17;
            this.FinalDepthBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.FinalDepthBox.Value = new decimal(new int[] {
            9,
            0,
            0,
            65536});
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(45, 269);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(53, 13);
            this.label6.TabIndex = 16;
            this.label6.Text = "Init Depth";
            // 
            // InitDepthBox
            // 
            this.InitDepthBox.DecimalPlaces = 5;
            this.InitDepthBox.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.InitDepthBox.Location = new System.Drawing.Point(112, 267);
            this.InitDepthBox.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.InitDepthBox.Name = "InitDepthBox";
            this.InitDepthBox.Size = new System.Drawing.Size(87, 20);
            this.InitDepthBox.TabIndex = 15;
            this.InitDepthBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.InitDepthBox.Value = new decimal(new int[] {
            9,
            0,
            0,
            65536});
            // 
            // RippleDemoForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(940, 554);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.FinalDepthBox);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.InitDepthBox);
            this.Controls.Add(this.FreqResButton);
            this.Controls.Add(this.Full);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.FinalDensBox);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.InitDensBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.FinalVelBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.InitVelBox);
            this.Controls.Add(this.zedGraph);
            this.Controls.Add(this.ExitButton);
            this.Controls.Add(this.ApplyButton);
            this.Name = "RippleDemoForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Moving Ripple Noise Demo";
            ((System.ComponentModel.ISupportInitialize)(this.InitVelBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.FinalVelBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.FinalDensBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.InitDensBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.FinalDepthBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.InitDepthBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button ApplyButton;
        private System.Windows.Forms.Button ExitButton;
        private ZedGraph.ZedGraphControl zedGraph;
        private System.Windows.Forms.NumericUpDown InitVelBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown FinalVelBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown FinalDensBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown InitDensBox;
        private System.Windows.Forms.Button Full;
        private System.Windows.Forms.Button FreqResButton;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.NumericUpDown FinalDepthBox;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.NumericUpDown InitDepthBox;
    }
}

