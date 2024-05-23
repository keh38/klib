 using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Turandot_Editor
{
    public class KCheckBox : CheckBox
    {
        private static int _boxSize = 15;
        private Rectangle _boxRect;
        private ImageList imageList;
        private System.ComponentModel.IContainer components;
        private RectangleF _textRect;
        private Point _checkLocation;

        public KCheckBox() : base()
        {
            InitializeComponent();

            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.ResizeRedraw, true);
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            UpdateStyles();
        }

        protected override void OnLayout(LayoutEventArgs levent)
        {
            Size sz = TextRenderer.MeasureText(Text, Font);

            int left = 1 + Padding.Left;

            int imageWidth = imageList.Images["Check"].Width;
            int imageHeight = imageList.Images["Check"].Height;
            int boxcenter = left + imageWidth/2;
            int h = Math.Max(imageHeight, sz.Height); // + Margin.Top + Margin.Bottom;
            h = imageHeight;

            _checkLocation = new Point(boxcenter - imageWidth / 2, h / 2 - imageHeight / 2);

            _textRect = new Rectangle(2 * Padding.Left + 6 + imageWidth, h / 2 - sz.Height / 2, sz.Width + 50, sz.Height);
            _boxRect = new Rectangle(boxcenter - _boxSize / 2, h / 2 - _boxSize / 2, _boxSize, _boxSize);

            SetClientSizeCore(2*Padding.Left + imageWidth + sz.Width + Margin.Right+50, h+1);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.Clear(BackColor);

            e.Graphics.FillRectangle(Brushes.White, _boxRect);
            e.Graphics.DrawRectangle(Pens.Black, _boxRect);
            using (StringFormat stringFormat = new StringFormat())
            {
                stringFormat.LineAlignment = StringAlignment.Near;
                e.Graphics.DrawString(Text, Font, Brushes.Black, _textRect, stringFormat);
            }

            if (Checked)
            {
                e.Graphics.DrawImageUnscaled(imageList.Images["Check"], _checkLocation);
            }
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(KCheckBox));
            this.imageList = new System.Windows.Forms.ImageList(this.components);
            this.SuspendLayout();
            // 
            // imageList
            // 
            this.imageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList.ImageStream")));
            this.imageList.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList.Images.SetKeyName(0, "Check");
            this.ResumeLayout(false);

        }
    }
}
