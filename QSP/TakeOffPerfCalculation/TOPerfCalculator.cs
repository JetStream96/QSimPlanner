using System;
using System.Linq;
using QSP.MathTools;
using QSP.AviationTools;
using QSP.Core;

namespace QSP.TakeOffPerfCalculation
{

    public class TOPerfCalculator
    {

        private TOPerfParameters toPara;

        private AircraftParameters acPara;

        public TOPerfCalculator(TOPerfParameters item, Aircraft ac)
        {
            toPara = item;
            acPara = LoadedData.GetPara(ac);
        }

        /// <summary>
        /// Computes runway length required for take off, for user input and all available assumed temperatures.
        /// </summary>
        /// <exception cref="RunwayTooShortException">The runway length in the parameters is not sufficient.</exception>
        /// <exception cref="PoorClimbPerformanceException">Aircraft cannot meet the required climb performance.</exception>
        public TOPerfResult TakeOffReport()
        {
            
            TOPerfResult result = new TOPerfResult();

            Table2D slopeTable = null;
            Table2D windTable = null;
            Table3D weightTable = null;

            setTables(ref slopeTable, ref windTable, ref weightTable);
            var equivWtTon = equivalentWeightKG() / 1000;

            //For this table, x: Wind and slope corrected runway length. 
            //f(x): Slope corrected runway length. 
            var slopeCorrTable = tableSlopeCorrLength(windTable, headwindComp());

            //For this table, x: Slope corrected runway length. 
            // f(x): Field length available. 
            var fieldLengthTable = tableFieldLength(slopeTable, toPara.RwySlope);

            int maxOat = Convert.ToInt32(weightTable.ZArray.Last());
            const int TEMP_INCREMENT = 1;

            Table1D rwyLengthRequired = null;
            double corrLength = 0;
            double slopeCorrLength = 0;
            int rwyReq = 0;


            for (int oat = toPara.OatCelsius; oat <= maxOat; oat += TEMP_INCREMENT)
            {
                rwyLengthRequired = tableComputeRwyRequired(rwyPressureAltFt(), oat, weightTable);
                corrLength = rwyLengthRequired.ValueAt(equivWtTon);
                slopeCorrLength = slopeCorrTable.ValueAt(corrLength);
                rwyReq = Convert.ToInt32(fieldLengthTable.ValueAt(slopeCorrLength));


                if (oat == toPara.OatCelsius)
                {

                    if (rwyReq <= toPara.RwyLengthMeter)
                    {
                        if (climbLimitWeightTon(oat) * 1000 >= toPara.TOWT_KG)
                        {
                            result.SetPrimaryResult(toPara.OatCelsius, rwyReq, toPara.RwyLengthMeter - rwyReq);
                        }
                        else
                        {
                            throw new PoorClimbPerformanceException();
                        }

                    }
                    else
                    {
                        throw new RunwayTooShortException();
                    }


                }
                else
                {
                    if (rwyReq <= toPara.RwyLengthMeter && climbLimitWeightTon(oat) * 1000 >= toPara.TOWT_KG)
                    {
                        result.AddAssumedTemp(oat, rwyReq, toPara.RwyLengthMeter - rwyReq);
                    }
                    else
                    {
                        return result;
                    }

                }

            }

            return result;

        }

        /// <summary>
        /// Based on anti-ice,packs and TO/TO1/TO2, gets the equivalent weight.
        /// e.g. If takeoff weight is 80.0 tons, with packs off it may become 79.5 tons.
        /// </summary>
        private double equivalentWeightKG()
        {

            double correctedWtKG = toPara.TOWT_KG;

            //correct weight for TO1/TO2, if applicable
            if (acPara.AltnRatingAvail && toPara.ThrustRating != ThrustRatingOption.Normal)
            {
                correctedWtKG = 1000 * thrustRatingFieldCorrWt(correctedWtKG / 1000, AltnThrustOption.GetEquivFullThrustWeight);
            }

            //correct weight for packs, anti-ice, etc
            correctedWtKG -= fieldLimitModificationKG();

            return correctedWtKG;

        }

