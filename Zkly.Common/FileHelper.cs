using System.Collections.Generic;
using System.Configuration;
using System.IO;

namespace Zkly.Common
{
    public class FileHelper
    {
        /// <summary>
        /// 得到完整路径
        /// </summary>
        /// <param name="path">相对路径值</param>
        /// <returns>返回完整路径</returns>
        public static string GetFilePath(string path)
        {
            return ConfigurationManager.AppSettings["UploadFolder"] + "/" + path;
        }

        /// <summary>
        /// 视频上传路径的方法主要用于为vhall路径上传
        /// </summary>
        /// <param name="path">视频路径</param>
        /// <returns>视频的完整路径</returns>
        public static string GetVideoPath(string path)
        {
            return Path.Combine(ConfigurationManager.AppSettings["UploadFolder"], Dictionary.Enums.FolderPath.Videos.ToString(), path);
        }

        /// <summary>
        /// 根据文件名称获取文件的文件类型
        /// </summary>
        /// <param name="path">文件路径或文件名</param>
        /// <returns>文件所属类型</returns>
        public static string GetFileContentType(string path)
        {
            var contentType = "image/jpeg";
            string extension = Path.GetExtension(path).ToLower().Trim('.');
            switch (extension)
            {
                case "pdf":
                    contentType = "application/pdf";
                    break;

                case "txt":
                    contentType = "text/plain";
                    break;
                case "doc":
                case "docx":
                    contentType = "application/msword";
                    break;

                case "xls":
                case "xlsx":
                    contentType = "application/x-excel";
                    break;

                case "gif":
                    contentType = "image/gif";
                    break;
                case "png":
                    contentType = "image/png";
                    break;
                default:
                    contentType = "image/jpeg";
                    break;
            }

            return contentType;
        }

        /// <summary>
        /// 判断是否为图片   
        /// </summary>
        /// <param name="path">图片路径或者文件名称</param>
        /// <returns>是否为图片</returns>
        public static bool IsImage(string path)
        {
            string extend = Path.GetExtension(path);
            if (!string.IsNullOrEmpty(extend))
            {
                extend = extend.ToLower();
                var listImg = new List<string> { ".jpeg", ".pjpeg", ".jpg", ".gif", ".png", ".x-png", ".bmp" };
                if (listImg.Contains(extend))
                {
                    return true;
                }
            }
            
            return false;
        }
      
        /// <summary>
        ///  //ie会把 jpg、jpeg翻译成image/pjpeg，png翻译成image/x-png
        /// </summary>
        /// <returns>图片类型</returns>
        public static List<string> ImageContentTypeList()
        {
            return new List<string>()
                       {
                           "image/jpeg", 
                           "image/pjpeg",
                           "image/png",
                           "image/x-png",
                           "image/bmp"
                       };
        }

        public static List<string> VideoContentTypeList()
        {
            return new List<string>()
                       {
                          "video/rm",
                          "video/rmvb",
                          "video/wmv",
                          "video/avi",
                          "video/mpg",
                          "video/mpeg",
                          "video/mp4"
                       };
        }
    }
}
