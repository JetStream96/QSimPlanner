namespace QSP.UI.Forms
{
    partial class AirportInfoControl
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.lengthUnitComboBox = new QSP.UI.Controls.ComboBoxWithBorder();
            this.slopeComboBox = new QSP.UI.Controls.ComboBoxWithBorder();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.Label92 = new System.Windows.Forms.Label();
            this.reqAirportBtn = new System.Windows.Forms.Button();
            this.Label91 = new System.Windows.Forms.Label();
            this.rwyComboBox = new QSP.UI.Controls.ComboBoxWithBorder();
            this.Label89 = new System.Windows.Forms.Label();
            this.Label87 = new System.Windows.Forms.Label();
            this.getMetarBtn = new System.Windows.Forms.Button();
            this.Label94 = new System.Windows.Forms.Label();
            this.Label88 = new System.Windows.Forms.Label();
            this.Label93 = new System.Windows.Forms.Label();
            this.Label90 = new System.Windows.Forms.Label();
            this.airportTxtBox = new System.Windows.Forms.TextBox();
            this.elevationTxtBox = new System.Windows.Forms.TextBox();
            this.rwyHeadingTxtBox = new System.Windows.Forms.TextBox();
            this.lengthTxtBox = new System.Windows.Forms.TextBox();
            this.airportNameLbl = new System.Windows.Forms.Label();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 36.17021F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 21.85687F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 42.16634F));
            this.tableLayoutPanel1.Controls.Add(this.lengthUnitComboBox, 2, 2);
            this.tableLayoutPanel1.Controls.Add(this.slopeComboBox, 1, 5);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.Label91, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.rwyComboBox, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.Label89, 2, 5);
            this.tableLayoutPanel1.Controls.Add(this.Label87, 2, 3);
            this.tableLayoutPanel1.Controls.Add(this.getMetarBtn, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.Label94, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.Label88, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.Label93, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.Label90, 0, 5);
            this.tableLayoutPanel1.Controls.Add(this.airportTxtBox, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.elevationTxtBox, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.rwyHeadingTxtBox, 1, 4);
            this.tableLayoutPanel1.Controls.Add(this.lengthTxtBox, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.airportNameLbl, 2, 0);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 6;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(517, 240);
            this.tableLayoutPanel1.TabIndex = 42;
            // 
            // lengthUnitComboBox
            // 
            this.lengthUnitComboBox.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lengthUnitComboBox.BorderColor = System.Drawing.Color.DimGray;
            this.lengthUnitComboBox.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.lengthUnitComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.lengthUnitComboBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lengthUnitComboBox.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lengthUnitComboBox.FormattingEnabled = true;
            this.lengthUnitComboBox.Items.AddRange(new object[] {
            "M",
            "FT"});
            this.lengthUnitComboBox.Location = new System.Drawing.Point(358, 82);
            this.lengthUnitComboBox.Name = "lengthUnitComboBox";
            this.lengthUnitComboBox.Size = new System.Drawing.Size(98, 31);
            this.lengthUnitComboBox.TabIndex = 42;
            // 
            // slopeComboBox
            // 
            this.slopeComboBox.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.slopeComboBox.BorderColor = System.Drawing.Color.DimGray;
            this.slopeComboBox.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.slopeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.slopeComboBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.slopeComboBox.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.slopeComboBox.FormattingEnabled = true;
            this.slopeComboBox.Location = new System.Drawing.Point(193, 202);
            this.slopeComboBox.Name = "slopeComboBox";
            this.slopeComboBox.Size = new System.Drawing.Size(98, 31);
            this.slopeComboBox.TabIndex = 41;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 84F));
            this.tableLayoutPanel2.Controls.Add(this.Label92, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.reqAirportBtn, 1, 0);
            this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(163, 39);
            this.tableLayoutPanel2.TabIndex = 33;
            // 
            // Label92
            // 
            this.Label92.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.Label92.AutoSize = true;
            this.Label92.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label92.Location = new System.Drawing.Point(4, 8);
            this.Label92.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.Label92.Name = "Label92";
            this.Label92.Size = new System.Drawing.Size(63, 23);
            this.Label92.TabIndex = 22;
            this.Label92.Text = "Airport";
            // 
            // reqAirportBtn
            // 
            this.reqAirportBtn.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.reqAirportBtn.BackColor = System.Drawing.SystemColors.Control;
            this.reqAirportBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.reqAirportBtn.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.reqAirportBtn.Location = new System.Drawing.Point(89, 3);
            this.reqAirportBtn.Name = "reqAirportBtn";
            this.reqAirportBtn.Size = new System.Drawing.Size(64, 33);
            this.reqAirportBtn.TabIndex = 39;
            this.reqAirportBtn.Text = "Req";
            this.reqAirportBtn.UseVisualStyleBackColor = false;
            // 
            // Label91
            // 
            this.Label91.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.Label91.AutoSize = true;
            this.Label91.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label91.Location = new System.Drawing.Point(4, 47);
            this.Label91.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.Label91.Name = "Label91";
            this.Label91.Size = new System.Drawing.Size(45, 23);
            this.Label91.TabIndex = 24;
            this.Label91.Text = "RWY";
            // 
            // rwyComboBox
            // 
            this.rwyComboBox.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.rwyComboBox.BorderColor = System.Drawing.Color.DimGray;
            this.rwyComboBox.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.rwyComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.rwyComboBox.Enabled = false;
            this.rwyComboBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rwyComboBox.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rwyComboBox.FormattingEnabled = true;
            this.rwyComboBox.Location = new System.Drawing.Point(193, 43);
            this.rwyComboBox.Name = "rwyComboBox";
            this.rwyComboBox.Size = new System.Drawing.Size(98, 31);
            this.rwyComboBox.TabIndex = 40;
            this.rwyComboBox.SelectedIndexChanged += new System.EventHandler(this.rwyComboBox_SelectedIndexChanged);
            // 
            // Label89
            // 
            this.Label89.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.Label89.AutoSize = true;
            this.Label89.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label89.Location = new System.Drawing.Point(395, 206);
            this.Label89.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.Label89.Name = "Label89";
            this.Label89.Size = new System.Drawing.Size(24, 23);
            this.Label89.TabIndex = 28;
            this.Label89.Text = "%";
            // 
            // Label87
            // 
            this.Label87.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.Label87.AutoSize = true;
            this.Label87.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label87.Location = new System.Drawing.Point(394, 125);
            this.Label87.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.Label87.Name = "Label87";
            this.Label87.Size = new System.Drawing.Size(27, 23);
            this.Label87.TabIndex = 32;
            this.Label87.Text = "FT";
            // 
            // getMetarBtn
            // 
            this.getMetarBtn.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.getMetarBtn.BackColor = System.Drawing.Color.DarkSlateGray;
            this.getMetarBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.getMetarBtn.Font = new System.Drawing.Font("Segoe UI", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.getMetarBtn.ForeColor = System.Drawing.Color.White;
            this.getMetarBtn.Location = new System.Drawing.Point(359, 43);
            this.getMetarBtn.Margin = new System.Windows.Forms.Padding(0);
            this.getMetarBtn.Name = "getMetarBtn";
            this.getMetarBtn.Size = new System.Drawing.Size(97, 31);
            this.getMetarBtn.TabIndex = 35;
            this.getMetarBtn.Text = "Get METAR";
            this.getMetarBtn.UseVisualStyleBackColor = false;
            // 
            // Label94
            // 
            this.Label94.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.Label94.AutoSize = true;
            this.Label94.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label94.Location = new System.Drawing.Point(4, 86);
            this.Label94.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.Label94.Name = "Label94";
            this.Label94.Size = new System.Drawing.Size(136, 23);
            this.Label94.TabIndex = 19;
            this.Label94.Text = "Available Length";
            // 
            // Label88
            // 
            this.Label88.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.Label88.AutoSize = true;
            this.Label88.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label88.Location = new System.Drawing.Point(4, 125);
            this.Label88.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.Label88.Name = "Label88";
            this.Label88.Size = new System.Drawing.Size(79, 23);
            this.Label88.TabIndex = 30;
            this.Label88.Text = "Elevation";
            // 
            // Label93
            // 
            this.Label93.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.Label93.AutoSize = true;
            this.Label93.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label93.Location = new System.Drawing.Point(4, 164);
            this.Label93.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.Label93.Name = "Label93";
            this.Label93.Size = new System.Drawing.Size(74, 23);
            this.Label93.TabIndex = 20;
            this.Label93.Text = "Heading";
            // 
            // Label90
            // 
            this.Label90.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.Label90.AutoSize = true;
            this.Label90.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label90.Location = new System.Drawing.Point(4, 206);
            this.Label90.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.Label90.Name = "Label90";
            this.Label90.Size = new System.Drawing.Size(52, 23);
            this.Label90.TabIndex = 26;
            this.Label90.Text = "Slope";
            // 
            // airportTxtBox
            // 
            this.airportTxtBox.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.airportTxtBox.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.airportTxtBox.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.airportTxtBox.Location = new System.Drawing.Point(193, 5);
            this.airportTxtBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.airportTxtBox.Name = "airportTxtBox";
            this.airportTxtBox.Size = new System.Drawing.Size(98, 30);
            this.airportTxtBox.TabIndex = 23;
            this.airportTxtBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.airportTxtBox.TextChanged += new System.EventHandler(this.airportTxtBox_TextChanged);
            // 
            // elevationTxtBox
            // 
            this.elevationTxtBox.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.elevationTxtBox.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.elevationTxtBox.Location = new System.Drawing.Point(193, 122);
            this.elevationTxtBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.elevationTxtBox.Name = "elevationTxtBox";
            this.elevationTxtBox.Size = new System.Drawing.Size(98, 30);
            this.elevationTxtBox.TabIndex = 31;
            this.elevationTxtBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // rwyHeadingTxtBox
            // 
            this.rwyHeadingTxtBox.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.rwyHeadingTxtBox.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rwyHeadingTxtBox.Location = new System.Drawing.Point(193, 161);
            this.rwyHeadingTxtBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.rwyHeadingTxtBox.Name = "rwyHeadingTxtBox";
            this.rwyHeadingTxtBox.Size = new System.Drawing.Size(98, 30);
            this.rwyHeadingTxtBox.TabIndex = 21;
            this.rwyHeadingTxtBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // lengthTxtBox
            // 
            this.lengthTxtBox.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lengthTxtBox.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lengthTxtBox.Location = new System.Drawing.Point(193, 83);
            this.lengthTxtBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.lengthTxtBox.Name = "lengthTxtBox";
            this.lengthTxtBox.Size = new System.Drawing.Size(98, 30);
            this.lengthTxtBox.TabIndex = 19;
            this.lengthTxtBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // airportNameLbl
            // 
            this.airportNameLbl.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.airportNameLbl.AutoSize = true;
            this.airportNameLbl.Font = new System.Drawing.Font("Segoe UI", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.airportNameLbl.Location = new System.Drawing.Point(315, 10);
            this.airportNameLbl.Name = "airportNameLbl";
            this.airportNameLbl.Size = new System.Drawing.Size(185, 19);
            this.airportNameLbl.TabIndex = 34;
            this.airportNameLbl.Text = "   TAIWAN TAOYUAN INTL   ";
            // 
            // AirportInfoControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "AirportInfoControl";
            this.Size = new System.Drawing.Size(517, 240);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private Controls.ComboBoxWithBorder lengthUnitComboBox;
        private Controls.ComboBoxWithBorder slopeComboBox;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        internal System.Windows.Forms.Label Label92;
        internal System.Windows.Forms.Button reqAirportBtn;
        internal System.Windows.Forms.Label Label91;
        private Controls.ComboBoxWithBorder rwyComboBox;
        internal System.Windows.Forms.Label Label89;
        internal System.Windows.Forms.Label Label87;
        internal System.Windows.Forms.Button getMetarBtn;
        internal System.Windows.Forms.Label Label94;
        internal System.Windows.Forms.Label Label88;
        internal System.Windows.Forms.Label Label93;
        internal System.Windows.Forms.Label Label90;
        internal System.Windows.Forms.TextBox airportTxtBox;
        internal System.Windows.Forms.TextBox elevationTxtBox;
        internal System.Windows.Forms.TextBox rwyHeadingTxtBox;
        internal System.Windows.Forms.TextBox lengthTxtBox;
        internal System.Windows.Forms.Label airportNameLbl;
    }
}
