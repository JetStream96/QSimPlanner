using static QSP.RouteFinding.RouteFindingCore;
using System.Windows.Forms;
using System;
using QSP.RouteFinding.Airports;
using QSP.RouteFinding.AirwayStructure;

namespace QSP.RouteFinding.Data
{
    public static class DatabaseLoader
    {
        public static void LoadAllDB(string path)
        {
            //e.g. path can be "E:\Aviation\Projects\Integrated Flight Planner\AIRAC\Aerosoft Airbus X 1.22_later\Navigraph"

            // Import the texts in ats.txt into WptList.
            WptList = new WaypointList();

            WptList.ReadFixesFromFile(path + "\\waypoints.txt");
            WptList.ReadAtsFromFile(path + "\\ats.txt"); 
             
            try
            {
                AirportList = new AirportManager(path + "\\Airports.txt");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }
    }
}

