namespace KLib.Controls
{
    partial class ValueSlider
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.maxLabel = new System.Windows.Forms.Label();
            this.minLabel = new System.Windows.Forms.Label();
            this.titleLabel = new System.Windows.Forms.Label();
            this.valueTrackBar = new System.Windows.Forms.TrackBar();
            this.valueNumeric = new KLib.Controls.KNumericBox();
            ((System.ComponentModel.ISupportInitialize)(this.valueTrackBar)).BeginInit();
            this.SuspendLayout();
            // 
            // maxLabel
            // 
            this.maxLabel.AutoSize = true;
            this.maxLabel.ForeColor = System.Drawing.Color.DarkGray;
            this.maxLabel.Location = new System.Drawing.Point(146, 46);
            this.maxLabel.Name = "maxLabel";
            this.maxLabel.Size = new System.Drawing.Size(43, 13);
            this.maxLabel.TabIndex = 33;
            this.maxLabel.Text = "opaque";
            this.maxLabel.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // minLabel
            // 
            this.minLabel.AutoSize = true;
            this.minLabel.ForeColor = System.Drawing.Color.DarkGray;
            this.minLabel.Location = new System.Drawing.Point(6, 46);
            this.minLabel.Name = "minLabel";
            this.minLabel.Size = new System.Drawing.Size(60, 13);
            this.minLabel.TabIndex = 32;
            this.minLabel.Text = "transparent";
            // 
            // titleLabel
            // 
            this.titleLabel.AutoSize = true;
            this.titleLabel.Location = new System.Drawing.Point(4, 2);
            this.titleLabel.Name = "titleLabel";
            this.titleLabel.Size = new System.Drawing.Size(34, 13);
            this.titleLabel.TabIndex = 31;
            this.titleLabel.Text = "Alpha";
            // 
            // valueTrackBar
            // 
            this.valueTrackBar.Location = new System.Drawing.Point(0, 23);
            this.valueTrackBar.Maximum = 100;
            this.valueTrackBar.Name = "valueTrackBar";
            this.valueTrackBar.Size = new System.Drawing.Size(192, 45);
            this.valueTrackBar.TabIndex = 30;
            this.valueTrackBar.TabStop = false;
            this.valueTrackBar.TickFrequency = 10;
            this.valueTrackBar.ValueChanged += new System.EventHandler(this.valueTrackBar_ValueChanged);
            // 
            // valueNumeric
            // 
            this.valueNumeric.AutoSize = true;
            this.valueNumeric.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.valueNumeric.FloatValue = 0F;
            this.valueNumeric.IntValue = 0;
            this.valueNumeric.IsInteger = false;
            this.valueNumeric.Location = new System.Drawing.Point(146, 2);
            this.valueNumeric.MaxCoerce = false;
            this.valueNumeric.MaximumSize = new System.Drawing.Size(20000, 20);
            this.valueNumeric.MaxValue = 1.7976931348623157E+308D;
            this.valueNumeric.MinCoerce = false;
            this.valueNumeric.MinimumSize = new System.Drawing.Size(10, 20);
            this.valueNumeric.MinValue = 0D;
            this.valueNumeric.Name = "valueNumeric";
            this.valueNumeric.Size = new System.Drawing.Size(43, 20);
            this.valueNumeric.TabIndex = 35;
            this.valueNumeric.TextFormat = "K4";
            this.valueNumeric.Units = "";
            this.valueNumeric.Value = 0D;
            this.valueNumeric.ValueChanged += new System.EventHandler(this.valueNumeric_ValueChanged);
            // 
            // ValueSlider
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.valueNumeric);
            this.Controls.Add(this.maxLabel);
            this.Controls.Add(this.minLabel);
            this.Controls.Add(this.titleLabel);
            this.Controls.Add(this.valueTrackBar);
            this.Name = "ValueSlider";
            this.Size = new System.Drawing.Size(193, 71);
            this.Resize += new System.EventHandler(this.KValueSlider_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.valueTrackBar)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label maxLabel;
        private System.Windows.Forms.Label minLabel;
        private System.Windows.Forms.Label titleLabel;
        public System.Windows.Forms.TrackBar valueTrackBar;
        private KNumericBox valueNumeric;
    }
}
