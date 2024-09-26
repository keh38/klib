using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;


namespace KLib.KGraphics
{
    public class RotatedRectangle
    {
        private Point location;
        private Point size;
        private Point dragStartControl;
        private Point dragStartLocation;
        private double angle;
        private Point[] unrotVertices;
        private Point[] vertices;
        private double[,] rotMat;
        Cursor selectionCursor;
        ControlPointType controlPointType;

        private enum ControlPointType { None, Move, TopRight, BottomRight, BottomLeft };

        public Point[] Vertices
        {
            get { return vertices; }
        }

        public Cursor ControlPointCursor
        {
            get { return selectionCursor; }
        }

        private RotatedRectangle()
        {
            controlPointType = ControlPointType.None;

            unrotVertices = new Point[4];
            vertices = new Point[4];
            rotMat = new double[2, 2];
        }

        public RotatedRectangle(Point loc, Point sz, double phi)
            : this()
        {
            location = loc;
            size = sz;
            angle = phi * Math.PI / 180;

            ApplyAngle();
            ApplyLocationAndSize();
            Rotate();
        }

        public RotatedRectangle(int X, int Y, int W, int H, double phi)
            : this()
        {
            location.X = X;
            location.Y = Y;
            size.X = W;
            size.Y = H;
            angle = phi * Math.PI / 180;

            ApplyAngle();
            ApplyLocationAndSize();
            Rotate();
        }

        private void ApplyAngle()
        {
            rotMat[0, 0] = Math.Cos(angle);
            rotMat[0, 1] = -Math.Sin(angle);
            rotMat[1, 0] = -rotMat[0, 1];
            rotMat[1, 1] = rotMat[0, 0];
        }

        private void ApplyLocationAndSize()
        {
            for (int k = 0; k < unrotVertices.Length; k++)
            {
                unrotVertices[k] = location;
            }
            unrotVertices[1].X += size.X;
            unrotVertices[2].X += size.X;
            unrotVertices[2].Y += size.Y;
            unrotVertices[3].Y += size.Y;
        }

        public bool ControlPointSelected(int X, int Y, int radius)
        {
            int selPt = -1;

            for (int k = 1; k < vertices.Length; k++)
            {
                int dist = (int)Math.Pow((double)Math.Abs(X - vertices[k].X), 2) + (int)Math.Pow((double)Math.Abs(Y - vertices[k].Y), 2);

                if (dist <= radius * radius)
                {
                    selPt = k;
                    break;
                }
            }

            switch (selPt)
            {
                case 1:
                    controlPointType = ControlPointType.TopRight;
                    selectionCursor = Cursors.SizeNESW;
                    break;
                case 2:
                    controlPointType = ControlPointType.BottomRight;
                    selectionCursor = Cursors.SizeNWSE;
                    break;
                case 3:
                    controlPointType = ControlPointType.BottomLeft;
                    selectionCursor = Cursors.SizeNESW;
                    break;
                default:
                    if (Contains(X, Y))
                    {
                        controlPointType = ControlPointType.Move;
                        selectionCursor = Cursors.SizeAll;
                    }
                    else
                    {
                        controlPointType = ControlPointType.None;
                        selectionCursor = Cursors.Arrow;
                    }
                    break;
            }

            dragStartControl.X = X;
            dragStartControl.Y = Y;
            dragStartLocation = location;

            return (controlPointType != ControlPointType.None);
        }

        public bool Contains(int X, int Y)
        {
            return Contains(new Point(X, Y));
        }

        public bool Contains(Point pt)
        {
            int dx, dy;

            pt = RotatePoint(pt, false);

            dx = pt.X - location.X;
            dy = -(pt.Y - location.Y);

            return (dx > 0 && dx < size.X && dy > 0 && dy < size.Y);
        }

        public Rectangle[] GetControlPoints(int radius)
        {
            Rectangle[] cp = new Rectangle[3];

            for (int k = 0; k < cp.Length; k++)
            {
                cp[k] = new Rectangle(vertices[k].X - radius / 2, vertices[k].Y - radius / 2, radius, radius);
            }

            return (cp);
        }

        public void Drag(int X, int Y)
        {
            double u, v;
            int dx, dy;

            dx = X - dragStartControl.X;
            dy = Y - dragStartControl.Y;

            switch (controlPointType)
            {
                case ControlPointType.Move:
                    location = dragStartLocation;
                    location.Offset(dx, dy);
                    break;

                case ControlPointType.TopRight:
                    u = (double)(X - dragStartLocation.X);
                    v = (double)(Y - dragStartLocation.Y);

                    size.X = Math.Max(1, (int)Math.Round(Math.Sqrt(u * u + v * v)));
                    angle = Math.Atan2(v, u);
                    ApplyAngle();
                    break;

                case ControlPointType.BottomRight:
                    u = (double)(X - vertices[1].X);
                    v = (double)(Y - vertices[1].Y);

                    size.Y = Math.Max(1, (int)Math.Round(Math.Sqrt(u * u + v * v)));

                    break;
            }

            ApplyLocationAndSize();
            Rotate();
        }

        private void Rotate()
        {
            vertices[0] = location;
            for (int k = 1; k < 4; k++)
            {
                vertices[k] = RotatePoint(unrotVertices[k], true);
            }
        }

        private Point RotatePoint(Point pt, bool forward)
        {
            double dx, dy;
            Point rotatedPoint = new Point();
            double directionFactor = (forward) ? 1 : -1;

            dx = pt.X - location.X;
            dy = pt.Y - location.Y;

            rotatedPoint.X = location.X + (int)(dx * rotMat[0, 0] + dy * directionFactor * rotMat[0, 1]);
            rotatedPoint.Y = location.Y + (int)(dx * rotMat[1, 0] + dy * directionFactor * rotMat[1, 1]);

            return (rotatedPoint);
        }

        public override String ToString()
        {
            String str = location.X.ToString() + ", " + location.Y.ToString() + ", " + size.X.ToString() + ", " + size.Y.ToString() + "," + angle.ToString();
            return (str);
        }

        public void FromString(String str)
        {
            Regex rx = new Regex(@"([\+-]?\d+\.?\d*)");
            MatchCollection matches = rx.Matches(str);
            int x, y, w, h;
            double a;

            if (matches.Count == 5)
            {
                x = Convert.ToInt32(matches[0].Value);
                y = Convert.ToInt32(matches[1].Value);
                w = Convert.ToInt32(matches[2].Value);
                h = Convert.ToInt32(matches[3].Value);
                a = Convert.ToDouble(matches[4].Value);

                if (x >= 0 && y >= 0 && w > 0 & h > 0)
                {
                    location.X = x;
                    location.Y = y;
                    size.X = w;
                    size.Y = h;
                    angle = a;

                    ApplyAngle();
                    ApplyLocationAndSize();
                    Rotate();
                }

            }


        }

    }
}
