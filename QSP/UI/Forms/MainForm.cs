using QSP.AviationTools;
using QSP.Core;
using QSP.LibraryExtension;
using QSP.RouteFinding;
using QSP.RouteFinding.Airports;
using QSP.RouteFinding.Data;
using QSP.RouteFinding.Routes;
using QSP.RouteFinding.TerminalProcedures.Sid;
using QSP.RouteFinding.TerminalProcedures.Star;
using QSP.TakeOffPerfCalculation;
using QSP.UI;
using QSP.Utilities;
using QSP.WindAloft;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using static QSP.AviationTools.Constants;
using static QSP.RouteFinding.RouteFindingCore;
using static QSP.Utilities.ErrorLogger;

namespace QSP
{
    public partial class MainForm
    {
        public int OperatingEmptyWtKg;
        //OperatingEmptyWt = Basic Operating Wt
        public int MissedAppFuelKG;

        public int MaxZfwKg;

        public bool AppExitOverride = false;
        private FormStateSaver formStateManagerFuel;
        private FormStateSaver formStateManagerTO;
        private FormStateSaver formStateManagerLDG;

        private ViewManager viewChanger;

        #region "FuelCalculation"

        public static FuelCalculator ComputeFuelIteration(FuelCalculationParameters para, uint precisionLevel)
        {
            //presisionLevel = 0, 1, 2, ... 
            //smaller num = less precise
            //0 = disregard wind completely, 1 is good enough

            FuelCalculator FuelCalc = new FuelCalculator(para);
            OptCrzCalculator OptCrzCalc = new OptCrzCalculator(para.AC);

            //calculate altn first
            double fuelTon = 0;
            double avgWeightTon = 0;
            double crzAltFt = 0;
            int tailwind = 0;
            double tas = 0;

            for (uint i = 0; i <= precisionLevel; i++)
            {
                fuelTon = FuelCalc.GetAltnFuelTon();
                avgWeightTon = FuelCalc.LandWeightTonAltn + fuelTon / 2;
                crzAltFt = OptCrzCalc.ActualCrzAlt(avgWeightTon, para.DisToAltn);
                tas = OptCrzCalc.CruiseTas(crzAltFt);
                tailwind = computeTailWind(TailWindCalcOptions.DestToAltn, Convert.ToInt32(tas), Convert.ToInt32(crzAltFt / 100));
                para.AvgWindToAltn = tailwind;

                Debug.WriteLine("TO ALTN, CRZ ALT {0} FT, TAS {1} KTS, TAILWIND {2} KTS", crzAltFt, tas, tailwind);
            }


            for (uint i = 0; i <= precisionLevel; i++)
            {
                fuelTon = FuelCalc.GetDestFuelTon();
                avgWeightTon = FuelCalc.LandWeightTonDest + fuelTon / 2;
                crzAltFt = OptCrzCalc.ActualCrzAlt(avgWeightTon, para.DisToDest);
                tas = OptCrzCalc.CruiseTas(crzAltFt);
                tailwind = computeTailWind(TailWindCalcOptions.OrigToDest, Convert.ToInt32(tas), Convert.ToInt32(crzAltFt / 100));
                para.AvgWindToDest = tailwind;

                Debug.WriteLine("TO DEST, CRZ ALT {0} FT, TAS {1} KTS, TAILWIND {2} KTS", crzAltFt, tas, tailwind);

            }
            return FuelCalc;
        }


        private void Calculate(object sender, EventArgs e)
        {
            FuelReport_TxtBox.ForeColor = Color.Black;
            FuelReport_TxtBox.Text = "";

            FuelCalculationParameters Parameters = new FuelCalculationParameters();
            Parameters.FillInDefaultValueIfLeftBlank();

            try
            {
                Parameters.ImportValues();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
                return;
            }

            FuelCalculator FuelCalc = null;
            try
            {
                FuelCalc = ComputeFuelIteration(Parameters, 1);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return;
            }

            double FuelToAltnTon = FuelCalc.GetAltnFuelTon();
            double FuelToDestTon = FuelCalc.GetDestFuelTon();

            FuelReportResult fuelCalcResult = new FuelReportResult(FuelToDestTon, FuelToAltnTon, Parameters, FuelCalc);


            if (fuelCalcResult.TotalFuelKG > FuelCalc.max_fuel_kg)
            {
                MessageBox.Show(insufficientFuelMsg(fuelCalcResult.TotalFuelKG, FuelCalc.max_fuel_kg, Parameters.WtUnit()));
                return;

            }

            string OutputText = fuelCalcResult.ToString(Parameters.WtUnit());

            FuelReport_TxtBox.Text = Environment.NewLine + Strings.ShiftStringToRight(OutputText, 20);
            formStateManagerFuel.Save();

            //send weights to takeoff/ldg calc form 
            AC_Req = ACList.Text;
            TOWT_Req_Unit = Parameters.WtUnit();
            LDG_fuel_prediction_unit = Parameters.WtUnit();

            TOWT_Req = Convert.ToInt32(Parameters.Zfw + fuelCalcResult.TakeoffFuelKg * (Parameters.WtUnit() == WeightUnit.KG ? 1.0 : KG_LB));
            LDG_ZFW = Convert.ToInt32(Parameters.Zfw);
            LDG_fuel_prediction = Convert.ToInt32(fuelCalcResult.LdgFuelKgPredict * (Parameters.WtUnit() == WeightUnit.KG ? 1.0 : KG_LB));

            viewChanger.ShowPage(ViewManager.Pages.FuelReport);
        }

        public enum TailWindCalcOptions
        {
            OrigToDest,
            DestToAltn
        }

        private static string insufficientFuelMsg(double fuelReqKG, double fuelCapacityKG, WeightUnit unit)
        {
            if (unit == WeightUnit.KG)
            {
                return "Insufficient fuel" + Environment.NewLine + "Fuel required for this flight is " + fuelReqKG + " KG. Maximum fuel tank capacity is " + fuelCapacityKG + " KG.";
            }
            else
            {
                return "Insufficient fuel" + Environment.NewLine + "Fuel required for this flight is " + Math.Round(fuelReqKG * KG_LB) + " LB. Maximum fuel tank capacity is " + Math.Round(fuelCapacityKG * KG_LB) + " LB.";
            }
        }

        #endregion

        private void LoadDefaultState()
        {
            ACList.SelectedIndex = 0;
            WtUnitSel_ComboBox.SelectedIndex = 0;

            FinalRsv.Text = "30";
            ContPercentToDest.Text = "5";
            ExtraFuel.Text = "0";
            APUTime.Text = "30";
            TaxiTime.Text = "20";
            HoldTime_TxtBox.Text = "0";

            RouteDisLbl.Text = "";
            RouteDisAltnLbl.Text = "";
            Label56.Text = "0";

            RadioButton1.Checked = true;
        }

        private bool IsRunAsAdministrator()
        {
            WindowsIdentity wi = WindowsIdentity.GetCurrent();
            WindowsPrincipal wp = new WindowsPrincipal(wi);

            return wp.IsInRole(WindowsBuiltInRole.Administrator);
        }

        private async void Startup(object sender, EventArgs e)
        {
            var splash = new Splash();
            splash.Show();
            splash.Refresh();

            checkRegistry();
            LoadDefaultState();

            if (WtUnitSel_ComboBox.Text == "KG")
            {
                ZFW.Text = Convert.ToString(OperatingEmptyWtKg);
            }
            else
            {
                ZFW.Text = Convert.ToString(Math.Round(OperatingEmptyWtKg * KG_LB));
            }

            LoadNavDBUpdateStatusStrip(true);
            TakeOffPerfCalculation.LoadedData.Load();
            LandingPerfCalculation.LoadedData.Load();

            //load previous form states
            formStateManagerFuel = new FormStateSaver(FormStateSaver.PageOfForm.FuelCalculation);
            formStateManagerTO = new FormStateSaver(FormStateSaver.PageOfForm.Takeoff);
            formStateManagerLDG = new FormStateSaver(FormStateSaver.PageOfForm.Landing);

            formStateManagerFuel.Load();

            this.Size = new Size(1280, 900);
            splash.Close();

            viewChanger = new ViewManager();
            viewChanger.ShowPage(ViewManager.Pages.FuelCalculation);

            startTracksDlAsReq();
            await startWindDlAsReq();
        }

