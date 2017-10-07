using LibraryExtension.Sets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Hosting;
using System.Xml.Linq;

namespace ServerCore
{
    public static class HiddenFiles
    {
        // @Throws
        // If this fails, the server should not start.
        public static IReadOnlySet<string> LoadFromFileAndLog()
        {
            try
            {
                return LoadFromFile();
            }
            catch (Exception e)
            {
                Shared.Logger.Log(e.ToString());
                throw;
            }
        }

        // @Throws
        public static IReadOnlySet<string> LoadFromFile()
        {
            var root = XDocument.Load(HostingEnvironment.MapPath(Shared.ConfigFile)).Root;
            var strings = root.Elements("hidden").Select(s => s.Value.ToLower());
            return new ReadOnlySet<string>(new HashSet<string>(strings));
        }
    }
}