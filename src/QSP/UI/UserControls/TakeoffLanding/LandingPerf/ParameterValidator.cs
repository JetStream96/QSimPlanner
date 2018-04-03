using QSP.AviationTools;
using QSP.Common;
using QSP.LandingPerfCalculation;
using System;

namespace QSP.UI.UserControls.TakeoffLanding.LandingPerf
{
    public class ParameterValidator
    {
        private LandingPerfElements elements;

        public ParameterValidator(LandingPerfElements elements)
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
            int reverser = 0;
            int surfaceCondition = 0;
            int flapsIndex = 0;
            int brakeIndex = 0;

            try
            {
                weightKg = Convert.ToDouble(e.Weight.Text);

                if (e.WtUnit.SelectedIndex == 1) //LB
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

                if (e.LengthUnit.SelectedIndex == 1) // FT
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
                   Convert.ToDouble(e.RwyHeading.Text),
                   Convert.ToDouble(e.WindDirection.Text),
                   Convert.ToDouble(e.WindSpeed.Text));
            }
            catch
            {
                throw new InvalidUserInputException("Wind entry is not valid.");
            }

            try
            {
                slopePercent = Convert.ToDouble(e.Slope.Text);
            }
            catch
            {
                throw new InvalidUserInputException("Runway slope is not valid.");
            }

            try
            {
                tempCelsius = Convert.ToDouble(e.Oat.Text);

                if (e.TempUnit.SelectedIndex == 1) // deg F
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
                qnh = Convert.ToDouble(e.Pressure.Text);

                if (e.PressureUnit.SelectedIndex == 1)
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
                appSpeedIncrease = double.Parse(e.AppSpeedIncrease.Text);
            }
            catch
            {
                throw new InvalidUserInputException("APP speed increase is not valid.");
            }

            try
            {
                reverser = e.Reverser.SelectedIndex;
            }
            catch
            {
                throw new InvalidUserInputException("Reverser selection is not valid.");
            }

            try
            {
                surfaceCondition = e.SurfCond.SelectedIndex;
            }
            catch
            {
                throw new InvalidUserInputException("Surface condition is not valid.");
            }

            flapsIndex = e.Flaps.SelectedIndex;
            brakeIndex = e.Brake.SelectedIndex;

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
