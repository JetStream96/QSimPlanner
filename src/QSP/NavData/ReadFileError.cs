namespace QSP.NavData
{
    public class ReadFileError
    {
        public int LineNumber { get; }
        public string Line { get; }

        public ReadFileError(int LineNumber, string Line)
        {
            this.LineNumber = LineNumber;
            this.Line = Line;
        }
    }
}