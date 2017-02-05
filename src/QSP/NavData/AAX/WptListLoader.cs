using QSP.RouteFinding.AirwayStructure;
using QSP.RouteFinding.Containers.CountryCode;
using System.IO;
using QSP.Utilities;

namespace QSP.NavData.AAX
{
    public class WptListLoader
    {
        private readonly string navDataLocation;

        public WptListLoader(string navDataLocation)
        {
            this.navDataLocation = navDataLocation;
        }

        private string waypointsFilePath => Path.Combine(navDataLocation, "waypoints.txt");

        private string atsFilePath => Path.Combine(navDataLocation, "ats.txt");

        /// <exception cref="WaypointFileReadException"></exception>
        /// <exception cref="LoadCountryNamesException"></exception>
        public LoadResult LoadFromFile()
        {
            var wptList = new WaypointList();
            var countryCodes = new FixesLoader(wptList, Logger.Instance)
                .ReadFromFile(waypointsFilePath);

            var err = AtsFileLoader.ReadFromFile(wptList, atsFilePath);
            if (err != null) LoggerInstance.Log(err);

            var countryFullNames = FullNamesLoader.Load();
            var countryManager = new CountryCodeManager(countryCodes, countryFullNames);

            return new LoadResult()
            {
                WptList = wptList,
                CountryCodes = countryManager
            };
        }

        public struct LoadResult
        {
            public WaypointList WptList;
            public CountryCodeManager CountryCodes;
        }
    }
}
