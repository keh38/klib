using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

using Vector = System.Windows.Vector;


namespace KLib.KGraphics
{
    /// <summary>
    /// RBCircle class description. Hooray!
    /// </summary>
    public class RBCircle
    {
        private Point location;
        private int radius;

        private Point[] controlPoints;

        private Point dragStartControl;
        private Point dragStartLocation;
        private int dragStartRadius;
        private Vector Vref;

        private int top;
        private int left;
        private int diameter;

        private bool resizeOnly;

        Cursor selectionCursor;
        ControlPointType controlPointType;

        private enum ControlPointType { None, Move, TopLeft, Top, TopRight, Right, BottomRight, Bottom, BottomLeft, Left, Resize };

        public Rectangle Bounds
        {
            get { return new Rectangle(left, top, diameter, diameter); }
        }
        public int Left
        {
            get { return left; }
        }
        public int Top
        {
            get { return top; }
        }
        public int Diameter
        {
            get { return diameter; }
            set 
            { 
                radius = value/2;
                ApplyLocationAndSize();
            }
        }

        public Point Center
        {
            get { return location; }
            set
            {
                location = value;
                ApplyLocationAndSize();
            }
        }

        public bool ResizeOnly
        {
            set { resizeOnly = value; }
        }
 
        public Cursor ControlPointCursor
        {
            get { return selectionCursor; }
        }

        private RBCircle()
        {
            controlPointType = ControlPointType.None;
            resizeOnly = false;
        }

        public RBCircle(Point location, int radius)
            : this()
        {
            this.location = location;
            this.radius = radius;
            ApplyLocationAndSize();
        }

        public RBCircle(int X, int Y, int R)
            : this()
        {
            location.X = X;
            location.Y = Y;
            this.radius = R;

            ApplyLocationAndSize();
        }

        private void ApplyLocationAndSize()
        {
            top = location.Y - radius;
            left = location.X - radius;
            diameter = 2 * radius;

            UpdateControlPoints();
        }
        
        private void UpdateControlPoints()
        {
            controlPoints = new Point[8];

            for (int k = 0; k < controlPoints.Length; k++)
            {
                controlPoints[k] = new Point(location.X, location.Y);
            }
            controlPoints[0].Offset(-radius, -radius);
            controlPoints[1].Offset(0, -radius);
            controlPoints[2].Offset(radius, -radius);
            controlPoints[3].Offset(radius, 0);
            controlPoints[4].Offset(radius, radius);
            controlPoints[5].Offset(0, radius);
            controlPoints[6].Offset(-radius, radius);
            controlPoints[7].Offset(-radius, 0);
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

            if (!resizeOnly)
            {
                for (int k = 0; k < controlPoints.Length; k++)
                {
                    if (Geometry.CircleContains(new Point(X, Y), controlPoints[k], tolerance))
                    {
                        selPt = k;
                        break;
                    }
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
                        Vref = new Vector(1 / Math.Sqrt(2), 1 / Math.Sqrt(2));
                        if (controlPointType == ControlPointType.TopLeft) Vref = -Vref;
                        break;

                    case ControlPointType.Top:
                    case ControlPointType.Bottom:
                        selectionCursor = Cursors.SizeNS;
                        Vref = new Vector(0, 1);
                        if (controlPointType == ControlPointType.Top) Vref = -Vref;
                        break;

                    case ControlPointType.TopRight:
                    case ControlPointType.BottomLeft:
                        selectionCursor = Cursors.SizeNESW;
                        Vref = new Vector(1 / Math.Sqrt(2), -1 / Math.Sqrt(2));
                        if (controlPointType == ControlPointType.BottomLeft) Vref = -Vref;
                        break;

                    case ControlPointType.Right:
                    case ControlPointType.Left:
                        selectionCursor = Cursors.SizeWE;
                        Vref = new Vector(1, 0);
                        if (controlPointType == ControlPointType.Left) Vref = -Vref;
                        break;
                }
            }
            else
            {
                if (Geometry.AnnulusContains(new Point(X, Y), location, radius - tolerance, radius + tolerance))
                {
                    if (!resizeOnly)
                    {
                        controlPointType = ControlPointType.Move;
                        selectionCursor = Cursors.SizeAll;
                    }
                    else
                    {
                        controlPointType = ControlPointType.Resize;
                        selectionCursor = Cursors.SizeAll;
                    }
                }
                else
                {
                    controlPointType = ControlPointType.None;
                    selectionCursor = Cursors.Arrow;
                }
            }

            dragStartControl.X = X;
            dragStartControl.Y = Y;
            dragStartLocation = location;
            dragStartRadius = radius;

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
                location = dragStartLocation;
                location.Offset(dx, dy);
            }
            else
            {
                double dr = Vector.Multiply(v, Vref);

                radius = dragStartRadius + (int)(dr / 2);
                radius = Math.Max(radius, 10);

                switch (controlPointType)
                {
                    case ControlPointType.TopLeft:
                        location.X = controlPoints[4].X - radius;
                        location.Y = controlPoints[4].Y - radius;
                        break;
                    case ControlPointType.Top:
                        location.Y = controlPoints[5].Y - radius;
                        break;
                    case ControlPointType.TopRight:
                        location.X = controlPoints[6].X + radius;
                        location.Y = controlPoints[6].Y - radius;
                        break;
                    case ControlPointType.Right:
                        location.X = controlPoints[7].X + radius;
                        break;
                    case ControlPointType.BottomRight:
                        location.X = controlPoints[0].X + radius;
                        location.Y = controlPoints[0].Y + radius;
                        break;
                    case ControlPointType.Bottom:
                        location.Y = controlPoints[1].Y + radius;
                        break;
                    case ControlPointType.BottomLeft:
                        location.X = controlPoints[2].X - radius;
                        location.Y = controlPoints[2].Y - radius;
                        break;
                    case ControlPointType.Left:
                        location.X = controlPoints[3].X - radius;
                        break;
                }
            }

            ApplyLocationAndSize();
        }

        public void Resize(int X, int Y, int Rmin, int Rmax)
        {
            int dx, dy;

            dx = X - dragStartControl.X;
            dy = Y - dragStartControl.Y;

            Vector v = new Vector(dx, dy);

            if (controlPointType != ControlPointType.Resize)
            {
                return;
            }

            double r = Geometry.Distance(location, new Point(X, Y));
            r = Math.Max(r, Rmin);
            radius = Math.Min((int)r, Rmax);

            ApplyLocationAndSize();
        }

        public override String ToString()
        {
            String str = location.X.ToString() + ", " + location.Y.ToString() + ", " + radius.ToString();
            return (str);
        }

        public void FromString(String str)
        {
            Regex rx = new Regex(@"([\+-]?\d+\.?\d*)");
            MatchCollection matches = rx.Matches(str);
            int x, y, r;
            double a;

            if (matches.Count == 3)
            {
                x = Convert.ToInt32(matches[0].Value);
                y = Convert.ToInt32(matches[1].Value);
                r = Convert.ToInt32(matches[2].Value);

                if (x >= 0 && y >= 0 && r > 0)
                {
                    location.X = x;
                    location.Y = y;
                    radius = r;
                    ApplyLocationAndSize();
                }

            }


        }

    }
}
