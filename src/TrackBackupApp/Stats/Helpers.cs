using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;
using System.Xml.Linq;
using static QSP.LibraryExtension.Tasks.Util;
using System.Threading;

namespace TrackBackupApp.Stats
{
    public static class Helpers
    {
        public const string FilePath = "~/stats.xml";

        // @Throws
        public static Statistics LoadOrGenerateFile(string path = FilePath)
        {
            if (!File.Exists(HostingEnvironment.MapPath(path)))
            {
                var stats = new Statistics();
                SaveToFile(stats, path);
                return stats;
            }

            return LoadFromFile(path);
        }

        // @NoThrow
        public static async Task SavePeriodic(Statistics stats, int periodMs)
        {
            await RunPeriodicAsync(() =>
            {
                try
                {
                    SaveToFile(stats);
                }
                catch (Exception e)
                {
                    Shared.Logger.Log(e.ToString());
                }
            }, new TimeSpan(0, 0, 0, 0, periodMs), new CancellationToken());
        }

        // @Throws
        public static Statistics LoadFromFile(string path = FilePath)
        {
            var doc = XDocument.Load(HostingEnvironment.MapPath(path));
            return new Statistics.Serializer().Deserialize(doc.Root);
        }

        // @Throws
        public static void SaveToFile(Statistics stats, string path = FilePath)
        {
            var xElem = new Statistics.Serializer().Serialize(stats, "root");
            File.WriteAllText(HostingEnvironment.MapPath(path), xElem.ToString());
        }
    }
}