using QSP.AviationTools;
using QSP.Core;
using QSP.TOPerfCalculation.Boeing.PerfData;
using System;
using static QSP.MathTools.Utilities;

namespace QSP.TOPerfCalculation.Boeing
{
    public class TOCalculator
    {
        private IndividualPerfTable table;
        private TOParameters para;

        private SlopeCorrTable slopeTable;
        private WindCorrTable windTable;
        private FieldLimitWtTable weightTable;

        public TOCalculator(BoeingPerfTable item, TOParameters para)
        {
            table = item.GetTable(para.FlapsIndex);
            this.para = para;
            setTables();
        }

        /// <summary>
        /// Computes runway length required for take off, 
        /// for user input and all available assumed temperatures.
        /// </summary>
        /// <exception cref="RunwayTooShortException">
        /// <exception cref="PoorClimbPerformanceException">
        public TOPerfResult TakeOffReport()
        {
            var result = new TOPerfResult();

            int maxOat = (int)Math.Round(weightTable.MaxOat);
            const int tempIncrement = 1;

            for (int oat = (int)Math.Round(para.OatCelsius);
                 oat <= maxOat;
                 oat += tempIncrement)
            {
                double rwyReq = takeoffDisMeterOverrideOat(oat);

                if (oat == para.OatCelsius)
                {
                    if (rwyReq <= para.RwyLengthMeter)
                    {
                        if (climbLimitWeightTon(oat) * 1000.0 >= para.WeightKg)
                        {
                            result.SetPrimaryResult(
                                (int)para.OatCelsius,
                                (int)rwyReq,
                                (int)(para.RwyLengthMeter - rwyReq));
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
                    if (rwyReq <= para.RwyLengthMeter &&
                        climbLimitWeightTon(oat) * 1000 >= para.WeightKg)
                    {
                        result.AddAssumedTemp(oat,
                                              (int)rwyReq,
                                              (int)(para.RwyLengthMeter - rwyReq));
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
            double correctedWtKG = para.WeightKg;

            //correct weight for TO1/TO2, if applicable
            if (table.AltnRatingAvail && para.ThrustRating != 0)
            {
                correctedWtKG = 1000 * thrustRatingFieldCorrWt(correctedWtKG / 1000.0,
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
            return takeoffDisMeterOverrideOat(para.OatCelsius);
        }

        private double takeoffDisMeterOverrideOat(double oat)
        {
            double corrLength = weightTable.CorrectedLengthRequired(
                rwyPressureAltFt(), oat, equivalentWeightKG() / 1000.0);

            double slopeCorrLength = windTable.SlopeCorrectedLength(
                headwindComp(), corrLength);

            return slopeTable.FieldLengthRequired(para.RwySlope, slopeCorrLength);
        }

        private enum AltnThrustOption
        {
            GetLimitWeight,
            GetEquivFullThrustWeight
        }

        /// <summary>
        /// Returns the corrected weight (for field) for TO1 or TO2, in ton. 
        /// The aircraft MUST have alternate rating available.
        /// It's NECESSARY that either TO1 or TO2 is selected in toPara.
        /// </summary>
        /// <param name="fullRatedWtTon">Full rated thrust weight, in ton.</param>
        private double thrustRatingFieldCorrWt(double fullRatedWtTon, AltnThrustOption para)
        {
            var altnRatingTable = table.AlternateThrustTables[this.para.ThrustRating - 1];

            switch (para)
            {
                case AltnThrustOption.GetEquivFullThrustWeight:
                    return altnRatingTable.EquivalentFullThrustWeight(
                        fullRatedWtTon,
                        this.para.SurfaceWet ?
                        AlternateThrustTable.TableType.Wet :
                        AlternateThrustTable.TableType.Dry);

                default:
                    return altnRatingTable.CorrectedLimitWeight(
                        fullRatedWtTon,
                        this.para.SurfaceWet ?
                        AlternateThrustTable.TableType.Wet :
                        AlternateThrustTable.TableType.Dry);
            }
        }

        /// <summary>
        /// Gets the field limit weight for the aircraft and take off parameters.
        /// </summary>
        public double FieldLimitWeightTon()
        {
            double slopeCorrLength = slopeTable.CorrectedLength(
                para.RwyLengthMeter, para.RwySlope);

            double windCorrLength = windTable.CorrectedLength(
                slopeCorrLength, headwindComp());

            var limitWtTon = weightTable.FieldLimitWeight(rwyPressureAltFt(),
                windCorrLength,
                para.OatCelsius) +
                fieldLimitModificationKG() / 1000.0;

            //correct weight for TO1/TO2, if applicable
            if (table.AltnRatingAvail && para.ThrustRating != 0)
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
            return climbLimitWeightTon(para.OatCelsius);
        }

        // Gets the climb limit weight based on the aircraft and 
        // take off environment, for the given OAT.
        //
        private double climbLimitWeightTon(double oat)
        {
            var limitWtTon = table.ClimbLimitWt.ClimbLimitWeight(rwyPressureAltFt(), oat)
                + climbLimitModificationKG() / 1000.0;

            if (table.AltnRatingAvail && para.ThrustRating != 0)
            {
                return table.AlternateThrustTables[para.ThrustRating - 1]
                    .CorrectedLimitWeight(limitWtTon,
                             AlternateThrustTable.TableType.Climb);
            }
            else
            {
                return limitWtTon;
            }
        }

        // Based on runway condition (dry or wet), sets the tables for further computation.      
        private void setTables()
        {
            if (para.SurfaceWet)
            {
                slopeTable = table.SlopeCorrWet;
                windTable = table.WindCorrWet;
                weightTable = table.WeightTableWet;
            }
            else
            {
                slopeTable = table.SlopeCorrDry;
                windTable = table.WindCorrDry;
                weightTable = table.WeightTableDry;
            }
        }

        /// <summary>
        /// Field limit weight is increased by this amount.
        /// </summary>
        private double fieldLimitModificationKG()
        {
            double correction = 0.0;

            //correction for packs

            if (para.PacksOn == false)
            {
                if (para.SurfaceWet)
                {
                    correction += table.PacksOffWet;
                }
                else
                {
                    correction += table.PacksOffDry;
                }
            }

            //correction for anti-ice
            if (para.SurfaceWet)
            {
                switch (para.AntiIce)
                {
                    case AntiIceOption.Engine:
                        correction -= table.AIEngWet;
                        break;

                    case AntiIceOption.EngAndWing:
                        correction -= table.AIBothWet;
                        break;
                }
            }
            else
            {
                switch (para.AntiIce)
                {
                    case AntiIceOption.Engine:
                        correction -= table.AIEngDry;
                        break;

                    case AntiIceOption.EngAndWing:
                        correction -= table.AIBothDry;
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
            if (para.PacksOn == false)
            {
                result += table.PacksOffClimb;
            }

            //correction for anti-ice

            switch (para.AntiIce)
            {
                case AntiIceOption.Engine:
                    result -= table.AIEngClimb;
                    break;

                case AntiIceOption.EngAndWing:
                    result -= table.AIBothClimb;
                    break;
            }

            return result;
        }

        //knots
        private double headwindComp()
        {
            return para.WindSpeed *
                Math.Cos(ToRadian(para.RwyHeading - para.WindHeading));
        }

        private double rwyPressureAltFt()
        {
            return CoversionTools.PressureAltitudeFt(para.RwyElevationFt, para.QNH);
        }

    }
}
