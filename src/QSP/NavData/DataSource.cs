namespace QSP.NavData
{
    public class DataSource
    {
        public enum Type
        {
            OpenData = 0,
            Navigraph = 1
        }

        public Type DataType { get; private set; }
        public string Path { get; private set; }

        public DataSource(Type DataType, string Path)
        {
            this.DataType = DataType;
            this.Path = Path;
        }
    }
}
