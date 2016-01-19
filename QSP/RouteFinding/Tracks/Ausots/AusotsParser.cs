using QSP.LibraryExtension;
using QSP.RouteFinding.Tracks.Common;
using QSP.RouteFinding.Tracks.Interaction;
using System.Collections.Generic;
using QSP.RouteFinding.Airports;
using QSP.RouteFinding.Tracks.Ausots.Utilities;

namespace QSP.RouteFinding.Tracks.Ausots
{
    // Parse AusotsMessage into a list of AusTracks.
    // The entire string is first split into several strings, which contains the information of one track.
    // Then IndividualAusotsParser is called to generate a AusTrack.
    //
    // If no track can be extracted from the AusotsMessage, a TrackParseException is thrown.
    // If an exception occurs in IndividualAusotsParser, StatusRecorder will record such event but other tracks will still be
    // processed. 

    public class AusotsParser : TrackParser<AusTrack>
    {
        private StatusRecorder statusRecorder;
        private AirportManager airportList;
        private string allTxt;
        private List<AusTrack> allTracks;
        
        public AusotsParser(AusotsMessage data, StatusRecorder statusRecorder, AirportManager airportList)
        {
            allTxt = data.AllText;
            this.statusRecorder = statusRecorder;
            this.airportList = airportList;
        }
        
        /// <exception cref="TrackParseException"></exception>
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

