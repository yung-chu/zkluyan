using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Zkly.DAL.Models;

namespace Zkly.BLL.ViewModels
{
    public class InvestAgreementViewModel
    {
        public long InvestId { get; set; }

        public InvestAgreement InvestAgreement { get; set; }

        public IList<AgencyCommission> CashOptions { get; set; }

        public IList<AgencyCommission> StockOptions { get; set; }

        public IList<AgencyCommission> CombineOptions { get; set; }

        public string ErrorInfo { get; set; }
    }

    public class InvestAgreementPostModel
    {
        public long InvestId { get; set; }

        [Required(ErrorMessage = "请选择锁定期")]
        public short LockMonth { get; set; }

        [Required(ErrorMessage = "请选佣金方式")]
        public int AgencyCommissionId { get; set; }

        public long AgencyCommission { get; set; }

        public bool AgreementChecked { get; set; }
    }
}
