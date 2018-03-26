using QSP.Attributes;
using QSP.UI.Controllers.Units;
using QSP.UI.Views.FuelPlan;
using QSP.Utilities.Units;
using System;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using static QSP.LibraryExtension.XmlSerialization.SerializationHelper;
using static QSP.Utilities.ExceptionHelpers;

namespace QSP.UI.UserControls
{
    public class FuelPageState
    {
        public static readonly string FileLocation = @"SavedStates\FuelPlanningControl.xml";

        // Strings for xml tags.
        private static readonly string aircraft = "Aircraft";
        private static readonly string registration = "Registration";
        private static readonly string wtUnit = "WtUnit";
        private static readonly string zfw = "Zfw";
        private static readonly string origin = "Origin";
        private static readonly string originRwy = "OriginRwy";
        private static readonly string originSid = "OriginSid";
        private static readonly string destination = "Destination";
        private static readonly string destRwy = "DestinRwy";
        private static readonly string destStar = "DestStar";
        private static readonly string alternates = "Alternates";
        private static readonly string altnEntry = "AltnEntry";
        private static readonly string altnIcao = "AltnIcao";
        private static readonly string altnRwy = "AltnRwy";
        private static readonly string contPerc = "ContPerc";
        private static readonly string holdMin = "HoldMin";
        private static readonly string extraKg = "ExtraKg";
        private static readonly string apuMin = "ApuMin";
        private static readonly string taxiMin = "TaxiMin";
        private static readonly string finalRsvMin = "FinalRsvMin";
        private static readonly string altnEnabled = "AltnEnabled";

        private FuelPlanningControl control;

        public FuelPageState(FuelPlanningControl control)
        {
            this.control = control;
        }

        public void SaveToFile()
        {
            var dir = Path.GetDirectoryName(FileLocation);
            Directory.CreateDirectory(dir);
            File.WriteAllText(FileLocation, Save().ToString());
        }

        [NoThrow]
        public XElement Save()
        {
            var c = control;
            var d = c.RouteFinderView.DestRow.OptionView;
            var o = c.RouteFinderView.OrigRow.OptionView;

            return new XElement("FuelPlanningState", new XElement[]
            {
                c.acListComboBox.Text.Serialize(aircraft),
                c.registrationComboBox.Text.Serialize(registration),
                ((int)c.WeightUnit).Serialize(wtUnit),
                TryGetWeightKg(c.Zfw).Serialize(zfw),
                o.Icao.Serialize(origin),
                o.SelectedRwy.Serialize(originRwy),
                o.SelectedProcedureText.Serialize(originSid),
                d.Icao.Serialize(destination),
                d.SelectedRwy.Serialize(destRwy),
                d.SelectedProcedureText.Serialize(destStar),
                new XElement(alternates, GetAlternates()),
                c.ContPercentComboBox.Text.Serialize(contPerc),
                c.HoldTimeTxtBox.Text.Serialize(holdMin),
                TryGetWeightKg(c.Extra).Serialize(extraKg),
                c.ApuTimeTxtBox.Text.Serialize(apuMin),
                c.TaxiTimeTxtBox.Text.Serialize(taxiMin),
                c.FinalReserveTxtBox.Text.Serialize(finalRsvMin),
                c.AltnEnabledCheckBox.Checked.Serialize(altnEnabled)
            });
        }

        private XElement[] GetAlternates()
        {
            var altnInfo = control.AltnPresenter.GetAlternates()
                .Select(a => new XElement(altnEntry,
                    a.icao.Serialize(altnIcao),
                    a.rwy.Serialize(altnRwy)));

            return altnInfo.ToArray();
        }

        private void SetAlternates(XElement node)
        {
            var icaoRwys = node.Elements(altnEntry)
                .Select(e => (e.GetString(altnIcao), e.GetString(altnRwy)));

            control.AltnPresenter.SetAlternates(icaoRwys.ToList());
        }

        private static double TryGetWeightKg(WeightTextBoxController c)
        {
            try
            {
                return c.GetWeightKg();
            }
            catch
            {
                return 0.0;
            }
        }

        public void LoadFromFile()
        {
            Load(XDocument.Load(FileLocation));
        }

        [NoThrow]
        public void Load(XDocument doc)
        {
            var c = control;
            var r = doc.Root;
            var d = c.RouteFinderView.DestRow.OptionView;
            var o = c.RouteFinderView.OrigRow.OptionView;

            Action[] actions =
            {
                () => c.acListComboBox.Text = r.GetString(aircraft),
                () => c.registrationComboBox.Text = r.GetString(registration),
                () => c.WeightUnit = (WeightUnit)r.GetInt(wtUnit),
                () => c.WeightControl.ZfwKg = r.GetDouble(zfw),
                () => o.Icao = r.GetString(origin),
                () => o.SelectedRwy = r.GetString(originRwy),
                () => o.SelectedProcedureText = r.GetString(originSid),
                () => d.Icao = r.GetString(destination),
                () => d.SelectedRwy = r.GetString(destRwy),
                () => d.SelectedProcedureText = r.GetString(destStar),
                () => SetAlternates(r.Element(alternates)),
                () => c.ContPercentComboBox.Text = r.GetString(contPerc),
                () => c.HoldTimeTxtBox.Text = r.GetString(holdMin),
                () => c.Extra.SetWeight(r.GetDouble(extraKg)),
                () => c.ApuTimeTxtBox.Text = r.GetString(apuMin),
                () => c.TaxiTimeTxtBox.Text = r.GetString(taxiMin),
                () => c.FinalReserveTxtBox.Text = r.GetString(finalRsvMin),
                () => c.AltnEnabledCheckBox.Checked=r.GetBool(altnEnabled)
            };

            foreach (var a in actions) IgnoreException(a);
        }
    }
}
