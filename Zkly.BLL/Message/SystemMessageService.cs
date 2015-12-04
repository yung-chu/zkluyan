using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Zkly.BLL.Repositories;
using Zkly.Common.Log;

namespace Zkly.BLL.Message
{
    public class SystemMessageService : IIdentityMessageService
    {
        private readonly MessageRepository _messageRepository = new MessageRepository();

        public async Task SendAsync(IdentityMessage message)
        {
            try
            {
                var data = new DAL.Models.Message
                {
                    UserId = message.Destination,
                    Body = message.Body,
                    Subject = message.Subject,
                    UpdateTime = DateTime.Now
                };

                await _messageRepository.AddMessageAysnc(data);
            }
            catch (Exception exception)
            {
                var msg = new StringBuilder()
                            .AppendLine("消息保存失败！")
                            .AppendFormat("用户ID：{0}\r\n", message.Destination)
                            .AppendFormat("消息标题：{0}\r\n", message.Subject)
                            .AppendFormat("消息内容：{0}\r\n", message.Body);
                Logger.Error(msg.ToString(), exception);
            }
        }
    }
}
