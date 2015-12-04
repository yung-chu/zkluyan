using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Zkly.BLL.Vhall;

namespace Zkly.Admin.Web.Models
{
    public class RecordViewModel
    {
        public long? CapitalId { get; set; }

        public List<Record> Records { get; set; }

        public List<RecordPart> RecordParts { get; set; }
    }
}