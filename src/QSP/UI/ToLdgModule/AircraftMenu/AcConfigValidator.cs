using QSP.AircraftProfiles.Configs;
using QSP.AviationTools;
using QSP.Common;
using QSP.Utilities.Units;

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
            var wtUnit = elem.ZfwUnit.SelectedIndex == 0 ?
                            WeightUnit.KG :
                            WeightUnit.LB;

            double wtUnitFactor = elem.ZfwUnit.SelectedIndex == 0 ?
                                    1.0 :
                                    Constants.LbKgRatio;

            double zfw;
            double maxToWt;
            double maxLdgWt;

            if (double.TryParse(elem.Zfw.Text, out zfw) == false)
            {
                throw new InvalidUserInputException("Invalid ZFW.");
            }

            if (double.TryParse(elem.MaxToWt.Text, out maxToWt) == false)
            {
                throw new InvalidUserInputException(
                    "Invalid max takeoff weight.");
            }

            if (double.TryParse(elem.MaxLdgWt.Text, out maxLdgWt) == false)
            {
                throw new InvalidUserInputException(
                    "Invalid max landing weight.");
            }

            zfw *= wtUnitFactor;
            maxToWt *= wtUnitFactor;
            maxLdgWt *= wtUnitFactor;

            return new AircraftConfigItem(
                elem.AcType.Text.Trim(),
                elem.Registration.Text.Trim().ToUpper(),
                elem.ToProfile.Text,
                elem.LdgProfile.Text,
                zfw,
                maxToWt,
                maxLdgWt,
                wtUnit);
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

            return item;
        }
    }
}
