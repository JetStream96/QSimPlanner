using System.Collections.Generic;
using System.Linq;

namespace QSP.RouteFinding.TerminalProcedures.Parser
{
    public static class SectionSplitter
    {
        public static IEnumerable<SplitEntry> Split(IEnumerable<string> lines, Type type)
        {
            var keyword = type == Type.Sid ? "SID," : "STAR,";

            // Skip to first line that contains the keyword.
            var usefulLines = lines.SkipWhile(line => !IsSectionStart(line, keyword));
            var queue = new Queue<string>(usefulLines);

            List<string> result = null;

            while (true)
            {
                result = GetEntry(queue, keyword).ToList();
                if (result.Count < 2) yield break;
                yield return new SplitEntry() { Lines = result };
            }
        }

        private static IEnumerable<string> GetEntry(Queue<string> lines, string keyword)
        {
            while (lines.Count > 0 && !IsSectionStart(lines.Peek(), keyword))
            {
                lines.Dequeue();
            }

            if (lines.Count > 0) yield return lines.Dequeue();

            while (lines.Count > 0)
            {
                var peek = lines.Peek();
                if (IsSectionStart(peek, keyword) || IsEmptyLine(peek)) yield break;
                yield return lines.Dequeue();
            }
        }

        private static bool IsSectionStart(string line, string keyword)
        {
            var len = keyword.Length;
            return line.Length >= len && line.Substring(0, len) == keyword;
        }

        private static bool IsEmptyLine(string line) => !line.Contains(',');

        public enum Type
        {
            Sid,
            Star
        }

        public class SplitEntry
        {
            /// <summary>
            /// The first line contains either SID or STAR. The rest are waypoints/vectors.
            /// </summary>
            public IReadOnlyList<string> Lines;
        }
    }
}