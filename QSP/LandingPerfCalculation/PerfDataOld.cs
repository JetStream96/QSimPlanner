using QSP.Core;

namespace QSP.LandingPerfCalculation
{
    /// <summary>
    /// Every aircraft should have a unique instance of this class.
    /// </summary>
    public class PerfDataOld
    {
        //All length in meter, all weights in KG.
        private double weightRef;
        private double weightStep;
        private string[] autoBrkDry;
        private string[] autoBrkWet;
        private string[] flaps;

        private string[] reversers;
        private double[][][] mainDataDry;
        //first index = corresponding index in flaps
        //second index = corresponding index in autoBrkDry
        //third index = parameters

        private double[][][][] mainDataWet;
        //first index = corresponding index in flaps
        //second index : good = 0, medium = 1, poor =2 
        //third index = corresponding index in autoBrkDry
        //forth index = parameters

        public PerfDataOld(
            double weightRef, 
            double weightStep, 
            string[] autoBrkDry, 
            string[] autoBrkWet, 
            string[] flaps,
            string[] reversers,
            double[][][] mainDataDry, 
            double[][][][] mainDataWet)
        {
            this.weightRef = weightRef;
            this.weightStep = weightStep;
            this.autoBrkDry = autoBrkDry;
            this.autoBrkWet = autoBrkWet;
            this.flaps = flaps;
            this.reversers = reversers;
            this.mainDataDry = mainDataDry;
            this.mainDataWet = mainDataWet;
        }

        public string[] BrakesAvailable(SurfaceCondition item)
        {
            if (item == SurfaceCondition.Dry)
            {
                return autoBrkDry;
            }
            else
            {
                return autoBrkWet;
            }
        }

        public string[] FlapsAvailable()
        {
            return flaps;
        }

        public string[] RevAvailable()
        {
            return reversers;
        }

        private enum dataColumn
        {
            RefDis = 0,
            WtAdjustAbove = 1,
            WtAdjustBelow = 2,
            AltAdjust = 3,
            AltAdjustHigh = 4,
            HeadwindCorr = 5,
            TailwindCorr = 6,
            DownhillCorr = 7,
            UphillCorr = 8,
            TempAboveISA = 9,
            TempBelowISA = 10,
            AppSpdAdjust = 11,
            OneRev = 12,
            NoRev = 13
        }

        /// <summary>
        /// Based on landing parameters with brake setting overriden, gets the requested data.
        /// </summary>
        /// <param name="para">Landing parameters</param>
        /// <param name="request">Requested data</param>
        private double reqData(LandingParameters para, dataColumn request, int brakeSetting)
        {
            if (para.SurfaceCondition == SurfaceCondition.Dry)
            {
                return mainDataDry[para.FlapsIndex][brakeSetting][(int)request];
            }
            return mainDataWet[(int)para.SurfaceCondition - 1][para.FlapsIndex][brakeSetting][(int)request];
        }

        public LandingCalcResult GetLandingReport(LandingParameters para)
        {
            LandingCalcResult result = new LandingCalcResult();

            var brkList = 
                para.SurfaceCondition == SurfaceCondition.Dry ? 
                autoBrkDry : 
                autoBrkWet;

            int disReqMeter = 0;
            int disRemainMeter = 0;

            //compute the user input
            disReqMeter = (int)(GetLandingDistanceMeter(para, para.AutoBrakeIndex));
            disRemainMeter = para.RwyLengthMeter - disReqMeter;

            if (disRemainMeter >= 0)
            {
                result.SetSelectedBrakesResult(
                    brkList[para.AutoBrakeIndex], disReqMeter, disRemainMeter);
            }
            else
            {
                throw new RunwayTooShortException();
            }

            //compute all possible brake settings

            for (int i = 0; i < brkList.Length; i++)
            {
                if (i == para.AutoBrakeIndex)
                {
                    result.AddOtherResult();
                }
                else
                {
                    disReqMeter = (int)(GetLandingDistanceMeter(para, i));
                    disRemainMeter = para.RwyLengthMeter - disReqMeter;

                    if (disRemainMeter >= 0)
                    {
                        result.AddOtherResult(brkList[i], disReqMeter, disRemainMeter);
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
        /// Gets the landing distance for the given landing parameters.
        /// </summary>
        public double GetLandingDistanceMeter(LandingParameters para, int brakeSetting)
        {
            double wtExcessSteps = (para.WeightKG - weightRef) / weightStep;

            double totalDisMeter = reqData(para, dataColumn.RefDis, brakeSetting) +
                wtExcessSteps *

                (wtExcessSteps >= 0 ? 
                reqData(para, dataColumn.WtAdjustAbove, brakeSetting) : 
                -reqData(para, dataColumn.WtAdjustBelow, brakeSetting))

                + para.ElevationFT / 1000 * reqData(para, dataColumn.AltAdjust, brakeSetting)
                + para.HeadwindKts / 10 *

                (para.HeadwindKts >= 0 ? 
                reqData(para, dataColumn.HeadwindCorr, brakeSetting) : 
                -reqData(para, dataColumn.TailwindCorr, brakeSetting))

                + para.SlopePercent *

                (para.SlopePercent <= 0 ? 
                -reqData(para, dataColumn.DownhillCorr, brakeSetting) : 
                reqData(para, dataColumn.UphillCorr, brakeSetting));

            double tempExcess = para.TempCelsius - AviationTools.CoversionTools.IsaTemp(para.ElevationFT);

            totalDisMeter += tempExcess / 10 * tempExcess >= 0 ? 
                reqData(para, dataColumn.TempAboveISA, brakeSetting) : 
                -reqData(para, dataColumn.TempBelowISA, brakeSetting);

            totalDisMeter += para.AppSpeedIncrease / 10;

            if (para.Reverser == ReverserOption.HalfRev)
            {
                totalDisMeter += reqData(para, dataColumn.OneRev, brakeSetting);
            }
            else if (para.Reverser == ReverserOption.NoRev)
            {
                totalDisMeter += reqData(para, dataColumn.NoRev, brakeSetting);
            }

            return totalDisMeter;

        }

        /// <summary>
        /// Gets the landing distance for the given landing parameters.
        /// </summary>
        public double GetLandingDistanceMeter(LandingParameters para)
        {
            return GetLandingDistanceMeter(para, para.AutoBrakeIndex);
        }
    }
}
