using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Zkly.Common.Log;

namespace Zkly.Common.Utils
{
    public class UploadUtil
    {
        public static bool Upload(Stream stream, string fileDir, string fileName)
        {
            if (!Directory.Exists(fileDir))
            {
                Directory.CreateDirectory(fileDir);
            }

            var filePath = Path.Combine(fileDir, fileName);
            
            return Upload(stream, filePath);
        }

        public static bool Upload(Stream stream, string filePath)
        {
            var dir = Path.GetDirectoryName(filePath);

            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            FileStream fs = null;

            int length = 2048;
            byte[] buffer = new byte[length];
            int bytesRead = stream.Read(buffer, 0, length);

            try
            {
                fs = new FileStream(filePath, FileMode.Create);

                while (bytesRead > 0)
                {
                    fs.Write(buffer, 0, bytesRead);
                    bytesRead = stream.Read(buffer, 0, length);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex.ToString());
                throw ex;
            }
            finally
            {
                if (fs != null)
                {
                    fs.Close();
                }

                if (stream != null)
                {
                    stream.Close();
                }
            }

            return true;
        }
    }
}
