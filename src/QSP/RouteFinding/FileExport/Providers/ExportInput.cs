using QSP.LibraryExtension;
using QSP.RouteFinding.Airports;
using QSP.RouteFinding.AirwayStructure;
using QSP.RouteFinding.Navaids;
using QSP.RouteFinding.Routes;
using QSP.WindAloft;

namespace QSP.RouteFinding.FileExport.Providers
{
    public class ExportInput
    {
        public Route Route { get; set; }
        public AirportManager Airports { get; set; }
        public WaypointList Waypoints { get; set; }
        public MultiMap<string, Navaid> Navaids { get; set; }
        public IWxTableCollection WindTables { get; set; }
    }
}
