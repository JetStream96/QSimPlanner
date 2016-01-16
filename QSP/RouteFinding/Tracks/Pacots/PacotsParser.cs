using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QSP.LibraryExtension;
using QSP.RouteFinding.Tracks.Common;
using QSP.RouteFinding.Tracks.Interaction;
using QSP.RouteFinding.Airports;

namespace QSP.RouteFinding.Tracks.Pacots
{
    public class PacotsParser : TrackParser<PacificTrack>
    {
        private StatusRecorder statusRecorder;
        private AirportManager airportList;
        private PacotsMessage message;
        private List<PacificTrack> allTracks;

        public PacotsParser(PacotsMessage message, StatusRecorder statusRecorder, AirportManager airportList)
        {
            this.message = message;
            this.statusRecorder = statusRecorder;
            this.airportList = airportList;
        }

        public override List<PacificTrack> Parse()
        {
            var tracks = new EastTracksParser(airportList).CreateEastboundTracks(message);

            foreach (var i in message.WestboundTracks)
            {
                tracks.Add(new WestTrackParser(i,airportList).Parse());
            }
            return tracks;            
        }
    }
}
