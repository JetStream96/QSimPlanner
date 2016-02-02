using QSP.LibraryExtension.StringParser;
using System;
using static QSP.LibraryExtension.StringParser.Utilities;

namespace QSP.RouteFinding.Tracks.Common.TDM.Parser
{
    // Parse the string which represents a single track, into a temporary format.
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
    // (2) The next line contains two time stamps, indicating TimeStart and TimeEnd respectively.
    //
    // (3) The main route starts from the next line, to:
    //        a. The char before "RTS/" if exists.
    //        b. The char before "RMK/" if "RTS/" does not exist but "RMK/" exists.
    //        c. The last char of the string if neither "RMK/" nor "RTS/" exists.
    //
    // (4) After main route, the connecting routes contains the part after "RTS/", to:
    //        a. The char before "RMK/" if exists.
    //        b. The last char of the string if "RMK/" does not exist. 
    // 
    // (5) If "RTS/" does not exist the RouteFrom/To is empty.
    //
    // (6) After connecting routes, the remarks contains the part after "RMK/", to the end of string.
    //     If "RMK/" does not exist the remarks is am empty string.   
    //

    public class TdmParser
    {
        private string text;

        private int rtsIndex;
        private int rmkIndex;

        private string Ident;
        private string TimeStart;
        private string TimeEnd;
        private string Remarks;
        private string MainRoute;
        private string[] ConnectionRoutes;

        public TdmParser(string text)
        {
            this.text = text;
        }

        /// <exception cref="TrackParseException"></exception>
        public ParseResult Parse()
        {
            try
            {
                parse();
            }
            catch
            {
                throw new TrackParseException();
            }

            return new ParseResult(Ident,
                                   TimeStart,
                                   TimeEnd,
                                   Remarks,
                                   MainRoute,
                                   ConnectionRoutes);

        }

        // Exception may occur if the input string format is not as expected (especially IndexOutOfRangeException).
        private void parse()
        {
            var sp = new StringParser(text);
            var d = DelimiterWords;

            sp.MoveToNextIndexOf("TDM TRK");
            sp.MoveRight("TDM TRK".Length);
            sp.SkipAny(d);

            Ident = sp.ReadToNextDelimeter(d);

            sp.SkipToNextLine();
            sp.SkipAny(d);

            TimeStart = sp.ReadToNextDelimeter(d);
            sp.SkipAny(d);
            TimeEnd = sp.ReadToNextDelimeter(d);

            sp.SkipToNextLine();
            getRtsRmkIndices(sp.CurrentIndex);
            MainRoute = sp.ReadString(mainRouteEndIndex());

            addRts(sp);
            addRmk(sp);
        }

        private void addRmk(StringParser sp)
        {
            if (rmkIndex >= 0)
            {
                sp.MoveRight("RMK/".Length + 1);
                Remarks = sp.ReadString(sp.Length - 1);
            }
            else
            {
                Remarks = "";
            }
        }

        private void addRts(StringParser sp)
        {
            if (rtsIndex >= 0)
            {
                sp.MoveRight("RTS/".Length + 1);
                ConnectionRoutes = sp.ReadString(rtsEndIndex())
                                     .Split(DelimiterLines, StringSplitOptions.RemoveEmptyEntries);
            }
            else
            {
                ConnectionRoutes = new string[0];
            }
        }

        private void getRtsRmkIndices(int index)
        {
            rtsIndex = text.IndexOf("RTS/", index);
            rmkIndex = text.IndexOf("RMK/", index);
        }

        private static int FirstNonNegativeTerm(int[] array)
        {
            foreach (var i in array)
            {
                if (i >= 0)
                {
                    return i;
                }
            }
            throw new ArgumentException();
        }

        private int mainRouteEndIndex()
        {
            return FirstNonNegativeTerm(new int[] { rtsIndex, rmkIndex, text.Length }) - 1;
        }

        private int rtsEndIndex()
        {
            return FirstNonNegativeTerm(new int[] { rmkIndex, text.Length }) - 1;
        }
    }
}
