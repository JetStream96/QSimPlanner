using QSP.Common;
using QSP.FuelCalculation;
using QSP.RouteFinding;
using QSP.UI.Controllers.Units;
using QSP.Utilities.Units;
using System;
using System.Windows.Forms;
using static QSP.AviationTools.Constants;
using static QSP.UI.FormInstanceGetter;

namespace QSP.UI.UserControls
{
    public class FuelParameterValidator
    {
        private FuelPlanningControl control;

        public FuelParameterValidator(FuelPlanningControl control)
        {
            this.control = control;
        }

        /// <exception cref="InvalidUserInputException"></exception>
        public FuelParameters Validate()
        {
            var c = control;

            var WtUnit = c.WeightUnit;
            double ZfwKg = ImportPattern1(c.Zfw, "Invalid ZFW.");
            double ContPerc = ImportPattern2(
                c.ContPercentComboBox, "Invalid contingency fuel.");

            double MissedAppFuelKg = ImportPattern1(
                c.MissedApproach, "Invalid missed approach fuel.");

            double HoldingMin = ImportPattern2(
                c.HoldTimeTxtBox, "Invalid holding time.");

            double ExtraFuelKg =
                ImportPattern1(c.Extra, "Invalid extra fuel.");

            double ApuTime = ImportPattern2(
                c.ApuTimeTxtBox, "Invalid APU time.");

            double TaxiTime = ImportPattern2(
                c.TaxiTimeTxtBox, "Invalid taxi time.");

            double FinalRsvMin = ImportPattern2(
                c.FinalReserveTxtBox, "Invalid time for final reserve.");

            return new FuelParameters(
                ZfwKg,
                ContPerc,
                MissedAppFuelKg,
                HoldingMin,
                ExtraFuelKg,
                ApuTime,
                TaxiTime,
                FinalRsvMin,
                0.0,
                0.0,
                0.0,
                0.0,
                WtUnit,
                null);
        }

        private static double ImportPattern1(
            WeightTextBoxController weightControl, string exceptionMsg)
        {
            try
            {
                double weightKg = weightControl.GetWeightKg();

                if (weightKg >= 0.0)
                {
                    return weightKg;
                }
            }
            catch { }
            finally
            {
                throw new InvalidUserInputException(exceptionMsg);
            }
        }

        private static double ImportPattern2(
            object control, string exceptionMsg)
        {
            try
            {
                double weightKg = double.Parse(GetText(control));

                if (weightKg >= 0.0)
                {
                    return weightKg;
                }
            }
            catch { }
            finally
            {
                throw new InvalidUserInputException(exceptionMsg);
            }
        }

        private static string GetText(object control)
        {
            if (control is TextBox)
            {
                return ((TextBox)control).Text;
            }
            else if (control is ComboBox)
            {
                return ((ComboBox)control).Text;
            }

            throw new ArgumentException();
        }
    }
}

