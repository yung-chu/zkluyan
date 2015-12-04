using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zkly.DAL.Models
{
    public class LoanRecent
    {
        public int Id { get; set; }

        public DateTime Time { get; set; }

        public decimal LoanAmount { get; set; }

        public decimal LoanSum { get; set; }
       
        public int BorrowUser { get; set; }
    }
}
