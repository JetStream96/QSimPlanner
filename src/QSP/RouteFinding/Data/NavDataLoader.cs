using QSP.NavData.AAX;
using QSP.RouteFinding.Airports;
using QSP.RouteFinding.AirwayStructure;
using System;
using System.Windows.Forms;
using static QSP.RouteFinding.RouteFindingCore;

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
                                   new AirportDataLoader(airportsFilePath())
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

