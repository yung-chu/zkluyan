using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zkly.BLL.Vhall
{
    public interface IVhallApi
    {
        ResponseData CreateActivity(CreateActivity postData);

        ResponseData DeleteActivity(DeleteActivity postData);

        ResponseData StartActivity(StartActivity postData);

        ResponseData EndActivity(EndActivity postData);

        ResponseData UpdateActivity(UpdateActivity postData);

        ResponseData GetActivityStatus(ActivityStatus postData);

        ResponseData GetRecordList(ActivityRecord postData);

        ResponseData GetRecordPart(ActivityRecordPart postData);

        ResponseData GenerateRecord(GenerateActivityRecord postData);
    }
}
