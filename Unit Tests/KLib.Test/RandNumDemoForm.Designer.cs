namespace KLibUnitTests
{
    partial class RandNumDemoForm
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
            this.zedGraphControl = new ZedGraph.ZedGraphControl();
            this.FinishedButton = new System.Windows.Forms.Button();
            this.NSamplesBox = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.DistributionCombo = new System.Windows.Forms.ComboBox();
            this.GaussianPanel = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.StdDevBox = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.MeanBox = new System.Windows.Forms.NumericUpDown();
            this.SinPanel = new System.Windows.Forms.Panel();
            this.label5 = new System.Windows.Forms.Label();
            this.ModDepthBox = new System.Windows.Forms.NumericUpDown();
            this.GenerateButton = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.ExpBox = new System.Windows.Forms.NumericUpDown();
            this.TruncPanel = new System.Windows.Forms.Panel();
            this.label12 = new System.Windows.Forms.Label();
            this.TruncSigmaBox = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.NSamplesBox)).BeginInit();
            this.GaussianPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.StdDevBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.MeanBox)).BeginInit();
            this.SinPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ModDepthBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ExpBox)).BeginInit();
            this.TruncPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.TruncSigmaBox)).BeginInit();
            this.SuspendLayout();
            // 
            // zedGraphControl
            // 
            this.zedGraphControl.Location = new System.Drawing.Point(282, 34);
            this.zedGraphControl.Name = "zedGraphControl";
            this.zedGraphControl.ScrollGrace = 0D;
            this.zedGraphControl.ScrollMaxX = 0D;
            this.zedGraphControl.ScrollMaxY = 0D;
            this.zedGraphControl.ScrollMaxY2 = 0D;
            this.zedGraphControl.ScrollMinX = 0D;
            this.zedGraphControl.ScrollMinY = 0D;
            this.zedGraphControl.ScrollMinY2 = 0D;
            this.zedGraphControl.Size = new System.Drawing.Size(316, 259);
            this.zedGraphControl.TabIndex = 0;
            // 
            // FinishedButton
            // 
            this.FinishedButton.Location = new System.Drawing.Point(523, 343);
            this.FinishedButton.Name = "FinishedButton";
            this.FinishedButton.Size = new System.Drawing.Size(75, 23);
            this.FinishedButton.TabIndex = 1;
            this.FinishedButton.Text = "Finished";
            this.FinishedButton.UseVisualStyleBackColor = true;
            this.FinishedButton.Click += new System.EventHandler(this.FinishedButton_Click);
            // 
            // NSamplesBox
            // 
            this.NSamplesBox.Location = new System.Drawing.Point(150, 273);
            this.NSamplesBox.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.NSamplesBox.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.NSamplesBox.Name = "NSamplesBox";
            this.NSamplesBox.Size = new System.Drawing.Size(74, 20);
            this.NSamplesBox.TabIndex = 2;
            this.NSamplesBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.NSamplesBox.Value = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(44, 275);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(100, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Number of samples:";
            // 
            // DistributionCombo
            // 
            this.DistributionCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.DistributionCombo.FormattingEnabled = true;
            this.DistributionCombo.Items.AddRange(new object[] {
            "Gaussian",
            "Raised Cosine",
            "Truncated Normal"});
            this.DistributionCombo.Location = new System.Drawing.Point(103, 34);
            this.DistributionCombo.Name = "DistributionCombo";
            this.DistributionCombo.Size = new System.Drawing.Size(121, 21);
            this.DistributionCombo.TabIndex = 4;
            this.DistributionCombo.SelectedIndexChanged += new System.EventHandler(this.DistributionCombo_SelectedIndexChanged);
            // 
            // GaussianPanel
            // 
            this.GaussianPanel.Controls.Add(this.label3);
            this.GaussianPanel.Controls.Add(this.StdDevBox);
            this.GaussianPanel.Controls.Add(this.label2);
            this.GaussianPanel.Controls.Add(this.MeanBox);
            this.GaussianPanel.Location = new System.Drawing.Point(84, 70);
            this.GaussianPanel.Name = "GaussianPanel";
            this.GaussianPanel.Size = new System.Drawing.Size(160, 100);
            this.GaussianPanel.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(5, 42);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(55, 13);
            this.label3.TabIndex = 9;
            this.label3.Text = "Std. Dev.:";
            // 
            // StdDevBox
            // 
            this.StdDevBox.Location = new System.Drawing.Point(66, 40);
            this.StdDevBox.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.StdDevBox.Name = "StdDevBox";
            this.StdDevBox.Size = new System.Drawing.Size(74, 20);
            this.StdDevBox.TabIndex = 8;
            this.StdDevBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.StdDevBox.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(23, 16);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(37, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "Mean:";
            // 
            // MeanBox
            // 
            this.MeanBox.Location = new System.Drawing.Point(66, 14);
            this.MeanBox.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.MeanBox.Minimum = new decimal(new int[] {
            10000,
            0,
            0,
            -2147483648});
            this.MeanBox.Name = "MeanBox";
            this.MeanBox.Size = new System.Drawing.Size(74, 20);
            this.MeanBox.TabIndex = 6;
            this.MeanBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // SinPanel
            // 
            this.SinPanel.Controls.Add(this.label4);
            this.SinPanel.Controls.Add(this.label5);
            this.SinPanel.Controls.Add(this.ExpBox);
            this.SinPanel.Controls.Add(this.ModDepthBox);
            this.SinPanel.Location = new System.Drawing.Point(84, 70);
            this.SinPanel.Name = "SinPanel";
            this.SinPanel.Size = new System.Drawing.Size(160, 100);
            this.SinPanel.TabIndex = 10;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(5, 37);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(39, 13);
            this.label5.TabIndex = 7;
            this.label5.Text = "Depth:";
            // 
            // ModDepthBox
            // 
            this.ModDepthBox.DecimalPlaces = 2;
            this.ModDepthBox.Location = new System.Drawing.Point(66, 35);
            this.ModDepthBox.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.ModDepthBox.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            -2147483648});
            this.ModDepthBox.Name = "ModDepthBox";
            this.ModDepthBox.Size = new System.Drawing.Size(74, 20);
            this.ModDepthBox.TabIndex = 6;
            this.ModDepthBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.ModDepthBox.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // GenerateButton
            // 
            this.GenerateButton.Location = new System.Drawing.Point(282, 343);
            this.GenerateButton.Name = "GenerateButton";
            this.GenerateButton.Size = new System.Drawing.Size(75, 23);
            this.GenerateButton.TabIndex = 11;
            this.GenerateButton.Text = "Generate!";
            this.GenerateButton.UseVisualStyleBackColor = true;
            this.GenerateButton.Click += new System.EventHandler(this.GenerateButton_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(5, 11);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(55, 13);
            this.label4.TabIndex = 13;
            this.label4.Text = "Exponent:";
            // 
            // ExpBox
            // 
            this.ExpBox.Location = new System.Drawing.Point(66, 9);
            this.ExpBox.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.ExpBox.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.ExpBox.Name = "ExpBox";
            this.ExpBox.Size = new System.Drawing.Size(74, 20);
            this.ExpBox.TabIndex = 12;
            this.ExpBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.ExpBox.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // TruncPanel
            // 
            this.TruncPanel.Controls.Add(this.label12);
            this.TruncPanel.Controls.Add(this.TruncSigmaBox);
            this.TruncPanel.Location = new System.Drawing.Point(84, 70);
            this.TruncPanel.Name = "TruncPanel";
            this.TruncPanel.Size = new System.Drawing.Size(160, 100);
            this.TruncPanel.TabIndex = 12;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(5, 37);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(39, 13);
            this.label12.TabIndex = 7;
            this.label12.Text = "Sigma:";
            // 
            // TruncSigmaBox
            // 
            this.TruncSigmaBox.DecimalPlaces = 2;
            this.TruncSigmaBox.Increment = new decimal(new int[] {
            5,
            0,
            0,
            131072});
            this.TruncSigmaBox.Location = new System.Drawing.Point(66, 35);
            this.TruncSigmaBox.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.TruncSigmaBox.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            -2147483648});
            this.TruncSigmaBox.Name = "TruncSigmaBox";
            this.TruncSigmaBox.Size = new System.Drawing.Size(74, 20);
            this.TruncSigmaBox.TabIndex = 6;
            this.TruncSigmaBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.TruncSigmaBox.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // RandNumDemoForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(646, 393);
            this.Controls.Add(this.TruncPanel);
            this.Controls.Add(this.GenerateButton);
            this.Controls.Add(this.DistributionCombo);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.NSamplesBox);
            this.Controls.Add(this.FinishedButton);
            this.Controls.Add(this.zedGraphControl);
            this.Controls.Add(this.SinPanel);
            this.Controls.Add(this.GaussianPanel);
            this.Name = "RandNumDemoForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Random Number Generation Demo";
            this.Load += new System.EventHandler(this.RandNumDemoForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.NSamplesBox)).EndInit();
            this.GaussianPanel.ResumeLayout(false);
            this.GaussianPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.StdDevBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.MeanBox)).EndInit();
            this.SinPanel.ResumeLayout(false);
            this.SinPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ModDepthBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ExpBox)).EndInit();
            this.TruncPanel.ResumeLayout(false);
            this.TruncPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.TruncSigmaBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ZedGraph.ZedGraphControl zedGraphControl;
        private System.Windows.Forms.Button FinishedButton;
        private System.Windows.Forms.NumericUpDown NSamplesBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox DistributionCombo;
        private System.Windows.Forms.Panel GaussianPanel;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown MeanBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown StdDevBox;
        private System.Windows.Forms.Panel SinPanel;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.NumericUpDown ModDepthBox;
        private System.Windows.Forms.Button GenerateButton;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown ExpBox;
        private System.Windows.Forms.Panel TruncPanel;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.NumericUpDown TruncSigmaBox;
    }
}