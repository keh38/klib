using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace KLib
{
    public static class KString
    {
        public static string RemoveTrailingZeros(string s)
        {
            int dot = s.IndexOf('.');
            if (dot < 0)
            {
                return s;
            }

            int firstKeep = dot - 1;
            for (int k = s.Length - 1; k > dot; k--)
            {
                if (s[k] != '0')
                {
                    firstKeep = k;
                    break;
                }
            }

            return s.Substring(0, firstKeep + 1);
        }

        public static string InsertSpacesAtCaseChanges(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return "";
            StringBuilder newText = new StringBuilder(text.Length * 2);
            newText.Append(text[0]);
            for (int i = 1; i < text.Length; i++)
            {
                if (char.IsUpper(text[i]) && text[i - 1] != ' ')
                    newText.Append(' ');
                newText.Append(text[i]);
            }
            return newText.ToString();
        }
    }
}