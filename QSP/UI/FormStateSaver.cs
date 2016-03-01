using System;
using System.IO;
using System.Windows.Forms;
using static QSP.UI.Utilities;
using QSP.Core;

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

                case PageOfForm.Takeoff:

                    txtBoxes = new TextBox[] {
                        frm.AD,
                        frm.elevation,
                        frm.RwyHeading,
                        frm.windspd,
                        frm.winddir,
                        frm.OAT,
                        frm.altimeter,
                        frm.Weight
                    };

                    comboBoxes = new ComboBox[] {
                        frm.RWY,
                        frm.m_ft,
                        frm.temp_c_f,
                        frm.hpa_inHg,
                        frm.surf_cond,
                        frm.ACListTOComboBox,
                        frm.WTunit,
                        frm.Flaps,
                        frm.AISel,
                        frm.PacksSel,
                        frm.ThrustRating_Box,
                        frm.Flaps,
                        frm.AISel,
                        frm.PacksSel
                    };

                    filePath = statesDirectory + "\\LastRunTO.txt";
                    break;

                //case PageOfForm.Landing:

                //    txtBoxes = new TextBox[] {
                //        frm.AD_LDG,
                //        frm.elevation_LDG,
                //        frm.RwyHeading_LDG,
                //        frm.winddir_LDG,
                //        frm.windspd_LDG,
                //        frm.oat_LDG,
                //        frm.altimeter_LDG,
                //        frm.Weight_LDG,
                //        frm.AppSpdInc_LDG,
                //        frm.Weight_LDG
                //    };

                //    comboBoxes = new ComboBox[]{
                //        frm.RWY_LDG,
                //        frm.m_ft_LDG,
                //        frm.temp_c_f_LDG,
                //        frm.WtUnit_LDG,
                //        frm.Flaps_LDG,
                //        frm.REVSel_LDG,
                //        frm.BrakeSel_LDG,
                //        frm.ACListLDG,
                //        frm.Flaps_LDG,
                //        frm.SurfCond_LDG,
                //        frm.hpa_inhg_LDG
                //    };

                //    filePath = statesDirectory + "\\LastRunLdg.txt";
                //    break;
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

