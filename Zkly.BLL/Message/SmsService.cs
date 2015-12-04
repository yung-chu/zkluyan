using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Zkly.Common.Log;
using Zkly.Common.SMS;

namespace Zkly.BLL.Message
{
    public class SmsService : IIdentityMessageService
    {
        public async Task SendAsync(IdentityMessage message)
        {
            try
            {
                var client = new SmsClient();
                var numbers = message.Destination.Split(',', ';');
                var result = await client.SendAsync(message.Body, numbers);
                if (!result.Success)
                {
                    var msg = new StringBuilder()
                                .AppendLine("短信发送失败！")
                                .AppendFormat("短信号码：{0}\r\n", message.Destination)
                                .AppendFormat("短信内容：{0}\r\n", message.Body)
                                .AppendFormat("错误编号：{0}\r\n", result.Code)
                                .AppendFormat("错误描述：{0}\r\n", result.Message);
                    Logger.Error(msg.ToString());
                }
            }
            catch (Exception exception)
            {
                var msg = new StringBuilder()
                            .AppendLine("短信发送异常！")
                            .AppendFormat("短信号码：{0}\r\n", message.Destination)
                            .AppendFormat("短信内容：{0}\r\n", message.Body);
                Logger.Error(msg.ToString(), exception);
            }
        }
    }
}
