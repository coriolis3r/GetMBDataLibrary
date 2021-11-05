using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;

namespace Keepener.Keepy.Configuration
{
    public class Json
    {
        public static T Read<T>(string filename)
        {
            var json = File.ReadAllText(filename);
            T result = JsonConvert.DeserializeObject<T>(json);
            return result;
        }
    }
}
