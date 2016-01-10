using QSP.LibraryExtension;
using QSP.RouteFinding.Tracks.Common;
using QSP.RouteFinding.Tracks.Interaction;
using System.Collections.Generic;
using QSP.RouteFinding.Airports;

namespace QSP.RouteFinding.Tracks.Ausots
{
    public class AusotsParser : TrackParser<AusTrack>
    {
        private StatusRecorder statusRecorder;
        private AirportManager airportList;
        private string allTxt;
        private List<AusTrack> allTracks;

        public AusotsParser(AusotsRawData data) : this(data, RouteFindingCore.TrackStatusRecorder, RouteFindingCore.AirportList) { }

        public AusotsParser(AusotsRawData data, StatusRecorder statusRecorder, AirportManager airportList)
        {
            allTxt = data.AllText;
            this.statusRecorder = statusRecorder;
            this.airportList = airportList;
        }

        public override List<AusTrack> Parse()
        {
            allTracks = new List<AusTrack>();
            var indices = allTxt.IndicesOf("TDM TRK");

            if (indices.Count < 2)
            {
                throw new TrackParseException("Failed to interpret Ausots message.");
            }

            fixLastEntry(indices);

            for (int i = 0; i <= indices.Count - 2; i++)
            {
                tryAddTrk(indices, i);
            }

            return allTracks;
        }

        private void fixLastEntry(List<int> item)
        {
            item.Add(allTxt.Length);
        }

        private void tryAddTrk(List<int> indices, int index)
        {
            try
            {
                var trk = new IndividualAusotsParser(allTxt.Substring(indices[index], indices[index + 1] - indices[index]),
                                                     airportList)
                              .Parse();

                if (trk != null)
                {
                    allTracks.Add(trk);
                }
            }
            catch
            {
                statusRecorder.AddEntry(StatusRecorder.Severity.Caution, "Unable to interpret one track.", TrackType.Ausots);
            }
        }
    }
}

