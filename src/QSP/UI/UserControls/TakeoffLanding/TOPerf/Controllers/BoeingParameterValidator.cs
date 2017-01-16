using System;
using QSP.Common;
using QSP.TOPerfCalculation.Boeing;
using QSP.TOPerfCalculation.Boeing.PerfData;

namespace QSP.UI.UserControls.TakeoffLanding.TOPerf.Controllers
{
    public class BoeingParameterValidator
    {
        private TOPerfElements elements;

        public BoeingParameterValidator(TOPerfElements elements)
        {
            this.elements = elements;
        }

        /// <exception cref="InvalidUserInputException"></exception>
        public TOParameters Validate()
        {
            double weightKg = 0.0;
            double rwyLengthMeter = 0.0;
            double tempCelsius = 0.0;
            double QNH = 0;

            try
            {
                weightKg = Convert.ToDouble(elements.Weight.Text);

                if (elements.WtUnit.SelectedIndex == 1) //LB
                {
                    weightKg *= AviationTools.Constants.LbKgRatio;
                }

                if (weightKg < 0)
                {
                    throw new InvalidUserInputException("Takeoff weight is not valid.");
                }

            }
            catch
            {
                throw new InvalidUserInputException("Takeoff weight is not valid.");
            }

            try
            {
                rwyLengthMeter = Convert.ToDouble(elements.Length.Text);

                if (elements.lengthUnit.SelectedIndex == 1) // FT
                {
                    rwyLengthMeter *= AviationTools.Constants.FtMeterRatio;
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

            double elevationFt;
            if (!double.TryParse(elements.Elevation.Text, out elevationFt) ||
                elevationFt < -2000.0 || elevationFt > 25000.0)
            {
                throw new InvalidUserInputException("Runway elevation is not valid.");
            }

            double rwyHeading;
            if (!double.TryParse(elements.RwyHeading.Text, out rwyHeading))
            {
                throw new InvalidUserInputException("Runway heading is not valid.");
            }

            double windHeading;
            if (!double.TryParse(elements.WindDirection.Text, out windHeading))
            {
                throw new InvalidUserInputException("Wind direction is not valid.");
            }

            double windSpeed;
            if (!double.TryParse(elements.WindSpeed.Text, out windSpeed) ||
                windSpeed < -200.0 || windSpeed > 200.0)
            {
                throw new InvalidUserInputException("Wind speed is not valid.");
            }

            try
            {
                QNH = Convert.ToDouble(elements.Pressure.Text);

                if (elements.PressureUnit.SelectedIndex == 1)
                {
                    QNH *= 1013.0 / 29.92;
                }

                if (QNH < 900.0 | QNH > 1100.0)
                {
                    throw new InvalidUserInputException("Altimeter setting is not valid.");
                }
            }
            catch
            {
                throw new InvalidUserInputException("Altimeter setting is not valid.");
            }

            double slopePercent;
            if (!double.TryParse(elements.Slope.Text, out slopePercent))
            {
                throw new InvalidUserInputException("Runway slope is not valid.");
            }

            try
            {
                tempCelsius = Convert.ToDouble(elements.Oat.Text);

                if (elements.TempUnit.SelectedIndex == 1) // deg F
                {
                    tempCelsius = AviationTools.ConversionTools.ToCelsius(tempCelsius);
                }
            }
            catch
            {
                throw new InvalidUserInputException("OAT is not valid.");
            }

            var surfaceWet = elements.SurfCond.SelectedIndex != 0;
            var thrustRating = elements.ThrustRating.SelectedIndex;

            if (elements.ThrustRating.Visible && thrustRating < 0)
            {
                throw new InvalidUserInputException("Thrust rating is invalid.");
            }

            var flapsIndex = elements.Flaps.SelectedIndex;

            AntiIceOption antiIce = default(AntiIceOption);
            try
            {
                antiIce = (AntiIceOption)elements.AntiIce.SelectedIndex;

            }
            catch
            {
                throw new InvalidUserInputException("Anti-ice setting is invalid.");
            }

            var packOn = elements.Packs.SelectedIndex == 0;

            return new TOParameters(
                rwyLengthMeter,
                elevationFt,
                rwyHeading,
                slopePercent,
                windHeading,
                windSpeed,
                tempCelsius,
                QNH,
                surfaceWet,
                weightKg,
                thrustRating,
                antiIce,
                packOn,
                flapsIndex);
        }
    }
}
