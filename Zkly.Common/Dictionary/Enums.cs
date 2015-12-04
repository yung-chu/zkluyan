namespace Zkly.Common.Dictionary
{
    public class Enums
    {
        #region 数据字典类型枚举
        /// <summary>
        /// 数据字典类型(数据库存储)
        /// </summary>
        /// <remarks>此枚举只用名称（ToString），不用值</remarks>
        public enum DataDictionaryType
        {
            /// <summary>
            /// 全部
            /// </summary>
            All,

            /// <summary>
            /// 产业
            /// </summary>
            Industries,

            /// <summary>
            /// 是否禁用前台注册
            /// </summary>
            IsRegister  
        }
        #endregion

        /// <summary>
        /// 枚举类型(非数据库存储)
        /// </summary>
        public enum EnumDictionaryType
        {
            /// <summary>
            /// 全部
            /// </summary>
            All,

            /// <summary>
            /// 记录状态
            /// </summary>
            RecordState,

            /// <summary>
            /// 是否状态
            /// </summary>
            YesOrNo,

            /// <summary>
            /// 文档类型
            /// </summary>
            FileType
        }

        /// <summary>
        /// 记录状态
        /// </summary>
        public enum RecordState
        {
            /// <summary>
            /// All
            /// </summary>
            All = -1,

            /// <summary>
            /// 启用
            /// </summary>
            Enable = 1,

            /// <summary>
            /// 禁用
            /// </summary>
            Disabled = 0
        }

        /// <summary>
        /// 记录状态
        /// </summary>
        public enum YesOrNo
        {
            /// <summary>
            /// All
            /// </summary>
            All = -1,

            /// <summary>
            /// 是
            /// </summary>
            Yes = 1,

            /// <summary>
            /// 否
            /// </summary>
            No = 0
        }

        /// <summary>
        /// 文件夹路径配置
        /// </summary>
        public enum FolderPath
        {
            /// <summary>
            /// All
            /// </summary>
            All = -1,
            Business = 1,
            Capital = 2,
            InvestSecondAuditFiles = 3,
            Videos = 4,
            BusinessLicenseFiles=5,
            InvestAgreement=6
        }

        /// <summary>
        /// 文件夹路径配置
        /// </summary>
        public enum InvestSecondAuditFilesUpload
        {
            //资信证明
            CreditCertificate = 1,

            //机构代码证
            IdOrganization = 2,

            //法人身份证正面
            IdCardFront = 3,

            //法人身份证反面
            IdCardBack = 4,

            //营业执照
            BusinessLicense = 5,

            //税务登记证
            TaxRegistration = 6,

            //商业计划书
            BusinessPlan = 7,

            //证明书
            Manual = 8,

            //附件1
            Accessory1FileId = 9,

            //附件2
            Accessory2FileId = 10,

            //附件3
            Accessory3FileId = 11,

            //项目图片
            ProjectImage=13
        }

        /// <summary>
        /// 记录状态
        /// </summary>
        public enum FileOrImage
        {
            /// <summary>
            /// All
            /// </summary>
            All = -1,

            /// <summary>
            /// 启用
            /// </summary>
            Image = 1,

            /// <summary>
            /// 启用
            /// </summary>
            File = 2
        }

        //RoadshowUploadWatcher表中SyncStatus同步状态
        public enum SyncStatus
        {
            /// <summary>
            /// All
            /// </summary>
            All = -1,

            /// <summary>
            /// 未同步
            /// </summary>
            UnSync = 0,

            /// <summary>
            /// 正在同步
            /// </summary>
            OnSync = 1,

            /// <summary>
            /// 同步完成
            /// </summary>
            OkSync = 2
        }   
    }
}
