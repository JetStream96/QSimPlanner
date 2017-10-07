using LibraryExtension.Sets;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace ServerCore
{
    public static class HiddenFiles
    {
        // @Throws
        // If this fails, the server should not start.
        public static IReadOnlySet<string> LoadFromFileAndLog(IHostingEnvironment env)
        {
            var s = SharedData.GetInstance(env);

            try
            {
                return LoadFromFile(s);
            }
            catch (Exception e)
            {
                s.Logger.Log(e.ToString());
                throw;
            }
        }

        // @Throws
        public static IReadOnlySet<string> LoadFromFile(SharedData sd)
        {
            var root = XDocument.Load(sd.MapPath(SharedData.ConfigFile)).Root;
            var strings = root.Elements("hidden").Select(s => s.Value.ToLower());
            return new ReadOnlySet<string>(new HashSet<string>(strings));
        }
    }
}