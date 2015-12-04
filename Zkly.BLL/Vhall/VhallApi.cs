using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Zkly.Common.Config;
using Zkly.Common.Serialize;
using Zkly.Common.Utils;

namespace Zkly.BLL.Vhall
{
    public class VhallApi : IVhallApi
    {
        public ResponseData CreateActivity(CreateActivity postData)
        {
            var response = this.GetReposponse(postData.ToParamString());

            return response;
        }

        public ResponseData DeleteActivity(DeleteActivity postData)
        {
            string param = postData.GetRequiredParam();

            var response = this.GetReposponse(param);

            return response;
        }

        public ResponseData StartActivity(StartActivity postData)
        {
            var response = this.GetReposponse(postData.ToParamString());

            return response;
        }

        public ResponseData EndActivity(EndActivity postData)
        {
            var response = this.GetReposponse(postData.ToParamString());

            return response;
        }

        public ResponseData UpdateActivity(UpdateActivity postData)
        {
            var response = this.GetReposponse(postData.ToParamString());

            return response;
        }

        public ResponseData GetActivityStatus(ActivityStatus postData)
        {
            return this.GetReposponse(postData.ToParamString());
        }

        public ResponseData GetRecordList(ActivityRecord postData)
        {
            return this.GetReposponse(postData.ToParamString());
        }

        public ResponseData GetRecordPart(ActivityRecordPart postData)
        {
            return this.GetReposponse(postData.ToParamString());
        }
        
        public ResponseData GenerateRecord(GenerateActivityRecord postData)
        {
            return this.GetReposponse(postData.ToParamString());
        }

        private ResponseData GetReposponse(string data)
        {
            var uri = VhallSettings.ApiUri;

            var client = new WebClient();
            client.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
            string result = client.UploadString(uri, "POST", data);

            return JsonSerializer.Deserialize<ResponseData>(result);
        }
    }
}
