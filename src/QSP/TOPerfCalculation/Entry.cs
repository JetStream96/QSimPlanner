namespace QSP.TOPerfCalculation
{
    public sealed class Entry
    {
        public string Aircraft { get; private set; }
        public string ProfileName { get; private set; }
        public string Designator { get; private set; }

        public Entry(string Aircraft, string ProfileName, string Designator)
        {
            this.Aircraft = Aircraft;
            this.ProfileName = ProfileName;
            this.Designator = Designator;
        }
    }
}
