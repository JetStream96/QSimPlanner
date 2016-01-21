using System;
using System.Collections.Generic;
using System.Linq;
using QSP.LibraryExtension;
using static QSP.LibraryExtension.StringParser.Utilities;
using QSP.RouteFinding.Airports;

namespace QSP.RouteFinding.Tracks.Pacots
{
    public class EastTracksParser
    {
        //UPR means upper (airway). This should be ignored when parsing routeFrom/To.
        private static string[] SPECIAL_WORD = { "UPR" };

        private AirportManager airportList;

        public EastTracksParser(AirportManager airportList)
        {
            this.airportList = airportList;
        }

        public List<PacificTrack> CreateEastboundTracks(PacotsMessage item)
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

            var tracksStr = SplitTrackMsg(item);
            //each string is like:
            //
            // FLEX ROUTE : PUTER A590 PASRO POWAL CURVS 53N170W 52N160W 51N150W
            //              51N140W ORNAI
            //JAPAN ROUTE : PEXEL A590 PUTER
            //  NAR ROUTE : ACFT LDG KSEA--ORNAI SIMLU KEPKO TOU JAWBN KSEA
            //              ACFT LDG KPDX--ORNAI SIMLU KEPKO TOU KEIKO KPDX
            //              ACFT LDG CYVR--ORNAI SIMLU KEPKO YAZ FOCHE CYVR
            //       RMK: ACFT LDG OTHER DEST--ORNAI SIMLU KEPKO UPR TO DEST

            PacificTrack[] result = new PacificTrack[tracksStr.Count];

            for (int i = 0; i <= result.Length - 1; i++)
            {
                string str = tracksStr[i].Item2;

                int x = 0;
                int y = 0;

                //find text "FLEX ROUTE"
                x = str.IndexOf("FLEX ROUTE");
                //find next colon
                y = str.IndexOf(':', x);
                //find next line that contains a colon
                x = lastLineBeforeColon(str, y + 1);

                string flexRoute = str.Substring(y + 1, x - y);
                var mainRoute = flexRoute.Split(DelimiterWords, StringSplitOptions.RemoveEmptyEntries);

                //remarks, may not exist for some tracks
                int z = str.IndexOf("RMK");
                //get remarks and set z equal to the last index of routeTo/From
                string remarks = null;

                if (z >= 0)
                {
                    remarks = str.Substring(z);
                    z--;
                }
                else
                {
                    z = str.Length - 1;
                }

                //now get the routeTo/From
                x = str.IndexOf(':', x) + 1;
                var allToFromRoutes = getLinesRouteToFrom(str, x, z);
                //try to merge lines if needed
                tryMergeLines(mainRoute, allToFromRoutes);

                var lists = getTrack(mainRoute, allToFromRoutes);

                result[i] = new PacificTrack(PacotDirection.Eastbound,
                                             tracksStr[i].Item1,
                                             timeInfo.Item1,
                                             timeInfo.Item2,
                                             remarks,
                                             Array.AsReadOnly(mainRoute),
                                             lists.Item1.AsReadOnly(),
                                             lists.Item2.AsReadOnly(),
                                             Constants.JAPAN_LATLON);
            }
            return result;
        }

        private static void tryMergeLines(string[] mainRoute, List<string> item)
        {
            //e.g.
            //
            //RCTP/VHHH ROUTE : MOLKA M750 BUNGU Y81 SYOYU Y809 KAGIS A590 PABBA
            //                  OTR5 KALNA
            //       NAR ROUTE : ACFT LDG KSFO--ALLBE PIRAT OSI KSFO
            //                   ACFT LDG KLAX--ALLBE PIRAT AVE FIM KLAX
            //
            //the RCTP/VHHH route is not contained in one single line and need fixing


            for (int i = item.Count - 2; i >= 0; i--)
            {
                var s = item[i].Split(DelimiterWords, StringSplitOptions.RemoveEmptyEntries);

                if (s.First() != mainRoute.Last() && s.Last() != mainRoute.First())
                {
                    item[i] += item[i + 1];
                    item.RemoveAt(i + 1);
                }
            }
        }

        private Tuple<List<string[]>, List<string[]>> getTrack(string[] mainRoute, List<string> allToFromRoutes)
        {
            List<string[]> listTo = new List<string[]>();
            List<string[]> listFrom = new List<string[]>();
            Tuple<string[], int> m = null;

            foreach (var i in allToFromRoutes)
            {
                m = getTrack(mainRoute, i);

                if (m.Item2 == 0)
                {
                    listFrom.Add(m.Item1);
                }
                else
                {
                    listTo.Add(m.Item1);
                }
            }
            return new Tuple<List<string[]>, List<string[]>>(listFrom, listTo);
        }

        private Tuple<string[], int> getTrack(string[] mainRoute, string line)
        {
            var words = line.Split(DelimiterWords, StringSplitOptions.RemoveEmptyEntries);

            if (words.Length > 0)
            {

                if (words.Last() == mainRoute.First())
                {
                    if (airportList.Find(words[0]) != null)
                    {
                        return new Tuple<string[], int>(words.SubArray(1, words.Length - 1, SPECIAL_WORD), 0);
                    }
                    else
                    {
                        return new Tuple<string[], int>(words.Exclude(SPECIAL_WORD), 0);
                    }
                }
                else if (words[0] == mainRoute.Last() && words.Length > 1)
                {
                    if (airportList.Find(words.Last()) != null)
                    {
                        return new Tuple<string[], int>(words.SubArray(0, words.Length - 1, SPECIAL_WORD), 1);
                    }
                    else
                    {
                        return new Tuple<string[], int>(words.Exclude(SPECIAL_WORD), 1);
                    }
                }
            }
            throw new ArgumentException("Bad format.");
        }

