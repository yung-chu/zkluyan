using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Data.Entity.Repository;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Zkly.BLL.Services;
using Zkly.BLL.ViewModels;
using Zkly.Common;
using Zkly.Common.Config;
using Zkly.Common.Delegate;
using Zkly.Common.Dictionary;
using Zkly.Common.Extension;
using Zkly.Common.Log;
using Zkly.Common.Utils;
using Zkly.DAL.Context;
using Zkly.DAL.Models;

namespace Zkly.BLL.Repositories
{
    public class RoadshowRepository : RepositoryBase<UserDbContext>
    {
        public Invest GetInvestById(long id)
        {
            return FirstOrDefault<Invest>(m => m.Id == id, m => m.FirstAuditInfo, m => m.Roadshow);
        }

        public UploadInfoDelegate UploadInfo { get; set; }

        public bool AddRoadshow(RoadshowViewModel roadshowViewModel)
        {
            var folder = DateTime.Now.ToString("yyyyMMdd");
            var ticks = DateTime.Now.Ticks;

            var nameTemplate = "zkly_{0}{1}";

            var videoFileName = string.Format(nameTemplate, ticks, roadshowViewModel.VideoFile.GetFileSuffix());
            var coverFileName = string.Format(nameTemplate, ticks, roadshowViewModel.CoverFile.GetFileSuffix());

            var uploadSuccess = UploadVideoFile(roadshowViewModel.VideoFile, videoFileName, folder, roadshowViewModel.Guid);
            var imgFileId = UploadImageFile(roadshowViewModel.CoverFile);

            if (uploadSuccess && imgFileId > 0)
            {
                var roadshow = new Roadshow
                {
                    Id = roadshowViewModel.Id,
                    VideoName = roadshowViewModel.VideoName,
                    VideoDescrition = roadshowViewModel.VideoDescrition,
                    VideoFileName = videoFileName,
                    Folder = folder,
                    CoverFileId = imgFileId,
                    SubmitDate = DateTime.Now,
                    UpdateDate = DateTime.Now
                };

                Add(roadshow);

                this.AddRoadshowUploadWatcher(folder, videoFileName);

                return true;
            }

            return false;
        }

        public bool EditRoadshow(RoadshowViewModel roadshowViewModel)
        {
            try
            {
                var roadshow = this.GetRoadshowById(roadshowViewModel.Id);

                if (roadshowViewModel.VideoFile != null)
                {
                    this.DeleteVideoFile(roadshow);

                    var folder = DateTime.Now.ToString("yyyyMMdd");
                    var ticks = DateTime.Now.Ticks;

                    var nameTemplate = "zkly_{0}{1}";

                    var videoFileName = string.Format(nameTemplate, ticks, roadshowViewModel.VideoFile.GetFileSuffix());

                    roadshow.VideoFileName = videoFileName;
                    roadshow.Folder = folder;
                }

                if (roadshowViewModel.CoverFile != null)
                {
                    this.DeleteCoverFile(roadshow);
                }

                //视频上传
                this.UploadVideoFile(roadshowViewModel.VideoFile, roadshow.VideoFileName, roadshow.Folder, roadshowViewModel.Guid);

                //图片上传
                var fileid = UploadImageFile(roadshowViewModel.CoverFile);
                if (fileid != 0)
                {
                    roadshow.CoverFileId = fileid;
                }

                roadshow.UpdateDate = DateTime.Now;
                roadshow.VideoName = roadshowViewModel.VideoName;
                roadshow.VideoDescrition = roadshowViewModel.VideoDescrition;

                Update(roadshow);

                if (roadshowViewModel.VideoFile != null)
                {
                    this.AddRoadshowUploadWatcher(roadshow.Folder, roadshow.VideoFileName);
                }

                return true;
            }
            catch (Exception ex)
            {
                Logger.Error(ex.ToString());
            }

            return false;
        }

        public List<Roadshow> GetAllRoadshows()
        {
            return GetAll<Roadshow>().ToList();
        }

