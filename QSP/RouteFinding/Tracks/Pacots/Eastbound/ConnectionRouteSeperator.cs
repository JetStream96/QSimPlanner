using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QSP.LibraryExtension;

namespace QSP.RouteFinding.Tracks.Pacots.Eastbound
{
    public class ConnectionRouteSeperator
    {
        private string route;

        public ConnectionRouteSeperator(string route)
        {
            this.route = route;
        }

        public List<string> Seperate()
        {
            var lineRecords = getLines(route);
            var result = new List<string>();

            foreach (var i in lineRecords)
            {
                if (i.IsRouteStart)
                {
                    result.Add(i.Text);
                }
                else
                {
                    if (result.Count > 0)
                    {
                        result[result.Count - 1] += i.Text;
                    }
                }
            }
            return result;
        }

        private static List<Line> getLines(string route)
        {
            var lineRecords = new List<Line>();
            var lines = route.Lines();

            for (int i = 0; i < lines.Length; i++)
            {
                int x = lineStartIndex(lines[i]);

                if (x > lines[i].Length - 1)
                {
                    lineRecords.Add(new Line(false, ""));
                }
                else
                {
                    lineRecords.Add(new Line(x != 0, lines[i].Substring(x)));
                }
            }
            return lineRecords;
        }

        // Index may be larger than s.Length-1
        private static int lineStartIndex(string s)
        {
            int x = s.LastIndexOf("--");

            if (x >= 0)
            {
                return x + 2;
            }
            else
            {
                x = s.LastIndexOf(':');
                if (x < 0)
                {
                    return 0;
                }
                return x + 1;
            }
        }

        private struct Line
        {
            public bool IsRouteStart;
            public string Text;

            public Line(bool IsRouteStart, string Text)
            {
                this.IsRouteStart = IsRouteStart;
                this.Text = Text;
            }
        }
    }
}
