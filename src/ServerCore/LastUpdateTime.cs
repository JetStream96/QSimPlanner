using System;
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace ServerCore
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
        public static void LoadFromFile(IHostingEnvironment env)
        {
            LoadFromFile(w => WestUtc = w, fileWest, env);
            LoadFromFile(e => EastUtc = e, fileEast, env);
        }

        // @NoThrow
        private static void LoadFromFile(Action<DateTime> setter, string file, IHostingEnvironment env)
        {
            var path = Util.MapPath(env, file);

            try
            {
                setter(DateTime.FromBinary(long.Parse(File.ReadAllText(path))));
            }
            catch (Exception e)
            {
                Shared.Logger.Log(e.ToString());
                setter(DateTime.UtcNow);
            }
        }

        // @NoThrow
        public static void SaveWest(IHostingEnvironment env)
        {
            Util.TryAndLogIfFail(() => Save(() => WestUtc, fileWest, env));
        }

        // @NoThrow
        public static void SaveEast(IHostingEnvironment env)
        {
            Util.TryAndLogIfFail(() => Save(() => EastUtc, fileEast, env));
        }

        // @Throws
        private static void Save(Func<DateTime> getter, string file, IHostingEnvironment env)
        {
            var path = Util.MapPath(env, file);
            Directory.CreateDirectory(Path.GetDirectoryName(path));
            File.WriteAllText(path, getter().ToBinary().ToString());
        }
    }
}