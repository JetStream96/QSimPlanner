using QSP.AviationTools.Coordinates;
using QSP.RouteFinding.Airports;
using QSP.RouteFinding.Tracks.Common;
using QSP.RouteFinding.Tracks.Interaction;
using QSP.RouteFinding.Tracks.Nats.Utilities;
using System;
using System.Collections.Generic;

namespace QSP.RouteFinding.Tracks.Nats
{
    public class NatsParser : TrackParser<NorthAtlanticTrack>
    {
        private static readonly char[] DelimiterWords =
               { ' ', '\n', '\r', '\t' };

        private StatusRecorder statusRecorder;
        private AirportManager airportList;
        private NatsMessage message;

        public NatsParser(
            NatsMessage message,
            StatusRecorder statusRecorder,
            AirportManager airportList)
        {
            this.message = message;
            this.statusRecorder = statusRecorder;
            this.airportList = airportList;
        }

        public override List<NorthAtlanticTrack> Parse()
        {
            var NatTrackCollection = TryAddMessage(message.WestMessage);
            NatTrackCollection.AddRange(TryAddMessage(message.EastMessage));
            return NatTrackCollection;
        }

        private List<NorthAtlanticTrack> TryAddMessage(
            IndividualNatsMessage msg)
        {
            try
            {
                return ConvertToTracks(msg);
            }
            catch
            {
                var dir = msg.Direction == NatsDirection.East ?
                "eastbound" : "westbound";

                statusRecorder.AddEntry(
                    StatusRecorder.Severity.Caution,
                    $"Unable to interpret {dir} tracks.",
                    TrackType.Nats);

                return new List<NorthAtlanticTrack>();
            }
        }

        private static List<NorthAtlanticTrack> ConvertToTracks(
            IndividualNatsMessage msg)
        {
            char trkStartChar =
                msg.Direction == NatsDirection.West ? 'A' : 'N';

            var Message = msg.Message;
            var tracks = new List<NorthAtlanticTrack>();

            for (int i = trkStartChar; i < trkStartChar + 13; i++)
            {
                char id = (char)i;

                int j = Message.IndexOf($"\n{id} ");

                if (j < 0)
                {
                    continue;
                }

                j += 2;
                var newLinePos = Message.IndexOf('\n', j);

                var route = Message.Substring(j, newLinePos - j)
                    .Split(DelimiterWords,
                    StringSplitOptions.RemoveEmptyEntries);

                TryConvertNatsLatLon(route);

                tracks.Add(new NorthAtlanticTrack(
                    msg.Direction,
                    id.ToString(),
                    "",
                    "",
                    "",
                    new List<string>(route).AsReadOnly(),
                    Constants.CenterAtlantic));
            }

            return tracks;
        }

        private static void TryConvertNatsLatLon(string[] wpts)
        {
            for (int i = 0; i < wpts.Length; i++)
            {
                LatLon latLon;

                if (LatLonConverter.TryConvertNatsCoordinate(
                    wpts[i], out latLon))
                {
                    wpts[i] = latLon.AutoChooseFormat();
                }
            }
        }
    }
}
