using PagedList;

using Zkly.DAL.Models;

namespace Zkly.Admin.Web.Models
{
    public class InvestViewModel
    {
        public Invest Invest { get; set; }
      
        public string ProjectSource { get; set; }

        //知识产权形式
        public string IpRform { get; set; }

        //资信证明
        public long CreditCertificateFile { get; set; }

        //机构代码证
        public long IdOrganizationFile { get; set; }

        //身份证正面
        public long IdCardFrontFile { get; set; }

        //身份证反面
        public long IdCardBackFile { get; set; }

        //营业执照
        public long BusinessLicenseFile { get; set; }

        //商业计划书
        public long BusinessPlanFile { get; set; }

        //税务登记
        public long TaxRegistrationFile { get; set; }

        //证明书
        public long ManualFile { get; set; }

        //附件1
        public long Accessory1File { get; set; }

        //附件2
        public long Accessory2File { get; set; }

        //附件3
        public long Accessory3File { get; set; }
    }

    public class TempFolderModel
    {
        public IPagedList<InvestTempFolder> ListTempFolder { get; set; }
    }
}