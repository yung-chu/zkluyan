using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zkly.Common.Extension;

namespace Zkly.Common.Config
{
    public static class VhallSettings
    {
        private static NameValueCollection Settings
        {
            get
            {
                return ConfigurationManager.GetSection("vhallSettings") as NameValueCollection;
            }
        }

        public static readonly string FtpServer = Settings["FtpServer"].TrimNull();

        public static readonly string FtpUserName = Settings["FtpUserName"].TrimNull();

        public static readonly string FtpPassword = Settings["FtpPassword"].TrimNull();

        public static readonly string ApiUri = Settings["ApiUri"].TrimNull();

        public static readonly string ApiUid = Settings["ApiUid"].TrimNull();

        public static readonly string ApiUkey = Settings["ApiUkey"].TrimNull();

        public static readonly string WebinarUrl = Settings["WebinarUrl"].TrimNull();

        public static readonly string WebinarGuestUrl = Settings["WebinarGuestUrl"].TrimNull();
    }
}
