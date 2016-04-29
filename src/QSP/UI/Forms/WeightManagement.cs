using System;
using System.Drawing;
using static QSP.UI.FormInstanceGetter;
using static QSP.Utilities.LoggerInstance;

namespace QSP
{

    public partial class WtManagement
    {

        const double KG_LB = 2.20462;
        bool Sync_payload_InProgress = false;

        private void WtManagement_Load(object sender, EventArgs e)
        {
            //sync with main form
            SyncWithMain(0);
        }


        private void UpdateWtLabels()
        {
            Label4.Text = WtUnit_ComboBox.Text;
            Label5.Text = WtUnit_ComboBox.Text;
            Label6.Text = WtUnit_ComboBox.Text;
            Label10.Text = WtUnit_ComboBox.Text;
            Label11.Text = WtUnit_ComboBox.Text;
            Label12.Text = WtUnit_ComboBox.Text;

        }

        private void SyncWithMain(int p)
        {
            //p=0 : copy data FROM main to this form
            //p=1 : copy data TO main

            var frm = MainFormInstance();

            try
            {
                switch (p)
                {
                    case 0:

                        int oew = frm.OperatingEmptyWtKg;
                        int maxZfw = frm.MaxZfwKg;

                        if (frm.WtUnitSel_ComboBox.Text == "KG")
                        {
                            WtUnit_ComboBox.SelectedIndex = 0;
                            OEW_Label.Text = oew.ToString();
                            MaxZFW_Label.Text = maxZfw.ToString();
                            MaxPayload_Label.Text = (maxZfw - oew).ToString();

                        }
                        else
                        {
                            WtUnit_ComboBox.SelectedIndex = 1;
                            OEW_Label.Text = Math.Round(frm.OperatingEmptyWtKg * KG_LB).ToString();
                            MaxZFW_Label.Text = Math.Round(frm.MaxZfwKg * KG_LB).ToString();
                            MaxPayload_Label.Text = Math.Round((maxZfw - oew) * KG_LB).ToString();

                        }

                        OEW_TextBox.Text = OEW_Label.Text;
                        // Payload_TextBox.Text = main.Payload.Text
                        ZFW_TextBox.Text = frm.ZFW.Text;

                        UpdateWtLabels();
                        SetColor();

                        break;

                    case 1:

                        if (WtUnit_ComboBox.SelectedIndex == 0)
                        {
                            //KG
                            frm.WtUnitSel_ComboBox.Text = "KG";
                            //LB
                        }
                        else
                        {
                            frm.WtUnitSel_ComboBox.Text = "LB";
                        }

                        // main.Payload.Text = Payload_TextBox.Text
                        frm.ZFW.Text = ZFW_TextBox.Text;

                        break;
                }


            }
            catch (Exception ex)
            {
                WriteToLog(ex);
            }            
        }

        private void Sync_Payload_TrackBar_ZFW(int p)
        {
            //p=0 : Reference =  textbox
            //p=1 : Reference =  trackbar
            //p=2 : Reference =  ZFW

            if (Sync_payload_InProgress == true)
            {
                return;
            }

            Sync_payload_InProgress = true;

            try
            {
                switch (p)
                {
                    case 0:
                        PayloadTrackBar.Value = Convert.ToInt32(Math.Round(Convert.ToDouble(Payload_TextBox.Text) / Convert.ToDouble(MaxPayload_Label.Text) * 100));

                        ZFW_TextBox.Text = Convert.ToString(Math.Round(Convert.ToDouble(Payload_TextBox.Text) + Convert.ToDouble(OEW_TextBox.Text)));

                        break;
                    case 1:
                        Payload_TextBox.Text = Convert.ToString(Math.Round(Convert.ToDouble(MaxPayload_Label.Text) * PayloadTrackBar.Value / 100));

                        ZFW_TextBox.Text = Convert.ToString(Math.Round(Convert.ToDouble(Payload_TextBox.Text) + Convert.ToDouble(OEW_TextBox.Text)));

                        break;
                    case 2:
                        Payload_TextBox.Text = Convert.ToString(Math.Round(Convert.ToDouble(ZFW_TextBox.Text) - Convert.ToDouble(OEW_TextBox.Text)));
                        PayloadTrackBar.Value = Convert.ToInt32(Math.Round(Convert.ToDouble(Payload_TextBox.Text) / Convert.ToDouble(MaxPayload_Label.Text) * 100));

                        break;
                }

                SetColor();


            }
            catch 
            {
                Payload_TextBox.ForeColor = Color.Red;
                ZFW_TextBox.ForeColor = Color.Red;
            }

            Sync_payload_InProgress = false;
        }


