using System.Collections.Generic;

using Newtonsoft.Json;

namespace Zkly.BLL.Vhall
{
    public class VhallUtil
    {
        public static List<Record> GetRecords(string webinarId)
        {
            var vhallApi = new VhallApi();
            var record = vhallApi.GetRecordList(new ActivityRecord { WebinarId = webinarId });
            List<Record> recs = JsonConvert.DeserializeObject<List<Record>>(record.Data);

            return recs;
        }

        public static string GetRecordUrl(string webinarId)
        {
            List<Record> recs = GetRecords(webinarId);

            if (recs != null && recs.Count > 0)
            {
                return recs[0].Url;
            }

            return string.Empty;
        }

        public static bool HasRecording(string webinarId)
        {
            var recordUrl = GetRecordUrl(webinarId);

            return !string.IsNullOrEmpty(recordUrl);
        }
    }
}
