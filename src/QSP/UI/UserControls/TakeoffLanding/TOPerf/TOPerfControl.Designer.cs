using QSP.UI.UserControls.TakeoffLanding.Common;
using QSP.UI.UserControls.TakeoffLanding.Common.AirportInfo;
using QSP.UI.Views;

namespace QSP.UI.UserControls.TakeoffLanding.TOPerf
{
    partial class TOPerfControl
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.GroupBox13 = new System.Windows.Forms.GroupBox();
            this.airportInfoControl = new QSP.UI.UserControls.TakeoffLanding.Common.AirportInfo.AirportInfoControl();
            this.GroupBox14 = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel8 = new System.Windows.Forms.TableLayoutPanel();
            this.weightTxtBox = new System.Windows.Forms.TextBox();
            this.thrustRatingLbl = new System.Windows.Forms.Label();
            this.wtUnitComboBox = new ComboBoxWithBorder();
            this.packsComboBox = new ComboBoxWithBorder();
            this.Label99 = new System.Windows.Forms.Label();
            this.regComboBox = new ComboBoxWithBorder();
            this.antiIceComboBox = new ComboBoxWithBorder();
            this.Label102 = new System.Windows.Forms.Label();
            this.Label100 = new System.Windows.Forms.Label();
            this.acListComboBox = new ComboBoxWithBorder();
            this.label1 = new System.Windows.Forms.Label();
            this.Label97 = new System.Windows.Forms.Label();
            this.Label101 = new System.Windows.Forms.Label();
            this.requestBtn = new System.Windows.Forms.Button();
            this.thrustRatingComboBox = new ComboBoxWithBorder();
            this.flapsComboBox = new ComboBoxWithBorder();
            this.calculateBtn = new System.Windows.Forms.Button();
            this.GroupBox16 = new System.Windows.Forms.GroupBox();
            this.resultsRichTxtBox = new System.Windows.Forms.RichTextBox();
            this.GroupBox15 = new System.Windows.Forms.GroupBox();
            this.weatherInfoControl = new QSP.UI.UserControls.TakeoffLanding.Common.WeatherInfoControl();
            this.tableLayoutPanel5 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel6 = new System.Windows.Forms.TableLayoutPanel();
            this.GroupBox13.SuspendLayout();
            this.GroupBox14.SuspendLayout();
            this.tableLayoutPanel8.SuspendLayout();
            this.GroupBox16.SuspendLayout();
            this.GroupBox15.SuspendLayout();
            this.tableLayoutPanel5.SuspendLayout();
            this.tableLayoutPanel6.SuspendLayout();
            this.SuspendLayout();
            // 
            // GroupBox13
            // 
            this.GroupBox13.BackColor = System.Drawing.Color.Transparent;
            this.GroupBox13.Controls.Add(this.airportInfoControl);
            this.GroupBox13.Font = new System.Drawing.Font("Segoe UI Semibold", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.GroupBox13.Location = new System.Drawing.Point(3, 4);
            this.GroupBox13.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.GroupBox13.Name = "GroupBox13";
            this.GroupBox13.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.GroupBox13.Size = new System.Drawing.Size(414, 224);
            this.GroupBox13.TabIndex = 0;
            this.GroupBox13.TabStop = false;
            this.GroupBox13.Text = "Runway";
            // 
            // airportInfoControl
            // 
            this.airportInfoControl.Airports = null;
            this.airportInfoControl.AutoSize = true;
            this.airportInfoControl.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.airportInfoControl.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.airportInfoControl.Location = new System.Drawing.Point(7, 22);
            this.airportInfoControl.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.airportInfoControl.Name = "airportInfoControl";
            this.airportInfoControl.Size = new System.Drawing.Size(361, 186);
            this.airportInfoControl.TabIndex = 0;
            // 
            // GroupBox14
            // 
            this.GroupBox14.BackColor = System.Drawing.Color.Transparent;
            this.GroupBox14.Controls.Add(this.tableLayoutPanel8);
            this.GroupBox14.Font = new System.Drawing.Font("Segoe UI Semibold", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.GroupBox14.Location = new System.Drawing.Point(423, 4);
            this.GroupBox14.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.GroupBox14.MinimumSize = new System.Drawing.Size(400, 0);
            this.GroupBox14.Name = "GroupBox14";
            this.GroupBox14.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.GroupBox14.Size = new System.Drawing.Size(400, 224);
            this.GroupBox14.TabIndex = 2;
            this.GroupBox14.TabStop = false;
            this.GroupBox14.Text = "A/C Configuration";
            // 
            // tableLayoutPanel8
            // 
            this.tableLayoutPanel8.AutoSize = true;
            this.tableLayoutPanel8.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel8.ColumnCount = 3;
            this.tableLayoutPanel8.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel8.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel8.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel8.Controls.Add(this.weightTxtBox, 1, 2);
            this.tableLayoutPanel8.Controls.Add(this.thrustRatingLbl, 0, 3);
            this.tableLayoutPanel8.Controls.Add(this.wtUnitComboBox, 2, 2);
            this.tableLayoutPanel8.Controls.Add(this.packsComboBox, 1, 6);
            this.tableLayoutPanel8.Controls.Add(this.Label99, 0, 6);
            this.tableLayoutPanel8.Controls.Add(this.regComboBox, 1, 1);
            this.tableLayoutPanel8.Controls.Add(this.antiIceComboBox, 1, 5);
            this.tableLayoutPanel8.Controls.Add(this.Label102, 0, 2);
            this.tableLayoutPanel8.Controls.Add(this.Label100, 0, 5);
            this.tableLayoutPanel8.Controls.Add(this.acListComboBox, 1, 0);
            this.tableLayoutPanel8.Controls.Add(this.label1, 0, 1);
            this.tableLayoutPanel8.Controls.Add(this.Label97, 0, 0);
            this.tableLayoutPanel8.Controls.Add(this.Label101, 0, 4);
            this.tableLayoutPanel8.Controls.Add(this.requestBtn, 2, 1);
            this.tableLayoutPanel8.Controls.Add(this.thrustRatingComboBox, 1, 3);
            this.tableLayoutPanel8.Controls.Add(this.flapsComboBox, 1, 4);
            this.tableLayoutPanel8.Location = new System.Drawing.Point(6, 16);
            this.tableLayoutPanel8.Margin = new System.Windows.Forms.Padding(2);
            this.tableLayoutPanel8.Name = "tableLayoutPanel8";
            this.tableLayoutPanel8.RowCount = 7;
            this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 32F));
            this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel8.Size = new System.Drawing.Size(364, 206);
            this.tableLayoutPanel8.TabIndex = 7;
            // 
            // weightTxtBox
            // 
            this.weightTxtBox.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.weightTxtBox.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.weightTxtBox.Location = new System.Drawing.Point(104, 59);
            this.weightTxtBox.Margin = new System.Windows.Forms.Padding(0);
            this.weightTxtBox.Name = "weightTxtBox";
            this.weightTxtBox.Size = new System.Drawing.Size(165, 26);
            this.weightTxtBox.TabIndex = 3;
            this.weightTxtBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.weightTxtBox.TextChanged += new System.EventHandler(this.WeightTxtBoxChanged);
            // 
            // thrustRatingLbl
            // 
            this.thrustRatingLbl.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.thrustRatingLbl.AutoSize = true;
            this.thrustRatingLbl.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.thrustRatingLbl.Location = new System.Drawing.Point(8, 93);
            this.thrustRatingLbl.Margin = new System.Windows.Forms.Padding(8, 5, 8, 5);
            this.thrustRatingLbl.Name = "thrustRatingLbl";
            this.thrustRatingLbl.Size = new System.Drawing.Size(77, 19);
            this.thrustRatingLbl.TabIndex = 1;
            this.thrustRatingLbl.Text = "THR Rating";
            // 
            // wtUnitComboBox
            // 
            this.wtUnitComboBox.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.wtUnitComboBox.BorderColor = System.Drawing.Color.DimGray;
            this.wtUnitComboBox.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.wtUnitComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.wtUnitComboBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.wtUnitComboBox.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.wtUnitComboBox.FormattingEnabled = true;
            this.wtUnitComboBox.Items.AddRange(new object[] {
            "KG",
            "LB"});
            this.wtUnitComboBox.Location = new System.Drawing.Point(283, 59);
            this.wtUnitComboBox.Margin = new System.Windows.Forms.Padding(8, 0, 0, 0);
            this.wtUnitComboBox.Name = "wtUnitComboBox";
            this.wtUnitComboBox.Size = new System.Drawing.Size(80, 27);
            this.wtUnitComboBox.TabIndex = 4;
            // 
            // packsComboBox
            // 
            this.packsComboBox.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.packsComboBox.BorderColor = System.Drawing.Color.DimGray;
            this.packsComboBox.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.packsComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.packsComboBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.packsComboBox.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.packsComboBox.FormattingEnabled = true;
            this.packsComboBox.Location = new System.Drawing.Point(104, 178);
            this.packsComboBox.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.packsComboBox.Name = "packsComboBox";
            this.packsComboBox.Size = new System.Drawing.Size(165, 27);
            this.packsComboBox.TabIndex = 8;
            // 
            // Label99
            // 
            this.Label99.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.Label99.AutoSize = true;
            this.Label99.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label99.Location = new System.Drawing.Point(8, 182);
            this.Label99.Margin = new System.Windows.Forms.Padding(8, 5, 8, 5);
            this.Label99.Name = "Label99";
            this.Label99.Size = new System.Drawing.Size(43, 19);
            this.Label99.TabIndex = 23;
            this.Label99.Text = "Packs";
            // 
            // regComboBox
            // 
            this.regComboBox.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.regComboBox.BorderColor = System.Drawing.Color.DimGray;
            this.regComboBox.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.regComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.regComboBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.regComboBox.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.regComboBox.FormattingEnabled = true;
            this.regComboBox.Location = new System.Drawing.Point(104, 30);
            this.regComboBox.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.regComboBox.Name = "regComboBox";
            this.regComboBox.Size = new System.Drawing.Size(165, 27);
            this.regComboBox.TabIndex = 1;
            this.regComboBox.TextChanged += new System.EventHandler(this.RegistrationChanged);
            // 
            // antiIceComboBox
            // 
            this.antiIceComboBox.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.antiIceComboBox.BorderColor = System.Drawing.Color.DimGray;
            this.antiIceComboBox.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.antiIceComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.antiIceComboBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.antiIceComboBox.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.antiIceComboBox.FormattingEnabled = true;
            this.antiIceComboBox.Location = new System.Drawing.Point(104, 149);
            this.antiIceComboBox.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.antiIceComboBox.Name = "antiIceComboBox";
            this.antiIceComboBox.Size = new System.Drawing.Size(165, 27);
            this.antiIceComboBox.TabIndex = 7;
            // 
            // Label102
            // 
            this.Label102.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.Label102.AutoSize = true;
            this.Label102.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label102.Location = new System.Drawing.Point(8, 63);
            this.Label102.Margin = new System.Windows.Forms.Padding(8, 5, 8, 5);
            this.Label102.Name = "Label102";
            this.Label102.Size = new System.Drawing.Size(46, 19);
            this.Label102.TabIndex = 8;
            this.Label102.Text = "TOWT";
            // 
            // Label100
            // 
            this.Label100.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.Label100.AutoSize = true;
            this.Label100.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label100.Location = new System.Drawing.Point(8, 153);
            this.Label100.Margin = new System.Windows.Forms.Padding(8, 5, 8, 5);
            this.Label100.Name = "Label100";
            this.Label100.Size = new System.Drawing.Size(56, 19);
            this.Label100.TabIndex = 20;
            this.Label100.Text = "Anti-ice";
            // 
            // acListComboBox
            // 
            this.acListComboBox.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.acListComboBox.BorderColor = System.Drawing.Color.DimGray;
            this.acListComboBox.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.acListComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.acListComboBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.acListComboBox.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.acListComboBox.FormattingEnabled = true;
            this.acListComboBox.Location = new System.Drawing.Point(104, 1);
            this.acListComboBox.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.acListComboBox.Name = "acListComboBox";
            this.acListComboBox.Size = new System.Drawing.Size(165, 27);
            this.acListComboBox.TabIndex = 0;
            this.acListComboBox.SelectedIndexChanged += new System.EventHandler(this.RefreshRegistrations);
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(8, 34);
            this.label1.Margin = new System.Windows.Forms.Padding(8, 5, 8, 5);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(82, 19);
            this.label1.TabIndex = 38;
            this.label1.Text = "Registration";
            // 
            // Label97
            // 
            this.Label97.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.Label97.AutoSize = true;
            this.Label97.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label97.Location = new System.Drawing.Point(8, 5);
            this.Label97.Margin = new System.Windows.Forms.Padding(8, 5, 8, 5);
            this.Label97.Name = "Label97";
            this.Label97.Size = new System.Drawing.Size(53, 19);
            this.Label97.TabIndex = 36;
            this.Label97.Text = "Aircraft";
            // 
            // Label101
            // 
            this.Label101.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.Label101.AutoSize = true;
            this.Label101.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label101.Location = new System.Drawing.Point(8, 124);
            this.Label101.Margin = new System.Windows.Forms.Padding(8, 5, 8, 5);
            this.Label101.Name = "Label101";
            this.Label101.Size = new System.Drawing.Size(40, 19);
            this.Label101.TabIndex = 9;
            this.Label101.Text = "Flaps";
            // 
            // requestBtn
            // 
            this.requestBtn.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.requestBtn.AutoSize = true;
            this.requestBtn.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.requestBtn.BackColor = System.Drawing.SystemColors.Control;
            this.requestBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.requestBtn.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.requestBtn.Location = new System.Drawing.Point(283, 29);
            this.requestBtn.Margin = new System.Windows.Forms.Padding(8, 0, 0, 0);
            this.requestBtn.Name = "requestBtn";
            this.requestBtn.Padding = new System.Windows.Forms.Padding(7, 0, 7, 0);
            this.requestBtn.Size = new System.Drawing.Size(81, 29);
            this.requestBtn.TabIndex = 2;
            this.requestBtn.Text = "Request";
            this.requestBtn.UseVisualStyleBackColor = false;
            this.requestBtn.Visible = false;
            this.requestBtn.Click += new System.EventHandler(this.RequestBtnClick);
            // 
            // thrustRatingComboBox
            // 
            this.thrustRatingComboBox.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.thrustRatingComboBox.BorderColor = System.Drawing.Color.DimGray;
            this.thrustRatingComboBox.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.thrustRatingComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.thrustRatingComboBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.thrustRatingComboBox.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.thrustRatingComboBox.FormattingEnabled = true;
            this.thrustRatingComboBox.Items.AddRange(new object[] {
            "TO",
            "TO1",
            "TO2"});
            this.thrustRatingComboBox.Location = new System.Drawing.Point(104, 89);
            this.thrustRatingComboBox.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.thrustRatingComboBox.Name = "thrustRatingComboBox";
            this.thrustRatingComboBox.Size = new System.Drawing.Size(165, 27);
            this.thrustRatingComboBox.TabIndex = 5;
            // 
            // flapsComboBox
            // 
            this.flapsComboBox.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.flapsComboBox.BorderColor = System.Drawing.Color.DimGray;
            this.flapsComboBox.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.flapsComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.flapsComboBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.flapsComboBox.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.flapsComboBox.FormattingEnabled = true;
            this.flapsComboBox.Location = new System.Drawing.Point(104, 120);
            this.flapsComboBox.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.flapsComboBox.Name = "flapsComboBox";
            this.flapsComboBox.Size = new System.Drawing.Size(165, 27);
            this.flapsComboBox.TabIndex = 6;
            // 
            // calculateBtn
            // 
            this.calculateBtn.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.calculateBtn.BackColor = System.Drawing.Color.Green;
            this.calculateBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.calculateBtn.Font = new System.Drawing.Font("Segoe UI", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.calculateBtn.ForeColor = System.Drawing.Color.White;
            this.calculateBtn.Location = new System.Drawing.Point(478, 314);
            this.calculateBtn.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.calculateBtn.Name = "calculateBtn";
            this.calculateBtn.Size = new System.Drawing.Size(304, 40);
            this.calculateBtn.TabIndex = 3;
            this.calculateBtn.Text = "Calculate";
            this.calculateBtn.UseVisualStyleBackColor = false;
            // 
            // GroupBox16
            // 
            this.GroupBox16.AutoSize = true;
            this.GroupBox16.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.GroupBox16.BackColor = System.Drawing.Color.Transparent;
            this.GroupBox16.Controls.Add(this.resultsRichTxtBox);
            this.GroupBox16.Font = new System.Drawing.Font("Segoe UI Semibold", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.GroupBox16.Location = new System.Drawing.Point(4, 439);
            this.GroupBox16.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.GroupBox16.MinimumSize = new System.Drawing.Size(812, 232);
            this.GroupBox16.Name = "GroupBox16";
            this.GroupBox16.Padding = new System.Windows.Forms.Padding(2);
            this.GroupBox16.Size = new System.Drawing.Size(815, 235);
            this.GroupBox16.TabIndex = 4;
            this.GroupBox16.TabStop = false;
            this.GroupBox16.Text = "Results";
            // 
            // resultsRichTxtBox
            // 
            this.resultsRichTxtBox.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.resultsRichTxtBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.resultsRichTxtBox.Font = new System.Drawing.Font("Courier New", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.resultsRichTxtBox.Location = new System.Drawing.Point(2, 20);
            this.resultsRichTxtBox.Margin = new System.Windows.Forms.Padding(2);
            this.resultsRichTxtBox.Name = "resultsRichTxtBox";
            this.resultsRichTxtBox.ReadOnly = true;
            this.resultsRichTxtBox.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.None;
            this.resultsRichTxtBox.Size = new System.Drawing.Size(809, 192);
            this.resultsRichTxtBox.TabIndex = 0;
            this.resultsRichTxtBox.Text = "";
            this.resultsRichTxtBox.ContentsResized += new System.Windows.Forms.ContentsResizedEventHandler(this.TxtRichTextBoxContentsResized);
            // 
            // GroupBox15
            // 
            this.GroupBox15.AutoSize = true;
            this.GroupBox15.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.GroupBox15.BackColor = System.Drawing.Color.Transparent;
            this.GroupBox15.Controls.Add(this.weatherInfoControl);
            this.GroupBox15.Font = new System.Drawing.Font("Segoe UI Semibold", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.GroupBox15.Location = new System.Drawing.Point(3, 236);
            this.GroupBox15.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.GroupBox15.MinimumSize = new System.Drawing.Size(414, 0);
            this.GroupBox15.Name = "GroupBox15";
            this.GroupBox15.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.GroupBox15.Size = new System.Drawing.Size(414, 196);
            this.GroupBox15.TabIndex = 1;
            this.GroupBox15.TabStop = false;
            this.GroupBox15.Text = "Weather";
            // 
            // weatherInfoControl
            // 
            this.weatherInfoControl.AutoSize = true;
            this.weatherInfoControl.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.weatherInfoControl.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.weatherInfoControl.Location = new System.Drawing.Point(6, 18);
            this.weatherInfoControl.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.weatherInfoControl.Name = "weatherInfoControl";
            this.weatherInfoControl.Size = new System.Drawing.Size(365, 153);
            this.weatherInfoControl.TabIndex = 0;
            // 
            // tableLayoutPanel5
            // 
            this.tableLayoutPanel5.AutoSize = true;
            this.tableLayoutPanel5.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel5.ColumnCount = 2;
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel5.Controls.Add(this.GroupBox13, 0, 0);
            this.tableLayoutPanel5.Controls.Add(this.calculateBtn, 1, 1);
            this.tableLayoutPanel5.Controls.Add(this.GroupBox14, 1, 0);
            this.tableLayoutPanel5.Controls.Add(this.GroupBox15, 0, 1);
            this.tableLayoutPanel5.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel5.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel5.Name = "tableLayoutPanel5";
            this.tableLayoutPanel5.RowCount = 2;
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel5.Size = new System.Drawing.Size(840, 436);
            this.tableLayoutPanel5.TabIndex = 5;
            // 
            // tableLayoutPanel6
            // 
            this.tableLayoutPanel6.AutoSize = true;
            this.tableLayoutPanel6.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel6.ColumnCount = 1;
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel6.Controls.Add(this.GroupBox16, 0, 1);
            this.tableLayoutPanel6.Controls.Add(this.tableLayoutPanel5, 0, 0);
            this.tableLayoutPanel6.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel6.Margin = new System.Windows.Forms.Padding(2);
            this.tableLayoutPanel6.MinimumSize = new System.Drawing.Size(840, 0);
            this.tableLayoutPanel6.Name = "tableLayoutPanel6";
            this.tableLayoutPanel6.RowCount = 2;
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel6.Size = new System.Drawing.Size(840, 677);
            this.tableLayoutPanel6.TabIndex = 6;
            // 
            // TOPerfControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.Controls.Add(this.tableLayoutPanel6);
            this.DoubleBuffered = true;
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "TOPerfControl";
            this.Size = new System.Drawing.Size(842, 679);
            this.GroupBox13.ResumeLayout(false);
            this.GroupBox13.PerformLayout();
            this.GroupBox14.ResumeLayout(false);
            this.GroupBox14.PerformLayout();
            this.tableLayoutPanel8.ResumeLayout(false);
            this.tableLayoutPanel8.PerformLayout();
            this.GroupBox16.ResumeLayout(false);
            this.GroupBox15.ResumeLayout(false);
            this.GroupBox15.PerformLayout();
            this.tableLayoutPanel5.ResumeLayout(false);
            this.tableLayoutPanel5.PerformLayout();
            this.tableLayoutPanel6.ResumeLayout(false);
            this.tableLayoutPanel6.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        internal System.Windows.Forms.GroupBox GroupBox13;
        internal AirportInfoControl airportInfoControl;
        internal System.Windows.Forms.GroupBox GroupBox14;
        internal ComboBoxWithBorder packsComboBox;
        internal ComboBoxWithBorder antiIceComboBox;
        internal ComboBoxWithBorder wtUnitComboBox;
        internal ComboBoxWithBorder flapsComboBox;
        internal ComboBoxWithBorder acListComboBox;
        internal System.Windows.Forms.Button requestBtn;
        internal System.Windows.Forms.Label Label97;
        internal System.Windows.Forms.Label Label102;
        internal System.Windows.Forms.Label Label101;
        internal System.Windows.Forms.Label Label100;
        internal System.Windows.Forms.Label Label99;
        internal System.Windows.Forms.TextBox weightTxtBox;
        internal System.Windows.Forms.Button calculateBtn;
        internal System.Windows.Forms.GroupBox GroupBox16;
        internal System.Windows.Forms.RichTextBox resultsRichTxtBox;
        internal System.Windows.Forms.GroupBox GroupBox15;
        internal WeatherInfoControl weatherInfoControl;
        internal System.Windows.Forms.Label thrustRatingLbl;
        internal ComboBoxWithBorder thrustRatingComboBox;
        internal ComboBoxWithBorder regComboBox;
        internal System.Windows.Forms.Label label1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel5;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel6;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel8;
    }
}
