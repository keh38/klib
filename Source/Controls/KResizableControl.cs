using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace KLib.Controls
{
    public partial class KResizableControl : KUserControl
    {
        [TypeConverter(typeof(SizableSidesTypeConverter))]
        public class SizableSides
        {
            public bool Left { get; set; }
            public bool Top { get; set; }
            public bool Right { get; set; }
            public bool Bottom { get; set; }
            public SizableSides()
            {
                this.Left = this.Top = this.Right = this.Bottom = false;
            }
            public override string ToString()
            {
                return Left + "," + Top + "," + Right + "," + Bottom;
            }
        }

        public class SizableSidesTypeConverter : TypeConverter
        {
            public override bool GetPropertiesSupported(ITypeDescriptorContext context)
            {
                return true;
            }
            public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, object value, Attribute[] attributes)
            {
                return TypeDescriptor.GetProperties(typeof(SizableSides));
            }
        }

        [Flags]
        private enum ResizeDirection
        {
            None = 0,
            Left = 1,
            Top = 2,
            Right = 4,
            Bottom = 8
        }

        private ResizeDirection _direction = ResizeDirection.None;
        private SizableSides _sizable = new SizableSides();
        private Point _startPoint;
        private Point _startLocation;
        private Size _startSize;

        public KResizableControl()
        {
            InitializeComponent();
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public SizableSides Sizable
        {
            get { return _sizable; }
            set { _sizable = value; }
        }

        private void KResizableControl_MouseDown(object sender, MouseEventArgs e)
        {
            int margin = 10;

            _direction = ResizeDirection.None;
            if (_sizable.Left && e.X < margin) _direction |= ResizeDirection.Left;
            if (_sizable.Top && e.Y < margin) _direction |= ResizeDirection.Top;
            if (_sizable.Right && e.X > this.Width - margin) _direction |= ResizeDirection.Right;
            if (_sizable.Bottom && e.Y > this.Height - margin) _direction |= ResizeDirection.Bottom;

            if (_direction == ResizeDirection.None)
            {
                this.Cursor = Cursors.Default;
            }
            else
            {
                _startPoint = PointToScreen(e.Location);
                _startLocation = this.Location;
                _startSize = this.Size;

                if ((_direction & ResizeDirection.Left) != 0)
                {
                    if ((_direction & ResizeDirection.Top) != 0) this.Cursor = Cursors.SizeNWSE;
                    else if ((_direction & ResizeDirection.Bottom) != 0) this.Cursor = Cursors.SizeNESW;
                    else this.Cursor = Cursors.SizeWE;
                }
                else if ((_direction & ResizeDirection.Right) != 0)
                {
                    if ((_direction & ResizeDirection.Top) != 0) this.Cursor = Cursors.SizeNESW;
                    else if ((_direction & ResizeDirection.Bottom) != 0) this.Cursor = Cursors.SizeNWSE;
                    else this.Cursor = Cursors.SizeWE;
                }
                else if ((_direction & ResizeDirection.Top) != 0 || (_direction & ResizeDirection.Bottom) != 0)
                {
                    this.Cursor = Cursors.SizeNS;
                }

            }

        }

        private void KResizableControl_MouseUp(object sender, MouseEventArgs e)
        {
            _direction = ResizeDirection.None;
            this.Cursor = Cursors.Default;
        }

        private void KResizableControl_MouseMove(object sender, MouseEventArgs e)
        {
            if (_direction == ResizeDirection.None) return;
            
            int dx = 0;
            int dy = 0;
            int dw = 0;
            int dh = 0;

            Point pt = PointToScreen(e.Location);

            if ((_direction & ResizeDirection.Left) != 0)
            {
                dx = pt.X - _startPoint.X;
                dw = -dx;
            }
            if ((_direction & ResizeDirection.Top) != 0)
            {
                dy = pt.Y - _startPoint.Y;
                dh = -dy;
            }
            if ((_direction & ResizeDirection.Right) != 0)
            {
                dw = pt.X - _startPoint.X;
            }
            if ((_direction & ResizeDirection.Bottom) != 0)
            {
                dh = pt.Y - _startPoint.Y;
            }

            this.Location = new Point(_startLocation.X + dx, _startLocation.Y + dy);
            this.Width = _startSize.Width + dw;
            this.Height = _startSize.Height + dh;
        }
    }
}
