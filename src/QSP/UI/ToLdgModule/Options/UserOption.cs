using QSP.NavData;

namespace QSP.UI.ToLdgModule.Options
{
    public class UserOption
    {
        public DataSource.Type SourceType { get; set; }
        public string OpenDataPath { get; set; }
        public string PaywarePath { get; set; }

        public UserOption(DataSource.Type SourceType,
            string OpenDataPath, string PaywarePath)
        {
            this.SourceType = SourceType;
            this.OpenDataPath = OpenDataPath;
            this.PaywarePath = PaywarePath;
        }

        public static UserOption Default
        {
            get
            {
                return new UserOption(
                    DataSource.Type.OpenData,
                    @"NavData\OpenData",
                    "");
            }
        }
    }
}
