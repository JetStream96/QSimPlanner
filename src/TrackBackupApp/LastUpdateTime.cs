using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using QSP.Utilities;

namespace TrackBackupApp
{
    public static class LastUpdateTime
    {
        private static readonly string fileEast = "~/nats/LastUpdateTimeEast.txt";
        private static readonly string fileWest = "~/nats/LastUpdateTimeWest.txt";

        public static DateTime WestUtc { get; set; }
        public static DateTime EastUtc { get; set; }

        // @NoThrow
        // If the file cannot be parsed correctly or is not found,
        // the current UTC time is set.
        public static void LoadFromFile()
        {
            LoadFromFile(w => WestUtc = w, fileWest);
            LoadFromFile(e => EastUtc = e, fileEast);
        }

        // @NoThrow
        private static void LoadFromFile(Action<DateTime> setter, string file)
        {
            var path = HostingEnvironment.MapPath(file);

            try
            {
                setter(DateTime.FromBinary(long.Parse(File.ReadAllText(path))));
            }
            catch (Exception e)
            {
                Global.WriteToLog(e.ToString());
                setter(DateTime.UtcNow);
            }
        }

        // @NoThrow
        public static void Save()
        {
            Global.TryAndLogIfFail(() => Save(() => WestUtc, fileWest));
            Global.TryAndLogIfFail(() => Save(() => EastUtc, fileEast));
        }

        // @Throws
        private static void Save(Func<DateTime> getter, string file)
        {
            var path = HostingEnvironment.MapPath(file);
            Directory.CreateDirectory(Path.GetDirectoryName(path));
            File.WriteAllText(path, getter().ToBinary().ToString());
        }
    }
}