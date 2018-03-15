using QSP.LibraryExtension;
using QSP.AviationTools;
using QSP.Metar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QSP.RouteFinding.Airports
{
    public static class RunwaySelect
    {
        /// <summary>
        /// Selects a runway with most headwind component. Returns its ident.
        /// </summary>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="InvalidOperationException">rwys is empty</exception>
        public static string MaxHeadwind(this IEnumerable<IRwyData> rwys, double windHeading)
        {
            return rwys
                .Where(r => r.Heading.IsDouble())
                .MaxBy(r => ConversionTools.HeadwindComponent(
                    double.Parse(r.Heading), windHeading, 1.0))
                .RwyIdent;
        }

        /// <summary>
        /// Downloads the weather and selects the runway with max headwind component.
        /// If downloading timed out or downloaded data cannot be parsed, returns
        /// false for 'success'. Timeout is 1000 ms.
        /// </summary>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="InvalidOperationException">rwys is empty</exception>
        public static async Task<(bool success, string rwy)> SelectRwyBasedOnWind(
          IEnumerable<IRwyData> rwys, string icao)
        {
            var (timedout, metarDownloadResult) = 
                await LibraryExtension.Tasks.Util.RunAsyncWithTimeout(
                    () => MetarDownloader.DownloadMetar(icao), 1000);

            if (timedout) return (false, null);
            var (downloaded, metar) = metarDownloadResult;
            if (!downloaded) return (false, null);
            var wind = ParaExtractor.GetWind(metar);
            if (!wind.HasValue) return (false, null);
            return (true, rwys.MaxHeadwind(wind.Value.Direction));
        }

        /// <summary>
        /// If the ICAO code is in airportList, and the number of runways is at least 1,
        /// we set the list of runways. Then we attempt to download the matar and 
        /// select a runway with maximum headwind component.
        /// 
        /// If ICAO is not found in airportList, this method clears the list of runways.
        /// If we cannot successfully find the runway with max headwind component, the 
        /// selected runway is set arbitrarily.
        /// 
        /// GetIcao should always return a non-null result. SetRunways and SetSelectedRunway
        /// should not throw with non-null inputs.
        /// </summary>
        public static async Task UpdateRunways(
            Func<string> GetIcao,
            Action<IEnumerable<string>> SetRunways,
            Action<string> SetSelectedRunway,
            AirportManager airportList)
        {
            var icaoBeforeAwait = GetIcao();
            var rwyList = airportList.RwyIdents(icaoBeforeAwait)?.ToList();

            if (rwyList != null && rwyList.Count > 0)
            {
                SetRunways(rwyList);
                var runways = airportList[icaoBeforeAwait].Rwys;
                var (success, rwy) = await SelectRwyBasedOnWind(
                    runways, icaoBeforeAwait);

                // Only choose runway based on weather if the ICAO does not change
                // during the await.
                SetSelectedRunway((success && icaoBeforeAwait == GetIcao()) ? rwy : rwyList[0]);
            }
            else
            {
                SetRunways(new string[0]);
            }
        }
    }
}
