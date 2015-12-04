using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zkly.Common.Utils
{
    public class IOUtil
    {        
        public static string Read(string path)
        {
            var reader = new StreamReader(path, Encoding.Default);

            var result = new StringBuilder();

            string line;

            while ((line = reader.ReadLine()) != null)
            {
                result.AppendLine(line);
            }

            return result.ToString();
        }

        public static bool Delete(string path)
        {
            if (File.Exists(path))
            {
                File.Delete(path);
                return true;
            }

            return false;
        }
    }
}
