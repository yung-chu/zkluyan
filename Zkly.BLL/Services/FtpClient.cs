using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Zkly.Common.Config;
using Zkly.Common.Delegate;
using Zkly.Common.Log;
using Zkly.Common.Utils;

namespace Zkly.BLL.Services
{
    public class FtpClient
    {
        private string ftpServer = VhallSettings.FtpServer;

        /// <summary>
        /// FTP上传文件
        /// </summary>
        /// <param name="stream">上传文件的文件流</param>
        /// <param name="contentLength">上传文件的大小</param>
        /// <param name="folder">服务器上的文件夹，例如：20150321</param>
        /// <param name="fileName">要保存到FTP服务器的文件名</param>
        /// <returns>true</returns> 
        public bool Upload(Stream stream, long contentLength, string folder, string fileName)
        {
            if (!DirectoryExists(folder))
            {
                CreateDirectory(folder);
            }

            string uri = string.Format("{0}/{1}/{2}", ftpServer, folder, fileName);

            FtpWebRequest request = CreateFtpRequest(uri, WebRequestMethods.Ftp.UploadFile);
            request.UsePassive = true;
            request.ContentLength = contentLength;

            int buffLength = 8192;
            byte[] buff = new byte[buffLength];
            int contentLen;

            Stream requestStream = null;

            try
            {
                requestStream = request.GetRequestStream();

                contentLen = stream.Read(buff, 0, buffLength);

                while (contentLen != 0)
                {
                    requestStream.Write(buff, 0, contentLen);
                    contentLen = stream.Read(buff, 0, buffLength);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex.ToString());
                throw ex;
            }
            finally
            {
                if (requestStream != null)
                {
                    requestStream.Close();
                }

                if (stream != null)
                {
                    stream.Close();
                }
            }

            return true;
        }

        /// <summary>
        /// 从FTP服务器下载文件
        /// </summary>
        /// <param name="ftpFile">FTP服务器上的文件，除去FTP server部分</param>
        /// <param name="localFile">本地保存下载文件的路径及文件名</param>
        /// <returns>success</returns>
        public bool Download(string ftpFile, string localFile)
        {
            bool success = false;
            string uri = string.Format("{0}/{1}", ftpServer, ftpFile);

            FtpWebRequest request = CreateFtpRequest(uri, WebRequestMethods.Ftp.DownloadFile);

            FtpWebResponse response = null;
            Stream responseStream = null;
            FileStream outputStream = null;
            try
            {
                outputStream = new FileStream(localFile, FileMode.OpenOrCreate);

                response = (FtpWebResponse)request.GetResponse();
                responseStream = response.GetResponseStream();
                long contentLength = response.ContentLength;
                int bufferSize = 2048;
                byte[] buffer = new byte[bufferSize];
                int readCount;
                readCount = responseStream.Read(buffer, 0, bufferSize);
                while (readCount > 0)
                {
                    outputStream.Write(buffer, 0, readCount);
                    readCount = responseStream.Read(buffer, 0, bufferSize);
                }

                success = true;
            }
            catch (Exception ex)
            {
                Logger.Error(ex.ToString());
                success = false;
            }
            finally
            {
                if (outputStream != null)
                {
                    outputStream.Close();
                }

                if (responseStream != null)
                {
                    responseStream.Close();
                }

                if (response != null)
                {
                    response.Close();
                }
            }

            return success;
        }

        public string DownloadContent(string ftpFile)
        {
            var sb = new StringBuilder();

            var uri = string.Format("{0}/{1}", ftpServer, ftpFile);

            FtpWebRequest request = CreateFtpRequest(uri, WebRequestMethods.Ftp.DownloadFile);

            FtpWebResponse response = null;
            Stream responseStream = null;

            try
            {
                response = (FtpWebResponse)request.GetResponse();
                responseStream = response.GetResponseStream();

                var reader = new StreamReader(responseStream, System.Text.Encoding.Default);

                var s = reader.ReadLine();
                while (!string.IsNullOrEmpty(s))
                {
                    sb.AppendLine(s);
                    s = reader.ReadLine();
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex.ToString());
                return string.Empty;
            }
            finally
            {
                if (responseStream != null)
                {
                    responseStream.Close();
                }

                if (response != null)
                {
                    response.Close();
                }
            }

            return sb.ToString();
        }

        /// <summary>
        /// 删除FTP服务器上的文件
        /// </summary>
        /// <param name="ftpFile">FTP服务器上文件的文件名</param>
        /// <returns> success</returns>
        public bool Delete(string ftpFile)
        {
            bool success = false;
            string uri = string.Format("{0}/{1}", ftpServer, ftpFile);
            FtpWebRequest request = CreateFtpRequest(uri, WebRequestMethods.Ftp.DeleteFile);
            FtpWebResponse response = null;
            Stream responseStream = null;
            StreamReader streamReader = null;
            try
            {
                response = (FtpWebResponse)request.GetResponse();
                long size = response.ContentLength;
                responseStream = response.GetResponseStream();
                streamReader = new StreamReader(responseStream);
                string result = streamReader.ReadToEnd();
                success = true;
            }
            catch (Exception ex)
            {
                Logger.Error(ex.ToString());
                success = false;
            }
            finally
            {
                if (streamReader != null)
                {
                    streamReader.Close();
                }

                if (responseStream != null)
                {
                    responseStream.Close();
                }

                if (response != null)
                {
                    response.Close();
                }
            }

            return success;
        }

        public void CreateDirectory(string folder)
        {
            FtpWebResponse response = null;

            try
            {
                string uri = string.Format("{0}/{1}", ftpServer, folder);

                FtpWebRequest request = CreateFtpRequest(uri, WebRequestMethods.Ftp.MakeDirectory);

                response = (FtpWebResponse)request.GetResponse();
            }
            catch (Exception ex)
            {
                Logger.Error(ex.ToString());
            }
            finally
            {
                if (response != null)
                {
                    response.Close();
                }
            }
        }

        public List<string> GetDirectList(string uri)
        {
            var list = new List<string>();

            WebResponse response = null;
            StreamReader reader = null;
            try
            {
                FtpWebRequest request = CreateFtpRequest(uri, WebRequestMethods.Ftp.ListDirectory);

                response = request.GetResponse();
                reader = new StreamReader(response.GetResponseStream());
                string line = reader.ReadLine();
                while (line != null)
                {
                    list.Add(line);
                    line = reader.ReadLine();
                }

                return list;
            }
            catch (Exception ex)
            {
                Logger.Error(ex.ToString());
                return list;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }

                if (response != null)
                {
                    response.Close();
                }
            }
        }

        public bool DirectoryExists(string directory)
        {
            var directories = GetDirectList(ftpServer);

            return directories.Any(d => d == directory);
        }

        private FtpWebRequest CreateFtpRequest(string uri, string method)
        {
            FtpWebRequest request = (FtpWebRequest)FtpWebRequest.Create(new Uri(uri));
            request.KeepAlive = false;
            request.Method = method;
            request.UseBinary = true;
            request.Proxy = null; //disable proxy 
            request.Credentials = new NetworkCredential(VhallSettings.FtpUserName, VhallSettings.FtpPassword);

            return request;
        }
    }
}
