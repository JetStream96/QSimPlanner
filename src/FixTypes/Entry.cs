using System.Collections.Generic;

namespace FixTypes
{
    public class Entry
    {
        public string Icao { get; }
        public IReadOnlyList<Line> Lines { get; }

        public Entry(string Icao, IReadOnlyList<Line> Lines)
        {
            this.Icao = Icao;
            this.Lines = Lines;
        }
    }

    public class Line
    {
        public int LineNum { get; }
        public string Content { get; }
        public string FixType { get; }

        public Line(int LineNum, string Content)
        {
            this.LineNum = LineNum;
            this.Content = Content;

        }
    }
}