        private void SetColor()
        {
            if (Convert.ToDouble(Payload_TextBox.Text) > Convert.ToDouble(MaxPayload_Label.Text))
            {
                Payload_TextBox.ForeColor = Color.Red;
            }
            else
            {
                Payload_TextBox.ForeColor = Color.Green;
            }

            if (Convert.ToDouble(ZFW_TextBox.Text) > Convert.ToDouble(MaxZFW_Label.Text))
            {
                ZFW_TextBox.ForeColor = Color.Red;
            }
            else
            {
                ZFW_TextBox.ForeColor = Color.Green;
            }

        }

        private void Payload_TrackBar_Scroll(object sender, EventArgs e)
        {
            Sync_Payload_TrackBar_ZFW(1);
        }

        private void OK_Button_Click(object sender, EventArgs e)
        {
            SyncWithMain(1);
            this.Close();
        }

        private void Cancel_Button_Click(object sender, EventArgs e)
        {
            this.Close();
        }


        private void WtUnit_ComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateWtLabels();

            Sync_payload_InProgress = true;


            try
            {

                if (WtUnit_ComboBox.SelectedIndex == 0)
                {
                    OEW_Label.Text = Convert.ToString(Math.Round(Convert.ToDouble(OEW_Label.Text) / KG_LB));
                    OEW_TextBox.Text = Convert.ToString(Math.Round(Convert.ToDouble(OEW_TextBox.Text) / KG_LB));
                    MaxPayload_Label.Text = Convert.ToString(Math.Round(Convert.ToDouble(MaxPayload_Label.Text) / KG_LB));
                    Payload_TextBox.Text = Convert.ToString(Math.Round(Convert.ToDouble(Payload_TextBox.Text) / KG_LB));
                    MaxZFW_Label.Text = Convert.ToString(Math.Round(Convert.ToDouble(MaxZFW_Label.Text) / KG_LB));
                    ZFW_TextBox.Text = Convert.ToString(Math.Round(Convert.ToDouble(ZFW_TextBox.Text) / KG_LB));


                }
                else
                {
                    OEW_Label.Text = Convert.ToString(Math.Round(Convert.ToDouble(OEW_Label.Text) * KG_LB));
                    OEW_TextBox.Text = Convert.ToString(Math.Round(Convert.ToDouble(OEW_TextBox.Text) * KG_LB));
                    MaxPayload_Label.Text = Convert.ToString(Math.Round(Convert.ToDouble(MaxPayload_Label.Text) * KG_LB));
                    Payload_TextBox.Text = Convert.ToString(Math.Round(Convert.ToDouble(Payload_TextBox.Text) * KG_LB));
                    MaxZFW_Label.Text = Convert.ToString(Math.Round(Convert.ToDouble(MaxZFW_Label.Text) * KG_LB));
                    ZFW_TextBox.Text = Convert.ToString(Math.Round(Convert.ToDouble(ZFW_TextBox.Text) * KG_LB));

                }


            }
            catch 
            {
            }

            Sync_payload_InProgress = false;

        }

        private void Payload_TextBox_TextChanged(object sender, EventArgs e)
        {
            Sync_Payload_TrackBar_ZFW(0);
        }

        private void ZFW_TextBox_TextChanged(object sender, EventArgs e)
        {
            Sync_Payload_TrackBar_ZFW(2);
        }
        public WtManagement()
        {
            Load += WtManagement_Load;
            InitializeComponent();
        }
    }
}
