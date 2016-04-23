using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using System.Linq;
using System.Xml.Linq;
namespace QSP
{
    partial class MainForm : System.Windows.Forms.Form
    {

        //Form overrides dispose to clean up the component list.
        [System.Diagnostics.DebuggerNonUserCode()]
        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null)
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        //Required by the Windows Form Designer

        private System.ComponentModel.IContainer components;
        //NOTE: The following procedure is required by the Windows Form Designer
        //It can be modified using the Windows Form Designer.  
        //Do not modify it using the code editor.
        [System.Diagnostics.DebuggerStepThrough()]
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.MenuStrip1 = new System.Windows.Forms.MenuStrip();
            this.MainToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.OptionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ExitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.AboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.AboutToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.StatusStrip1 = new System.Windows.Forms.StatusStrip();
            this.StatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.ToolStripStatusLabel3 = new System.Windows.Forms.ToolStripStatusLabel();
            this.LblTrackDownloadStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.ToolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.WindDownloadStatus_Lbl = new System.Windows.Forms.ToolStripStatusLabel();
            this.WindowSelPanel = new System.Windows.Forms.Panel();
            this.TableLayoutPanel17 = new System.Windows.Forms.TableLayoutPanel();
            this.PictureBox_Icon_APData = new System.Windows.Forms.PictureBox();
            this.PictureBox_Icon_LDG = new System.Windows.Forms.PictureBox();
            this.PictureBox_Icon_Fuel = new System.Windows.Forms.PictureBox();
            this.PictureBox_Icon_TO = new System.Windows.Forms.PictureBox();
            this.TableLayoutPanel16 = new System.Windows.Forms.TableLayoutPanel();
            this.ShowMisc_Btn = new System.Windows.Forms.Button();
            this.ShowFuel_Btn = new System.Windows.Forms.Button();
            this.ShowLDG_Btn = new System.Windows.Forms.Button();
            this.ShowTO_Btn = new System.Windows.Forms.Button();
            this.ToolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.UpdateAll_Btn = new System.Windows.Forms.Button();
            this.MainWin_TablessControl = new QSP.TablessControl();
            this.Route_TabPage = new System.Windows.Forms.TabPage();
            this.FuelReportView_Btn = new System.Windows.Forms.Button();
            this.GroupBox6 = new System.Windows.Forms.GroupBox();
            this.TableLayoutPanel20 = new System.Windows.Forms.TableLayoutPanel();
            this.Label19 = new System.Windows.Forms.Label();
            this.FinalRsv = new System.Windows.Forms.TextBox();
            this.Label16 = new System.Windows.Forms.Label();
            this.Label28 = new System.Windows.Forms.Label();
            this.Label15 = new System.Windows.Forms.Label();
            this.TaxiTime = new System.Windows.Forms.TextBox();
            this.Label13 = new System.Windows.Forms.Label();
            this.ExtraFuel = new System.Windows.Forms.TextBox();
            this.APUTime = new System.Windows.Forms.TextBox();
            this.Label8 = new System.Windows.Forms.Label();
            this.Label7 = new System.Windows.Forms.Label();
            this.Label6 = new System.Windows.Forms.Label();
            this.TableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.Label25 = new System.Windows.Forms.Label();
            this.HoldTime_TxtBox = new System.Windows.Forms.TextBox();
            this.Label11 = new System.Windows.Forms.Label();
            this.Label12 = new System.Windows.Forms.Label();
            this.WtUnitSel_ComboBox = new System.Windows.Forms.ComboBox();
            this.ContPercentToDest = new System.Windows.Forms.ComboBox();
            this.Label112 = new System.Windows.Forms.Label();
            this.MissedAppFuel = new System.Windows.Forms.TextBox();
            this.Label20 = new System.Windows.Forms.Label();
            this.Label4 = new System.Windows.Forms.Label();
            this.Label5 = new System.Windows.Forms.Label();
            this.Calculate_Btn = new System.Windows.Forms.Button();
            this.GroupBox5 = new System.Windows.Forms.GroupBox();
            this.RouteDisAltnLbl = new System.Windows.Forms.Label();
            this.RouteDisLbl = new System.Windows.Forms.Label();
            this.TableLayoutPanel24 = new System.Windows.Forms.TableLayoutPanel();
            this.ExportBtn = new System.Windows.Forms.Button();
            this.ShowMap_Btn = new System.Windows.Forms.Button();
            this.FindRouteToDestBtn = new System.Windows.Forms.Button();
            this.AnalyzeRteToDest_Btn = new System.Windows.Forms.Button();
            this.Label42 = new System.Windows.Forms.Label();
            this.TableLayoutPanel21 = new System.Windows.Forms.TableLayoutPanel();
            this.genRteAltn_Btn = new System.Windows.Forms.Button();
            this.Label41 = new System.Windows.Forms.Label();
            this.RouteDisplayAltnRichTxtBox = new System.Windows.Forms.RichTextBox();
            this.Label40 = new System.Windows.Forms.Label();
            this.RouteDisplayRichTxtBox = new System.Windows.Forms.RichTextBox();
            this.GroupBox_BasicInfo = new System.Windows.Forms.GroupBox();
            this.Label34 = new System.Windows.Forms.Label();
            this.ZFW = new System.Windows.Forms.TextBox();
            this.ACList = new System.Windows.Forms.ComboBox();
            this.WtManage_Btn = new System.Windows.Forms.Button();
            this.TableLayoutPanel8 = new System.Windows.Forms.TableLayoutPanel();
            this.Label111 = new System.Windows.Forms.Label();
            this.Label110 = new System.Windows.Forms.Label();
            this.Label38 = new System.Windows.Forms.Label();
            this.AltnStarComboBox = new System.Windows.Forms.ComboBox();
            this.Label36 = new System.Windows.Forms.Label();
            this.DestStarComboBox = new System.Windows.Forms.ComboBox();
            this.OrigSidComboBox = new System.Windows.Forms.ComboBox();
            this.Label109 = new System.Windows.Forms.Label();
            this.Label18 = new System.Windows.Forms.Label();
            this.AltnTxtBox = new System.Windows.Forms.TextBox();
            this.AltnRwyComboBox = new System.Windows.Forms.ComboBox();
            this.OrigRwyComboBox = new System.Windows.Forms.ComboBox();
            this.DestRwyComboBox = new System.Windows.Forms.ComboBox();
            this.DestTxtBox = new System.Windows.Forms.TextBox();
            this.OrigTxtBox = new System.Windows.Forms.TextBox();
            this.Label26 = new System.Windows.Forms.Label();
            this.FindAltn_Btn = new System.Windows.Forms.Button();
            this.Label27 = new System.Windows.Forms.Label();
            this.Label33 = new System.Windows.Forms.Label();
            this.TableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.Label_AC = new System.Windows.Forms.Label();
            this.Label17 = new System.Windows.Forms.Label();
            this.TabPage9 = new System.Windows.Forms.TabPage();
            this.PanelTO = new System.Windows.Forms.Panel();
            this.toPerfControl = new QSP.UI.ToLdgModule.TOPerf.TOPerfControl();
            this.TabPage10 = new System.Windows.Forms.TabPage();
            this.landingPerfControl = new QSP.UI.ToLdgModule.LandingPerf.LandingPerfControl();
            this.TabPage11 = new System.Windows.Forms.TabPage();
            this.PanelAirportData = new System.Windows.Forms.Panel();
            this.TabControl1 = new System.Windows.Forms.TabControl();
            this.TabPage1 = new System.Windows.Forms.TabPage();
            this.airportMapControl = new QSP.UI.ToLdgModule.AirportMap.AirportMapControl();
            this.TabPage2 = new System.Windows.Forms.TabPage();
            this.GroupBox18 = new System.Windows.Forms.GroupBox();
            this.RichTextBox2 = new System.Windows.Forms.RichTextBox();
            this.GroupBox17 = new System.Windows.Forms.GroupBox();
            this.MetarToFindTxtBox = new System.Windows.Forms.TextBox();
            this.DownloadMetar_Btn = new System.Windows.Forms.Button();
            this.RichTextBox1 = new System.Windows.Forms.RichTextBox();
            this.TabPage6 = new System.Windows.Forms.TabPage();
            this.GroupBox19 = new System.Windows.Forms.GroupBox();
            this.DesForcast_RTextBox = new System.Windows.Forms.RichTextBox();
            this.Label86 = new System.Windows.Forms.Label();
            this.TabPage7 = new System.Windows.Forms.TabPage();
            this.GroupBox7 = new System.Windows.Forms.GroupBox();
            this.LinkLabel8 = new System.Windows.Forms.LinkLabel();
            this.LinkLabel7 = new System.Windows.Forms.LinkLabel();
            this.LinkLabel6 = new System.Windows.Forms.LinkLabel();
            this.LinkLabel5 = new System.Windows.Forms.LinkLabel();
            this.LinkLabel4 = new System.Windows.Forms.LinkLabel();
            this.LinkLabel3 = new System.Windows.Forms.LinkLabel();
            this.LinkLabel2 = new System.Windows.Forms.LinkLabel();
            this.LinkLabel1 = new System.Windows.Forms.LinkLabel();
            this.TabPage12 = new System.Windows.Forms.TabPage();
            this.TabPage13 = new System.Windows.Forms.TabPage();
            this.Return_Btn = new System.Windows.Forms.Button();
            this.MapDisWebBrowser = new System.Windows.Forms.WebBrowser();
            this.TabPage14 = new System.Windows.Forms.TabPage();
            this.Return_AdvancedTool_Btn = new System.Windows.Forms.Button();
            this.GroupBox8 = new System.Windows.Forms.GroupBox();
            this.Label56 = new System.Windows.Forms.Label();
            this.Find_Btn = new System.Windows.Forms.Button();
            this.RouteAdvancedRichTxtBox = new System.Windows.Forms.RichTextBox();
            this.WptSelToCBox = new System.Windows.Forms.ComboBox();
            this.WptSelFromCBox = new System.Windows.Forms.ComboBox();
            this.RadioButton4 = new System.Windows.Forms.RadioButton();
            this.RadioButton3 = new System.Windows.Forms.RadioButton();
            this.RadioButton2 = new System.Windows.Forms.RadioButton();
            this.RadioButton1 = new System.Windows.Forms.RadioButton();
            this.Label64 = new System.Windows.Forms.Label();
            this.ToStarCBox = new System.Windows.Forms.ComboBox();
            this.FromSidCBox = new System.Windows.Forms.ComboBox();
            this.ToRwyCBox = new System.Windows.Forms.ComboBox();
            this.FromRwyCBox = new System.Windows.Forms.ComboBox();
            this.Label65 = new System.Windows.Forms.Label();
            this.FromTxtbox = new System.Windows.Forms.TextBox();
            this.ToTxtbox = new System.Windows.Forms.TextBox();
            this.Label66 = new System.Windows.Forms.Label();
            this.TabPage3 = new System.Windows.Forms.TabPage();
            this.Return2_Btn = new System.Windows.Forms.Button();
            this.GroupBox3 = new System.Windows.Forms.GroupBox();
            this.FuelReport_TxtBox = new System.Windows.Forms.RichTextBox();
            this.MenuStrip1.SuspendLayout();
            this.StatusStrip1.SuspendLayout();
            this.WindowSelPanel.SuspendLayout();
            this.TableLayoutPanel17.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PictureBox_Icon_APData)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PictureBox_Icon_LDG)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PictureBox_Icon_Fuel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PictureBox_Icon_TO)).BeginInit();
            this.TableLayoutPanel16.SuspendLayout();
            this.MainWin_TablessControl.SuspendLayout();
            this.Route_TabPage.SuspendLayout();
            this.GroupBox6.SuspendLayout();
            this.TableLayoutPanel20.SuspendLayout();
            this.TableLayoutPanel1.SuspendLayout();
            this.GroupBox5.SuspendLayout();
            this.TableLayoutPanel24.SuspendLayout();
            this.TableLayoutPanel21.SuspendLayout();
            this.GroupBox_BasicInfo.SuspendLayout();
            this.TableLayoutPanel8.SuspendLayout();
            this.TableLayoutPanel4.SuspendLayout();
            this.TabPage9.SuspendLayout();
            this.PanelTO.SuspendLayout();
            this.TabPage10.SuspendLayout();
            this.TabPage11.SuspendLayout();
            this.PanelAirportData.SuspendLayout();
            this.TabControl1.SuspendLayout();
            this.TabPage1.SuspendLayout();
            this.TabPage2.SuspendLayout();
            this.GroupBox18.SuspendLayout();
            this.GroupBox17.SuspendLayout();
            this.TabPage6.SuspendLayout();
            this.GroupBox19.SuspendLayout();
            this.TabPage7.SuspendLayout();
            this.GroupBox7.SuspendLayout();
            this.TabPage13.SuspendLayout();
            this.TabPage14.SuspendLayout();
            this.GroupBox8.SuspendLayout();
            this.TabPage3.SuspendLayout();
            this.GroupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // MenuStrip1
            // 
            this.MenuStrip1.BackColor = System.Drawing.Color.Transparent;
            this.MenuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.MenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MainToolStripMenuItem,
            this.AboutToolStripMenuItem});
            this.MenuStrip1.Location = new System.Drawing.Point(0, 0);
            this.MenuStrip1.Name = "MenuStrip1";
            this.MenuStrip1.Padding = new System.Windows.Forms.Padding(8, 2, 0, 2);
            this.MenuStrip1.Size = new System.Drawing.Size(1262, 28);
            this.MenuStrip1.TabIndex = 33;
            this.MenuStrip1.Text = "MenuStrip1";
            // 
            // MainToolStripMenuItem
            // 
            this.MainToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.OptionsToolStripMenuItem,
            this.ExitToolStripMenuItem});
            this.MainToolStripMenuItem.Name = "MainToolStripMenuItem";
            this.MainToolStripMenuItem.Size = new System.Drawing.Size(76, 24);
            this.MainToolStripMenuItem.Text = "Window";
            // 
            // OptionsToolStripMenuItem
            // 
            this.OptionsToolStripMenuItem.Name = "OptionsToolStripMenuItem";
            this.OptionsToolStripMenuItem.Size = new System.Drawing.Size(136, 26);
            this.OptionsToolStripMenuItem.Text = "Options";
            this.OptionsToolStripMenuItem.Click += new System.EventHandler(this.OptionsToolStripMenuItem_Click_1);
            // 
            // ExitToolStripMenuItem
            // 
            this.ExitToolStripMenuItem.Name = "ExitToolStripMenuItem";
            this.ExitToolStripMenuItem.Size = new System.Drawing.Size(136, 26);
            this.ExitToolStripMenuItem.Text = "Exit";
            this.ExitToolStripMenuItem.Click += new System.EventHandler(this.ExitToolStripMenuItem_Click);
            // 
            // AboutToolStripMenuItem
            // 
            this.AboutToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.AboutToolStripMenuItem1});
            this.AboutToolStripMenuItem.Name = "AboutToolStripMenuItem";
            this.AboutToolStripMenuItem.Size = new System.Drawing.Size(53, 24);
            this.AboutToolStripMenuItem.Text = "Help";
            // 
            // AboutToolStripMenuItem1
            // 
            this.AboutToolStripMenuItem1.Name = "AboutToolStripMenuItem1";
            this.AboutToolStripMenuItem1.Size = new System.Drawing.Size(125, 26);
            this.AboutToolStripMenuItem1.Text = "About";
            this.AboutToolStripMenuItem1.Click += new System.EventHandler(this.AboutToolStripMenuItem1_Click);
            // 
            // StatusStrip1
            // 
            this.StatusStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.StatusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.StatusLabel1,
            this.ToolStripStatusLabel3,
            this.LblTrackDownloadStatus,
            this.ToolStripStatusLabel2,
            this.WindDownloadStatus_Lbl});
            this.StatusStrip1.Location = new System.Drawing.Point(0, 830);
            this.StatusStrip1.Name = "StatusStrip1";
            this.StatusStrip1.Size = new System.Drawing.Size(1262, 25);
            this.StatusStrip1.TabIndex = 39;
            this.StatusStrip1.Text = "StatusStrip1";
            // 
            // StatusLabel1
            // 
            this.StatusLabel1.BackColor = System.Drawing.Color.Transparent;
            this.StatusLabel1.Image = global::QSP.Properties.Resources.GreenLight;
            this.StatusLabel1.Name = "StatusLabel1";
            this.StatusLabel1.Size = new System.Drawing.Size(195, 20);
            this.StatusLabel1.Text = "Database Loaded: PMDG";
            this.StatusLabel1.ToolTipText = "Click to open options page";
            this.StatusLabel1.Click += new System.EventHandler(this.StatusLabel1_Click);
            this.StatusLabel1.MouseEnter += new System.EventHandler(this.setHandCusor);
            this.StatusLabel1.MouseLeave += new System.EventHandler(this.setDefaultCursor);
            // 
            // ToolStripStatusLabel3
            // 
            this.ToolStripStatusLabel3.BackColor = System.Drawing.Color.Transparent;
            this.ToolStripStatusLabel3.Name = "ToolStripStatusLabel3";
            this.ToolStripStatusLabel3.Size = new System.Drawing.Size(45, 20);
            this.ToolStripStatusLabel3.Text = "    |    ";
            // 
            // LblTrackDownloadStatus
            // 
            this.LblTrackDownloadStatus.BackColor = System.Drawing.Color.Transparent;
            this.LblTrackDownloadStatus.Image = global::QSP.Properties.Resources.YellowLight;
            this.LblTrackDownloadStatus.Name = "LblTrackDownloadStatus";
            this.LblTrackDownloadStatus.Size = new System.Drawing.Size(188, 20);
            this.LblTrackDownloadStatus.Text = "NatDownloadStatus_Lbl";
            this.LblTrackDownloadStatus.Click += new System.EventHandler(this.LblTrackDownloadStatus_Click);
            this.LblTrackDownloadStatus.MouseEnter += new System.EventHandler(this.setHandCusor);
            this.LblTrackDownloadStatus.MouseLeave += new System.EventHandler(this.setDefaultCursor);
            // 
            // ToolStripStatusLabel2
            // 
            this.ToolStripStatusLabel2.BackColor = System.Drawing.Color.Transparent;
            this.ToolStripStatusLabel2.Name = "ToolStripStatusLabel2";
            this.ToolStripStatusLabel2.Size = new System.Drawing.Size(29, 20);
            this.ToolStripStatusLabel2.Text = "  |  ";
            // 
            // WindDownloadStatus_Lbl
            // 
            this.WindDownloadStatus_Lbl.BackColor = System.Drawing.Color.Transparent;
            this.WindDownloadStatus_Lbl.Name = "WindDownloadStatus_Lbl";
            this.WindDownloadStatus_Lbl.Size = new System.Drawing.Size(179, 20);
            this.WindDownloadStatus_Lbl.Text = "WindDownloadStatus_Lbl";
            this.WindDownloadStatus_Lbl.Click += new System.EventHandler(this.WindDownloadStatus_Lbl_Click);
            this.WindDownloadStatus_Lbl.MouseEnter += new System.EventHandler(this.setHandCusor);
            this.WindDownloadStatus_Lbl.MouseLeave += new System.EventHandler(this.setDefaultCursor);
            // 
            // WindowSelPanel
            // 
            this.WindowSelPanel.BackColor = System.Drawing.SystemColors.Control;
            this.WindowSelPanel.Controls.Add(this.TableLayoutPanel17);
            this.WindowSelPanel.Controls.Add(this.TableLayoutPanel16);
            this.WindowSelPanel.Location = new System.Drawing.Point(0, 45);
            this.WindowSelPanel.Name = "WindowSelPanel";
            this.WindowSelPanel.Size = new System.Drawing.Size(117, 730);
            this.WindowSelPanel.TabIndex = 40;
            // 
            // TableLayoutPanel17
            // 
            this.TableLayoutPanel17.ColumnCount = 1;
            this.TableLayoutPanel17.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.TableLayoutPanel17.Controls.Add(this.PictureBox_Icon_APData, 0, 3);
            this.TableLayoutPanel17.Controls.Add(this.PictureBox_Icon_LDG, 0, 2);
            this.TableLayoutPanel17.Controls.Add(this.PictureBox_Icon_Fuel, 0, 0);
            this.TableLayoutPanel17.Controls.Add(this.PictureBox_Icon_TO, 0, 1);
            this.TableLayoutPanel17.Location = new System.Drawing.Point(92, 44);
            this.TableLayoutPanel17.Name = "TableLayoutPanel17";
            this.TableLayoutPanel17.RowCount = 4;
            this.TableLayoutPanel17.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 90F));
            this.TableLayoutPanel17.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 90F));
            this.TableLayoutPanel17.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 90F));
            this.TableLayoutPanel17.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 90F));
            this.TableLayoutPanel17.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.TableLayoutPanel17.Size = new System.Drawing.Size(20, 354);
            this.TableLayoutPanel17.TabIndex = 77;
            // 
            // PictureBox_Icon_APData
            // 
            this.PictureBox_Icon_APData.BackgroundImage = global::QSP.Properties.Resources.arrow;
            this.PictureBox_Icon_APData.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.PictureBox_Icon_APData.InitialImage = null;
            this.PictureBox_Icon_APData.Location = new System.Drawing.Point(0, 270);
            this.PictureBox_Icon_APData.Margin = new System.Windows.Forms.Padding(0);
            this.PictureBox_Icon_APData.Name = "PictureBox_Icon_APData";
            this.PictureBox_Icon_APData.Size = new System.Drawing.Size(20, 30);
            this.PictureBox_Icon_APData.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.PictureBox_Icon_APData.TabIndex = 81;
            this.PictureBox_Icon_APData.TabStop = false;
            // 
            // PictureBox_Icon_LDG
            // 
            this.PictureBox_Icon_LDG.BackgroundImage = global::QSP.Properties.Resources.arrow;
            this.PictureBox_Icon_LDG.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.PictureBox_Icon_LDG.InitialImage = null;
            this.PictureBox_Icon_LDG.Location = new System.Drawing.Point(0, 180);
            this.PictureBox_Icon_LDG.Margin = new System.Windows.Forms.Padding(0);
            this.PictureBox_Icon_LDG.Name = "PictureBox_Icon_LDG";
            this.PictureBox_Icon_LDG.Size = new System.Drawing.Size(20, 30);
            this.PictureBox_Icon_LDG.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.PictureBox_Icon_LDG.TabIndex = 80;
            this.PictureBox_Icon_LDG.TabStop = false;
            // 
            // PictureBox_Icon_Fuel
            // 
            this.PictureBox_Icon_Fuel.BackgroundImage = global::QSP.Properties.Resources.arrow;
            this.PictureBox_Icon_Fuel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.PictureBox_Icon_Fuel.InitialImage = null;
            this.PictureBox_Icon_Fuel.Location = new System.Drawing.Point(0, 0);
            this.PictureBox_Icon_Fuel.Margin = new System.Windows.Forms.Padding(0);
            this.PictureBox_Icon_Fuel.Name = "PictureBox_Icon_Fuel";
            this.PictureBox_Icon_Fuel.Size = new System.Drawing.Size(20, 30);
            this.PictureBox_Icon_Fuel.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.PictureBox_Icon_Fuel.TabIndex = 31;
            this.PictureBox_Icon_Fuel.TabStop = false;
            // 
            // PictureBox_Icon_TO
            // 
            this.PictureBox_Icon_TO.BackgroundImage = global::QSP.Properties.Resources.arrow;
            this.PictureBox_Icon_TO.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.PictureBox_Icon_TO.InitialImage = null;
            this.PictureBox_Icon_TO.Location = new System.Drawing.Point(0, 90);
            this.PictureBox_Icon_TO.Margin = new System.Windows.Forms.Padding(0);
            this.PictureBox_Icon_TO.Name = "PictureBox_Icon_TO";
            this.PictureBox_Icon_TO.Size = new System.Drawing.Size(20, 30);
            this.PictureBox_Icon_TO.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.PictureBox_Icon_TO.TabIndex = 79;
            this.PictureBox_Icon_TO.TabStop = false;
            // 
            // TableLayoutPanel16
            // 
            this.TableLayoutPanel16.ColumnCount = 1;
            this.TableLayoutPanel16.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.TableLayoutPanel16.Controls.Add(this.ShowMisc_Btn, 0, 3);
            this.TableLayoutPanel16.Controls.Add(this.ShowFuel_Btn, 0, 0);
            this.TableLayoutPanel16.Controls.Add(this.ShowLDG_Btn, 0, 2);
            this.TableLayoutPanel16.Controls.Add(this.ShowTO_Btn, 0, 1);
            this.TableLayoutPanel16.Location = new System.Drawing.Point(12, 21);
            this.TableLayoutPanel16.Name = "TableLayoutPanel16";
            this.TableLayoutPanel16.RowCount = 4;
            this.TableLayoutPanel16.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 90F));
            this.TableLayoutPanel16.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 90F));
            this.TableLayoutPanel16.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 90F));
            this.TableLayoutPanel16.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 90F));
            this.TableLayoutPanel16.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.TableLayoutPanel16.Size = new System.Drawing.Size(80, 377);
            this.TableLayoutPanel16.TabIndex = 77;
            // 
            // ShowMisc_Btn
            // 
            this.ShowMisc_Btn.BackColor = System.Drawing.Color.Red;
            this.ShowMisc_Btn.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ShowMisc_Btn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ShowMisc_Btn.Font = new System.Drawing.Font("Arial", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ShowMisc_Btn.ForeColor = System.Drawing.Color.White;
            this.ShowMisc_Btn.Location = new System.Drawing.Point(0, 270);
            this.ShowMisc_Btn.Margin = new System.Windows.Forms.Padding(0);
            this.ShowMisc_Btn.Name = "ShowMisc_Btn";
            this.ShowMisc_Btn.Size = new System.Drawing.Size(80, 80);
            this.ShowMisc_Btn.TabIndex = 35;
            this.ShowMisc_Btn.Text = "Misc";
            this.ShowMisc_Btn.UseVisualStyleBackColor = false;
            this.ShowMisc_Btn.Click += new System.EventHandler(this.ShowAPData_Btn_Click);
            // 
            // ShowFuel_Btn
            // 
            this.ShowFuel_Btn.BackColor = System.Drawing.Color.CornflowerBlue;
            this.ShowFuel_Btn.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ShowFuel_Btn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ShowFuel_Btn.Font = new System.Drawing.Font("Arial", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ShowFuel_Btn.ForeColor = System.Drawing.Color.White;
            this.ShowFuel_Btn.Location = new System.Drawing.Point(0, 0);
            this.ShowFuel_Btn.Margin = new System.Windows.Forms.Padding(0);
            this.ShowFuel_Btn.Name = "ShowFuel_Btn";
            this.ShowFuel_Btn.Size = new System.Drawing.Size(80, 80);
            this.ShowFuel_Btn.TabIndex = 32;
            this.ShowFuel_Btn.Text = "Fuel";
            this.ShowFuel_Btn.UseVisualStyleBackColor = false;
            this.ShowFuel_Btn.Click += new System.EventHandler(this.Button8_Click);
            // 
            // ShowLDG_Btn
            // 
            this.ShowLDG_Btn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.ShowLDG_Btn.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ShowLDG_Btn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ShowLDG_Btn.Font = new System.Drawing.Font("Arial", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ShowLDG_Btn.ForeColor = System.Drawing.Color.White;
            this.ShowLDG_Btn.Location = new System.Drawing.Point(0, 180);
            this.ShowLDG_Btn.Margin = new System.Windows.Forms.Padding(0);
            this.ShowLDG_Btn.Name = "ShowLDG_Btn";
            this.ShowLDG_Btn.Size = new System.Drawing.Size(80, 80);
            this.ShowLDG_Btn.TabIndex = 34;
            this.ShowLDG_Btn.Text = "LDG Perf";
            this.ShowLDG_Btn.UseVisualStyleBackColor = false;
            this.ShowLDG_Btn.Click += new System.EventHandler(this.ShowLDG_Btn_Click);
            // 
            // ShowTO_Btn
            // 
            this.ShowTO_Btn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.ShowTO_Btn.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ShowTO_Btn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ShowTO_Btn.Font = new System.Drawing.Font("Arial", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ShowTO_Btn.ForeColor = System.Drawing.Color.White;
            this.ShowTO_Btn.Location = new System.Drawing.Point(0, 90);
            this.ShowTO_Btn.Margin = new System.Windows.Forms.Padding(0);
            this.ShowTO_Btn.Name = "ShowTO_Btn";
            this.ShowTO_Btn.Size = new System.Drawing.Size(80, 80);
            this.ShowTO_Btn.TabIndex = 33;
            this.ShowTO_Btn.Text = "T/O Perf";
            this.ShowTO_Btn.UseVisualStyleBackColor = false;
            this.ShowTO_Btn.Click += new System.EventHandler(this.ShowTO_Btn_Click);
            // 
            // UpdateAll_Btn
            // 
            this.UpdateAll_Btn.BackgroundImage = global::QSP.Properties.Resources.processing9;
            this.UpdateAll_Btn.Location = new System.Drawing.Point(1043, 22);
            this.UpdateAll_Btn.Name = "UpdateAll_Btn";
            this.UpdateAll_Btn.Size = new System.Drawing.Size(33, 33);
            this.UpdateAll_Btn.TabIndex = 80;
            this.ToolTip1.SetToolTip(this.UpdateAll_Btn, "Refresh all Metar/TAF");
            this.UpdateAll_Btn.UseVisualStyleBackColor = true;
            this.UpdateAll_Btn.Click += new System.EventHandler(this.UpdateAll_Btn_Click);
            // 
            // MainWin_TablessControl
            // 
            this.MainWin_TablessControl.Controls.Add(this.Route_TabPage);
            this.MainWin_TablessControl.Controls.Add(this.TabPage9);
            this.MainWin_TablessControl.Controls.Add(this.TabPage10);
            this.MainWin_TablessControl.Controls.Add(this.TabPage11);
            this.MainWin_TablessControl.Controls.Add(this.TabPage7);
            this.MainWin_TablessControl.Controls.Add(this.TabPage12);
            this.MainWin_TablessControl.Controls.Add(this.TabPage13);
            this.MainWin_TablessControl.Controls.Add(this.TabPage14);
            this.MainWin_TablessControl.Controls.Add(this.TabPage3);
            this.MainWin_TablessControl.Location = new System.Drawing.Point(123, 31);
            this.MainWin_TablessControl.Name = "MainWin_TablessControl";
            this.MainWin_TablessControl.SelectedIndex = 0;
            this.MainWin_TablessControl.Size = new System.Drawing.Size(1127, 780);
            this.MainWin_TablessControl.TabIndex = 77;
            // 
            // Route_TabPage
            // 
            this.Route_TabPage.Controls.Add(this.FuelReportView_Btn);
            this.Route_TabPage.Controls.Add(this.GroupBox6);
            this.Route_TabPage.Controls.Add(this.Calculate_Btn);
            this.Route_TabPage.Controls.Add(this.GroupBox5);
            this.Route_TabPage.Controls.Add(this.GroupBox_BasicInfo);
            this.Route_TabPage.Location = new System.Drawing.Point(4, 25);
            this.Route_TabPage.Name = "Route_TabPage";
            this.Route_TabPage.Padding = new System.Windows.Forms.Padding(3);
            this.Route_TabPage.Size = new System.Drawing.Size(1119, 751);
            this.Route_TabPage.TabIndex = 0;
            this.Route_TabPage.Text = "Route";
            this.Route_TabPage.UseVisualStyleBackColor = true;
            // 
            // FuelReportView_Btn
            // 
            this.FuelReportView_Btn.BackColor = System.Drawing.SystemColors.Control;
            this.FuelReportView_Btn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.FuelReportView_Btn.Font = new System.Drawing.Font("Arial", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FuelReportView_Btn.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.FuelReportView_Btn.Location = new System.Drawing.Point(919, 582);
            this.FuelReportView_Btn.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.FuelReportView_Btn.Name = "FuelReportView_Btn";
            this.FuelReportView_Btn.Size = new System.Drawing.Size(180, 28);
            this.FuelReportView_Btn.TabIndex = 34;
            this.FuelReportView_Btn.Text = "View Fuel Report";
            this.FuelReportView_Btn.UseVisualStyleBackColor = false;
            this.FuelReportView_Btn.Click += new System.EventHandler(this.FuelReportView_Btn_Click);
            // 
            // GroupBox6
            // 
            this.GroupBox6.Controls.Add(this.TableLayoutPanel20);
            this.GroupBox6.Controls.Add(this.TableLayoutPanel1);
            this.GroupBox6.Location = new System.Drawing.Point(8, 339);
            this.GroupBox6.Name = "GroupBox6";
            this.GroupBox6.Size = new System.Drawing.Size(1097, 169);
            this.GroupBox6.TabIndex = 33;
            this.GroupBox6.TabStop = false;
            this.GroupBox6.Text = "Fuel Parameters";
            // 
            // TableLayoutPanel20
            // 
            this.TableLayoutPanel20.ColumnCount = 3;
            this.TableLayoutPanel20.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.TableLayoutPanel20.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 124F));
            this.TableLayoutPanel20.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 73F));
            this.TableLayoutPanel20.Controls.Add(this.Label19, 2, 3);
            this.TableLayoutPanel20.Controls.Add(this.FinalRsv, 1, 3);
            this.TableLayoutPanel20.Controls.Add(this.Label16, 2, 2);
            this.TableLayoutPanel20.Controls.Add(this.Label28, 0, 3);
            this.TableLayoutPanel20.Controls.Add(this.Label15, 2, 1);
            this.TableLayoutPanel20.Controls.Add(this.TaxiTime, 1, 2);
            this.TableLayoutPanel20.Controls.Add(this.Label13, 2, 0);
            this.TableLayoutPanel20.Controls.Add(this.ExtraFuel, 1, 0);
            this.TableLayoutPanel20.Controls.Add(this.APUTime, 1, 1);
            this.TableLayoutPanel20.Controls.Add(this.Label8, 0, 2);
            this.TableLayoutPanel20.Controls.Add(this.Label7, 0, 1);
            this.TableLayoutPanel20.Controls.Add(this.Label6, 0, 0);
            this.TableLayoutPanel20.Location = new System.Drawing.Point(499, 24);
            this.TableLayoutPanel20.Name = "TableLayoutPanel20";
            this.TableLayoutPanel20.RowCount = 4;
            this.TableLayoutPanel20.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.TableLayoutPanel20.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.TableLayoutPanel20.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.TableLayoutPanel20.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.TableLayoutPanel20.Size = new System.Drawing.Size(368, 125);
            this.TableLayoutPanel20.TabIndex = 42;
            // 
            // Label19
            // 
            this.Label19.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.Label19.AutoSize = true;
            this.Label19.Font = new System.Drawing.Font("Arial", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label19.Location = new System.Drawing.Point(300, 98);
            this.Label19.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.Label19.Name = "Label19";
            this.Label19.Size = new System.Drawing.Size(40, 19);
            this.Label19.TabIndex = 21;
            this.Label19.Text = "Min.";
            // 
            // FinalRsv
            // 
            this.FinalRsv.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.FinalRsv.Font = new System.Drawing.Font("Arial", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FinalRsv.Location = new System.Drawing.Point(176, 94);
            this.FinalRsv.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.FinalRsv.Name = "FinalRsv";
            this.FinalRsv.Size = new System.Drawing.Size(89, 27);
            this.FinalRsv.TabIndex = 12;
            // 
            // Label16
            // 
            this.Label16.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.Label16.AutoSize = true;
            this.Label16.Font = new System.Drawing.Font("Arial", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label16.Location = new System.Drawing.Point(300, 65);
            this.Label16.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.Label16.Name = "Label16";
            this.Label16.Size = new System.Drawing.Size(40, 19);
            this.Label16.TabIndex = 23;
            this.Label16.Text = "Min.";
            // 
            // Label28
            // 
            this.Label28.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.Label28.AutoSize = true;
            this.Label28.Font = new System.Drawing.Font("Arial", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label28.Location = new System.Drawing.Point(5, 98);
            this.Label28.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.Label28.Name = "Label28";
            this.Label28.Size = new System.Drawing.Size(108, 19);
            this.Label28.TabIndex = 4;
            this.Label28.Text = "Final Reserve";
            // 
            // Label15
            // 
            this.Label15.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.Label15.AutoSize = true;
            this.Label15.Font = new System.Drawing.Font("Arial", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label15.Location = new System.Drawing.Point(300, 35);
            this.Label15.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.Label15.Name = "Label15";
            this.Label15.Size = new System.Drawing.Size(40, 19);
            this.Label15.TabIndex = 22;
            this.Label15.Text = "Min.";
            // 
            // TaxiTime
            // 
            this.TaxiTime.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.TaxiTime.Font = new System.Drawing.Font("Arial", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TaxiTime.Location = new System.Drawing.Point(176, 64);
            this.TaxiTime.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.TaxiTime.Name = "TaxiTime";
            this.TaxiTime.Size = new System.Drawing.Size(89, 27);
            this.TaxiTime.TabIndex = 15;
            // 
            // Label13
            // 
            this.Label13.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.Label13.AutoSize = true;
            this.Label13.Font = new System.Drawing.Font("Arial", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label13.Location = new System.Drawing.Point(300, 5);
            this.Label13.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.Label13.Name = "Label13";
            this.Label13.Size = new System.Drawing.Size(32, 19);
            this.Label13.TabIndex = 20;
            this.Label13.Text = "KG";
            // 
            // ExtraFuel
            // 
            this.ExtraFuel.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.ExtraFuel.Font = new System.Drawing.Font("Arial", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ExtraFuel.Location = new System.Drawing.Point(176, 4);
            this.ExtraFuel.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.ExtraFuel.Name = "ExtraFuel";
            this.ExtraFuel.Size = new System.Drawing.Size(89, 27);
            this.ExtraFuel.TabIndex = 13;
            // 
            // APUTime
            // 
            this.APUTime.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.APUTime.Font = new System.Drawing.Font("Arial", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.APUTime.Location = new System.Drawing.Point(176, 34);
            this.APUTime.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.APUTime.Name = "APUTime";
            this.APUTime.Size = new System.Drawing.Size(89, 27);
            this.APUTime.TabIndex = 14;
            // 
            // Label8
            // 
            this.Label8.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.Label8.AutoSize = true;
            this.Label8.Font = new System.Drawing.Font("Arial", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label8.Location = new System.Drawing.Point(5, 65);
            this.Label8.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.Label8.Name = "Label8";
            this.Label8.Size = new System.Drawing.Size(36, 19);
            this.Label8.TabIndex = 7;
            this.Label8.Text = "Taxi";
            // 
            // Label7
            // 
            this.Label7.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.Label7.AutoSize = true;
            this.Label7.Font = new System.Drawing.Font("Arial", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label7.Location = new System.Drawing.Point(5, 35);
            this.Label7.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.Label7.Name = "Label7";
            this.Label7.Size = new System.Drawing.Size(42, 19);
            this.Label7.TabIndex = 6;
            this.Label7.Text = "APU";
            // 
            // Label6
            // 
            this.Label6.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.Label6.AutoSize = true;
            this.Label6.Font = new System.Drawing.Font("Arial", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label6.Location = new System.Drawing.Point(5, 5);
            this.Label6.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.Label6.Name = "Label6";
            this.Label6.Size = new System.Drawing.Size(46, 19);
            this.Label6.TabIndex = 5;
            this.Label6.Text = "Extra";
            // 
            // TableLayoutPanel1
            // 
            this.TableLayoutPanel1.ColumnCount = 3;
            this.TableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.TableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 121F));
            this.TableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 80F));
            this.TableLayoutPanel1.Controls.Add(this.Label25, 2, 3);
            this.TableLayoutPanel1.Controls.Add(this.HoldTime_TxtBox, 1, 3);
            this.TableLayoutPanel1.Controls.Add(this.Label11, 2, 1);
            this.TableLayoutPanel1.Controls.Add(this.Label12, 2, 2);
            this.TableLayoutPanel1.Controls.Add(this.WtUnitSel_ComboBox, 1, 0);
            this.TableLayoutPanel1.Controls.Add(this.ContPercentToDest, 1, 2);
            this.TableLayoutPanel1.Controls.Add(this.Label112, 0, 0);
            this.TableLayoutPanel1.Controls.Add(this.MissedAppFuel, 1, 1);
            this.TableLayoutPanel1.Controls.Add(this.Label20, 0, 3);
            this.TableLayoutPanel1.Controls.Add(this.Label4, 0, 2);
            this.TableLayoutPanel1.Controls.Add(this.Label5, 0, 1);
            this.TableLayoutPanel1.Location = new System.Drawing.Point(57, 24);
            this.TableLayoutPanel1.Name = "TableLayoutPanel1";
            this.TableLayoutPanel1.RowCount = 4;
            this.TableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.TableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.TableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.TableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.TableLayoutPanel1.Size = new System.Drawing.Size(368, 125);
            this.TableLayoutPanel1.TabIndex = 30;
            // 
            // Label25
            // 
            this.Label25.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.Label25.AutoSize = true;
            this.Label25.Font = new System.Drawing.Font("Arial", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label25.Location = new System.Drawing.Point(293, 98);
            this.Label25.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.Label25.Name = "Label25";
            this.Label25.Size = new System.Drawing.Size(40, 19);
            this.Label25.TabIndex = 28;
            this.Label25.Text = "Min.";
            // 
            // HoldTime_TxtBox
            // 
            this.HoldTime_TxtBox.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.HoldTime_TxtBox.Font = new System.Drawing.Font("Arial", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.HoldTime_TxtBox.Location = new System.Drawing.Point(172, 94);
            this.HoldTime_TxtBox.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.HoldTime_TxtBox.Name = "HoldTime_TxtBox";
            this.HoldTime_TxtBox.Size = new System.Drawing.Size(89, 27);
            this.HoldTime_TxtBox.TabIndex = 27;
            // 
            // Label11
            // 
            this.Label11.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.Label11.AutoSize = true;
            this.Label11.Font = new System.Drawing.Font("Arial", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label11.Location = new System.Drawing.Point(293, 35);
            this.Label11.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.Label11.Name = "Label11";
            this.Label11.Size = new System.Drawing.Size(32, 19);
            this.Label11.TabIndex = 18;
            this.Label11.Text = "KG";
            // 
            // Label12
            // 
            this.Label12.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.Label12.AutoSize = true;
            this.Label12.Font = new System.Drawing.Font("Arial", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label12.Location = new System.Drawing.Point(293, 65);
            this.Label12.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.Label12.Name = "Label12";
            this.Label12.Size = new System.Drawing.Size(24, 19);
            this.Label12.TabIndex = 19;
            this.Label12.Text = "%";
            // 
            // WtUnitSel_ComboBox
            // 
            this.WtUnitSel_ComboBox.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.WtUnitSel_ComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.WtUnitSel_ComboBox.Font = new System.Drawing.Font("Arial", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.WtUnitSel_ComboBox.FormattingEnabled = true;
            this.WtUnitSel_ComboBox.Items.AddRange(new object[] {
            "KG",
            "LB"});
            this.WtUnitSel_ComboBox.Location = new System.Drawing.Point(170, 3);
            this.WtUnitSel_ComboBox.Name = "WtUnitSel_ComboBox";
            this.WtUnitSel_ComboBox.Size = new System.Drawing.Size(89, 27);
            this.WtUnitSel_ComboBox.TabIndex = 34;
            this.WtUnitSel_ComboBox.SelectedIndexChanged += new System.EventHandler(this.WtUnitSel_ComboBox_SelectedIndexChanged);
            // 
            // ContPercentToDest
            // 
            this.ContPercentToDest.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.ContPercentToDest.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ContPercentToDest.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.ContPercentToDest.Font = new System.Drawing.Font("Arial", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ContPercentToDest.FormattingEnabled = true;
            this.ContPercentToDest.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9",
            "10",
            "11",
            "12",
            "13",
            "14",
            "15",
            "16",
            "17",
            "18",
            "19",
            "20"});
            this.ContPercentToDest.Location = new System.Drawing.Point(172, 64);
            this.ContPercentToDest.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.ContPercentToDest.Name = "ContPercentToDest";
            this.ContPercentToDest.Size = new System.Drawing.Size(89, 27);
            this.ContPercentToDest.TabIndex = 24;
            // 
            // Label112
            // 
            this.Label112.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.Label112.AutoSize = true;
            this.Label112.Font = new System.Drawing.Font("Arial", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label112.Location = new System.Drawing.Point(5, 5);
            this.Label112.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.Label112.Name = "Label112";
            this.Label112.Size = new System.Drawing.Size(94, 19);
            this.Label112.TabIndex = 27;
            this.Label112.Text = "Weight Unit";
            // 
            // MissedAppFuel
            // 
            this.MissedAppFuel.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.MissedAppFuel.Font = new System.Drawing.Font("Arial", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MissedAppFuel.Location = new System.Drawing.Point(172, 34);
            this.MissedAppFuel.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.MissedAppFuel.Name = "MissedAppFuel";
            this.MissedAppFuel.Size = new System.Drawing.Size(89, 27);
            this.MissedAppFuel.TabIndex = 12;
            this.MissedAppFuel.TextChanged += new System.EventHandler(this.MissedAppFuel_TextChanged);
            // 
            // Label20
            // 
            this.Label20.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.Label20.AutoSize = true;
            this.Label20.Font = new System.Drawing.Font("Arial", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label20.Location = new System.Drawing.Point(5, 98);
            this.Label20.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.Label20.Name = "Label20";
            this.Label20.Size = new System.Drawing.Size(63, 19);
            this.Label20.TabIndex = 26;
            this.Label20.Text = "Holding";
            // 
            // Label4
            // 
            this.Label4.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.Label4.AutoSize = true;
            this.Label4.Font = new System.Drawing.Font("Arial", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label4.Location = new System.Drawing.Point(5, 65);
            this.Label4.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.Label4.Name = "Label4";
            this.Label4.Size = new System.Drawing.Size(101, 19);
            this.Label4.TabIndex = 3;
            this.Label4.Text = "Contingency";
            // 
            // Label5
            // 
            this.Label5.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.Label5.AutoSize = true;
            this.Label5.Font = new System.Drawing.Font("Arial", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label5.Location = new System.Drawing.Point(5, 35);
            this.Label5.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.Label5.Name = "Label5";
            this.Label5.Size = new System.Drawing.Size(134, 19);
            this.Label5.TabIndex = 4;
            this.Label5.Text = "Missed approach";
            // 
            // Calculate_Btn
            // 
            this.Calculate_Btn.BackColor = System.Drawing.Color.Green;
            this.Calculate_Btn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Calculate_Btn.Font = new System.Drawing.Font("Arial", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Calculate_Btn.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.Calculate_Btn.Location = new System.Drawing.Point(804, 521);
            this.Calculate_Btn.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.Calculate_Btn.Name = "Calculate_Btn";
            this.Calculate_Btn.Size = new System.Drawing.Size(295, 53);
            this.Calculate_Btn.TabIndex = 27;
            this.Calculate_Btn.Text = "Calculate";
            this.Calculate_Btn.UseVisualStyleBackColor = false;
            this.Calculate_Btn.Click += new System.EventHandler(this.Calculate);
            // 
            // GroupBox5
            // 
            this.GroupBox5.Controls.Add(this.RouteDisAltnLbl);
            this.GroupBox5.Controls.Add(this.RouteDisLbl);
            this.GroupBox5.Controls.Add(this.TableLayoutPanel24);
            this.GroupBox5.Controls.Add(this.Label42);
            this.GroupBox5.Controls.Add(this.TableLayoutPanel21);
            this.GroupBox5.Controls.Add(this.Label41);
            this.GroupBox5.Controls.Add(this.RouteDisplayAltnRichTxtBox);
            this.GroupBox5.Controls.Add(this.Label40);
            this.GroupBox5.Controls.Add(this.RouteDisplayRichTxtBox);
            this.GroupBox5.Font = new System.Drawing.Font("Arial", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.GroupBox5.Location = new System.Drawing.Point(8, 134);
            this.GroupBox5.Name = "GroupBox5";
            this.GroupBox5.Size = new System.Drawing.Size(1097, 204);
            this.GroupBox5.TabIndex = 14;
            this.GroupBox5.TabStop = false;
            this.GroupBox5.Text = "Routes";
            // 
            // RouteDisAltnLbl
            // 
            this.RouteDisAltnLbl.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.RouteDisAltnLbl.AutoSize = true;
            this.RouteDisAltnLbl.Font = new System.Drawing.Font("Arial", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RouteDisAltnLbl.Location = new System.Drawing.Point(844, 160);
            this.RouteDisAltnLbl.Name = "RouteDisAltnLbl";
            this.RouteDisAltnLbl.Size = new System.Drawing.Size(201, 19);
            this.RouteDisAltnLbl.TabIndex = 26;
            this.RouteDisAltnLbl.Text = "Total Dis: 72 NM (+10.3%)";
            // 
            // RouteDisLbl
            // 
            this.RouteDisLbl.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.RouteDisLbl.AutoSize = true;
            this.RouteDisLbl.Font = new System.Drawing.Font("Arial", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RouteDisLbl.Location = new System.Drawing.Point(844, 68);
            this.RouteDisLbl.Name = "RouteDisLbl";
            this.RouteDisLbl.Size = new System.Drawing.Size(201, 19);
            this.RouteDisLbl.TabIndex = 16;
            this.RouteDisLbl.Text = "Total Dis: 459 NM (+3.5%)";
            // 
            // TableLayoutPanel24
            // 
            this.TableLayoutPanel24.ColumnCount = 4;
            this.TableLayoutPanel24.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 150F));
            this.TableLayoutPanel24.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 150F));
            this.TableLayoutPanel24.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 150F));
            this.TableLayoutPanel24.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 150F));
            this.TableLayoutPanel24.Controls.Add(this.ExportBtn, 3, 0);
            this.TableLayoutPanel24.Controls.Add(this.ShowMap_Btn, 2, 0);
            this.TableLayoutPanel24.Controls.Add(this.FindRouteToDestBtn, 0, 0);
            this.TableLayoutPanel24.Controls.Add(this.AnalyzeRteToDest_Btn, 1, 0);
            this.TableLayoutPanel24.Location = new System.Drawing.Point(220, 12);
            this.TableLayoutPanel24.Name = "TableLayoutPanel24";
            this.TableLayoutPanel24.RowCount = 1;
            this.TableLayoutPanel24.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.TableLayoutPanel24.Size = new System.Drawing.Size(600, 36);
            this.TableLayoutPanel24.TabIndex = 48;
            // 
            // ExportBtn
            // 
            this.ExportBtn.BackColor = System.Drawing.SystemColors.Control;
            this.ExportBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ExportBtn.Font = new System.Drawing.Font("Arial", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ExportBtn.Location = new System.Drawing.Point(453, 3);
            this.ExportBtn.Name = "ExportBtn";
            this.ExportBtn.Size = new System.Drawing.Size(140, 30);
            this.ExportBtn.TabIndex = 45;
            this.ExportBtn.Text = "Export";
            this.ExportBtn.UseVisualStyleBackColor = false;
            this.ExportBtn.Click += new System.EventHandler(this.ExportBtn_Click);
            // 
            // ShowMap_Btn
            // 
            this.ShowMap_Btn.BackColor = System.Drawing.SystemColors.Control;
            this.ShowMap_Btn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ShowMap_Btn.Font = new System.Drawing.Font("Arial", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ShowMap_Btn.Location = new System.Drawing.Point(303, 3);
            this.ShowMap_Btn.Name = "ShowMap_Btn";
            this.ShowMap_Btn.Size = new System.Drawing.Size(140, 30);
            this.ShowMap_Btn.TabIndex = 44;
            this.ShowMap_Btn.Text = "Show Map";
            this.ShowMap_Btn.UseVisualStyleBackColor = false;
            this.ShowMap_Btn.Click += new System.EventHandler(this.ShowMap_Btn_Click);
            // 
            // FindRouteToDestBtn
            // 
            this.FindRouteToDestBtn.BackColor = System.Drawing.SystemColors.Control;
            this.FindRouteToDestBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.FindRouteToDestBtn.Font = new System.Drawing.Font("Arial", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FindRouteToDestBtn.Location = new System.Drawing.Point(3, 3);
            this.FindRouteToDestBtn.Name = "FindRouteToDestBtn";
            this.FindRouteToDestBtn.Size = new System.Drawing.Size(140, 30);
            this.FindRouteToDestBtn.TabIndex = 14;
            this.FindRouteToDestBtn.Text = "Find";
            this.FindRouteToDestBtn.UseVisualStyleBackColor = false;
            this.FindRouteToDestBtn.Click += new System.EventHandler(this.FindRouteToDestBtn_Click);
            // 
            // AnalyzeRteToDest_Btn
            // 
            this.AnalyzeRteToDest_Btn.BackColor = System.Drawing.SystemColors.Control;
            this.AnalyzeRteToDest_Btn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.AnalyzeRteToDest_Btn.Font = new System.Drawing.Font("Arial", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.AnalyzeRteToDest_Btn.Location = new System.Drawing.Point(153, 3);
            this.AnalyzeRteToDest_Btn.Name = "AnalyzeRteToDest_Btn";
            this.AnalyzeRteToDest_Btn.Size = new System.Drawing.Size(140, 30);
            this.AnalyzeRteToDest_Btn.TabIndex = 30;
            this.AnalyzeRteToDest_Btn.Text = "Analyze Route";
            this.AnalyzeRteToDest_Btn.UseVisualStyleBackColor = false;
            this.AnalyzeRteToDest_Btn.Click += new System.EventHandler(this.Analyze_RteToDest_Click);
            // 
            // Label42
            // 
            this.Label42.AutoSize = true;
            this.Label42.Cursor = System.Windows.Forms.Cursors.Hand;
            this.Label42.Font = new System.Drawing.Font("Arial", 10.2F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label42.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.Label42.Location = new System.Drawing.Point(951, 22);
            this.Label42.Name = "Label42";
            this.Label42.Size = new System.Drawing.Size(114, 19);
            this.Label42.TabIndex = 47;
            this.Label42.Text = "Advanced Tool";
            this.Label42.Click += new System.EventHandler(this.Label42_Click);
            // 
            // TableLayoutPanel21
            // 
            this.TableLayoutPanel21.ColumnCount = 3;
            this.TableLayoutPanel21.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 150F));
            this.TableLayoutPanel21.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 150F));
            this.TableLayoutPanel21.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 150F));
            this.TableLayoutPanel21.Controls.Add(this.genRteAltn_Btn, 0, 0);
            this.TableLayoutPanel21.Location = new System.Drawing.Point(220, 106);
            this.TableLayoutPanel21.Name = "TableLayoutPanel21";
            this.TableLayoutPanel21.RowCount = 1;
            this.TableLayoutPanel21.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 36F));
            this.TableLayoutPanel21.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 36F));
            this.TableLayoutPanel21.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 36F));
            this.TableLayoutPanel21.Size = new System.Drawing.Size(450, 36);
            this.TableLayoutPanel21.TabIndex = 46;
            // 
            // genRteAltn_Btn
            // 
            this.genRteAltn_Btn.BackColor = System.Drawing.SystemColors.Control;
            this.genRteAltn_Btn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.genRteAltn_Btn.Font = new System.Drawing.Font("Arial", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.genRteAltn_Btn.Location = new System.Drawing.Point(3, 3);
            this.genRteAltn_Btn.Name = "genRteAltn_Btn";
            this.genRteAltn_Btn.Size = new System.Drawing.Size(139, 29);
            this.genRteAltn_Btn.TabIndex = 28;
            this.genRteAltn_Btn.Text = "Find";
            this.genRteAltn_Btn.UseVisualStyleBackColor = false;
            this.genRteAltn_Btn.Click += new System.EventHandler(this.GenRteAltnBtnClick);
            // 
            // Label41
            // 
            this.Label41.AutoSize = true;
            this.Label41.Font = new System.Drawing.Font("Arial", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label41.Location = new System.Drawing.Point(6, 115);
            this.Label41.Name = "Label41";
            this.Label41.Size = new System.Drawing.Size(110, 19);
            this.Label41.TabIndex = 45;
            this.Label41.Text = "DEST - ALTN";
            // 
            // RouteDisplayAltnRichTxtBox
            // 
            this.RouteDisplayAltnRichTxtBox.Font = new System.Drawing.Font("Arial", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RouteDisplayAltnRichTxtBox.Location = new System.Drawing.Point(6, 144);
            this.RouteDisplayAltnRichTxtBox.Name = "RouteDisplayAltnRichTxtBox";
            this.RouteDisplayAltnRichTxtBox.Size = new System.Drawing.Size(830, 50);
            this.RouteDisplayAltnRichTxtBox.TabIndex = 24;
            this.RouteDisplayAltnRichTxtBox.Text = "";
            // 
            // Label40
            // 
            this.Label40.AutoSize = true;
            this.Label40.Font = new System.Drawing.Font("Arial", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label40.Location = new System.Drawing.Point(6, 20);
            this.Label40.Name = "Label40";
            this.Label40.Size = new System.Drawing.Size(111, 19);
            this.Label40.TabIndex = 44;
            this.Label40.Text = "ORIG - DEST";
            // 
            // RouteDisplayRichTxtBox
            // 
            this.RouteDisplayRichTxtBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.RouteDisplayRichTxtBox.Font = new System.Drawing.Font("Arial", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RouteDisplayRichTxtBox.Location = new System.Drawing.Point(6, 52);
            this.RouteDisplayRichTxtBox.Name = "RouteDisplayRichTxtBox";
            this.RouteDisplayRichTxtBox.Size = new System.Drawing.Size(830, 50);
            this.RouteDisplayRichTxtBox.TabIndex = 12;
            this.RouteDisplayRichTxtBox.Text = "";
            // 
            // GroupBox_BasicInfo
            // 
            this.GroupBox_BasicInfo.BackColor = System.Drawing.Color.Transparent;
            this.GroupBox_BasicInfo.Controls.Add(this.Label34);
            this.GroupBox_BasicInfo.Controls.Add(this.ZFW);
            this.GroupBox_BasicInfo.Controls.Add(this.ACList);
            this.GroupBox_BasicInfo.Controls.Add(this.WtManage_Btn);
            this.GroupBox_BasicInfo.Controls.Add(this.TableLayoutPanel8);
            this.GroupBox_BasicInfo.Controls.Add(this.TableLayoutPanel4);
            this.GroupBox_BasicInfo.Font = new System.Drawing.Font("Arial", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.GroupBox_BasicInfo.Location = new System.Drawing.Point(8, 6);
            this.GroupBox_BasicInfo.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.GroupBox_BasicInfo.Name = "GroupBox_BasicInfo";
            this.GroupBox_BasicInfo.Padding = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.GroupBox_BasicInfo.Size = new System.Drawing.Size(1091, 120);
            this.GroupBox_BasicInfo.TabIndex = 32;
            this.GroupBox_BasicInfo.TabStop = false;
            this.GroupBox_BasicInfo.Text = "Basic Info";
            // 
            // Label34
            // 
            this.Label34.AutoSize = true;
            this.Label34.Font = new System.Drawing.Font("Arial", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label34.Location = new System.Drawing.Point(241, 56);
            this.Label34.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.Label34.Name = "Label34";
            this.Label34.Size = new System.Drawing.Size(32, 19);
            this.Label34.TabIndex = 28;
            this.Label34.Text = "KG";
            // 
            // ZFW
            // 
            this.ZFW.Font = new System.Drawing.Font("Arial", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ZFW.ForeColor = System.Drawing.Color.Green;
            this.ZFW.Location = new System.Drawing.Point(120, 52);
            this.ZFW.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.ZFW.Name = "ZFW";
            this.ZFW.Size = new System.Drawing.Size(105, 27);
            this.ZFW.TabIndex = 28;
            this.ZFW.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.ZFW.TextChanged += new System.EventHandler(this.ZFW_TextChanged);
            // 
            // ACList
            // 
            this.ACList.BackColor = System.Drawing.SystemColors.Control;
            this.ACList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ACList.Font = new System.Drawing.Font("Arial", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ACList.FormattingEnabled = true;
            this.ACList.Items.AddRange(new object[] {
            "737-600",
            "737-700",
            "737-800",
            "737-900",
            "777-200LR",
            "777F"});
            this.ACList.Location = new System.Drawing.Point(120, 20);
            this.ACList.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.ACList.Name = "ACList";
            this.ACList.Size = new System.Drawing.Size(162, 27);
            this.ACList.TabIndex = 30;
            this.ACList.SelectedIndexChanged += new System.EventHandler(this.AC_list_SelectedIndexChanged);
            // 
            // WtManage_Btn
            // 
            this.WtManage_Btn.BackColor = System.Drawing.SystemColors.Control;
            this.WtManage_Btn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.WtManage_Btn.Font = new System.Drawing.Font("Arial", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.WtManage_Btn.ForeColor = System.Drawing.SystemColors.ControlText;
            this.WtManage_Btn.Location = new System.Drawing.Point(120, 83);
            this.WtManage_Btn.Name = "WtManage_Btn";
            this.WtManage_Btn.Size = new System.Drawing.Size(162, 30);
            this.WtManage_Btn.TabIndex = 72;
            this.WtManage_Btn.Text = "Advanced";
            this.WtManage_Btn.UseVisualStyleBackColor = false;
            this.WtManage_Btn.Click += new System.EventHandler(this.WtManage_Btn_Click);
            // 
            // TableLayoutPanel8
            // 
            this.TableLayoutPanel8.ColumnCount = 7;
            this.TableLayoutPanel8.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.TableLayoutPanel8.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 81F));
            this.TableLayoutPanel8.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 71F));
            this.TableLayoutPanel8.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 73F));
            this.TableLayoutPanel8.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 92F));
            this.TableLayoutPanel8.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 80F));
            this.TableLayoutPanel8.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 194F));
            this.TableLayoutPanel8.Controls.Add(this.Label111, 5, 2);
            this.TableLayoutPanel8.Controls.Add(this.Label110, 5, 1);
            this.TableLayoutPanel8.Controls.Add(this.Label38, 3, 2);
            this.TableLayoutPanel8.Controls.Add(this.AltnStarComboBox, 6, 2);
            this.TableLayoutPanel8.Controls.Add(this.Label36, 3, 1);
            this.TableLayoutPanel8.Controls.Add(this.DestStarComboBox, 6, 1);
            this.TableLayoutPanel8.Controls.Add(this.OrigSidComboBox, 6, 0);
            this.TableLayoutPanel8.Controls.Add(this.Label109, 5, 0);
            this.TableLayoutPanel8.Controls.Add(this.Label18, 3, 0);
            this.TableLayoutPanel8.Controls.Add(this.AltnTxtBox, 1, 2);
            this.TableLayoutPanel8.Controls.Add(this.AltnRwyComboBox, 4, 2);
            this.TableLayoutPanel8.Controls.Add(this.OrigRwyComboBox, 4, 0);
            this.TableLayoutPanel8.Controls.Add(this.DestRwyComboBox, 4, 1);
            this.TableLayoutPanel8.Controls.Add(this.DestTxtBox, 1, 1);
            this.TableLayoutPanel8.Controls.Add(this.OrigTxtBox, 1, 0);
            this.TableLayoutPanel8.Controls.Add(this.Label26, 0, 0);
            this.TableLayoutPanel8.Controls.Add(this.FindAltn_Btn, 2, 2);
            this.TableLayoutPanel8.Controls.Add(this.Label27, 0, 1);
            this.TableLayoutPanel8.Controls.Add(this.Label33, 0, 2);
            this.TableLayoutPanel8.Location = new System.Drawing.Point(316, 12);
            this.TableLayoutPanel8.Name = "TableLayoutPanel8";
            this.TableLayoutPanel8.RowCount = 3;
            this.TableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 33F));
            this.TableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 33F));
            this.TableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 33F));
            this.TableLayoutPanel8.Size = new System.Drawing.Size(670, 99);
            this.TableLayoutPanel8.TabIndex = 45;
            // 
            // Label111
            // 
            this.Label111.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.Label111.AutoSize = true;
            this.Label111.Font = new System.Drawing.Font("Arial", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label111.Location = new System.Drawing.Point(401, 73);
            this.Label111.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.Label111.Name = "Label111";
            this.Label111.Size = new System.Drawing.Size(50, 19);
            this.Label111.TabIndex = 37;
            this.Label111.Text = "STAR";
            // 
            // Label110
            // 
            this.Label110.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.Label110.AutoSize = true;
            this.Label110.Font = new System.Drawing.Font("Arial", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label110.Location = new System.Drawing.Point(401, 40);
            this.Label110.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.Label110.Name = "Label110";
            this.Label110.Size = new System.Drawing.Size(50, 19);
            this.Label110.TabIndex = 36;
            this.Label110.Text = "STAR";
            // 
            // Label38
            // 
            this.Label38.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.Label38.AutoSize = true;
            this.Label38.Font = new System.Drawing.Font("Arial", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label38.Location = new System.Drawing.Point(236, 73);
            this.Label38.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.Label38.Name = "Label38";
            this.Label38.Size = new System.Drawing.Size(48, 19);
            this.Label38.TabIndex = 37;
            this.Label38.Text = "RWY";
            // 
            // AltnStarComboBox
            // 
            this.AltnStarComboBox.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.AltnStarComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.AltnStarComboBox.Font = new System.Drawing.Font("Arial", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.AltnStarComboBox.FormattingEnabled = true;
            this.AltnStarComboBox.Location = new System.Drawing.Point(480, 71);
            this.AltnStarComboBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.AltnStarComboBox.Name = "AltnStarComboBox";
            this.AltnStarComboBox.Size = new System.Drawing.Size(184, 27);
            this.AltnStarComboBox.TabIndex = 27;
            // 
            // Label36
            // 
            this.Label36.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.Label36.AutoSize = true;
            this.Label36.Font = new System.Drawing.Font("Arial", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label36.Location = new System.Drawing.Point(236, 40);
            this.Label36.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.Label36.Name = "Label36";
            this.Label36.Size = new System.Drawing.Size(48, 19);
            this.Label36.TabIndex = 36;
            this.Label36.Text = "RWY";
            // 
            // DestStarComboBox
            // 
            this.DestStarComboBox.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.DestStarComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.DestStarComboBox.Font = new System.Drawing.Font("Arial", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DestStarComboBox.FormattingEnabled = true;
            this.DestStarComboBox.Location = new System.Drawing.Point(480, 38);
            this.DestStarComboBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.DestStarComboBox.Name = "DestStarComboBox";
            this.DestStarComboBox.Size = new System.Drawing.Size(184, 27);
            this.DestStarComboBox.TabIndex = 13;
            // 
            // OrigSidComboBox
            // 
            this.OrigSidComboBox.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.OrigSidComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.OrigSidComboBox.Font = new System.Drawing.Font("Arial", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.OrigSidComboBox.FormattingEnabled = true;
            this.OrigSidComboBox.Location = new System.Drawing.Point(480, 5);
            this.OrigSidComboBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.OrigSidComboBox.Name = "OrigSidComboBox";
            this.OrigSidComboBox.Size = new System.Drawing.Size(184, 27);
            this.OrigSidComboBox.TabIndex = 12;
            // 
            // Label109
            // 
            this.Label109.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.Label109.AutoSize = true;
            this.Label109.Font = new System.Drawing.Font("Arial", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label109.Location = new System.Drawing.Point(401, 7);
            this.Label109.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.Label109.Name = "Label109";
            this.Label109.Size = new System.Drawing.Size(37, 19);
            this.Label109.TabIndex = 35;
            this.Label109.Text = "SID";
            // 
            // Label18
            // 
            this.Label18.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.Label18.AutoSize = true;
            this.Label18.Font = new System.Drawing.Font("Arial", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label18.Location = new System.Drawing.Point(236, 7);
            this.Label18.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.Label18.Name = "Label18";
            this.Label18.Size = new System.Drawing.Size(48, 19);
            this.Label18.TabIndex = 35;
            this.Label18.Text = "RWY";
            // 
            // AltnTxtBox
            // 
            this.AltnTxtBox.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.AltnTxtBox.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.AltnTxtBox.Font = new System.Drawing.Font("Arial", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.AltnTxtBox.Location = new System.Drawing.Point(83, 70);
            this.AltnTxtBox.Margin = new System.Windows.Forms.Padding(4);
            this.AltnTxtBox.Name = "AltnTxtBox";
            this.AltnTxtBox.Size = new System.Drawing.Size(73, 27);
            this.AltnTxtBox.TabIndex = 54;
            this.AltnTxtBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.AltnTxtBox.TextChanged += new System.EventHandler(this.AltnIcao_TextChanged);
            // 
            // AltnRwyComboBox
            // 
            this.AltnRwyComboBox.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.AltnRwyComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.AltnRwyComboBox.Font = new System.Drawing.Font("Arial", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.AltnRwyComboBox.FormattingEnabled = true;
            this.AltnRwyComboBox.Location = new System.Drawing.Point(308, 71);
            this.AltnRwyComboBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.AltnRwyComboBox.Name = "AltnRwyComboBox";
            this.AltnRwyComboBox.Size = new System.Drawing.Size(84, 27);
            this.AltnRwyComboBox.TabIndex = 26;
            this.AltnRwyComboBox.SelectedIndexChanged += new System.EventHandler(this.AltnRwyComboBox_SelectedIndexChanged);
            // 
            // OrigRwyComboBox
            // 
            this.OrigRwyComboBox.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.OrigRwyComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.OrigRwyComboBox.Font = new System.Drawing.Font("Arial", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.OrigRwyComboBox.FormattingEnabled = true;
            this.OrigRwyComboBox.Location = new System.Drawing.Point(308, 5);
            this.OrigRwyComboBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.OrigRwyComboBox.Name = "OrigRwyComboBox";
            this.OrigRwyComboBox.Size = new System.Drawing.Size(84, 27);
            this.OrigRwyComboBox.TabIndex = 10;
            this.OrigRwyComboBox.SelectedIndexChanged += new System.EventHandler(this.OrigRwyComboBox_SelectedIndexChanged);
            // 
            // DestRwyComboBox
            // 
            this.DestRwyComboBox.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.DestRwyComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.DestRwyComboBox.Font = new System.Drawing.Font("Arial", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DestRwyComboBox.FormattingEnabled = true;
            this.DestRwyComboBox.Location = new System.Drawing.Point(308, 38);
            this.DestRwyComboBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.DestRwyComboBox.Name = "DestRwyComboBox";
            this.DestRwyComboBox.Size = new System.Drawing.Size(84, 27);
            this.DestRwyComboBox.TabIndex = 11;
            this.DestRwyComboBox.SelectedIndexChanged += new System.EventHandler(this.DestRwyComboBox_SelectedIndexChanged);
            // 
            // DestTxtBox
            // 
            this.DestTxtBox.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.DestTxtBox.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.DestTxtBox.Font = new System.Drawing.Font("Arial", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DestTxtBox.Location = new System.Drawing.Point(83, 37);
            this.DestTxtBox.Margin = new System.Windows.Forms.Padding(4);
            this.DestTxtBox.Name = "DestTxtBox";
            this.DestTxtBox.Size = new System.Drawing.Size(73, 27);
            this.DestTxtBox.TabIndex = 53;
            this.DestTxtBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.DestTxtBox.TextChanged += new System.EventHandler(this.DestIcao_TextChanged);
            // 
            // OrigTxtBox
            // 
            this.OrigTxtBox.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.OrigTxtBox.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.OrigTxtBox.Font = new System.Drawing.Font("Arial", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.OrigTxtBox.Location = new System.Drawing.Point(83, 4);
            this.OrigTxtBox.Margin = new System.Windows.Forms.Padding(4);
            this.OrigTxtBox.Name = "OrigTxtBox";
            this.OrigTxtBox.Size = new System.Drawing.Size(73, 27);
            this.OrigTxtBox.TabIndex = 52;
            this.OrigTxtBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.OrigTxtBox.TextChanged += new System.EventHandler(this.OrigIcao_TextChanged);
            // 
            // Label26
            // 
            this.Label26.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.Label26.AutoSize = true;
            this.Label26.Font = new System.Drawing.Font("Arial", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label26.Location = new System.Drawing.Point(4, 7);
            this.Label26.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.Label26.Name = "Label26";
            this.Label26.Size = new System.Drawing.Size(49, 19);
            this.Label26.TabIndex = 55;
            this.Label26.Text = "ORIG";
            // 
            // FindAltn_Btn
            // 
            this.FindAltn_Btn.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.FindAltn_Btn.BackColor = System.Drawing.SystemColors.Control;
            this.FindAltn_Btn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.FindAltn_Btn.Font = new System.Drawing.Font("Arial", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FindAltn_Btn.ForeColor = System.Drawing.SystemColors.ControlText;
            this.FindAltn_Btn.Location = new System.Drawing.Point(163, 69);
            this.FindAltn_Btn.Name = "FindAltn_Btn";
            this.FindAltn_Btn.Size = new System.Drawing.Size(63, 27);
            this.FindAltn_Btn.TabIndex = 73;
            this.FindAltn_Btn.Text = "Find";
            this.FindAltn_Btn.UseVisualStyleBackColor = false;
            this.FindAltn_Btn.Click += new System.EventHandler(this.FindAltn_Btn_Click);
            // 
            // Label27
            // 
            this.Label27.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.Label27.AutoSize = true;
            this.Label27.Font = new System.Drawing.Font("Arial", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label27.Location = new System.Drawing.Point(4, 40);
            this.Label27.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.Label27.Name = "Label27";
            this.Label27.Size = new System.Drawing.Size(52, 19);
            this.Label27.TabIndex = 56;
            this.Label27.Text = "DEST";
            // 
            // Label33
            // 
            this.Label33.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.Label33.AutoSize = true;
            this.Label33.Font = new System.Drawing.Font("Arial", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label33.Location = new System.Drawing.Point(4, 73);
            this.Label33.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.Label33.Name = "Label33";
            this.Label33.Size = new System.Drawing.Size(48, 19);
            this.Label33.TabIndex = 57;
            this.Label33.Text = "ALTN";
            // 
            // TableLayoutPanel4
            // 
            this.TableLayoutPanel4.ColumnCount = 1;
            this.TableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.TableLayoutPanel4.Controls.Add(this.Label_AC, 0, 0);
            this.TableLayoutPanel4.Controls.Add(this.Label17, 0, 1);
            this.TableLayoutPanel4.Location = new System.Drawing.Point(6, 24);
            this.TableLayoutPanel4.Name = "TableLayoutPanel4";
            this.TableLayoutPanel4.RowCount = 2;
            this.TableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.TableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.TableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.TableLayoutPanel4.Size = new System.Drawing.Size(100, 60);
            this.TableLayoutPanel4.TabIndex = 41;
            // 
            // Label_AC
            // 
            this.Label_AC.AutoSize = true;
            this.Label_AC.Font = new System.Drawing.Font("Arial", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label_AC.Location = new System.Drawing.Point(5, 0);
            this.Label_AC.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.Label_AC.Name = "Label_AC";
            this.Label_AC.Size = new System.Drawing.Size(63, 19);
            this.Label_AC.TabIndex = 34;
            this.Label_AC.Text = "Aircraft";
            // 
            // Label17
            // 
            this.Label17.AutoSize = true;
            this.Label17.Font = new System.Drawing.Font("Arial", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label17.Location = new System.Drawing.Point(5, 30);
            this.Label17.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.Label17.Name = "Label17";
            this.Label17.Size = new System.Drawing.Size(45, 19);
            this.Label17.TabIndex = 34;
            this.Label17.Text = "ZFW";
            // 
            // TabPage9
            // 
            this.TabPage9.Controls.Add(this.PanelTO);
            this.TabPage9.Location = new System.Drawing.Point(4, 25);
            this.TabPage9.Name = "TabPage9";
            this.TabPage9.Padding = new System.Windows.Forms.Padding(3);
            this.TabPage9.Size = new System.Drawing.Size(1119, 751);
            this.TabPage9.TabIndex = 2;
            this.TabPage9.Text = "TO Perf";
            this.TabPage9.UseVisualStyleBackColor = true;
            // 
            // PanelTO
            // 
            this.PanelTO.Controls.Add(this.toPerfControl);
            this.PanelTO.Location = new System.Drawing.Point(0, 0);
            this.PanelTO.Name = "PanelTO";
            this.PanelTO.Size = new System.Drawing.Size(1107, 779);
            this.PanelTO.TabIndex = 74;
            // 
            // toPerfControl
            // 
            this.toPerfControl.Airports = null;
            this.toPerfControl.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.toPerfControl.Location = new System.Drawing.Point(0, 0);
            this.toPerfControl.Name = "toPerfControl";
            this.toPerfControl.Size = new System.Drawing.Size(1024, 676);
            this.toPerfControl.TabIndex = 0;
            // 
            // TabPage10
            // 
            this.TabPage10.Controls.Add(this.landingPerfControl);
            this.TabPage10.Location = new System.Drawing.Point(4, 25);
            this.TabPage10.Name = "TabPage10";
            this.TabPage10.Padding = new System.Windows.Forms.Padding(3);
            this.TabPage10.Size = new System.Drawing.Size(1119, 751);
            this.TabPage10.TabIndex = 3;
            this.TabPage10.Text = "LDG Perf";
            this.TabPage10.UseVisualStyleBackColor = true;
            // 
            // landingPerfControl
            // 
            this.landingPerfControl.Airports = null;
            this.landingPerfControl.BackColor = System.Drawing.Color.White;
            this.landingPerfControl.Location = new System.Drawing.Point(0, 0);
            this.landingPerfControl.Name = "landingPerfControl";
            this.landingPerfControl.Size = new System.Drawing.Size(1024, 683);
            this.landingPerfControl.TabIndex = 0;
            // 
            // TabPage11
            // 
            this.TabPage11.Controls.Add(this.PanelAirportData);
            this.TabPage11.Location = new System.Drawing.Point(4, 25);
            this.TabPage11.Name = "TabPage11";
            this.TabPage11.Padding = new System.Windows.Forms.Padding(3);
            this.TabPage11.Size = new System.Drawing.Size(1119, 751);
            this.TabPage11.TabIndex = 4;
            this.TabPage11.Text = "Misc";
            this.TabPage11.UseVisualStyleBackColor = true;
            // 
            // PanelAirportData
            // 
            this.PanelAirportData.Controls.Add(this.TabControl1);
            this.PanelAirportData.Location = new System.Drawing.Point(0, 0);
            this.PanelAirportData.Name = "PanelAirportData";
            this.PanelAirportData.Size = new System.Drawing.Size(1114, 785);
            this.PanelAirportData.TabIndex = 76;
            // 
            // TabControl1
            // 
            this.TabControl1.Controls.Add(this.TabPage1);
            this.TabControl1.Controls.Add(this.TabPage2);
            this.TabControl1.Controls.Add(this.TabPage6);
            this.TabControl1.Location = new System.Drawing.Point(3, 4);
            this.TabControl1.Name = "TabControl1";
            this.TabControl1.SelectedIndex = 0;
            this.TabControl1.Size = new System.Drawing.Size(1099, 778);
            this.TabControl1.TabIndex = 43;
            this.TabControl1.SelectedIndexChanged += new System.EventHandler(this.Refresh_TabControl);
            // 
            // TabPage1
            // 
            this.TabPage1.Controls.Add(this.airportMapControl);
            this.TabPage1.Location = new System.Drawing.Point(4, 25);
            this.TabPage1.Name = "TabPage1";
            this.TabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.TabPage1.Size = new System.Drawing.Size(1091, 749);
            this.TabPage1.TabIndex = 0;
            this.TabPage1.Text = "Airport Data";
            this.TabPage1.UseVisualStyleBackColor = true;
            // 
            // airportMapControl
            // 
            this.airportMapControl.AirportList = null;
            this.airportMapControl.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.airportMapControl.BrowserEnabled = false;
            this.airportMapControl.Location = new System.Drawing.Point(6, 6);
            this.airportMapControl.Name = "airportMapControl";
            this.airportMapControl.Size = new System.Drawing.Size(1024, 670);
            this.airportMapControl.StaticMapEnabled = false;
            this.airportMapControl.TabIndex = 0;
            // 
            // TabPage2
            // 
            this.TabPage2.Controls.Add(this.GroupBox18);
            this.TabPage2.Controls.Add(this.GroupBox17);
            this.TabPage2.Location = new System.Drawing.Point(4, 25);
            this.TabPage2.Name = "TabPage2";
            this.TabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.TabPage2.Size = new System.Drawing.Size(1091, 749);
            this.TabPage2.TabIndex = 1;
            this.TabPage2.Text = "METAR/TAF";
            this.TabPage2.UseVisualStyleBackColor = true;
            // 
            // GroupBox18
            // 
            this.GroupBox18.Controls.Add(this.UpdateAll_Btn);
            this.GroupBox18.Controls.Add(this.RichTextBox2);
            this.GroupBox18.Location = new System.Drawing.Point(3, 234);
            this.GroupBox18.Name = "GroupBox18";
            this.GroupBox18.Size = new System.Drawing.Size(1082, 509);
            this.GroupBox18.TabIndex = 80;
            this.GroupBox18.TabStop = false;
            this.GroupBox18.Text = "Monitor";
            // 
            // RichTextBox2
            // 
            this.RichTextBox2.Font = new System.Drawing.Font("Courier New", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RichTextBox2.Location = new System.Drawing.Point(10, 22);
            this.RichTextBox2.Name = "RichTextBox2";
            this.RichTextBox2.Size = new System.Drawing.Size(1066, 481);
            this.RichTextBox2.TabIndex = 79;
            this.RichTextBox2.Text = "";
            // 
            // GroupBox17
            // 
            this.GroupBox17.Controls.Add(this.MetarToFindTxtBox);
            this.GroupBox17.Controls.Add(this.DownloadMetar_Btn);
            this.GroupBox17.Controls.Add(this.RichTextBox1);
            this.GroupBox17.Location = new System.Drawing.Point(3, 3);
            this.GroupBox17.Name = "GroupBox17";
            this.GroupBox17.Size = new System.Drawing.Size(1082, 229);
            this.GroupBox17.TabIndex = 79;
            this.GroupBox17.TabStop = false;
            this.GroupBox17.Text = "Search";
            // 
            // MetarToFindTxtBox
            // 
            this.MetarToFindTxtBox.Font = new System.Drawing.Font("Arial", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MetarToFindTxtBox.Location = new System.Drawing.Point(10, 21);
            this.MetarToFindTxtBox.Name = "MetarToFindTxtBox";
            this.MetarToFindTxtBox.Size = new System.Drawing.Size(100, 27);
            this.MetarToFindTxtBox.TabIndex = 77;
            // 
            // DownloadMetar_Btn
            // 
            this.DownloadMetar_Btn.BackColor = System.Drawing.SystemColors.Control;
            this.DownloadMetar_Btn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.DownloadMetar_Btn.Font = new System.Drawing.Font("Arial", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DownloadMetar_Btn.Location = new System.Drawing.Point(144, 20);
            this.DownloadMetar_Btn.Name = "DownloadMetar_Btn";
            this.DownloadMetar_Btn.Size = new System.Drawing.Size(226, 27);
            this.DownloadMetar_Btn.TabIndex = 78;
            this.DownloadMetar_Btn.Text = "Download METAR/TAF";
            this.DownloadMetar_Btn.UseVisualStyleBackColor = false;
            this.DownloadMetar_Btn.Click += new System.EventHandler(this.DownloadMetar_Btn_Click);
            // 
            // RichTextBox1
            // 
            this.RichTextBox1.Font = new System.Drawing.Font("Courier New", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RichTextBox1.Location = new System.Drawing.Point(10, 64);
            this.RichTextBox1.Name = "RichTextBox1";
            this.RichTextBox1.Size = new System.Drawing.Size(1066, 156);
            this.RichTextBox1.TabIndex = 0;
            this.RichTextBox1.Text = "";
            // 
            // TabPage6
            // 
            this.TabPage6.Controls.Add(this.GroupBox19);
            this.TabPage6.Location = new System.Drawing.Point(4, 25);
            this.TabPage6.Name = "TabPage6";
            this.TabPage6.Padding = new System.Windows.Forms.Padding(3);
            this.TabPage6.Size = new System.Drawing.Size(1091, 749);
            this.TabPage6.TabIndex = 2;
            this.TabPage6.Text = "Upper Wind";
            this.TabPage6.UseVisualStyleBackColor = true;
            // 
            // GroupBox19
            // 
            this.GroupBox19.Controls.Add(this.DesForcast_RTextBox);
            this.GroupBox19.Controls.Add(this.Label86);
            this.GroupBox19.Location = new System.Drawing.Point(12, 12);
            this.GroupBox19.Name = "GroupBox19";
            this.GroupBox19.Size = new System.Drawing.Size(525, 369);
            this.GroupBox19.TabIndex = 0;
            this.GroupBox19.TabStop = false;
            this.GroupBox19.Text = "Descend Forcast";
            // 
            // DesForcast_RTextBox
            // 
            this.DesForcast_RTextBox.Font = new System.Drawing.Font("Courier New", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DesForcast_RTextBox.Location = new System.Drawing.Point(27, 63);
            this.DesForcast_RTextBox.Name = "DesForcast_RTextBox";
            this.DesForcast_RTextBox.Size = new System.Drawing.Size(486, 287);
            this.DesForcast_RTextBox.TabIndex = 1;
            this.DesForcast_RTextBox.Text = "";
            // 
            // Label86
            // 
            this.Label86.AutoSize = true;
            this.Label86.Font = new System.Drawing.Font("Arial", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label86.Location = new System.Drawing.Point(25, 31);
            this.Label86.Name = "Label86";
            this.Label86.Size = new System.Drawing.Size(110, 19);
            this.Label86.TabIndex = 0;
            this.Label86.Text = "DEST / RCTP";
            // 
            // TabPage7
            // 
            this.TabPage7.Controls.Add(this.GroupBox7);
            this.TabPage7.Location = new System.Drawing.Point(4, 25);
            this.TabPage7.Name = "TabPage7";
            this.TabPage7.Padding = new System.Windows.Forms.Padding(3);
            this.TabPage7.Size = new System.Drawing.Size(1119, 751);
            this.TabPage7.TabIndex = 5;
            this.TabPage7.Text = "TabPage7";
            this.TabPage7.UseVisualStyleBackColor = true;
            // 
            // GroupBox7
            // 
            this.GroupBox7.BackColor = System.Drawing.Color.Transparent;
            this.GroupBox7.Controls.Add(this.LinkLabel8);
            this.GroupBox7.Controls.Add(this.LinkLabel7);
            this.GroupBox7.Controls.Add(this.LinkLabel6);
            this.GroupBox7.Controls.Add(this.LinkLabel5);
            this.GroupBox7.Controls.Add(this.LinkLabel4);
            this.GroupBox7.Controls.Add(this.LinkLabel3);
            this.GroupBox7.Controls.Add(this.LinkLabel2);
            this.GroupBox7.Controls.Add(this.LinkLabel1);
            this.GroupBox7.Font = new System.Drawing.Font("Arial", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.GroupBox7.Location = new System.Drawing.Point(816, 64);
            this.GroupBox7.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.GroupBox7.Name = "GroupBox7";
            this.GroupBox7.Padding = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.GroupBox7.Size = new System.Drawing.Size(284, 303);
            this.GroupBox7.TabIndex = 34;
            this.GroupBox7.TabStop = false;
            this.GroupBox7.Text = "Related sites";
            // 
            // LinkLabel8
            // 
            this.LinkLabel8.AutoSize = true;
            this.LinkLabel8.Font = new System.Drawing.Font("Arial", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LinkLabel8.Location = new System.Drawing.Point(7, 258);
            this.LinkLabel8.Name = "LinkLabel8";
            this.LinkLabel8.Size = new System.Drawing.Size(185, 19);
            this.LinkLabel8.TabIndex = 71;
            this.LinkLabel8.TabStop = true;
            this.LinkLabel8.Text = "European AIS Database";
            this.LinkLabel8.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LinkLabel8_LinkClicked);
            // 
            // LinkLabel7
            // 
            this.LinkLabel7.AutoSize = true;
            this.LinkLabel7.Font = new System.Drawing.Font("Arial", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LinkLabel7.Location = new System.Drawing.Point(7, 232);
            this.LinkLabel7.Name = "LinkLabel7";
            this.LinkLabel7.Size = new System.Drawing.Size(159, 19);
            this.LinkLabel7.TabIndex = 70;
            this.LinkLabel7.TabStop = true;
            this.LinkLabel7.Text = "FAA Digital Products";
            this.LinkLabel7.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LinkLabel7_LinkClicked);
            // 
            // LinkLabel6
            // 
            this.LinkLabel6.AutoSize = true;
            this.LinkLabel6.Font = new System.Drawing.Font("Arial", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LinkLabel6.Location = new System.Drawing.Point(6, 187);
            this.LinkLabel6.Name = "LinkLabel6";
            this.LinkLabel6.Size = new System.Drawing.Size(195, 38);
            this.LinkLabel6.TabIndex = 69;
            this.LinkLabel6.TabStop = true;
            this.LinkLabel6.Text = "Defense Internet NOTAM \r\nService";
            this.LinkLabel6.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LinkLabel6_LinkClicked);
            // 
            // LinkLabel5
            // 
            this.LinkLabel5.AutoSize = true;
            this.LinkLabel5.Font = new System.Drawing.Font("Arial", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LinkLabel5.Location = new System.Drawing.Point(6, 159);
            this.LinkLabel5.Name = "LinkLabel5";
            this.LinkLabel5.Size = new System.Drawing.Size(211, 19);
            this.LinkLabel5.TabIndex = 68;
            this.LinkLabel5.TabStop = true;
            this.LinkLabel5.Text = "Significant Weather(NOAA)";
            this.LinkLabel5.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LinkLabel5_LinkClicked);
            // 
            // LinkLabel4
            // 
            this.LinkLabel4.AutoSize = true;
            this.LinkLabel4.Font = new System.Drawing.Font("Arial", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LinkLabel4.Location = new System.Drawing.Point(6, 131);
            this.LinkLabel4.Name = "LinkLabel4";
            this.LinkLabel4.Size = new System.Drawing.Size(206, 19);
            this.LinkLabel4.TabIndex = 67;
            this.LinkLabel4.TabStop = true;
            this.LinkLabel4.Text = "Wind/Temp Charts (NOAA)";
            this.LinkLabel4.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LinkLabel4_LinkClicked);
            // 
            // LinkLabel3
            // 
            this.LinkLabel3.AutoSize = true;
            this.LinkLabel3.Font = new System.Drawing.Font("Arial", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LinkLabel3.Location = new System.Drawing.Point(6, 104);
            this.LinkLabel3.Name = "LinkLabel3";
            this.LinkLabel3.Size = new System.Drawing.Size(112, 19);
            this.LinkLabel3.TabIndex = 66;
            this.LinkLabel3.TabStop = true;
            this.LinkLabel3.Text = "Metar (NOAA)";
            this.LinkLabel3.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LinkLabel3_LinkClicked);
            // 
            // LinkLabel2
            // 
            this.LinkLabel2.AutoSize = true;
            this.LinkLabel2.Font = new System.Drawing.Font("Arial", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LinkLabel2.Location = new System.Drawing.Point(6, 76);
            this.LinkLabel2.Name = "LinkLabel2";
            this.LinkLabel2.Size = new System.Drawing.Size(103, 19);
            this.LinkLabel2.TabIndex = 65;
            this.LinkLabel2.TabStop = true;
            this.LinkLabel2.Text = "Route Finder";
            this.LinkLabel2.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LinkLabel2_LinkClicked);
            // 
            // LinkLabel1
            // 
            this.LinkLabel1.AutoSize = true;
            this.LinkLabel1.Font = new System.Drawing.Font("Arial", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LinkLabel1.Location = new System.Drawing.Point(6, 33);
            this.LinkLabel1.Name = "LinkLabel1";
            this.LinkLabel1.Size = new System.Drawing.Size(172, 38);
            this.LinkLabel1.TabIndex = 64;
            this.LinkLabel1.TabStop = true;
            this.LinkLabel1.Text = "FlightAware IFR Route\r\nAnalyzer";
            this.LinkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LinkLabel1_LinkClicked);
            // 
            // TabPage12
            // 
            this.TabPage12.Location = new System.Drawing.Point(4, 25);
            this.TabPage12.Name = "TabPage12";
            this.TabPage12.Padding = new System.Windows.Forms.Padding(3);
            this.TabPage12.Size = new System.Drawing.Size(1119, 751);
            this.TabPage12.TabIndex = 6;
            this.TabPage12.Text = "TabPage12";
            this.TabPage12.UseVisualStyleBackColor = true;
            // 
            // TabPage13
            // 
            this.TabPage13.Controls.Add(this.Return_Btn);
            this.TabPage13.Controls.Add(this.MapDisWebBrowser);
            this.TabPage13.Location = new System.Drawing.Point(4, 25);
            this.TabPage13.Name = "TabPage13";
            this.TabPage13.Padding = new System.Windows.Forms.Padding(3);
            this.TabPage13.Size = new System.Drawing.Size(1119, 751);
            this.TabPage13.TabIndex = 7;
            this.TabPage13.Text = "Map of Route";
            this.TabPage13.UseVisualStyleBackColor = true;
            // 
            // Return_Btn
            // 
            this.Return_Btn.BackColor = System.Drawing.Color.Green;
            this.Return_Btn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Return_Btn.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Return_Btn.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.Return_Btn.Location = new System.Drawing.Point(894, 666);
            this.Return_Btn.Name = "Return_Btn";
            this.Return_Btn.Size = new System.Drawing.Size(200, 40);
            this.Return_Btn.TabIndex = 16;
            this.Return_Btn.Text = "Return";
            this.Return_Btn.UseVisualStyleBackColor = false;
            this.Return_Btn.Click += new System.EventHandler(this.Return_Btn_Click);
            // 
            // MapDisWebBrowser
            // 
            this.MapDisWebBrowser.Location = new System.Drawing.Point(10, 10);
            this.MapDisWebBrowser.MinimumSize = new System.Drawing.Size(20, 20);
            this.MapDisWebBrowser.Name = "MapDisWebBrowser";
            this.MapDisWebBrowser.ScrollBarsEnabled = false;
            this.MapDisWebBrowser.Size = new System.Drawing.Size(1086, 637);
            this.MapDisWebBrowser.TabIndex = 15;
            this.MapDisWebBrowser.Url = new System.Uri("", System.UriKind.Relative);
            // 
            // TabPage14
            // 
            this.TabPage14.Controls.Add(this.Return_AdvancedTool_Btn);
            this.TabPage14.Controls.Add(this.GroupBox8);
            this.TabPage14.Location = new System.Drawing.Point(4, 25);
            this.TabPage14.Name = "TabPage14";
            this.TabPage14.Padding = new System.Windows.Forms.Padding(3);
            this.TabPage14.Size = new System.Drawing.Size(1119, 751);
            this.TabPage14.TabIndex = 8;
            this.TabPage14.Text = "Advanced Tool For Routes";
            this.TabPage14.UseVisualStyleBackColor = true;
            // 
            // Return_AdvancedTool_Btn
            // 
            this.Return_AdvancedTool_Btn.BackColor = System.Drawing.Color.Green;
            this.Return_AdvancedTool_Btn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Return_AdvancedTool_Btn.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Return_AdvancedTool_Btn.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.Return_AdvancedTool_Btn.Location = new System.Drawing.Point(905, 280);
            this.Return_AdvancedTool_Btn.Name = "Return_AdvancedTool_Btn";
            this.Return_AdvancedTool_Btn.Size = new System.Drawing.Size(200, 40);
            this.Return_AdvancedTool_Btn.TabIndex = 17;
            this.Return_AdvancedTool_Btn.Text = "Return";
            this.Return_AdvancedTool_Btn.UseVisualStyleBackColor = false;
            this.Return_AdvancedTool_Btn.Click += new System.EventHandler(this.Return_Btn_Click);
            // 
            // GroupBox8
            // 
            this.GroupBox8.Controls.Add(this.Label56);
            this.GroupBox8.Controls.Add(this.Find_Btn);
            this.GroupBox8.Controls.Add(this.RouteAdvancedRichTxtBox);
            this.GroupBox8.Controls.Add(this.WptSelToCBox);
            this.GroupBox8.Controls.Add(this.WptSelFromCBox);
            this.GroupBox8.Controls.Add(this.RadioButton4);
            this.GroupBox8.Controls.Add(this.RadioButton3);
            this.GroupBox8.Controls.Add(this.RadioButton2);
            this.GroupBox8.Controls.Add(this.RadioButton1);
            this.GroupBox8.Controls.Add(this.Label64);
            this.GroupBox8.Controls.Add(this.ToStarCBox);
            this.GroupBox8.Controls.Add(this.FromSidCBox);
            this.GroupBox8.Controls.Add(this.ToRwyCBox);
            this.GroupBox8.Controls.Add(this.FromRwyCBox);
            this.GroupBox8.Controls.Add(this.Label65);
            this.GroupBox8.Controls.Add(this.FromTxtbox);
            this.GroupBox8.Controls.Add(this.ToTxtbox);
            this.GroupBox8.Controls.Add(this.Label66);
            this.GroupBox8.Font = new System.Drawing.Font("Arial", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.GroupBox8.Location = new System.Drawing.Point(6, 18);
            this.GroupBox8.Name = "GroupBox8";
            this.GroupBox8.Size = new System.Drawing.Size(1099, 225);
            this.GroupBox8.TabIndex = 15;
            this.GroupBox8.TabStop = false;
            this.GroupBox8.Text = "Advanced Tool For Routes";
            // 
            // Label56
            // 
            this.Label56.AutoSize = true;
            this.Label56.Font = new System.Drawing.Font("Arial", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label56.Location = new System.Drawing.Point(871, 171);
            this.Label56.Name = "Label56";
            this.Label56.Size = new System.Drawing.Size(201, 19);
            this.Label56.TabIndex = 45;
            this.Label56.Text = "Total Dis: 459 NM (+3.5%)";
            // 
            // Find_Btn
            // 
            this.Find_Btn.Font = new System.Drawing.Font("Arial", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Find_Btn.Location = new System.Drawing.Point(998, 66);
            this.Find_Btn.Name = "Find_Btn";
            this.Find_Btn.Size = new System.Drawing.Size(89, 59);
            this.Find_Btn.TabIndex = 28;
            this.Find_Btn.Text = "Find Route";
            this.Find_Btn.UseVisualStyleBackColor = true;
            this.Find_Btn.Click += new System.EventHandler(this.Find_Btn_Click);
            // 
            // RouteAdvancedRichTxtBox
            // 
            this.RouteAdvancedRichTxtBox.Font = new System.Drawing.Font("Arial", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RouteAdvancedRichTxtBox.Location = new System.Drawing.Point(25, 144);
            this.RouteAdvancedRichTxtBox.Name = "RouteAdvancedRichTxtBox";
            this.RouteAdvancedRichTxtBox.Size = new System.Drawing.Size(827, 65);
            this.RouteAdvancedRichTxtBox.TabIndex = 34;
            this.RouteAdvancedRichTxtBox.Text = "";
            // 
            // WptSelToCBox
            // 
            this.WptSelToCBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.WptSelToCBox.Font = new System.Drawing.Font("Arial", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.WptSelToCBox.FormattingEnabled = true;
            this.WptSelToCBox.Location = new System.Drawing.Point(595, 98);
            this.WptSelToCBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.WptSelToCBox.Name = "WptSelToCBox";
            this.WptSelToCBox.Size = new System.Drawing.Size(391, 27);
            this.WptSelToCBox.TabIndex = 43;
            // 
            // WptSelFromCBox
            // 
            this.WptSelFromCBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.WptSelFromCBox.Font = new System.Drawing.Font("Arial", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.WptSelFromCBox.FormattingEnabled = true;
            this.WptSelFromCBox.Location = new System.Drawing.Point(595, 64);
            this.WptSelFromCBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.WptSelFromCBox.Name = "WptSelFromCBox";
            this.WptSelFromCBox.Size = new System.Drawing.Size(391, 27);
            this.WptSelFromCBox.TabIndex = 42;
            // 
            // RadioButton4
            // 
            this.RadioButton4.AutoSize = true;
            this.RadioButton4.Location = new System.Drawing.Point(674, 36);
            this.RadioButton4.Name = "RadioButton4";
            this.RadioButton4.Size = new System.Drawing.Size(168, 20);
            this.RadioButton4.TabIndex = 41;
            this.RadioButton4.Text = "Waypoint To Waypoint";
            this.RadioButton4.UseVisualStyleBackColor = true;
            this.RadioButton4.CheckedChanged += new System.EventHandler(this.RadioButton4_CheckedChanged);
            // 
            // RadioButton3
            // 
            this.RadioButton3.AutoSize = true;
            this.RadioButton3.Location = new System.Drawing.Point(497, 36);
            this.RadioButton3.Name = "RadioButton3";
            this.RadioButton3.Size = new System.Drawing.Size(155, 20);
            this.RadioButton3.TabIndex = 40;
            this.RadioButton3.Text = "Waypoint To Airport ";
            this.RadioButton3.UseVisualStyleBackColor = true;
            this.RadioButton3.CheckedChanged += new System.EventHandler(this.RadioButton3_CheckedChanged);
            // 
            // RadioButton2
            // 
            this.RadioButton2.AutoSize = true;
            this.RadioButton2.Location = new System.Drawing.Point(324, 36);
            this.RadioButton2.Name = "RadioButton2";
            this.RadioButton2.Size = new System.Drawing.Size(152, 20);
            this.RadioButton2.TabIndex = 39;
            this.RadioButton2.Text = "Airport To Waypoint";
            this.RadioButton2.UseVisualStyleBackColor = true;
            this.RadioButton2.CheckedChanged += new System.EventHandler(this.RadioButton2_CheckedChanged);
            // 
            // RadioButton1
            // 
            this.RadioButton1.AutoSize = true;
            this.RadioButton1.Checked = true;
            this.RadioButton1.Location = new System.Drawing.Point(157, 36);
            this.RadioButton1.Name = "RadioButton1";
            this.RadioButton1.Size = new System.Drawing.Size(135, 20);
            this.RadioButton1.TabIndex = 38;
            this.RadioButton1.TabStop = true;
            this.RadioButton1.Text = "Airport To Airport";
            this.RadioButton1.UseVisualStyleBackColor = true;
            this.RadioButton1.CheckedChanged += new System.EventHandler(this.RadioButton1_CheckedChanged);
            // 
            // Label64
            // 
            this.Label64.AutoSize = true;
            this.Label64.Font = new System.Drawing.Font("Arial", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label64.Location = new System.Drawing.Point(21, 37);
            this.Label64.Name = "Label64";
            this.Label64.Size = new System.Drawing.Size(113, 19);
            this.Label64.TabIndex = 37;
            this.Label64.Text = "Find route for:";
            // 
            // ToStarCBox
            // 
            this.ToStarCBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ToStarCBox.Font = new System.Drawing.Font("Arial", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ToStarCBox.FormattingEnabled = true;
            this.ToStarCBox.Location = new System.Drawing.Point(392, 98);
            this.ToStarCBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.ToStarCBox.Name = "ToStarCBox";
            this.ToStarCBox.Size = new System.Drawing.Size(184, 27);
            this.ToStarCBox.TabIndex = 36;
            // 
            // FromSidCBox
            // 
            this.FromSidCBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.FromSidCBox.Font = new System.Drawing.Font("Arial", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FromSidCBox.FormattingEnabled = true;
            this.FromSidCBox.Location = new System.Drawing.Point(392, 64);
            this.FromSidCBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.FromSidCBox.Name = "FromSidCBox";
            this.FromSidCBox.Size = new System.Drawing.Size(184, 27);
            this.FromSidCBox.TabIndex = 35;
            // 
            // ToRwyCBox
            // 
            this.ToRwyCBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ToRwyCBox.Font = new System.Drawing.Font("Arial", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ToRwyCBox.FormattingEnabled = true;
            this.ToRwyCBox.Location = new System.Drawing.Point(270, 98);
            this.ToRwyCBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.ToRwyCBox.Name = "ToRwyCBox";
            this.ToRwyCBox.Size = new System.Drawing.Size(98, 27);
            this.ToRwyCBox.TabIndex = 34;
            this.ToRwyCBox.SelectedIndexChanged += new System.EventHandler(this.To_rwy_CBox_SelectedIndexChanged);
            // 
            // FromRwyCBox
            // 
            this.FromRwyCBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.FromRwyCBox.Font = new System.Drawing.Font("Arial", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FromRwyCBox.FormattingEnabled = true;
            this.FromRwyCBox.Location = new System.Drawing.Point(270, 64);
            this.FromRwyCBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.FromRwyCBox.Name = "FromRwyCBox";
            this.FromRwyCBox.Size = new System.Drawing.Size(98, 27);
            this.FromRwyCBox.TabIndex = 33;
            this.FromRwyCBox.SelectedIndexChanged += new System.EventHandler(this.FromRwyCBox_SelectedIndexChanged);
            // 
            // Label65
            // 
            this.Label65.AutoSize = true;
            this.Label65.Font = new System.Drawing.Font("Arial", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label65.Location = new System.Drawing.Point(34, 72);
            this.Label65.Name = "Label65";
            this.Label65.Size = new System.Drawing.Size(55, 19);
            this.Label65.TabIndex = 31;
            this.Label65.Text = "FROM";
            // 
            // FromTxtbox
            // 
            this.FromTxtbox.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.FromTxtbox.Font = new System.Drawing.Font("Arial", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FromTxtbox.Location = new System.Drawing.Point(130, 67);
            this.FromTxtbox.Name = "FromTxtbox";
            this.FromTxtbox.Size = new System.Drawing.Size(123, 27);
            this.FromTxtbox.TabIndex = 29;
            this.FromTxtbox.TextChanged += new System.EventHandler(this.FromTxtbox_TextChanged);
            // 
            // ToTxtbox
            // 
            this.ToTxtbox.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.ToTxtbox.Font = new System.Drawing.Font("Arial", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ToTxtbox.Location = new System.Drawing.Point(130, 96);
            this.ToTxtbox.Name = "ToTxtbox";
            this.ToTxtbox.Size = new System.Drawing.Size(123, 27);
            this.ToTxtbox.TabIndex = 30;
            this.ToTxtbox.TextChanged += new System.EventHandler(this.ToTxtbox_TextChanged);
            // 
            // Label66
            // 
            this.Label66.AutoSize = true;
            this.Label66.Font = new System.Drawing.Font("Arial", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label66.Location = new System.Drawing.Point(34, 101);
            this.Label66.Name = "Label66";
            this.Label66.Size = new System.Drawing.Size(30, 19);
            this.Label66.TabIndex = 32;
            this.Label66.Text = "TO";
            // 
            // TabPage3
            // 
            this.TabPage3.Controls.Add(this.Return2_Btn);
            this.TabPage3.Controls.Add(this.GroupBox3);
            this.TabPage3.Location = new System.Drawing.Point(4, 25);
            this.TabPage3.Name = "TabPage3";
            this.TabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.TabPage3.Size = new System.Drawing.Size(1119, 751);
            this.TabPage3.TabIndex = 9;
            this.TabPage3.Text = "Fuel Report";
            this.TabPage3.UseVisualStyleBackColor = true;
            // 
            // Return2_Btn
            // 
            this.Return2_Btn.BackColor = System.Drawing.Color.Green;
            this.Return2_Btn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Return2_Btn.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Return2_Btn.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.Return2_Btn.Location = new System.Drawing.Point(875, 561);
            this.Return2_Btn.Name = "Return2_Btn";
            this.Return2_Btn.Size = new System.Drawing.Size(200, 40);
            this.Return2_Btn.TabIndex = 29;
            this.Return2_Btn.Text = "Return";
            this.Return2_Btn.UseVisualStyleBackColor = false;
            this.Return2_Btn.Click += new System.EventHandler(this.Return2_Btn_Click);
            // 
            // GroupBox3
            // 
            this.GroupBox3.BackColor = System.Drawing.Color.Transparent;
            this.GroupBox3.Controls.Add(this.FuelReport_TxtBox);
            this.GroupBox3.Font = new System.Drawing.Font("Arial", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.GroupBox3.Location = new System.Drawing.Point(43, 57);
            this.GroupBox3.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.GroupBox3.Name = "GroupBox3";
            this.GroupBox3.Padding = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.GroupBox3.Size = new System.Drawing.Size(1032, 470);
            this.GroupBox3.TabIndex = 28;
            this.GroupBox3.TabStop = false;
            this.GroupBox3.Text = "Fuel Report";
            // 
            // FuelReport_TxtBox
            // 
            this.FuelReport_TxtBox.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.FuelReport_TxtBox.Font = new System.Drawing.Font("Courier New", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FuelReport_TxtBox.Location = new System.Drawing.Point(24, 23);
            this.FuelReport_TxtBox.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.FuelReport_TxtBox.Name = "FuelReport_TxtBox";
            this.FuelReport_TxtBox.ReadOnly = true;
            this.FuelReport_TxtBox.Size = new System.Drawing.Size(982, 422);
            this.FuelReport_TxtBox.TabIndex = 1;
            this.FuelReport_TxtBox.Text = "";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(120F, 120F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.Color.White;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.ClientSize = new System.Drawing.Size(1262, 855);
            this.Controls.Add(this.MainWin_TablessControl);
            this.Controls.Add(this.WindowSelPanel);
            this.Controls.Add(this.StatusStrip1);
            this.Controls.Add(this.MenuStrip1);
            this.MainMenuStrip = this.MenuStrip1;
            this.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.Name = "MainForm";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "QSimPlanner";
            this.MenuStrip1.ResumeLayout(false);
            this.MenuStrip1.PerformLayout();
            this.StatusStrip1.ResumeLayout(false);
            this.StatusStrip1.PerformLayout();
            this.WindowSelPanel.ResumeLayout(false);
            this.TableLayoutPanel17.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.PictureBox_Icon_APData)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PictureBox_Icon_LDG)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PictureBox_Icon_Fuel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PictureBox_Icon_TO)).EndInit();
            this.TableLayoutPanel16.ResumeLayout(false);
            this.MainWin_TablessControl.ResumeLayout(false);
            this.Route_TabPage.ResumeLayout(false);
            this.GroupBox6.ResumeLayout(false);
            this.TableLayoutPanel20.ResumeLayout(false);
            this.TableLayoutPanel20.PerformLayout();
            this.TableLayoutPanel1.ResumeLayout(false);
            this.TableLayoutPanel1.PerformLayout();
            this.GroupBox5.ResumeLayout(false);
            this.GroupBox5.PerformLayout();
            this.TableLayoutPanel24.ResumeLayout(false);
            this.TableLayoutPanel21.ResumeLayout(false);
            this.GroupBox_BasicInfo.ResumeLayout(false);
            this.GroupBox_BasicInfo.PerformLayout();
            this.TableLayoutPanel8.ResumeLayout(false);
            this.TableLayoutPanel8.PerformLayout();
            this.TableLayoutPanel4.ResumeLayout(false);
            this.TableLayoutPanel4.PerformLayout();
            this.TabPage9.ResumeLayout(false);
            this.PanelTO.ResumeLayout(false);
            this.TabPage10.ResumeLayout(false);
            this.TabPage11.ResumeLayout(false);
            this.PanelAirportData.ResumeLayout(false);
            this.TabControl1.ResumeLayout(false);
            this.TabPage1.ResumeLayout(false);
            this.TabPage2.ResumeLayout(false);
            this.GroupBox18.ResumeLayout(false);
            this.GroupBox17.ResumeLayout(false);
            this.GroupBox17.PerformLayout();
            this.TabPage6.ResumeLayout(false);
            this.GroupBox19.ResumeLayout(false);
            this.GroupBox19.PerformLayout();
            this.TabPage7.ResumeLayout(false);
            this.GroupBox7.ResumeLayout(false);
            this.GroupBox7.PerformLayout();
            this.TabPage13.ResumeLayout(false);
            this.TabPage14.ResumeLayout(false);
            this.GroupBox8.ResumeLayout(false);
            this.GroupBox8.PerformLayout();
            this.TabPage3.ResumeLayout(false);
            this.GroupBox3.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        internal System.Windows.Forms.Label Label4;
        internal System.Windows.Forms.Label Label5;
        internal System.Windows.Forms.Label Label6;
        internal System.Windows.Forms.Label Label7;
        internal System.Windows.Forms.Label Label8;
        internal System.Windows.Forms.TextBox MissedAppFuel;
        internal System.Windows.Forms.TextBox ExtraFuel;
        internal System.Windows.Forms.TextBox APUTime;
        internal System.Windows.Forms.TextBox TaxiTime;
        internal System.Windows.Forms.Label Label11;
        internal System.Windows.Forms.Label Label12;
        internal System.Windows.Forms.Label Label13;
        internal System.Windows.Forms.Label Label15;
        internal System.Windows.Forms.Label Label16;
        internal System.Windows.Forms.ComboBox ContPercentToDest;
        internal System.Windows.Forms.Label Label19;
        internal System.Windows.Forms.TextBox FinalRsv;
        internal System.Windows.Forms.Label Label28;
        internal System.Windows.Forms.Button Calculate_Btn;
        internal System.Windows.Forms.GroupBox GroupBox3;
        internal System.Windows.Forms.RichTextBox FuelReport_TxtBox;
        internal System.Windows.Forms.Label Label25;
        internal System.Windows.Forms.TextBox HoldTime_TxtBox;
        internal System.Windows.Forms.Label Label20;
        internal System.Windows.Forms.TextBox ZFW;
        internal System.Windows.Forms.GroupBox GroupBox_BasicInfo;
        internal System.Windows.Forms.Label Label17;
        internal System.Windows.Forms.ComboBox ACList;
        internal System.Windows.Forms.Label Label_AC;
        internal System.Windows.Forms.Label Label34;
        internal System.Windows.Forms.MenuStrip MenuStrip1;
        internal System.Windows.Forms.ToolStripMenuItem MainToolStripMenuItem;
        internal System.Windows.Forms.ToolStripMenuItem ExitToolStripMenuItem;
        internal System.Windows.Forms.ToolStripMenuItem AboutToolStripMenuItem;
        internal System.Windows.Forms.ToolStripMenuItem AboutToolStripMenuItem1;
        internal System.Windows.Forms.GroupBox GroupBox7;
        internal System.Windows.Forms.ToolStripMenuItem OptionsToolStripMenuItem;
        internal System.Windows.Forms.Label Label33;
        internal System.Windows.Forms.Label Label27;
        internal System.Windows.Forms.Label Label26;
        internal System.Windows.Forms.TextBox AltnTxtBox;
        internal System.Windows.Forms.TextBox DestTxtBox;
        internal System.Windows.Forms.TextBox OrigTxtBox;
        internal System.Windows.Forms.StatusStrip StatusStrip1;
        internal System.Windows.Forms.ToolStripStatusLabel StatusLabel1;
        internal System.Windows.Forms.Panel WindowSelPanel;
        internal System.Windows.Forms.Button ShowMisc_Btn;
        internal System.Windows.Forms.PictureBox PictureBox_Icon_Fuel;
        internal System.Windows.Forms.Button ShowLDG_Btn;
        internal System.Windows.Forms.Button ShowTO_Btn;
        internal System.Windows.Forms.Button ShowFuel_Btn;
        internal System.Windows.Forms.LinkLabel LinkLabel8;
        internal System.Windows.Forms.LinkLabel LinkLabel7;
        internal System.Windows.Forms.LinkLabel LinkLabel6;
        internal System.Windows.Forms.LinkLabel LinkLabel5;
        internal System.Windows.Forms.LinkLabel LinkLabel4;
        internal System.Windows.Forms.LinkLabel LinkLabel3;
        internal System.Windows.Forms.LinkLabel LinkLabel2;
        internal System.Windows.Forms.LinkLabel LinkLabel1;
        internal System.Windows.Forms.TableLayoutPanel TableLayoutPanel1;
        internal System.Windows.Forms.TableLayoutPanel TableLayoutPanel4;
        internal System.Windows.Forms.TableLayoutPanel TableLayoutPanel8;
        internal System.Windows.Forms.Button genRteAltn_Btn;
        internal System.Windows.Forms.ComboBox AltnStarComboBox;
        internal System.Windows.Forms.ComboBox AltnRwyComboBox;
        internal System.Windows.Forms.Button FindRouteToDestBtn;
        internal System.Windows.Forms.ComboBox DestStarComboBox;
        internal System.Windows.Forms.ComboBox OrigSidComboBox;
        internal System.Windows.Forms.ComboBox DestRwyComboBox;
        internal System.Windows.Forms.ComboBox OrigRwyComboBox;
        internal System.Windows.Forms.GroupBox GroupBox5;
        internal System.Windows.Forms.Button AnalyzeRteToDest_Btn;
        internal System.Windows.Forms.RichTextBox RouteDisplayRichTxtBox;
        internal System.Windows.Forms.Label RouteDisLbl;
        internal System.Windows.Forms.RichTextBox RouteDisplayAltnRichTxtBox;
        internal System.Windows.Forms.Label RouteDisAltnLbl;
        internal System.Windows.Forms.GroupBox GroupBox8;
        internal System.Windows.Forms.Label Label56;
        internal System.Windows.Forms.Button Find_Btn;
        internal System.Windows.Forms.RichTextBox RouteAdvancedRichTxtBox;
        internal System.Windows.Forms.ComboBox WptSelToCBox;
        internal System.Windows.Forms.ComboBox WptSelFromCBox;
        internal System.Windows.Forms.RadioButton RadioButton4;
        internal System.Windows.Forms.RadioButton RadioButton3;
        internal System.Windows.Forms.RadioButton RadioButton2;
        internal System.Windows.Forms.RadioButton RadioButton1;
        internal System.Windows.Forms.Label Label64;
        internal System.Windows.Forms.ComboBox ToStarCBox;
        internal System.Windows.Forms.ComboBox FromSidCBox;
        internal System.Windows.Forms.ComboBox ToRwyCBox;
        internal System.Windows.Forms.ComboBox FromRwyCBox;
        internal System.Windows.Forms.Label Label65;
        internal System.Windows.Forms.TextBox FromTxtbox;
        internal System.Windows.Forms.TextBox ToTxtbox;
        internal System.Windows.Forms.Label Label66;
        internal System.Windows.Forms.Panel PanelTO;
        internal System.Windows.Forms.Panel PanelAirportData;
        internal System.Windows.Forms.TableLayoutPanel TableLayoutPanel16;
        internal System.Windows.Forms.TableLayoutPanel TableLayoutPanel17;
        internal System.Windows.Forms.PictureBox PictureBox_Icon_APData;
        internal System.Windows.Forms.PictureBox PictureBox_Icon_LDG;
        internal System.Windows.Forms.PictureBox PictureBox_Icon_TO;
        internal System.Windows.Forms.TabControl TabControl1;
        internal System.Windows.Forms.TabPage TabPage1;
        internal System.Windows.Forms.TabPage TabPage2;
        internal System.Windows.Forms.GroupBox GroupBox18;
        internal System.Windows.Forms.Button UpdateAll_Btn;
        internal System.Windows.Forms.RichTextBox RichTextBox2;
        internal System.Windows.Forms.GroupBox GroupBox17;
        internal System.Windows.Forms.TextBox MetarToFindTxtBox;
        internal System.Windows.Forms.Button DownloadMetar_Btn;
        internal System.Windows.Forms.RichTextBox RichTextBox1;
        internal System.Windows.Forms.ToolTip ToolTip1;
        internal System.Windows.Forms.WebBrowser MapDisWebBrowser;
        internal System.Windows.Forms.TabPage TabPage6;
        internal System.Windows.Forms.GroupBox GroupBox19;
        internal System.Windows.Forms.RichTextBox DesForcast_RTextBox;
        internal System.Windows.Forms.Label Label86;
        internal QSP.TablessControl MainWin_TablessControl;
        internal System.Windows.Forms.TabPage Route_TabPage;
        internal System.Windows.Forms.TabPage TabPage9;
        internal System.Windows.Forms.TabPage TabPage10;
        internal System.Windows.Forms.TabPage TabPage11;
        internal System.Windows.Forms.TabPage TabPage7;
        internal System.Windows.Forms.Button WtManage_Btn;
        internal System.Windows.Forms.Button FindAltn_Btn;
        internal System.Windows.Forms.TabPage TabPage12;
        internal System.Windows.Forms.Label Label110;
        internal System.Windows.Forms.Label Label111;
        internal System.Windows.Forms.Label Label109;
        internal System.Windows.Forms.Label Label18;
        internal System.Windows.Forms.Label Label36;
        internal System.Windows.Forms.Label Label38;
        internal System.Windows.Forms.Button ShowMap_Btn;
        internal System.Windows.Forms.TabPage TabPage13;
        internal System.Windows.Forms.Button Return_Btn;
        internal System.Windows.Forms.Label Label40;
        internal System.Windows.Forms.TableLayoutPanel TableLayoutPanel21;
        internal System.Windows.Forms.Label Label41;
        internal System.Windows.Forms.Label Label42;
        internal System.Windows.Forms.TabPage TabPage14;
        internal System.Windows.Forms.Button Return_AdvancedTool_Btn;
        internal System.Windows.Forms.TableLayoutPanel TableLayoutPanel24;
        internal System.Windows.Forms.GroupBox GroupBox6;
        internal System.Windows.Forms.TableLayoutPanel TableLayoutPanel20;
        internal System.Windows.Forms.ComboBox WtUnitSel_ComboBox;
        internal System.Windows.Forms.Label Label112;
        internal System.Windows.Forms.TabPage TabPage3;
        internal System.Windows.Forms.Button Return2_Btn;
        internal System.Windows.Forms.ToolStripStatusLabel ToolStripStatusLabel3;
        internal System.Windows.Forms.ToolStripStatusLabel LblTrackDownloadStatus;
        internal System.Windows.Forms.ToolStripStatusLabel ToolStripStatusLabel2;
        internal System.Windows.Forms.ToolStripStatusLabel WindDownloadStatus_Lbl;
        internal System.Windows.Forms.Button FuelReportView_Btn;
        internal System.Windows.Forms.Button ExportBtn;

        public MainForm()
        {
            Closing += CloseMain;
            Load += Startup;
            InitializeComponent();
        }

        private UI.ToLdgModule.LandingPerf.LandingPerfControl landingPerfControl;
        private UI.ToLdgModule.TOPerf.TOPerfControl toPerfControl;
        private UI.ToLdgModule.AirportMap.AirportMapControl airportMapControl;
    }
}
