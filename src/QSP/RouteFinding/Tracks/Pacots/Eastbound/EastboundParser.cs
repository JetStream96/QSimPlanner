using QSP.RouteFinding.Airports;
using QSP.RouteFinding.Tracks.Common.TDM.Parser;
using System;
using System.Collections.Generic;

namespace QSP.RouteFinding.Tracks.Pacots.Eastbound
{
    public class EastboundParser
    {
        private AirportManager airportList;

        public EastboundParser(AirportManager airportList)
        {
            this.airportList = airportList;
        }

        public List<PacificTrack> Parse(PacotsMessage item)
        {
            var result = new List<PacificTrack>();

            foreach (var i in item.EastboundTracks)
            {
                result.AddRange(CreateEastboundTracks(i));
            }

            return result;
        }

        private PacificTrack[] CreateEastboundTracks(string item)
        {
            var timeInfo = TrackValidPeriod.GetValidPeriod(item);
            var tracksStr = new Splitter(item).Split();
            PacificTrack[] result = new PacificTrack[tracksStr.Count];

            for (int i = 0; i < result.Length; i++)
            {
                var trk = new Interpreter(tracksStr[i]).Parse();
                var mainRoute = new MainRouteInterpreter(trk.FlexRoute).Convert();

                var connectionRoutes =
                    new ConnectionRouteInterpreter(
                        mainRoute,
                        new ConnectionRouteSeperator(trk.ConnectionRoute).Seperate().AsReadOnly(),
                        airportList)
                    .Convert();
                
                result[i] = new PacificTrack(PacotDirection.Eastbound,
                                             trk.ID.ToString(),
                                             timeInfo.Start,
                                             timeInfo.End,
                                             trk.Remark,
                                             Array.AsReadOnly(mainRoute),
                                             connectionRoutes.RouteFrom.AsReadOnly(),
                                             connectionRoutes.RouteTo.AsReadOnly(),
                                             Constants.JAPAN_LATLON);
            }
            return result;
        }
    }
}
