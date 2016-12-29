using System;
using System.Xml.Linq;
using static QSP.LibraryExtension.XmlSerialization.SerializationHelper;
using static QSP.Utilities.ExceptionHelpers;

namespace QSP.UI.ToLdgModule.TOPerf
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
        private string toWt = "TakeOffWeight";
        private string flaps = "Flaps";
        private string thrustRating = "ThrustRating";
        private string antiIce = "AntiIce";
        private string packs = "Packs";

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

            foreach (var action in actions) IgnoreException(action);
        }
    }
}
