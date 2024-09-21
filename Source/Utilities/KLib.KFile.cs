using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

using Newtonsoft.Json;
using ProtoBuf;

namespace KLib
{
    public static class KFile
    {
        public static void SaveToXML<T>(T obj, string path)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            using (Stream s = File.Create(path))
            {
                serializer.Serialize(s, obj);
            }
        }

        public static void XmlSerialize<T>(T obj, string path)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            using (Stream s = File.Create(path))
            {
                serializer.Serialize(s, obj);
            }
        }

        public static T RestoreFromXML<T>(string path) where T : new()
        {
            T obj = new T();
            if (File.Exists(path))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(T));
                using (Stream s = File.OpenRead(path))
                {
                    obj = (T)serializer.Deserialize(s);
                }
            }
            return obj;
        }
        public static T XmlDeserialize<T>(string path) where T : new()
        {
            T obj = new T();
            if (File.Exists(path))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(T));
                using (Stream s = File.OpenRead(path))
                {
                    obj = (T)serializer.Deserialize(s);
                }
            }
            return obj;
        }

        public static string ToXMLString<T>(T obj)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));

            using (StringWriter s = new StringWriter())
            {
                serializer.Serialize(s, obj);
                return s.ToString();
            }
        }

        public static T FromXMLString<T>(string xml)
        {
            T obj = default(T);
            XmlSerializer serializer = new XmlSerializer(typeof(T));

            using (StringReader s = new StringReader(xml))
            {
                obj = (T)serializer.Deserialize(s);
            }

            return obj;
        }

        public static T RestoreFromJson<T>(string path) where T : new()
        {
            T obj = new T();
            if (File.Exists(path))
            {
                obj = JsonConvert.DeserializeObject<T>(File.ReadAllText(path));
            }
            return obj;
        }

        public static void SaveToJson<T>(T obj, string path)
        {
            using (Stream s = File.Create(path))
            {
                byte[] b = Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(obj));
                s.Write(b, 0, b.Length);
            }
        }

        public static string ExtractJsonObjectString(string json, string name, string next)
        {
            string objIndicator = ",\"" + name + "\":";
            int objStart = json.IndexOf(objIndicator);
            if (objStart < 0)
                throw new Exception("Json object not found: " + name);

            objStart += objIndicator.Length;

            int objEnd = json.Length - 1;
            if (!string.IsNullOrEmpty(next))
            {
            }

            return json.Substring(objStart, objEnd - objStart);
        }

        public static T ExtractJsonObject<T>(string json, string name, string next)
        {
            string objString = ExtractJsonObjectString(json, name, next);
            return JsonConvert.DeserializeObject<T>(objString);
        }

        public static void AppendTextFile(string path, string text)
        {
            using (Stream s = File.Open(path, FileMode.Append))
            {
                byte[] b = System.Text.Encoding.ASCII.GetBytes(text);
                s.Write(b, 0, b.Length);
            }
        }

        public static byte[] ToProtoBuf<T>(T obj)
        {
            byte[] pbuf;
            using (var ms = new System.IO.MemoryStream())
            {
                Serializer.Serialize<T>(ms, obj);
                pbuf = ms.ToArray();
            }
            return pbuf;
        }

        public static T FromProtoBuf<T>(byte[] pbuf)
        {
            T obj = default(T);
            using (var ms = new System.IO.MemoryStream(pbuf))
            {
                obj = Serializer.Deserialize<T>(ms);
            }

            return obj;
        }

    }
}