        public List<Roadshow> GetRoadshowsByStatus(string status)
        {
            if (string.IsNullOrEmpty(status))
            {
                return GetAll<Roadshow>().ToList();
            }

            var roadshows = status == "done" ?
                Find<Roadshow>(r => r.Invest.Status == EInvestAuditStatus.Accepted) :
                Find<Roadshow>(r => r.Invest.Status != EInvestAuditStatus.Accepted);

            return roadshows.OrderByDescending(r => r.Priority).ToList();
        }

        public List<Roadshow> GetPagingRoadshows(int page, int pageSize)
        {
            return GetAll<Roadshow>().ToList().GetRange((page - 1) * pageSize, pageSize);
        }

        public Roadshow GetRoadshowById(long? id)
        {
            var roadshow = FirstOrDefault<Roadshow>(r => r.Id == id, r => r.Invest);

            if (roadshow == null)
            {
                return null;
            }

            var folder = roadshow.Folder;
            var fileName = roadshow.VideoFileName;

            roadshow.VhallRoadshowAddress = this.GetRoadshowAddress(folder, fileName);

            return roadshow;
        }

        public bool SaveRoadshowPriority(int id, int priority)
        {
            var rd = FirstOrDefault<Roadshow>(i => i.Id == id);

            if (rd != null)
            {
                rd.Priority = priority;
            }

            Update(rd);

            return true;
        }

        private bool DeleteVideoFile(Roadshow roadshow)
        {
            var ftpFile = string.Format("{0}/{1}", roadshow.Folder, roadshow.VideoFileName);
            var ftpClient = new FtpClient();
            return ftpClient.Delete(ftpFile);
        }

        private bool DeleteCoverFile(Roadshow roadshow)
        {
            return new UploadFileInfoRepository().RemoveUploadFileInfo(roadshow.CoverFileId);
        }

        private void AddRoadshowUploadWatcher(string folder, string videoFileName)
        {
            var roadshowUploadWatcher = new RoadshowUploadWatcher
            {
                Folder = folder,
                FileName = videoFileName,
                ServerName = AppSettings.MachineName,
                SyncStatus = 0,
                UpdateTime = DateTime.Now
            };

            Add(roadshowUploadWatcher);
        }

        //视频文件上传
        private bool UploadVideoFile(HttpPostedFileBase videoFile, string videoFileName, string folder, string guid)
        {
            bool videoSuccess = true;

            if (videoFile != null)
            {
                var videoUploader = new RoadshowVideoUploader { Guid = guid, ProgressInfo = this.UploadInfo };
                videoSuccess = videoUploader.Upload(videoFile, folder, videoFileName);
            }

            return videoSuccess;
        }

        //图片文件上传
        private long UploadImageFile(HttpPostedFileBase coverPostFile)
        {
            if (coverPostFile != null)
            {
                UploadFileInfo uploadFileInfo = new UploadFileInfoRepository().GetFileInfo(coverPostFile, (int)Enums.FolderPath.Business);
                if (uploadFileInfo != null)
                {
                    return uploadFileInfo.Id;
                }
            }

            return 0;
        }

        private string GetRoadshowAddress(string folder, string fileName)
        {
            var localFileName = string.Format("vhall_{0}.txt", folder);

            var index = GetMetaIndexFromLocal(localFileName, fileName);

            return index == null ? null : index.VhallShowAddress;
        }

        private MetaIndex GetMetaIndexFromLocal(string localFileName, string videoName)
        {
            var ftpFile = string.Format("meta/{0}", localFileName);

            using (var metaRepository = new MetaIndexRepository())
            {
                var index = metaRepository.GetMetaIndex(localFileName, videoName);
                if (index == null)
                {
                    var ftpClient = new FtpClient();
                    var content = ftpClient.DownloadContent(ftpFile);
                    Logger.Info("从Vhall获取到的Meta：" + content);
                    metaRepository.AddIndex(localFileName, content);

                    index = metaRepository.GetMetaIndex(localFileName, videoName);
                }

                return index;
            }
        }
    }
}
