using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Zkly.Web.Startup))]

namespace Zkly.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            ConfigureContainer(app);
        }
    }
}
