using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Zkly.Common;
using Zkly.DAL.Models;

namespace Zkly.Admin.Web.Models
{
    public class InvestAuditViewModel
    {
        public EInvestAuditStage Stage { get; set; }

        public Invest Invest { get; set; }

        public IList<InvestHistory> Histories { get; set; }
    }
}