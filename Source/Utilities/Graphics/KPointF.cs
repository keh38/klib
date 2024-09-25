using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;


namespace KLib.Graphics
{
    public class KPointF
    {
        public float X = 0;
        public float Y = 0;

        public KPointF() { }
        public KPointF(KPointF p)
        {
            X = p.X;
            Y = p.Y;
        }
        public KPointF(float X, float Y)
        {
            this.X = X;
            this.Y = Y;
        }
        public KPointF(Point p)
        {
            X = p.X;
            Y = p.Y;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is KPointF))
                return false;
            return (KPointF) obj == this;
        }

        public float Length { get { return (float)Math.Sqrt(X * X + Y * Y); } }
        public float LengthSquared { get { return X * X + Y * Y; } }

        public static bool operator ==(KPointF KPointF0, KPointF KPointF1)
        {
            return KPointF0.X == KPointF1.X && KPointF0.Y == KPointF1.Y;
        }

        public static bool operator !=(KPointF KPointF0, KPointF KPointF1)
        {
            return !(KPointF0 == KPointF1);
        }

        public static KPointF operator -(KPointF KPointF0)
        {
            return new KPointF(-KPointF0.X, -KPointF0.Y);
        }
        /// <summary>
        /// the negation operator
        /// </summary>
        /// <returns></returns>
        public KPointF Negate()
        {
            return -this;
        }

        /// <summary>
        ///  the addition
        /// </summary>
        /// <param name="KPointF0"></param>
        /// <param name="KPointF1"></param>
        /// <returns></returns>
        public static KPointF operator +(KPointF KPointF0, KPointF KPointF1)
        {
            return new KPointF(KPointF0.X + KPointF1.X, KPointF0.Y + KPointF1.Y);
        }
        /// <summary>
        ///  the addition
        /// </summary>
        /// <param name="KPointF0"></param>
        /// <param name="KPointF1"></param>
        /// <returns></returns>
        public static KPointF Add(KPointF KPointF0, KPointF KPointF1)
        {
            return KPointF0 + KPointF1;
        }
        /// <summary>
        /// overrides the substraction
        /// </summary>
        /// <param name="KPointF0"></param>
        /// <param name="KPointF1"></param>
        /// <returns></returns>
        public static KPointF operator -(KPointF KPointF0, KPointF KPointF1)
        {
            return new KPointF(KPointF0.X - KPointF1.X, KPointF0.Y - KPointF1.Y);
        }


        /// <summary>
        /// overrides the substraction
        /// </summary>
        /// <param name="KPointF0"></param>
        /// <param name="KPointF1"></param>
        /// <returns></returns>
        public static KPointF Subtract(KPointF KPointF0, KPointF KPointF1)
        {
            return KPointF0 - KPointF1;
        }

        /// <summary>
        /// othe internal product
        /// </summary>
        /// <param name="KPointF0"></param>
        /// <param name="KPointF1"></param>
        /// <returns></returns>
        public static float operator *(KPointF KPointF0, KPointF KPointF1)
        {
            return KPointF0.X * KPointF1.X + KPointF0.Y * KPointF1.Y;
        }


        /// <summary>
        /// cross product
        /// </summary>
        /// <param name="KPointF0"></param>
        /// <param name="KPointF1"></param>
        /// <returns></returns>
        public static float CrossProduct(KPointF KPointF0, KPointF KPointF1)
        {
            return KPointF0.X * KPointF1.Y - KPointF0.Y * KPointF1.X;
        }
        /// <summary>
        /// the multipliction by scalar in x and y
        /// </summary>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "x"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "y")]
        public static KPointF Scale(float xScale, float yScale, KPointF KPointF)
        {
            return new KPointF(xScale * KPointF.X, yScale * KPointF.Y);
        }
        /// <summary>
        /// the multipliction by scalar
        /// </summary>
        /// <param name="coefficient"></param>
        /// <param name="KPointF"></param>
        /// <returns></returns>
        public static KPointF operator *(float coefficient, KPointF KPointF)
        {
            return new KPointF(coefficient * KPointF.X, coefficient * KPointF.Y);
        }
        /// <summary>
        /// multiplication on coefficient scalar
        /// </summary>
        /// <param name="KPointF"></param>
        /// <param name="coefficient"></param>
        /// <returns></returns>
        public static KPointF operator *(KPointF KPointF, float coefficient)
        {
            return new KPointF(coefficient * KPointF.X, coefficient * KPointF.Y);
        }
        /// <summary>
        ///  multiplication on coefficient scalar
        /// </summary>
        /// <param name="coefficient"></param>
        /// <param name="KPointF"></param>
        /// <returns></returns>
        public static KPointF Multiply(float coefficient, KPointF KPointF)
        {
            return new KPointF(coefficient * KPointF.X, coefficient * KPointF.Y);
        }

        /// <summary>
        ///  multiplication on coefficient scalar
        /// </summary>
        /// <param name="KPointF"></param>
        /// <param name="coefficient"></param>
        /// <returns></returns>
        public static KPointF Multiply(KPointF KPointF, float coefficient)
        {
            return new KPointF(coefficient * KPointF.X, coefficient * KPointF.Y);
        }
        /// <summary>
        ///  division on coefficient scalar
        /// </summary>
        /// <param name="KPointF"></param>
        /// <param name="coefficient"></param>
        /// <returns></returns>
        public static KPointF operator /(KPointF KPointF, float coefficient)
        {
            return new KPointF(KPointF.X / coefficient, KPointF.Y / coefficient);
        }

        /// <summary>
        /// division on coefficient scalar
        /// </summary>
        /// <param name="coefficient"></param>
        /// <param name="KPointF"></param>
        /// <returns></returns>
        public static KPointF operator /(float coefficient, KPointF KPointF)
        {
            return new KPointF(KPointF.X / coefficient, KPointF.Y / coefficient);
        }
        /// <summary>
        /// division on coefficient scalar
        /// </summary>
        /// <param name="KPointF"></param>
        /// <param name="coefficient"></param>
        /// <returns></returns>
        public static KPointF Divide(KPointF KPointF, float coefficient)
        {
            return new KPointF(KPointF.X / coefficient, KPointF.Y / coefficient);
        }
        /// <summary>
        /// division on coefficient scalar
        /// </summary>
        /// <param name="coefficient"></param>
        /// <param name="KPointF"></param>
        /// <returns></returns>
        public static KPointF Divide(float coefficient, KPointF KPointF)
        {
            return new KPointF(KPointF.X / coefficient, KPointF.Y / coefficient);
        }

        /// <summary>
        /// returns this rotated by the angle counterclockwise; does not change "this" value
        /// </summary>
        /// <param name="angle"></param>
        /// <returns></returns>
        public KPointF Rotate(float angle)
        {
            float c = (float) Math.Cos(angle);
            float s = (float) Math.Sin(angle);

            return new KPointF(c * X - s * Y, s * X + c * Y);
        }

        public static float Dot(KPointF v1, KPointF v2)
        {
            return v1.X * v2.X + v1.Y * v2.Y;
        }

        public static float DistToLineSegment(KPointF segmentStart, KPointF segmentEnd, KPointF point)
        {
            KPointF V = segmentEnd - segmentStart;
            float Vlen = V.Length;
            if (Vlen == 0)
            {
                return (point - segmentStart).Length;
            }
            float t = KPointF.Dot(point - segmentStart, V) / (Vlen * Vlen);
            if (t < 0)
            {
                return (point - segmentStart).Length;
            }
            if (t > 1)
            {
                return (point - segmentEnd).Length;
            }

            KPointF closestPoint = segmentStart + t * V;
            return (point - closestPoint).Length;
        }

        public static float DistToLineSegment(Point segmentStart, Point segmentEnd, Point point)
        {
            return DistToLineSegment(new KPointF(segmentStart), new KPointF(segmentEnd), new KPointF(point));
        }

    }

}