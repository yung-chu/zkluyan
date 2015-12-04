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
using Zkly.Common.Dictionary;
using Zkly.Common.Log;
using Zkly.DAL.Context;
using Zkly.DAL.Models;

namespace Zkly.BLL.Repositories
{
    public class UploadFileInfoRepository: RepositoryBase<UserDbContext>
    {
        private UserDbContext context = new UserDbContext();
        private string root = ConfigurationManager.AppSettings["UploadFolder"];

        public UploadFileInfo GetUploadFileInfoById(long id)
        {
            return context.UploadFileInfos.Find(id);
        }

        public bool AddUploadFileInfo(UploadFileInfo model)
        {
            context.UploadFileInfos.Add(model);
            context.SaveChanges();
            return true;
        }

        //返回增加后的主键
        public long RetunUploadFileInfoId(UploadFileInfo model)
        {
            context.UploadFileInfos.Add(model);
            context.SaveChanges();
            return model.Id;
        }

        public bool UpdateUploadFileInfo(UploadFileInfo model)
        {
            context.Entry(model).State = EntityState.Modified;
            context.SaveChanges();
            return true;
        }

        #region 单文件跟随表单上传过来
        /// <summary>
        /// 得到文件上传对象
        /// </summary>
        /// <param name="file">客户端对象</param>
        /// <param name="folderPath">文件夹路径</param>
        public UploadFileInfo GetFileInfo(HttpPostedFileBase file, int folderPath)
        {
            UploadFileInfo uploadFile = null;
            try
            {
                if (file != null)
                {
                    uploadFile = new UploadFileInfo();
                    string path = EnumService.GetFolderPathName(folderPath);
                    string date = DateTime.Now.ToString("yyyyMMdd");
                    string folder = Path.Combine(path, date);

                    //判断是否存在该文件夹，如果不存在就创建文件夹
                    if (!Directory.Exists(root + "/" + folder))
                    {
                        Directory.CreateDirectory(root + "/" + folder);
                    }

                    uploadFile.FileName = Path.GetFileName(file.FileName);
                    string guid = Guid.NewGuid().ToString();
                    uploadFile.FilePath = folder+"/"+guid;
                    uploadFile.CreateTime = DateTime.Now;
                    file.SaveAs(root + "/" + folder+"/"+guid);

                    //数据库进行数据保存
                    context.UploadFileInfos.Add(uploadFile);
                    context.SaveChanges();
                    return uploadFile;
                }
            }
            catch (Exception se)
            {
                Logger.Error("文件上传失败！", se);
            }

            return uploadFile;
        }
        #endregion

        #region 多文件实现单个文件上传到服务器操作
        //将临时文件剪切到正式文件夹中
        public UploadFileInfo AddUploadFileInfo(int folderPath, string fileInfo)
        {
            string path = Zkly.Common.Dictionary.EnumService.GetFolderPathName(folderPath);
            string date = DateTime.Now.ToString("yyyyMMdd");
            string folder = Path.Combine(path, date);

            //判断是否存在该文件夹，如果不存在就创建文件夹
            if (!Directory.Exists(root + "/" + folder))
            {
                Directory.CreateDirectory(root + "/" + folder);
            }

            string[] strs = fileInfo.Split(':');
            var filePath = folder + "\\" + strs[0];

            UploadFileInfo uploadFile = new UploadFileInfo();
            uploadFile.FileName = strs[1];
            uploadFile.FilePath = filePath;
            uploadFile.CreateTime = DateTime.Now;

            //从临时文件夹拷贝到正式文件夹
            if (File.Exists(root + "/TempFolder/" + strs[0]))
            {
                File.Move(root + "/TempFolder/" + strs[0], root + "/" + filePath);
            }

            //数据库进行数据保存
            context.UploadFileInfos.Add(uploadFile);
            context.SaveChanges();
            return uploadFile;
        }

        /// <summary>
        ///  临时上传的公共文件
        /// </summary>
        /// <param name="file">客户端对象</param>
        /// <returns>返回文件上传后的对象</returns>
        public string TempFileUpload(HttpPostedFileBase file)
        {   
            //判断是否存在该文件夹，如果不存在就创建文件夹
            if (!Directory.Exists(root + "/TempFolder"))
            {
                Directory.CreateDirectory(root + "/TempFolder");
            }

            string guid = Guid.NewGuid().ToString();
            file.SaveAs(root + "/TempFolder/" + guid);
            string fileName = Path.GetFileName(file.FileName);
            return guid + ":" + fileName;
        }
        #endregion

        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="id">主键Id</param>
        /// <returns>返回删除成功状态</returns>
        public bool RemoveUploadFileInfo(long id)
        {
            try
            {
                UploadFileInfo uploadFileInfo = context.UploadFileInfos.Single(o => o.Id == id);
                context.UploadFileInfos.Remove(uploadFileInfo);
                context.SaveChanges();

                //同时删除文件目录
                File.Delete(root + "/" + uploadFileInfo.FilePath);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
