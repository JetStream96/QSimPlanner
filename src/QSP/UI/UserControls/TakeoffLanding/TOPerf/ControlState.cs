using System;
using System.Xml.Linq;
using QSP.LibraryExtension.XmlSerialization;
using QSP.Utilities;

namespace QSP.UI.UserControls.TakeoffLanding.TOPerf
{
    public class ControlState
    {
        // Strings for xml tags.
        private static readonly string airportIcao = "AirportIcao";
        private static readonly string rwy = "Runway";
        private static readonly string lengthUnit = "LengthUnit";
        private static readonly string windDir = "WindDir";
        private static readonly string windSpeed = "WindSpeed";
        private static readonly string tempUnit = "TempUnit";
        private static readonly string oat = "OAT";
        private static readonly string pressUnit = "PressUnit";
        private static readonly string pressure = "Pressure";
        private static readonly string surfCond = "SurfCond";
        private static readonly string aircraft = "Aircraft";
        private static readonly string wtUnit = "WeightUnit";
        private static readonly string toWt = "TakeOffWeight";
        private static readonly string flaps = "Flaps";
        private static readonly string thrustRating = "ThrustRating";
        private static readonly string antiIce = "AntiIce";
        private static readonly string packs = "Packs";

        private TOPerfControl control;

        public ControlState(TOPerfControl control)
        {
            this.control = control;
        }

        public XElement Save()
        {
            var c = control;
            var a = control.airportInfoControl;
            var w = control.weatherInfoControl;

            return new XElement("TakeoffPerfState", new XElement[]
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
                c.weightTxtBox.Text.Serialize(toWt),
                c.flapsComboBox.Text.Serialize(flaps),
                c.thrustRatingComboBox.Text.Serialize(thrustRating),
                c.antiIceComboBox.Text.Serialize(antiIce),
                c.packsComboBox.Text.Serialize(packs)
            });
        }

        public void Load(XDocument doc)
        {
            var r = doc.Root;
            var c = control;
            var a = control.airportInfoControl;
            var w = control.weatherInfoControl;

            // The order is important. E.g. "pressUnit" has to be 
            // loaded before "pressure", due to events handlers attached 
            // to pressure.TextChanged.
            Action[] actions =
            {
                () => a.airportTxtBox.Text = r.GetString(airportIcao),
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
                () => c.weightTxtBox.Text = r.GetString(toWt),
                () => c.flapsComboBox.Text = r.GetString(flaps),
                () => c.thrustRatingComboBox.Text = r.GetString(thrustRating),
                () => c.antiIceComboBox.Text = r.GetString(antiIce),
                () => c.packsComboBox.Text = r.GetString(packs)
            };

            foreach (var action in actions) ExceptionHelpers.IgnoreException(action);
        }
    }
}
