using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QSP.LibraryExtension;

namespace FixTypes
{
    class Program
    {
        static void Main(string[] args)
        {
            const string dir = @"C:\Data\Programming\Projects\QSimPlanner\AIRAC\1607\Proc";
            string[] knownTypes = { "IF", "DF", "TF", "FD", "CF", "AF" };

            var analyzer = new Analyzer(dir);
            var report = new Report(analyzer.Analyze(), QSP.RouteFinding.FixTypes.FixTypesWithCoords);

            var summary = report.Summary();
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
