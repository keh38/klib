using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KLib
{
    public static class Utilities
    {
        public static T DeepClone<T>(T obj)
        {
            return KFile.JSONDeserializeFromString<T>(KFile.JSONSerializeToString(obj));
        }
    }
}
