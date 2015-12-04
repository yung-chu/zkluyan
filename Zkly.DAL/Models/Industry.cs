using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zkly.DAL.Models
{
    public class Industry
    {
        public int Id { get; set; }

        [Display(Name = "所属行业")]
        [MaxLength(20, ErrorMessage = "所属行业长度不能大于20！")]
        public string Name { get; set; }
    }
}
