using System.Collections.Generic;

namespace QSP.RouteFinding.TerminalProcedures
{
    // TODO: Add test.

    // Represents the SIDs/STARs that are blacklisted or whitelisted by the user.
    //
    public class ProcedureFilter
    {
        private Dictionary<string, FilterEntry> items = new Dictionary<string, FilterEntry>();

        public ProcedureFilter() { }

        public FilterEntry this[string icao, string rwy]
        {
            set
            {
                var key = (icao + rwy).ToUpper();
                items.Remove(key);
                items.Add(key, value);
            }

            get
            {
                var key = (icao + rwy).ToUpper();
                return items[key];
            }
        }

        public bool Exists(string icao, string rwy)
        {
            return items.ContainsKey((icao + rwy).ToUpper());
        }

        /// <summary>
        /// Returns null if cannot find entry.
        /// </summary>
        public FilterEntry TryGetEntry(string icao, string rwy)
        {
            var key = (icao + rwy).ToUpper();
            if (items.TryGetValue(key, out var val)) return val;
            return null;
        }
    }

    public class FilterEntry
    {
        public bool IsBlackList { get; }
        public IReadOnlyList<string> Procedures { get; }

        public FilterEntry(bool IsBlackList, IReadOnlyList<string> Procedures)
        {
            this.IsBlackList = IsBlackList;
            this.Procedures = Procedures;
        }
    }
}
