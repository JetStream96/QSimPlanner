namespace QSP.UI.ToLdgModule.Common
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
            this.tableLayoutPanel6 = new System.Windows.Forms.TableLayoutPanel();
            this.pressUnitComboBox = new QSP.UI.Controls.ComboBoxWithBorder();
            this.pressTxtBox = new System.Windows.Forms.TextBox();
            this.tableLayoutPanel5 = new System.Windows.Forms.TableLayoutPanel();
            this.tempUnitComboBox = new QSP.UI.Controls.ComboBoxWithBorder();
            this.oatTxtBox = new System.Windows.Forms.TextBox();
            this.Label108 = new System.Windows.Forms.Label();
            this.Label107 = new System.Windows.Forms.Label();
            this.Label106 = new System.Windows.Forms.Label();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.windSpdTxtBox = new System.Windows.Forms.TextBox();
            this.Label103 = new System.Windows.Forms.Label();
            this.windDirTxtBox = new System.Windows.Forms.TextBox();
            this.Label104 = new System.Windows.Forms.Label();
            this.GetMetarBtn = new System.Windows.Forms.Button();
            this.ViewMetarBtn = new System.Windows.Forms.Button();
            this.picBox = new System.Windows.Forms.PictureBox();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.surfCondComboBox = new QSP.UI.Controls.ComboBoxWithBorder();
            this.Label105 = new System.Windows.Forms.Label();
            this.tableLayoutPanel.SuspendLayout();
            this.tableLayoutPanel6.SuspendLayout();
            this.tableLayoutPanel5.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picBox)).BeginInit();
            this.tableLayoutPanel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel
            // 
            this.tableLayoutPanel.ColumnCount = 2;
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 19.34236F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 80.65764F));
            this.tableLayoutPanel.Controls.Add(this.tableLayoutPanel6, 1, 2);
            this.tableLayoutPanel.Controls.Add(this.tableLayoutPanel5, 1, 1);
            this.tableLayoutPanel.Controls.Add(this.Label108, 0, 0);
            this.tableLayoutPanel.Controls.Add(this.Label107, 0, 1);
            this.tableLayoutPanel.Controls.Add(this.Label106, 0, 2);
            this.tableLayoutPanel.Controls.Add(this.tableLayoutPanel4, 1, 0);
            this.tableLayoutPanel.Location = new System.Drawing.Point(0, 35);
            this.tableLayoutPanel.Name = "tableLayoutPanel";
            this.tableLayoutPanel.RowCount = 3;
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.tableLayoutPanel.Size = new System.Drawing.Size(461, 105);
            this.tableLayoutPanel.TabIndex = 2;
            // 
            // tableLayoutPanel6
            // 
            this.tableLayoutPanel6.ColumnCount = 2;
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 31.79348F));
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 68.20652F));
            this.tableLayoutPanel6.Controls.Add(this.pressUnitComboBox, 1, 0);
            this.tableLayoutPanel6.Controls.Add(this.pressTxtBox, 0, 0);
            this.tableLayoutPanel6.Location = new System.Drawing.Point(89, 70);
            this.tableLayoutPanel6.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel6.Name = "tableLayoutPanel6";
            this.tableLayoutPanel6.RowCount = 1;
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel6.Size = new System.Drawing.Size(368, 35);
            this.tableLayoutPanel6.TabIndex = 34;
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
            this.pressUnitComboBox.Location = new System.Drawing.Point(193, 2);
            this.pressUnitComboBox.Margin = new System.Windows.Forms.Padding(0);
            this.pressUnitComboBox.Name = "pressUnitComboBox";
            this.pressUnitComboBox.Size = new System.Drawing.Size(98, 31);
            this.pressUnitComboBox.TabIndex = 1;
            this.pressUnitComboBox.SelectedIndexChanged += new System.EventHandler(this.pressUnitComboBox_SelectedIndexChanged);
            // 
            // pressTxtBox
            // 
            this.pressTxtBox.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.pressTxtBox.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pressTxtBox.Location = new System.Drawing.Point(9, 2);
            this.pressTxtBox.Margin = new System.Windows.Forms.Padding(0);
            this.pressTxtBox.Name = "pressTxtBox";
            this.pressTxtBox.Size = new System.Drawing.Size(98, 30);
            this.pressTxtBox.TabIndex = 0;
            this.pressTxtBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // tableLayoutPanel5
            // 
            this.tableLayoutPanel5.ColumnCount = 2;
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 31.79348F));
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 68.20652F));
            this.tableLayoutPanel5.Controls.Add(this.tempUnitComboBox, 1, 0);
            this.tableLayoutPanel5.Controls.Add(this.oatTxtBox, 0, 0);
            this.tableLayoutPanel5.Location = new System.Drawing.Point(89, 35);
            this.tableLayoutPanel5.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel5.Name = "tableLayoutPanel5";
            this.tableLayoutPanel5.RowCount = 1;
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel5.Size = new System.Drawing.Size(368, 35);
            this.tableLayoutPanel5.TabIndex = 33;
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
            this.tempUnitComboBox.Location = new System.Drawing.Point(193, 2);
            this.tempUnitComboBox.Margin = new System.Windows.Forms.Padding(0);
            this.tempUnitComboBox.Name = "tempUnitComboBox";
            this.tempUnitComboBox.Size = new System.Drawing.Size(98, 31);
            this.tempUnitComboBox.TabIndex = 1;
            this.tempUnitComboBox.SelectedIndexChanged += new System.EventHandler(this.tempUnitComboBox_SelectedIndexChanged);
            // 
            // oatTxtBox
            // 
            this.oatTxtBox.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.oatTxtBox.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.oatTxtBox.Location = new System.Drawing.Point(9, 2);
            this.oatTxtBox.Margin = new System.Windows.Forms.Padding(0);
            this.oatTxtBox.Name = "oatTxtBox";
            this.oatTxtBox.Size = new System.Drawing.Size(98, 30);
            this.oatTxtBox.TabIndex = 0;
            this.oatTxtBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // Label108
            // 
            this.Label108.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.Label108.AutoSize = true;
            this.Label108.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label108.Location = new System.Drawing.Point(4, 6);
            this.Label108.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.Label108.Name = "Label108";
            this.Label108.Size = new System.Drawing.Size(50, 23);
            this.Label108.TabIndex = 8;
            this.Label108.Text = "Wind";
            // 
            // Label107
            // 
            this.Label107.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.Label107.AutoSize = true;
            this.Label107.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label107.Location = new System.Drawing.Point(4, 41);
            this.Label107.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.Label107.Name = "Label107";
            this.Label107.Size = new System.Drawing.Size(43, 23);
            this.Label107.TabIndex = 9;
            this.Label107.Text = "OAT";
            // 
            // Label106
            // 
            this.Label106.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.Label106.AutoSize = true;
            this.Label106.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label106.Location = new System.Drawing.Point(4, 76);
            this.Label106.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.Label106.Name = "Label106";
            this.Label106.Size = new System.Drawing.Size(74, 23);
            this.Label106.TabIndex = 10;
            this.Label106.Text = "Pressure";
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.ColumnCount = 4;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 66.10169F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.8983F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 130F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 71F));
            this.tableLayoutPanel4.Controls.Add(this.windSpdTxtBox, 2, 0);
            this.tableLayoutPanel4.Controls.Add(this.Label103, 3, 0);
            this.tableLayoutPanel4.Controls.Add(this.windDirTxtBox, 0, 0);
            this.tableLayoutPanel4.Controls.Add(this.Label104, 1, 0);
            this.tableLayoutPanel4.Location = new System.Drawing.Point(89, 0);
            this.tableLayoutPanel4.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 1;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel4.Size = new System.Drawing.Size(368, 35);
            this.tableLayoutPanel4.TabIndex = 0;
            // 
            // windSpdTxtBox
            // 
            this.windSpdTxtBox.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.windSpdTxtBox.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.windSpdTxtBox.Location = new System.Drawing.Point(182, 2);
            this.windSpdTxtBox.Margin = new System.Windows.Forms.Padding(0);
            this.windSpdTxtBox.Name = "windSpdTxtBox";
            this.windSpdTxtBox.Size = new System.Drawing.Size(98, 30);
            this.windSpdTxtBox.TabIndex = 1;
            this.windSpdTxtBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // Label103
            // 
            this.Label103.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.Label103.AutoSize = true;
            this.Label103.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label103.Location = new System.Drawing.Point(313, 6);
            this.Label103.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.Label103.Name = "Label103";
            this.Label103.Size = new System.Drawing.Size(38, 23);
            this.Label103.TabIndex = 36;
            this.Label103.Text = "KTS";
            // 
            // windDirTxtBox
            // 
            this.windDirTxtBox.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.windDirTxtBox.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.windDirTxtBox.Location = new System.Drawing.Point(6, 2);
            this.windDirTxtBox.Margin = new System.Windows.Forms.Padding(0);
            this.windDirTxtBox.Name = "windDirTxtBox";
            this.windDirTxtBox.Size = new System.Drawing.Size(98, 30);
            this.windDirTxtBox.TabIndex = 0;
            this.windDirTxtBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // Label104
            // 
            this.Label104.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.Label104.AutoSize = true;
            this.Label104.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label104.Location = new System.Drawing.Point(125, 6);
            this.Label104.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.Label104.Name = "Label104";
            this.Label104.Size = new System.Drawing.Size(25, 23);
            this.Label104.TabIndex = 8;
            this.Label104.Text = "at";
            // 
            // GetMetarBtn
            // 
            this.GetMetarBtn.BackColor = System.Drawing.Color.DarkSlateGray;
            this.GetMetarBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.GetMetarBtn.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.GetMetarBtn.ForeColor = System.Drawing.Color.White;
            this.GetMetarBtn.Location = new System.Drawing.Point(7, 2);
            this.GetMetarBtn.Margin = new System.Windows.Forms.Padding(0);
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
            this.ViewMetarBtn.Location = new System.Drawing.Point(237, 2);
            this.ViewMetarBtn.Margin = new System.Windows.Forms.Padding(0);
            this.ViewMetarBtn.Name = "ViewMetarBtn";
            this.ViewMetarBtn.Size = new System.Drawing.Size(82, 31);
            this.ViewMetarBtn.TabIndex = 1;
            this.ViewMetarBtn.Text = "View";
            this.ViewMetarBtn.UseVisualStyleBackColor = false;
            // 
            // picBox
            // 
            this.picBox.Image = global::QSP.Properties.Resources.checkIconLarge;
            this.picBox.Location = new System.Drawing.Point(203, 4);
            this.picBox.Name = "picBox";
            this.picBox.Size = new System.Drawing.Size(27, 27);
            this.picBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picBox.TabIndex = 43;
            this.picBox.TabStop = false;
            this.picBox.Visible = false;
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 2;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 29.09091F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 70.90909F));
            this.tableLayoutPanel3.Controls.Add(this.surfCondComboBox, 1, 0);
            this.tableLayoutPanel3.Controls.Add(this.Label105, 0, 0);
            this.tableLayoutPanel3.Location = new System.Drawing.Point(0, 140);
            this.tableLayoutPanel3.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 1;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(440, 35);
            this.tableLayoutPanel3.TabIndex = 3;
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
            this.surfCondComboBox.Location = new System.Drawing.Point(160, 4);
            this.surfCondComboBox.Margin = new System.Windows.Forms.Padding(0);
            this.surfCondComboBox.Name = "surfCondComboBox";
            this.surfCondComboBox.Size = new System.Drawing.Size(247, 31);
            this.surfCondComboBox.TabIndex = 0;
            // 
            // Label105
            // 
            this.Label105.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.Label105.AutoSize = true;
            this.Label105.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label105.Location = new System.Drawing.Point(4, 8);
            this.Label105.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.Label105.Name = "Label105";
            this.Label105.Size = new System.Drawing.Size(106, 23);
            this.Label105.TabIndex = 11;
            this.Label105.Text = "RWY Surface";
            // 
            // WeatherInfoControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(120F, 120F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.Controls.Add(this.tableLayoutPanel3);
            this.Controls.Add(this.picBox);
            this.Controls.Add(this.ViewMetarBtn);
            this.Controls.Add(this.GetMetarBtn);
            this.Controls.Add(this.tableLayoutPanel);
            this.DoubleBuffered = true;
            this.Name = "WeatherInfoControl";
            this.Size = new System.Drawing.Size(490, 175);
            this.tableLayoutPanel.ResumeLayout(false);
            this.tableLayoutPanel.PerformLayout();
            this.tableLayoutPanel6.ResumeLayout(false);
            this.tableLayoutPanel6.PerformLayout();
            this.tableLayoutPanel5.ResumeLayout(false);
            this.tableLayoutPanel5.PerformLayout();
            this.tableLayoutPanel4.ResumeLayout(false);
            this.tableLayoutPanel4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picBox)).EndInit();
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel6;
        internal System.Windows.Forms.TextBox pressTxtBox;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel5;
        internal System.Windows.Forms.TextBox oatTxtBox;
        internal System.Windows.Forms.Label Label108;
        internal System.Windows.Forms.Label Label107;
        internal System.Windows.Forms.Label Label106;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
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
    }
}
