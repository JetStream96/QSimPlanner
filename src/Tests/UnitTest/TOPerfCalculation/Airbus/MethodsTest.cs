using NUnit.Framework;

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

        [Test]
        public void LoadPerfTableTest()
        {
            
        }
    }
}