using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace AutoHamster.Utils
{
    public class Reflection
    {
        public static byte[] SaveObject(Object obj, string path)
        {
            if (obj == null)
                return null;

            BinaryFormatter bf = new BinaryFormatter();
            MemoryStream ms = new MemoryStream();
            bf.Serialize(ms, obj);

            System.IO.File.WriteAllBytes(path, ms.ToArray());
            return ms.ToArray();
        }
         

        public static T LoadObject<T>(string path)
        {
            var arr = System.IO.File.ReadAllBytes(path); 
            MemoryStream memStream = new MemoryStream(arr);
            BinaryFormatter binForm = new BinaryFormatter();
            Object obj = binForm.Deserialize(memStream);
            return (T)obj;
        }
    }
}
