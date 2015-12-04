using System.Net;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using Microsoft.AspNet.Identity;

using Zkly.Common.Log;

namespace Zkly.BLL.Message
{
    public class EmailService : IIdentityMessageService
    {
        public async Task SendAsync(IdentityMessage message)
        {
            try
            {
                SmtpClient client = new SmtpClient();
                var from = new MailAddress(((NetworkCredential)client.Credentials).UserName, "中科路演");
                MailAddress to = new MailAddress(message.Destination);
                var msg = new MailMessage(from, to);
                msg.Subject = message.Subject;
                msg.Body = message.Body;
                msg.IsBodyHtml = Regex.IsMatch(message.Body, @"<(?:""[^""]*""['""]*|'[^']*'['""]*|[^'"">])+>");
                await client.SendMailAsync(msg);
            }
            catch (SmtpException exception)
            {
                var msg = new StringBuilder()
                            .AppendLine("邮件发送失败！")
                            .AppendFormat("邮件标题：{0}\r\n", message.Subject)
                            .AppendFormat("邮件地址：{0}\r\n", message.Destination);
                Logger.Error(msg.ToString(), exception);
            }
        }
    }
}
