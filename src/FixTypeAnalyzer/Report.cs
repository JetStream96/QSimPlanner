using System.Collections.Generic;
using System.Linq;
using System.Text;
using QSP.LibraryExtension;
using CommonLibrary.LibraryExtension.Sets;

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
                sb.AppendLine(TypeStats(i));
            }

            sb.AppendLine($"\nOther fix types ({total - known}):");

            foreach (var j in NotKnownTypes())
            {
                sb.AppendLine(TypeStats(j));
            }

            return sb.ToString();
        }

        private string TypeStats(string type)
        {
            var entries = TypeEntries(type);
            var count = entries.Count;
            var valid = entries.Where(i => i.Distance != null).ToList();
            var in1000 = valid.Where(i => i.Distance.Value <= 1000.0).ToList();
            var in500 = in1000.Where(i => i.Distance.Value <= 500.0).ToList();
            var in250 = in500.Where(i => i.Distance.Value <= 250.0);
            return $"{type}: Found {count} time(s), Has valid coords: {valid.Count}, " +
                   $"in 1000 nm: {in1000.Count}, in 500 nm: {in500.Count}, " +
                   $"in 250 nm: {in250.Count()}";
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

        private IReadOnlyList<IndividualEntry> TypeEntries(string type)
        {
            if (FixTypes.ContainsKey(type)) return FixTypes[type];
            return new IndividualEntry[0];
        }
    }
}