using Microsoft.VisualStudio.TestTools.UnitTesting;
using QSP.AviationTools;
using QSP.LibraryExtension;
using QSP.MathTools.Tables;
using QSP.TOPerfCalculation.Boeing;
using QSP.TOPerfCalculation.Boeing.PerfData;
using System.Linq;
using System.Xml.Linq;

namespace UnitTest.TOPerfCalculation.Boeing
{
    [TestClass]
    public class PerfDataLoaderTest
    {
        private const double delta = 1E-7;

        public static readonly string PerfXml =
            @"<?xml version=""1.0"" encoding=""UTF-8""?>
<TOPerf>
  <Parameters>
    <Aircraft>B777-200LR</Aircraft>
    <Description></Description>
    <Designator>B77L</Designator>
  </Parameters>
  
  <IndividualTable>
    <Flaps>5</Flaps>
    
<WeightUnit>1000KG</WeightUnit>
<!-- 1000KG or 1000LB -->

<LengthUnit>FT</LengthUnit>
<!-- FT or M -->

<Adjustments>
<Packs>
<Dry>500</Dry>
<Wet>500</Wet> 
<Climb>1700</Climb>
</Packs>

<AntiIce>
<EngineOnly>
<Dry>0</Dry>
<Wet>0</Wet>
<Climb>0</Climb>
</EngineOnly>

<EngineAndWing>
<Dry>2050</Dry>
<Wet>2200</Wet>
<Climb>2100</Climb>
</EngineAndWing>

</AntiIce>

</Adjustments>

<Dry>

<SlopeCorrection>-2.0 -1.5 
4200 4370 4330
4600 4810 4760 
</SlopeCorrection>

<WindCorrection>-15 -10
4200 3120 3480
4600 3460 3840
</WindCorrection>

<WeightTable>

<Altitude>0</Altitude>
<!-- In feet -->

<Table>-40 14
5550 280.1 252.3
5800 287.3 258.8
</Table>

<Climb>351.5 350.9</Climb>
<!-- CLIMB LIMIT WT -->
</WeightTable>

<WeightTable>
<Altitude>2000</Altitude>

<Table>-40 14
5550 262.6 239.1
5800 269.4 245.3
</Table>

<Climb>335.7 335.5</Climb>
</WeightTable>

</Dry> 

<Wet>

<SlopeCorrection>-2.0 -1.5
4200 4380 4330
4600 4820 4760
</SlopeCorrection>

<WindCorrection>-15 -10
4200 3030 3420
4600 3370 3780
</WindCorrection>

<WeightTable>

<Altitude>0</Altitude>
<!-- In feet -->

<Table>-40 14
7340 320.9 286.8
7400 323.4 289.2
</Table>

<Climb>351.5 350.9</Climb>
<!-- CLIMB LIMIT WT -->
</WeightTable>

<WeightTable>
<Altitude>2000</Altitude>

<Table>-40 14
7340 299.2 270.7
7400 301.6 273.0
</Table>

<Climb>335.7 335.5</Climb>
</WeightTable>

</Wet>
 <Derates>
   <FullThrustName>TO</FullThrustName>
   
<TO1>160 180
151.5 170.2
152.2 170.5
145.1 162.5
</TO1>

<TO2>160 180
142.6 159.8
144.3 161.4
128.5 144.1
</TO2> 

   </Derates>
</IndividualTable>
 
</TOPerf>";

