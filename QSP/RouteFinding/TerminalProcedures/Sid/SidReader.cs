using System;
using System.Collections.Generic;
using System.IO;
using static QSP.Utilities.ErrorLogger;
using static QSP.LibraryExtension.StringParser.Utilities;
using static QSP.RouteFinding.Utilities;

namespace QSP.RouteFinding.TerminalProcedures.Sid
{
    // Read from file and gets a SidCollection for an airport.
    public class SidReader
    {
        private string allText;

        public SidReader() { }

        public SidReader(string allText)
        {
            this.allText = allText;
        }

        /// <exception cref="LoadSidFileException"></exception>
        public void ReadFromFile(string path)
        {
            try
            {
                allText = File.ReadAllText(path);
            }
            catch (Exception ex)
            {
                WriteToLog(ex.ToString());
                throw new LoadSidFileException();
            }
        }

        public SidCollection Parse()
        {
            if (allText == null || allText.Length == 0)
            {
                throw new ArgumentException();
            }

            var sids = new List<SidEntry>();

            int index = 0;

            if (IsEmptyLine(allText, 0))
            {
                index = Math.Min(1, allText.Length - 1);
            }

            while (true)
            {
                if (LineStartsWithSid(allText, index))
                {
                    var entry = ReadSid(allText, ref index);

                    if (entry != null)
                    {
                        sids.Add(entry);
                    }
                }

                if (SkipToNextNonEmptyLine(allText, ref index) == false)
                {
                    break;
                }
            }

            return new SidCollection(sids);
        }

        private static SidEntry ReadSid(string item, ref int index)
        {
            try
            {
                // Go to the char after next comma.
                index = item.IndexOf(',', index) + 1;

                var name = ReadString(item, ref index, ',');
                var rwy = ReadString(item, ref index, ',');

                var wpts = new List<Waypoint>();
                bool endWithVector = true;

                while (true)
                {
                    index = item.IndexOf('\n', index) + 1;

                    if (index <= 0 || index >= item.Length || IsEmptyLine(item, index))
                    {
                        return new SidEntry(rwy, name, wpts, GetEntryType.GetType(rwy), endWithVector);
                    }
                    else
                    {
                        if (index + 1 < item.Length && HasCorrds(item.Substring(index, 2)))
                        {
                            endWithVector = false;
                            wpts.Add(GetWpt(item, ref index));
                        }
                        else
                        {
                            endWithVector = true;
                        }
                    }
                }
            }
            catch
            {
                return null;
            }
        }

        public static Waypoint GetWpt(string item, ref int index)
        {
            index = item.IndexOf(',', index) + 1;
            var ident = ReadString(item, ref index, ',');
            double lat = ParseDouble(item, ref index, ',');
            double lon = ParseDouble(item, ref index, ',');

            return new Waypoint(ident, lat, lon);
        }

        private static bool LineStartsWithSid(string item, int index)
        {
            return (index + 2 < item.Length && item[index] == 'S' && item[index + 1] == 'I' && item[index + 2] == 'D');
        }

        public static bool IsEmptyLine(string item, int index)
        {
            char firstChar = item[index];
            return (firstChar == '\n' || firstChar == '\r');
        }

        public static bool SkipToNextNonEmptyLine(string item, ref int index)
        {
            while (true)
            {
                if (SkipToNextLine(item, ref index) == false)
                {
                    return false;
                }

                if (IsEmptyLine(item, index) == false)
                {
                    return true;
                }
            }
        }
    }
}
