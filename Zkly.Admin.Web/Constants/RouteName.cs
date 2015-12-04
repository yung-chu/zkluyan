namespace Zkly.Admin.Web.Constants
{
    public static class RouteName
    {
        public const string Error = "Error";

        public const string Home = "Home";
        public const string HomeGetAbout = ControllerName.Home + "GetAbout";
        public const string HomeGetContact = ControllerName.Home + "GetContact";
        public const string HomeGetIndex = ControllerName.Home + "GetIndex";
        public const string HomeRobotsText = ControllerName.Home + "RobotsText";
        public const string HomeSitemapXml = ControllerName.Home + "SitemapXml";

        public const string AccountGetLogin = ControllerName.Account + "GetLogin";
        public const string AccountPostLogin = ControllerName.Account + "PostLogin";
    }
}