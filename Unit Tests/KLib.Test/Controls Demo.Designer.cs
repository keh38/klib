namespace KLibUnitTests
{
    partial class ControlsDemoForm
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
            this.thresholdLabel = new System.Windows.Forms.Label();
            this.histogram = new KLib.Controls.Histogram();
            this.colorSlider1 = new KLib.Controls.ColorSlider();
            this.enumDropDown1 = new KLib.Controls.EnumDropDown();
            this.fileBrowser1 = new KLib.Controls.FileBrowser();
            this.SuspendLayout();
            // 
            // thresholdLabel
            // 
            this.thresholdLabel.AutoSize = true;
            this.thresholdLabel.Location = new System.Drawing.Point(149, 316);
            this.thresholdLabel.Name = "thresholdLabel";
            this.thresholdLabel.Size = new System.Drawing.Size(35, 13);
            this.thresholdLabel.TabIndex = 4;
            this.thresholdLabel.Text = "label1";
            // 
            // histogram
            // 
            this.histogram.DisplayColor = System.Drawing.Color.Gray;
            this.histogram.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.histogram.Location = new System.Drawing.Point(32, 198);
            this.histogram.Name = "histogram";
            this.histogram.Offset = 1;
            this.histogram.ShowBox = false;
            this.histogram.Size = new System.Drawing.Size(273, 115);
            this.histogram.TabIndex = 3;
            this.histogram.ThresholdBar.Color = System.Drawing.Color.Red;
            this.histogram.ThresholdBar.Drag = true;
            this.histogram.ThresholdBar.GrabTolerance = 10;
            this.histogram.ThresholdBar.Value = 128;
            this.histogram.ThresholdBar.Visible = true;
            this.histogram.ThresholdBar.Width = 3;
            this.histogram.XScale.Auto = true;
            this.histogram.XScale.Max = 255;
            this.histogram.XScale.Min = 0;
            this.histogram.ValueChanged += new System.EventHandler(this.histogram_ValueChanged);
            // 
            // colorSlider1
            // 
            this.colorSlider1.BackColor = System.Drawing.Color.Transparent;
            this.colorSlider1.BorderRoundRectSize = new System.Drawing.Size(8, 8);
            this.colorSlider1.LargeChange = ((uint)(5u));
            this.colorSlider1.Location = new System.Drawing.Point(32, 139);
            this.colorSlider1.Name = "colorSlider1";
            this.colorSlider1.Size = new System.Drawing.Size(206, 30);
            this.colorSlider1.SmallChange = ((uint)(1u));
            this.colorSlider1.TabIndex = 2;
            this.colorSlider1.Text = "colorSlider1";
            this.colorSlider1.ThumbRoundRectSize = new System.Drawing.Size(8, 8);
            // 
            // enumDropDown1
            // 
            this.enumDropDown1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.enumDropDown1.FormattingEnabled = true;
            this.enumDropDown1.Location = new System.Drawing.Point(32, 103);
            this.enumDropDown1.Name = "enumDropDown1";
            this.enumDropDown1.Size = new System.Drawing.Size(206, 21);
            this.enumDropDown1.Sort = false;
            this.enumDropDown1.TabIndex = 1;
            // 
            // fileBrowser1
            // 
            this.fileBrowser1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.fileBrowser1.DefaultFolder = null;
            this.fileBrowser1.Filter = null;
            this.fileBrowser1.FoldersOnly = false;
            this.fileBrowser1.Location = new System.Drawing.Point(32, 50);
            this.fileBrowser1.Name = "fileBrowser1";
            this.fileBrowser1.Size = new System.Drawing.Size(206, 20);
            this.fileBrowser1.TabIndex = 0;
            this.fileBrowser1.UseEllipsis = true;
            this.fileBrowser1.Value = "";
            // 
            // ControlsDemoForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(330, 381);
            this.Controls.Add(this.thresholdLabel);
            this.Controls.Add(this.histogram);
            this.Controls.Add(this.colorSlider1);
            this.Controls.Add(this.enumDropDown1);
            this.Controls.Add(this.fileBrowser1);
            this.Name = "ControlsDemoForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Controls Demo";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private KLib.Controls.FileBrowser fileBrowser1;
        private KLib.Controls.EnumDropDown enumDropDown1;
        private KLib.Controls.ColorSlider colorSlider1;
        private KLib.Controls.Histogram histogram;
        private System.Windows.Forms.Label thresholdLabel;
    }
}