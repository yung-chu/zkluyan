using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using Zkly.Common.Serialize;

namespace Zkly.BLL.Vhall
{
    public class ResponseData
    {
        public string Code { get; set; }

        public string Data { get; set; }

        public bool Success
        {
            get
            {
                return this.Code.Equals("1");
            }
        }
    }

    public class Record
    {
        public string Id { get; set; }

        public string Subject { get; set; }

        public DateTime Create_Time { get; set; }

        public int Is_Public { get; set; }

        public string Url { get; set; }
    }

    public class RecordPart
    {
        public string Session_Id { get; set; }

        public string Start_Time { get; set; }

        public string End_Time { get; set; }

        public List<Fragment> List { get; set; }

        public string ToRecList()
        {
            return string.Join(",", this.List.Select(f => f.Id).ToArray());
        }
    }

    public class Fragment
    {
        public string Id { get; set; }

        public string Subject { get; set; }

        public string Start_Time { get; set; }

        public string End_Time { get; set; }
    }

    public class MultiRecord
    {
        public string Subject { get; set; }

        public Dictionary<string, List<long>> RecList { get; set; }

        public string ToParam()
        {
            string jsonData = JsonSerializer.Serialize(this);

            jsonData = Regex.Replace(jsonData, "\\[\\d{1,2}\\]", " ").Replace("Subject", "subject").Replace("RecList", "rec_list");

            return HttpUtility.UrlEncode(jsonData, Encoding.UTF8);
        }
    }
}
