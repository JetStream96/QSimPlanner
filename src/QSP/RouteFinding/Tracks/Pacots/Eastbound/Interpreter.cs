using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace QSP.RouteFinding.Tracks.Pacots.Eastbound
{
    // Interpret this message:
    // (The final 'RMK' line is optional.)
    //
    //   TRACK 1.
    //    FLEX ROUTE : KALNA 42N160E 45N170E 47N180E 49N170W 50N160W 51N150W
    //                 51N140W ORNAI
    //   JAPAN ROUTE : ONION OTR5 KALNA
    //     NAR ROUTE : ACFT LDG KSEA--ORNAI SIMLU KEPKO TOU MARNR KSEA
    //                 ACFT LDG KPDX--ORNAI SIMLU KEPKO TOU KEIKO KPDX
    //                 ACFT LDG CYVR--ORNAI SIMLU KEPKO YAZ FOCHE CYVR
    //           RMK : ACFT LDG OTHER DEST--ORNAI SIMLU KEPKO UPR TO DEST
    //
    public class Interpreter
    {
        private string text;

        public Interpreter(string text)
        {
            this.text = text;
        }

        public EastboundTrackStrings Parse()
        {
            var result = new EastboundTrackStrings();

            // ID
            var matchId = Regex.Match(text, @"TRACK\s+?(\d+)");
            result.ID = int.Parse(matchId.Groups[1].Value);

            // Flex Route
            var matchFlexRoute = Regex.Match(
                text,
                @"FLEX ROUTE\s+?:(.*?)(\n[^\n]*?:)",
                RegexOptions.Singleline);

            var flexRoute = matchFlexRoute.Groups[1].Value;

            result.FlexRoute = flexRoute.Split(
                new char[] { ' ', '\t', '\n', '\r' },
                StringSplitOptions.RemoveEmptyEntries);

            // Remarks (optional)
            // Get all text after "RMK :"
            var matchRemarks = Regex.Match(
                text,
                @"RMK\s+?:(.+)",
                RegexOptions.Singleline);

            result.Remark = matchRemarks.Success ?
                matchRemarks.Groups[1].Value : "";

            // ConnectionRoute
            // Get text, starting from the line which contains the 
            // second colon, to the line before remarks (if exists).
            var pattern = matchRemarks.Success ?
                @".*?:.*?\n([^\n]*?:.*?)RMK" :
                @".*?:.*?\n([^\n]*?:.*)";

            var matchConnection =
                Regex.Match(text, pattern, RegexOptions.Singleline);

            result.ConnectionRoute = matchConnection.Groups[1].Value;

            return result;
        }

        public class EastboundTrackStrings
        {
            public int ID;
            public IEnumerable<string> FlexRoute;
            public string ConnectionRoute;
            public string Remark;
        }
    }
}
