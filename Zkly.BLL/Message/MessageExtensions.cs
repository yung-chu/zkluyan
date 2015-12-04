using System;
using Microsoft.AspNet.Identity;
using AsyncHelper = Zkly.Common.Extension.AsyncHelper;

namespace Zkly.BLL.Message
{
    public static class MessageExtensions
    {
        public static void Send(this IIdentityMessageService service, IdentityMessage message)
        {
            if (service == null)
            {
                throw new ArgumentNullException("service");
            }

            AsyncHelper.RunSync(() => service.SendAsync(message));
        }
    }
}
