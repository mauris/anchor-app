using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace anchor
{
    static class DataFile
    {

        public static void Write(string path, object data)
        {
            using (Stream stream = File.Open(path, FileMode.Create))
            {
                BinaryFormatter bin = new BinaryFormatter();
                bin.Serialize(stream, data);
            }
        }

        public static object Read(string path)
        {
            object data = null;
            using (Stream stream = File.OpenRead(path))
            {
                BinaryFormatter bin = new BinaryFormatter();
                data = bin.Deserialize(stream);
            }
            return data;
        }

        public static T Read<T>(string path)
        {
            return (T)Read(path);
        }

    }
}
