using QSP.AircraftProfiles.Configs;
using QSP.AviationTools;
using QSP.Common;
using QSP.Utilities.Units;
using static QSP.UI.Utilities.InputValidator;

namespace QSP.UI.ToLdgModule.AircraftMenu
{
    public class AcConfigValidator
    {
        private AcMenuElements elem;

        public AcConfigValidator(AcMenuElements elem)
        {
            this.elem = elem;
        }
        
        /// <exception cref="InvalidUserInputException"></exception>
        public AircraftConfigItem Read()
        {
            var wtUnit = (WeightUnit)elem.WeightUnitCBox.SelectedIndex;

            double wtUnitFactor = wtUnit == WeightUnit.KG ?
                    1.0 : Constants.LbKgRatio;

            double oew = ParseNonNegative(elem.Oew, "Invalid OEW.");
            double maxTOWt = ParseNonNegative(
                elem.MaxToWt, "Invalid max takeoff weight.");
            double maxLdgWt = ParseNonNegative(
                elem.MaxLdgWt, "Invalid max landing weight.");
            double maxZfw = ParseNonNegative(
                elem.MaxZfw, "Invalid max zero fuel weight.");
            double maxFuel = ParseNonNegative(
                elem.MaxFuel, "Invalid max fuel capacity.");
            double fuelBias = ParseNonNegative(
                elem.Bias, "Invalid fuel bias.");
            
            oew *= wtUnitFactor;
            maxTOWt *= wtUnitFactor;
            maxLdgWt *= wtUnitFactor;
            maxZfw *= wtUnitFactor;
            maxFuel *= wtUnitFactor;
            fuelBias /= 100.0;

            return new AircraftConfigItem(
                elem.AcType.Text.Trim(),
                elem.Registration.Text.Trim().ToUpper(),
                elem.FuelProfile.Text,
                elem.ToProfile.Text,
                elem.LdgProfile.Text,
                oew,
                maxTOWt,
                maxLdgWt,
                maxZfw,
                maxFuel,
                fuelBias,
                wtUnit);
        }

        private static void CheckRange(AircraftConfigItem item)
        {
            Ensure(item.OewKg < item.MaxZfwKg,
                "Max ZFW must be larger than OEW.");
            Ensure(item.OewKg < item.MaxTOWtKg,
                "Max takeoff weight must be larger than OEW.");
            Ensure(item.OewKg < item.MaxLdgWtKg,
                "Max landing weight must be larger than OEW.");           
        }

        private static void Ensure(bool condition, string message)
        {
            if (!condition) throw new InvalidUserInputException(message);
        }

        /// <exception cref="InvalidUserInputException"></exception>
        public AircraftConfigItem Validate()
        {
            var item = Read();

            if (item.AC == "")
            {
                throw new InvalidUserInputException(
                    "Aircraft type cannot be empty.");
            }

            if (item.Registration == "")
            {
                throw new InvalidUserInputException(
                    "Registration cannot be empty.");
            }

            CheckRange(item);

            return item;
        }
    }
}
