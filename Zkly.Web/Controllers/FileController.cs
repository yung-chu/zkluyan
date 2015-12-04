using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Microsoft.Ajax.Utilities;

using Zkly.BLL.Repositories;
using Zkly.Common;
using Zkly.Common.Dictionary;
using Zkly.Common.Extension;
using Zkly.Common.Log;
using Zkly.Common.Mvc.UI;
using Zkly.DAL.Models;

namespace Zkly.Web.Controllers
{
    public class FileController : Controller
    {
        /// <summary>
        /// 通过传图片表中的路径进行图片显示
        /// </summary>
        /// <param name="path">文件表中的主键Id</param>
        /// <param name="fileName">文件名称</param>
        /// <returns>返回图片流对象</returns>
        public ActionResult FileDisplayByPath(string path, string fileName)
        {
            return File(
                ConfigurationManager.AppSettings["UploadFolder"] + "/TempFolder/" + path,
                FileHelper.GetFileContentType(fileName));
        }

        /// <summary>
        /// 返回临时文件下载地址
        /// </summary>
        /// <param name="path">文件的guid</param>
        /// <param name="fileName">文件名</param>
        /// <returns>返回文件流对象</returns>
        public ActionResult TempFileDownLoad(string path, string fileName)
        {
            return File(
                ConfigurationManager.AppSettings["UploadFolder"] + "/TempFolder/" + path,
                FileHelper.GetFileContentType(fileName), 
                fileName);
        }

        /// <summary>
        /// 通过传图片表中的主键Id进行图片显示
        /// </summary>
        /// <param name="id">文件表中的主键Id</param>
        /// <returns>返回图片流对象</returns>
        public ActionResult FileDisplayById(long id)
        {
            UploadFileInfo uploadFile = new UploadFileInfoRepository().GetUploadFileInfoById(id);
            if (uploadFile != null)
            {
                return File(
                    FileHelper.GetFilePath(uploadFile.FilePath),
                    FileHelper.GetFileContentType(uploadFile.FileName));    
            }
            else
            {
                return null;
            }
        }

         /// <summary>
         /// 通过文件传表的返回文件下载地址
         /// </summary>
         /// <param name="id">文件表中的主键Id</param>
        /// <returns>返回文件流对象</returns>
        public ActionResult FileDownLoad(long id)
        {
            UploadFileInfo uploadFile = new UploadFileInfoRepository().GetUploadFileInfoById(id);
            if (uploadFile != null)
            {
                return File(
                    FileHelper.GetFilePath(uploadFile.FilePath),
                    FileHelper.GetFileContentType(uploadFile.FileName), 
                    uploadFile.FileName);
            }
            else
            {
                return null;
            }
        }

        #region 多文件实现单个文件上传到临时目录
        /// <summary>
        /// 文件上传
        /// </summary>
        /// <returns>文件上传页面</returns>
        public ActionResult FileUpload(string fileType, string fileHidId, string fileId)
        {
            ViewBag.fileType = fileType;
            ViewBag.fileHidId = fileHidId;
            ViewBag.fileId = fileId;
            return View();
        }

        [HttpPost]
        public ActionResult FileUpload(FormCollection form)
        {
            if (Request.Files != null && Request.Files[0] != null)
            {
                UploadFileInfoRepository upload = new UploadFileInfoRepository();
                UploadFileInfo uploadFile = new UploadFileInfo();
                if (Request.Files[0] != null)
                {
                    string result = new UploadFileInfoRepository().TempFileUpload(Request.Files[0]);
                    ViewBag.Flag = "1";
                    ViewBag.Path = result;
                    ViewBag.fileType = form["fileType"];
                    ViewBag.fileId = form["fileId"];
                    ViewBag.fileHidId = form["fileHidId"];
                }
            }

            return View();
        }

        /// <summary>
        /// 二审文件上传
        /// </summary>
        /// <returns>文件上传页面</returns>
        public ActionResult FileUploadForSecondAuditInfo(string fileType, string fileHidId, string fileId)
        {
            ViewBag.fileType = fileType;
            ViewBag.fileHidId = fileHidId;
            ViewBag.fileId = fileId;
            return View();
        }

        [HttpPost]
        public ActionResult FileUploadForSecondAuditInfo(FileInfo fileInfo)
        {
            string errList = string.Empty;

            if (Request.Files != null && Request.Files[0] != null)
            {
                ViewBag.Flag = "1";
                ViewBag.fileType = fileInfo.FileType;
                ViewBag.fileId = fileInfo.FileId;
                ViewBag.fileHidId = fileInfo.FileHidId;

                //上传数据检查
                errList = ErrList(fileInfo.FileType, fileInfo.FileHidId);
                ViewBag.errorInfo = errList;

                //验证正确
                if (string.IsNullOrEmpty(errList))
                {
                    //临时上传的公共文件
                    string result = new UploadFileInfoRepository().TempFileUpload(Request.Files[0]);
                    ViewBag.Path = result;
                }
            }

            return View();
        }

