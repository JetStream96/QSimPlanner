using System;
using QSP.RouteFinding.Tracks.Common;
using QSP.RouteFinding.Tracks.Interaction;
using System.Collections.Generic;
using QSP.RouteFinding.Airports;
using QSP.RouteFinding.Tracks.Ausots.Utilities;

namespace QSP.RouteFinding.Tracks.Ausots
{
    // Parse AusotsMessage into a list of AusTracks.
    // The entire string is first split into several strings, 
    // which contains the information of one track.
    // Then IndividualAusotsParser is called to generate a AusTrack.
    //
    // If no track can be extracted from the AusotsMessage, 
    // a TrackParseException is thrown.
    // If an exception occurs in IndividualAusotsParser, StatusRecorder will 
    // record such event but other tracks will still be
    // processed. 

    public class AusotsParser : TrackParser<AusTrack>
    {
        private StatusRecorder statusRecorder;
        private AirportManager airportList;
        private string allTxt;
        private List<AusTrack> allTracks;

        public AusotsParser(
            AusotsMessage data,
            StatusRecorder statusRecorder,
            AirportManager airportList)
        {
            allTxt = data.AllText;
            this.statusRecorder = statusRecorder;
            this.airportList = airportList;
        }

        /// <exception cref="Exception"></exception>
        public override List<AusTrack> Parse()
        {
            allTracks = new List<AusTrack>();
            var msgs = MessageSplitter.Split(allTxt);

            if (msgs.Count == 0)
            {
                throw new Exception("Failed to interpret AUSOTS message.");
            }
            
            foreach (var i in msgs)
            {
                TryAddTrk(i);
            }

            return allTracks;
        }

        private void TryAddTrk(string msg)
        {
            try
            {
                var trk = new IndividualAusotsParser(msg, airportList).Parse();

                if (trk != null)
                {
                    allTracks.Add(trk);
                }
            }
            catch
            {
                statusRecorder.AddEntry(
                    StatusRecorder.Severity.Caution,
                    "Unable to interpret one track.",
                    TrackType.Ausots);
            }
        }
    }
}
