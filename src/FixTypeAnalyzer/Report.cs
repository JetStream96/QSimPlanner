using System.Collections.Generic;
using System.Linq;
using System.Text;
using QSP.LibraryExtension;
using QSP.LibraryExtension.Sets;

namespace FixTypeAnalyzer
{
    public class Report
    {
        public Dictionary<string, List<IndividualEntry>> FixTypes { get; }
        public IReadOnlySet<string> KnownTypes { get; }

        public Report(Dictionary<string, List<IndividualEntry>> FixTypes,
            IReadOnlySet<string> KnownTypes)
        {
            this.FixTypes = FixTypes;
            this.KnownTypes = KnownTypes;

            var comparer = Comparer<IndividualEntry>.Create((x, y) => x.Icao.CompareTo(y.Icao));
            this.FixTypes.Values.ForEach(t => t.Sort(comparer));
        }

        public string Summary()
        {
            int total = FixTypes.Keys.Count;
            int known = KnownTypes.Count;
            var sb = new StringBuilder();
            sb.Append($"Fix types found: {total}, known types: {known}.");
            sb.Append("\n\nKnown types:\n");

            foreach (var i in KnownTypes.OrderBy(t => t))
            {
                sb.AppendLine($"{i}: Found {TypeCount(i)} times");
            }

            sb.AppendLine($"\nOther fix types ({total - known}):");

            foreach (var j in NotKnownTypes())
            {
                sb.AppendLine($"{j}: Found {TypeCount(j)} times");
            }

            return sb.ToString();
        }

        public IEnumerable<string> NotKnownTypes()
        {
            return FixTypes.Keys.Except(KnownTypes);
        }

        public string TypeSummary(string type)
        {
            var sb = new StringBuilder();
            foreach (var i in FixTypes[type])
            {
                sb.AppendLine($"ICAO: {i.Icao}; {i.Line}");
            }

            return sb.ToString();
        }

        private int TypeCount(string type)
        {
            if (FixTypes.ContainsKey(type)) return FixTypes[type].Count;
            return 0;
        }
    }
}