using NUnit.Framework;
using QSP.LandingPerfCalculation.Airbus;
using QSP.MathTools.Tables;
using System.Xml.Linq;
using static QSP.LibraryExtension.Types;

namespace UnitTest.LandingPerfCalculation.Airbus
{
    [TestFixture]
    public class LoaderTest
    {
        public static readonly string Data =
@"<?xml version=""1.0"" encoding=""utf-8""?>
<Root>
  <!-- All lengths in ft.All weights in 1000 LB.-->
  
  <Parameters>
    <ProfileName>Airbus A319</ProfileName>

    <!-- Here, 1 represents this landing performance data format. 
         This node does not exist for the data format like 737-600. -->
    <Format>1</Format>
  </Parameters>
  
  <!-- 
        First row: landing weight
        Second row: Actual landing distance (dry)
        Third row: Actual landing distance (wet)

        Attributes: 
        e.g.
        elevation_dry= ""3"": In dry condition, add 3% to landing distance per 1000 ft elevation.
        tailwind_dry= ""20"": In dry condition, add 20% to landing distance per 10 knots.
        both_reverser_dry= ""3"": In dry condition, subtract 3% to landing distance if both reversers are operational.
        speed_5kts= ""8"": Add 8% for each 5 knots of extra speed.

        headwind_dry and headwind_wet are 0 because of wind correction on approach speed.
    -->
  <Table flaps = ""Full"" autobrake= ""MAX"" elevation_dry= ""3"" elevation_wet= ""4"" headwind_dry= ""0"" headwind_wet= ""0"" tailwind_dry= ""20"" tailwind_wet= ""26"" both_reverser_dry= ""3"" both_reverser_wet= ""6"" speed_5kts= ""8"" >
    88.106  96.916  105.727  
    2460    2427.2  2460     
    2952    2952    3017.6   
  </Table>
</Root>";

        public static AirbusPerfTable GetTable()
        {
            return new AirbusPerfTable()
            {
                Entries = List(
                    new Entry()
                    {
                        Dry = new Table1D(Arr(88.106, 96.916, 105.727),
                                          Arr(2460, 2427.2, 2460)),
                        Wet = new Table1D(Arr(88.106, 96.916, 105.727),
                                          Arr(2952, 2952, 3017.6)),
                        Flaps = "Full",
                        Autobrake = "MAX",
                        ElevationDry = 3,
                        ElevationWet = 4,
                        HeadwindDry = 0,
                        HeadwindWet = 0,
                        TailwindDry = 20,
                        TailwindWet = 26,
                        BothReversersDry = 3,
                        BothReversersWet = 6,
                        Speed5Knots = 8
                    })
            };
        }

        [Test]
        public void LoadTest()
        {
            var t = Loader.LoadPerfTable(XDocument.Parse(Data).Root);
            Assert.IsTrue(t.Equals(GetTable(), 1e-8));
        }
    }
}
