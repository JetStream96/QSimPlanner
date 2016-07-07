using QSP.AircraftProfiles;
using QSP.AircraftProfiles.Configs;
using QSP.Common.Options;
using QSP.FuelCalculation;
using QSP.GoogleMap;
using QSP.LibraryExtension;
using QSP.Metar;
using QSP.NavData.AAX;
using QSP.RouteFinding;
using QSP.RouteFinding.Airports;
using QSP.RouteFinding.AirwayStructure;
using QSP.RouteFinding.Containers;
using QSP.RouteFinding.Containers.CountryCode;
using QSP.RouteFinding.FileExport;
using QSP.RouteFinding.FileExport.Providers;
using QSP.RouteFinding.RouteAnalyzers;
using QSP.RouteFinding.Routes;
using QSP.RouteFinding.Routes.TrackInUse;
using QSP.RouteFinding.TerminalProcedures;
using QSP.RouteFinding.TerminalProcedures.Sid;
using QSP.RouteFinding.Tracks.Common;
using QSP.UI;
using QSP.UI.Controllers;
using QSP.UI.UserControls;
using QSP.UI.Utilities;
using QSP.Utilities;
using QSP.Utilities.Units;
using QSP.WindAloft;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static QSP.AviationTools.Constants;
using static QSP.MathTools.Doubles;
using static QSP.RouteFinding.RouteFindingCore;
using static QSP.Utilities.LoggerInstance;
using static QSP.UI.Factories.FormFactory;

namespace QSP
{
    public partial class MainForm
    {
        public ProfileManager Profiles { get; private set; }

        public int OperatingEmptyWtKg;
        //OperatingEmptyWt = Basic Operating Wt
        public int MissedAppFuelKG;
        public int MaxZfwKg;

        private FormStateSaver formStateManagerFuel;
        private FormStateSaver formStateManagerTO;

        private ViewManager viewChanger;

        private AppOptions AppSettings;
        private AirportManager airportList;
        private WaypointList wptList;
        private CountryCodeManager countryCodes;
        private WindTableCollection windTables;
        private ProcedureFilter procFilter;

        private RouteFinderSelection origAirport;
        private RouteFinderSelection destAirport;
        private RouteFinderSelection altnAirport;

        #region "FuelCalculation"

        public FuelReportResult ComputeFuelIteration(
            FuelCalculationParameters para, FuelData data, uint precisionLevel)
        {
            //presisionLevel = 0, 1, 2, ... 
            //smaller num = less precise
            //0 = disregard wind completely, 1 is good enough

            var FuelCalc = new FuelCalculator(para, data);
            var optCrz = data.OptCrzTable;
            var speedProfile = data.SpeedProfile;

            //calculate altn first
            double fuelTon = 0;
            double avgWeightTon = 0;
            double crzAltFt = 0;
            int tailwind = 0;
            double tas = 0;

            for (uint i = 0; i <= precisionLevel; i++)
            {
                FuelCalc.ReCompute();
                var result = FuelCalc.GetBriefResult();
                fuelTon = result.FuelToAltnTon;
                avgWeightTon = result.LandWeightTonAltn + fuelTon / 2;
                crzAltFt = optCrz.ActualCrzAltFt(avgWeightTon, para.DisToAltn);
                tas = speedProfile.CruiseTasKnots(crzAltFt);
                tailwind = ComputeTailWind(TailWindCalcOptions.DestToAltn, Convert.ToInt32(tas), Convert.ToInt32(crzAltFt / 100));
                para.AvgWindToAltn = tailwind;

                Debug.WriteLine("TO ALTN, CRZ ALT {0} FT, TAS {1} KTS, TAILWIND {2} KTS", crzAltFt, tas, tailwind);
            }

            for (uint i = 0; i <= precisionLevel; i++)
            {
                FuelCalc.ReCompute();
                var result = FuelCalc.GetBriefResult();
                fuelTon = result.FuelToDestTon;
                avgWeightTon = result.LandWeightTonDest + fuelTon / 2;
                crzAltFt = optCrz.ActualCrzAltFt(avgWeightTon, para.DisToDest);
                tas = speedProfile.CruiseTasKnots(crzAltFt);
                tailwind = ComputeTailWind(TailWindCalcOptions.OrigToDest, Convert.ToInt32(tas), Convert.ToInt32(crzAltFt / 100));
                para.AvgWindToDest = tailwind;

                Debug.WriteLine("TO DEST, CRZ ALT {0} FT, TAS {1} KTS, TAILWIND {2} KTS", crzAltFt, tas, tailwind);
            }

            FuelCalc.ReCompute();
            return FuelCalc.GetFullResult();
        }

