namespace QSP.TOPerfCalculation
{
    public sealed class Entry
    {
        public string ProfileName { get; private set; }
        public string FilePath { get; private set; }

        public Entry(string ProfileName, string FilePath)
        {
            this.ProfileName = ProfileName;
            this.FilePath = FilePath;
        }
    }
}
