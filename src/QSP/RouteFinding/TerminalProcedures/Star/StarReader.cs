using QSP.RouteFinding.Containers;
using System;
using System.Collections.Generic;
using static QSP.LibraryExtension.StringParser.Utilities;
using static QSP.RouteFinding.FixTypes;
using static QSP.RouteFinding.TerminalProcedures.Sid.SidReader;

namespace QSP.RouteFinding.TerminalProcedures.Star
{
    // Read from file and gets a StarCollection for an airport.
    public class StarReader
    {
        private string allText;

        public StarReader() { }

        public StarReader(string allText)
        {
            this.allText = allText;
        }
        
        public StarCollection Parse()
        {
            if (allText == null || allText.Length == 0)
            {
                throw new ArgumentException();
            }

            var stars = new List<StarEntry>();

            int index = 0;

            if (IsEmptyLine(allText, 0))
            {
                index = Math.Min(1, allText.Length - 1);
            }

            while (true)
            {
                if (LineStartsWithStar(allText, index))
                {
                    var entry = ReadStar(allText, ref index);

                    if (entry != null)
                    {
                        stars.Add(entry);
                    }
                }

                if (SkipToNextNonEmptyLine(allText, ref index) == false)
                {
                    break;
                }
            }
            
            return new StarCollection(stars);
        }

        private static StarEntry ReadStar(string item, ref int index)
        {
            try
            {
                // Go to the char after next comma.
                index = item.IndexOf(',', index) + 1;

                var name = ReadString(item, ref index, ',');
                var rwy = ReadString(item, ref index, ',');

                var wpts = new List<Waypoint>();

                while (true)
                {
                    index = item.IndexOf('\n', index) + 1;

                    if (index <= 0 || index >= item.Length || IsEmptyLine(item, index))
                    {
                        return new StarEntry(rwy, name, wpts, GetEntryType.GetType(rwy));
                    }
                    else
                    {
                        if (index + 1 < item.Length && HasCorrds(item.Substring(index, 2)))
                        {
                            wpts.Add(GetWpt(item, ref index));
                        }
                    }
                }
            }
            catch
            {
                return null;
            }
        }

        private static bool LineStartsWithStar(string item, int index)
        {
            return (index + 3 < item.Length &&
                    item[index] == 'S' &&
                    item[index + 1] == 'T' &&
                    item[index + 2] == 'A' &&
                    item[index + 3] == 'R');
        }
    }
}

