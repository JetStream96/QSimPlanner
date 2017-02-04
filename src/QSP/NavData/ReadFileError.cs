using System.Collections.Generic;
using System.Linq;

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

    public static class ReadFileErrorMsg
    {
        public static string ErrorMsg(IReadOnlyList<ReadFileError> errors, string fileName)
        {
            if (errors.Count == 0) return null;
            return $"Cannot parse the following lines of {fileName}:\n" +
                string.Join("\n", errors.Select(e => $"Line {e.LineNumber}: {e.Line}"));
        }
    }
}