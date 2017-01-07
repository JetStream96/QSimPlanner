using MetroFramework.Controls;
using QSP.UI.UserControls.AirportMap;

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
            this.airportMapControl = new AirportMapControl();
            this.TabPage2 = new System.Windows.Forms.TabPage();
            this.metarViewer = new QSP.UI.UserControls.MetarViewer();
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
            this.TabControl1.SelectedIndex = 1;
            this.TabControl1.Size = new System.Drawing.Size(1099, 700);
            this.TabControl1.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
            this.TabControl1.TabIndex = 44;
            this.TabControl1.UseSelectable = true;
            // 
            // TabPage1
            // 
            this.TabPage1.Controls.Add(this.airportMapControl);
            this.TabPage1.Location = new System.Drawing.Point(4, 38);
            this.TabPage1.Margin = new System.Windows.Forms.Padding(2);
            this.TabPage1.Name = "TabPage1";
            this.TabPage1.Padding = new System.Windows.Forms.Padding(2);
            this.TabPage1.Size = new System.Drawing.Size(1091, 658);
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
            this.airportMapControl.Size = new System.Drawing.Size(1061, 268);
            this.airportMapControl.StaticMapEnabled = false;
            this.airportMapControl.TabIndex = 0;
            // 
            // TabPage2
            // 
            this.TabPage2.Controls.Add(this.metarViewer);
            this.TabPage2.Controls.Add(this.tableLayoutPanel1);
            this.TabPage2.Location = new System.Drawing.Point(4, 38);
            this.TabPage2.Margin = new System.Windows.Forms.Padding(2);
            this.TabPage2.Name = "TabPage2";
            this.TabPage2.Padding = new System.Windows.Forms.Padding(2);
            this.TabPage2.Size = new System.Drawing.Size(1091, 658);
            this.TabPage2.TabIndex = 1;
            this.TabPage2.Text = "METAR/TAF";
            this.TabPage2.UseVisualStyleBackColor = true;
            // 
            // metarViewer
            // 
            this.metarViewer.AutoSize = true;
            this.metarViewer.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.metarViewer.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.metarViewer.Location = new System.Drawing.Point(10, 10);
            this.metarViewer.Name = "metarViewer";
            this.metarViewer.Size = new System.Drawing.Size(1076, 756);
            this.metarViewer.TabIndex = 82;
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
            this.TabPage6.Location = new System.Drawing.Point(4, 38);
            this.TabPage6.Margin = new System.Windows.Forms.Padding(2);
            this.TabPage6.Name = "TabPage6";
            this.TabPage6.Padding = new System.Windows.Forms.Padding(2);
            this.TabPage6.Size = new System.Drawing.Size(1091, 658);
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
            this.tableLayoutPanel7.Location = new System.Drawing.Point(10, 10);
            this.tableLayoutPanel7.Margin = new System.Windows.Forms.Padding(2);
            this.tableLayoutPanel7.Name = "tableLayoutPanel7";
            this.tableLayoutPanel7.RowCount = 2;
            this.tableLayoutPanel7.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel7.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel7.Size = new System.Drawing.Size(684, 395);
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
            this.panel1.Size = new System.Drawing.Size(678, 52);
            this.panel1.TabIndex = 1;
            // 
            // destIcaoLbl
            // 
            this.destIcaoLbl.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.destIcaoLbl.AutoSize = true;
            this.destIcaoLbl.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.destIcaoLbl.Location = new System.Drawing.Point(2, 12);
            this.destIcaoLbl.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.destIcaoLbl.Name = "destIcaoLbl";
            this.destIcaoLbl.Size = new System.Drawing.Size(151, 23);
            this.destIcaoLbl.TabIndex = 0;
            this.destIcaoLbl.Text = "Destination : RCTP";
            // 
            // updateDesForcastBtn
            // 
            this.updateDesForcastBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.updateDesForcastBtn.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.updateDesForcastBtn.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.updateDesForcastBtn.Location = new System.Drawing.Point(564, 2);
            this.updateDesForcastBtn.Margin = new System.Windows.Forms.Padding(2);
            this.updateDesForcastBtn.Name = "updateDesForcastBtn";
            this.updateDesForcastBtn.Size = new System.Drawing.Size(111, 41);
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
            this.desForcastLastUpdatedLbl.Location = new System.Drawing.Point(232, 12);
            this.desForcastLastUpdatedLbl.Margin = new System.Windows.Forms.Padding(2);
            this.desForcastLastUpdatedLbl.Name = "desForcastLastUpdatedLbl";
            this.desForcastLastUpdatedLbl.Size = new System.Drawing.Size(290, 23);
            this.desForcastLastUpdatedLbl.TabIndex = 81;
            this.desForcastLastUpdatedLbl.Text = "Last Updated : 7/18/2016 6:55:48 PM";
            // 
            // desForcastRichTxtBox
            // 
            this.desForcastRichTxtBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.desForcastRichTxtBox.Font = new System.Drawing.Font("Courier New", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.desForcastRichTxtBox.Location = new System.Drawing.Point(2, 58);
            this.desForcastRichTxtBox.Margin = new System.Windows.Forms.Padding(2);
            this.desForcastRichTxtBox.Name = "desForcastRichTxtBox";
            this.desForcastRichTxtBox.Size = new System.Drawing.Size(680, 335);
            this.desForcastRichTxtBox.TabIndex = 1;
            this.desForcastRichTxtBox.Text = "";
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
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "MiscInfoControl";
            this.Size = new System.Drawing.Size(1101, 702);
            this.TabControl1.ResumeLayout(false);
            this.TabPage1.ResumeLayout(false);
            this.TabPage1.PerformLayout();
            this.TabPage2.ResumeLayout(false);
            this.TabPage2.PerformLayout();
            this.TabPage6.ResumeLayout(false);
            this.tableLayoutPanel7.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        internal System.Windows.Forms.TabPage TabPage1;
        internal System.Windows.Forms.TabPage TabPage2;
        internal System.Windows.Forms.TabPage TabPage6;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        internal AirportMapControl airportMapControl;
        internal MetroTabControl TabControl1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel7;
        private System.Windows.Forms.Panel panel1;
        internal System.Windows.Forms.Label destIcaoLbl;
        internal System.Windows.Forms.Button updateDesForcastBtn;
        private System.Windows.Forms.Label desForcastLastUpdatedLbl;
        internal System.Windows.Forms.RichTextBox desForcastRichTxtBox;
        private MetarViewer metarViewer;
    }
}
