using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Web.Caching;
using System.Web.Mvc;

using Microsoft.AspNet.Identity;

using PagedList;

using Zkly.BLL.Message;
using Zkly.BLL.Repositories;
using Zkly.BLL.Vhall;
using Zkly.BLL.ViewModels;
using Zkly.Common;
using Zkly.Common.Extension;
using Zkly.Common.Log;
using Zkly.Common.Utils;
using Zkly.DAL.Models;

namespace Zkly.Web.Controllers
{
    public class RoadshowController : BaseController
    {
        #region lasy loading repository
        private const int PageSize = 12;
        private readonly IVhallApi vhallApi = new VhallApi();
        private RoadshowRepository roadshowRepository = new RoadshowRepository();
        private CapitalRoadshowRepository capitalRoadshowRepository = new CapitalRoadshowRepository();

        public RoadshowRepository RoadshowRepository
        {
            get { return this.roadshowRepository ?? (this.roadshowRepository = new RoadshowRepository()); }
        }

        public CapitalRoadshowRepository CapitalRoadshowRepository
        {
            get { return this.capitalRoadshowRepository ?? (this.capitalRoadshowRepository = new CapitalRoadshowRepository()); }
        }
        #endregion

        #region 路演厅

        #region 业务路演

        [Route("business-roadshow")]
        public ActionResult Business(int? page, string status)
        {
            status = status ?? "ongoing";
            ViewBag.Status = status;
            int pageNumber = page ?? 1;

            var roadshows = RoadshowRepository.GetRoadshowsByStatus(status);

            return View(roadshows.ToPagedList(pageNumber, PageSize));
        }

        [Route("business-roadshow/{id}")]
        public ActionResult BusinessRoadshow(int? id)
        {
            var model = RoadshowRepository.GetRoadshowById(id);

            return View("BusinessRoadshow", model);
        }

        //0:参与成功; 1:不是投资人; 2:参与失败;
        [HttpPost]
        public JsonResult Attend(Roadshow rd)
        {
            var userId = User.Identity.GetUserId();
            var isInvester = UserManager.IsInvestor(userId);
            var result = 0;

            try
            {
                if (!isInvester)
                {
                    result = 1;
                }
                else
                {
                    var invest = RoadshowRepository.GetInvestById(rd.Id);

                    if (invest != null && invest.Roadshow != null)
                    {
                        var body = new StringBuilder();
                        body.Append("投资人：").AppendLine(User.Identity.Name);
                        body.Append("邮箱：").AppendLine(UserManager.GetEmail(userId));
                        body.Append("联系电话：").AppendLine(UserManager.GetPhoneNumber(userId));
                        body.Append("感兴趣的公司：").AppendLine(invest.FirstAuditInfo.CompanyName);
                        body.Append("项目名：").AppendLine(invest.FirstAuditInfo.ProjectName);

                        var message = new IdentityMessage
                        {
                            Destination = ConfigurationManager.AppSettings["ZkjlEmail"],
                            Subject = "投资人申请参加资本路演",
                            Body = body.ToString()
                        };

                        var emailService = new EmailService();
                        emailService.Send(message);
                    }
                }
            }
            catch (Exception e)
            {
                Logger.Error(e.ToString());
                result = 2;
            }

            return new JsonResult { Data = result };
        }

        #endregion

        #region 资本路演

        public ActionResult Capital(int? page)
        {
            var capitals = CapitalRoadshowRepository.GetAllCapitalRoadshows();
            var pagingData = capitals.ToPagedList(page ?? 1, PageSize);

            foreach (var data in pagingData)
            {
                var status = this.vhallApi.GetActivityStatus(new ActivityStatus { WebinarId = data.WebinarId });
                data.Status = (EActivityStatus)Enum.Parse(typeof(EActivityStatus), status.Data);
            }

            return View(pagingData);
        }

        [Route("capital-roadshow/{id}")]
        public ActionResult CapitalRoadshow(int? id)
        {
            var model = CapitalRoadshowRepository.GetCapitalRoadshowById(id);
            var status = this.vhallApi.GetActivityStatus(new ActivityStatus { WebinarId = model.WebinarId });
            model.Status = EnumUtil.Parse<EActivityStatus>(status.Data);
            ViewBag.Email = UserManager.GetEmail(User.Identity.GetUserId());
            ViewBag.Name = User.Identity.Name;
            ViewBag.IsEnterprise = UserManager.IsEnterprise(User.Identity.GetUserId());

            return View("CapitalRoadshow", model);
        }

        public ActionResult LikeThisCapitalRoadshow(CapitalRoadshow capital)
        {
            var cr = CapitalRoadshowRepository.GetCapitalRoadshowById(capital.Id);

            if (cr != null)
            {
                var body = new StringBuilder();
                body.Append("投资人：").AppendLine(User.Identity.Name);
                body.Append("投资人Email：").AppendLine(UserManager.GetEmail(User.Identity.GetUserId()));
                body.Append("联系电话：").AppendLine(UserManager.GetPhoneNumber(User.Identity.GetUserId()));

                var msg = new IdentityMessage
                {
                    Destination = ConfigurationManager.AppSettings["ZkjlEmail"],
                    Subject = string.Format("投资人对公司：（{0}） 感兴趣", cr.CompanyName),
                    Body = body.ToString(),
                };

                UserManager.EmailService.Send(msg);
                return new JsonResult { Data = true };
            }

            return new JsonResult { Data = false };
        }
        #endregion

