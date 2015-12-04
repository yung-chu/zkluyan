using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Zkly.BLL.ViewModels
{
    public class InvestorPreferenceViewModel
    {
        public long Id { get; set; }

        public string InvestmentScales { get; set; }

        public long FileId { get; set; }

        public HttpPostedFileBase BusinessLicense { get; set; }

        public List<OrgPreferenceViewModel> OrgPreferences { get; set; }

        public List<IndustryViewModel> IndustryPreferences { get; set; }
    }

    public class OrgPreferenceViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public bool IsAssign { get; set; }
    }

    public class IndustryViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public bool IsAssign { get; set; }
    }
}
