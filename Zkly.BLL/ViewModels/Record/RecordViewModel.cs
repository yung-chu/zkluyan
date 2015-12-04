using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Zkly.BLL.Vhall;

namespace Zkly.BLL.ViewModels
{
    public class RecordViewModel
    {
        public long? CapitalId { get; set; }

        public string OriginData { get; set; }

        public List<Record> Records { get; set; }

        public List<RecordPart> RecordParts { get; set; }
    }
}
