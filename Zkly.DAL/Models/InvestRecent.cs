using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zkly.DAL.Models
{
    public class InvestRecent
    {
        public int Id { get; set; }

        public DateTime Time { get; set; }

        public decimal InvestAmount { get; set; }

        public decimal InvestSum { get; set; }

        public int CompanySum { get; set; }
    }
}