        #endregion

        #region 企业 - 业务路演的上传，修改

        /// <summary>
        /// 上传路演首页面，容许企业上传自己的也业务路演
        /// </summary>
        [Authorize]
        [Route("bussiness-roadshow/{id}")]
        public ActionResult RoadshowIndex(long id)
        {
            var invest = RoadshowRepository.GetInvestById(id);
            if (invest == null || invest.UserId != UserId)
            {
                ViewBag.Message = "无效的投资申请记录！";
                return View("Error");
            }

            var model = new RoadshowViewModel { Guid = Guid.NewGuid().ToString(), Id = id };
            if (invest.Roadshow != null)
            {
                model.VideoName = invest.Roadshow.VideoName;
                model.VideoDescrition = invest.Roadshow.VideoDescrition;
                model.CoverFileId = invest.Roadshow.CoverFileId;
                model.RoadshowAddress = invest.Roadshow.VhallRoadshowAddress;
            }

            return View("UploadIndex", model);
        }

        [HttpPost]
        public JsonResult Upload(RoadshowViewModel model)
        {
            var message = string.Empty;
            var result = false;

            try
            {
                var getError = CheckUploadData(model);
                if (getError.Count>0)
                {
                    return new JsonResult { Data = new { status = result, message = getError.Join(",") } };
                }
                else
                {
                    var temproadshow = new RoadshowRepository().GetRoadshowById(model.Id);

                    RoadshowRepository.UploadInfo = this.SetUploadToCache;
                    if (temproadshow == null)
                    {
                        result = RoadshowRepository.AddRoadshow(model);
                    }
                    else
                    {
                        result = RoadshowRepository.EditRoadshow(model);
                    }

                    return new JsonResult { Data = new { status = result, message ="数据保存成功！" } };
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex.ToString());
                message = ex.ToString();
            }

            return new JsonResult { Data = new { status = false, message = message } };
        }

        public IList<string> CheckUploadData(RoadshowViewModel model)
        {
            List<string> list=new List<string>();
            var contentLength = Convert.ToDouble(model.VideoFile.ContentLength);
            var limitedUploadCapacity = 500;
         
            if (model.VideoFile==null)
            {
                list.Add("上传视频文件不能为空!");
            }
            else
            {
                if (!FileHelper.VideoContentTypeList().Contains(model.VideoFile.ContentType.ToLower()))
                {
                    list.Add("上传视频文件格式不正确!");
                }
            }

            if (string.IsNullOrEmpty(model.VideoName))
            {
                list.Add("视频名称不能为空!");
            }

            if (string.IsNullOrEmpty(model.VideoDescrition))
            {
                list.Add("视频简介不能为空!");
            }

            if (model.CoverFile==null)
            {
                list.Add("活动封面不能为空!");
            }

            if (contentLength / 1024d / 1024d > limitedUploadCapacity)
            {
                 list.Add("视频文件大于500M!"); 
            }

            return list;
        }

        [ActionName("upload-success")]
        public ActionResult UploadSuccess()
        {
            ViewBag.Message = "完成，路演已成功提交。";
            return View("Success_Info", new ResultViewModel { ActionName = "index", ControllerName = "enterprise" });
        }

        public void SetUploadToCache(string text, long valueNow, long valueMax, string guid)
        {
            var cachedItem = HttpContext.Cache[guid];
            if (cachedItem != null)
            {
                HttpContext.Cache.Remove(guid);
            }

            HttpContext.Cache.Add(
                guid,
                new { text = text, valueNow = valueNow, valueMax = valueMax },
                null,
                    Cache.NoAbsoluteExpiration,
                    new TimeSpan(0, 20, 0),
                    CacheItemPriority.Default,
                    null);
        }

        [HttpPost]
        public ActionResult UploadStatus(RoadshowViewModel model)
        {
            if (string.IsNullOrEmpty(model.Guid))
            {
                return new JsonResult { Data = null };
            }

            var info = HttpContext.Cache.Get(model.Guid);

            return new JsonResult { Data = info };
        }

        [HttpPost]
        public ActionResult RemoveCache(RoadshowViewModel model)
        {
            if (!string.IsNullOrEmpty(model.Guid))
            {
                HttpContext.Cache.Remove(model.Guid);
            }

            return new JsonResult { Data = true };
        }

        [Route("bussiness-roadshow-edit/{id}")]
        public ActionResult Edit(int? id)
        {
            var r = RoadshowRepository.GetRoadshowById(id);

            var viewModel = new RoadshowViewModel
            {
                VideoName = r.VideoName,
                VideoDescrition = r.VideoDescrition,
                Id = r.Id,
                Guid = Guid.NewGuid().ToString(),
                CoverFileId = r.CoverFileId
            };

            return View("UploadIndex", viewModel);
        }

        #endregion

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (this.roadshowRepository != null)
                {
                    this.roadshowRepository.Dispose();
                    this.roadshowRepository = null;
                }

                if (this.capitalRoadshowRepository != null)
                {
                    this.capitalRoadshowRepository.Dispose();
                    this.capitalRoadshowRepository = null;
                }
            }

            base.Dispose(disposing);
        }
    }
}