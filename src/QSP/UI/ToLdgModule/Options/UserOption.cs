namespace QSP.UI.ToLdgModule.Options
{
    public class UserOption
    {
        // 0: Open data
        // 1: AAX (payware)
        public int SourceType { get; set; }

        public string OpenDataPath { get; set; }
        public string PaywarePath { get; set; }

        public UserOption(
            int SourceType, string OpenDataPath, string PaywarePath)
        {
            if (SourceType != 0 && SourceType != 1)
            {
                SourceType = 0;
            }

            this.SourceType = SourceType;
            this.OpenDataPath = OpenDataPath;
            this.PaywarePath = PaywarePath;
        }

        public static UserOption Default
        {
            get
            {
                return new UserOption(
                    0,
                    @"NavData\OpenData",
                    "");// TODO: Maybe check registry to 
                        // find the path automatically?
            }
        }
    }
}
