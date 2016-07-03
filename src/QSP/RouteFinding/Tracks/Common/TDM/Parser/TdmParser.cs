using QSP.LibraryExtension;
using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace QSP.RouteFinding.Tracks.Common.TDM.Parser
{
    // Parse the string which represents a single track, 
    // into a temporary format.
    //
    // Sample input:
    // TDM TRK MY15 151112233001 
    // 1511122330 1511131400 
    // JAMOR IBABI LEC CALAR OOD 25S133E LEESA 20S128E 15S122E ATMAP 
    // RTS/YMML ML H164 JAMOR 
    // RMK/AUSOTS GROUP A
    //
    // Format:
    // TDM TRK [Ident] 
    // [TimeStart] [TimeEnd]
    // [Main Route]
    // [Main Route continued]
    // RTS/[RouteFrom/To 1]
    // [RouteFrom/To 2]
    // [RouteFrom/To 3]
    // RMK/[Remarks]
    // [Remarks continued]
    //
    //    
    // Assumptions:
    // (1) The word after "TDM TRK" is the track ident (MY15 in above example).
    //
    // (2) The next line contains two time stamps, indicating TimeStart 
    //     and TimeEnd respectively.
    //
    // (3) The main route starts from the next line, to:
    //        a. The char before "RTS/" if exists.
    //        b. The char before "RMK/" if "RTS/" does not exist 
    //           but "RMK/" exists.
    //        c. The last char of the string if neither "RMK/" 
    //           nor "RTS/" exists.
    //
    // (4) After main route, the connecting routes contains the part 
    //     after "RTS/", to:
    //        a. The char before "RMK/" if exists.
    //        b. The last char of the string if "RMK/" does not exist. 
    // 
    // (5) If "RTS/" does not exist the RouteFrom/To is empty.
    //
    // (6) After connecting routes, the remarks contains the part 
    //     after "RMK/", to the end of string.
    //     If "RMK/" does not exist the remarks is am empty string.   
    //

    public class TdmParser
    {
        private string text;

        private string Ident;
        private string TimeStart;
        private string TimeEnd;
        private string Remarks;
        private string MainRoute;
        private string[] ConnectionRoutes;

        public TdmParser(string text)
        {
            this.text = text;
            FixFormat();
        }

        private void FixFormat()
        {
            if (text.Contains("RMK/") == false)
            {
                text += "RMK/";
            }

            if (text.Contains("RTS/") == false)
            {
                text = text.Replace("RMK/", "RTS/\nRMK/");
            }
        }

        /// <exception cref="TrackParseException"></exception>
        public ParseResult Parse()
        {
            try
            {
                ParseTracks();
            }
            catch
            {
                throw new TrackParseException();
            }

            return new ParseResult(
                Ident,
                TimeStart,
                TimeEnd,
                Remarks,
                MainRoute,
                ConnectionRoutes);
        }

        private static string GetPattern()
        {
            var matchId = @"TDM TRK\s+?(?<id>\w+).*?\n";
            var matchTime = @"\W?(?<timeStart>\w+)\W+(?<timeEnd>\w+)";
            var mainRoute = @"\W+(?<main>.*?)RTS/";
            var connections = @"(?<connect>.*?)RMK/";
            var remarks = @"(?<remark>.*)";

            return matchId + matchTime + mainRoute + connections + remarks;
        }

        private void ParseTracks()
        {
            var match = Regex.Match(
                text, GetPattern(), RegexOptions.Singleline);

            if (match.Success == false)
            {
                throw new ArgumentException();
            }

            Ident = match.Groups["id"].Value;
            TimeStart = match.Groups["timeStart"].Value;
            TimeEnd = match.Groups["timeEnd"].Value;
            MainRoute = match.Groups["main"].Value;

            ConnectionRoutes = match.Groups["connect"].Value
                .Lines()
                .Where(x => string.IsNullOrWhiteSpace(x) == false)
                .ToArray();

            Remarks = match.Groups["remark"].Value;
        }
    }
}