        private static string GetFileName(Aircraft ac)
        {
            string s = null;

            switch (ac)
            {
                case Aircraft.B737600:
                    s = "737600";
                    break;

                case Aircraft.B737700:
                    s = "737700";
                    break;

                case Aircraft.B737800:
                    s = "737800";
                    break;

                case Aircraft.B737900:
                    s = "737900";
                    break;

                case Aircraft.B777200LR:
                    s = "777200LR";
                    break;

                case Aircraft.B777F:
                    s = "777F";
                    break;
            }

            return $"PerformanceData/FuelCalc/{s}.xml";
        }

        private void Calculate(object sender, EventArgs e)
        {
            FuelReport_TxtBox.ForeColor = Color.Black;
            FuelReport_TxtBox.Text = "";

            var parameters = new FuelCalculationParameters();
            parameters.FillInDefaultValueIfLeftBlank();

            try
            {
                parameters.ImportValues();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
                return;
            }

            var data = FuelData.FromFile(GetFileName(parameters.AC));
            FuelReportResult fuelCalcResult = null;

            try
            {
                fuelCalcResult = ComputeFuelIteration(parameters, data, 1);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return;
            }

            if (fuelCalcResult.TotalFuelKG > data.MaxFuelKg)
            {
                MessageBox.Show(InsufficientFuelMsg(fuelCalcResult.TotalFuelKG, data.MaxFuelKg, parameters.WtUnit));
                return;
            }

            string outputText = fuelCalcResult.ToString(parameters.WtUnit);

            FuelReport_TxtBox.Text = "\n" + outputText.ShiftToRight(20);
            formStateManagerFuel.Save();

            //send weights to takeoff/ldg calc form 
            AC_Req = ACList.Text;
            TOWT_Req_Unit = parameters.WtUnit;
            //TODO:        LDG_fuel_prediction_unit = Parameters.WtUnit();

            TOWT_Req = Convert.ToInt32(parameters.Zfw + fuelCalcResult.TakeoffFuelKg * (parameters.WtUnit == WeightUnit.KG ? 1.0 : KgLbRatio));
            //TODO:       LDG_ZFW = Convert.ToInt32(Parameters.Zfw);
            //TODO:  LDG_fuel_prediction = Convert.ToInt32(fuelCalcResult.LdgFuelKgPredict * (Parameters.WtUnit() == WeightUnit.KG ? 1.0 : KG_LB));

            viewChanger.ShowPage(ViewManager.Pages.FuelReport);
        }

        public enum TailWindCalcOptions
        {
            OrigToDest,
            DestToAltn
        }

        private static string InsufficientFuelMsg(double fuelReqKG, double fuelCapacityKG, WeightUnit unit)
        {
            if (unit == WeightUnit.KG)
            {
                return "Insufficient fuel" + Environment.NewLine + "Fuel required for this flight is " + fuelReqKG + " KG. Maximum fuel tank capacity is " + fuelCapacityKG + " KG.";
            }
            else
            {
                return "Insufficient fuel" + Environment.NewLine + "Fuel required for this flight is " + Math.Round(fuelReqKG * KgLbRatio) + " LB. Maximum fuel tank capacity is " + Math.Round(fuelCapacityKG * KgLbRatio) + " LB.";
            }
        }

