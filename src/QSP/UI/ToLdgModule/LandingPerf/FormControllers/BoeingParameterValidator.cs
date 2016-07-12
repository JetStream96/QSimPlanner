using QSP.Common;
using QSP.LandingPerfCalculation.Boeing;
using QSP.LandingPerfCalculation.Boeing.PerfData;
using System;
using static QSP.MathTools.Angles;
using static QSP.AviationTools.CoversionTools;
using static QSP.AviationTools.Constants;

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
        {
            var e = elements;

            double WeightKG = 0.0;
            double RwyLengthMeter = 0.0;
            double ElevationFT = 0.0;
            double HeadwindKts = 0.0;
            double SlopePercent = 0.0;
            double TempCelsius = 0.0;
            double QNH = 0.0;
            double AppSpeedIncrease = 0.0;
            ReverserOption Reverser = default(ReverserOption);
            SurfaceCondition SurfaceCondition = default(SurfaceCondition);
            int FlapsIndex = 0;
            int BrakeIndex = 0;

            try
            {
                WeightKG = Convert.ToDouble(e.weight.Text);

                if (e.wtUnit.SelectedIndex == 1) //LB
                {
                    WeightKG *= LbKgRatio;
                }

                if (WeightKG < 0)
                {
                    throw new InvalidUserInputException(
                        "Landing weight is not valid.");
                }

            }
            catch
            {
                throw new InvalidUserInputException(
                    "Landing weight is not valid.");
            }

            try
            {
                RwyLengthMeter = Convert.ToDouble(e.Length.Text);

                if (e.lengthUnit.SelectedIndex == 1) // FT
                {
                    RwyLengthMeter *= FtMeterRatio;
                }

                if (RwyLengthMeter < 0)
                {
                    throw new InvalidUserInputException(
                        "Runway length is not valid.");
                }

            }
            catch
            {
                throw new InvalidUserInputException(
                    "Runway length is not valid.");
            }

            try
            {
                ElevationFT = Convert.ToDouble(e.Elevation.Text);

                if (ElevationFT < -2000.0 || ElevationFT > 20000.0)
                {
                    throw new InvalidUserInputException(
                        "Runway elevation is not valid.");
                }
            }
            catch
            {
                throw new InvalidUserInputException(
                    "Runway elevation is not valid.");
            }

            try
            {
                HeadwindKts =
                    Math.Cos(
                        ToRadian(
                            Convert.ToDouble(e.rwyHeading.Text) -
                            Convert.ToDouble(e.windDirection.Text)))
                            * Convert.ToDouble(e.windSpeed.Text);
            }
            catch
            {
                throw new InvalidUserInputException(
                    "Wind entry is not valid.");
            }

            try
            {
                SlopePercent = Convert.ToDouble(e.slope.Text);
            }
            catch
            {
                throw new InvalidUserInputException(
                    "Runway slope is not valid.");
            }

            try
            {
                TempCelsius = Convert.ToDouble(e.oat.Text);

                if (e.tempUnit.SelectedIndex == 1) // deg F
                {
                    TempCelsius = ToCelsius(TempCelsius);
                }
            }
            catch
            {
                throw new InvalidUserInputException("OAT is not valid.");
            }

            try
            {
                QNH = Convert.ToDouble(e.pressure.Text);

                if (e.pressureUnit.SelectedIndex == 1)
                {
                    QNH *= 1013.0 / 29.92;
                }

                if (QNH < 900.0 | QNH > 1100.0)
                {
                    throw new InvalidUserInputException(
                        "Altimeter setting is not valid.");
                }
            }
            catch
            {
                throw new InvalidUserInputException(
                    "Altimeter setting is not valid.");
            }

            try
            {
                AppSpeedIncrease = double.Parse(e.appSpeedIncrease.Text);
            }
            catch
            {
                throw new InvalidUserInputException(
                    "APP speed increase is not valid.");
            }

            try
            {
                Reverser = (ReverserOption)e.reverser.SelectedIndex;
            }
            catch
            {
                throw new InvalidUserInputException(
                    "Reverser selection is not valid.");
            }

            try
            {
                SurfaceCondition = (SurfaceCondition)e.surfCond.SelectedIndex;
            }
            catch
            {
                throw new InvalidUserInputException(
                    "Surface condition is not valid.");
            }
            
            FlapsIndex = e.flaps.SelectedIndex;
            BrakeIndex = e.brake.SelectedIndex;

            return new LandingParameters(
                WeightKG,
                RwyLengthMeter,
                ElevationFT,
                HeadwindKts,
                SlopePercent,
                TempCelsius,
                QNH,
                AppSpeedIncrease,
                Reverser,
                SurfaceCondition,
                FlapsIndex,
                BrakeIndex);
        }
    }
}
