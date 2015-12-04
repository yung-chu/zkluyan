using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Zkly.DAL.Models
{
    public class InvestAgreement
    {
        [Key, ForeignKey("Invest")]
        public long Id { get; set; }

        public short LockMonth { get; set; }

        public int AgencyCommissionId { get; set; }

        public long? AgreementFileId { get; set; }

        public virtual Invest Invest { get; set; }

        public virtual AgencyCommission AgencyCommission { get; set; }

        public virtual UploadFileInfo AgreementFile { get; set; }
    }

    public class AgencyCommission
    {
        [Display(Name = "编号")]
        public int Id { get; set; }

        [Display(Name = "现金比例")]
        [DisplayFormat(DataFormatString = "{0:P2}")]
        public float CashPercent { get; set; }

        [Display(Name = "股权比例")]
        [DisplayFormat(DataFormatString = "{0:P2}")]
        public float StockPercent { get; set; }

        [Display(Name = "描述")]
        public string Description { get; set; }
    }
}
