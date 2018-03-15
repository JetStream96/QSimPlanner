using QSP.LibraryExtension;
using QSP.RouteFinding;
using System;
using System.IO;
using System.Linq;
using System.Text;

namespace FixTypeAnalyzer
{
    class Program
    {
        static void Main(string[] args)
        {
            const string dir = @"C:\Data\Programming\Projects\QSimPlanner\AIRAC\1607\Proc";

            var analyzer = new Analyzer(dir);
            var report = new Report(analyzer.Analyze(), FixTypes.FixTypesWithCoords);

            var summary = analyzer.Message + "\n" + report.Summary();
            Console.WriteLine(summary);
            File.WriteAllText("BriefReport.txt", summary);

            var sb = new StringBuilder();
            report.NotKnownTypes()
                .Select(t => report.TypeSummary(t))
                .ForEach(s => sb.AppendLine(s));

            File.WriteAllText("DetailedReport.txt", sb.ToString());
        }
    }
}
