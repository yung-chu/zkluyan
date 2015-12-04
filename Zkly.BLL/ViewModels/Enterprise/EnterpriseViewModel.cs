using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using PagedList;

using Zkly.DAL.Models;

namespace Zkly.BLL.ViewModels
{
    public class EnterpriseViewModel
    {
        public IList<Loan> Loans { get; set; }

        public IList<DAL.Models.Invest> Invests { get; set; }

        public IList<DAL.Models.Roadshow> Roadshows { get; set; }

        public IList<CapitalRoadshow> CapitalRoadshows { get; set; }

        public DAL.Models.Message Message { get; set; }

        public bool? MessageState { get; set; }

        public IPagedList<DAL.Models.Message> ListMessage { get; set; }

        public bool HasMessage()
        {
            return Message != null;
        }
    }
}
