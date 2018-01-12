using System.Collections.Generic;

namespace QSP.RouteFinding.TerminalProcedures
{
    // Represents the SIDs/STARs that are blacklisted or whitelisted by the user.
    //
    public class ProcedureFilter
    {
        private Dictionary<string, FilterEntry> items = new Dictionary<string, FilterEntry>();

        public ProcedureFilter() { }

        public FilterEntry this[string icao, string rwy, bool isSid]
        {
            set
            {
                var key = ToKey(icao, rwy, isSid);
                items.Remove(key);
                items.Add(key, value);
            }

            /// <exception cref="Exception">Entry not found.</exception>
            get
            {
                var key = ToKey(icao, rwy, isSid);
                return items[key];
            }
        }

        public bool Exists(string icao, string rwy, bool isSid)
        {
            return items.ContainsKey(ToKey(icao, rwy, isSid));
        }

        /// <summary>
        /// Returns null if cannot find entry.
        /// </summary>
        public FilterEntry TryGetEntry(string icao, string rwy, bool isSid)
        {
            var key = ToKey(icao, rwy, isSid);
            if (items.TryGetValue(key, out var val)) return val;
            return null;
        }

        private static string ToKey(string icao, string rwy, bool isSid)
        {
            return (icao + rwy + (isSid ? "0" : "1")).ToUpper();
        }
    }

    public class FilterEntry
    {
        public bool IsBlackList { get; }
        public IReadOnlyList<string> Procedures { get; }
        public bool IsSid { get; }

        public FilterEntry(bool IsBlackList, IReadOnlyList<string> Procedures, bool IsSid)
        {
            this.IsBlackList = IsBlackList;
            this.Procedures = Procedures;
            this.IsSid = IsSid;
        }
    }
}
