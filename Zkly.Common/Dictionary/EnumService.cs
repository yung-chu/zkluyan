using System.Collections.Generic;
using System.Linq;

namespace Zkly.Common.Dictionary
{
    public class EnumService
    {
        /// <summary>
        /// 判断该键值是否存在的方法
        /// </summary>
        /// <param name="key">键值</param>
        /// <param name="dics">集合类型</param>
        /// <returns>返回名称</returns>
        private static string GetResult(int key, Dictionary<int, string> dics)
        {
            string result = "";
            if (dics.Any(item => key == item.Key))
            {
                result = dics[key];
            }

            return result;
        }

        #region RecordState  记录状态

        /// <summary>
        /// 获记录状态
        /// </summary>
        /// <param name="includeAll">是否包含全部</param>
        /// <returns>记录状态字典</returns>
        public static Dictionary<int, string> GetRecordState(bool includeAll)
        {
            Dictionary<int, string> recordStates = new Dictionary<int, string>();
            if (includeAll)
            {
                recordStates.Add((int)Enums.RecordState.All, "全部");
            }

            recordStates.Add((int)Enums.RecordState.Enable, "启用");
            recordStates.Add((int)Enums.RecordState.Disabled, "禁用");
            return recordStates;
        }

        /// <summary>
        /// 获取状态名称
        /// </summary>
        /// <param name="key">主键值</param>
        /// <returns>获取名称</returns>
        public static string GetRecordStateName(int key)
        {
            return GetResult(key, GetRecordState(false));
        }

        #endregion

        #region FolderPath  文件夹路径

        /// <summary>
        /// 获记录状态
        /// </summary>
        /// <param name="includeAll">是否包含全部</param>
        /// <returns>记录状态字典</returns>
        public static Dictionary<int, string> GetFolderPath(bool includeAll)
        {
            Dictionary<int, string> folderPaths = new Dictionary<int, string>();
            if (includeAll)
            {
                folderPaths.Add((int)Enums.FolderPath.All, "全部");
            }

            folderPaths.Add((int)Enums.FolderPath.Business, @"Covers\Business");
            folderPaths.Add((int)Enums.FolderPath.Capital, @"Covers\Capital");
            folderPaths.Add((int)Enums.FolderPath.InvestSecondAuditFiles, "InvestSecondAuditFiles");
            folderPaths.Add((int)Enums.FolderPath.Videos, "Videos");
            folderPaths.Add((int)Enums.FolderPath.BusinessLicenseFiles, "BusinessLicenseFiles");
            folderPaths.Add((int)Enums.FolderPath.InvestAgreement, "InvestAgreements");
            
            return folderPaths;
        }

        /// <summary>
        /// 获取状态名称
        /// </summary>
        /// <param name="key">主键值</param>
        /// <returns>获取名称</returns>
        public static string GetFolderPathName(int key)
        {
            return GetResult(key, GetFolderPath(false));
        }

        #endregion

        #region InvestSecondAuditFilesUpload  文件夹路径

        /// <summary>
        /// 获记录状态
        /// </summary>
        /// <returns>记录状态字典</returns>
        public static Dictionary<int, ZkjlKeyValuePair> GetInvestSecondAuditFilesUpload()
        {
            Dictionary<int, ZkjlKeyValuePair> folderPaths = new Dictionary<int, ZkjlKeyValuePair>();

            //图片类型
            folderPaths.Add((int)Enums.InvestSecondAuditFilesUpload.CreditCertificate, new ZkjlKeyValuePair((int)Enums.FileOrImage.Image, "资信证明"));
            folderPaths.Add((int)Enums.InvestSecondAuditFilesUpload.IdOrganization, new ZkjlKeyValuePair((int)Enums.FileOrImage.Image, "机构代码证"));
            folderPaths.Add((int)Enums.InvestSecondAuditFilesUpload.IdCardFront, new ZkjlKeyValuePair((int)Enums.FileOrImage.Image, "法人身份证正面"));
            folderPaths.Add((int)Enums.InvestSecondAuditFilesUpload.IdCardBack, new ZkjlKeyValuePair((int)Enums.FileOrImage.Image, "法人身份证反面"));
            folderPaths.Add((int)Enums.InvestSecondAuditFilesUpload.BusinessLicense, new ZkjlKeyValuePair((int)Enums.FileOrImage.Image, "营业执照"));
            folderPaths.Add((int)Enums.InvestSecondAuditFilesUpload.TaxRegistration, new ZkjlKeyValuePair((int)Enums.FileOrImage.Image, "税务登记证"));
            folderPaths.Add((int)Enums.InvestSecondAuditFilesUpload.ProjectImage, new ZkjlKeyValuePair((int)Enums.FileOrImage.Image, "项目图片"));

            //文件类型
            folderPaths.Add((int)Enums.InvestSecondAuditFilesUpload.BusinessPlan, new ZkjlKeyValuePair((int)Enums.FileOrImage.File, "商业计划书"));
            folderPaths.Add((int)Enums.InvestSecondAuditFilesUpload.Manual, new ZkjlKeyValuePair((int)Enums.FileOrImage.File, "证明书"));
            folderPaths.Add((int)Enums.InvestSecondAuditFilesUpload.Accessory1FileId, new ZkjlKeyValuePair((int)Enums.FileOrImage.File, "附件1"));
            folderPaths.Add((int)Enums.InvestSecondAuditFilesUpload.Accessory2FileId, new ZkjlKeyValuePair((int)Enums.FileOrImage.File, "附件2"));
            folderPaths.Add((int)Enums.InvestSecondAuditFilesUpload.Accessory3FileId, new ZkjlKeyValuePair((int)Enums.FileOrImage.File, "附件3"));
            return folderPaths;
        }

        /// <summary>
        /// 获取状态名称
        /// </summary>
        /// <param name="key">主键值</param>
        /// <returns>获取名称</returns>
        public static string GetInvestSecondAuditFilesUploadName(int key)
        {
            Dictionary<int, ZkjlKeyValuePair> folderPaths = GetInvestSecondAuditFilesUpload();
            foreach (var item in folderPaths)
            {
                if (item.Key == key)
                {
                    return item.Value.Value;
                }
            }

            return "";
        }
        #endregion
    }
}
