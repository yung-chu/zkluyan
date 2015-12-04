using System.Collections.Specialized;
using System.Configuration;
using Zkly.Common.Extension;

namespace Zkly.Common.Config
{
    public static class SmsSettings
    {
        private static NameValueCollection Settings
        {
            get
            {
                return ConfigurationManager.GetSection("SmsSettings") as NameValueCollection;
            }
        }

        public static readonly string RegisterId = Settings["RegisterId"].TrimNull();

        public static readonly string Password = Settings["Password"].TrimNull();

        public static readonly string Signature = Settings["Signature"].TrimNull();
    }
}