        private static void checkRegistry()
        {
            //try to check/add registry so that google map works properly 
            IeEmulationChecker RegChecker = new IeEmulationChecker();

            try
            {
                RegChecker.Run();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

#if DEBUG
            try
            {
                RegChecker.DebugRun();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
#endif

        }

        private void startTracksDlAsReq()
        {
            if (QspCore.AppSettings.AutoDLTracks)
            {
                //TODO: add code to start download tracks automatically.
            }
            else
            {
                LblTrackDownloadStatus.Image = Properties.Resources.YellowLight;
                LblTrackDownloadStatus.Text = "Tracks: Not downloaded";
            }
        }

        private async Task startWindDlAsReq()
        {
            if (QspCore.AppSettings.AutoDLWind)
            {
                await downloadWind();
            }
            else
            {
                ShowWindDownloadStatus(WindDownloadStatus.WaitingManualDL);
            }
        }

        private async Task downloadWind()
        {
            ShowWindDownloadStatus(WindDownloadStatus.Downloading);

            try
            {
                await new WindManager().DownloadWindAsync();
            }
            catch (Exception ex)
            {
                WriteToLog(ex);
                ShowWindDownloadStatus(WindDownloadStatus.Failed);
            }
            ShowWindDownloadStatus(WindDownloadStatus.Finished);
        }

        public enum WindDownloadStatus
        {
            Downloading,
            Finished,
            Failed,
            WaitingManualDL
        }

        public void ShowWindDownloadStatus(WindDownloadStatus item)
        {
            switch (item)
            {
                case WindDownloadStatus.Downloading:
                    WindDownloadStatus_Lbl.Text = "Downloading lastest wind ...";
                    WindDownloadStatus_Lbl.Image = null;
                    break;

                case WindDownloadStatus.Finished:
                    WindDownloadStatus_Lbl.Text = "Lastest wind ready";
                    WindDownloadStatus_Lbl.Image = Properties.Resources.GreenLight;
                    break;

                case WindDownloadStatus.Failed:
                    WindDownloadStatus_Lbl.Text = "Failed to download wind data";
                    WindDownloadStatus_Lbl.Image = Properties.Resources.RedLight;
                    break;

                case WindDownloadStatus.WaitingManualDL:
                    WindDownloadStatus_Lbl.Text = "Click here to download wind data";
                    WindDownloadStatus_Lbl.Image = Properties.Resources.YellowLight;
                    break;
            }
        }

        public void LoadNavDBUpdateStatusStrip(bool startingApp)
        {
            try
            {
                //loading the navigation database
                QspCore.AppSettings = new AppOptions(XDocument.Load(QspCore.QspAppDataDirectory + "\\SavedStates\\options.xml"));

                NavDataLoader.LoadAllDB(QspCore.AppSettings.NavDBLocation);
                //if success, update the status strip

                Tuple<string, string> t = OptionsForm.AiracCyclePeriod(QspCore.AppSettings.NavDBLocation);
                //this returns, for example, (1407,26JUN23JUL/14)

                bool expired = !AiracTools.AiracValid(t.Item2);
                if (expired)
                {
                    StatusLabel1.Image = Properties.Resources.YellowLight;
                    StatusLabel1.Text = "AIRAC: " + t.Item1 + " (" + t.Item2 + ") - Expired";
                }
                else
                {
                    StatusLabel1.Image = Properties.Resources.GreenLight;
                    StatusLabel1.Text = "AIRAC: " + t.Item1 + " (" + t.Item2 + ")";
                }
            }
            catch (Exception ex)
            {
                //if error occurred, the open the option window
                StatusLabel1.Image = Properties.Resources.RedLight;
                StatusLabel1.Text = "Failed to load Nav DB.";

                if (startingApp)
                {
                    new OptionsForm().ShowDialog();
                }
                WriteToLog(ex);
            }
        }

        private void MissedAppFuel_TextChanged(object sender, EventArgs e)
        {
            FuelReport_TxtBox.Text = "";
        }

        private void TailwindToAltn_TextChanged(object sender, EventArgs e)
        {
            FuelReport_TxtBox.Text = "";
        }

        private void updateACWtProperty()
        {
            switch (ACList.Text)
            {
                case "737-600":
                    OperatingEmptyWtKg = 36378;
                    MaxZfwKg = 51483;
                    MissedAppFuelKG = 130;
                    break;
                case "737-700":
                    OperatingEmptyWtKg = 37648;
                    MaxZfwKg = 54658;
                    MissedAppFuelKG = 130;
                    break;
                case "737-800":
                    OperatingEmptyWtKg = 41413;
                    MaxZfwKg = 61689;
                    MissedAppFuelKG = 130;
                    break;
                case "737-900":
                    OperatingEmptyWtKg = 42901;
                    MaxZfwKg = 63639;
                    MissedAppFuelKG = 130;
                    break;
                case "777-200LR":
                    OperatingEmptyWtKg = 145150;
                    MaxZfwKg = 209106;
                    MissedAppFuelKG = 300;
                    //subject to change
                    break;
                case "777F":
                    OperatingEmptyWtKg = 144400;
                    MaxZfwKg = 248115;
                    MissedAppFuelKG = 300;
                    //subject to change
                    break;
            }
        }

        private void AC_list_SelectedIndexChanged(object sender, EventArgs e)
        {
            FuelReport_TxtBox.Text = "";
            updateACWtProperty();

            double zfwKg = 0;
            double.TryParse(ZFW.Text, out zfwKg);

            if (WtUnitSel_ComboBox.Text == "KG")
            {
                MissedAppFuel.Text = Convert.ToString(MissedAppFuelKG);

                if (zfwKg > MaxZfwKg || zfwKg < OperatingEmptyWtKg)
                {
                    ZFW.Text = Convert.ToString(OperatingEmptyWtKg);
                }
            }
            else
            {
                zfwKg *= LB_KG;
                MissedAppFuel.Text = Convert.ToString(Math.Round(MissedAppFuelKG * KG_LB));

                if (zfwKg > MaxZfwKg || zfwKg < OperatingEmptyWtKg)
                {
                    ZFW.Text = Convert.ToString(Math.Round(OperatingEmptyWtKg * KG_LB));
                }

            }
            checkZfwInRange();
        }


        private void checkZfwInRange()
        {
            double ZFWKg = 0;

            if (WtUnitSel_ComboBox.Text == "KG")
            {
                ZFWKg = Convert.ToDouble(ZFW.Text);
            }
            else
            {
                ZFWKg = Convert.ToDouble(ZFW.Text) * LB_KG;
            }

            if (ZFWKg > MaxZfwKg || ZFWKg < OperatingEmptyWtKg)
            {
                ZFW.ForeColor = Color.Red;
            }
            else
            {
                ZFW.ForeColor = Color.Green;
            }

        }

        private void ZFW_TextChanged(object sender, EventArgs e)
        {
            checkZfwInRange();
            FuelReport_TxtBox.Text = "";
        }

        private void ExitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void AboutToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            new about().ShowDialog();
        }

        private void OptionsToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            var optionForm = new OptionsForm();
            optionForm.Visible = false;
            optionForm.ShowDialog();
        }

        private void setHandCusor(object sender, EventArgs e)
        {
            Cursor = Cursors.Hand;
        }

        private void setDefaultCursor(object sender, EventArgs e)
        {
            Cursor = Cursors.Default;
        }

        private void StatusLabel1_Click(object sender, EventArgs e)
        {
            new OptionsForm().ShowDialog();
        }

        private void ShowTO_Btn_Click(object sender, EventArgs e)
        {
            if (InitializeFinished_TO == false)
            {
                TakeoffLoadDefaultState();
            }
            viewChanger.ShowPage(ViewManager.Pages.TakeoffPerf);
        }

        private void ShowLDG_Btn_Click(object sender, EventArgs e)
        {
            if (InitializeFinished_LDG == false)
            {
                LDG_Load();
            }
            viewChanger.ShowPage(ViewManager.Pages.LandingPerf);
        }

        private void ShowAPData_Btn_Click(object sender, EventArgs e)
        {
            if (!InitializeFinished_AirportDataFinder)
            {
                AirportDataFinder_Load();
            }

            viewChanger.ShowPage(ViewManager.Pages.Misc);

            ICAO_ComboBox.Items.Clear();
            ICAO_ComboBox.Items.Add(OrigTxtBox.Text);
            ICAO_ComboBox.Items.Add(DestTxtBox.Text);
            ICAO_ComboBox.Items.Add(AltnTxtBox.Text);

        }

        private bool tabRefreshed = false;
        //last time the descend forcast is generated for this airport
        private string DesForcastAirportIcao = "";

        private static string GenDesForcastString(string icao)
        {
            var latlon = AirportList.AirportLatlon(icao);
            int[] FLs = { 60, 90, 120, 180, 240, 300, 340, 390, 440, 490 };
            var forcastGen = new DescendForcastGenerator(latlon.Lat, latlon.Lon, FLs);

            Wind[] w = forcastGen.Generate();
            var result = new StringBuilder();

            for (int i = 0; i < FLs.Length; i++)
            {
                result.AppendLine("        FL" + FLs[i].ToString().PadLeft(3, '0') + "   " + w[i].DirectionString() +
                    "/" + (int)w[i].Speed);
            }

            return result.ToString();
        }

