using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zkly.DAL.Models
{
    public class InvestTempFolder
    {
        public long Id { get; set; }

        [MaxLength(128)]
        public string UserId { get; set; }

        public long InvestId { get; set; }

        public DateTime CreateTime { get; set; }

        public virtual Invest Invest { get; set; }

        public virtual ApplicationUser User { get; set; }
    }
}
