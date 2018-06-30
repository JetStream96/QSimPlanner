using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;

namespace Server.GoogleMap
{
    public static class Maps
    {
        public static string ApiKey;

        public static void LoadAll()
        {
            InteractiveMap.LoadTemplate();
            RouteDrawing.LoadTemplate();
            LoadApiKey();
        }

        private static void LoadApiKey()
        {
            var file = HostingEnvironment.MapPath("~/App_Data/gmap_api_key.txt");
            Func<string> load = () => File.ReadAllLines(file)[0];

#if DEBUG
            ApiKey = File.Exists(file) ? load() : "";
#else
            ApiKey = load();
#endif
        }
    }
}