        [TestMethod]
        public void ReadTableTest()
        {
            var allTables = new PerfDataLoader().ReadTable(
                 XDocument.Parse(PerfXml).Root);

            Assert.AreEqual(1, allTables.Flaps.Length);

            var table = allTables.GetTable(0);

            Assert.AreEqual(500.0, table.PacksOffDry, delta);
            Assert.AreEqual(500.0, table.PacksOffWet, delta);
            Assert.AreEqual(1700.0, table.PacksOffClimb, delta);
            Assert.AreEqual(0.0, table.AIEngDry, delta);
            Assert.AreEqual(0.0, table.AIEngWet, delta);
            Assert.AreEqual(0.0, table.AIEngClimb, delta);
            Assert.AreEqual(2050.0, table.AIBothDry, delta);
            Assert.AreEqual(2200.0, table.AIBothWet, delta);
            Assert.AreEqual(2100.0, table.AIBothClimb, delta);
            Assert.IsTrue(table.Flaps == "5");
            Assert.IsTrue(table.AltnRatingAvail);
            Assert.AreEqual(2, table.AlternateThrustTables.Length);

            assertAltnThrustTables(table.AlternateThrustTables[0],
                new AlternateThrustTable(
                    new double[] { 160, 180 },
                    new double[] { 151.5, 170.2 },
                    new double[] { 152.2, 170.5 },
                    new double[] { 145.1, 162.5 }));

            assertAltnThrustTables(table.AlternateThrustTables[1],
                new AlternateThrustTable(
                    new double[] { 160, 180 },
                    new double[] { 142.6, 159.8 },
                    new double[] { 144.3, 161.4 },
                    new double[] { 128.5, 144.1 }));

            Assert.IsTrue(Enumerable.SequenceEqual(table.ThrustRatings,
                new string[] { "TO", "TO1", "TO2" }));

            assertSlopeOrWindCorrTable(
                table.SlopeCorrDry,
                new SlopeCorrTable(
                    new double[] { 4200, 4600 },
                    new double[] { -2.0, -1.5 },
                    new double[][]
                    {
                        new double[]{4370,4330},
                        new double[]{4810,4760}
                    }
                ),
                false);

            assertSlopeOrWindCorrTable(
                table.SlopeCorrWet,
                new SlopeCorrTable(
                    new double[] { 4200, 4600 },
                    new double[] { -2.0, -1.5 },
                    new double[][]
                    {
                        new double[]{4380,4330},
                        new double[]{4820,4760}
                    }
                ),
                false);

            assertSlopeOrWindCorrTable(
                table.WindCorrDry,
                new WindCorrTable(
                    new double[] { 4200, 4600 },
                    new double[] { -15, -10 },
                    new double[][]
                    {
                        new double[]{3120,3480},
                        new double[]{3460,3840}
                    }
                ),
                false);

            assertSlopeOrWindCorrTable(
                table.WindCorrWet,
                new WindCorrTable(
                    new double[] { 4200, 4600 },
                    new double[] { -15, -10 },
                    new double[][]
                    {
                        new double[]{3030,3420},
                        new double[]{3370,3780}
                    }
                ),
                false);

            assertFieldLimTable(table.WeightTableDry,
                new FieldLimitWtTable(
                    new double[] { 0, 2000 },
                    new double[] { 5550, 5800 },
                    new double[] { -40, 14 },

                    new double[][][]
                    {
                        new double[][]
                        {
                            new double[]{280.1,252.3},
                            new double[]{287.3,258.8}
                        },
                        new double[][]
                        {
                            new double[]{262.6,239.1},
                            new double[]{269.4,245.3}
                        }
                    }),
                false,
                true);

            assertFieldLimTable(table.WeightTableWet,
                new FieldLimitWtTable(
                    new double[] { 0, 2000 },
                    new double[] { 7340, 7400 },
                    new double[] { -40, 14 },

                    new double[][][]
                    {
                        new double[][]
                        {
                            new double[]{320.9, 286.8},
                            new double[]{323.4, 289.2}
                        },
                        new double[][]
                        {
                            new double[]{299.2, 270.7},
                            new double[]{301.6,273.0}
                        }
                    }),
                false,
                true);

            assertClimbLimTable(
                table.ClimbLimitWt,
                new ClimbLimitWtTable(
                    new double[] { 0, 2000 },
                    new double[] { -40, 14 },
                    new double[][]
                    {
                        new double[] { 351.5, 350.9 },
                        new double[] { 335.7, 335.5 }
                    }),
                true);
        }

        private void assertAltnThrustTables(AlternateThrustTable item,
            AlternateThrustTable other)
        {
            Assert.IsTrue(Enumerable.SequenceEqual(item.FullThrustWeights, other.FullThrustWeights));
            Assert.IsTrue(Enumerable.SequenceEqual(item.DryWeights, other.DryWeights));
            Assert.IsTrue(Enumerable.SequenceEqual(item.WetWeights, other.WetWeights));
            Assert.IsTrue(Enumerable.SequenceEqual(item.ClimbWeights, other.ClimbWeights));
        }

        private void assertSlopeOrWindCorrTable(Table2D item,
            Table2D other,
            bool lengthUnitIsMeter)
        {
            if (lengthUnitIsMeter == false)
            {
                other.x.Multiply(Constants.FtMeterRatio);
                other.f.Multiply(Constants.FtMeterRatio);
            }

            Assert.IsTrue(item.Equals(other, delta));
        }

        private void assertFieldLimTable(FieldLimitWtTable item,
            FieldLimitWtTable other,
            bool lengthUnitIsMeter,
            bool wtUnitIsTon)
        {
            if (lengthUnitIsMeter == false)
            {
                other.y.Multiply(Constants.FtMeterRatio);
            }

            if (wtUnitIsTon == false)
            {
                other.f.Multiply(Constants.LbKgRatio);
            }

            Assert.IsTrue(item.Equals(other, delta));
        }

        private void assertClimbLimTable(Table2D item,
            Table2D other,
            bool wtUnitIsTon)
        {
            if (wtUnitIsTon == false)
            {
                other.f.Multiply(Constants.LbKgRatio);
            }

            Assert.IsTrue(item.Equals(other, delta));
        }
    }
}
