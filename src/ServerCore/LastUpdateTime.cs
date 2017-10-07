using System;
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace ServerCore
{
    //TODO: To class.
    public static class LastUpdateTime
    {
        private static readonly string fileEast = "~/nats/LastUpdateTimeEast.txt";
        private static readonly string fileWest = "~/nats/LastUpdateTimeWest.txt";

        public static DateTime WestUtc { get; set; }
        public static DateTime EastUtc { get; set; }

        // @NoThrow
        // If the file cannot be parsed correctly or is not found,
        // the current UTC time is set.
        public static void LoadFromFile(SharedData s)
        {
            LoadFromFile(w => WestUtc = w, fileWest, s);
            LoadFromFile(e => EastUtc = e, fileEast, s);
        }

        // @NoThrow
        private static void LoadFromFile(Action<DateTime> setter, string file, SharedData s)
        {
            var path = s.MapPath(file);

            try
            {
                setter(DateTime.FromBinary(long.Parse(File.ReadAllText(path))));
            }
            catch (Exception e)
            {
                s.Logger.Log(e.ToString());
                setter(DateTime.UtcNow);
            }
        }

        // @NoThrow
        public static void SaveWest(SharedData shared)
        {
            Util.TryAndLogIfFail(shared, () => Save(() => WestUtc, fileWest, shared));
        }

        // @NoThrow
        public static void SaveEast(SharedData shared)
        {
            Util.TryAndLogIfFail(shared, () => Save(() => EastUtc, fileEast, shared));
        }

        // @Throws
        private static void Save(Func<DateTime> getter, string file, SharedData shared)
        {
            var path = shared.MapPath( file);
            Directory.CreateDirectory(Path.GetDirectoryName(path));
            File.WriteAllText(path, getter().ToBinary().ToString());
        }
    }
}