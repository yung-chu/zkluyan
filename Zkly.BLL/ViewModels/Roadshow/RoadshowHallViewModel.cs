using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Zkly.DAL.Models;

namespace Zkly.BLL.ViewModels
{
    public class RoadshowHallViewModel
    {
        public List<DAL.Models.Roadshow> BusinessRoadshows { get; set; }

        public List<CapitalRoadshow> CapitalRoadshows { get; set; }
    }
}
