using System;
using System.Collections.Generic;
using System.Linq;
using QSP.LibraryExtension;

namespace QSP.RouteFinding.Tracks.Pacots.Eastbound
{
    public static class ConnectionRouteSeperator
    {
        public static List<string> Seperate(string route)
        {
            var lines = route.Lines().ToList();

            for (int i = lines.Count - 1; i >= 0; i--)
            {
                var line = lines[i];

                if (line.Contains("--") == false &&
                    line.Contains(':') == false)
                {
                    if (i != 0) lines[i - 1] += line;
                    lines.RemoveAt(i);
                }
                else
                {
                    int rightMost = Math.Max(
                        line.IndexOf(':') + ":".Length,
                        line.IndexOf("--") + "--".Length);

                    int index = Math.Min(rightMost, line.Length - 1);
                    lines[i] = line.Substring(index);
                }
            }

            return lines;
        }
    }
}
