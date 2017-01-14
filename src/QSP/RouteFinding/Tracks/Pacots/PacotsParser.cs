using System.Collections.Generic;
using QSP.RouteFinding.Tracks.Common;
using QSP.RouteFinding.Airports;

namespace QSP.RouteFinding.Tracks.Pacots
{
    public class PacotsParser : ITrackParser<PacificTrack>
    {
        private AirportManager airportList;
        private PacotsMessage message;

        public PacotsParser(ITrackMessage message, AirportManager airportList)
        {
            this.message = (PacotsMessage)message;
            this.airportList = airportList;
        }

        public List<PacificTrack> Parse()
        {
            var eastParser = new Eastbound.EastboundParser(airportList);
            var tracks = eastParser.Parse(message);

            foreach (var i in message.WestboundTracks)
            {
                tracks.Add(new WestTrackParser(i, airportList).Parse());
            }

            return tracks;
        }
    }
}
