namespace QSP.UI.UserControls.TakeoffLanding.Common
{
    partial class WeatherInfoControl
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
            this.tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.GetMetarBtn = new System.Windows.Forms.Button();
            this.ViewMetarBtn = new System.Windows.Forms.Button();
            this.picBox = new System.Windows.Forms.PictureBox();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.surfCondComboBox = new QSP.UI.Controls.ComboBoxWithBorder();
            this.Label105 = new System.Windows.Forms.Label();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.Label103 = new System.Windows.Forms.Label();
            this.pressUnitComboBox = new QSP.UI.Controls.ComboBoxWithBorder();
            this.tempUnitComboBox = new QSP.UI.Controls.ComboBoxWithBorder();
            this.windSpdTxtBox = new System.Windows.Forms.TextBox();
            this.Label108 = new System.Windows.Forms.Label();
            this.pressTxtBox = new System.Windows.Forms.TextBox();
            this.Label104 = new System.Windows.Forms.Label();
            this.oatTxtBox = new System.Windows.Forms.TextBox();
            this.Label107 = new System.Windows.Forms.Label();
            this.windDirTxtBox = new System.Windows.Forms.TextBox();
            this.Label106 = new System.Windows.Forms.Label();
            this.tableLayoutPanel.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picBox)).BeginInit();
            this.tableLayoutPanel3.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel
            // 
            this.tableLayoutPanel.AutoSize = true;
            this.tableLayoutPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel.ColumnCount = 1;
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel.Controls.Add(this.tableLayoutPanel2, 0, 0);
            this.tableLayoutPanel.Controls.Add(this.tableLayoutPanel3, 0, 2);
            this.tableLayoutPanel.Controls.Add(this.tableLayoutPanel1, 0, 1);
            this.tableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel.Name = "tableLayoutPanel";
            this.tableLayoutPanel.RowCount = 3;
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel.Size = new System.Drawing.Size(451, 184);
            this.tableLayoutPanel.TabIndex = 2;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 3;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 231F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 58F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Controls.Add(this.GetMetarBtn, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.ViewMetarBtn, 2, 0);
            this.tableLayoutPanel2.Controls.Add(this.picBox, 1, 0);
            this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(390, 37);
            this.tableLayoutPanel2.TabIndex = 0;
            // 
            // GetMetarBtn
            // 
            this.GetMetarBtn.BackColor = System.Drawing.Color.DarkSlateGray;
            this.GetMetarBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.GetMetarBtn.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.GetMetarBtn.ForeColor = System.Drawing.Color.White;
            this.GetMetarBtn.Location = new System.Drawing.Point(15, 3);
            this.GetMetarBtn.Margin = new System.Windows.Forms.Padding(15, 3, 0, 3);
            this.GetMetarBtn.Name = "GetMetarBtn";
            this.GetMetarBtn.Size = new System.Drawing.Size(191, 31);
            this.GetMetarBtn.TabIndex = 0;
            this.GetMetarBtn.Text = "Import METAR";
            this.GetMetarBtn.UseVisualStyleBackColor = false;
            // 
            // ViewMetarBtn
            // 
            this.ViewMetarBtn.BackColor = System.Drawing.SystemColors.MenuHighlight;
            this.ViewMetarBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ViewMetarBtn.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ViewMetarBtn.ForeColor = System.Drawing.Color.White;
            this.ViewMetarBtn.Location = new System.Drawing.Point(289, 3);
            this.ViewMetarBtn.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this.ViewMetarBtn.Name = "ViewMetarBtn";
            this.ViewMetarBtn.Size = new System.Drawing.Size(82, 31);
            this.ViewMetarBtn.TabIndex = 1;
            this.ViewMetarBtn.Text = "View";
            this.ViewMetarBtn.UseVisualStyleBackColor = false;
            // 
            // picBox
            // 
            this.picBox.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.picBox.Image = global::QSP.Properties.Resources.GreenLight;
            this.picBox.Location = new System.Drawing.Point(231, 5);
            this.picBox.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this.picBox.Name = "picBox";
            this.picBox.Size = new System.Drawing.Size(27, 27);
            this.picBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picBox.TabIndex = 43;
            this.picBox.TabStop = false;
            this.picBox.Visible = false;
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.AutoSize = true;
            this.tableLayoutPanel3.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel3.ColumnCount = 2;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Controls.Add(this.surfCondComboBox, 1, 0);
            this.tableLayoutPanel3.Controls.Add(this.Label105, 0, 0);
            this.tableLayoutPanel3.Location = new System.Drawing.Point(0, 147);
            this.tableLayoutPanel3.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 1;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(393, 37);
            this.tableLayoutPanel3.TabIndex = 2;
            // 
            // surfCondComboBox
            // 
            this.surfCondComboBox.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.surfCondComboBox.BorderColor = System.Drawing.Color.DimGray;
            this.surfCondComboBox.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.surfCondComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.surfCondComboBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.surfCondComboBox.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.surfCondComboBox.FormattingEnabled = true;
            this.surfCondComboBox.Location = new System.Drawing.Point(141, 3);
            this.surfCondComboBox.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.surfCondComboBox.Name = "surfCondComboBox";
            this.surfCondComboBox.Size = new System.Drawing.Size(247, 31);
            this.surfCondComboBox.TabIndex = 0;
            // 
            // Label105
            // 
            this.Label105.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.Label105.AutoSize = true;
            this.Label105.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label105.Location = new System.Drawing.Point(15, 7);
            this.Label105.Margin = new System.Windows.Forms.Padding(15, 0, 15, 0);
            this.Label105.Name = "Label105";
            this.Label105.Size = new System.Drawing.Size(106, 23);
            this.Label105.TabIndex = 11;
            this.Label105.Text = "RWY Surface";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.AutoSize = true;
            this.tableLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel1.ColumnCount = 5;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.Label103, 4, 0);
            this.tableLayoutPanel1.Controls.Add(this.pressUnitComboBox, 3, 2);
            this.tableLayoutPanel1.Controls.Add(this.tempUnitComboBox, 3, 1);
            this.tableLayoutPanel1.Controls.Add(this.windSpdTxtBox, 3, 0);
            this.tableLayoutPanel1.Controls.Add(this.Label108, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.pressTxtBox, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.Label104, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.oatTxtBox, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.Label107, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.windDirTxtBox, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.Label106, 0, 2);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 37);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(451, 110);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // Label103
            // 
            this.Label103.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.Label103.AutoSize = true;
            this.Label103.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label103.Location = new System.Drawing.Point(403, 6);
            this.Label103.Margin = new System.Windows.Forms.Padding(10, 0, 10, 0);
            this.Label103.Name = "Label103";
            this.Label103.Size = new System.Drawing.Size(38, 23);
            this.Label103.TabIndex = 36;
            this.Label103.Text = "KTS";
            // 
            // pressUnitComboBox
            // 
            this.pressUnitComboBox.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.pressUnitComboBox.BorderColor = System.Drawing.Color.DimGray;
            this.pressUnitComboBox.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.pressUnitComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.pressUnitComboBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.pressUnitComboBox.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pressUnitComboBox.FormattingEnabled = true;
            this.pressUnitComboBox.Items.AddRange(new object[] {
            "hPa",
            "inHg"});
            this.pressUnitComboBox.Location = new System.Drawing.Point(280, 76);
            this.pressUnitComboBox.Margin = new System.Windows.Forms.Padding(15, 3, 15, 3);
            this.pressUnitComboBox.Name = "pressUnitComboBox";
            this.pressUnitComboBox.Size = new System.Drawing.Size(98, 31);
            this.pressUnitComboBox.TabIndex = 5;
            this.pressUnitComboBox.SelectedIndexChanged += new System.EventHandler(this.pressUnitComboBoxSelectedIndexChanged);
            // 
            // tempUnitComboBox
            // 
            this.tempUnitComboBox.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.tempUnitComboBox.BorderColor = System.Drawing.Color.DimGray;
            this.tempUnitComboBox.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.tempUnitComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.tempUnitComboBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.tempUnitComboBox.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tempUnitComboBox.FormattingEnabled = true;
            this.tempUnitComboBox.Items.AddRange(new object[] {
            "°C",
            "°F"});
            this.tempUnitComboBox.Location = new System.Drawing.Point(280, 39);
            this.tempUnitComboBox.Margin = new System.Windows.Forms.Padding(15, 3, 15, 3);
            this.tempUnitComboBox.Name = "tempUnitComboBox";
            this.tempUnitComboBox.Size = new System.Drawing.Size(98, 31);
            this.tempUnitComboBox.TabIndex = 3;
            this.tempUnitComboBox.SelectedIndexChanged += new System.EventHandler(this.tempUnitComboBoxSelectedIndexChanged);
            // 
            // windSpdTxtBox
            // 
            this.windSpdTxtBox.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.windSpdTxtBox.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.windSpdTxtBox.Location = new System.Drawing.Point(280, 3);
            this.windSpdTxtBox.Margin = new System.Windows.Forms.Padding(15, 3, 15, 3);
            this.windSpdTxtBox.Name = "windSpdTxtBox";
            this.windSpdTxtBox.Size = new System.Drawing.Size(98, 30);
            this.windSpdTxtBox.TabIndex = 1;
            this.windSpdTxtBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // Label108
            // 
            this.Label108.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.Label108.AutoSize = true;
            this.Label108.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label108.Location = new System.Drawing.Point(15, 6);
            this.Label108.Margin = new System.Windows.Forms.Padding(15, 0, 15, 0);
            this.Label108.Name = "Label108";
            this.Label108.Size = new System.Drawing.Size(50, 23);
            this.Label108.TabIndex = 8;
            this.Label108.Text = "Wind";
            // 
            // pressTxtBox
            // 
            this.pressTxtBox.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.pressTxtBox.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pressTxtBox.Location = new System.Drawing.Point(119, 76);
            this.pressTxtBox.Margin = new System.Windows.Forms.Padding(15, 3, 15, 3);
            this.pressTxtBox.Name = "pressTxtBox";
            this.pressTxtBox.Size = new System.Drawing.Size(98, 30);
            this.pressTxtBox.TabIndex = 4;
            this.pressTxtBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // Label104
            // 
            this.Label104.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.Label104.AutoSize = true;
            this.Label104.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label104.Location = new System.Drawing.Point(236, 6);
            this.Label104.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.Label104.Name = "Label104";
            this.Label104.Size = new System.Drawing.Size(25, 23);
            this.Label104.TabIndex = 8;
            this.Label104.Text = "at";
            // 
            // oatTxtBox
            // 
            this.oatTxtBox.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.oatTxtBox.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.oatTxtBox.Location = new System.Drawing.Point(119, 39);
            this.oatTxtBox.Margin = new System.Windows.Forms.Padding(15, 3, 15, 3);
            this.oatTxtBox.Name = "oatTxtBox";
            this.oatTxtBox.Size = new System.Drawing.Size(98, 30);
            this.oatTxtBox.TabIndex = 2;
            this.oatTxtBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // Label107
            // 
            this.Label107.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.Label107.AutoSize = true;
            this.Label107.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label107.Location = new System.Drawing.Point(15, 43);
            this.Label107.Margin = new System.Windows.Forms.Padding(15, 0, 15, 0);
            this.Label107.Name = "Label107";
            this.Label107.Size = new System.Drawing.Size(43, 23);
            this.Label107.TabIndex = 9;
            this.Label107.Text = "OAT";
            // 
            // windDirTxtBox
            // 
            this.windDirTxtBox.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.windDirTxtBox.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.windDirTxtBox.Location = new System.Drawing.Point(119, 3);
            this.windDirTxtBox.Margin = new System.Windows.Forms.Padding(15, 3, 15, 3);
            this.windDirTxtBox.Name = "windDirTxtBox";
            this.windDirTxtBox.Size = new System.Drawing.Size(98, 30);
            this.windDirTxtBox.TabIndex = 0;
            this.windDirTxtBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // Label106
            // 
            this.Label106.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.Label106.AutoSize = true;
            this.Label106.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label106.Location = new System.Drawing.Point(15, 80);
            this.Label106.Margin = new System.Windows.Forms.Padding(15, 0, 15, 0);
            this.Label106.Name = "Label106";
            this.Label106.Size = new System.Drawing.Size(74, 23);
            this.Label106.TabIndex = 10;
            this.Label106.Text = "Pressure";
            // 
            // WeatherInfoControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(120F, 120F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.Controls.Add(this.tableLayoutPanel);
            this.DoubleBuffered = true;
            this.Name = "WeatherInfoControl";
            this.Size = new System.Drawing.Size(451, 184);
            this.tableLayoutPanel.ResumeLayout(false);
            this.tableLayoutPanel.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picBox)).EndInit();
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel3.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
        internal System.Windows.Forms.TextBox pressTxtBox;
        internal System.Windows.Forms.TextBox oatTxtBox;
        internal System.Windows.Forms.Label Label108;
        internal System.Windows.Forms.Label Label107;
        internal System.Windows.Forms.Label Label106;
        internal System.Windows.Forms.TextBox windSpdTxtBox;
        internal System.Windows.Forms.Label Label103;
        internal System.Windows.Forms.TextBox windDirTxtBox;
        internal System.Windows.Forms.Label Label104;
        internal System.Windows.Forms.Button GetMetarBtn;
        internal Controls.ComboBoxWithBorder tempUnitComboBox;
        internal Controls.ComboBoxWithBorder pressUnitComboBox;
        internal System.Windows.Forms.Button ViewMetarBtn;
        internal System.Windows.Forms.PictureBox picBox;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        internal Controls.ComboBoxWithBorder surfCondComboBox;
        internal System.Windows.Forms.Label Label105;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
    }
}
