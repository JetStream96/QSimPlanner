namespace QSP.NavData
{
    public class DataSource
    {
        public Type DataType { get; }
        public string Path { get; }

        public DataSource(Type DataType, string Path)
        {
            this.DataType = DataType;
            this.Path = Path;
        }

        public enum Type
        {
            OpenData = 0,
            Navigraph = 1
        }
    }
}
