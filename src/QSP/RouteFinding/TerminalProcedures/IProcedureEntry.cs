namespace QSP.RouteFinding.TerminalProcedures
{
    public interface IProcedureEntry
    {
        string RunwayOrTransition { get; }
        string Name { get; }
        EntryType Type { get; }
    }
}
