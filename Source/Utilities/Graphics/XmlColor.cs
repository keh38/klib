using System.Drawing;

namespace KLib.Graphics
{
    public class XmlColor
    {
        public byte A = 0;
        public byte R = 0;
        public byte G = 0;
        public byte B = 0;

        public XmlColor() { }
        public XmlColor(Color c)
        {
            A = c.A;
            R = c.R;
            G = c.G;
            B = c.B;
        }

        public Color SystemColor
        {
            get { return Color.FromArgb(A, R, G, B); }
        }

        public bool IsEqualTo(XmlColor other)
        {
            return A == other.A && R == other.R && B == other.B && G == other.G;
        }
    }
}