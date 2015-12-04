using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Core.Mapping;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Migrations;
using System.Data.Entity.Repository;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Zkly.Common.Log;
using Zkly.DAL.Context;
using Zkly.DAL.Models;

namespace Zkly.BLL.Repositories
{
    public class InvestSecondAuditFileReponsitory: RepositoryBase<UserDbContext>
    {
        #region 一对多数据保存
        /// <summary>
        /// 一对多数据保存
        /// </summary>
        /// <param name="listInvestSecondAuditFile">InvestSecondAuditFile对象</param>
        public string AddOrUpdateInvestSecondAuditFile(List<InvestSecondAuditFile> listInvestSecondAuditFile, UserDbContext db)
        {
            string getInfo = string.Empty;
            List<InvestSecondAuditFile> toAddInvestSecondAuditFile=new List<InvestSecondAuditFile>();

            foreach (var fileInfo in listInvestSecondAuditFile)
            {
                //是否有记录存在InvestSecondAuditFiles表
                var model = db.InvestSecondAuditFiles.FirstOrDefault(p =>
                    p.InvestSecondAuditId == fileInfo.InvestSecondAuditId && p.FileTypeId == fileInfo.FileTypeId);

                if (model != null)
                {
                    //更新新上传的文件Id
                    string sql =
                        string.Format(
                            "update InvestSecondAuditFiles set UploadFileInfoesId='{0}' where InvestSecondAuditId='{1}' and FileTypeId='{2}'",
                            fileInfo.UploadFileInfoesId,
                            fileInfo.InvestSecondAuditId,
                            fileInfo.FileTypeId);

                    bool updateResult = db.Database.ExecuteSqlCommand(sql) == 1;

                    //删除之前存在的上传文件
                    bool deleteResult =
                        new UploadFileInfoRepository().RemoveUploadFileInfo(model.UploadFileInfoesId);

                    //失败
                    if (!updateResult&&!deleteResult)
                    {
                        getInfo = "二审上传文件更新或删除失败!";
                        break;
                    }
                }
                else
                {
                    toAddInvestSecondAuditFile.Add(fileInfo);
                }
            }

            db.InvestSecondAuditFiles.AddRange(toAddInvestSecondAuditFile);
            return getInfo;
        }

        /// <summary>
        /// 删除关联表数据
        /// </summary>
        /// <param name="fileInfo">第三张表对象</param>
        public bool RemoveInvestSecondAuditFile(InvestSecondAuditFile fileInfo)
        {
            using (UserDbContext db = new UserDbContext())
            {
                //数据库保存操作
                db.InvestSecondAuditFiles.Remove(fileInfo);
                db.SaveChanges();
            }

            return true;
        }

        public long? GetInvestSecondAuditFile(long investSecondAuditId, long fileTypeId)
        {
            using (UserDbContext db = new UserDbContext())
            {
                var model =
                    db.InvestSecondAuditFiles.AsNoTracking()
                        .FirstOrDefault(p => p.InvestSecondAuditId == investSecondAuditId && p.FileTypeId == fileTypeId);

                if (model != null)
                {
                    return model.UploadFileInfo.Id;
                }
            }

            return null;
        }
        #endregion
    }
}
