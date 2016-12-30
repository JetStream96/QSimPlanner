using static QSP.RouteFinding.RwyIdent;

namespace QSP.RouteFinding.TerminalProcedures
{
    public enum EntryType
    {
        RwySpecific,
        Common,
        Transition
    }

    public static class GetEntryType
    {
        public static EntryType GetType(string rwy)
        {
            if (IsRwyIdent(rwy)) return EntryType.RwySpecific;
            if (rwy == "ALL") return EntryType.Common;
            return EntryType.Transition;
        }
    }
}