        #endregion

        public void Init()
        {
            ShowSplashWhile(() =>
            {
                InitData();
                InitAircraftData(Profiles);
                InitRouteFinderSelections();

                //TODO: track in use is wrong
                advancedRouteTool.Init(
                    AppSettings,
                    wptList,
                    airportList,
                    new TrackInUseCollection(),
                    new ProcedureFilter(),
                    countryCodes);
            });
        }

        private void InitData()
        {
            try
            {
                // Aircraft data
                Profiles = new ProfileManager();
                Profiles.Initialize();
            }
            catch (PerfFileNotFoundException ex)
            {
                WriteToLog(ex);
                MsgBoxHelper.ShowWarning(ex.Message);
            }

            // Load options.
            try
            {
                AppSettings = OptionManager.ReadOrCreateFile();
            }
            catch (Exception ex)
            {
                WriteToLog(ex);
                MsgBoxHelper.ShowError("Cannot load options.");
            }

            // Airports and waypoints
            // TODO: exceptions?
            try
            {
                InitAirportList();
                InitWptList();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            procFilter = new ProcedureFilter();
        }

        private void InitAirportList()
        {
            string navDataPath = AppSettings.NavDataLocation;

            airportList =
            new AirportManager(
                new AirportDataLoader(navDataPath + @"\Airports.txt")
                .LoadFromFile());
        }

        private void InitWptList()
        {
            string navDataPath = AppSettings.NavDataLocation;

            var result = new WptListLoader(navDataPath)
                .LoadFromFile();

            wptList = result.WptList;
            countryCodes = result.CountryCodes;
        }

        private void ShowSplashWhile(Action action)
        {
            var splash = new Splash();
            splash.Show();
            splash.Refresh();

            action();

            splash.Close();
        }

        private void InitRouteFinderSelections()
        {
            origAirport = new RouteFinderSelection(
                OrigTxtBox,
                true,
                OrigRwyComboBox,
                OrigSidComboBox,
                new Button(),
                AppSettings,
                airportList,
                wptList,
                procFilter);

            origAirport.Subscribe();

            destAirport = new RouteFinderSelection(
                DestTxtBox,
                false,
                DestRwyComboBox,
                DestStarComboBox,
                new Button(),
                AppSettings,
                airportList,
                wptList,
                procFilter);

            destAirport.Subscribe();

            altnAirport = new RouteFinderSelection(
                AltnTxtBox,
                false,
                AltnRwyComboBox,
                AltnStarComboBox,
                new Button(),
                AppSettings,
                airportList,
                wptList,
                procFilter);

            altnAirport.Subscribe();
        }

        private void InitAircraftData(ProfileManager profiles)
        {
            toPerfControl.Initialize(
                profiles.AcConfigs, profiles.TOTables.ToList(), null);

            // TODO: toPerfControl.Airports = AirportList;
            //toPerfControl.TryLoadState();

            landingPerfControl.InitializeAircrafts(
                profiles.AcConfigs, profiles.LdgTables.ToList(), null);
            //landingPerfControl.Airports = AirportList;
            //landingPerfControl.TryLoadState();
        }

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
        }

        private bool IsRunAsAdministrator()
        {
            WindowsIdentity wi = WindowsIdentity.GetCurrent();
            WindowsPrincipal wp = new WindowsPrincipal(wi);

            return wp.IsInRole(WindowsBuiltInRole.Administrator);
        }

