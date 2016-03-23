using QSP.Core;
using QSP.LandingPerfCalculation.Boeing;
using QSP.LandingPerfCalculation.Boeing.PerfData;
using System;

namespace QSP.UI.ToLdgModule.LandingPerf.FormControllers
{
    public class BoeingParameterValidator
    {
        private LandingPerfElements elements;

        public BoeingParameterValidator(LandingPerfElements elements)
        {
            this.elements = elements;
        }

        /// <exception cref="InvalidUserInputException"></exception>
        public LandingParameters Validate()
        {//TODO: QNH??
            double WeightKG = 0.0;
            double RwyLengthMeter = 0.0;
            double ElevationFT = 0.0;
            int HeadwindKts = 0;
            double SlopePercent = 0.0;
            double TempCelsius = 0.0;
            int AppSpeedIncrease = 0;
            ReverserOption Reverser = default(ReverserOption);
            SurfaceCondition SurfaceCondition = default(SurfaceCondition);
            int FlapsIndex = 0;
            int BrakeIndex = 0;

            try
            {
                WeightKG = Convert.ToDouble(elements.weight.Text);

                if (elements.wtUnit.SelectedIndex == 1) //LB
                {
                    WeightKG *= AviationTools.Constants.LbKgRatio;
                }

                if (WeightKG < 0)
                {
                    throw new InvalidUserInputException("Landing weight is not valid.");
                }

            }
            catch
            {
                throw new InvalidUserInputException("Landing weight is not valid.");
            }

            try
            {
                RwyLengthMeter = Convert.ToDouble(elements.Length.Text);

                if (elements.lengthUnit.SelectedIndex == 1) // FT
                {
                    RwyLengthMeter *= AviationTools.Constants.FtMeterRatio;
                }

                if (RwyLengthMeter < 0)
                {
                    throw new InvalidUserInputException("Runway length is not valid.");
                }

            }
            catch
            {
                throw new InvalidUserInputException("Runway length is not valid.");
            }

            try
            {
                ElevationFT = Convert.ToDouble(elements.Elevation.Text);

                if (ElevationFT < -2000.0 || ElevationFT > 20000.0)
                {
                    throw new InvalidUserInputException("Runway elevation is not valid.");
                }
            }
            catch
            {
                throw new InvalidUserInputException("Runway elevation is not valid.");
            }

            try
            {
                HeadwindKts = (int)(
                    Math.Cos(
                        MathTools.Utilities.ToRadian(
                            Convert.ToDouble(elements.rwyHeading.Text) -
                            Convert.ToDouble(elements.windDirection.Text)))
                            * Convert.ToDouble(elements.windSpeed.Text));
            }
            catch
            {
                throw new InvalidUserInputException("Wind entry is not valid.");
            }

            try
            {
                SlopePercent = Convert.ToDouble(elements.slope.Text);
            }
            catch
            {
                throw new InvalidUserInputException("Runway slope is not valid.");
            }

            try
            {
                TempCelsius = Convert.ToDouble(elements.oat.Text);

                if (elements.tempUnit.SelectedIndex == 1) // deg F
                {
                    TempCelsius = AviationTools.CoversionTools.ToCelsius(TempCelsius);
                }
            }
            catch
            {
                throw new InvalidUserInputException("OAT is not valid.");
            }

            try
            {
                AppSpeedIncrease = (int)Convert.ToDouble(elements.appSpeedIncrease.Text);
            }
            catch
            {
                throw new InvalidUserInputException("APP speed increase is not valid.");
            }

            try
            {
                Reverser = (ReverserOption)elements.reverser.SelectedIndex;
            }
            catch
            {
                throw new InvalidUserInputException("Reverser selection is not valid.");
            }

            try
            {
                SurfaceCondition = (SurfaceCondition)elements.surfCond.SelectedIndex;
            }
            catch
            {
                throw new InvalidUserInputException("Surface condition is not valid.");
            }


            FlapsIndex = elements.flaps.SelectedIndex;
            BrakeIndex = elements.brake.SelectedIndex;

            return new LandingParameters(
                (int)WeightKG,
                (int)RwyLengthMeter,
                (int)ElevationFT,
                HeadwindKts,
                SlopePercent,
                (int)TempCelsius,
                AppSpeedIncrease,
                Reverser,
                SurfaceCondition,
                FlapsIndex,
                BrakeIndex);
        }
    }
}
