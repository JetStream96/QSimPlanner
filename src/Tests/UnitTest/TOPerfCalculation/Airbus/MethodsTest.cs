using NUnit.Framework;
using QSP.MathTools.Tables;
using QSP.TOPerfCalculation.Airbus;
using QSP.TOPerfCalculation.Airbus.DataClasses;
using System.Xml.Linq;
using static QSP.LibraryExtension.Types;

namespace UnitTest.TOPerfCalculation.Airbus
{
    [TestFixture]
    public class MethodsTest
    {
        private static readonly string PerfFile = @"
<Root>
  <Parameters>
    <ProfileName>Airbus A320-200 CFM56</ProfileName>
    
    <!-- Here, 1 represents this takeoff performance data format. 
         This node does not exist for the data format like 737-600. -->
    <Format>1</Format>
  </Parameters>
  
  <!-- 
        Wind correction for runway length. 
        First row is runway length.
        Second row is length addition per knot headwind, in feet.
        Third row is length subtraction per knot tailwind, in feet. 

        Second row is from A319 FCOM. Third row is guessed based on Boeing's FCOM.
    -->
  <Wind>
    4920     5740
    21.32    22.96
    74.62    80.36
  </Wind>

  <!-- 
        Similar to wind. Data is all from A319 FCOM. Second row is uphill correction per percent, and third row is for downhill.    
        Example for applying corrections:
        4920 ft physical length, 10 knots headwind and 1% uphill slope => Corrected length is (4920 + 10 * 21.32 - 524.8) ft.
    -->
  <Slope>
    4920     5740
    524.8    705.2
    55.76    75.44
  </Slope>

  <!-- 
        From airport planning manual. 
        First row is runway lengths.
        First column is elevation.
        The table is takeoff limit weight. 
    -->
  <Table flaps=""1+F"" ISA_offset=""0"">
            4000    5000
    0       139     152
    2000    132     146
  </Table>

  <!-- If ISA_offset is ""15"", it means this table is for temperature at ISA+15°C. -->
  <Table flaps=""1+F"" ISA_offset=""15"">
            4000       4350
    0       132        137
    2000    127        132
  </Table>

  <!-- 
        All data above is for dry condition. Apply this wet correction to get actual takeoff limit weight.
        First row is runway length.
        Second row is takeoff limit weight decrement.
        Data from A319 FCOM and adjusted to the weight of A320.
    -->
  <WetCorrection>
    8000  10000
    2.273 2.273
  </WetCorrection>

  <!-- In 1000 LB. -->
  <Bleeds engine_ai=""0.5556"" all_ai=""1.6667"" packs_on=""4.8889""/>
</Root>
";

        public static AirbusPerfTable GetTable()
        {
            var headwind = new Table1D(Arr(4920.0, 5740), Arr(21.32, 22.96));
            var tailwind = new Table1D(Arr(4920.0, 5740), Arr(74.62, 80.36));
            var up = new Table1D(Arr(4920.0, 5740), Arr(524.8, 705.2));
            var down = new Table1D(Arr(4920.0, 5740), Arr(55.76, 75.44));

            var t0 = new Table2D(Arr(0.0, 2000), Arr(4000.0, 5000), Arr(Arr(139.0, 152), Arr(132.0, 146)));
            var t1 = new Table2D(Arr(0.0, 2000), Arr(4000.0, 4350), Arr(Arr(132.0, 137), Arr(127.0, 132)));


            var tables = List(
                new TableDataNode() { Table = t0, Flaps = "1+F", IsaOffset = 0 },
                new TableDataNode() { Table = t1, Flaps = "1+F", IsaOffset = 15 }
                );

            var wet = new Table1D(Arr(8000.0, 10000), Arr(2.273, 2.273));

            return new AirbusPerfTable()
            {
                HeadwindCorrectionTable = headwind,
                TailwindCorrectionTable = tailwind,
                UphillCorrectionTable = up,
                DownHillCorrectionTable = down,
                Tables = tables,
                WetCorrectionTable = wet,
                EngineAICorrection = 0.5556,
                AllAICorrection = 1.6667,
                PacksOnCorrection = 4.8889
            };
        }

        [Test]
        public void LoadPerfTableTest()
        {
            var (success, t) = Methods.LoadPerfTable(XDocument.Parse(PerfFile).Root);
            Assert.IsTrue(success);
            Assert.IsTrue(t.Equals(GetTable(), 1e-8));
        }

        [Test]
        public void LoadPerfTableFailTest()
        {
            var (success, t) = Methods.LoadPerfTable(new XElement("name", "2"));
            Assert.IsFalse(success);
        }
    }
}