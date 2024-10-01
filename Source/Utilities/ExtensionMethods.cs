using System;
using System.Collections.Generic;

namespace KLib.ExtensionMethods
{
    public static class MyExtensions
    {
        public static string FirstLetterToUpperCase(this string s)
        {
            if (string.IsNullOrEmpty(s))
                throw new ArgumentException("There is no first letter");

            char[] a = s.ToCharArray();
            a[0] = char.ToUpper(a[0]);
            return new string(a);
        }

        public static bool ContainsChar(this string s, char c)
        {
            return s.Contains(c.ToString());
        }

        public static string ToVectorString(this float[] vals)
        {
            if (vals.Length == 1)
            {
                return vals[0].ToString();
            }

            string s = "[";
            foreach (var v in vals)
            {
                s += " " + v.ToString();
            }
            s += " ]";

            return s;
        }

        public static double[] ToDouble(this float[] vals)
        {
            double[] d = new double[vals.Length];
            for (int k = 0; k < vals.Length; k++) d[k] = (double)vals[k];
            return d;
        }

        public static float[] FromVectorString(this string s)
        {
            int istart = s.IndexOf('[');
            int iend = s.IndexOf(']');
            if (istart < 0 || iend < 0)
                throw new System.Exception("Vector string must start with '[' and end with ']'");

            var valuesStr = s.Substring(istart + 1, iend - istart - 1).Trim();
            var items = valuesStr.Split(new char[] { ';', ' ' }, StringSplitOptions.RemoveEmptyEntries);

            float[] values = new float[items.Length];
            for (int k = 0; k < items.Length; k++) values[k] = float.Parse(items[k]);

            return values;
        }
    }
}