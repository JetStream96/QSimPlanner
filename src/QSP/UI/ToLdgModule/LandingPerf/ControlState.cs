using System;
using System.Xml.Linq;
using static QSP.LibraryExtension.XmlSerialization.SerializationHelper;
using static QSP.Utilities.ExceptionHelpers;

namespace QSP.UI.ToLdgModule.LandingPerf
{
    public class ControlState
    {
        // Strings for xml tags.
        private string airportIcao = "AirportIcao";
        private string rwy = "Runway";
        private string lengthUnit = "LengthUnit";
        private string windDir = "WindDir";
        private string windSpeed = "WindSpeed";
        private string tempUnit = "TempUnit";
        private string oat = "OAT";
        private string pressUnit = "PressUnit";
        private string pressure = "Pressure";
        private string surfCond = "SurfCond";
        private string aircraft = "Aircraft";
        private string wtUnit = "WeightUnit";
        private string ldgWt = "LandingWeight";
        private string flaps = "Flaps";
        private string rev = "Reverser";
        private string brakes = "Brakes";
        private string appSpeedInc = "AppSpeedInc";

        private LandingPerfControl control;

        public ControlState(LandingPerfControl control)
        {
            this.control = control;
        }

        public XElement Save()
        {
            var a = control.airportInfoControl;
            var w = control.weatherInfoControl;
            var c = control;

            return new XElement("LandingPerfState", new XElement[]
            {
                a.airportTxtBox.Text.Serialize(airportIcao),
                a.rwyComboBox.Text.Serialize(rwy),
                a.lengthUnitComboBox.Text.Serialize(lengthUnit),
                w.windDirTxtBox.Text.Serialize(windDir),
                w.windSpdTxtBox.Text.Serialize(windSpeed),
                w.tempUnitComboBox.Text.Serialize(tempUnit),
                w.oatTxtBox.Text.Serialize(oat),
                w.pressUnitComboBox.Text.Serialize(pressUnit),
                w.pressTxtBox.Text.Serialize(pressure),
                w.surfCondComboBox.Text.Serialize(surfCond),
                c.acListComboBox.Text.Serialize(aircraft),
                c.wtUnitComboBox.Text.Serialize(wtUnit),
                c.weightTxtBox.Text.Serialize(ldgWt),
                c.flapsComboBox.Text.Serialize(flaps),
                c.revThrustComboBox.Text.Serialize(rev),
                c.brakeComboBox.Text.Serialize(brakes),
                c.appSpdIncTxtBox.Text.Serialize(appSpeedInc)
            });
        }

        public void Load(XDocument doc)
        {
            var r = doc.Root;
            var a = control.airportInfoControl;
            var w = control.weatherInfoControl;
            var c = control;

            // The order is important. E.g. "pressUnit" has to be 
            // loaded before "pressure", due to events handlers attached 
            // to pressure.TextChanged.
            Action[] actions =
            {
                () =>a.airportTxtBox.Text = r.GetString(airportIcao),
                () => a.rwyComboBox.Text = r.GetString(rwy),
                () => a.lengthUnitComboBox.Text = r.GetString(lengthUnit),
                () => w.windDirTxtBox.Text = r.GetString(windDir),
                () => w.windSpdTxtBox.Text = r.GetString(windSpeed),
                () => w.tempUnitComboBox.Text = r.GetString(tempUnit),
                () => w.oatTxtBox.Text = r.GetString(oat),
                () => w.pressUnitComboBox.Text = r.GetString(pressUnit),
                () => w.pressTxtBox.Text = r.GetString(pressure),
                () => w.surfCondComboBox.Text = r.GetString(surfCond),
                () => c.acListComboBox.Text = r.GetString(aircraft),
                () => c.wtUnitComboBox.Text = r.GetString(wtUnit),
                () => c.weightTxtBox.Text = r.GetString(ldgWt),
                () => c.flapsComboBox.Text = r.GetString(flaps),
                () => c.revThrustComboBox.Text = r.GetString(rev),
                () => c.brakeComboBox.Text = r.GetString(brakes),
                () => c.appSpdIncTxtBox.Text = r.GetString(appSpeedInc)
            };

            foreach (var action in actions) IgnoreException(action);
        }
    }
}
