using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Zkly.Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            // Imprive SEO by stopping duplicate URL's due to case or trailing slashes.
            // See http://googlewebmastercentral.blogspot.co.uk/2010/04/to-slash-or-not-to-slash.html
            routes.AppendTrailingSlash = true;
            routes.LowercaseUrls = true;

            // IgnoreRoute - Tell the routing system to ignore certain routes for better performance.
            // Ignore .axd files.
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            // Ignore everything in the Content folder.
            routes.IgnoreRoute("Content/{*pathInfo}");

            // Ignore everything in the Scripts folder.
            routes.IgnoreRoute("Scripts/{*pathInfo}");

            // Enable attribute routing.
            routes.MapMvcAttributeRoutes();
            
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional });
        }
    }
}
