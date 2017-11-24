using CommonLibrary.Attributes;
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
        private string aircraft = "Aircraft";
        private string registration = "Registration";
        private string wtUnit = "WtUnit";
        private string zfw = "Zfw";
        private string origin = "Origin";
        private string originRwy = "OriginRwy";
        private string originSid = "OriginSid";
        private string destination = "Destination";
        private string destRwy = "DestinRwy";
        private string destStar = "DestStar";
        private string alternates = "Alternates";
        private string altnEntry = "AltnEntry";
        private string altnIcao = "AltnIcao";
        private string altnRwy = "AltnRwy";
        private string contPerc = "ContPerc";
        private string holdMin = "HoldMin";
        private string extraKg = "ExtraKg";
        private string apuMin = "ApuMin";
        private string taxiMin = "TaxiMin";
        private string finalRsvMin = "FinalRsvMin";

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
            var d = c.DestPresenter;
            var o = c.OrigPresenter;

            return new XElement("FuelPlanningState", new XElement[]
            {
                c.acListComboBox.Text.Serialize(aircraft),
                c.registrationComboBox.Text.Serialize(registration),
                ((int)c.WeightUnit).Serialize(wtUnit),
                TryGetWeightKg(c.Zfw).Serialize(zfw),
                o.Icao.Serialize(origin),
                o.Rwy.Serialize(originRwy),
                o.SelectedProcedureText.Serialize(originSid),
                d.Icao.Serialize(destination),
                d.Rwy.Serialize(destRwy),
                d.SelectedProcedureText.Serialize(destStar),
                new XElement(alternates, GetAlternates()),
                c.ContPercentComboBox.Text.Serialize(contPerc),
                c.HoldTimeTxtBox.Text.Serialize(holdMin),
                TryGetWeightKg(c.Extra).Serialize(extraKg),
                c.ApuTimeTxtBox.Text.Serialize(apuMin),
                c.TaxiTimeTxtBox.Text.Serialize(taxiMin),
                c.FinalReserveTxtBox.Text.Serialize(finalRsvMin)
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
            var d = c.DestPresenter;
            var o = c.OrigPresenter;

            Action[] actions =
            {
                () => c.acListComboBox.Text = r.GetString(aircraft),
                () => c.registrationComboBox.Text = r.GetString(registration),
                () => c.WeightUnit = (WeightUnit)r.GetInt(wtUnit),
                () => c.WeightControl.ZfwKg = r.GetDouble(zfw),
                () => o.Icao = r.GetString(origin),
                () => o.Rwy = r.GetString(originRwy),
                () => o.SelectedProcedureText = r.GetString(originSid),
                () => d.Icao = r.GetString(destination),
                () => d.Rwy = r.GetString(destRwy),
                () => d.SelectedProcedureText = r.GetString(destStar),
                () => SetAlternates(r.Element(alternates)),
                () => c.ContPercentComboBox.Text = r.GetString(contPerc),
                () => c.HoldTimeTxtBox.Text = r.GetString(holdMin),
                () => c.Extra.SetWeight(r.GetDouble(extraKg)),
                () => c.ApuTimeTxtBox.Text = r.GetString(apuMin),
                () => c.TaxiTimeTxtBox.Text = r.GetString(taxiMin),
                () => c.FinalReserveTxtBox.Text = r.GetString(finalRsvMin)
            };

            foreach (var a in actions) IgnoreException(a);
        }
    }
}
