using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QSP.TOPerfCalculation.Boeing.PerfData;
using QSP.MathTools.Tables;
using QSP.Core;
using static QSP.MathTools.Utilities;
using QSP.AviationTools;

namespace QSP.TOPerfCalculation.Boeing
{
    public class TOCalculator
    {
        private IndividualPerfTable acPara;
        private TOParameters toPara;

        public TOCalculator(BoeingPerfTable item, TOParameters para)
        {
            toPara = para;
            acPara = item.GetTable(para.FlapsIndex);
        }

        /// <summary>
        /// Computes runway length required for take off, 
        /// for user input and all available assumed temperatures.
        /// </summary>
        /// <exception cref="RunwayTooShortException">
        /// <exception cref="PoorClimbPerformanceException">
        public TOPerfResult TakeOffReport()
        {
            TOPerfResult result = new TOPerfResult();

            SlopeCorrTable slopeTable;
            WindCorrTable windTable;
            FieldLimitWtTable weightTable;

            setTables(out slopeTable, out windTable, out weightTable);
            double equivWtTon = equivalentWeightKG() / 1000.0;

            //For this table, x: Wind and slope corrected runway length. 
            //f(x): Slope corrected runway length. 
            var slopeCorrTable = tableSlopeCorrLength(windTable, headwindComp());

            //For this table, x: Slope corrected runway length. 
            // f(x): Field length available. 
            var fieldLengthTable = tableFieldLength(slopeTable, toPara.RwySlope);

            int maxOat = (int)Math.Round(weightTable.z.Last());
            const int tempIncrement = 1;

            Table1D rwyLengthRequired = null;
            double corrLength = 0;
            double slopeCorrLength = 0;
            int rwyReq = 0;

            for (int oat = (int)Math.Round(toPara.OatCelsius);
                 oat <= maxOat;
                 oat += tempIncrement)
            {
                rwyLengthRequired = tableComputeRwyRequired(rwyPressureAltFt(), oat, weightTable);
                corrLength = rwyLengthRequired.ValueAt(equivWtTon);
                slopeCorrLength = slopeCorrTable.ValueAt(corrLength);
                rwyReq = (int)fieldLengthTable.ValueAt(slopeCorrLength);

                if (oat == toPara.OatCelsius)
                {
                    if (rwyReq <= toPara.RwyLengthMeter)
                    {
                        if (climbLimitWeightTon(oat) * 1000.0 >= toPara.WeightKg)
                        {
                            result.SetPrimaryResult((int)toPara.OatCelsius,
                                                    rwyReq,
                                                    (int)(toPara.RwyLengthMeter - rwyReq));
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
                    if (rwyReq <= toPara.RwyLengthMeter && climbLimitWeightTon(oat) * 1000 >= toPara.WeightKg)
                    {
                        result.AddAssumedTemp(oat,
                                              rwyReq,
                                              (int)(toPara.RwyLengthMeter - rwyReq));
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
            double correctedWtKG = toPara.WeightKg;

            //correct weight for TO1/TO2, if applicable
            if (acPara.AltnRatingAvail && toPara.ThrustRating != ThrustRatingOption.Normal)
            {
                correctedWtKG = 1000 * thrustRatingFieldCorrWt(correctedWtKG / 1000,
                    AltnThrustOption.GetEquivFullThrustWeight);
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
            SlopeCorrTable slopeTable;
            WindCorrTable windTable;
            FieldLimitWtTable weightTable;

            setTables(out slopeTable, out windTable, out weightTable);

            var rwyLengthRequired = tableComputeRwyRequired(rwyPressureAltFt(), toPara.OatCelsius, weightTable);

            //this is the slope and wind corrected length
            double corrLength = rwyLengthRequired.ValueAt(equivalentWeightKG() / 1000.0);
            double slopeCorrLength = tableSlopeCorrLength(windTable, headwindComp()).ValueAt(corrLength);

            return tableFieldLength(slopeTable, toPara.RwySlope).ValueAt(slopeCorrLength);
        }

        private enum AltnThrustOption
        {
            GetLimitWeight,
            GetEquivFullThrustWeight
        }

        /// <summary>
        /// Returns the corrected weight (for field) for TO1 or TO2, in ton. The aircraft MUST have alternate rating available.
        /// It's NECESSARY that either TO1 or TO2 is selected in toPara.
        /// </summary>
        /// <param name="fullRatedWtTon">Full rated thrust weight, in ton.</param>
        private double thrustRatingFieldCorrWt(double fullRatedWtTon, AltnThrustOption para)
        {
            AlternateThrustTable altnRatingTable = null;

            if (toPara.ThrustRating == ThrustRatingOption.TO1)
            {
                altnRatingTable = acPara.AlternateThrustTables[0];
            }
            else if (toPara.ThrustRating == ThrustRatingOption.TO2)
            {
                altnRatingTable = acPara.AlternateThrustTables[1];
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
            double[] fieldLength = new double[slopeTable.x.Length];

            for (int i = 0; i < fieldLength.Length; i++)
            {
                fieldLength[i] = slopeTable.ValueAt(slopeTable.x[i], slope);
            }

            return new Table1D(fieldLength, slopeTable.x);
        }

        /// <summary>
        /// Generate a table such that:
        /// x: Wind and slope corrected runway length. 
        /// f(x): Slope corrected runway length. 
        /// </summary>
        private Table1D tableSlopeCorrLength(Table2D windTable, double headwindComponent)
        {
            var slopeCorrLength = new double[windTable.x.Length];

            for (int i = 0; i < slopeCorrLength.Length; i++)
            {
                slopeCorrLength[i] = windTable.ValueAt(windTable.x[i], headwindComponent);
            }

            return new Table1D(slopeCorrLength, windTable.x);
        }

        /// <summary>
        /// Gets the field limit weight for the aircraft and take off parameters.
        /// </summary>
        public double FieldLimitWeightTon()
        {
            SlopeCorrTable slopeTable;
            WindCorrTable windTable;
            FieldLimitWtTable weightTable;

            setTables(out slopeTable, out windTable, out weightTable);

            double slopeCorrLength = slopeTable.CorrectedLength(toPara.RwyLengthMeter, toPara.RwySlope);
            double windCorrLength = windTable.CorrectedLength(slopeCorrLength, headwindComp());

            var limitWtTon = weightTable.FieldLimitWeight(rwyPressureAltFt(),
                windCorrLength,
                toPara.OatCelsius) +
                fieldLimitModificationKG() / 1000.0;

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

        // Gets the climb limit weight based on the aircraft and 
        // take off environment, for the given OAT.
        //
        private double climbLimitWeightTon(double oat)
        {
            var limitWtTon = acPara.ClimbLimitWt.ClimbLimitWeight(rwyPressureAltFt(), oat)
                + climbLimitModificationKG() / 1000.0;

            if (acPara.AltnRatingAvail && toPara.ThrustRating != ThrustRatingOption.Normal)
            {
                if (toPara.ThrustRating == ThrustRatingOption.TO1)
                {
                    return acPara.AlternateThrustTables[0].CorrectedLimitWeight(limitWtTon,
                        AlternateThrustTable.WeightProperty.Climb);
                }
                else
                {
                    return acPara.AlternateThrustTables[1].CorrectedLimitWeight(limitWtTon,
                        AlternateThrustTable.WeightProperty.Climb);
                }
            }
            else
            {
                return limitWtTon;
            }
        }

        // Based on runway condition (dry or wet), sets the tables for further computation.      
        private void setTables(
            out SlopeCorrTable slopeTable,
            out WindCorrTable windTable,
            out FieldLimitWtTable weightTable)
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
            double correction = 0.0;

            //correction for packs

            if (toPara.PacksOn == false)
            {
                if (toPara.SurfaceWet)
                {
                    correction += acPara.PacksOffWet;
                }
                else
                {
                    correction += acPara.PacksOffDry;
                }
            }

            //correction for anti-ice
            if (toPara.SurfaceWet)
            {
                switch (toPara.AntiIce)
                {
                    case AntiIceOption.Engine:
                        correction -= acPara.AIEngWet;
                        break;

                    case AntiIceOption.EngAndWing:
                        correction -= acPara.AIBothWet;
                        break;
                }
            }
            else
            {
                switch (toPara.AntiIce)
                {
                    case AntiIceOption.Engine:
                        correction -= acPara.AIEngDry;
                        break;

                    case AntiIceOption.EngAndWing:
                        correction -= acPara.AIBothDry;
                        break;
                }
            }
            return correction;
        }

        /// <summary>
        /// Climb limit weight is increased by this amount.
        /// </summary>
        private double climbLimitModificationKG()
        {
            double result = 0.0;

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

        //knots
        private double headwindComp()
        {
            return toPara.WindSpeed * Math.Cos(ToRadian(toPara.RwyHeading - toPara.WindHeading));
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
            double[] weights = new double[weightTable.y.Length];

            for (int i = 0; i < weights.Length; i++)
            {
                weights[i] = weightTable.ValueAt(altitudeFt, weightTable.y[i], oat);
            }

            return new Table1D(weights, weightTable.y);
        }
    }
}
