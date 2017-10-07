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
            try
            {
                return LoadFromFile(env);
            }
            catch (Exception e)
            {
                Shared.Logger.Log(e.ToString());
                throw;
            }
        }

        // @Throws
        public static IReadOnlySet<string> LoadFromFile(IHostingEnvironment env)
        {
            var root = XDocument.Load(Util.MapPath(env, Shared.ConfigFile)).Root;
            var strings = root.Elements("hidden").Select(s => s.Value.ToLower());
            return new ReadOnlySet<string>(new HashSet<string>(strings));
        }
    }
}