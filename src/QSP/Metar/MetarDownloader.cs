using System.Net;

namespace QSP.Metar
{
    public static class MetarDownloader
    {
        private const string metarUrl = "http://tgftp.nws.noaa.gov/data/observations/metar/stations/";
        private const string tafUrl = "http://tgftp.nws.noaa.gov/data/forecasts/taf/stations/";

        private static string TrimIcao(string icao)
        {
            return icao.Trim().ToUpper();
        }
        
        public static string GetMetar(string icao)
        {
            using (var client = new WebClient())
            {
                return client.DownloadString(metarUrl + TrimIcao(icao) + ".TXT");
            }
        }

        public static string GetTaf(string icao)
        {
            using (var client = new WebClient())
            {
                return client.DownloadString(tafUrl + TrimIcao(icao) + ".TXT");
            }
        }

        /// <summary>
        /// Returns whether downloading metar is successful. 
        /// </summary>
        public static bool TryGetMetar(string icao, out string metar)
        {
            try
            {
                metar = GetMetar(icao);
                return true;
            }
            catch
            {
                metar = null;
                return false;
            }
        }

        /// <summary>
        /// Returns whether downloading TAF is successful. 
        /// </summary>
        public static bool TryGetTaf(string icao, out string taf)
        {
            try
            {
                taf = GetTaf(icao);
                return true;
            }
            catch
            {
                taf = null;
                return false;
            }
        }

        /// <summary>
        /// Returns null if failed.
        /// </summary>
        public static string GetMetarTaf(string icao)
        {
            if (TryGetMetar(icao, out var metar) &&
                TryGetTaf(icao, out var taf))
            {
                return metar + "\n\n" + taf;
            }

            return null;
        }

        public static string TryGetMetar(string icao)
        {
            try
            {
                return GetMetar(icao);
            }
            catch
            {
                return "Downloading Metar failed.";
            }
        }

        public static string TryGetTaf(string icao)
        {
            try
            {
                return GetTaf(icao);
            }
            catch
            {
                return "Downloading TAF failed.";
            }
        }

        public static string TryGetMetarTaf(string icao)
        {
            return TryGetMetar(icao) + "\n\n" + TryGetTaf(icao);
        }
    }
}
