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
            this.TabPage2 = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.GroupBox17 = new System.Windows.Forms.GroupBox();
            this.metarToFindTxtBox = new System.Windows.Forms.TextBox();
            this.downloadMetarBtn = new System.Windows.Forms.Button();
            this.RichTextBox1 = new System.Windows.Forms.RichTextBox();
            this.GroupBox18 = new System.Windows.Forms.GroupBox();
            this.UpdateAll_Btn = new System.Windows.Forms.Button();
            this.RichTextBox2 = new System.Windows.Forms.RichTextBox();
            this.TabPage6 = new System.Windows.Forms.TabPage();
            this.GroupBox19 = new System.Windows.Forms.GroupBox();
            this.DesForcast_RTextBox = new System.Windows.Forms.RichTextBox();
            this.Label86 = new System.Windows.Forms.Label();
            this.airportMapControl = new QSP.UI.ToLdgModule.AirportMap.AirportMapControl();
            this.TabControl1.SuspendLayout();
            this.TabPage1.SuspendLayout();
            this.TabPage2.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.GroupBox17.SuspendLayout();
            this.GroupBox18.SuspendLayout();
            this.TabPage6.SuspendLayout();
            this.GroupBox19.SuspendLayout();
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
            this.TabControl1.Size = new System.Drawing.Size(1099, 778);
            this.TabControl1.TabIndex = 44;
            // 
            // TabPage1
            // 
            this.TabPage1.Controls.Add(this.airportMapControl);
            this.TabPage1.Location = new System.Drawing.Point(4, 26);
            this.TabPage1.Name = "TabPage1";
            this.TabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.TabPage1.Size = new System.Drawing.Size(1091, 748);
            this.TabPage1.TabIndex = 0;
            this.TabPage1.Text = "Airport Data";
            this.TabPage1.UseVisualStyleBackColor = true;
            // 
            // TabPage2
            // 
            this.TabPage2.Controls.Add(this.tableLayoutPanel1);
            this.TabPage2.Location = new System.Drawing.Point(4, 26);
            this.TabPage2.Name = "TabPage2";
            this.TabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.TabPage2.Size = new System.Drawing.Size(1091, 748);
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
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1088, 750);
            this.tableLayoutPanel1.TabIndex = 81;
            // 
            // GroupBox17
            // 
            this.GroupBox17.Controls.Add(this.metarToFindTxtBox);
            this.GroupBox17.Controls.Add(this.downloadMetarBtn);
            this.GroupBox17.Controls.Add(this.RichTextBox1);
            this.GroupBox17.Location = new System.Drawing.Point(3, 3);
            this.GroupBox17.Name = "GroupBox17";
            this.GroupBox17.Size = new System.Drawing.Size(1082, 229);
            this.GroupBox17.TabIndex = 79;
            this.GroupBox17.TabStop = false;
            this.GroupBox17.Text = "Search";
            // 
            // metarToFindTxtBox
            // 
            this.metarToFindTxtBox.Font = new System.Drawing.Font("Arial", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.metarToFindTxtBox.Location = new System.Drawing.Point(10, 21);
            this.metarToFindTxtBox.Name = "metarToFindTxtBox";
            this.metarToFindTxtBox.Size = new System.Drawing.Size(100, 27);
            this.metarToFindTxtBox.TabIndex = 77;
            // 
            // downloadMetarBtn
            // 
            this.downloadMetarBtn.BackColor = System.Drawing.SystemColors.Control;
            this.downloadMetarBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.downloadMetarBtn.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.downloadMetarBtn.Location = new System.Drawing.Point(145, 20);
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
            this.RichTextBox1.Location = new System.Drawing.Point(10, 64);
            this.RichTextBox1.Name = "RichTextBox1";
            this.RichTextBox1.Size = new System.Drawing.Size(1066, 156);
            this.RichTextBox1.TabIndex = 0;
            this.RichTextBox1.Text = "";
            // 
            // GroupBox18
            // 
            this.GroupBox18.Controls.Add(this.UpdateAll_Btn);
            this.GroupBox18.Controls.Add(this.RichTextBox2);
            this.GroupBox18.Location = new System.Drawing.Point(3, 238);
            this.GroupBox18.Name = "GroupBox18";
            this.GroupBox18.Size = new System.Drawing.Size(1082, 509);
            this.GroupBox18.TabIndex = 80;
            this.GroupBox18.TabStop = false;
            this.GroupBox18.Text = "Monitor";
            // 
            // UpdateAll_Btn
            // 
            this.UpdateAll_Btn.BackgroundImage = global::QSP.Properties.Resources.processing9;
            this.UpdateAll_Btn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.UpdateAll_Btn.Location = new System.Drawing.Point(1043, 22);
            this.UpdateAll_Btn.Name = "UpdateAll_Btn";
            this.UpdateAll_Btn.Size = new System.Drawing.Size(33, 33);
            this.UpdateAll_Btn.TabIndex = 80;
            this.UpdateAll_Btn.UseVisualStyleBackColor = true;
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
            // TabPage6
            // 
            this.TabPage6.Controls.Add(this.GroupBox19);
            this.TabPage6.Location = new System.Drawing.Point(4, 26);
            this.TabPage6.Name = "TabPage6";
            this.TabPage6.Padding = new System.Windows.Forms.Padding(3);
            this.TabPage6.Size = new System.Drawing.Size(1091, 748);
            this.TabPage6.TabIndex = 2;
            this.TabPage6.Text = "Upper Wind";
            this.TabPage6.UseVisualStyleBackColor = true;
            // 
            // GroupBox19
            // 
            this.GroupBox19.Controls.Add(this.DesForcast_RTextBox);
            this.GroupBox19.Controls.Add(this.Label86);
            this.GroupBox19.Location = new System.Drawing.Point(0, 0);
            this.GroupBox19.Name = "GroupBox19";
            this.GroupBox19.Size = new System.Drawing.Size(685, 356);
            this.GroupBox19.TabIndex = 0;
            this.GroupBox19.TabStop = false;
            this.GroupBox19.Text = "Descend Forcast";
            // 
            // DesForcast_RTextBox
            // 
            this.DesForcast_RTextBox.Font = new System.Drawing.Font("Courier New", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DesForcast_RTextBox.Location = new System.Drawing.Point(13, 60);
            this.DesForcast_RTextBox.Name = "DesForcast_RTextBox";
            this.DesForcast_RTextBox.Size = new System.Drawing.Size(666, 287);
            this.DesForcast_RTextBox.TabIndex = 1;
            this.DesForcast_RTextBox.Text = "";
            // 
            // Label86
            // 
            this.Label86.AutoSize = true;
            this.Label86.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label86.Location = new System.Drawing.Point(11, 28);
            this.Label86.Name = "Label86";
            this.Label86.Size = new System.Drawing.Size(106, 23);
            this.Label86.TabIndex = 0;
            this.Label86.Text = "DEST / RCTP";
            // 
            // airportMapControl
            // 
            this.airportMapControl.Airports = null;
            this.airportMapControl.Altn = null;
            this.airportMapControl.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.airportMapControl.BrowserEnabled = false;
            this.airportMapControl.Dest = null;
            this.airportMapControl.Location = new System.Drawing.Point(0, 0);
            this.airportMapControl.Name = "airportMapControl";
            this.airportMapControl.Orig = null;
            this.airportMapControl.Size = new System.Drawing.Size(1024, 670);
            this.airportMapControl.StaticMapEnabled = false;
            this.airportMapControl.TabIndex = 0;
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
            this.Size = new System.Drawing.Size(1102, 781);
            this.TabControl1.ResumeLayout(false);
            this.TabPage1.ResumeLayout(false);
            this.TabPage2.ResumeLayout(false);
            this.TabPage2.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.GroupBox17.ResumeLayout(false);
            this.GroupBox17.PerformLayout();
            this.GroupBox18.ResumeLayout(false);
            this.TabPage6.ResumeLayout(false);
            this.GroupBox19.ResumeLayout(false);
            this.GroupBox19.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        internal System.Windows.Forms.TabControl TabControl1;
        internal System.Windows.Forms.TabPage TabPage1;
        private ToLdgModule.AirportMap.AirportMapControl airportMapControl;
        internal System.Windows.Forms.TabPage TabPage2;
        internal System.Windows.Forms.GroupBox GroupBox18;
        internal System.Windows.Forms.Button UpdateAll_Btn;
        internal System.Windows.Forms.RichTextBox RichTextBox2;
        internal System.Windows.Forms.GroupBox GroupBox17;
        internal System.Windows.Forms.TextBox metarToFindTxtBox;
        internal System.Windows.Forms.Button downloadMetarBtn;
        internal System.Windows.Forms.RichTextBox RichTextBox1;
        internal System.Windows.Forms.TabPage TabPage6;
        internal System.Windows.Forms.GroupBox GroupBox19;
        internal System.Windows.Forms.RichTextBox DesForcast_RTextBox;
        internal System.Windows.Forms.Label Label86;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
    }
}
