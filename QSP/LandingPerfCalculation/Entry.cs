namespace QSP.LandingPerfCalculation
{
    public sealed class Entry
    {
        public string FilePath { get; private set; }
        public string Aircraft { get; private set; }
        public string Description { get; private set; }
        public string Designator { get; private set; }

        public Entry(string FilePath, string Aircraft, string Description, string Designator)
        {
            this.FilePath = FilePath;
            this.Aircraft = Aircraft;
            this.Description = Description;
            this.Designator = Designator;
        }
    }
}
