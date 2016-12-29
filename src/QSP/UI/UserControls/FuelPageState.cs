using QSP.UI.Controllers.Units;
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
        private string missedAppKg = "MissedAppKg";
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

        // Does not throw exception.
        public XElement Save()
        {
            var c = control;

            return new XElement("FuelPlanningState", new XElement[]
            {
                c.acListComboBox.Text.Serialize(aircraft),
                c.registrationComboBox.Text.Serialize(registration),
                ((int)c.WeightUnit).Serialize(wtUnit),
                TryGetWeightKg(c.Zfw).Serialize(zfw),
                c.origTxtBox.Text.Serialize(origin),
                c.origRwyComboBox.Text.Serialize(originRwy),
                c.sidComboBox.Text.Serialize(originSid),
                c.destTxtBox.Text.Serialize(destination),
                c.destRwyComboBox.Text.Serialize(destRwy),
                c.starComboBox.Text.Serialize(destStar),
                new XElement(alternates, GetAlternates()),
                TryGetWeightKg(c.MissedApproach).Serialize(missedAppKg),
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
            var altnInfo = control.AltnControl.Controls
                .Select(c => new XElement(altnEntry,
                    c.IcaoTxtBox.Text.Serialize(altnIcao),
                    c.RwyComboBox.Text.Serialize(altnRwy)));

            return altnInfo.ToArray();
        }

        private void SetAlternates(XElement node)
        {
            var altns = node.Elements(altnEntry).ToList();
            if (altns.Count == 0) return;

            // Set number of alternates
            while (control.AltnControl.RowCount < altns.Count)
            {
                control.AltnControl.AddRow();
            }

            var altnControls = control.AltnControl.Controls.ToList();

            for (int i = 0; i < altns.Count; i++)
            {
                var c = altnControls[i];
                c.IcaoTxtBox.Text = altns[i].Element(altnIcao).Value;
                c.RwyComboBox.Text = altns[i].Element(altnRwy).Value;
            }
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

        // Does not throw exception.
        public void Load(XDocument doc)
        {
            var c = control;
            var r = doc.Root;

            Action[] actions =
            {
                () => c.acListComboBox.Text = r.GetString(aircraft),
                () => c.registrationComboBox.Text = r.GetString(registration),
                () => c.WeightUnit = (WeightUnit)r.GetInt(wtUnit),
                () => c.WeightControl.ZfwKg =r.GetDouble(zfw),
                () => c.origTxtBox.Text = r.GetString(origin),
                () => c.origRwyComboBox.Text = r.GetString(originRwy),
                () => c.sidComboBox.Text = r.GetString(originSid),
                () => c.destTxtBox.Text = r.GetString(destination),
                () => c.destRwyComboBox.Text = r.GetString(destRwy),
                () => c.starComboBox.Text = r.GetString(destStar),
                () => SetAlternates(r.Element(alternates)),
                () => c.MissedApproach.SetWeight(r.GetDouble(missedAppKg)),
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
