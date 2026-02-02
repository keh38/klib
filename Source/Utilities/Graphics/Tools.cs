using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace KLib.KGraphics
{
    public class Tools
    {
        public static string CompactPath(string val, int width, Font font, TextFormatFlags flags)
        {
            string result = string.Copy(val);
            TextRenderer.MeasureText(result, font, new Size(width, 0), flags | TextFormatFlags.ModifyString);
            return result;
        }

        private void CompactPath(ToolStripStatusLabel pathLabel, string filePath)
        {
            string compactPath = filePath;
            string filename = Path.GetFileName(filePath);
            string folder = Path.GetDirectoryName(filePath) ?? "";

            var sub = folder.Split(Path.DirectorySeparatorChar);

            int lastSubIndex = sub.Length - 2;
            int firstRemovedIndex = lastSubIndex;

            using (Graphics g = pathLabel.GetCurrentParent().CreateGraphics())
            {
                while (true)
                {
                    var size = TextRenderer.MeasureText(g, compactPath, pathLabel.Font);

                    if (size.Width <= pathLabel.Width)
                    {
                        break;
                    }

                    string toRemove = "";
                    for (int k = firstRemovedIndex; k <= lastSubIndex; k++)
                    {
                        toRemove += sub[k] + Path.DirectorySeparatorChar;
                    }

                    compactPath = filePath.Replace(toRemove, "..." + Path.DirectorySeparatorChar);

                    if (firstRemovedIndex > 1)
                    {
                        firstRemovedIndex--;
                    }
                    else if (lastSubIndex < sub.Length - 1)
                    {
                        lastSubIndex++;
                    }
                    else
                    {
                        break;
                    }

                }
            }
            pathLabel.Text = compactPath;
        }


    }
}
