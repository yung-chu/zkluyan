using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zkly.DAL.Models
{
    public class Organization
    {
        public int Id { get; set; }

        [MaxLength(128)]
        public string Name { get; set; }
    }
}
