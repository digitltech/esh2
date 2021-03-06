using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace ESH
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapRoute(
                name: "sitemap",
                url: "sitemap.xml",
                defaults: new { controller = "Home", action = "Sitemap" }
            );
            routes.MapRoute("Robots.txt",
                "robots.txt",
                new { controller = "Home", action = "Robots" });
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "Portal.Controllers" }
            );
        }
    }
}
