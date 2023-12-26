using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace MusicPortal
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            // Создание альбома
            routes.MapRoute(
                name: "CreateAlbum",
                url: "groups/{id}/create",
                defaults: new { controller = "Portal", action = "CreateAlbum", id = UrlParameter.Optional }
            );
            // Создание песни
            routes.MapRoute(
                name: "CreateSong",
                url: "songs/{id}/create",
                defaults: new { controller = "Portal", action = "CreateSong", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Portal", action = "ListOfGroups", id = UrlParameter.Optional }
            );
        }
    }
}
