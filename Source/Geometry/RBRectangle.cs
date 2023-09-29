using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Runtime.Serialization;

using Vector = System.Windows.Vector;


namespace KLib.Geometry
{
    /// <summary>
    /// RBRectangle class description. Hooray!
    /// </summary>
    [DataContract]
    public class RBRectangle
    {
        [DataMember]
        private Rectangle rect;

        private Point[] controlPoints;

        private Rectangle dragStartRect;
        private Point dragStartControl;
        private Point dragStartLocation;

        Cursor selectionCursor;
        ControlPointType controlPointType;

        private enum ControlPointType { None, Move, TopLeft, Top, TopRight, Right, BottomRight, Bottom, BottomLeft, Left};

        public int X
        {
            get { return rect.Left; }
        }

        public int Left
        {
            get { return rect.Left; }
        }
        public int Y
        {
            get { return rect.Top; }
        }

        public int Top
        {
            get { return rect.Top; }
        }

        public int Width
        {
            get { return rect.Width; }
        }

        public int Height
        {
            get { return rect.Height; }
        }

        public Rectangle Value
        {
            get { return rect; }
        }

        public Cursor ControlPointCursor
        {
            get { return selectionCursor; }
        }

        private RBRectangle() : this(0,0,1,1)
        {
        }

        public RBRectangle(int X, int Y, int W, int H)
        {
            rect = new Rectangle(X, Y, W, H);
            controlPointType = ControlPointType.None;
            UpdateControlPoints();
        }

        public RBRectangle(Rectangle rect)
        {
            this.rect = rect;
            controlPointType = ControlPointType.None;
            UpdateControlPoints();
        }

        public void Initialize()
        {
            UpdateControlPoints();
        }

        private void UpdateControlPoints()
        {
            controlPoints = new Point[8];

            for (int k = 0; k < controlPoints.Length; k++)
            {
                controlPoints[k] = new Point(rect.Left, rect.Top);
            }
            controlPoints[1].Offset(rect.Width/2, 0);
            controlPoints[2].Offset(rect.Width, 0);
            controlPoints[3].Offset(rect.Width, rect.Height/2);
            controlPoints[4].Offset(rect.Width, rect.Height);
            controlPoints[5].Offset(rect.Width/2, rect.Height);
            controlPoints[6].Offset(0, rect.Height);
            controlPoints[7].Offset(0, rect.Height/2);
        }

        public Rectangle[] GetControlPointRects(int size)
        {
            Rectangle[] cpr = new Rectangle[controlPoints.Length];

            for (int k = 0; k < cpr.Length; k++)
            {
                cpr[k] = new Rectangle(controlPoints[k].X - size / 2, controlPoints[k].Y - size / 2, size, size);
            }

            return (cpr);
        }

        public bool ControlPointSelected(int X, int Y, int tolerance)
        {
            int selPt = -1;

            for (int k = 0; k < controlPoints.Length; k++)
            {
                if (Geometry.CircleContains(new Point(X, Y), controlPoints[k], tolerance))
                {
                    selPt = k;
                    break;
                }
            }

            if (selPt >= 0)
            {
                controlPointType = (ControlPointType)(selPt + 2);
                switch (controlPointType)
                {
                    case ControlPointType.TopLeft:
                    case ControlPointType.BottomRight:
                        selectionCursor = Cursors.SizeNWSE;
                        break;

                    case ControlPointType.Top:
                    case ControlPointType.Bottom:
                        selectionCursor = Cursors.SizeNS;
                        break;

                    case ControlPointType.TopRight:
                    case ControlPointType.BottomLeft:
                        selectionCursor = Cursors.SizeNESW;
                        break;

                    case ControlPointType.Right:
                    case ControlPointType.Left:
                        selectionCursor = Cursors.SizeWE;
                        break;
                }
            }
            else
            {
                Point pt = new Point(X,Y);
                
                Rectangle inner = rect;
                inner.Inflate(-tolerance, -tolerance);
                inner.Offset(tolerance/2, tolerance/2);
                
                Rectangle outer = rect;
                outer.Inflate(tolerance, tolerance);
                outer.Offset(-tolerance/2, -tolerance/2);

                if (outer.Contains(pt) && !inner.Contains(pt))
                {
                    controlPointType = ControlPointType.Move;
                    selectionCursor = Cursors.SizeAll;
                }
                else
                {
                    controlPointType = ControlPointType.None;
                    selectionCursor = Cursors.Arrow;
                }
            }

            dragStartControl.X = X;
            dragStartControl.Y = Y;
            dragStartRect = rect;

            return (controlPointType != ControlPointType.None);
        }

        public void Drag(int X, int Y)
        {
            int dx, dy;

            dx = X - dragStartControl.X;
            dy = Y - dragStartControl.Y;

            Vector v = new Vector(dx, dy);

            if (controlPointType == ControlPointType.None)
            {
                return;
            }

            if (controlPointType == ControlPointType.Move)
            {
                rect = dragStartRect;
                rect.Offset(dx, dy);
            }
            else
            {
                int x = dragStartRect.X;
                int y = dragStartRect.Y;
                int w = dragStartRect.Width;
                int h = dragStartRect.Height;

                switch (controlPointType)
                {
                    case ControlPointType.TopLeft:
                        x += dx;
                        w -= dx;
                        y += dy;
                        h -= dy;
                        break;
                    case ControlPointType.Top:
                        y += dy;
                        h -= dy;
                        break;
                    case ControlPointType.TopRight:
                        w += dx;
                        y += dy;
                        h -= dy;
                        break;
                    case ControlPointType.Right:
                        w += dx;
                        break;
                    case ControlPointType.BottomRight:
                        w += dx;
                        h += dy;
                        break;
                    case ControlPointType.Bottom:
                        h += dy;
                        break;
                    case ControlPointType.BottomLeft:
                        x += dx;
                        w -= dx;
                        h += dy;
                        break;
                    case ControlPointType.Left:
                        x += dx;
                        w -= dx;
                        break;
                }

                if (w < 0)
                {
                    x = x + w;
                    w = -w;
                }
                if (h < 0)
                {
                    y = y + h;
                    h = -h;
                }

                rect = new Rectangle(x, y, w, h);
            }


            UpdateControlPoints();
        }

        public void Resize(int W, int H)
        {
            rect.Inflate((int)Math.Round(0.5 * (float)(W - rect.Width)), (int)Math.Round(0.5 * (float)(H - rect.Height)));
            UpdateControlPoints();
        }

        public override String ToString()
        {
            return rect.ToString();
        }

        public static RBRectangle FromString(String str)
        {
            Regex rx = new Regex(@"([\+-]?\d+\.?\d*)");
            MatchCollection matches = rx.Matches(str);
            int x=0, y=0, w=1, h=1;

            if (matches.Count == 4)
            {
                x = Convert.ToInt32(matches[0].Value);
                y = Convert.ToInt32(matches[1].Value);
                w = Convert.ToInt32(matches[2].Value);
                h = Convert.ToInt32(matches[3].Value);
            }

            return new RBRectangle(x, y, w, h);
        }

    }
}
