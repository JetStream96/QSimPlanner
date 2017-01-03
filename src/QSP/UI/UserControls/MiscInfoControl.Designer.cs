using MetroFramework.Controls;

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
            this.TabControl1 = new MetroFramework.Controls.MetroTabControl();
            this.TabPage1 = new System.Windows.Forms.TabPage();
            this.airportMapControl = new QSP.UI.ToLdgModule.AirportMap.AirportMapControl();
            this.TabPage2 = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel5 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.metarToFindTxtBox = new System.Windows.Forms.TextBox();
            this.metarLastUpdatedLbl = new System.Windows.Forms.Label();
            this.downloadAllBtn = new System.Windows.Forms.Button();
            this.matarTafRichTxtBox = new System.Windows.Forms.RichTextBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.TabPage6 = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel7 = new System.Windows.Forms.TableLayoutPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.destIcaoLbl = new System.Windows.Forms.Label();
            this.updateDesForcastBtn = new System.Windows.Forms.Button();
            this.desForcastLastUpdatedLbl = new System.Windows.Forms.Label();
            this.desForcastRichTxtBox = new System.Windows.Forms.RichTextBox();
            this.TabControl1.SuspendLayout();
            this.TabPage1.SuspendLayout();
            this.TabPage2.SuspendLayout();
            this.tableLayoutPanel5.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            this.TabPage6.SuspendLayout();
            this.tableLayoutPanel7.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // TabControl1
            // 
            this.TabControl1.Controls.Add(this.TabPage1);
            this.TabControl1.Controls.Add(this.TabPage2);
            this.TabControl1.Controls.Add(this.TabPage6);
            this.TabControl1.FontWeight = MetroFramework.MetroTabControlWeight.Regular;
            this.TabControl1.Location = new System.Drawing.Point(0, 0);
            this.TabControl1.Margin = new System.Windows.Forms.Padding(2);
            this.TabControl1.Name = "TabControl1";
            this.TabControl1.SelectedIndex = 0;
            this.TabControl1.Size = new System.Drawing.Size(879, 704);
            this.TabControl1.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
            this.TabControl1.TabIndex = 44;
            this.TabControl1.UseSelectable = true;
            // 
            // TabPage1
            // 
            this.TabPage1.Controls.Add(this.airportMapControl);
            this.TabPage1.Location = new System.Drawing.Point(4, 44);
            this.TabPage1.Margin = new System.Windows.Forms.Padding(2);
            this.TabPage1.Name = "TabPage1";
            this.TabPage1.Padding = new System.Windows.Forms.Padding(2);
            this.TabPage1.Size = new System.Drawing.Size(871, 656);
            this.TabPage1.TabIndex = 0;
            this.TabPage1.Text = "Airport Data";
            this.TabPage1.UseVisualStyleBackColor = true;
            // 
            // airportMapControl
            // 
            this.airportMapControl.Airports = null;
            this.airportMapControl.AutoSize = true;
            this.airportMapControl.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.airportMapControl.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.airportMapControl.BrowserEnabled = false;
            this.airportMapControl.Location = new System.Drawing.Point(0, 0);
            this.airportMapControl.Margin = new System.Windows.Forms.Padding(2);
            this.airportMapControl.Name = "airportMapControl";
            this.airportMapControl.Size = new System.Drawing.Size(850, 772);
            this.airportMapControl.StaticMapEnabled = false;
            this.airportMapControl.TabIndex = 0;
            // 
            // TabPage2
            // 
            this.TabPage2.Controls.Add(this.tableLayoutPanel5);
            this.TabPage2.Controls.Add(this.tableLayoutPanel1);
            this.TabPage2.Location = new System.Drawing.Point(4, 44);
            this.TabPage2.Margin = new System.Windows.Forms.Padding(2);
            this.TabPage2.Name = "TabPage2";
            this.TabPage2.Padding = new System.Windows.Forms.Padding(2);
            this.TabPage2.Size = new System.Drawing.Size(871, 656);
            this.TabPage2.TabIndex = 1;
            this.TabPage2.Text = "METAR/TAF";
            this.TabPage2.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel5
            // 
            this.tableLayoutPanel5.ColumnCount = 1;
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel5.Controls.Add(this.tableLayoutPanel4, 0, 0);
            this.tableLayoutPanel5.Controls.Add(this.matarTafRichTxtBox, 0, 1);
            this.tableLayoutPanel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel5.Location = new System.Drawing.Point(2, 2);
            this.tableLayoutPanel5.Margin = new System.Windows.Forms.Padding(2);
            this.tableLayoutPanel5.Name = "tableLayoutPanel5";
            this.tableLayoutPanel5.RowCount = 2;
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 16F));
            this.tableLayoutPanel5.Size = new System.Drawing.Size(867, 652);
            this.tableLayoutPanel5.TabIndex = 80;
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.AutoSize = true;
            this.tableLayoutPanel4.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel4.ColumnCount = 5;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 16F));
            this.tableLayoutPanel4.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel4.Controls.Add(this.button1, 1, 0);
            this.tableLayoutPanel4.Controls.Add(this.metarToFindTxtBox, 1, 0);
            this.tableLayoutPanel4.Controls.Add(this.metarLastUpdatedLbl, 4, 0);
            this.tableLayoutPanel4.Controls.Add(this.downloadAllBtn, 2, 0);
            this.tableLayoutPanel4.Location = new System.Drawing.Point(2, 2);
            this.tableLayoutPanel4.Margin = new System.Windows.Forms.Padding(2);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 1;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel4.Size = new System.Drawing.Size(646, 31);
            this.tableLayoutPanel4.TabIndex = 79;
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(2, 6);
            this.label1.Margin = new System.Windows.Forms.Padding(2);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(42, 19);
            this.label1.TabIndex = 83;
            this.label1.Text = "ICAO";
            // 
            // button1
            // 
            this.button1.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.button1.AutoSize = true;
            this.button1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.button1.BackColor = System.Drawing.SystemColors.Control;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.Location = new System.Drawing.Point(133, 2);
            this.button1.Margin = new System.Windows.Forms.Padding(2);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(131, 27);
            this.button1.TabIndex = 82;
            this.button1.Text = "View Selected Airport";
            this.button1.UseVisualStyleBackColor = false;
            // 
            // metarToFindTxtBox
            // 
            this.metarToFindTxtBox.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.metarToFindTxtBox.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.metarToFindTxtBox.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.metarToFindTxtBox.Location = new System.Drawing.Point(48, 2);
            this.metarToFindTxtBox.Margin = new System.Windows.Forms.Padding(2);
            this.metarToFindTxtBox.Name = "metarToFindTxtBox";
            this.metarToFindTxtBox.Size = new System.Drawing.Size(81, 26);
            this.metarToFindTxtBox.TabIndex = 77;
            this.metarToFindTxtBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // metarLastUpdatedLbl
            // 
            this.metarLastUpdatedLbl.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.metarLastUpdatedLbl.AutoSize = true;
            this.metarLastUpdatedLbl.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.metarLastUpdatedLbl.Location = new System.Drawing.Point(401, 6);
            this.metarLastUpdatedLbl.Margin = new System.Windows.Forms.Padding(2);
            this.metarLastUpdatedLbl.Name = "metarLastUpdatedLbl";
            this.metarLastUpdatedLbl.Size = new System.Drawing.Size(243, 19);
            this.metarLastUpdatedLbl.TabIndex = 81;
            this.metarLastUpdatedLbl.Text = "Last Updated : 7/18/2016 6:55:48 PM";
            // 
            // downloadAllBtn
            // 
            this.downloadAllBtn.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.downloadAllBtn.AutoSize = true;
            this.downloadAllBtn.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.downloadAllBtn.BackColor = System.Drawing.SystemColors.Control;
            this.downloadAllBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.downloadAllBtn.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.downloadAllBtn.Location = new System.Drawing.Point(268, 2);
            this.downloadAllBtn.Margin = new System.Windows.Forms.Padding(2);
            this.downloadAllBtn.Name = "downloadAllBtn";
            this.downloadAllBtn.Size = new System.Drawing.Size(129, 27);
            this.downloadAllBtn.TabIndex = 78;
            this.downloadAllBtn.Text = "View DEP/ARR/ATLN";
            this.downloadAllBtn.UseVisualStyleBackColor = false;
            // 
            // matarTafRichTxtBox
            // 
            this.matarTafRichTxtBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.matarTafRichTxtBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.matarTafRichTxtBox.Font = new System.Drawing.Font("Courier New", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.matarTafRichTxtBox.Location = new System.Drawing.Point(2, 37);
            this.matarTafRichTxtBox.Margin = new System.Windows.Forms.Padding(2);
            this.matarTafRichTxtBox.Name = "matarTafRichTxtBox";
            this.matarTafRichTxtBox.Size = new System.Drawing.Size(863, 613);
            this.matarTafRichTxtBox.TabIndex = 0;
            this.matarTafRichTxtBox.Text = "";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.AutoSize = true;
            this.tableLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(2);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(0, 0);
            this.tableLayoutPanel1.TabIndex = 81;
            // 
            // TabPage6
            // 
            this.TabPage6.Controls.Add(this.tableLayoutPanel7);
            this.TabPage6.Location = new System.Drawing.Point(4, 44);
            this.TabPage6.Margin = new System.Windows.Forms.Padding(2);
            this.TabPage6.Name = "TabPage6";
            this.TabPage6.Padding = new System.Windows.Forms.Padding(2);
            this.TabPage6.Size = new System.Drawing.Size(871, 656);
            this.TabPage6.TabIndex = 2;
            this.TabPage6.Text = "Des. Forcast";
            this.TabPage6.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel7
            // 
            this.tableLayoutPanel7.ColumnCount = 1;
            this.tableLayoutPanel7.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel7.Controls.Add(this.panel1, 0, 0);
            this.tableLayoutPanel7.Controls.Add(this.desForcastRichTxtBox, 0, 1);
            this.tableLayoutPanel7.Location = new System.Drawing.Point(8, 8);
            this.tableLayoutPanel7.Margin = new System.Windows.Forms.Padding(2);
            this.tableLayoutPanel7.Name = "tableLayoutPanel7";
            this.tableLayoutPanel7.RowCount = 2;
            this.tableLayoutPanel7.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel7.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel7.Size = new System.Drawing.Size(547, 316);
            this.tableLayoutPanel7.TabIndex = 2;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.destIcaoLbl);
            this.panel1.Controls.Add(this.updateDesForcastBtn);
            this.panel1.Controls.Add(this.desForcastLastUpdatedLbl);
            this.panel1.Location = new System.Drawing.Point(2, 2);
            this.panel1.Margin = new System.Windows.Forms.Padding(2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(542, 42);
            this.panel1.TabIndex = 1;
            // 
            // destIcaoLbl
            // 
            this.destIcaoLbl.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.destIcaoLbl.AutoSize = true;
            this.destIcaoLbl.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.destIcaoLbl.Location = new System.Drawing.Point(2, 10);
            this.destIcaoLbl.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.destIcaoLbl.Name = "destIcaoLbl";
            this.destIcaoLbl.Size = new System.Drawing.Size(122, 19);
            this.destIcaoLbl.TabIndex = 0;
            this.destIcaoLbl.Text = "Destination : RCTP";
            // 
            // updateDesForcastBtn
            // 
            this.updateDesForcastBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.updateDesForcastBtn.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.updateDesForcastBtn.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.updateDesForcastBtn.Location = new System.Drawing.Point(451, 2);
            this.updateDesForcastBtn.Margin = new System.Windows.Forms.Padding(2);
            this.updateDesForcastBtn.Name = "updateDesForcastBtn";
            this.updateDesForcastBtn.Size = new System.Drawing.Size(89, 33);
            this.updateDesForcastBtn.TabIndex = 80;
            this.updateDesForcastBtn.Text = "Refresh";
            this.updateDesForcastBtn.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.updateDesForcastBtn.UseVisualStyleBackColor = true;
            // 
            // desForcastLastUpdatedLbl
            // 
            this.desForcastLastUpdatedLbl.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.desForcastLastUpdatedLbl.AutoSize = true;
            this.desForcastLastUpdatedLbl.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.desForcastLastUpdatedLbl.Location = new System.Drawing.Point(186, 10);
            this.desForcastLastUpdatedLbl.Margin = new System.Windows.Forms.Padding(2);
            this.desForcastLastUpdatedLbl.Name = "desForcastLastUpdatedLbl";
            this.desForcastLastUpdatedLbl.Size = new System.Drawing.Size(243, 19);
            this.desForcastLastUpdatedLbl.TabIndex = 81;
            this.desForcastLastUpdatedLbl.Text = "Last Updated : 7/18/2016 6:55:48 PM";
            // 
            // desForcastRichTxtBox
            // 
            this.desForcastRichTxtBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.desForcastRichTxtBox.Font = new System.Drawing.Font("Courier New", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.desForcastRichTxtBox.Location = new System.Drawing.Point(2, 48);
            this.desForcastRichTxtBox.Margin = new System.Windows.Forms.Padding(2);
            this.desForcastRichTxtBox.Name = "desForcastRichTxtBox";
            this.desForcastRichTxtBox.Size = new System.Drawing.Size(543, 266);
            this.desForcastRichTxtBox.TabIndex = 1;
            this.desForcastRichTxtBox.Text = "";
            // 
            // MiscInfoControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.Controls.Add(this.TabControl1);
            this.DoubleBuffered = true;
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "MiscInfoControl";
            this.Size = new System.Drawing.Size(881, 706);
            this.TabControl1.ResumeLayout(false);
            this.TabPage1.ResumeLayout(false);
            this.TabPage1.PerformLayout();
            this.TabPage2.ResumeLayout(false);
            this.TabPage2.PerformLayout();
            this.tableLayoutPanel5.ResumeLayout(false);
            this.tableLayoutPanel5.PerformLayout();
            this.tableLayoutPanel4.ResumeLayout(false);
            this.tableLayoutPanel4.PerformLayout();
            this.TabPage6.ResumeLayout(false);
            this.tableLayoutPanel7.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        internal System.Windows.Forms.TabPage TabPage1;
        internal System.Windows.Forms.TabPage TabPage2;
        internal System.Windows.Forms.TextBox metarToFindTxtBox;
        internal System.Windows.Forms.Button downloadAllBtn;
        internal System.Windows.Forms.RichTextBox matarTafRichTxtBox;
        internal System.Windows.Forms.TabPage TabPage6;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label metarLastUpdatedLbl;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel5;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        internal ToLdgModule.AirportMap.AirportMapControl airportMapControl;
        private System.Windows.Forms.Label label1;
        internal System.Windows.Forms.Button button1;
        internal MetroTabControl TabControl1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel7;
        private System.Windows.Forms.Panel panel1;
        internal System.Windows.Forms.Label destIcaoLbl;
        internal System.Windows.Forms.Button updateDesForcastBtn;
        private System.Windows.Forms.Label desForcastLastUpdatedLbl;
        internal System.Windows.Forms.RichTextBox desForcastRichTxtBox;
    }
}