        private async void Refresh_TabControl(object sender, EventArgs e)
        {
            if (TabControl1.SelectedIndex == 1 && !tabRefreshed)
            {
                await Task.Factory.StartNew(() => metarMonitor.updateOrig(OrigTxtBox.Text));
                await Task.Factory.StartNew(() => metarMonitor.updateDest(DestTxtBox.Text));
                await Task.Factory.StartNew(() => metarMonitor.updateAltn(AltnTxtBox.Text));

                updateMTDisplay();
            }
            else if (TabControl1.SelectedIndex == 2 & DesForcastAirportIcao != DestTxtBox.Text)
            {
                try
                {
                    DesForcast_RTextBox.Text = Environment.NewLine + Environment.NewLine + Environment.NewLine + "           Refreshing ...";
                    Label86.Text = "DEST / " + DestTxtBox.Text;
                    DesForcastAirportIcao = DestTxtBox.Text;

                    DesForcast_RTextBox.Text = await Task.Factory.StartNew(() => GenDesForcastString(DestTxtBox.Text));
                }
                catch (Exception ex)
                {
                    WriteToLog(ex);
                    DesForcast_RTextBox.Text = Environment.NewLine + Environment.NewLine + Environment.NewLine + "     Unable to get descend forcast for " + DestTxtBox.Text;
                }
            }
        }

        private void LinkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("http://flightaware.com/statistics/ifr-route/");
        }

