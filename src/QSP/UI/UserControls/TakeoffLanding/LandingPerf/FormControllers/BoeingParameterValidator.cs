using System;
using QSP.AviationTools;
using QSP.Common;
using QSP.LandingPerfCalculation.Boeing;
using QSP.LandingPerfCalculation.Boeing.PerfData;

namespace QSP.UI.UserControls.TakeoffLanding.LandingPerf.FormControllers
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

            double weightKg = 0.0;
            double rwyLengthMeter = 0.0;
            double elevationFt = 0.0;
            double headwindKts = 0.0;
            double slopePercent = 0.0;
            double tempCelsius = 0.0;
            double qnh = 0.0;
            double appSpeedIncrease = 0.0;
            ReverserOption reverser = default(ReverserOption);
            SurfaceCondition surfaceCondition = default(SurfaceCondition);
            int flapsIndex = 0;
            int brakeIndex = 0;

            try
            {
                weightKg = Convert.ToDouble(e.weight.Text);

                if (e.wtUnit.SelectedIndex == 1) //LB
                {
                    weightKg *= Constants.LbKgRatio;
                }

                if (weightKg < 0)
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
                rwyLengthMeter = Convert.ToDouble(e.Length.Text);

                if (e.lengthUnit.SelectedIndex == 1) // FT
                {
                    rwyLengthMeter *= Constants.FtMeterRatio;
                }

                if (rwyLengthMeter < 0)
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
                elevationFt = Convert.ToDouble(e.Elevation.Text);

                if (elevationFt < -2000.0 || elevationFt > 20000.0)
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
                headwindKts = ConversionTools.HeadwindComponent(
                   Convert.ToDouble(e.rwyHeading.Text),
                   Convert.ToDouble(e.windDirection.Text),
                   Convert.ToDouble(e.windSpeed.Text));
            }
            catch
            {
                throw new InvalidUserInputException("Wind entry is not valid.");
            }

            try
            {
                slopePercent = Convert.ToDouble(e.slope.Text);
            }
            catch
            {
                throw new InvalidUserInputException("Runway slope is not valid.");
            }

            try
            {
                tempCelsius = Convert.ToDouble(e.oat.Text);

                if (e.tempUnit.SelectedIndex == 1) // deg F
                {
                    tempCelsius = ConversionTools.ToCelsius(tempCelsius);
                }
            }
            catch
            {
                throw new InvalidUserInputException("OAT is not valid.");
            }

            try
            {
                qnh = Convert.ToDouble(e.pressure.Text);

                if (e.pressureUnit.SelectedIndex == 1)
                {
                    qnh *= 1013.0 / 29.92;
                }

                if (qnh < 900.0 | qnh > 1100.0)
                {
                    throw new InvalidUserInputException("Altimeter setting is not valid.");
                }
            }
            catch
            {
                throw new InvalidUserInputException("Altimeter setting is not valid.");
            }

            try
            {
                appSpeedIncrease = double.Parse(e.appSpeedIncrease.Text);
            }
            catch
            {
                throw new InvalidUserInputException("APP speed increase is not valid.");
            }

            try
            {
                reverser = (ReverserOption)e.reverser.SelectedIndex;
            }
            catch
            {
                throw new InvalidUserInputException("Reverser selection is not valid.");
            }

            try
            {
                surfaceCondition = (SurfaceCondition)e.surfCond.SelectedIndex;
            }
            catch
            {
                throw new InvalidUserInputException("Surface condition is not valid.");
            }

            flapsIndex = e.flaps.SelectedIndex;
            brakeIndex = e.brake.SelectedIndex;

            return new LandingParameters(
                weightKg,
                rwyLengthMeter,
                elevationFt,
                headwindKts,
                slopePercent,
                tempCelsius,
                qnh,
                appSpeedIncrease,
                reverser,
                surfaceCondition,
                flapsIndex,
                brakeIndex);
        }
    }
}
