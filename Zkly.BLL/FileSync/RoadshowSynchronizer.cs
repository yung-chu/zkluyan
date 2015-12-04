using System;
using System.IO;
using Zkly.BLL.Repositories;
using Zkly.BLL.Services;
using Zkly.Common;
using Zkly.Common.Log;
using Zkly.Common.Utils;

namespace Zkly.BLL.FileSync
{
    public class RoadshowSynchronizer
    {
        private readonly RoadshowWatcherRepository repository = new RoadshowWatcherRepository();
        private readonly FtpClient ftpClient = new FtpClient();

        public void Init()
        {
            repository.ResetStatus();
        }

        public void Sync()
        {
            Logger.Info("=========================上传开始于： " + DateTime.Now + "============================");

            var list = repository.GetUnuploadedRoadshows(1);

            Logger.Info(string.Format("=> 共找到 {0} 个文件需要上传", list.Count));

            foreach (var rd in list)
            {
                var path = FileHelper.GetVideoPath(rd.Folder);
                var filePath = Path.Combine(path, rd.FileName);

                FileStream stream = null;

                if (File.Exists(filePath))
                {
                    try
                    {
                        stream = new FileStream(filePath, FileMode.Open);
                        repository.SetProcessingStatus(rd);
                        var success = ftpClient.Upload(stream, stream.Length, rd.Folder, rd.FileName);

                        if (success)
                        {
                            var us = repository.SetFinishStatus(rd);
                            if (us)
                            {
                                IOUtil.Delete(filePath);
                            }

                            Logger.Info("=> " + rd.FileName + " 上传成功！");
                        }
                        else
                        {
                            Logger.Info("=> " + rd.FileName + " 上传失败！");
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.Info("=> 失败原因：" + ex.ToString());
                        Logger.Error(ex.ToString());
                    }
                    finally
                    {
                        if (stream != null)
                        {
                            stream.Close();
                        }
                    }

                    Logger.Info("==============================上传结束================================");
                }
                else
                {
                    Logger.Info(string.Format("=> 失败原因：文件不存在,路径：{0}", filePath));
                    repository.SetErrorStatus(rd);
                }
            }
        }
    }
}
