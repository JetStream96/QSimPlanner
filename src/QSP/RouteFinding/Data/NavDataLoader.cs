using static QSP.RouteFinding.RouteFindingCore;
using System.Windows.Forms;
using System;
using QSP.RouteFinding.Airports;
using QSP.RouteFinding.AirwayStructure;

namespace QSP.RouteFinding.Data
{
    public class NavDataLoader
    {
        private string navDataLocation;

        //e.g. path can be "E:\Aviation\Projects\Integrated Flight Planner\AIRAC\Aerosoft Airbus X 1.22_later\Navigraph"
        public NavDataLoader(string navDataLocation)
        {
            this.navDataLocation = navDataLocation;
        }

        public void LoadAllData()
        {
            // Import the texts in ats.txt into WptList.
            WptList = new WaypointList();

            WptList.ReadFixesFromFile(waypointsFilePath());
            WptList.ReadAtsFromFile(atsFilePath());

            try
            {
                AirportList = new AirportManager(
                                   new FileLoader(airportsFilePath())
                                   .LoadFromFile());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private string waypointsFilePath()
        {
            return navDataLocation + @"\waypoints.txt";
        }

        private string atsFilePath()
        {
            return navDataLocation + @"\ats.txt";
        }

        private string airportsFilePath()
        {
            return navDataLocation + @"\Airports.txt";
        }

    }
}

