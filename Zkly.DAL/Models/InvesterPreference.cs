using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zkly.DAL.Models
{
    public class InvestorPreference
    {
        public long Id { get; set; }

        [MaxLength(128)]
        public string UserId { get; set; }

        public long Lower { get; set; }

        public long Upper { get; set; }

        [MaxLength(128)]
        public string OrgPreference { get; set; }

        [MaxLength(128)]
        public string IndustryPreference { get; set; }

        //引用的文件FileId
        public long FileId { get; set; }

        public virtual ApplicationUser User { get; set; }
    }
}