        /// <summary>
        /// Find the minimum (physical) runway length for the takeoff (meter).
        /// </summary>
        public double TakeoffDistanceMeter()
        {

            Table2D slopeTable = null;
            Table2D windTable = null;
            Table3D weightTable = null;

            setTables(ref slopeTable, ref windTable, ref weightTable);

            var rwyLengthRequired = tableComputeRwyRequired(rwyPressureAltFt(), toPara.OatCelsius, weightTable);

            //this is the slope and wind corrected length
            double corrLength = rwyLengthRequired.ValueAt(equivalentWeightKG() / 1000);

            double slopeCorrLength = tableSlopeCorrLength(windTable, headwindComp()).ValueAt(corrLength);

            return tableFieldLength(slopeTable, toPara.RwySlope).ValueAt(slopeCorrLength);

        }

        private enum AltnThrustOption
        {
            GetLimitWeight,
            GetEquivFullThrustWeight
        }

        /// <summary>
        /// Returns the corrected weight (for field) for TO1 or TO2, in TON. The aircraft MUST have alternate rating available.
        /// It's NECESSARY that either TO1 or TO2 is selected in toPara.
        /// </summary>
        /// <param name="fullRatedWtTon">Full rated thrust weight, in TON.</param>
        private double thrustRatingFieldCorrWt(double fullRatedWtTon, AltnThrustOption para)
        {

            AlternateThrustTable altnRatingTable = null;

            if (toPara.ThrustRating == ThrustRatingOption.TO1)
            {
                altnRatingTable = acPara.AlternateThrustRating[0];
            }
            else if (toPara.ThrustRating == ThrustRatingOption.TO2)
            {
                altnRatingTable = acPara.AlternateThrustRating[1];
            }

            switch (para)
            {

                case AltnThrustOption.GetEquivFullThrustWeight:

                    return altnRatingTable.EquivalentFullThrustWeight(fullRatedWtTon, toPara.SurfaceWet ? AlternateThrustTable.WeightProperty.Wet : AlternateThrustTable.WeightProperty.Dry);
                default:

                    return altnRatingTable.CorrectedLimitWeight(fullRatedWtTon, toPara.SurfaceWet ? AlternateThrustTable.WeightProperty.Wet : AlternateThrustTable.WeightProperty.Dry);
            }

        }

        /// <summary>
        /// Generate a table such that:
        /// x: Slope corrected runway length. 
        /// f(x): Field length available. 
        /// </summary>
        /// <param name="slopeTable"></param>
        /// <param name="slope"></param> 
        private Table1D tableFieldLength(Table2D slopeTable, double slope)
        {
            double[] fieldLength = new double[slopeTable.XArray.Length];

            for (int i = 0; i <= fieldLength.Length - 1; i++)
            {
                fieldLength[i] = slopeTable.ValueAt(slopeTable.XArray[i], slope);
            }

            return new Table1D(fieldLength, Interpolation.ArrayOrder.Increasing, slopeTable.XArray);
        }

        /// <summary>
        /// Generate a table such that:
        /// x: Wind and slope corrected runway length. 
        /// f(x): Slope corrected runway length. 
        /// </summary>
        /// <param name="windTable"></param>
        /// <param name="headwindComponent"></param>
        private Table1D tableSlopeCorrLength(Table2D windTable, double headwindComponent)
        {

            double[] slopeCorrLength = new double[windTable.XArray.Length];

            for (int i = 0; i <= slopeCorrLength.Length - 1; i++)
            {
                slopeCorrLength[i] = windTable.ValueAt(windTable.XArray[i], headwindComponent);
            }

            return new Table1D(slopeCorrLength, Interpolation.ArrayOrder.Increasing, windTable.XArray);

        }

        /// <summary>
        /// Gets the field limit weight for the aircraft and take off parameters.
        /// </summary>
        public double FieldLimitWeightTon()
        {

            Table2D slopeTable = null;
            Table2D windTable = null;
            Table3D weightTable = null;

            setTables(ref slopeTable, ref windTable, ref weightTable);

            double slopeCorrLength = 0;
            double windCorrLength = 0;

            slopeCorrLength = slopeTable.ValueAt(toPara.RwyLengthMeter, toPara.RwySlope);
            windCorrLength = windTable.ValueAt(slopeCorrLength, headwindComp());

            var limitWtTon = weightTable.ValueAt(rwyPressureAltFt(), windCorrLength, toPara.OatCelsius) + fieldLimitModificationKG() / 1000;

            //correct weight for TO1/TO2, if applicable
            if (acPara.AltnRatingAvail && toPara.ThrustRating != ThrustRatingOption.Normal)
            {
                return thrustRatingFieldCorrWt(limitWtTon, AltnThrustOption.GetLimitWeight);
            }
            else
            {
                return limitWtTon;
            }

        }

