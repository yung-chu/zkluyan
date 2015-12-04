using System.Collections.Generic;
using System.Configuration;
using System.Web.Mvc;
using Zkly.BLL.Repositories;
using Zkly.Common;
using Zkly.Common.Mvc.UI;
using Zkly.DAL.Models;

namespace Zkly.Admin.Web.Controllers
{
    [Authorize]
    public class FileController : Controller
    {
        [Route("get-file-{id}")]
        public ActionResult DownLoad(long id)
        {
            UploadFileInfo uploadFile = new UploadFileInfoRepository().GetUploadFileInfoById(id);
            if (uploadFile != null)
            {
                return File(FileHelper.GetFilePath(uploadFile.FilePath), FileHelper.GetFileContentType(uploadFile.FileName), uploadFile.FileName);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 通过传图片表中的主键Id进行图片显示
        /// </summary>
        /// <param name="id">文件表中的主键Id</param>
        /// <returns>返回图片流对象</returns>
        [Route("get-img-{id}")]
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

        public ActionResult ShowPartial(long investSecondAuditId, long typeId)
        {
            long? uploadFileInfoId = new InvestSecondAuditFileReponsitory().GetInvestSecondAuditFile(
                investSecondAuditId,
                typeId);

            if (uploadFileInfoId.HasValue)
            {
                UploadFileInfo uploadFileInfo = new UploadFileInfoRepository().GetUploadFileInfoById(uploadFileInfoId.Value);

                //是否是图片
                ViewBag.IsImg = false;
                if (uploadFileInfo != null && uploadFileInfo.FileName != null)
                {
                    ViewBag.IsImg = FileHelper.IsImage(uploadFileInfo.FileName);
                }

                return View(uploadFileInfo);
            }

            return View();
        }
    }
}