        private static List<string> getLinesRouteToFrom(string item, int startIndex, int endIndex)
        {
            List<string> result = new List<string>();
            int x = 0;
            int y = startIndex;

            while (y < endIndex)
            {
                x = y;
                y = lastCharLine(item, x + 1);

                if (y < 0)
                {
                    y = endIndex;
                }

                int v = Math.Min(y, endIndex);

                selectLineAfterColon(item, ref x, ref v);
                selectLineAfterDashes(item, ref x, ref v);

                var route = item.Substring(x, v - x + 1);

                if (isCompleteLine(route))
                {
                    result.Add(route);
                }
            }
            return result;
        }

        private static bool isCompleteLine(string item)
        {
            return item.Last() == '\n';
        }

        private static int lastCharLine(string item, int index)
        {
            int len = item.Length;

            if (index >= len)
            {
                throw new ArgumentOutOfRangeException("The index is out of the bound of string.");
            }

            for (int i = index; i <= len - 2; i++)
            {
                if (item[i] == '\r' && item[i + 1] == '\n')
                {
                    return i + 1;

                }
                else if (item[i] == '\n')
                {
                    return i;
                }
            }
            return len - 1;
        }


        private static void selectLineAfterColon(string item, ref int startIndex, ref int endIndex)
        {
            var x = item.LastIndexOf(':', endIndex, endIndex - startIndex + 1);

            if (x >= 0)
            {
                startIndex = x + 1;
            }
        }


        private static void selectLineAfterDashes(string item, ref int startIndex, ref int endIndex)
        {
            var x = item.LastIndexOf("--", endIndex, endIndex - startIndex + 1);

            if (x >= 0)
            {
                startIndex = x + 2;
            }
        }

        private static int lastLineBeforeColon(string item, int index)
        {
            int result = item.IndexOf(':', index);

            if (result >= 0)
            {
                for (int i = result; i >= index; i--)
                {
                    if (item[i] == '\r' || item[i] == '\n')
                    {
                        return i;
                    }
                }
            }
            return -1;
        }

        //ID, Track text
        public static List<Pair<string, string>> SplitTrackMsg(string msg)
        {
            List<Pair<string, string>> result = new List<Pair<string, string>>();
            var indices = getAllNums(msg, 0);

            int index = 0;
            int len = 0;

            for (int i = 0; i <= indices.Count - 1; i++)
            {
                index = indices[i].nextIndexSearch;
                len = ((i == indices.Count - 1) ? msg.Length : indices[i + 1].startIndex) - index;

                result.Add(new Pair<string, string>(indices[i].ID.ToString(), msg.Substring(index, len)));
            }
            return result;
        }

        private static List<trackIdent> getAllNums(string msg, int index)
        {
            //spot things like: 
            //TRACK 1.

            List<trackIdent> result = new List<trackIdent>();
            trackIdent t = null;

            while (index < msg.Length)
            {
                t = tryReadNextNum(msg, index);

                if (t != null)
                {
                    result.Add(t);
                    index = t.nextIndexSearch;
                }
                else
                {
                    return result;
                }
            }
            return result;
        }

        private static trackIdent tryReadNextNum(string msg, int index)
        {
            trackIdent result = null;
            index = msg.IndexOf("TRACK", index);

            while (index >= 0 && index < msg.Length)
            {
                result = readNum(msg, index);

                if (result != null)
                {
                    return result;
                }
                else
                {
                    index = msg.IndexOf("TRACK", index + 1);
                }
            }
            return null;
        }

        //returns track ID, index for next search
        private static trackIdent readNum(string msg, int index)
        {
            int trackId = 0;
            var startIndex = index;
            index += "TRACK".Length;

            bool flag = true;
            int n = 0;

            while (index >= 0 && index < msg.Length)
            {
                n = msg[index] - 48;

                if (n >= 0 && n <= 9)
                {
                    flag = false;
                    trackId *= 10;
                    trackId += n;
                    index++;
                }
                else
                {
                    if (flag)
                    {
                        if (DelimiterWords.Contains(msg[index]) == false)
                        {
                            //nothing is found
                            return null;
                        }
                        else
                        {
                            index++;
                        }
                    }
                    else
                    {
                        if (msg[index] == '.')
                        {
                            return new trackIdent(trackId, startIndex, index + 1);
                        }
                        else
                        {
                            //bad format
                            return null;
                        }
                    }
                }
            }
            return null;
        }

        private class trackIdent
        {
            public int ID;
            //index of "T" in "TRACKS
            public int startIndex;
            //index of the next char of "."
            public int nextIndexSearch;

            public trackIdent(int ID, int startIndex, int nextIndexSearch)
            {
                this.ID = ID;
                this.startIndex = startIndex;
                this.nextIndexSearch = nextIndexSearch;
            }
        }
    }
}
