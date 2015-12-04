using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zkly.DAL.Models;

namespace Zkly.BLL.ViewModels
{
    public class BusinessRoadshowShowOrderViewModel
    {
        public List<DAL.Models.Roadshow> Roadshows { get; set; }

        public List<DAL.Models.Roadshow> Top8Shows { get; set; }
    }
}
