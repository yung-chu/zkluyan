using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Zkly.Common;
using Zkly.Common.Delegate;
using Zkly.Common.Log;

namespace Zkly.BLL.Services
{
    public class RoadshowVideoUploader : FileUploader
    {
        public override void InitProgressInfo(long contentLength)
        {
            this.UpdateProgress("正在准备上传路演视频文件", 0, contentLength);
        }

        public override string GetFileDir(string folder)
        {
            return FileHelper.GetVideoPath(folder);
        }
    }

    public abstract class FileUploader
    {
        public UploadInfoDelegate ProgressInfo { get; set; }

        public string Guid { get; set; }

        public abstract void InitProgressInfo(long contentLength);

        public abstract string GetFileDir(string folder);

        public bool Upload(HttpPostedFileBase file, string folder, string fileName)
        {
            this.InitProgressInfo(file.ContentLength);

            var fileDir = this.GetFileDir(folder);

            return this.Upload(file.InputStream, fileDir, fileName);
        }

        public bool Upload(Stream stream, string fileDir, string fileName)
        {
            this.UpdateProgress("正在创建连接, 请稍后 ", 0, stream.Length);

            if (!Directory.Exists(fileDir))
            {
                Directory.CreateDirectory(fileDir);
            }

            FileStream fs = null;

            int length = 2048;
            byte[] buffer = new byte[length];
            int bytesRead = stream.Read(buffer, 0, length);

            this.UpdateProgress("连接创建成功 ", 0, stream.Length);

            try
            {
                var path = Path.Combine(fileDir, fileName);
                fs = new FileStream(path, FileMode.Create);

                long valueNow = 0;

                while (bytesRead > 0)
                {
                    fs.Write(buffer, 0, bytesRead);
                    bytesRead = stream.Read(buffer, 0, length);

                    this.UpdateProgress("文件正在上传 ", valueNow, stream.Length);
                    valueNow += length;
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

        protected void UpdateProgress(string text, long valueNow, long valueMax)
        {
            if (this.ProgressInfo != null)
            {
                this.ProgressInfo(text, valueNow, valueMax, this.Guid);
            }
        }
    }
}
