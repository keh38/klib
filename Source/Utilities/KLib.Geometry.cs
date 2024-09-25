using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;


namespace KLib
{
    public class Geometry
    {
        static public double Distance(Point p1, Point p2)
        {
            double dist = Math.Sqrt(Math.Pow((double)Math.Abs(p1.X - p2.X), 2) + (double)Math.Pow((double)Math.Abs(p1.Y - p2.Y), 2));
            return (dist);
        }

        static public double Distance(double x1, double y1, double x2, double y2)
        {
            double dist = Math.Sqrt(Math.Pow(Math.Abs(x1 - x2), 2) + Math.Pow(Math.Abs(y1 - y2), 2));
            return (dist);
        }

        static public bool CircleContains(Point pt, Point center, int radius)
        {
            int dist = (int)(Math.Pow((double)Math.Abs(pt.X - center.X), 2) + Math.Pow((double)Math.Abs(pt.Y - center.Y), 2));
            return (dist <= radius * radius);
        }

        static public bool AnnulusContains(Point pt, Point center, int innerRadius, int outerRadius)
        {
            int dist = (int)Math.Pow((double)Math.Abs(pt.X - center.X), 2) + (int)Math.Pow((double)Math.Abs(pt.Y - center.Y), 2);
            return ((dist <= outerRadius * outerRadius) && (dist >= innerRadius*innerRadius));
        }

        static public Rectangle RectangleByCenter(Point center, int width, int height)
        {
            int xl = center.X - width / 2;
            int yl = center.Y - height / 2;
            return new Rectangle(xl, yl, width, height);
        }

        static public Point PointOffset(Point point, Point offset)
        {
            return new Point(point.X + offset.X, point.Y + offset.Y);
        }
        static public Point PointDiff(Point p1, Point p2)
        {
            return new Point(p1.X - p2.X, p1.Y - p2.Y);
        }

    }

}