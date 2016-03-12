using System;
using QSP.AviationTools;
using static QSP.UI.FormInstanceGetter;
using QSP.Core;

namespace QSP.TakeOffPerfCalculation
{

    public static class ParameterImporter
    {

        public static TOPerfParameters Import()
        {
            var frm = MainFormInstance();
            int lengthMeter = 0;
            int elevationFt = 0;
            int rwyHeading = 0;
            double rwySlope = 0;
            int windHeading = 0;
            int windSpeed = 0;
            int oatCelsius = 0;
            double QHH = 0;
            bool surfaceWet = false;
            int towtKG = 0;
            ThrustRatingOption thrustRating = default(ThrustRatingOption);
            AntiIceOption antiIce = default(AntiIceOption);
            bool packOn = false;

            try
            {
                lengthMeter = (int)(Convert.ToDouble(frm.length.Text) * (frm.m_ft.Text == "M" ? 1.0 : Constants.FT_M_ratio));
                if (lengthMeter < 0)
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
                elevationFt = (int)Convert.ToDouble(frm.elevation.Text);

                if (elevationFt < -2000 | elevationFt > 20000)
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
                rwyHeading = Convert.ToInt32(frm.RwyHeading.Text);
            }
            catch
            {
                throw new InvalidUserInputException("Runway heading is not valid.");
            }

            try
            {
                rwySlope = Convert.ToDouble(frm.Slope.Text);
            }
            catch
            {
                throw new InvalidUserInputException("Runway slope is not valid.");
            }

            try
            {
                windHeading = Convert.ToInt32(frm.winddir.Text);
            }
            catch
            {
                throw new InvalidUserInputException("Wind direction is not valid.");
            }


            try
            {
                windSpeed = Convert.ToInt32(frm.windspd.Text);
                if (windSpeed < -60 | windSpeed > 60)
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
                oatCelsius = Convert.ToInt32(frm.OAT.Text);
                if (frm.temp_c_f.SelectedIndex == 1)
                {
                    oatCelsius = (int)CoversionTools.ToCelsius(oatCelsius);
                }

                if (oatCelsius < -150 | oatCelsius > 100)
                {
                    throw new InvalidUserInputException("OAT is not valid.");
                }

            }
            catch
            {
                throw new InvalidUserInputException("OAT is not valid.");
            }


            try
            {
                QHH = Convert.ToInt32(frm.altimeter.Text);
                if (frm.hpa_inHg.SelectedIndex == 1)
                {
                    QHH *= 1013 / 29.92;
                }

                if (QHH < 900 | QHH > 1100)
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
                surfaceWet = frm.surf_cond.SelectedIndex == 0 ? false : true;
            }
            catch
            {
                throw new InvalidUserInputException("Surface condition is not valid.");
            }


            try
            {
                towtKG = Convert.ToInt32(frm.Weight.Text);
                if (frm.WTunit.Text == "LB")
                {
                    towtKG = (int)(towtKG * Constants.LB_KG);
                }

                if (towtKG < 0)
                {
                    throw new InvalidUserInputException("Take off weight is not valid.");
                }

            }
            catch
            {
                throw new InvalidUserInputException("Take off weight is not valid.");
            }


            try
            {
                switch (frm.ThrustRating_Box.Text)
                {

                    case "TO":
                        thrustRating = ThrustRatingOption.Normal;

                        break;
                    case "TO1":
                        thrustRating = ThrustRatingOption.TO1;

                        break;
                    default:
                        //TO2
                        thrustRating = ThrustRatingOption.TO2;

                        break;
                }

            }
            catch
            {
                throw new InvalidUserInputException("Thrust rating is not valid.");
            }


            try
            {
                switch (frm.AISel.SelectedIndex)
                {

                    case 0:
                        antiIce = AntiIceOption.Off;
                        break;
                    case 1:
                        antiIce = AntiIceOption.Engine;
                        break;
                    default:
                        //2
                        antiIce = AntiIceOption.EngAndWing;

                        break;
                }

            }
            catch
            {
                throw new InvalidUserInputException("Anti-ice mode is not valid.");
            }

            try
            {
                packOn = frm.PacksSel.Text == "ON" ? true : false;
            }
            catch
            {
                throw new InvalidUserInputException("Packs selection is not valid.");
            }

            return new TOPerfParameters(lengthMeter, elevationFt, rwyHeading, rwySlope, windHeading,
                                        windSpeed, oatCelsius, (int)QHH, surfaceWet, towtKG,
                                        thrustRating, antiIce, packOn);
                
        }

    }

}
