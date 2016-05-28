using QSP.Common;
using System;
using System.IO;
using System.Windows.Forms;
using static QSP.UI.FormInstanceGetter;

namespace QSP.UI
{
    public class FormStateSaver
    {
        private TextBox[] txtBoxes;
        private ComboBox[] comboBoxes;

        private string filePath;
        private string statesDirectory = QspCore.QspAppDataDirectory + "\\SavedStates";

        public enum PageOfForm
        {
            FuelCalculation,
            Takeoff,
            Landing
        }

        public FormStateSaver(PageOfForm item)
        {
            MainForm frm = MainFormInstance();

            switch (item)
            {
                case PageOfForm.FuelCalculation:

                    txtBoxes = new TextBox[] {
                        frm.ZFW,
                        frm.OrigTxtBox,
                        frm.DestTxtBox,
                        frm.AltnTxtBox,
                        frm.MissedAppFuel,
                        frm.HoldTime_TxtBox,
                        frm.ExtraFuel,
                        frm.APUTime,
                        frm.TaxiTime,
                        frm.FinalRsv
                    };

                    comboBoxes = new ComboBox[]{
                        frm.WtUnitSel_ComboBox,
                        frm.ACList,
                        frm.ContPercentToDest
                    };

                    filePath = statesDirectory + "\\LastRunMain.txt";
                    break;
            }

        }

        public void Save()
        {
            try
            {
                saveState();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        public void Load()
        {
            try
            {
                loadLastState();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void loadLastState()
        {
            string Alltext = null;

            if (!File.Exists(filePath))
            {
                return;
            }
            else
            {
                Alltext = File.ReadAllText(filePath);
            }

            string[] load = Alltext.Split(';');

            int x = txtBoxes.Length;

            for (int i = 0; i < x; i++)
            {
                txtBoxes[i].Text = load[i];
            }

            for (int j = 0; j < comboBoxes.Length; j++)
            {
                comboBoxes[j].Text = load[j + x];
            }

        }
        
        private void saveState()
        {
            string SaveText = "";

            foreach (var i in txtBoxes)
            {
                SaveText += i.Text + ";";
            }

            foreach (var j in comboBoxes)
            {
                SaveText += j.Text + ";";
            }

            Directory.CreateDirectory(statesDirectory);

            if (!File.Exists(filePath))
            {
                using (StreamWriter writer = File.CreateText(filePath))
                {
                    writer.Write(SaveText);
                }
            }

            using (StreamWriter writer = new StreamWriter(filePath))
            {
                writer.Write(SaveText);
            }

        }
    }
}
