using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using QSP.LibraryExtension;
using QSP.RouteFinding.TerminalProcedures.Parser;
using static QSP.RouteFinding.TerminalProcedures.Parser.SectionSplitter;

namespace FixTypes
{
    public class Analyzer
    {
        private readonly string dir;

        public Analyzer(string dir)
        {
            this.dir = dir;
        }

        public int Analyze()
        {
            var files = Directory.GetFiles(dir);
            files.ForEach(f =>
            {
                var lines = File.ReadAllLines(f);
                var icao = GetIcao(lines);
                if (icao == null)
                {
                    Console.WriteLine($"Cannot find ICAO for file {f}.");
                    return;
                }

                var contents = GetAllFixLines(lines);

            });

        }

        private static IEnumerable<string> GetAllFixLines(string[] lines)
        {
            return GetSplitResult(lines).SelectMany(r => r.Lines);
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
                var match = Regex.Match(s, @"A,([A-Z]{4}),");
                if (match.Success) return match.Groups[1].Value;
            }

            return null;
        }

    }
}