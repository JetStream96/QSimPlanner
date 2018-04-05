using QSP.MathTools.Tables;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace A319Fuel
{
    class Program
    {
        static void Main(string[] args)
        {
            File.WriteAllText("./Out.txt", Convert());
        }

        // Convert to Boeing format.
        static string Convert()
        {
            string[] Split(string s, string splitters)
            {
                return s.Split(splitters.ToCharArray(), StringSplitOptions.RemoveEmptyEntries)
                    .Where(x => !x.All(c => c == ' ' || c == '\t'))
                    .ToArray();
            }

            var landingWtsKg = new[] { 40000, 50000, 60000, 70000 };
            var doc = XDocument.Load("./profile/A319_100_CFM.xml");
            var tableOutputs = doc.Root.Elements("Table").Select(t =>
            {
                var landingWtKg = double.Parse(t.Attribute("landing_weight").Value);
                var str = t.Value;
                var lines = Split(str, "\r\n");
                var firstLine = lines[0].Split('|')
                                        .Select(s => Split(s, " \t").Select(double.Parse))
                                        .ToList();

                var x1 = firstLine[0].ToArray();
                var x2 = firstLine[1].ToArray();

                // Ignore lines with time. We do not need them.
                var linesFuel = lines.Where((_, ind) => ind % 2 == 1);
                var f = linesFuel.Select(line =>
                {
                    var words = Split(line, " \t");
                    var nums = words.Select(s =>
                        s == "NoValue" ? double.PositiveInfinity : double.Parse(s)).ToList();
                    var dis = nums[0];
                    var f1 = nums.Skip(1).Take(x1.Length).ToArray();
                    var f2 = nums.Skip(1 + x1.Length).ToArray();
                    var tablesf2 = new Table1D(x2, f2);

                    var minFuel = landingWtsKg.Select(wt =>
                        f1.Select((x, ind) => x + (wt - landingWtKg) / 1000 * tablesf2.ValueAt(x1[ind]))
                          .Min()
                    );

                    return new double[] { dis }.Concat(minFuel);
                });

                var outputLines = f.Select(x => string.Join(" ", x));
                return string.Join("\n", outputLines);
            });

            var landingWtsStr = string.Join(" ", landingWtsKg);
            return string.Join("\n", new string[] { landingWtsStr }.Concat(tableOutputs));
        }
    }
}
