using QSP.UI.Controllers.Units;
using QSP.Utilities.Units;
using System.Linq;
using System.Xml.Linq;
using System.IO;

namespace QSP.UI.UserControls
{
    public class FuelPageState
    {
        public static readonly string FileLocation = 
            @"SavedStates\FuelPlanningControl.xml";

        // Strings for xml tags.
        private string aircraft = "Aircraft";
        private string registration = "Registration";
        private string wtUnit = "WtUnit";
        private string zfw = "Zfw";
        private string origin = "Origin";
        private string originRwy = "OriginRwy";
        private string originSid = "OriginSid";
        private string destination = "Destinatiob";
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
            File.WriteAllText(FileLocation, Save().ToString());
        }

        public XElement Save()
        {
            var c = control;

            return new XElement("FuelPlanningState", new XElement[]
            {
                new XElement(aircraft, c.acListComboBox.Text),
                new XElement(registration, c.registrationComboBox.Text),
                new XElement(wtUnit, (int)c.WeightUnit),
                new XElement(zfw, TryGetWeightKg(c.Zfw)),
                new XElement(origin, c.origTxtBox.Text),
                new XElement(originRwy, c.origRwyComboBox.Text),
                new XElement(originSid, c.sidComboBox.Text),
                new XElement(destination, c.destTxtBox.Text),
                new XElement(destRwy, c.destRwyComboBox.Text),
                new XElement(destStar, c.starComboBox.Text),
                new XElement(alternates, GetAlternates()),
                new XElement(missedAppKg, TryGetWeightKg(c.MissedApproach)),
                new XElement(contPerc, c.ContPercentComboBox.Text),
                new XElement(holdMin, c.HoldTimeTxtBox.Text),
                new XElement(extraKg, TryGetWeightKg(c.Extra)),
                new XElement(apuMin, c.ApuTimeTxtBox.Text),
                new XElement(taxiMin, c.TaxiTimeTxtBox.Text),
                new XElement(finalRsvMin, c.FinalReserveTxtBox.Text)
            });
        }

        private XElement[] GetAlternates()
        {
            var altnInfo = control.altnControl.Controls
                .Select(c =>
                new XElement(altnEntry,
                    new XElement(altnIcao, c.IcaoTxtBox.Text),
                    new XElement(altnRwy, c.RwyComboBox.Text)));

            return altnInfo.ToArray();
        }

        private void SetAlternates(XElement node)
        {
            var altns = node.Elements(altnEntry).ToList();

            if (altns.Count == 0) return;

            // Set number of alternates
            while (control.altnControl.RowCount < altns.Count)
            {
                control.altnControl.AddRow();
            }

            var altnControls = control.altnControl.Controls.ToList();

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

        public void Load(XDocument doc)
        {
            var c = control;
            var root = doc.Root;

            c.acListComboBox.Text = GetValue(root, aircraft);
            c.registrationComboBox.Text = GetValue(root, registration);
            c.WeightUnit = (WeightUnit)int.Parse(GetValue(root, wtUnit));
            c.Zfw.SetWeight(GetDouble(root, zfw));
            c.origTxtBox.Text = GetValue(root, origin);
            c.origRwyComboBox.Text = GetValue(root, originRwy);
            c.sidComboBox.Text = GetValue(root, originSid);
            c.destTxtBox.Text = GetValue(root, destination);
            c.destRwyComboBox.Text = GetValue(root, destRwy);
            c.starComboBox.Text = GetValue(root, destStar);
            SetAlternates(root.Element(alternates));
            c.MissedApproach.SetWeight(GetDouble(root, missedAppKg));
            c.ContPercentComboBox.Text = GetValue(root, contPerc);
            c.HoldTimeTxtBox.Text = GetValue(root, holdMin);
            c.Extra.SetWeight(GetDouble(root, extraKg));
            c.ApuTimeTxtBox.Text = GetValue(root, apuMin);
            c.TaxiTimeTxtBox.Text = GetValue(root, taxiMin);
            c.FinalReserveTxtBox.Text = GetValue(root, finalRsvMin);            
        }

        private static double GetDouble(XElement root, string key)
        {
            return double.Parse(GetValue(root, key));
        }

        private static string GetValue(XElement root, string key)
        {
            return root.Element(key).Value;
        }
    }
}
