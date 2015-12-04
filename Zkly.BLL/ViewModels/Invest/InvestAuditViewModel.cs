using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zkly.DAL.Models;

namespace Zkly.BLL.ViewModels
{
    public class InvestAuditViewModel
    {
        public EInvestAuditStage Stage { get; set; }

        public DAL.Models.Invest Invest { get; set; }

        public IList<InvestHistory> Histories { get; set; }
    }
}
