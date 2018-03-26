using QSP.MathTools.Tables;
using QSP.MathTools.Tables.Readers;
using QSP.TOPerfCalculation.Airbus.DataClasses;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using static QSP.Utilities.ExceptionHelpers;

namespace QSP.TOPerfCalculation.Airbus
{
    public static class Loader
    {
        /// <summary>
        /// Load an aircraft data from specified Xml file.
        /// </summary>
        /// /// <exception cref="Exception"></exception>
        public static PerfTable ReadFromXml(string filePath)
        {
            var doc = XDocument.Load(filePath);
            var (success, table) = LoadPerfTable(doc.Root);
            if (!success) throw new Exception($"Failed to load {filePath} as AirbusPerfTable.");
            return new PerfTable(table, TOTableLoader.GetEntry(filePath, doc));
        }

        /// <summary>
        /// Loads the performance table when given the root node of xml file.
        /// </summary>
        public static (bool success, AirbusPerfTable val) LoadPerfTable(XElement rootNode)
        {
            try
            {
                if (rootNode.Element("Parameters").Element("Format").Value != "1")
                {
                    return (false, null);
                }

                var (headwind, tailwind) = GetTable3Rows(rootNode.Element("Wind").Value);
                var (uphill, downhill) = GetTable3Rows(rootNode.Element("Slope").Value);
                (Table1D, Table1D) GetTable3Rows(string str)
                {
                    var nums = Regex.Split(str, @"\s").Where(s => s != "")
                                                      .Select(s => double.Parse(s))
                                                      .ToList();
                    var count = nums.Count;
                    Ensure<Exception>(count % 3 == 0);
                    var third = count / 3;
                    var x = nums.Take(third).ToArray();
                    return (new Table1D(x, nums.Skip(third).Take(third).ToArray()),
                            new Table1D(x, nums.Skip(third * 2).ToArray()));
                }

                var tables = rootNode.Elements("Table").Select(GetTableNode);
                TableDataNode GetTableNode(XElement e)
                {
                    var t = TableReader2D.Read(e.Value);
                    var flaps = e.Attribute("flaps").Value;
                    var isa = double.Parse(e.Attribute("ISA_offset").Value);
                    return new TableDataNode()
                    {
                        Table = t,
                        Flaps = flaps,
                        IsaOffset = isa
                    };
                }


                var wetTable = GetWetTable(rootNode.Element("WetCorrection").Value);
                Table1D GetWetTable(string str)
                {
                    var nums = Regex.Split(str, @"\s").Where(s => s != "")
                                                      .Select(s => double.Parse(s))
                                                      .ToList();
                    var count = nums.Count;
                    Ensure<Exception>(count % 2 == 0);
                    var half = count / 2;
                    return new Table1D(nums.Take(half).ToArray(), nums.Skip(half).ToArray());
                }

                var bleeds = rootNode.Element("Bleeds");
                var engineAI = double.Parse(bleeds.Attribute("engine_ai").Value);
                var allAI = double.Parse(bleeds.Attribute("all_ai").Value);
                var packsOn = double.Parse(bleeds.Attribute("packs_on").Value);

                return (true,
                        new AirbusPerfTable()
                        {
                            HeadwindCorrectionTable = headwind,
                            TailwindCorrectionTable = tailwind,
                            UphillCorrectionTable = uphill,
                            DownHillCorrectionTable = downhill,
                            Tables = tables.ToList(),
                            WetCorrectionTable = wetTable,
                            EngineAICorrection = engineAI,
                            AllAICorrection = allAI,
                            PacksOnCorrection = packsOn
                        });
            }
            catch
            {
                return (false, null);
            }
        }
    }
}