using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using QSP.AviationTools.Coordinates;
using QSP.LibraryExtension;
using QSP.RouteFinding.TerminalProcedures.Parser;
using static QSP.RouteFinding.TerminalProcedures.Parser.SectionSplitter;

namespace FixTypeAnalyzer
{
    public class Analyzer
    {
        private readonly string dir;
        private readonly StringBuilder messages = new StringBuilder();

        public string Message => messages.ToString();

        public Analyzer(string dir)
        {
            this.dir = dir;
        }

        public Dictionary<string, List<IndividualEntry>> Analyze()
        {
            return AllAirports()
                .SelectMany(e => e.Lines.Select(i => ToIndividualEntry(e.Airport, i)))
                .GroupBy(i => GetFixType(i.Line))
                .ToDictionary(x => x.Key, x => x.ToList());
        }

        private static IndividualEntry ToIndividualEntry(Airport a, string line)
        {
            var words = line.Split(',');

            double lat, lon;
            double? dis = null;
            if (words.Length >= 4 &&
                double.TryParse(words[2], out lat) &&
                double.TryParse(words[3], out lon) &&
                -90.0 <= lat && lat <= 90.0 &&
                -180.0 <= lon && lon <= 180.0)
            {
                dis = new LatLon(lat, lon).Distance(a.Lat, a.Lon);
            }

            return new IndividualEntry(words[1], line, dis);
        }

        private static string GetFixType(string line)
        {
            return line.Substring(0, line.IndexOf(','));
        }

        private IEnumerable<Entry> AllAirports()
        {
            var files = Directory.GetFiles(dir);

            foreach (var f in files)
            {
                var lines = File.ReadAllLines(f);
                var airport = GetAirport(lines);
                if (airport == null)
                {
                    messages.AppendLine($"Cannot find ICAO for file {f}.");
                    continue;
                }

                yield return new Entry(airport, GetAllFixLines(lines));
            }
        }

        private static IEnumerable<string> GetAllFixLines(string[] lines)
        {
            return GetSplitResult(lines).SelectMany(r => r.Lines.Skip(1));
        }

        private static IEnumerable<SplitEntry> GetSplitResult(string[] lines)
        {
            return Split(lines, SectionSplitter.Type.Sid)
                .Concat(Split(lines, SectionSplitter.Type.Star));
        }

        private static Airport GetAirport(string[] lines)
        {
            foreach (var s in lines)
            {
                var words = s.Split(',');
                double lat, lon;

                if (words.Length >= 5 &&
                    words[0] == "A" &&
                    double.TryParse(words[3], out lat) &&
                    double.TryParse(words[4], out lon))
                {
                    return new Airport()
                    {
                        Icao = words[1],
                        Lat = lat,
                        Lon = lon
                    };
                }
            }

            return null;
        }

        private class Entry
        {
            public Airport Airport { get; }
            public IEnumerable<string> Lines { get; }

            public Entry(Airport Airport, IEnumerable<string> Lines)
            {
                this.Airport = Airport;
                this.Lines = Lines;
            }
        }

        private class Airport
        {
            public string Icao;
            public double Lat;
            public double Lon;
        }
    }
}