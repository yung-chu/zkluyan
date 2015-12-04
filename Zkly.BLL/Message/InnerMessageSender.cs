using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zkly.BLL.Repositories;

namespace Zkly.BLL.Message
{
    public class InnerMessageSender : IMessageSender
    {
        private MessageRepository messageRepository = new MessageRepository();

        public void Send(MessageInfo messageInfo)
        {
            var message = new Zkly.DAL.Models.Message
            {
                UserId = messageInfo.UserId,
                To = messageInfo.To,
                Body = messageInfo.Body,
                Subject = messageInfo.Subject,
                UpdateTime = DateTime.Now
            };

            this.messageRepository.AddMessage(message);
        }
    }
}
