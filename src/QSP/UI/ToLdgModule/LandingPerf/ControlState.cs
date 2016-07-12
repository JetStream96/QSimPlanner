using System.Xml.Linq;

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
            var airport = control.airportInfoControl;
            var weather = control.weatherInfoControl;

            return new XElement("LandingPerfState", new XElement[]
            {
                new XElement(airportIcao,airport.airportTxtBox.Text),
                new XElement(rwy,airport.rwyComboBox.Text),
                new XElement(lengthUnit,airport.lengthUnitComboBox.Text),
                new XElement(windDir,weather.windDirTxtBox.Text),
                new XElement(windSpeed,weather.windSpdTxtBox.Text),
                new XElement(tempUnit,weather.tempUnitComboBox.Text),
                new XElement(oat,weather.oatTxtBox.Text),
                new XElement(pressUnit,weather.pressUnitComboBox.Text),
                new XElement(pressure,weather.pressTxtBox.Text),
                new XElement(surfCond,control.weatherInfoControl.surfCondComboBox.Text),
                new XElement(aircraft,control.acListComboBox.Text),
                new XElement(wtUnit,control.wtUnitComboBox.Text),
                new XElement(ldgWt,control.weightTxtBox.Text),
                new XElement(flaps,control.flapsComboBox.Text),
                new XElement(rev,control.revThrustComboBox.Text),
                new XElement(brakes,control.brakeComboBox.Text),
                new XElement(appSpeedInc,control.appSpdIncTxtBox.Text)
            });
        }

        public void Load(XDocument doc)
        {
            var root = doc.Root;
            var airport = control.airportInfoControl;
            var weather = control.weatherInfoControl;

            // The order is important. E.g. "pressUnit" has to be 
            // loaded before "pressure", due to events handlers attached 
            // to pressure.TextChanged.
            airport.airportTxtBox.Text = GetValue(root, airportIcao);
            airport.rwyComboBox.Text = GetValue(root, rwy);
            airport.lengthUnitComboBox.Text = GetValue(root, lengthUnit);
            weather.windDirTxtBox.Text = GetValue(root, windDir);
            weather.windSpdTxtBox.Text = GetValue(root, windSpeed);
            weather.tempUnitComboBox.Text = GetValue(root, tempUnit);
            weather.oatTxtBox.Text = GetValue(root, oat);
            weather.pressUnitComboBox.Text = GetValue(root, pressUnit);
            weather.pressTxtBox.Text = GetValue(root, pressure);
            control.weatherInfoControl.surfCondComboBox.Text = GetValue(root, surfCond);
            control.acListComboBox.Text = GetValue(root, aircraft);
            control.wtUnitComboBox.Text = GetValue(root, wtUnit);
            control.weightTxtBox.Text = GetValue(root, ldgWt);
            control.flapsComboBox.Text = GetValue(root, flaps);
            control.revThrustComboBox.Text = GetValue(root, rev);
            control.brakeComboBox.Text = GetValue(root, brakes);
            control.appSpdIncTxtBox.Text = GetValue(root, appSpeedInc);
        }

        private static string GetValue(XElement root, string key)
        {
            return root.Element(key).Value;
        }
    }
}
