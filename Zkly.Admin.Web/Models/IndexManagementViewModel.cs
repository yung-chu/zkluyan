using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Zkly.DAL.Models;

namespace Zkly.Admin.Web.Models
{
    public class IndexManagementViewModel
    {
        public ApplyLoan ApplyLoan { get; set; }

        public ApplyInvest ApplyInvest { get; set; }

        public string FirstAuditInfo { get; set; }

        public List<SelectListItem> SelectFristItems { get; set; }

        public IEnumerable<SelectListItem> SelectListItems { get; set; }

        public IList<ApplyLoan> ApplyLoans { get; set; }

        public IList<ApplyInvest> ApplyInvests { get; set; }
    }
}