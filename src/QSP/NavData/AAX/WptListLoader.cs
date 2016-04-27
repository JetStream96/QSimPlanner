using QSP.RouteFinding.AirwayStructure;

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
        
        /// <exception cref="LoadWaypointFileException"></exception>
        public WaypointList LoadFromFile()
        {
            var wptList = new WaypointList();

            new FixesLoader(wptList).ReadFromFile(waypointsFilePath);
            new AtsFileLoader(wptList).ReadFromFile(atsFilePath);

            return wptList;
        }
    }
}
