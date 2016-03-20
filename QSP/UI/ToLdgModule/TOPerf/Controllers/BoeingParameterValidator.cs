using QSP.Core;
using QSP.TOPerfCalculation.Boeing;
using QSP.TOPerfCalculation.Boeing.PerfData;
using System;

namespace QSP.UI.ToLdgModule.TOPerf.Controllers
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
            double WeightKG = 0.0;
            double RwyLengthMeter = 0.0;
            double ElevationFT = 0.0;
            double SlopePercent = 0.0;
            double TempCelsius = 0.0;
            double rwyHeading = 0;
            double windHeading = 0;
            double windSpeed = 0;
            double QHH = 0;
            int thrustRating = 0;
            bool surfaceWet = false;
            int FlapsIndex = 0;
            AntiIceOption antiIce = default(AntiIceOption);
            bool packOn = false;

            try
            {
                WeightKG = Convert.ToDouble(elements.weight.Text);

                if (elements.wtUnit.SelectedIndex == 1) //LB
                {
                    WeightKG *= AviationTools.Constants.LbKgRatio;
                }

                if (WeightKG < 0)
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
                rwyHeading = Convert.ToDouble(elements.rwyHeading.Text);
            }
            catch
            {
                throw new InvalidUserInputException("Runway heading is not valid.");
            }

            try
            {
                windHeading = Convert.ToDouble(elements.windDirection.Text);
            }
            catch
            {
                throw new InvalidUserInputException("Wind direction is not valid.");
            }

            try
            {
                windSpeed = Convert.ToDouble(elements.windSpeed.Text);
                if (windSpeed < -60.0 || windSpeed > 60.0)
                {
                    throw new InvalidUserInputException("Wind speed is not valid.");
                }
            }
            catch
            {
                throw new InvalidUserInputException("Wind speed is not valid.");
            }

            try
            {
                QHH = Convert.ToDouble(elements.pressure.Text);

                if (elements.pressureUnit.SelectedIndex == 1)
                {
                    QHH *= 1013.0 / 29.92;
                }

                if (QHH < 900.0 | QHH > 1100.0)
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
                surfaceWet = elements.surfCond.SelectedIndex != 0;
            }
            catch
            {
                throw new InvalidUserInputException("Surface condition is not valid.");
            }

            try
            {
                thrustRating = elements.thrustRating.SelectedIndex;

                if (elements.thrustRating.Visible && thrustRating < 0)
                {
                    throw new InvalidUserInputException("Thrust rating is invalid.");
                }
            }
            catch
            {
                throw new InvalidUserInputException("Thrust rating is invalid.");
            }

            FlapsIndex = elements.flaps.SelectedIndex;

            try
            {
                switch (elements.AntiIce.SelectedIndex)
                {

                    case 0:
                        antiIce = AntiIceOption.Off;
                        break;
                    case 1:
                        antiIce = AntiIceOption.Engine;
                        break;

                    case 2:
                        antiIce = AntiIceOption.EngAndWing;
                        break;

                    default:
                        throw new InvalidUserInputException("Anti-ice mode is not valid.");
                }
            }
            catch
            {
                throw new InvalidUserInputException("Anti-ice mode is not valid.");
            }

            try
            {
                packOn = elements.Packs.SelectedIndex == 0;
            }
            catch
            {
                throw new InvalidUserInputException("Packs selection is not valid.");
            }

            return new TOParameters(
                RwyLengthMeter,
                ElevationFT,
                rwyHeading,
                SlopePercent,
                windHeading,
                windSpeed,
                TempCelsius,
                QHH,
                surfaceWet,
                WeightKG,
                thrustRating,
                antiIce,
                packOn,
                FlapsIndex);
        }
    }
}
