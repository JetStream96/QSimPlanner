using QSP.Common;
using QSP.FuelCalculation.FuelData;
using QSP.UI.Controllers.Units;
using static QSP.UI.Utilities.InputValidator;

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
            double ZfwKg = ImportRequireNonNegative(c.Zfw, "Invalid ZFW.");
            double ContPerc = ParseNonNegative(
                c.ContPercentComboBox, "Invalid contingency fuel.");

            double MissedAppFuelKg = ImportRequireNonNegative(
                c.MissedApproach, "Invalid missed approach fuel.");

            double HoldingMin = ParseNonNegative(
                c.HoldTimeTxtBox, "Invalid holding time.");

            double ExtraFuelKg = ImportRequireNonNegative(
                c.Extra, "Invalid extra fuel.");

            double ApuTime = ParseNonNegative(
                c.ApuTimeTxtBox, "Invalid APU time.");

            double TaxiTime = ParseNonNegative(
                c.TaxiTimeTxtBox, "Invalid taxi time.");

            double FinalRsvMin = ParseNonNegative(
                c.FinalReserveTxtBox, "Invalid final reserve time.");

            var ac = c.GetCurrentAircraft();
            if (ac == null)
            {
                throw new InvalidUserInputException("No aircraft selected.");
            }

            return new FuelParameters(
                ZfwKg,
                ContPerc,
                MissedAppFuelKg,
                HoldingMin,
                ExtraFuelKg,
                ApuTime,
                TaxiTime,
                FinalRsvMin,
                ac.Config.FuelBias,
                c.GetFuelData());
        }

        private static double ImportRequireNonNegative(
            WeightTextBoxController weightControl, string exceptionMsg)
        {
            double weightKg;

            try
            {
                weightKg = weightControl.GetWeightKg();
            }
            catch
            {
                throw new InvalidUserInputException(exceptionMsg);
            }

            if (weightKg < 0.0)
            {
                throw new InvalidUserInputException(exceptionMsg);
            }

            return weightKg;
        }
    }
}

