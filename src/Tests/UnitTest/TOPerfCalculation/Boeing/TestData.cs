using QSP.AviationTools;
using QSP.LibraryExtension;
using QSP.MathTools.Tables;
using QSP.TOPerfCalculation.Boeing.PerfData;
using QSP.LibraryExtension.JaggedArrays;

namespace UnitTest.TOPerfCalculation.Boeing
{
    public class TestData
    {
        public string PerfXml
        {
            get
            {
                return
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
            }
        }

        public AlternateThrustTable[] AltnThrustTables
        {
            get
            {
                return new AlternateThrustTable[]
                {
                    new AlternateThrustTable(
                        new[] { 160.0, 180.0 },
                        new[] { 151.5, 170.2 },
                        new[] { 152.2, 170.5 },
                        new[] { 145.1, 162.5 }),

                    new AlternateThrustTable(
                        new[] { 160.0, 180.0 },
                        new[] { 142.6, 159.8 },
                        new[] { 144.3, 161.4 },
                        new[] { 128.5, 144.1 })
                };
            }
        }

        public SlopeCorrTable SlopeCorrDry => _slopeCorrDry;
        private SlopeCorrTable _slopeCorrDry = new SlopeCorrTable(
            new[] { 4200.0, 4600.0 },
            new[] { -2.0, -1.5 },
            new[]
            {
                new[]{ 4370.0, 4330.0},
                new[]{ 4810.0, 4760.0}
            });


        public SlopeCorrTable SlopeCorrWet => _slopeCorrWet;
        private SlopeCorrTable _slopeCorrWet = new SlopeCorrTable(
            new[] { 4200.0, 4600.0 },
            new[] { -2.0, -1.5 },
            new[]
            {
                new[]{ 4380.0, 4330.0 },
                new[]{ 4820.0, 4760.0 }
            });

        public WindCorrTable WindCorrDry => _windCorrDry;
        private WindCorrTable _windCorrDry = new WindCorrTable(
            new[] { 4200.0, 4600.0 },
            new[] { -15.0, -10.0 },
            new[]
            {
                new[] { 3120.0, 3480.0 },
                new[] { 3460.0, 3840.0 }
            });

        public WindCorrTable WindCorrWet => _windCorrWet;
        private WindCorrTable _windCorrWet = new WindCorrTable(
            new[] { 4200.0, 4600.0 },
            new[] { -15.0, -10.0 },
            new[]
            {
                new[] { 3030.0, 3420.0 },
                new[] { 3370.0, 3780.0 }
            });

        public FieldLimitWtTable WtTableDry => _wtTableDry;
        private FieldLimitWtTable _wtTableDry = new FieldLimitWtTable(
            new[] { 0.0, 2000.0 },
            new[] { 5550.0, 5800.0 },
            new[] { -40.0, 14.0 },

            new[]
            {
                new[]
                {
                    new[]{ 280.1, 252.3 },
                    new[]{ 287.3, 258.8 }
                },
                new[]
                {
                    new[]{ 262.6, 239.1 },
                    new[]{ 269.4, 245.3 }
                }
            });

        private FieldLimitWtTable _wtTableWet = new FieldLimitWtTable(
            new[] { 0.0, 2000.0 },
            new[] { 7340.0, 7400.0 },
            new[] { -40.0, 14.0 },

            new[]
            {
                new[]
                {
                    new[]{320.9, 286.8},
                    new[]{323.4, 289.2}
                },
                new[]
                {
                    new[]{299.2, 270.7},
                    new[]{301.6,273.0}
                }
            });

        public FieldLimitWtTable WtTableWet => _wtTableWet;

        public ClimbLimitWtTable ClimbLimTable => new ClimbLimitWtTable(
            new[] { 0.0, 2000.0 },
            new[] { -40.0, 14.0 },
            new[]
            {
                new[] { 351.5, 350.9 },
                new[] { 335.7, 335.5 }
            });

        public TestData()
        {
            CovertUnitSlopeOrWindCorrTable(_slopeCorrDry, false);
            CovertUnitSlopeOrWindCorrTable(_slopeCorrWet, false);
            CovertUnitSlopeOrWindCorrTable(_windCorrDry, false);
            CovertUnitSlopeOrWindCorrTable(_windCorrWet, false);
            CovertUnitFieldLimTable(_wtTableDry, false, true);
            CovertUnitFieldLimTable(_wtTableWet, false, true);
        }


        private void CovertUnitSlopeOrWindCorrTable(Table2D item, bool lengthUnitIsMeter)
        {
            if (lengthUnitIsMeter == false)
            {
                item.x.Multiply(Constants.FtMeterRatio);
                item.f.Multiply(Constants.FtMeterRatio);
            }
        }

        private void CovertUnitFieldLimTable(FieldLimitWtTable item,
            bool lengthUnitIsMeter,
            bool wtUnitIsTon)
        {
            if (lengthUnitIsMeter == false)
            {
                item.y.Multiply(Constants.FtMeterRatio);
            }

            if (wtUnitIsTon == false)
            {
                item.f.Multiply(Constants.LbKgRatio);
            }
        }
    }
}
