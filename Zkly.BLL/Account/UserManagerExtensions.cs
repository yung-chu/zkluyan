using System;
using AsyncHelper = Zkly.Common.Extension.AsyncHelper;

namespace Zkly.BLL.Account
{
    public static class UserManagerExtensions
    {
        public static void SendAllMessages(this ApplicationUserManager manager, string userId, string subject, string body)
        {
            if (manager == null)
            {
                throw new ArgumentNullException("manager");
            }

            AsyncHelper.RunSync(() => manager.SendAllMessagesAsync(userId, subject, body));
        }

        public static void SendSystemMessage(this ApplicationUserManager manager, string userId, string subject, string body)
        {
            if (manager == null)
            {
                throw new ArgumentNullException("manager");
            }

            AsyncHelper.RunSync(() => manager.SendSystemMessageAsync(userId, subject, body));
        }

        public static void SendSms(this ApplicationUserManager manager, string userId, string subject, string body)
        {
            if (manager == null)
            {
                throw new ArgumentNullException("manager");
            }

            AsyncHelper.RunSync(() => manager.SendSmsAsync(userId, subject, body));
        }
    }
}