        private async void Startup(object sender, EventArgs e)
        {
            CheckRegistry();
            LoadDefaultState();

            if (WtUnitSel_ComboBox.Text == "KG")
            {
                ZFW.Text = Convert.ToString(OperatingEmptyWtKg);
            }
            else
            {
                ZFW.Text = Convert.ToString(Math.Round(OperatingEmptyWtKg * KgLbRatio));
            }

            LoadNavDbUpdateStatusStrip(true);
            ServiceInitializer.Initailize(airportList, wptList);
            //TakeOffPerfCalculation.LoadedData.Load();
            //TODO: LandingPerfCalculation.LoadedData.Load();

            //load previous form states
            formStateManagerFuel = new FormStateSaver(FormStateSaver.PageOfForm.FuelCalculation);
            formStateManagerTO = new FormStateSaver(FormStateSaver.PageOfForm.Takeoff);

            formStateManagerFuel.Load();

            Size = new Size(1280, 900);

            viewChanger = new ViewManager();
            viewChanger.ShowPage(ViewManager.Pages.FuelCalculation);

            StartTracksDlAsReq();
            await StartWindDlAsReq();
        }

        private static void CheckRegistry()
        {
            // Try to check/add registry so that google map works properly. 
            var regChecker = new IeEmulationChecker();

            try
            {
#if DEBUG
                regChecker.DebugRun();
#endif                
                regChecker.Run();
            }
            catch (Exception ex)
            {
                WriteToLog(ex);
            }
        }

        private void StartTracksDlAsReq()
        {
            if (AppSettings.AutoDLTracks)
            {
                //RouteFinding.Tracks.Interaction.Interactions.SetAllTracksAsync();
                //TODO: add code to start download tracks automatically.
            }
            else
            {
                LblTrackDownloadStatus.Image = Properties.Resources.YellowLight;
                LblTrackDownloadStatus.Text = "Tracks: Not downloaded";
            }
        }

        private async Task StartWindDlAsReq()
        {
            if (AppSettings.AutoDLWind)
            {
                await DownloadWind();
            }
            else
            {
                ShowWindDownloadStatus(WindDownloadStatus.WaitingManualDL);
            }
        }

