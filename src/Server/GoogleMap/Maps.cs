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

            ApiKey = File.ReadAllLines(HostingEnvironment.MapPath(
                "~/App_Data/gmap_api_key.txt"))[0];
        }
    }
}