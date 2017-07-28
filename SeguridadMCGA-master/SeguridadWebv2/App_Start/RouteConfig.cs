using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace SeguridadWebv2
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Account", action = "Login", id = UrlParameter.Optional }
            );

            routes.MapRoute(
               name: "DoCheckout",
               url: "MercadoPago/DoCheckout",
               defaults: new { controller = "MercadoPago", action = "DoCheckout", id = UrlParameter.Optional }
               );

            routes.MapRoute(
                name: "CheckoutStatus",
                url: "MercadoPago/CheckoutStatus",
                defaults: new { controller = "MercadoPago", action = "CheckoutStatus" }
                );

        }
    }
}
