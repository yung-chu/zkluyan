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
    public static class GlimpseSettings
    {
        private static NameValueCollection Settings
        {
            get
            {
                return ConfigurationManager.GetSection("glimpseSettings") as NameValueCollection;
            }
        }

        public static readonly string GlimpseDebugRole = Settings["GlimpseDebugRole"];
    }
}
