namespace QSP.UI.ToLdgModule.Options
{
    public class UserOption
    {
        public int SourceType { get; private set; }
        public string SourcePath { get; private set; }

        public UserOption(int SourceType, string SourcePath)
        {
            if (SourceType != 0 && SourceType != 1)
            {
                SourceType = 0;
            }

            this.SourceType = SourceType;
            this.SourcePath = SourcePath;
        }
    }
}