        public string ErrList(string fileType, string fileId)
        {
            List<string> listComtentType =FileHelper.ImageContentTypeList();
            int imgLimitedUploadCapacity = 2;
            int fileLimitedUploadCapacity = 10;

            if (fileType.IsNullOrWhiteSpace() || fileId.IsNullOrWhiteSpace())
            {
                return "二审iframe传递参数错误";
            }
        
            int uploadFileId = Convert.ToInt32(fileId);

            //图片验证
            if (fileType=="1")
            {
                if (uploadFileId == (int)Enums.InvestSecondAuditFilesUpload.CreditCertificate)
                {
                    if (!listComtentType.Contains(Request.Files[0].ContentType.ToLower()))
                    {
                       return "资信证明格式有误";
                    }

                    if (Request.Files[0].IsOverlimitedCapacity(imgLimitedUploadCapacity))
                    {
                        return "资信证明不超过2M";
                    }
                }

                if (uploadFileId == (int)Enums.InvestSecondAuditFilesUpload.IdOrganization)
                {
                    if (!listComtentType.Contains(Request.Files[0].ContentType.ToLower()))
                    {
                       return "机构代码格式有误";
                    }

                    if (Request.Files[0].IsOverlimitedCapacity(imgLimitedUploadCapacity))
                    {
                        return "机构代码证不超过2M";
                    }
                }

                if (uploadFileId == (int)Enums.InvestSecondAuditFilesUpload.IdCardFront)
                {
                    if (!listComtentType.Contains(Request.Files[0].ContentType.ToLower()))
                    {
                       return "身份证正面格式有误";
                    }

                    if (Request.Files[0].IsOverlimitedCapacity(imgLimitedUploadCapacity))
                    {
                        return "身份证正面不超过2M";
                    }
                }

                if (uploadFileId == (int)Enums.InvestSecondAuditFilesUpload.IdCardBack)
                {
                    if (!listComtentType.Contains(Request.Files[0].ContentType.ToLower()))
                    {
                       return "身份证反面格式不正确";
                    }

                    if (Request.Files[0].IsOverlimitedCapacity(imgLimitedUploadCapacity))
                    {
                        return "身份证反面不超过2M";
                    }
                }

                if (uploadFileId == (int)Enums.InvestSecondAuditFilesUpload.BusinessLicense)
                {
                    if (!listComtentType.Contains(Request.Files[0].ContentType.ToLower()))
                    {
                       return "营业执照格式不正确";
                    }

                    if (Request.Files[0].IsOverlimitedCapacity(imgLimitedUploadCapacity))
                    {
                        return "营业执照不超过2M";
                    }
                }

                if (uploadFileId == (int)Enums.InvestSecondAuditFilesUpload.TaxRegistration)
                {
                    if (!listComtentType.Contains(Request.Files[0].ContentType.ToLower()))
                    {
                       return "税务登记格式不正确";
                    }

                    if (Request.Files[0].IsOverlimitedCapacity(imgLimitedUploadCapacity))
                    {
                        return "税务登记不超过2M";
                    }
                }

                if (uploadFileId == (int)Enums.InvestSecondAuditFilesUpload.ProjectImage)
                {
                    if (!listComtentType.Contains(Request.Files[0].ContentType.ToLower()))
                    {
                        return "项目图片格式不正确";
                    }

                    if (Request.Files[0].IsOverlimitedCapacity(imgLimitedUploadCapacity))
                    {
                        return "项目图片不超过2M";
                    }
                }
            }
            else
            {
                if (uploadFileId == (int)Enums.InvestSecondAuditFilesUpload.BusinessPlan)
                {
                    if (Request.Files[0].ContentLength==0)
                    {
                        return "商业计划书不能为0字节";
                    }

                    if (Request.Files[0].IsOverlimitedCapacity(fileLimitedUploadCapacity))
                    {
                        return "商业计划书不超过10M";
                    }
                }

                if (uploadFileId == (int)Enums.InvestSecondAuditFilesUpload.Manual)
                {
                    if (Request.Files[0].ContentLength == 0)
                    {
                        return "证明书不能为0字节";
                    }

                    if (Request.Files[0].IsOverlimitedCapacity(fileLimitedUploadCapacity))
                    {
                        return "证明书不超过10M";
                    }
                }

                if (uploadFileId == (int)Enums.InvestSecondAuditFilesUpload.Accessory1FileId)
                {
                    if (Request.Files[0].ContentLength == 0)
                    {
                        return "附件1不能为0字节";
                    }

                    if (Request.Files[0].IsOverlimitedCapacity(fileLimitedUploadCapacity))
                    {
                        return "附件1不超过10M";
                    }
                }

                if (uploadFileId == (int)Enums.InvestSecondAuditFilesUpload.Accessory2FileId)
                {
                    if (Request.Files[0].ContentLength == 0)
                    {
                        return "附件2不能为0字节";
                    }

                    if (Request.Files[0].IsOverlimitedCapacity(fileLimitedUploadCapacity))
                    {
                        return "附件2不超过10M";
                    }
                }

                if (uploadFileId == (int)Enums.InvestSecondAuditFilesUpload.Accessory3FileId)
                {
                    if (Request.Files[0].ContentLength == 0)
                    {
                        return "附件3不能为0字节";
                    }

                    if (Request.Files[0].IsOverlimitedCapacity(fileLimitedUploadCapacity))
                    {
                        return "附件3不超过10M";
                    }
                }
            }

            return string.Empty;
        }

        public class FileInfo
        {
            public string FileType { get; set; }

            public string FileHidId { get; set; }

            public string FileId { get; set; }
        }

        #endregion
    }
}