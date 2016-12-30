using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using QSP.LibraryExtension;
using QSP.RouteFinding.TerminalProcedures.Parser;
using static QSP.RouteFinding.TerminalProcedures.Parser.SectionSplitter;

namespace FixTypeAnalyzer
{
    public class Analyzer
    {
        private readonly string dir;

        public Analyzer(string dir)
        {
            this.dir = dir;
        }

        public Dictionary<string, List<IndividualEntry>> Analyze()
        {
            return AllAirports()
                .SelectMany(e => e.Lines.Select(i => new IndividualEntry(e.Icao, i)))
                .GroupBy(i => GetFixType(i.Line))
                .ToDictionary(x => x.Key, x => x.ToList());
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
                var icao = GetIcao(lines);
                if (icao == null)
                {
                    Console.WriteLine($"Cannot find ICAO for file {f}.");
                    continue;
                }

                yield return new Entry(icao, GetAllFixLines(lines));
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

        private static string GetIcao(string[] lines)
        {
            foreach (var s in lines)
            {
                var match = Regex.Match(s, @"A,([A-Z0-9]{4}),");
                if (match.Success) return match.Groups[1].Value;
            }

            return null;
        }

    }
}