using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using PagedList;

using Zkly.DAL.Models;

namespace Zkly.BLL.ViewModels
{
    public class TempFolderModel
    {
       public IPagedList<InvestTempFolder> ListTempFolder { get; set; }
    }
}
