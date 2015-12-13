using System.IO;
using QSP.RouteFinding.Containers;
using static QSP.RouteFinding.RouteFindingCore;
using System.Windows.Forms;
using System;
using QSP.RouteFinding.Airports;

namespace QSP.RouteFinding.Data
{
    public static class DatabaseLoader
    {
        public static void LoadAllDB(string path)
        {
            //e.g. path can be "E:\Aviation\Projects\Integrated Flight Planner\AIRAC\Aerosoft Airbus X 1.22_later\Navigraph"

            // Import the texts in ats.txt into WptList.
            WptList = new TrackedWptList();

            WptList.ReadFixesFromFile(path + "\\waypoints.txt");            
            new AtsFileLoader(WptList).ReadAtsFromFile(path + "\\ats.txt");

            WptFinder = WptList.GenerateSearchGrids();

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