        private void LinkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("http://rfinder.asalink.net/free/");
        }

        private void LinkLabel3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("http://aviationweather.gov/adds/metars/");
        }

        private void LinkLabel4_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("http://aviationweather.gov/webiffdp/page/public?name=iffdp_main");
        }

        private void LinkLabel5_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("http://aviationweather.gov/iffdp/sgwx");
        }

        private void LinkLabel6_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("https://www.notams.faa.gov/dinsQueryWeb/");
        }

        private void LinkLabel7_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("http://www.faa.gov/air_traffic/flight_info/aeronav/digital_products/");
        }

        private void LinkLabel8_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("http://www.ead.eurocontrol.int/eadcms/eadsite/index.php.html");
        }


        private void CloseMain(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (AppExitOverride)
            {
                return;
            }

            if (QspCore.AppSettings.PromptBeforeExit)
            {
                // Initializes variables to pass to the MessageBox.Show method. 

                string Message = "Exit the application?";
                string Caption = "";
                MessageBoxButtons Buttons = MessageBoxButtons.YesNo;
                MessageBoxIcon Icon = MessageBoxIcon.Question;

                DialogResult Result = default(DialogResult);

                //Displays the MessageBox
                Result = MessageBox.Show(Message, Caption, Buttons, Icon);

                // Gets the result of the MessageBox display. 
                if (Result == DialogResult.Yes)
                {
                    // Exit the app 
                    return;
                }
            }
            else
            {
                return;
            }

            e.Cancel = true;
        }

        #region "RouteGen"

        private static string PMDGrteFile;
        private enum SidStarSelection
        {
            Sid,
            Star
        }

        private void setSidStar(string icao, string rwy, ComboBox CBox, SidStarSelection para)
        {
            CBox.Items.Clear();
            List<string> proc = null;

            try
            {
                switch (para)
                {
                    case SidStarSelection.Sid:
                        proc = new SidHandler(icao).GetSidList(rwy);

                        break;
                    case SidStarSelection.Star:
                        proc = new StarHandler(icao).GetStarList(rwy);

                        break;
                    default:
                        throw new ArgumentException("Invalid Enum.");
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }

            if (proc == null || proc.Count == 0)
            {
                CBox.Items.Add("NONE");
            }
            else
            {
                CBox.Items.Add("AUTO");

                foreach (var i in proc)
                {
                    CBox.Items.Add(i);
                }
            }
            CBox.SelectedIndex = 0;
        }

        private void OrigRwyComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            setSidStar(OrigTxtBox.Text, OrigRwyComboBox.Text, OrigSidComboBox, SidStarSelection.Sid);
        }

        private void DestRwyComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            setSidStar(DestTxtBox.Text, DestRwyComboBox.Text, DestStarComboBox, SidStarSelection.Star);
        }

        private void AltnRwyComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            setSidStar(AltnTxtBox.Text, AltnRwyComboBox.Text, AltnStarComboBox, SidStarSelection.Star);
        }

        private List<string> getSidStarList(ComboBox CBox)
        {
            List<string> sidStar = new List<string>();

            if (CBox.Text == "AUTO")
            {
                foreach (var i in CBox.Items)
                {
                    string s = Convert.ToString(i);
                    if (s != "AUTO")
                    {
                        sidStar.Add(s);
                    }
                }
            }
            else if (CBox.Text != "NONE")
            {
                sidStar.Add(CBox.Text);
            }
            return sidStar;
        }

        private void FindRouteToDestBtn_Click(object sender, EventArgs e)
        {
            List<string> sid = getSidStarList(OrigSidComboBox);
            List<string> star = getSidStarList(DestStarComboBox);

            RouteToDest = new RouteFinder().FindRoute(OrigTxtBox.Text, OrigRwyComboBox.Text, sid, DestTxtBox.Text, DestRwyComboBox.Text, star);
            PMDGrteFile = FlightPlanExport.GeneratePmdgRteFile(RouteToDest);

            RouteDisplayRichTxtBox.Text = RouteToDest.ToString(false, false, ManagedRoute.TracksDisplayOption.Collapse);

            double directDis = MathTools.Utilities.GreatCircleDistance(RouteToDest.First.Waypoint.LatLon, RouteToDest.Last.Waypoint.LatLon);
            RouteDisLbl.Text = "Total Dis: " + Math.Round(RouteToDest.TotalDistance) + " NM (+" + Convert.ToString(Math.Round((RouteToDest.TotalDistance - directDis) / directDis * 1000) / 10) + "%)";
        }

        private static int computeTailWind(TailWindCalcOptions para, int tas, int Fl)
        {
            if (para == TailWindCalcOptions.OrigToDest)
            {
                return WindAloft.Utilities.AvgTailWind(RouteToDest, Fl, tas);
            }
            else
            {
                return WindAloft.Utilities.AvgTailWind(RouteToAltn, Fl, tas);
            }
        }

        private void GenRteAltnBtnClick(object sender, EventArgs e)
        {
            // Get a list of sids
            var sids = new SidHandler(DestTxtBox.Text).GetSidList(DestRwyComboBox.Text);
            var starAltn = getSidStarList(AltnStarComboBox);

            RouteToAltn = new RouteFinder().FindRoute(DestTxtBox.Text, DestRwyComboBox.Text, sids,
                                                      AltnTxtBox.Text, AltnRwyComboBox.Text, starAltn);

            RouteDisplayAltnRichTxtBox.Text = RouteToAltn.ToString(false, false, ManagedRoute.TracksDisplayOption.Collapse);

            double directDis = MathTools.Utilities.GreatCircleDistance(RouteToAltn.First.Waypoint.LatLon, RouteToAltn.First.Waypoint.LatLon);
            RouteDisAltnLbl.Text = "Total Dis: " + Math.Round(RouteToAltn.TotalDistance) + " NM (+" + Convert.ToString(Math.Round((RouteToAltn.TotalDistance - directDis) / directDis * 1000) / 10) + "%)";
        }

        private static void ExportRte()
        {
            var writer = new RouteFileWriter(PMDGrteFile);
            writer.Export();
        }

        private void ResetFromCBoxes()
        {
            FromRwyCBox.Items.Clear();
            FromSidCBox.Items.Clear();
        }

        private void ResetToCBoxes()
        {
            ToRwyCBox.Items.Clear();
            ToStarCBox.Items.Clear();
        }

        private void RadioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (RadioButton1.Checked == true)
            {
                RadioButton2.Checked = false;
                RadioButton3.Checked = false;
                RadioButton4.Checked = false;

                FromRwyCBox.Enabled = true;
                FromSidCBox.Enabled = true;
                ToRwyCBox.Enabled = true;
                ToStarCBox.Enabled = true;

                WptSelFromCBox.Enabled = false;
                WptSelToCBox.Enabled = false;
            }

        }

        private void RadioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (RadioButton2.Checked == true)
            {
                RadioButton1.Checked = false;
                RadioButton3.Checked = false;
                RadioButton4.Checked = false;

                FromRwyCBox.Enabled = true;
                FromSidCBox.Enabled = true;
                ToRwyCBox.Enabled = false;
                ToStarCBox.Enabled = false;

                WptSelFromCBox.Enabled = false;
                WptSelToCBox.Enabled = true;

                ResetToCBoxes();
            }

        }

        private void RadioButton3_CheckedChanged(object sender, EventArgs e)
        {
            if (RadioButton3.Checked == true)
            {
                RadioButton1.Checked = false;
                RadioButton2.Checked = false;
                RadioButton4.Checked = false;

                FromRwyCBox.Enabled = false;
                FromSidCBox.Enabled = false;
                ToRwyCBox.Enabled = true;
                ToStarCBox.Enabled = true;

                WptSelFromCBox.Enabled = true;
                WptSelToCBox.Enabled = false;

                ResetFromCBoxes();
            }

        }

        private void RadioButton4_CheckedChanged(object sender, EventArgs e)
        {
            if (RadioButton4.Checked == true)
            {
                RadioButton1.Checked = false;
                RadioButton2.Checked = false;
                RadioButton3.Checked = false;

                FromRwyCBox.Enabled = false;
                FromSidCBox.Enabled = false;
                ToRwyCBox.Enabled = false;
                ToStarCBox.Enabled = false;

                WptSelFromCBox.Enabled = true;
                WptSelToCBox.Enabled = true;

                ResetFromCBoxes();
                ResetToCBoxes();
            }

        }

        private void setCBox(TextBox txtBox, ComboBox RwyCBox, ComboBox WptCBox)
        {
            if (RadioButton1.Checked || RadioButton2.Checked)
            {
                RwyCBox.Items.Clear();

                if (txtBox.Text.Length != 4)
                {
                    return;
                }

                var rwyList = AirportList.RwyIdentList(txtBox.Text);

                if (rwyList != null)
                {
                    RwyCBox.Items.AddRange(rwyList);
                    RwyCBox.SelectedIndex = 0;
                }
            }
            else if (RadioButton3.Checked || RadioButton4.Checked)
            {
                WptCBox.Items.Clear();

                List<int> indices = WptList.FindAllByID(txtBox.Text);

                if (indices == null || indices.Count == 0)
                {
                    return;
                }

                string[] display = new string[indices.Count];

                for (int i = 0; i < indices.Count; i++)
                {
                    var wpt = WptList[indices[i]];
                    display[i] = "LAT/" + wpt.Lat + "  LON/" + wpt.Lon;
                }

                WptCBox.Items.AddRange(display);
                WptCBox.SelectedIndex = 0;
            }
        }

        private void FromTxtbox_TextChanged(object sender, EventArgs e)
        {
            setCBox(FromTxtbox, FromRwyCBox, WptSelFromCBox);
        }

        private void ToTxtbox_TextChanged(object sender, EventArgs e)
        {
            setCBox(ToTxtbox, ToRwyCBox, WptSelToCBox);
        }

        private void setSidStarList(ComboBox CBox, List<string> sidStarList)
        {
            CBox.Items.Clear();

            if (sidStarList.Count == 0)
            {
                CBox.Items.Add("NONE");
                CBox.SelectedIndex = 0;
                return;
            }
            else
            {
                CBox.Items.Add("AUTO");
            }

            foreach (var i in sidStarList)
            {
                CBox.Items.Add(i);
            }

            CBox.SelectedIndex = 0;
        }

        private void FromRwyCBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (RadioButton1.Checked || RadioButton2.Checked)
            {
                try
                {
                    SidHandler sidFinder = new SidHandler(FromTxtbox.Text);
                    setSidStarList(FromSidCBox, sidFinder.GetSidList(FromRwyCBox.Text));
                }
                catch (Exception ex)
                {
                    ErrorLogger.WriteToLog(ex.ToString());
                }
            }

        }

        private void To_rwy_CBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (RadioButton1.Checked || RadioButton3.Checked)
            {
                try
                {
                    var starManager = new StarHandler(ToTxtbox.Text);
                    setSidStarList(ToStarCBox, starManager.GetStarList(ToRwyCBox.Text));
                }
                catch (Exception ex)
                {
                    WriteToLog(ex);
                }
            }
        }

        private void Find_Btn_Click(object sender, EventArgs e)
        {
            if (RadioButton1.Checked)
            {
                var sid = new List<string>();
                var star = new List<string>();

                if (FromSidCBox.Text == "AUTO")
                {
                    foreach (var i in FromSidCBox.Items)
                    {
                        if (Convert.ToString(i) != "AUTO")
                        {
                            sid.Add(Convert.ToString(i));
                        }
                    }
                }
                else if (FromSidCBox.Text != "NONE")
                {
                    sid.Add(FromSidCBox.Text);
                }

                if (ToStarCBox.Text == "AUTO")
                {
                    foreach (var i in ToStarCBox.Items)
                    {
                        if (Convert.ToString(i) != "AUTO")
                        {
                            star.Add(Convert.ToString(i));
                        }
                    }
                }
                else if (ToStarCBox.Text != "NONE")
                {
                    star.Add(ToStarCBox.Text);
                }

                try
                {
                    ManagedRoute myRoute = new RouteFinder().FindRoute(FromTxtbox.Text, FromRwyCBox.Text, sid, ToTxtbox.Text, ToRwyCBox.Text, star);

                    RouteAdvancedRichTxtBox.Text = myRoute.ToString();
                    double directDis = MathTools.Utilities.GreatCircleDistance(myRoute.First.Waypoint.LatLon, myRoute.Last.Waypoint.LatLon);
                    Label56.Text = "Total Dis: " + Math.Round(myRoute.TotalDistance) + " NM (+" + Convert.ToString(Math.Round((myRoute.TotalDistance - directDis) / directDis * 1000) / 10) + "%)";


                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }

            }
            else if (RadioButton2.Checked)
            {
                List<string> sid = new List<string>();

                if (FromSidCBox.Text == "AUTO")
                {
                    foreach (var i in FromSidCBox.Items)
                    {
                        if (Convert.ToString(i) != "AUTO")
                        {
                            sid.Add(Convert.ToString(i));
                        }

                    }
                }
                else if (FromSidCBox.Text != "NONE")
                {
                    sid.Add(FromSidCBox.Text);
                }


                try
                {
                    Vector2D v = extractLatLon(WptSelToCBox.Text);

                    ManagedRoute myRoute = new RouteFinder().FindRoute(FromTxtbox.Text, FromRwyCBox.Text, sid, WptList.FindByWaypoint(ToTxtbox.Text, v.x, v.y));

                    RouteAdvancedRichTxtBox.Text = myRoute.ToString(false, true);
                    double directDis = MathTools.Utilities.GreatCircleDistance(myRoute.First.Waypoint.LatLon, myRoute.Last.Waypoint.LatLon);
                    Label56.Text = "Total Dis: " + Math.Round(myRoute.TotalDistance) + " NM (+" + Convert.ToString(Math.Round((myRoute.TotalDistance - directDis) / directDis * 1000) / 10) + "%)";


                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }

            }
            else if (RadioButton3.Checked)
            {
                List<string> star = new List<string>();

                if (ToStarCBox.Text == "AUTO")
                {
                    foreach (var i in ToStarCBox.Items)
                    {
                        if (Convert.ToString(i) != "AUTO")
                        {
                            star.Add(Convert.ToString(i));
                        }

                    }
                }
                else if (ToStarCBox.Text != "NONE")
                {
                    star.Add(ToStarCBox.Text);
                }

                try
                {
                    Vector2D v = extractLatLon(WptSelFromCBox.Text);

                    ManagedRoute myRoute = new RouteFinder().FindRoute(WptList.FindByWaypoint(FromTxtbox.Text, v.x, v.y), ToTxtbox.Text, ToRwyCBox.Text, star);

                    RouteAdvancedRichTxtBox.Text = myRoute.ToString(true, false);
                    double directDis = MathTools.Utilities.GreatCircleDistance(myRoute.First.Waypoint.LatLon, myRoute.Last.Waypoint.LatLon);
                    Label56.Text = "Total Dis: " + Math.Round(myRoute.TotalDistance) + " NM (+" + Convert.ToString(Math.Round((myRoute.TotalDistance - directDis) / directDis * 1000) / 10) + "%)";


                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }

            }
            else if (RadioButton4.Checked == true)
            {
                try
                {
                    Vector2D u = extractLatLon(WptSelFromCBox.Text);
                    Vector2D v = extractLatLon(WptSelToCBox.Text);

                    ManagedRoute myRoute = new RouteFinder().FindRoute(WptList.FindByWaypoint(FromTxtbox.Text, u.x, u.y), WptList.FindByWaypoint(ToTxtbox.Text, v.x, v.y));

                    RouteAdvancedRichTxtBox.Text = myRoute.ToString(true, true);
                    double directDis = MathTools.Utilities.GreatCircleDistance(myRoute.First.Waypoint.LatLon, myRoute.Last.Waypoint.LatLon);
                    Label56.Text = "Total Dis: " + Math.Round(myRoute.TotalDistance) + " NM (+" + Convert.ToString(Math.Round((myRoute.TotalDistance - directDis) / directDis * 1000) / 10) + "%)";


                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }
            }

        }

        /// <summary>
        /// Gets the lat and lon.
        /// </summary>
        /// <param name="s">e.g. LAT/22.55201 LON/121.3554</param>
        private static Vector2D extractLatLon(string s)
        {
            int i = s.IndexOf("LAT/");
            int j = s.IndexOf("  LON/");

            return new Vector2D(Convert.ToDouble(s.Substring(i + 4, j - i - 4)), Convert.ToDouble(s.Substring(j + 6)));
        }


        private void Analyze_RteToDest_Click(object sender, EventArgs e)
        {
            //TODO: Need better exception message for AUTO, RAND commands
            try
            {

                RouteDisplayRichTxtBox.Text = RouteDisplayRichTxtBox.Text.ToUpper();

                AnalyzerWithCommands rteAnalyzer = new AnalyzerWithCommands(OrigTxtBox.Text, OrigRwyComboBox.Text, DestTxtBox.Text, DestRwyComboBox.Text, RouteDisplayRichTxtBox.Text);

                RouteToDest = rteAnalyzer.Parse();

                PMDGrteFile = FlightPlanExport.GeneratePmdgRteFile(RouteToDest);
                RouteDisplayRichTxtBox.Text = RouteToDest.ToString(false, false, ManagedRoute.TracksDisplayOption.Collapse);

                double directDis = MathTools.Utilities.GreatCircleDistance(RouteToDest.First.Waypoint.LatLon, RouteToDest.Last.Waypoint.LatLon);
                RouteDisLbl.Text = "Total Dis: " + Math.Round(RouteToDest.TotalDistance) + " NM (+" + Convert.ToString(Math.Round((RouteToDest.TotalDistance - directDis) / directDis * 1000) / 10) + "%)";

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void Button8_Click(object sender, EventArgs e)
        {
            viewChanger.ShowPage(ViewManager.Pages.FuelCalculation);
        }

        #endregion

        #region "TakeOffPart"


        private Airport takeoffAirport;
        private bool InitializeFinished_TO = false;
        private string AC_Req;
        private WeightUnit TOWT_Req_Unit;

        private int TOWT_Req;

        private void AD_TextChanged(object sender, EventArgs e)
        {
            ADName.Text = "";
            RWY.Items.Clear();
            RWY.Enabled = false;

            if (AD.Text.Length != 4)
            {
                return;
            }

            takeoffAirport = AirportList.Find(AD.Text);


            if (takeoffAirport != null && takeoffAirport.Rwys.Count > 0)
            {
                ADName.Text = takeoffAirport.Name.PadLeft(24, ' ');

                foreach (var i in takeoffAirport.Rwys)
                {
                    RWY.Items.Add(i.RwyIdent);
                }

                RWY.SelectedIndex = 0;
                RWY.Enabled = true;

            }

        }


        private void RWY_SelectedIndexChanged(object sender, EventArgs e)
        {
            int elevationFt = 0;
            int elevationOppositeRwyFt = 0;
            int lengthFt = 0;

            int i = RWY.SelectedIndex;

            elevationFt = takeoffAirport.Rwys[i].Elevation;
            lengthFt = takeoffAirport.Rwys[i].Length;

            switch (m_ft.Text)
            {
                case "M":
                    length.Text = Convert.ToString((int)(lengthFt * AviationTools.Constants.FT_M_ratio));
                    break;
                case "FT":
                    length.Text = Convert.ToString(lengthFt);
                    break;
                case "":
                    m_ft.Text = "FT";
                    length.Text = Convert.ToString(lengthFt);
                    break;
            }

            elevation.Text = Convert.ToString(elevationFt);
            RwyHeading.Text = takeoffAirport.Rwys[i].Heading;

            int oppositeRwyIndex = -1;
            string oppRwyIdent = CoversionTools.RwyIdentOppositeDir(RWY.Text);

            for (int j = 0; j <= takeoffAirport.Rwys.Count - 1; j++)
            {
                if (takeoffAirport.Rwys[j].RwyIdent == oppRwyIdent)
                {
                    oppositeRwyIndex = j;
                }
            }

            elevationOppositeRwyFt = takeoffAirport.Rwys[oppositeRwyIndex].Elevation;

            if (oppositeRwyIndex == -1)
            {
                Slope.Text = "0";
            }
            else
            {
                Slope.Text = Convert.ToString(Math.Round((double)(elevationOppositeRwyFt - elevationFt) / lengthFt * 100 * 10) / 10);
            }

        }

        private void TakeoffLoadDefaultState()
        {
            ACListTOComboBox.Text = ACList.Text;
            ADName.Text = "";
            m_ft.SelectedIndex = 0;
            temp_c_f.SelectedIndex = 0;
            hpa_inHg.SelectedIndex = 0;
            surf_cond.SelectedIndex = 0;
            Flaps.SelectedIndex = 0;
            AISel.SelectedIndex = 0;
            PacksSel.SelectedIndex = 0;
            winddir.Text = "0";
            windspd.Text = "0";
            Slope.Text = "0";

            if (WtUnitSel_ComboBox.Text == "KG")
            {
                WTunit.Text = "KG";
            }
            else
            {
                WTunit.Text = "LB";
            }

            Slope.Items.Clear();
            for (int i = -20; i <= 20; i++)
            {
                Slope.Items.Add(Convert.ToString((double)i / 10));
            }

            formStateManagerTO.Load();
            InitializeFinished_TO = true;
        }

        private void AC_list_TO_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateFlapsComboBox();
            updateThrustRatingDisplay();
        }

        private void Calculate_Click(object sender, EventArgs e)
        {
            Aircraft ac = TOSelectedAC();
            TOPerfParameters toPara = null;

            try
            {
                toPara = QSP.TakeOffPerfCalculation.ParameterImporter.Import();
            }
            catch (InvalidUserInputException ex)
            {
                Results.ForeColor = Color.Red;
                Results.Text = ex.Message;
            }

            TOPerfCalculator perfCalc = new TOPerfCalculator(toPara, ac);

            try
            {
                Results.ForeColor = Color.Black;
                Results.Text = perfCalc.TakeOffReport().ToString(temp_c_f.SelectedIndex == 0 ? TemperatureUnit.Celsius : TemperatureUnit.Fahrenheit, m_ft.Text == "M" ? LengthUnit.Meter : LengthUnit.Feet);

            }
            catch (Exception ex)
            {

                if ((ex) is RunwayTooShortException)
                {
                    Results.ForeColor = Color.Red;
                    Results.Text = "Runway length is insufficient for take off.";


                }
                else if ((ex) is PoorClimbPerformanceException)
                {
                    Results.ForeColor = Color.Red;
                    Results.Text = "Aircraft is too heavy to meet climb performance requirement.";

                }
                else
                {
                    throw;
                }
            }
            formStateManagerTO.Save();
        }

        private Aircraft TOSelectedAC()
        {
            return ComboBoxIndexToAC(ACListTOComboBox.SelectedIndex);
        }

        private Aircraft ComboBoxIndexToAC(int index)
        {
            switch (index)
            {
                case 0:
                    return Aircraft.B737600;
                case 1:
                    return Aircraft.B737700;
                case 2:
                    return Aircraft.B737800;
                case 3:
                    return Aircraft.B737900;
                case 4:
                    return Aircraft.B777200LR;
                case 5:
                    return Aircraft.B777F;
            }
            throw new ArgumentOutOfRangeException("Invalid index for an aircraft.");
        }

        private void UpdateFlapsComboBox()
        {
            var allowedFlaps = InfoRequest.AllowedFlaps(TOSelectedAC());
            string FlapsOld = Flaps.Text;

            Flaps.Items.Clear();

            foreach (var i in allowedFlaps)
            {
                Flaps.Items.Add(i);
            }

            Flaps.SelectedIndex = 0;

            //do not clean the box if the original entry is still valid for the new aircraft
            foreach (var j in allowedFlaps)
            {
                if (FlapsOld == j)
                {
                    Flaps.Text = FlapsOld;
                    break;
                }
            }
        }


        private void updateThrustRatingDisplay()
        {
            var ac = TOSelectedAC();

            if (InfoRequest.AltnThrustRatingAvail(ac))
            {
                ThrustRating_Box.Items.Clear();

                foreach (var i in InfoRequest.AvailThrustRating(ac))
                {
                    ThrustRating_Box.Items.Add(InfoRequest.ThrustRatingToString(i));
                }

                ThrustRating_Lbl.Show();
                ThrustRating_Box.Show();
                ThrustRating_Box.SelectedIndex = 0;
            }
            else
            {
                ThrustRating_Lbl.Hide();
                ThrustRating_Box.Hide();
            }
        }

        private void m_ft_SelectedIndexChanged(object sender, EventArgs e)
        {
            double len;

            if (double.TryParse(length.Text, out len) == true)
            {
                if (m_ft.Text == "M")
                {
                    length.Text = Convert.ToString(Math.Round(len / AviationTools.Constants.M_FT_ratio));
                }
                else
                {
                    length.Text = Convert.ToString(Math.Round(len * AviationTools.Constants.M_FT_ratio));
                }
            }
        }

        private void WTunit_SelectedIndexChanged(object sender, EventArgs e)
        {
            double weight;

            if (double.TryParse(Weight.Text, out weight) && InitializeFinished_TO)
            {
                if (WTunit.Text == "KG")
                {
                    Weight.Text = Convert.ToString(Math.Round(weight * LB_KG));
                }
                else
                {
                    Weight.Text = Convert.ToString(Math.Round(weight * KG_LB));
                }
            }
        }

        private void temp_c_f_SelectedIndexChanged(object sender, EventArgs e)
        {
            double oat;

            if (double.TryParse(OAT.Text, out oat) & InitializeFinished_TO)
            {
                if (temp_c_f.Text == "°C")
                {
                    OAT.Text = Convert.ToString((int)CoversionTools.ToCelsius(oat));
                }
                else
                {
                    OAT.Text = Convert.ToString((int)CoversionTools.ToFahrenheit(oat));
                }
            }

        }

        private void hpa_inHg_SelectedIndexChanged(object sender, EventArgs e)
        {
            double press;
            if (double.TryParse(altimeter.Text, out press) & InitializeFinished_TO)
            {
                if (hpa_inHg.Text == "hPa")
                {
                    altimeter.Text = Convert.ToString(Math.Round(press * 1013 / 29.92));
                }
                else
                {
                    altimeter.Text = Convert.ToString(Math.Round((press * 29.92 / 1013) * 100) / 100);
                }
            }
        }


        private void GetMetar_Click(object sender, EventArgs e)
        {
            METAR metarForm = new METAR();

            metarForm.icao_code.Text = AD.Text;
            metarForm.FromFormName = "Takeoff";
            metarForm.icao_code.Enabled = false;

            metarForm.ShowDialog();

        }

        private void RequestBtn_Click(object sender, EventArgs e)
        {
            ACListTOComboBox.Text = AC_Req;
            WTunit.Text = EnumConversionTools.WeightUnitToString(TOWT_Req_Unit);
            Weight.Text = Convert.ToString(TOWT_Req);
        }

        private void ReqAirport_Btn_Click(object sender, EventArgs e)
        {
            AD.Text = OrigTxtBox.Text;
        }

        #endregion

        #region "LandingPart"

        public int LDG_fuel_prediction;
        public int LDG_ZFW;

        public WeightUnit LDG_fuel_prediction_unit;
        private bool InitializeFinished_LDG = false;

        private Airport landingAirport;

        private void AD_LDG_TextChanged(object sender, EventArgs e)
        {
            AD_name_LDG.Text = "";
            RWY_LDG.Items.Clear();
            RWY_LDG.Enabled = false;

            if (AD_LDG.Text.Length != 4)
            {
                return;
            }

            landingAirport = AirportList.Find(AD_LDG.Text);


            if (landingAirport != null && landingAirport.Rwys.Count > 0)
            {
                AD_name_LDG.Text = landingAirport.Name.PadLeft(24, ' ');

                foreach (var i in landingAirport.Rwys)
                {
                    RWY_LDG.Items.Add(i.RwyIdent);
                }

                RWY_LDG.SelectedIndex = 0;
                RWY_LDG.Enabled = true;

            }

        }


        private void RWY_LDG_SelectedIndexChanged(object sender, EventArgs e)
        {
            int elevationFt = 0;
            int elevationOppositeRwyFt = 0;
            int lengthFt = 0;

            int i = RWY_LDG.SelectedIndex;

            elevationFt = landingAirport.Rwys[i].Elevation;
            lengthFt = landingAirport.Rwys[i].Length;

            switch (m_ft_LDG.Text)
            {
                case "M":
                    length_LDG.Text = Convert.ToString((int)(lengthFt * AviationTools.Constants.FT_M_ratio));
                    break;
                case "FT":
                    length_LDG.Text = Convert.ToString(lengthFt);
                    break;
                case "":
                    m_ft_LDG.Text = "FT";
                    length_LDG.Text = Convert.ToString(lengthFt);
                    break;
            }

            elevation_LDG.Text = Convert.ToString(elevationFt);
            RwyHeading_LDG.Text = landingAirport.Rwys[i].Heading;

            int oppositeRwyIndex = -1;
            string oppRwyIdent = CoversionTools.RwyIdentOppositeDir(RWY_LDG.Text);

            for (int j = 0; j <= landingAirport.Rwys.Count - 1; j++)
            {
                if (landingAirport.Rwys[j].RwyIdent == oppRwyIdent)
                {
                    oppositeRwyIndex = j;
                }
            }

            elevationOppositeRwyFt = landingAirport.Rwys[oppositeRwyIndex].Elevation;

            if (oppositeRwyIndex == -1)
            {
                Slope_LDG.Text = "0";
            }
            else
            {
                Slope_LDG.Text = Convert.ToString(Math.Round((double)(elevationOppositeRwyFt - elevationFt) / lengthFt * 100 * 10) / 10);
            }

        }

        private void ACListLDG_SelectedIndexChanged(object sender, EventArgs e)
        {
            updateFlapsRev();
            updateBrks();
        }


        private void updateBrks()
        {
            string oldSetting = BrakeSel_LDG.Text;

            BrakeSel_LDG.Items.Clear();
            var perfData = LandingPerfCalculation.LoadedData.GetPara(ComboBoxIndexToAC(ACListLDG.SelectedIndex));
            BrakeSel_LDG.Items.AddRange(perfData.BrakesAvailable((LandingPerfCalculation.SurfaceCondition)SurfCond_LDG.SelectedIndex));

            for (int i = 0; i <= BrakeSel_LDG.Items.Count - 1; i++)
            {
                if (Convert.ToString(BrakeSel_LDG.Items[i]) == oldSetting)
                {
                    BrakeSel_LDG.SelectedIndex = i;
                    return;
                }
            }

            BrakeSel_LDG.SelectedIndex = BrakeSel_LDG.Items.Count - 1;

        }


        private void updateFlapsRev()
        {
            var perfData = LandingPerfCalculation.LoadedData.GetPara(ComboBoxIndexToAC(ACListLDG.SelectedIndex));

            //flaps
            Flaps_LDG.Items.Clear();
            Flaps_LDG.Items.AddRange(perfData.FlapsAvailable());
            Flaps_LDG.SelectedIndex = Flaps_LDG.Items.Count - 1;

            //reverser
            REVSel_LDG.Items.Clear();
            REVSel_LDG.Items.AddRange(perfData.RevAvailable());
            REVSel_LDG.SelectedIndex = 0;

        }

        private void RwyCond_SelectedIndexChanged(object sender, EventArgs e)
        {
            updateBrks();
        }


        private void Calc_Click(object sender, EventArgs e)
        {
            LandingPerfCalculation.LandingParameters landingPara = null;

            try
            {
                landingPara = LandingPerfCalculation.ParameterImporter.Import();

            }
            catch (InvalidUserInputException ex)
            {
                Results_LDG.ForeColor = Color.Red;
                Results_LDG.Text = ex.ToString();

            }

            var ac = ComboBoxIndexToAC(ACListLDG.SelectedIndex);
            var perfData = LandingPerfCalculation.LoadedData.GetPara(ac);

            LandingPerfCalculation.LandingCalcResult calcResult = null;

            try
            {
                calcResult = perfData.GetLandingReport(landingPara);
            }
            catch (RunwayTooShortException)
            {
                Results_LDG.ForeColor = Color.Red;
                Results_LDG.Text = "Runway length is insufficient for landing.";
                return;
            }

            Results_LDG.ForeColor = Color.Black;
            Results_LDG.Text = calcResult.ToString(m_ft_LDG.Text == "M" ? LengthUnit.Meter : LengthUnit.Feet);

            formStateManagerLDG.Save();

        }

        private int flapsDefaultSelectIndex()
        {
            return Flaps_LDG.Items.Count - 1;
        }

        private int revDefaultSelectIndex()
        {
            return 0;
        }

        private int brksDefaultSelectIndex()
        {
            return BrakeSel_LDG.Items.Count - 1;
        }


        private void LDG_Load()
        {
            ACListLDG.Text = ACList.Text;
            AD_name_LDG.Text = "";
            m_ft_LDG.SelectedIndex = 0;
            temp_c_f_LDG.SelectedIndex = 0;
            hpa_inhg_LDG.SelectedIndex = 0;
            SurfCond_LDG.SelectedIndex = 0;
            Flaps_LDG.SelectedIndex = flapsDefaultSelectIndex();
            REVSel_LDG.SelectedIndex = revDefaultSelectIndex();
            BrakeSel_LDG.SelectedIndex = brksDefaultSelectIndex();
            windspd_LDG.Text = "0";
            winddir_LDG.Text = "0";
            AppSpdInc_LDG.Text = "5";

            if (WtUnitSel_ComboBox.Text == "KG")
            {
                WtUnit_LDG.Text = "KG";
            }
            else
            {
                WtUnit_LDG.Text = "LB";
            }

            Slope_LDG.Items.Clear();
            for (int i = -20; i <= 20; i++)
            {
                Slope_LDG.Items.Add(Convert.ToString((double)i / 10));
            }

            formStateManagerLDG.Load();
            InitializeFinished_LDG = true;

        }


        private void GetMetar_LDG_Click(object sender, EventArgs e)
        {
            METAR metarForm = new METAR();

            metarForm.icao_code.Text = AD_LDG.Text;
            metarForm.FromFormName = "LDG";
            metarForm.icao_code.Enabled = false;

            metarForm.ShowDialog();

        }

        private void m_ft_LDG_SelectedIndexChanged(object sender, EventArgs e)
        {
            double len;
            if (double.TryParse(length_LDG.Text, out len))
            {
                if (m_ft_LDG.Text == "M")
                {
                    length_LDG.Text = Convert.ToString(Math.Round(len / AviationTools.Constants.M_FT_ratio));
                }
                else
                {
                    length_LDG.Text = Convert.ToString(Math.Round(len * AviationTools.Constants.M_FT_ratio));
                }
            }
        }

        private void temp_c_f_LDG_SelectedIndexChanged(object sender, EventArgs e)
        {
            double oat;
            if (double.TryParse(oat_LDG.Text, out oat) & InitializeFinished_LDG)
            {
                if (temp_c_f_LDG.Text == "°C")
                {
                    oat_LDG.Text = Convert.ToString(Math.Round((oat - 32) * 5 / 9));
                }
                else
                {
                    oat_LDG.Text = Convert.ToString(Math.Round(oat * 9 / 5 + 32));
                }
            }
        }

        private void hpa_inhg_LDG_SelectedIndexChanged(object sender, EventArgs e)
        {
            double press;
            if (double.TryParse(altimeter_LDG.Text, out press) & InitializeFinished_LDG)
            {
                if (hpa_inhg_LDG.Text == "hPa")
                {
                    altimeter_LDG.Text = Convert.ToString(Math.Round(press * 1013 / 29.92));
                }
                else
                {
                    altimeter_LDG.Text = Convert.ToString(Math.Round((press * 29.92 / 1013) * 100) / 100);
                }
            }
        }

        private void WtUnit_LDG_SelectedIndexChanged(object sender, EventArgs e)
        {
            double weight;
            if (double.TryParse(Weight_LDG.Text, out weight) && InitializeFinished_LDG)
            {
                if (WtUnit_LDG.Text == "KG")
                {
                    Weight_LDG.Text = Convert.ToString(Math.Round(weight * LB_KG));
                }
                else
                {
                    Weight_LDG.Text = Convert.ToString(Math.Round(weight * KG_LB));
                }
            }
        }

        private void RequestBtn_LDG_Click(object sender, EventArgs e)
        {
            Req_Panel.Show();
            WtUnit_Req_lbl.Text = EnumConversionTools.WeightUnitToString(LDG_fuel_prediction_unit);
        }

        private void Predict_Btn_Click(object sender, EventArgs e)
        {
            ACListLDG.Text = ACList.Text;
            WtUnit_LDG.Text = EnumConversionTools.WeightUnitToString(LDG_fuel_prediction_unit);
            Weight_LDG.Text = Convert.ToString(LDG_ZFW + LDG_fuel_prediction);
            Req_Panel.Hide();
        }

        private void Cancel_Btn_Click(object sender, EventArgs e)
        {
            Req_Panel.Hide();
        }

        private void OK_Btn_Click(object sender, EventArgs e)
        {
            double acturalLdgFuel;
            if (double.TryParse(ActualLDGFuel.Text, out acturalLdgFuel) && acturalLdgFuel >= 0)
            {
                ACListLDG.Text = AC_Req;
                WtUnit_LDG.Text = EnumConversionTools.WeightUnitToString(LDG_fuel_prediction_unit);
                Weight_LDG.Text = Convert.ToString(LDG_ZFW + acturalLdgFuel);
            }

            Req_Panel.Hide();

        }

        private void ReqAirport_Btn_Click_LDG(object sender, EventArgs e)
        {
            AD_LDG.Text = DestTxtBox.Text;
        }

        #endregion
        //========================================= Misc Part ==========================================

        bool InitializeFinished_AirportDataFinder = false;

        metar_monitor metarMonitor = new metar_monitor();
        private void metar_Lbl_Click(object sender, EventArgs e)
        {
            metar_Lbl.Text = METAR.GetMetar(ICAO_ComboBox.Text.ToUpper());
        }

        public class metar_monitor
        {
            public string orig;
            public string dest;
            public string altn;
            public string orig_mt;
            public string dest_mt;
            public string altn_mt;
            //orig = icao of origin
            //orig_mt = metar/taf of orig.

            public metar_monitor()
            {
                orig = "";
                dest = "";
                altn = "";
                orig_mt = "";
                dest_mt = "";
                altn_mt = "";
            }

            public void updateOrig(string new_orig)
            {
                orig = new_orig;
                orig_mt = METAR.GetMetarTAF(orig);
            }

            public void updateDest(string new_dest)
            {
                dest = new_dest;
                dest_mt = METAR.GetMetarTAF(dest);
            }

            public void updateAltn(string new_altn)
            {
                altn = new_altn;
                altn_mt = METAR.GetMetarTAF(altn);
            }

            public void refreshAll()
            {
                orig_mt = METAR.GetMetarTAF(orig);
                dest_mt = METAR.GetMetarTAF(dest);
                altn_mt = METAR.GetMetarTAF(altn);
            }

        }

        private void UpdateAll_Btn_Click(object sender, EventArgs e)
        {
            metarMonitor.updateOrig(OrigTxtBox.Text);
            metarMonitor.updateDest(DestTxtBox.Text);
            metarMonitor.updateAltn(AltnTxtBox.Text);

            metarMonitor.refreshAll();
            updateMTDisplay();
        }

        public void updateMTDisplay()
        {
            RichTextBox2.Text = metarMonitor.orig_mt + metarMonitor.dest_mt + metarMonitor.altn_mt;
        }

        private void DownloadMetar_Btn_Click(object sender, EventArgs e)
        {
            MetarToFindTxtBox.Text = MetarToFindTxtBox.Text.ToUpper();
            RichTextBox1.Text = METAR.GetMetarTAF(MetarToFindTxtBox.Text);
        }

        private void find_airport_btn_Click(object sender, EventArgs e)
        {
            ICAO_ComboBox.Text = ICAO_ComboBox.Text.ToUpper();
            findAirport();
        }

        private void findAirport()
        {
            Err_show_lbl.Hide();
            airport_name_Lbl.Text = "";

            string icao = ICAO_ComboBox.Text.Replace(" ", "");
            var airport = AirportList.Find(icao);


            if (airport != null && airport.Rwys.Count > 0)
            {
                metar_Lbl.Text = METAR.GetMetar(icao);

                airport_name_Lbl.Text = airport.Name;
                LatLon_Lbl2.Text = airport.Lat + " / " + airport.Lon;
                Elevation_Lbl2.Text = airport.Elevation + " FT";

                //if TL shows 0 then that means it's not a fixed value
                //show "-" instead
                if (Convert.ToInt32(airport.TransLvl) == 0)
                {
                    TATL_Lbl2.Text = Convert.ToString(airport.TransAlt) + " / -";
                }
                else
                {
                    TATL_Lbl2.Text = Convert.ToString(airport.TransAlt) + " / FL" + Convert.ToString(Math.Round((double)airport.TransLvl / 100));
                }

                LatLon_lbl1.Text = "LAT/LON:";
                Elevation_Lbl1.Text = "Elevation:";
                TATL_Lbl1.Text = "TA/TL:";

                Airport_DataGrid.Columns.Clear();
                Airport_DataGrid.Rows.Clear();
                Airport_DataGrid.ColumnCount = 10;
                Airport_DataGrid.RowCount = airport.Rwys.Count;
                setColumnsLables();

                RwyData rwy = null;


                for (int i = 0; i <= airport.Rwys.Count - 1; i++)
                {
                    rwy = airport.Rwys[i];

                    Airport_DataGrid[0, i].Value = rwy.RwyIdent;
                    Airport_DataGrid[1, i].Value = rwy.Length;
                    Airport_DataGrid[2, i].Value = rwy.Heading;
                    Airport_DataGrid[3, i].Value = rwy.Lat;
                    Airport_DataGrid[4, i].Value = rwy.Lon;

                    if (rwy.IlsAvail == true)
                    {
                        Airport_DataGrid[5, i].Value = rwy.IlsFreq;
                        Airport_DataGrid[6, i].Value = rwy.IlsHeading;
                    }
                    else
                    {
                        Airport_DataGrid[5, i].Value = "";
                        Airport_DataGrid[6, i].Value = "";
                    }

                    Airport_DataGrid[7, i].Value = rwy.ThresholdOverflyHeight;
                    Airport_DataGrid[8, i].Value = rwy.GlideslopeAngle.ToString("0.0");

                    switch (rwy.SurfaceType)
                    {
                        case 0:
                            Airport_DataGrid[9, i].Value = "Concrete";
                            break;
                        case 1:
                            Airport_DataGrid[9, i].Value = "Asphalt or Bitumen";
                            break;
                        case 2:
                            Airport_DataGrid[9, i].Value = "Gravel, Coral Or Ice";
                            break;
                        case 3:
                            Airport_DataGrid[9, i].Value = "Other";
                            break;
                    }
                }
                ShowMap(airport.Lat, airport.Lon);
            }
            else
            {
                Err_show_lbl.Text = "Airport not found.";
                Err_show_lbl.Show();
            }
        }

        private void AirportDataFinder_Load()
        {
            Err_show_lbl.Hide();

            airport_name_Lbl.Text = "";
            LatLon_lbl1.Text = "";
            LatLon_Lbl2.Text = "";
            Elevation_Lbl1.Text = "";
            Elevation_Lbl2.Text = "";
            TATL_Lbl1.Text = "";
            TATL_Lbl2.Text = "";
            metar_Lbl.Text = "";

            UpdateComboBoxList();

            InitializeFinished_AirportDataFinder = true;
        }

        public void UpdateComboBoxList()
        {
            ICAO_ComboBox.Items.Clear();
            ICAO_ComboBox.Items.Add(OrigTxtBox.Text);
            ICAO_ComboBox.Items.Add(DestTxtBox.Text);
            ICAO_ComboBox.Items.Add(AltnTxtBox.Text);
        }

        private void setColumnsLables()
        {
            Airport_DataGrid.Columns[0].Name = "RWY";
            Airport_DataGrid.Columns[1].Name = "Length(FT)";
            Airport_DataGrid.Columns[2].Name = "Heading";
            Airport_DataGrid.Columns[3].Name = "LAT";
            Airport_DataGrid.Columns[4].Name = "LON";
            Airport_DataGrid.Columns[5].Name = "ILS freq";
            Airport_DataGrid.Columns[6].Name = "ILS course";
            Airport_DataGrid.Columns[7].Name = "Threshold altitude(FT)";
            Airport_DataGrid.Columns[8].Name = "Glideslope angle";
            Airport_DataGrid.Columns[9].Name = "Surface Type";
        }

        public string GoogleMapHtml(double lat, double lon, int window_width, int window_height)
        {
            //will return a string containg the HTML code, showing in Google Map the desired airport

            string s = GoogleMapHtmlOriginal;
            // GoogleMapSample()

            s = s.Replace("51.508742,-0.120850", lat.ToString() + "," + lon.ToString());
            s = s.Replace("width:500px;height:380px", "width:" + (window_width - 20).ToString() + "px;height:" + (window_height - 30).ToString() + "px");

            return s;
        }

        private void ShowMap(double lat, double lon)
        {
            //this requires a registry fix
            //note that added registry has no effect when running in debug mode 

            MapDisplay_WebBrowser1.DocumentText = GoogleMapHtml(lat, lon, MapDisplay_WebBrowser1.Width, MapDisplay_WebBrowser1.Height);
        }

        private void ICAO_ComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            findAirport();
        }

        //==========================================  Text Data ==================================================

        string GoogleMapHtmlOriginal = @"<!DOCTYPE html>
        <html>
        <head>
        <script
        src=""http://maps.googleapis.com/maps/api/js"">
        </script>
        
        <script>
        function initialize() {
          var mapProp = {
            center:new google.maps.LatLng(51.508742,-0.120850),
            zoom:13,
            mapTypeId:google.maps.MapTypeId.ROADMAP
          };
          var map=new google.maps.Map(document.getElementById(""googleMap""), mapProp);
        }
        google.maps.event.addDomListener(window, 'load', initialize);
        </script>
        </head>
        
        <body>
        <div id=""googleMap"" style=""width:500px;height:380px;""></div>
        
        </body>
        </html>";


        private void DrawRouteToDest()
        {
            if (RouteToDest == null)
            {
                return;
            }

            StringBuilder GoogleMapDrawRoute = MapDrawing.MapDrawString(RouteToDest, MapDisWebBrowser.Width - 20, MapDisWebBrowser.Height - 30);

            var mapStr = GoogleMapDrawRoute.ToString();

            if (MapDisWebBrowser.DocumentText != mapStr)
            {
                MapDisWebBrowser.DocumentText = mapStr;
            }

        }

        private void WtManage_Btn_Click(object sender, EventArgs e)
        {
            new WtManagement().ShowDialog();
        }

        private void FindAltn_Btn_Click(object sender, EventArgs e)
        {
            new FindAltnForm().ShowDialog();
        }

        private void setRwyCBox(string icao, ComboBox combobox)
        {
            combobox.Items.Clear();

            if (icao.Length != 4)
            {
                return;
            }

            var rwyList = AirportList.RwyIdentList(icao);

            if (rwyList != null && rwyList.Count() > 0)
            {
                foreach (var i in rwyList)
                {
                    combobox.Items.Add(i);
                }

                combobox.SelectedIndex = 0;
            }
        }

        private void OrigIcao_TextChanged(object sender, EventArgs e)
        {
            setRwyCBox(OrigTxtBox.Text, OrigRwyComboBox);
        }

        private void DestIcao_TextChanged(object sender, EventArgs e)
        {
            setRwyCBox(DestTxtBox.Text, DestRwyComboBox);
        }

        private void AltnIcao_TextChanged(object sender, EventArgs e)
        {
            setRwyCBox(AltnTxtBox.Text, AltnRwyComboBox);
        }

        private void ShowMap_Btn_Click(object sender, EventArgs e)
        {
            DrawRouteToDest();
            MainWin_TablessControl.SelectedIndex = 6;
        }

        private void Return_Btn_Click(object sender, EventArgs e)
        {
            MainWin_TablessControl.SelectedIndex = 0;
        }

        private void Label42_Click(object sender, EventArgs e)
        {
            viewChanger.ShowPage(ViewManager.Pages.RouteTools);
        }

        private void moveBtnDown(int amount, Button btn)
        {
            Point pt = btn.Location;
            pt.Y += amount;
            btn.Location = pt;
        }

        private void Return2_Btn_Click(object sender, EventArgs e)
        {
            MainWin_TablessControl.SelectedIndex = 0;
        }

        private void WtUnitSel_ComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            double missedAppFuel;
            double extra;
            double zfw;

            if (string.IsNullOrEmpty(ZFW.Text))
            {
                ZFW.Text = "0";
            }

            if (double.TryParse(MissedAppFuel.Text, out missedAppFuel) &&
                    double.TryParse(ExtraFuel.Text, out extra) && double.TryParse(ZFW.Text, out zfw))
            {
                FuelReport_TxtBox.Text = "";

                if (WtUnitSel_ComboBox.Text == "KG")
                {
                    Label11.Text = "KG";
                    Label13.Text = "KG";
                    Label34.Text = "KG";

                    if (string.IsNullOrEmpty(ZFW.Text))
                    {
                        ZFW.Text = "0";
                    }

                    MissedAppFuel.Text = Convert.ToString(Math.Round(missedAppFuel * LB_KG));
                    ExtraFuel.Text = Convert.ToString(Math.Round(extra * LB_KG));
                    ZFW.Text = Convert.ToString(Math.Round(zfw * LB_KG));

                }
                else
                {
                    FuelReport_TxtBox.Text = "";

                    Label11.Text = "LB";
                    Label13.Text = "LB";
                    Label34.Text = "LB";

                    MissedAppFuel.Text = Convert.ToString(Math.Round(missedAppFuel * KG_LB));
                    ExtraFuel.Text = Convert.ToString(Math.Round(extra * KG_LB));
                    ZFW.Text = Convert.ToString(Math.Round(zfw * KG_LB));
                }
            }
        }

        private void FuelReportView_Btn_Click(object sender, EventArgs e)
        {
            viewChanger.ShowPage(ViewManager.Pages.FuelReport);
        }

        private TracksForm trkFormInstance()
        {
            return (TracksForm)Application.OpenForms.Cast<Form>().Where(x => x is TracksForm).FirstOrDefault();
        }

        private void LblTrackDownloadStatus_Click(object sender, EventArgs e)
        {
            var trkForm = trkFormInstance();

            if (trkForm == null)
            {
                trkForm = new TracksForm();
            }

            trkForm.Show();

        }

        private async void WindDownloadStatus_Lbl_Click(object sender, EventArgs e)
        {
            await downloadWind();
        }

        private void ExportBtn_Click(object sender, EventArgs e)
        {
            ExportRte();
        }

    }
}
