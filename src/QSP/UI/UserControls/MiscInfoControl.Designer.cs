namespace QSP.UI.UserControls
{
    partial class MiscInfoControl
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
            this.TabControl1 = new System.Windows.Forms.TabControl();
            this.TabPage1 = new System.Windows.Forms.TabPage();
            this.airportMapControl = new QSP.UI.ToLdgModule.AirportMap.AirportMapControl();
            this.TabPage2 = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.GroupBox17 = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel5 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.metarToFindTxtBox = new System.Windows.Forms.TextBox();
            this.downloadMetarBtn = new System.Windows.Forms.Button();
            this.RichTextBox1 = new System.Windows.Forms.RichTextBox();
            this.GroupBox18 = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.updateMetarBtn = new System.Windows.Forms.Button();
            this.metarLastUpdatedLbl = new System.Windows.Forms.Label();
            this.metarRichTxtBox = new System.Windows.Forms.RichTextBox();
            this.TabPage6 = new System.Windows.Forms.TabPage();
            this.GroupBox19 = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel6 = new System.Windows.Forms.TableLayoutPanel();
            this.updateDesForcastBtn = new System.Windows.Forms.Button();
            this.desForcastLastUpdatedLbl = new System.Windows.Forms.Label();
            this.desForcastRichTxtBox = new System.Windows.Forms.RichTextBox();
            this.destIcaoLbl = new System.Windows.Forms.Label();
            this.TabControl1.SuspendLayout();
            this.TabPage1.SuspendLayout();
            this.TabPage2.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.GroupBox17.SuspendLayout();
            this.tableLayoutPanel5.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            this.GroupBox18.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.TabPage6.SuspendLayout();
            this.GroupBox19.SuspendLayout();
            this.tableLayoutPanel6.SuspendLayout();
            this.SuspendLayout();
            // 
            // TabControl1
            // 
            this.TabControl1.Controls.Add(this.TabPage1);
            this.TabControl1.Controls.Add(this.TabPage2);
            this.TabControl1.Controls.Add(this.TabPage6);
            this.TabControl1.Font = new System.Drawing.Font("Segoe UI", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TabControl1.Location = new System.Drawing.Point(0, 0);
            this.TabControl1.Name = "TabControl1";
            this.TabControl1.SelectedIndex = 0;
            this.TabControl1.Size = new System.Drawing.Size(1099, 678);
            this.TabControl1.TabIndex = 44;
            // 
            // TabPage1
            // 
            this.TabPage1.Controls.Add(this.airportMapControl);
            this.TabPage1.Location = new System.Drawing.Point(4, 26);
            this.TabPage1.Name = "TabPage1";
            this.TabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.TabPage1.Size = new System.Drawing.Size(1091, 648);
            this.TabPage1.TabIndex = 0;
            this.TabPage1.Text = "Airport Data";
            this.TabPage1.UseVisualStyleBackColor = true;
            // 
            // airportMapControl
            // 
            this.airportMapControl.Airports = null;
            this.airportMapControl.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.airportMapControl.BrowserEnabled = false;
            this.airportMapControl.Location = new System.Drawing.Point(0, 0);
            this.airportMapControl.Name = "airportMapControl";
            this.airportMapControl.Size = new System.Drawing.Size(1024, 670);
            this.airportMapControl.StaticMapEnabled = false;
            this.airportMapControl.TabIndex = 0;
            // 
            // TabPage2
            // 
            this.TabPage2.Controls.Add(this.tableLayoutPanel1);
            this.TabPage2.Location = new System.Drawing.Point(4, 26);
            this.TabPage2.Name = "TabPage2";
            this.TabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.TabPage2.Size = new System.Drawing.Size(1091, 648);
            this.TabPage2.TabIndex = 1;
            this.TabPage2.Text = "METAR/TAF";
            this.TabPage2.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.AutoSize = true;
            this.tableLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.GroupBox17, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.GroupBox18, 0, 1);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1088, 641);
            this.tableLayoutPanel1.TabIndex = 81;
            // 
            // GroupBox17
            // 
            this.GroupBox17.Controls.Add(this.tableLayoutPanel5);
            this.GroupBox17.Location = new System.Drawing.Point(3, 3);
            this.GroupBox17.Name = "GroupBox17";
            this.GroupBox17.Size = new System.Drawing.Size(1082, 229);
            this.GroupBox17.TabIndex = 79;
            this.GroupBox17.TabStop = false;
            this.GroupBox17.Text = "Search";
            // 
            // tableLayoutPanel5
            // 
            this.tableLayoutPanel5.ColumnCount = 1;
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel5.Controls.Add(this.tableLayoutPanel4, 0, 0);
            this.tableLayoutPanel5.Controls.Add(this.RichTextBox1, 0, 1);
            this.tableLayoutPanel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel5.Location = new System.Drawing.Point(3, 21);
            this.tableLayoutPanel5.Name = "tableLayoutPanel5";
            this.tableLayoutPanel5.RowCount = 2;
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel5.Size = new System.Drawing.Size(1076, 205);
            this.tableLayoutPanel5.TabIndex = 80;
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.AutoSize = true;
            this.tableLayoutPanel4.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel4.ColumnCount = 2;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel4.Controls.Add(this.metarToFindTxtBox, 0, 0);
            this.tableLayoutPanel4.Controls.Add(this.downloadMetarBtn, 1, 0);
            this.tableLayoutPanel4.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 1;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel4.Size = new System.Drawing.Size(338, 36);
            this.tableLayoutPanel4.TabIndex = 79;
            // 
            // metarToFindTxtBox
            // 
            this.metarToFindTxtBox.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.metarToFindTxtBox.Location = new System.Drawing.Point(3, 3);
            this.metarToFindTxtBox.Name = "metarToFindTxtBox";
            this.metarToFindTxtBox.Size = new System.Drawing.Size(100, 30);
            this.metarToFindTxtBox.TabIndex = 77;
            // 
            // downloadMetarBtn
            // 
            this.downloadMetarBtn.BackColor = System.Drawing.SystemColors.Control;
            this.downloadMetarBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.downloadMetarBtn.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.downloadMetarBtn.Location = new System.Drawing.Point(109, 3);
            this.downloadMetarBtn.Name = "downloadMetarBtn";
            this.downloadMetarBtn.Size = new System.Drawing.Size(226, 30);
            this.downloadMetarBtn.TabIndex = 78;
            this.downloadMetarBtn.Text = "Download METAR/TAF";
            this.downloadMetarBtn.UseVisualStyleBackColor = false;
            this.downloadMetarBtn.Click += new System.EventHandler(this.downloadMetarBtnClick);
            // 
            // RichTextBox1
            // 
            this.RichTextBox1.Font = new System.Drawing.Font("Courier New", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RichTextBox1.Location = new System.Drawing.Point(3, 45);
            this.RichTextBox1.Name = "RichTextBox1";
            this.RichTextBox1.Size = new System.Drawing.Size(1066, 156);
            this.RichTextBox1.TabIndex = 0;
            this.RichTextBox1.Text = "";
            // 
            // GroupBox18
            // 
            this.GroupBox18.Controls.Add(this.tableLayoutPanel3);
            this.GroupBox18.Location = new System.Drawing.Point(3, 238);
            this.GroupBox18.Name = "GroupBox18";
            this.GroupBox18.Size = new System.Drawing.Size(1082, 400);
            this.GroupBox18.TabIndex = 80;
            this.GroupBox18.TabStop = false;
            this.GroupBox18.Text = "Monitor";
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 1;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Controls.Add(this.tableLayoutPanel2, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.metarRichTxtBox, 0, 1);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(3, 21);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 2;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(1076, 376);
            this.tableLayoutPanel3.TabIndex = 83;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel2.AutoSize = true;
            this.tableLayoutPanel2.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Controls.Add(this.updateMetarBtn, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.metarLastUpdatedLbl, 0, 0);
            this.tableLayoutPanel2.Location = new System.Drawing.Point(666, 3);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(407, 41);
            this.tableLayoutPanel2.TabIndex = 82;
            // 
            // updateMetarBtn
            // 
            this.updateMetarBtn.AutoSize = true;
            this.updateMetarBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.updateMetarBtn.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.updateMetarBtn.Image = global::QSP.Properties.Resources.processing9;
            this.updateMetarBtn.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.updateMetarBtn.Location = new System.Drawing.Point(296, 0);
            this.updateMetarBtn.Margin = new System.Windows.Forms.Padding(0);
            this.updateMetarBtn.Name = "updateMetarBtn";
            this.updateMetarBtn.Size = new System.Drawing.Size(111, 41);
            this.updateMetarBtn.TabIndex = 80;
            this.updateMetarBtn.Text = "Refresh";
            this.updateMetarBtn.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.updateMetarBtn.UseVisualStyleBackColor = true;
            this.updateMetarBtn.Click += new System.EventHandler(this.UpdateAllMetarTaf);
            // 
            // metarLastUpdatedLbl
            // 
            this.metarLastUpdatedLbl.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.metarLastUpdatedLbl.AutoSize = true;
            this.metarLastUpdatedLbl.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.metarLastUpdatedLbl.Location = new System.Drawing.Point(3, 9);
            this.metarLastUpdatedLbl.Margin = new System.Windows.Forms.Padding(3);
            this.metarLastUpdatedLbl.Name = "metarLastUpdatedLbl";
            this.metarLastUpdatedLbl.Size = new System.Drawing.Size(290, 23);
            this.metarLastUpdatedLbl.TabIndex = 81;
            this.metarLastUpdatedLbl.Text = "Last Updated : 7/18/2016 6:55:48 PM";
            // 
            // metarRichTxtBox
            // 
            this.metarRichTxtBox.Font = new System.Drawing.Font("Courier New", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.metarRichTxtBox.Location = new System.Drawing.Point(3, 50);
            this.metarRichTxtBox.Name = "metarRichTxtBox";
            this.metarRichTxtBox.Size = new System.Drawing.Size(1066, 323);
            this.metarRichTxtBox.TabIndex = 79;
            this.metarRichTxtBox.Text = "";
            // 
            // TabPage6
            // 
            this.TabPage6.Controls.Add(this.GroupBox19);
            this.TabPage6.Location = new System.Drawing.Point(4, 26);
            this.TabPage6.Name = "TabPage6";
            this.TabPage6.Padding = new System.Windows.Forms.Padding(3);
            this.TabPage6.Size = new System.Drawing.Size(1091, 648);
            this.TabPage6.TabIndex = 2;
            this.TabPage6.Text = "Upper Wind";
            this.TabPage6.UseVisualStyleBackColor = true;
            // 
            // GroupBox19
            // 
            this.GroupBox19.Controls.Add(this.tableLayoutPanel6);
            this.GroupBox19.Controls.Add(this.desForcastRichTxtBox);
            this.GroupBox19.Controls.Add(this.destIcaoLbl);
            this.GroupBox19.Location = new System.Drawing.Point(0, 0);
            this.GroupBox19.Name = "GroupBox19";
            this.GroupBox19.Size = new System.Drawing.Size(696, 372);
            this.GroupBox19.TabIndex = 0;
            this.GroupBox19.TabStop = false;
            this.GroupBox19.Text = "Descend Forcast";
            // 
            // tableLayoutPanel6
            // 
            this.tableLayoutPanel6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel6.AutoSize = true;
            this.tableLayoutPanel6.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel6.ColumnCount = 2;
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel6.Controls.Add(this.updateDesForcastBtn, 1, 0);
            this.tableLayoutPanel6.Controls.Add(this.desForcastLastUpdatedLbl, 0, 0);
            this.tableLayoutPanel6.Location = new System.Drawing.Point(272, 24);
            this.tableLayoutPanel6.Name = "tableLayoutPanel6";
            this.tableLayoutPanel6.RowCount = 1;
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel6.Size = new System.Drawing.Size(407, 41);
            this.tableLayoutPanel6.TabIndex = 83;
            // 
            // updateDesForcastBtn
            // 
            this.updateDesForcastBtn.AutoSize = true;
            this.updateDesForcastBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.updateDesForcastBtn.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.updateDesForcastBtn.Image = global::QSP.Properties.Resources.processing9;
            this.updateDesForcastBtn.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.updateDesForcastBtn.Location = new System.Drawing.Point(296, 0);
            this.updateDesForcastBtn.Margin = new System.Windows.Forms.Padding(0);
            this.updateDesForcastBtn.Name = "updateDesForcastBtn";
            this.updateDesForcastBtn.Size = new System.Drawing.Size(111, 41);
            this.updateDesForcastBtn.TabIndex = 80;
            this.updateDesForcastBtn.Text = "Refresh";
            this.updateDesForcastBtn.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.updateDesForcastBtn.UseVisualStyleBackColor = true;
            this.updateDesForcastBtn.Click += new System.EventHandler(this.UpdateDesForcast);
            // 
            // desForcastLastUpdatedLbl
            // 
            this.desForcastLastUpdatedLbl.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.desForcastLastUpdatedLbl.AutoSize = true;
            this.desForcastLastUpdatedLbl.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.desForcastLastUpdatedLbl.Location = new System.Drawing.Point(3, 9);
            this.desForcastLastUpdatedLbl.Margin = new System.Windows.Forms.Padding(3);
            this.desForcastLastUpdatedLbl.Name = "desForcastLastUpdatedLbl";
            this.desForcastLastUpdatedLbl.Size = new System.Drawing.Size(290, 23);
            this.desForcastLastUpdatedLbl.TabIndex = 81;
            this.desForcastLastUpdatedLbl.Text = "Last Updated : 7/18/2016 6:55:48 PM";
            // 
            // desForcastRichTxtBox
            // 
            this.desForcastRichTxtBox.Font = new System.Drawing.Font("Courier New", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.desForcastRichTxtBox.Location = new System.Drawing.Point(13, 71);
            this.desForcastRichTxtBox.Name = "desForcastRichTxtBox";
            this.desForcastRichTxtBox.Size = new System.Drawing.Size(666, 287);
            this.desForcastRichTxtBox.TabIndex = 1;
            this.desForcastRichTxtBox.Text = "";
            // 
            // Label86
            // 
            this.destIcaoLbl.AutoSize = true;
            this.destIcaoLbl.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.destIcaoLbl.Location = new System.Drawing.Point(20, 33);
            this.destIcaoLbl.Name = "Label86";
            this.destIcaoLbl.Size = new System.Drawing.Size(151, 23);
            this.destIcaoLbl.TabIndex = 0;
            this.destIcaoLbl.Text = "Destination : RCTP";
            // 
            // MiscInfoControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(120F, 120F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.Controls.Add(this.TabControl1);
            this.DoubleBuffered = true;
            this.Name = "MiscInfoControl";
            this.Size = new System.Drawing.Size(1102, 681);
            this.TabControl1.ResumeLayout(false);
            this.TabPage1.ResumeLayout(false);
            this.TabPage2.ResumeLayout(false);
            this.TabPage2.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.GroupBox17.ResumeLayout(false);
            this.tableLayoutPanel5.ResumeLayout(false);
            this.tableLayoutPanel5.PerformLayout();
            this.tableLayoutPanel4.ResumeLayout(false);
            this.tableLayoutPanel4.PerformLayout();
            this.GroupBox18.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel3.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.TabPage6.ResumeLayout(false);
            this.GroupBox19.ResumeLayout(false);
            this.GroupBox19.PerformLayout();
            this.tableLayoutPanel6.ResumeLayout(false);
            this.tableLayoutPanel6.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        internal System.Windows.Forms.TabControl TabControl1;
        internal System.Windows.Forms.TabPage TabPage1;
        private ToLdgModule.AirportMap.AirportMapControl airportMapControl;
        internal System.Windows.Forms.TabPage TabPage2;
        internal System.Windows.Forms.GroupBox GroupBox18;
        internal System.Windows.Forms.Button updateMetarBtn;
        internal System.Windows.Forms.RichTextBox metarRichTxtBox;
        internal System.Windows.Forms.GroupBox GroupBox17;
        internal System.Windows.Forms.TextBox metarToFindTxtBox;
        internal System.Windows.Forms.Button downloadMetarBtn;
        internal System.Windows.Forms.RichTextBox RichTextBox1;
        internal System.Windows.Forms.TabPage TabPage6;
        internal System.Windows.Forms.GroupBox GroupBox19;
        internal System.Windows.Forms.RichTextBox desForcastRichTxtBox;
        internal System.Windows.Forms.Label destIcaoLbl;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Label metarLastUpdatedLbl;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel5;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel6;
        internal System.Windows.Forms.Button updateDesForcastBtn;
        private System.Windows.Forms.Label desForcastLastUpdatedLbl;
    }
}
