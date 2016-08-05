namespace QSP.UI.ToLdgModule.AirportMap
{
    partial class AirportMapControl
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
            this.airportDataGrid = new System.Windows.Forms.DataGridView();
            this.Elevation_Lbl1 = new System.Windows.Forms.Label();
            this.elevationLbl = new System.Windows.Forms.Label();
            this.latLonLbl = new System.Windows.Forms.Label();
            this.airportNameLbl = new System.Windows.Forms.Label();
            this.transExistLbl = new System.Windows.Forms.Label();
            this.metarLbl = new System.Windows.Forms.Label();
            this.LatLon_lbl1 = new System.Windows.Forms.Label();
            this.transAltLbl = new System.Windows.Forms.Label();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.updateBtn = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.icaoComboBox = new QSP.UI.Controls.ComboBoxWithBorder();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            ((System.ComponentModel.ISupportInitialize)(this.airportDataGrid)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // airportDataGrid
            // 
            this.airportDataGrid.AllowUserToAddRows = false;
            this.airportDataGrid.AllowUserToDeleteRows = false;
            this.airportDataGrid.AllowUserToResizeRows = false;
            this.airportDataGrid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.airportDataGrid.BackgroundColor = System.Drawing.SystemColors.ButtonHighlight;
            this.airportDataGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.airportDataGrid.Location = new System.Drawing.Point(3, 3);
            this.airportDataGrid.Name = "airportDataGrid";
            this.airportDataGrid.ReadOnly = true;
            this.airportDataGrid.RowHeadersVisible = false;
            this.airportDataGrid.RowTemplate.Height = 24;
            this.airportDataGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.airportDataGrid.Size = new System.Drawing.Size(1005, 150);
            this.airportDataGrid.TabIndex = 4;
            // 
            // Elevation_Lbl1
            // 
            this.Elevation_Lbl1.AutoSize = true;
            this.Elevation_Lbl1.Font = new System.Drawing.Font("Arial", 10.2F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Elevation_Lbl1.ForeColor = System.Drawing.SystemColors.GrayText;
            this.Elevation_Lbl1.Location = new System.Drawing.Point(554, 0);
            this.Elevation_Lbl1.Name = "Elevation_Lbl1";
            this.Elevation_Lbl1.Size = new System.Drawing.Size(85, 19);
            this.Elevation_Lbl1.TabIndex = 3;
            this.Elevation_Lbl1.Text = "Elevation:";
            // 
            // elevationLbl
            // 
            this.elevationLbl.AutoSize = true;
            this.elevationLbl.Font = new System.Drawing.Font("Arial", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.elevationLbl.ForeColor = System.Drawing.SystemColors.ControlText;
            this.elevationLbl.Location = new System.Drawing.Point(651, 0);
            this.elevationLbl.Name = "elevationLbl";
            this.elevationLbl.Size = new System.Drawing.Size(61, 19);
            this.elevationLbl.TabIndex = 4;
            this.elevationLbl.Text = "105 FT";
            // 
            // latLonLbl
            // 
            this.latLonLbl.AutoSize = true;
            this.latLonLbl.Font = new System.Drawing.Font("Arial", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.latLonLbl.ForeColor = System.Drawing.SystemColors.ControlText;
            this.latLonLbl.Location = new System.Drawing.Point(347, 0);
            this.latLonLbl.Name = "latLonLbl";
            this.latLonLbl.Size = new System.Drawing.Size(187, 19);
            this.latLonLbl.TabIndex = 2;
            this.latLonLbl.Text = "25.080167 / 121.232222";
            // 
            // airportNameLbl
            // 
            this.airportNameLbl.AutoSize = true;
            this.airportNameLbl.Font = new System.Drawing.Font("Arial", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.airportNameLbl.ForeColor = System.Drawing.SystemColors.ControlText;
            this.airportNameLbl.Location = new System.Drawing.Point(3, 0);
            this.airportNameLbl.Name = "airportNameLbl";
            this.airportNameLbl.Size = new System.Drawing.Size(196, 19);
            this.airportNameLbl.TabIndex = 0;
            this.airportNameLbl.Text = "TAIWAN TAOYUAN INTL";
            // 
            // transExistLbl
            // 
            this.transExistLbl.AutoSize = true;
            this.transExistLbl.Font = new System.Drawing.Font("Arial", 10.2F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.transExistLbl.ForeColor = System.Drawing.SystemColors.GrayText;
            this.transExistLbl.Location = new System.Drawing.Point(751, 0);
            this.transExistLbl.Name = "transExistLbl";
            this.transExistLbl.Size = new System.Drawing.Size(60, 19);
            this.transExistLbl.TabIndex = 5;
            this.transExistLbl.Text = "TA/TL:";
            this.transExistLbl.Visible = false;
            // 
            // metarLbl
            // 
            this.metarLbl.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.metarLbl.Cursor = System.Windows.Forms.Cursors.Hand;
            this.metarLbl.Font = new System.Drawing.Font("Times New Roman", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.metarLbl.Location = new System.Drawing.Point(174, 8);
            this.metarLbl.Name = "metarLbl";
            this.metarLbl.Size = new System.Drawing.Size(786, 60);
            this.metarLbl.TabIndex = 1;
            this.metarLbl.Text = "2015/02/20 09:00\r\nRCTP 200900Z 36003KT 310V030 9999 FEW010 BKN040 18/13 Q1018 NOS" +
    "IG RMK A3006";
            this.metarLbl.Click += new System.EventHandler(this.metarLbl_Click);
            // 
            // LatLon_lbl1
            // 
            this.LatLon_lbl1.AutoSize = true;
            this.LatLon_lbl1.Font = new System.Drawing.Font("Arial", 10.2F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LatLon_lbl1.ForeColor = System.Drawing.SystemColors.GrayText;
            this.LatLon_lbl1.Location = new System.Drawing.Point(253, 0);
            this.LatLon_lbl1.Name = "LatLon_lbl1";
            this.LatLon_lbl1.Size = new System.Drawing.Size(85, 19);
            this.LatLon_lbl1.TabIndex = 1;
            this.LatLon_lbl1.Text = "LAT/LON:";
            // 
            // transAltLbl
            // 
            this.transAltLbl.AutoSize = true;
            this.transAltLbl.Font = new System.Drawing.Font("Arial", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.transAltLbl.ForeColor = System.Drawing.SystemColors.ControlText;
            this.transAltLbl.Location = new System.Drawing.Point(840, 0);
            this.transAltLbl.Name = "transAltLbl";
            this.transAltLbl.Size = new System.Drawing.Size(115, 19);
            this.transAltLbl.TabIndex = 6;
            this.transAltLbl.Text = "11000 / FL130";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 7;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 250F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 94F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 207F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 97F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 89F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 174F));
            this.tableLayoutPanel1.Controls.Add(this.airportNameLbl, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.LatLon_lbl1, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.latLonLbl, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.Elevation_Lbl1, 3, 0);
            this.tableLayoutPanel1.Controls.Add(this.transAltLbl, 6, 0);
            this.tableLayoutPanel1.Controls.Add(this.elevationLbl, 4, 0);
            this.tableLayoutPanel1.Controls.Add(this.transExistLbl, 5, 0);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(5, 79);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1011, 28);
            this.tableLayoutPanel1.TabIndex = 3;
            // 
            // updateBtn
            // 
            this.updateBtn.BackgroundImage = global::QSP.Properties.Resources.processing9;
            this.updateBtn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.updateBtn.Cursor = System.Windows.Forms.Cursors.Hand;
            this.updateBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.updateBtn.Location = new System.Drawing.Point(972, 19);
            this.updateBtn.Name = "updateBtn";
            this.updateBtn.Size = new System.Drawing.Size(33, 33);
            this.updateBtn.TabIndex = 2;
            this.updateBtn.UseVisualStyleBackColor = true;
            this.updateBtn.Click += new System.EventHandler(this.updateBtn_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(9, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(42, 19);
            this.label1.TabIndex = 5;
            this.label1.Text = "ICAO";
            // 
            // icaoComboBox
            // 
            this.icaoComboBox.BorderColor = System.Drawing.Color.DimGray;
            this.icaoComboBox.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.icaoComboBox.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.icaoComboBox.FormattingEnabled = true;
            this.icaoComboBox.Location = new System.Drawing.Point(12, 31);
            this.icaoComboBox.Name = "icaoComboBox";
            this.icaoComboBox.Size = new System.Drawing.Size(118, 31);
            this.icaoComboBox.TabIndex = 0;
            this.icaoComboBox.TextChanged += new System.EventHandler(this.icaoComboBox_TextChanged);
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 1;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Controls.Add(this.airportDataGrid, 0, 0);
            this.tableLayoutPanel2.Location = new System.Drawing.Point(5, 113);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 2;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(1011, 554);
            this.tableLayoutPanel2.TabIndex = 6;
            // 
            // AirportMapControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(120F, 120F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.Controls.Add(this.tableLayoutPanel2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.updateBtn);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.icaoComboBox);
            this.Controls.Add(this.metarLbl);
            this.DoubleBuffered = true;
            this.Name = "AirportMapControl";
            this.Size = new System.Drawing.Size(1024, 670);
            ((System.ComponentModel.ISupportInitialize)(this.airportDataGrid)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        internal System.Windows.Forms.DataGridView airportDataGrid;
        internal System.Windows.Forms.Label Elevation_Lbl1;
        internal System.Windows.Forms.Label elevationLbl;
        internal System.Windows.Forms.Label latLonLbl;
        internal System.Windows.Forms.Label airportNameLbl;
        internal System.Windows.Forms.Label transExistLbl;
        internal System.Windows.Forms.Label metarLbl;
        internal System.Windows.Forms.Label LatLon_lbl1;
        internal System.Windows.Forms.Label transAltLbl;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        internal System.Windows.Forms.Button updateBtn;
        private System.Windows.Forms.Label label1;
        internal Controls.ComboBoxWithBorder icaoComboBox;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
    }
}
