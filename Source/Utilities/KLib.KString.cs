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

    }
}