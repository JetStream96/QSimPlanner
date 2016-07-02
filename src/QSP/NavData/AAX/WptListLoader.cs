using QSP.RouteFinding.AirwayStructure;
using QSP.RouteFinding.Containers.CountryCode;

namespace QSP.NavData.AAX
{
    public class WptListLoader
    {
        private string navDataLocation;

        public WptListLoader(string navDataLocation)
        {
            this.navDataLocation = navDataLocation;
        }

        private string waypointsFilePath
        {
            get
            {
                return navDataLocation + @"\waypoints.txt";
            }
        }

        private string atsFilePath
        {
            get
            {
                return navDataLocation + @"\ats.txt";
            }
        }

        /// <exception cref="WaypointFileReadException"></exception>
        public LoadResult LoadFromFile()
        {
            var wptList = new WaypointList();

            var countryCodes =
                new FixesLoader(wptList).ReadFromFile(waypointsFilePath);

            new AtsFileLoader(wptList).ReadFromFile(atsFilePath);

            var countryFullNames = FullNamesLoader.Load();
            var countryManager = new CountryCodeManager(
                countryCodes, countryFullNames);

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
