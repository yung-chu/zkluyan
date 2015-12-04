using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Zkly.Common.Config;
using Zkly.Common.Serialize;

namespace Zkly.BLL.Vhall
{
    public class PostData
    {
        public PostData()
        {
            this.Uid = VhallSettings.ApiUid;
            this.Ukey = VhallSettings.ApiUkey;
        }

        public string Op { get; set; }

        public string Uid { get; set; }

        public string Ukey { get; set; }

        public string WebinarId { get; set; }

        public string GetRequiredParam()
        {
            return string.Format("op={0}&uid={1}&ukey={2}&webinar_id={3}", this.Op, this.Uid, this.Ukey, this.WebinarId);
        }

        public virtual string ToParamString()
        {
            return GetRequiredParam();
        }
    }

    public class CreateActivity : PostData
    {
        public CreateActivity()
        {
            this.Op = "datingwebinar";
        }

        public ActivityData Data { get; set; }

        public override string ToParamString()
        {
            var requiredParam = GetRequiredParam();

            return string.Format("{0}&data={1}", requiredParam, this.Data.ToEncodedJsonString());
        }
    }

    public class DescAttribute : Attribute
    {
        public DescAttribute(string description)
        {
            Description = description;
        }

        public string Description { get; set; }
    }

    public class ActivityData
    {
        [Desc("subject")]
        public string Subject { get; set; }

        [Desc("t_start")]
        public string TStart { get; set; }

        [Desc("t_end")]
        public string TEnd { get; set; }

        [Desc("host")]
        public string Host { get; set; }

        [Desc("webinar_desc")]
        public string WebinarDesc { get; set; }

        [Desc("channel_pass")]
        public string ChannelPass { get; set; }

        public string ToEncodedJsonString()
        {
            string jsonData = GetJsonData();

            return HttpUtility.UrlEncode(jsonData, Encoding.UTF8);
        }

        private string GetJsonData()
        {
            Hashtable values = new Hashtable();
            foreach (var property in this.GetType().GetProperties())
            {
                string valstr = string.Empty;

                var value = property.GetValue(this, null);
                if (value != null)
                {
                    valstr = value.ToString();
                }

                var attribute = property.GetCustomAttributes(typeof(DescAttribute), false).FirstOrDefault();
                string namestr = attribute == null ? property.Name : ((DescAttribute)attribute).Description;
                values.Add(namestr, valstr);
            }

            return JsonSerializer.Serialize(values);
        }
    }

    public class DeleteActivity : PostData
    {
        public DeleteActivity()
        {
            this.Op = "delwebinar";
        }
    }

    public class StartActivity : PostData
    {
        public StartActivity()
        {
            this.Op = "startwebinar";
            this.Forcerecord = 1;
        }

        public int Forcerecord { get; set; }

        public override string ToParamString()
        {
            var required = GetRequiredParam();

            return string.Format("{0}&forcerecord={1}", required, this.Forcerecord);
        }
    }

    public class EndActivity : PostData
    {
        public EndActivity()
        {
            this.Op = "endwebinar";
        }
    }

    public class UpdateActivity : PostData
    {
        public UpdateActivity()
        {
            this.Op = "updatewebinar";
        }

        public ActivityData Data { get; set; }

        public override string ToParamString()
        {
            var requiredParam = GetRequiredParam();

            return string.Format("{0}&data={1}", requiredParam, this.Data.ToEncodedJsonString());
        }
    }

    public class ActivityStatus : PostData
    {
        public ActivityStatus()
        {
            this.Op = "webinarstatus";
        }
    }

    public class ActivityRecord : PostData
    {
        public ActivityRecord()
        {
            this.Op = "getreclist";
        }
    }

    public class ActivityRecordPart : PostData
    {
        public ActivityRecordPart()
        {
            this.Op = "recpartlist";
        }
    }

    public class GenerateActivityRecord : PostData
    {
        public GenerateActivityRecord()
        {
            this.Op = "createmultirecord";
        }

        public MultiRecord Data { get; set; }

        public override string ToParamString()
        {
            var requiredParam = GetRequiredParam();

            return string.Format("{0}&data={1}", requiredParam, this.Data.ToParam());
        }
    }
}