        private async Task DownloadWind()
        {
            ShowWindDownloadStatus(WindDownloadStatus.Downloading);

            try
            {
                windTables = await WindManager.LoadWindAsync();
                ShowWindDownloadStatus(WindDownloadStatus.Finished);
            }
            catch (Exception ex)
            {
                WriteToLog(ex);
                ShowWindDownloadStatus(WindDownloadStatus.Failed);
            }
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

        public void LoadNavDbUpdateStatusStrip(bool startingApp)
        {
            try
            {
                //if success, update the status strip

                var t = OptionsControl.AiracCyclePeriod(AppSettings.NavDataLocation);
                //this returns, for example, (1407,26JUN23JUL/14)

                bool expired = !AiracTools.AiracValid(t.Period);
                StatusLabel1.Text = $"AIRAC: {t.Cycle} ({t.Period})";

                if (expired)
                {
                    StatusLabel1.Image = Properties.Resources.YellowLight;
                    StatusLabel1.Text += " - Expired";
                }
                else
                {
                    StatusLabel1.Image = Properties.Resources.GreenLight;
                }
            }
            catch (Exception ex)
            {
                WriteToLog(ex);
                // Open the option window
                StatusLabel1.Image = Properties.Resources.RedLight;
                StatusLabel1.Text = "Failed to load Nav DB.";

                if (startingApp)
                {
                    ShowOptionsForm();
                }
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

        private void UpdateAcWtProperty()
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
            UpdateAcWtProperty();

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
                zfwKg *= LbKgRatio;
                MissedAppFuel.Text = Convert.ToString(Math.Round(MissedAppFuelKG * KgLbRatio));

                if (zfwKg > MaxZfwKg || zfwKg < OperatingEmptyWtKg)
                {
                    ZFW.Text = Convert.ToString(Math.Round(OperatingEmptyWtKg * KgLbRatio));
                }

            }
            CheckZfwInRange();
        }


        private void CheckZfwInRange()
        {
            double ZFWKg = 0;

            if (WtUnitSel_ComboBox.Text == "KG")
            {
                ZFWKg = Convert.ToDouble(ZFW.Text);
            }
            else
            {
                ZFWKg = Convert.ToDouble(ZFW.Text) * LbKgRatio;
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
            CheckZfwInRange();
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
            ShowOptionsForm();
        }

        private void ShowOptionsForm()
        {
            var options = new OptionsControl();
            options.Init(AppSettings);
            options.AppSettingChanged += (sender, e) =>
            {
                AppSettings = options.AppSettings;
            };

            options.NavDataUpdated += (sender, e) =>
            {
                // TODO: Update Nav data here.
            };

            var frm = GetForm(options.Size);
            frm.Controls.Add(options);

            options.cancelBtn.Click += (sender, e) => frm.Close();
            options.saveBtn.Click += (sender, e) => frm.Close();

            frm.ShowDialog();
        }

        private void SetHandCursor(object sender, EventArgs e)
        {
            Cursor = Cursors.Hand;
        }

        private void SetDefaultCursor(object sender, EventArgs e)
        {
            Cursor = Cursors.Default;
        }

        private void StatusLabel1_Click(object sender, EventArgs e)
        {
            ShowOptionsForm();
        }

        private void ShowTO_Btn_Click(object sender, EventArgs e)
        {
            if (takeoffControlInitialized == false)
            {
                takeoffControlInitialized = true;
                //toPerfControl.InitializeAircrafts(null, null);//TODO: load the data here.
                toPerfControl.Airports = airportList;
                toPerfControl.TryLoadState();
            }
            viewChanger.ShowPage(ViewManager.Pages.TakeoffPerf);
        }

        private void ShowLDG_Btn_Click(object sender, EventArgs e)
        {
            if (landingControlInitialized == false)
            {
                landingControlInitialized = true;
                //landingPerfControl.InitializeAircrafts();
                landingPerfControl.Airports = airportList;
                landingPerfControl.TryLoadState();
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
            UpdateComboBoxList();
        }

        private bool tabRefreshed = false;
        //last time the descend forcast is generated for this airport
        private string DesForcastAirportIcao = "";

        private string GenDesForcastString(string icao)
        {
            var latlon = airportList.AirportLatlon(icao);
            int[] FLs = { 60, 90, 120, 180, 240, 300, 340, 390, 440, 490 };
            var forcastGen = new DescendForcastGenerator(
                windTables, latlon.Lat, latlon.Lon, FLs);

            Wind[] w = forcastGen.Generate();
            var result = new StringBuilder();

            for (int i = 0; i < FLs.Length; i++)
            {
                var flightLevel = FLs[i].ToString().PadLeft(3, '0');
                var direction = w[i].DirectionString();
                int speed = RoundToInt(w[i].Speed);

                result.AppendLine(
                    $"        FL{flightLevel}   {direction}/{speed}");
            }

            return result.ToString();
        }

        private async void Refresh_TabControl(object sender, EventArgs e)
        {
            if (TabControl1.SelectedIndex == 1 && !tabRefreshed)
            {
                await Task.Factory.StartNew(() => metarMonitor.UpdateOrig(OrigTxtBox.Text));
                await Task.Factory.StartNew(() => metarMonitor.UpdateDest(DestTxtBox.Text));
                await Task.Factory.StartNew(() => metarMonitor.UpdateAltn(AltnTxtBox.Text));

                UpdateMtDisplay();
            }
            else if (TabControl1.SelectedIndex == 2 && DesForcastAirportIcao != DestTxtBox.Text)
            {
                try
                {
                    DesForcast_RTextBox.Text = "\n\n\n           Refreshing ...";
                    Label86.Text = "DEST / " + DestTxtBox.Text;
                    DesForcastAirportIcao = DestTxtBox.Text;

                    DesForcast_RTextBox.Text =
                        await Task.Factory.StartNew(() =>
                        GenDesForcastString(DestTxtBox.Text));
                }
                catch (Exception ex)
                {
                    WriteToLog(ex);
                    DesForcast_RTextBox.Text =
                        "\n\n\n     Unable to get descend forcast for " + DestTxtBox.Text;
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

        private void CloseMain(object sender, CancelEventArgs e)
        {
            if (AppSettings.PromptBeforeExit)
            {
                string Message = "Exit the application?";
                string Caption = "";
                var Buttons = MessageBoxButtons.YesNo;
                var Icon = MessageBoxIcon.Question;

                var Result = MessageBox.Show(Message, Caption, Buttons, Icon);

                if (Result == DialogResult.No)
                {
                    // Do not exit the app.
                    e.Cancel = true;
                }
            }
        }

        #region "RouteGen"

        private static string PMDGrteFile;

        private List<string> GetSidStarList(ComboBox CBox)
        {
            var sidStar = new List<string>();

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
            var sid = GetSidStarList(OrigSidComboBox);
            var star = GetSidStarList(DestStarComboBox);

            // TODO: need to be integrated with fuel calculator
            var windCalc = windTables == null ?
                null : new AvgWindCalculator(windTables, 460, 370.0);

            var finder = new RouteFinderFacade(
                wptList,
                airportList,
                AppSettings.NavDataLocation,
                null, //TODO: add this
                windCalc);

            var result = finder.FindRoute(
                OrigTxtBox.Text, OrigRwyComboBox.Text, sid,
                DestTxtBox.Text, DestRwyComboBox.Text, star);

            RouteToDest = new RouteGroup(result, TracksInUse);

            var route = RouteToDest.Expanded;

            PMDGrteFile = new PmdgProvider(route, airportList)
                .GetExportText();

            RouteDisplayRichTxtBox.Text = route.ToString(false, false);
            UpdateRouteDistanceLbl(RouteDisLbl, route);
        }

        public static void UpdateRouteDistanceLbl(Label lbl, Route route)
        {
            double totalDis = route.GetTotalDistance();
            int disInt = RoundToInt(totalDis);
            double directDis =
                route.FirstWaypoint.DistanceFrom(route.LastWaypoint);
            double percentDiff = (totalDis - directDis) / directDis * 100;
            string diffStr = percentDiff.ToString("0.0");

            lbl.Text = $"Total Distance: {disInt} NM (+{diffStr}%)";
        }

        private int ComputeTailWind(TailWindCalcOptions para, int tas, int Fl)
        {
            if (para == TailWindCalcOptions.OrigToDest)
            {
                return WindAloft.Utilities.AvgTailWind(windTables, RouteToDest.Expanded, Fl, tas);
            }
            else
            {
                return WindAloft.Utilities.AvgTailWind(windTables, RouteToAltn.Expanded, Fl, tas);
            }
        }

        private void GenRteAltnBtnClick(object sender, EventArgs e)
        {
            // Get a list of sids
            var sids = SidHandlerFactory.GetHandler(DestTxtBox.Text, AppSettings.NavDataLocation, wptList, wptList.GetEditor(), airportList)
                                        .GetSidList(DestRwyComboBox.Text);
            var starAltn = GetSidStarList(AltnStarComboBox);

            RouteToAltn = new RouteGroup(new RouteFinderFacade(wptList, airportList, AppSettings.NavDataLocation)
                                           .FindRoute(DestTxtBox.Text, DestRwyComboBox.Text, sids,
                                                      AltnTxtBox.Text, AltnRwyComboBox.Text, starAltn),
                                           TracksInUse);

            var route = RouteToAltn.Expanded;

            RouteDisplayAltnRichTxtBox.Text = route.ToString(false, false);
            UpdateRouteDistanceLbl(RouteDisAltnLbl, route);
        }

        private void ExportRouteFiles()
        {
            var cmds = AppSettings.ExportCommands.Values;
            var writer = new FileExporter(RouteToDest.Expanded, airportList, cmds);

            var reports = writer.Export();
            ShowReports(reports);
        }

        private static void ShowReports(IEnumerable<FileExporter.Status> reports)
        {
            if (reports.Count() == 0)
            {
                MessageBox.Show(
                    "No route file to be exported. " +
                    "Please select select export settings in options page.",
                    "",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            else
            {
                var msg = new StringBuilder();
                var success = reports.Where(r => r.Successful);

                if (success.Count() > 0)
                {
                    msg.AppendLine(
                        $"{success.Count()} company route(s) exported:");

                    foreach (var i in success)
                    {
                        msg.AppendLine(i.FilePath);
                    }
                }

                var errors = reports.Where(r => r.Successful == false);

                if (errors.Count() > 0)
                {
                    msg.AppendLine(
                        $"\n\nFailed to export {errors.Count()} file(s) into:");

                    foreach (var j in errors)
                    {
                        msg.AppendLine(j.FilePath);
                    }
                }

                var icon =
                    errors.Count() > 0 ?
                    MessageBoxIcon.Warning :
                    MessageBoxIcon.Information;

                MessageBox.Show(
                    msg.ToString(),
                    "",
                    MessageBoxButtons.OK,
                    icon);
            }
        }

        private void Analyze_RteToDest_Click(object sender, EventArgs e)
        {
            //TODO: Need better exception message for AUTO, RAND commands
            try
            {
                RouteDisplayRichTxtBox.Text = RouteDisplayRichTxtBox.Text.ToUpper();

                RouteToDest =
                    new RouteGroup(
                        RouteAnalyzerFacade.AnalyzeWithCommands(
                            RouteDisplayRichTxtBox.Text,
                            OrigTxtBox.Text,
                            OrigRwyComboBox.Text,
                            DestTxtBox.Text,
                            DestRwyComboBox.Text,
                            AppSettings.NavDataLocation,
                            airportList,
                            wptList),
                        TracksInUse);

                var route = RouteToDest.Expanded;

                PMDGrteFile = new PmdgProvider(route, airportList)
                .GetExportText();

                RouteDisplayRichTxtBox.Text = route.ToString(false, false);
                UpdateRouteDistanceLbl(RouteDisLbl, route);
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

        private bool takeoffControlInitialized = false;
        private string AC_Req;
        private WeightUnit TOWT_Req_Unit;

        private int TOWT_Req;

        #endregion

        #region "LandingPart"

        private bool landingControlInitialized = false;

        #endregion
        //========================================= Misc Part ==========================================

        bool InitializeFinished_AirportDataFinder = false;

        MetarMonitor metarMonitor = new MetarMonitor();

        public class MetarMonitor
        {
            public string orig;
            public string dest;
            public string altn;
            public string orig_mt;
            public string dest_mt;
            public string altn_mt;
            //orig = icao of origin
            //orig_mt = metar/taf of orig.

            public MetarMonitor()
            {
                orig = "";
                dest = "";
                altn = "";
                orig_mt = "";
                dest_mt = "";
                altn_mt = "";
            }

            public void UpdateOrig(string new_orig)
            {
                orig = new_orig;
                orig_mt = MetarDownloader.TryGetMetarTaf(orig);
            }

            public void UpdateDest(string new_dest)
            {
                dest = new_dest;
                dest_mt = MetarDownloader.TryGetMetarTaf(dest);
            }

            public void UpdateAltn(string new_altn)
            {
                altn = new_altn;
                altn_mt = MetarDownloader.TryGetMetarTaf(altn);
            }

            public void RefreshAll()
            {
                orig_mt = MetarDownloader.TryGetMetarTaf(orig);
                dest_mt = MetarDownloader.TryGetMetarTaf(dest);
                altn_mt = MetarDownloader.TryGetMetarTaf(altn);
            }

        }

        private void UpdateAll_Btn_Click(object sender, EventArgs e)
        {
            metarMonitor.UpdateOrig(OrigTxtBox.Text);
            metarMonitor.UpdateDest(DestTxtBox.Text);
            metarMonitor.UpdateAltn(AltnTxtBox.Text);

            metarMonitor.RefreshAll();
            UpdateMtDisplay();
        }

        public void UpdateMtDisplay()
        {
            RichTextBox2.Text = metarMonitor.orig_mt + metarMonitor.dest_mt + metarMonitor.altn_mt;
        }

        private void DownloadMetar_Btn_Click(object sender, EventArgs e)
        {
            MetarToFindTxtBox.Text = MetarToFindTxtBox.Text.ToUpper();
            RichTextBox1.Text = MetarDownloader.TryGetMetarTaf(MetarToFindTxtBox.Text);
        }

        private void AirportDataFinder_Load()
        {
            airportMapControl.Initialize(airportList);
            airportMapControl.BrowserEnabled = true;
            UpdateComboBoxList();

            InitializeFinished_AirportDataFinder = true;
        }

        public void UpdateComboBoxList()
        {
            var icaoList = airportMapControl.icaoComboBox.Items;

            icaoList.Clear();
            icaoList.Add(OrigTxtBox.Text);
            icaoList.Add(DestTxtBox.Text);
            icaoList.Add(AltnTxtBox.Text);
        }

        private void DrawRouteToDest()
        {
            if (RouteToDest == null)
            {
                return;
            }

            StringBuilder GoogleMapDrawRoute =
                RouteDrawing.MapDrawString(
                    RouteToDest.Expanded, MapDisWebBrowser.Width - 20, MapDisWebBrowser.Height - 30);

            var mapStr = GoogleMapDrawRoute.ToString();
            MapDisWebBrowser.DocumentText = mapStr;
        }

        private void WtManage_Btn_Click(object sender, EventArgs e)
        {
            new WtManagement().ShowDialog();
        }

        private void FindAltn_Btn_Click(object sender, EventArgs e)
        {
            var altnFrm = new FindAltnForm();
            altnFrm.Initialize(airportList);
            altnFrm.ShowDialog();
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

        private void MoveBtnDown(int amount, Button btn)
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
                double.TryParse(ExtraFuel.Text, out extra) &&
                double.TryParse(ZFW.Text, out zfw))
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

                    MissedAppFuel.Text = Convert.ToString(Math.Round(missedAppFuel * LbKgRatio));
                    ExtraFuel.Text = Convert.ToString(Math.Round(extra * LbKgRatio));
                    ZFW.Text = Convert.ToString(Math.Round(zfw * LbKgRatio));
                }
                else
                {
                    FuelReport_TxtBox.Text = "";

                    Label11.Text = "LB";
                    Label13.Text = "LB";
                    Label34.Text = "LB";

                    MissedAppFuel.Text = Convert.ToString(Math.Round(missedAppFuel * KgLbRatio));
                    ExtraFuel.Text = Convert.ToString(Math.Round(extra * KgLbRatio));
                    ZFW.Text = Convert.ToString(Math.Round(zfw * KgLbRatio));
                }
            }
        }

        private void FuelReportView_Btn_Click(object sender, EventArgs e)
        {
            viewChanger.ShowPage(ViewManager.Pages.FuelReport);
        }

        private TracksForm TrkFormInstance()
        {
            return (TracksForm)Application.OpenForms.Cast<Form>().Where(x => x is TracksForm).FirstOrDefault();
        }

        private void LblTrackDownloadStatus_Click(object sender, EventArgs e)
        {
            var trkForm = TrkFormInstance();

            if (trkForm == null)
            {
                trkForm = new TracksForm();
            }

            trkForm.Show();
        }

        private async void WindDownloadStatus_Lbl_Click(object sender, EventArgs e)
        {
            await DownloadWind();
        }

        private void ExportBtn_Click(object sender, EventArgs e)
        {
            ExportRouteFiles();
        }

    }
}
