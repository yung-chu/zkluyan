using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Repository;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Zkly.Common;
using Zkly.Common.Dictionary;
using Zkly.Common.Utils;
using Zkly.DAL.Context;
using Zkly.DAL.Models;

namespace Zkly.BLL.Repositories
{
    public class PreferenceRepository : RepositoryBase<UserDbContext>
    {
        public InvestorPreference GetPreferenceByUserId(string userId)
        {
            return FirstOrDefault<InvestorPreference>(p => p.UserId == userId);
        }

        public bool SavePreference(InvestorPreference preference, HttpPostedFileBase businessLicenseFile)
        {
            if (preference.Id == 0)
            {
                if (businessLicenseFile != null)
                {
                    UploadFileInfoRepository uploadFileInfoRepository=new UploadFileInfoRepository();
                    UploadFileInfo uploadFileInfo=uploadFileInfoRepository.GetFileInfo(
                        businessLicenseFile,
                        (int)Enums.FolderPath.BusinessLicenseFiles);
                    if (uploadFileInfo != null)
                    {
                        preference.FileId = uploadFileInfo.Id;
                    }
                }

                Add(preference);
            }
            else
            {
                var orginal = Single<InvestorPreference>(p => p.Id == preference.Id);
                UploadFileInfoRepository uploadFileInfoRepository = new UploadFileInfoRepository();

                if (businessLicenseFile != null)
                {
                    //判断源文件是否有
                    if (orginal.FileId > 0)
                    {
                        uploadFileInfoRepository.RemoveUploadFileInfo(orginal.FileId);
                    }

                    //上传文件
                    UploadFileInfo uploadFileInfo = uploadFileInfoRepository.GetFileInfo(
                        businessLicenseFile,
                        (int)Enums.FolderPath.BusinessLicenseFiles);
                    if (uploadFileInfo != null)
                    {
                        preference.FileId = uploadFileInfo.Id;
                    }
                }
                
                Update(preference);
            }

            return true;
        }

        public InvestorPreference SelOneInvestorPreference(long? id)
        {
            return Find<InvestorPreference>(p => p.Id == id).FirstOrDefault();
        }
    }
}
