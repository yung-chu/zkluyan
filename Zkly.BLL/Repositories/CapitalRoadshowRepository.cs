using System;
using System.Collections.Generic;
using System.Data.Entity.Repository;
using System.Linq;

using Newtonsoft.Json;

using Zkly.BLL.Vhall;
using Zkly.Common.Dictionary;
using Zkly.Common.Utils;
using Zkly.DAL.Context;
using Zkly.DAL.Models;

namespace Zkly.BLL.Repositories
{
    public class CapitalRoadshowRepository : RepositoryBase<UserDbContext>
    {
        private IVhallApi vhallApi = new VhallApi();

        public CapitalRoadshow GetCapitalRoadshowById(long? id)
        {
            return FirstOrDefault<CapitalRoadshow>(c => c.Id == id, c => c.Invest);
        }

        public List<CapitalRoadshow> GetAllCapitalRoadshows()
        {
            return GetAll<CapitalRoadshow>().Where(c => c.RecordState).OrderByDescending(r => r.CreateTime).ToList();
        }

        public bool CreateRoadshow(CapitalRoadshow roadshow, bool isAdd)
        {
            var activity = new CreateActivity
            {
                Data = ConvertRoadshowToActivityData(roadshow)
            };

            var response = vhallApi.CreateActivity(activity);

            if (response.Success && !string.IsNullOrEmpty(response.Data))
            {
                var status = vhallApi.GetActivityStatus(new ActivityStatus { WebinarId = response.Data });

                if (null != roadshow.CoverFile)
                {
                    //文件上传代码
                    UploadFileInfo uploadFileInfo = new UploadFileInfoRepository().GetFileInfo(roadshow.CoverFile, (int)Enums.FolderPath.Capital);
                    if (uploadFileInfo != null)
                    {
                        roadshow.FileId = uploadFileInfo.Id;
                    }
                }

                roadshow.RecordState = true;
                roadshow.WebinarId = response.Data;
                roadshow.Status = (EActivityStatus)Enum.Parse(typeof(EActivityStatus), status.Data);
                roadshow.CreateTime = DateTime.Now;
                roadshow.UpdateTime = DateTime.Now;

                if (isAdd)
                {
                    Add(roadshow);
                }
                else
                {
                    Update(roadshow);
                }

                return true;
            }

            return false;
        }

        /// <summary>
        /// 资本路演修改
        /// </summary>
        /// <param name="roadshow">资本路演</param>
        /// <returns>true</returns>
        public bool UpdateRoadshow(CapitalRoadshow roadshow)
        {
            var activity = new UpdateActivity
            {
                WebinarId = roadshow.WebinarId,
                Data = this.ConvertRoadshowToActivityData(roadshow)
            };
            var response = vhallApi.UpdateActivity(activity);
            if (response.Success)
            {
                //IsChange为1代表需要修改文件信息，为0代表不需要修改图片信息
                if (null != roadshow.CoverFile && roadshow.IsChange == 1)
                {
                    //删除原文件
                    UploadFileInfoRepository uploadFileInfoRepositoryServer = new UploadFileInfoRepository();
                    uploadFileInfoRepositoryServer.RemoveUploadFileInfo(roadshow.FileId);

                    //上传新文件
                    UploadFileInfo uploadFileInfo = uploadFileInfoRepositoryServer.GetFileInfo(roadshow.CoverFile, (int)Enums.FolderPath.Capital);
                    if (uploadFileInfo != null)
                    {
                        roadshow.FileId = uploadFileInfo.Id;
                    }
                }

                roadshow.UpdateTime = DateTime.Now;
                Update(roadshow);
                return true;
            }

            return false;
        }

        public bool DeleteRoadshow(long? id)
        {
            var rd = FirstOrDefault<CapitalRoadshow>(c => c.Id == id);

            if (rd != null)
            {
                var res = vhallApi.DeleteActivity(new DeleteActivity
                {
                    WebinarId = rd.WebinarId
                });

                if (res.Success)
                {
                    // Delete(rd);
                    rd.RecordState = false;
                    Update(rd);
                    return true;
                }
            }

            return false;
        }

        public List<Record> GetRecords(long? id)
        {
            var rd = FirstOrDefault<CapitalRoadshow>(c => c.Id == id);

            return VhallUtil.GetRecords(rd.WebinarId);
        }

        public List<RecordPart> GetRecordParts(long? id, out string result)
        {
            var rd = FirstOrDefault<CapitalRoadshow>(c => c.Id == id);

            var data = new ActivityRecordPart
            {
                WebinarId = rd.WebinarId
            };

            var response = vhallApi.GetRecordPart(data);

            if (response.Success)
            {
                result = response.Data;
                var recordparts = JsonConvert.DeserializeObject<List<RecordPart>>(response.Data);

                return recordparts;
            }

            result = string.Empty;

            return null;
        }

        public bool GenerateRecord(long? id, MultiRecord record)
        {
            var roadshow = FirstOrDefault<CapitalRoadshow>(c => c.Id == id);

            var data = new GenerateActivityRecord
            {
                WebinarId = roadshow.WebinarId,
                Data = record
            };

            var response = vhallApi.GenerateRecord(data);

            return response.Success;
        }

        private ActivityData ConvertRoadshowToActivityData(CapitalRoadshow roadshow)
        {
            return new ActivityData
            {
                Subject = roadshow.Subject,
                TStart = DateTimeUtil.ConvertLinuxTimestamp(roadshow.StartDate).ToString(),
                TEnd = DateTimeUtil.ConvertLinuxTimestamp(roadshow.EndDate).ToString(),
                Host = roadshow.Hoster,
                WebinarDesc = roadshow.Description,
                ChannelPass = roadshow.PublicPassword
            };
        }
    }
}
