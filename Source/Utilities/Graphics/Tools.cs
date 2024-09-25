using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace KLib.Graphics
{
    public class Tools
    {
        public static string CompactPath(string val, int width, Font font, TextFormatFlags flags)
        {
            string result = string.Copy(val);
            TextRenderer.MeasureText(result, font, new Size(width, 0), flags | TextFormatFlags.ModifyString);
            return result;
        }
    }
}