        /// <summary>
        /// Gets the climb limit weight based on the aircraft and take off environment.
        /// </summary>
        public double ClimbLimitWeightTon()
        {
            return climbLimitWeightTon(toPara.OatCelsius);
        }

        /// <summary>
        /// Gets the climb limit weight based on the aircraft and take off environment, for the given OAT.
        /// </summary>
        private double climbLimitWeightTon(int oat)
        {
            var limitWtTon = acPara.ClimbLimitWt.ValueAt(rwyPressureAltFt(), oat) + climbLimitModificationKG() / 1000;

            if (acPara.AltnRatingAvail && toPara.ThrustRating != ThrustRatingOption.Normal)
            {
                if (toPara.ThrustRating == ThrustRatingOption.TO1)
                {
                    return acPara.AlternateThrustRating[0].CorrectedLimitWeight(limitWtTon, AlternateThrustTable.WeightProperty.Climb);
                }
                else
                {
                    return acPara.AlternateThrustRating[1].CorrectedLimitWeight(limitWtTon, AlternateThrustTable.WeightProperty.Climb);
                }
            }
            else
            {
                return limitWtTon;
            }

        }

        /// <summary>
        /// Based on runway condition (dry or wet), sets the tables for further computation.
        /// </summary>
        /// <param name="slopeTable"></param>
        /// <param name="windTable"></param>
        /// <param name="weightTable"></param>
        /// <remarks></remarks>

        private void setTables(ref Table2D slopeTable, ref Table2D windTable, ref Table3D weightTable)
        {

            if (toPara.SurfaceWet)
            {
                slopeTable = acPara.SlopeCorrWet;
                windTable = acPara.WindCorrWet;
                weightTable = acPara.WeightTableWet;


            }
            else
            {
                slopeTable = acPara.SlopeCorrDry;
                windTable = acPara.WindCorrDry;
                weightTable = acPara.WeightTableDry;

            }

        }

        /// <summary>
        /// Field limit weight is increased by this amount.
        /// </summary>
        private double fieldLimitModificationKG()
        {

            double result = 0;

            //correction for packs

            if (toPara.PacksOn == false)
            {
                if (toPara.SurfaceWet)
                {
                    result += acPara.PacksOffWet;
                }
                else
                {
                    result += acPara.PacksOffDry;
                }

            }

            //correction for anti-ice

            if (toPara.SurfaceWet)
            {
                switch (toPara.AntiIce)
                {

                    case AntiIceOption.Engine:
                        result -= acPara.AIEngWet;
                        break;
                    case AntiIceOption.EngAndWing:
                        result -= acPara.AIBothWet;

                        break;
                }


            }
            else
            {
                switch (toPara.AntiIce)
                {

                    case AntiIceOption.Engine:
                        result -= acPara.AIEngDry;
                        break;
                    case AntiIceOption.EngAndWing:
                        result -= acPara.AIBothDry;

                        break;
                }

            }

            return result;

        }

        /// <summary>
        /// Climb limit weight is increased by this amount.
        /// </summary>
        private double climbLimitModificationKG()
        {

            double result = 0;

            //correction for packs
            if (toPara.PacksOn == false)
            {
                result += acPara.PacksOffClimb;
            }

            //correction for anti-ice

            switch (toPara.AntiIce)
            {

                case AntiIceOption.Engine:
                    result -= acPara.AIEngClimb;
                    break;
                case AntiIceOption.EngAndWing:
                    result -= acPara.AIBothClimb;

                    break;
            }

            return result;

        }

        private double headwindComp()
        {
            //knots
            return toPara.WindSpeed * Math.Cos(MathTools.Utilities.ToRadian(toPara.RwyHeading - toPara.WindHeading));
        }

        private double rwyPressureAltFt()
        {
            return CoversionTools.PressureAltitudeFt(toPara.RwyElevationFt, toPara.QNH);
        }

        /// <summary>
        /// Generate a table such that:
        /// x: Take off weight (1000KG).
        /// f(x): Rwy length required. 
        /// </summary>
        private Table1D tableComputeRwyRequired(double altitudeFt, double oat, Table3D weightTable)
        {

            double[] weights = new double[weightTable.YArray.Length];

            for (int i = 0; i <= weights.Length - 1; i++)
            {
                weights[i] = weightTable.ValueAt(altitudeFt, weightTable.YArray[i], oat);
            }

            return new Table1D(weights, Interpolation.ArrayOrder.Increasing, weightTable.YArray);

        }

    }

}
