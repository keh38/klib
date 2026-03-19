using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace KLib.Drawing.Graphics
{
    public class Tools
    {
        public void CompactPath(ToolStripStatusLabel pathLabel, string filePath)
        {
            using (System.Drawing.Graphics g = pathLabel.GetCurrentParent().CreateGraphics())
            {
                bool Fits(string s) => TextRenderer.MeasureText(g, s, pathLabel.Font).Width <= pathLabel.Width;

                // Already fits — done
                if (Fits(filePath))
                {
                    pathLabel.Text = filePath;
                    return;
                }

                string folder = Path.GetDirectoryName(filePath) ?? "";
                string filename = Path.GetFileName(filePath);

                // No folder to truncate — show filename only
                if (string.IsNullOrEmpty(folder))
                {
                    pathLabel.Text = filename;
                    return;
                }

                // Progressively replace folder segments with ...
                var segments = folder.Split(Path.DirectorySeparatorChar);
                int lo = segments.Length - 1;
                int hi = lo;

                while (lo >= 0)
                {
                    var kept = segments.Take(lo)
                                       .Concat(new[] { "..." })
                                       .Concat(segments.Skip(hi + 1));

                    string compactFolder = string.Join(Path.DirectorySeparatorChar.ToString(), kept);
                    string candidate = Path.Combine(compactFolder, filename);

                    if (Fits(candidate))
                    {
                        pathLabel.Text = candidate;
                        return;
                    }

                    if (lo > 0)
                        lo--;
                    else
                        hi++;

                    // Eaten everything — just show filename
                    if (hi >= segments.Length)
                    {
                        pathLabel.Text = filename;
                        return;
                    }
                }

                pathLabel.Text = filename;
            }
        }

        public static string CompactPath(string val, int width, Font font, TextFormatFlags flags)
        {
            string result = string.Copy(val);
            TextRenderer.MeasureText(result, font, new Size(width, 0), flags | TextFormatFlags.ModifyString);
            return result;
        }

    }
}
