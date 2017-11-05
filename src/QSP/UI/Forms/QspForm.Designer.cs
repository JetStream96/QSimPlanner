using QSP.UI.UserControls;
using QSP.UI.Views;

namespace QSP.UI.Forms
{
    partial class QspForm
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.navDataStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.ToolStripStatusLabel3 = new System.Windows.Forms.ToolStripStatusLabel();
            this.trackStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.ToolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.windDataStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.StatusStrip1 = new System.Windows.Forms.StatusStrip();
            this.panel1 = new PanelSilentScrollbar();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.navBar = new NavBar();
            this.panel2 = new System.Windows.Forms.Panel();
            this.StatusStrip1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // navDataStatusLabel
            // 
            this.navDataStatusLabel.BackColor = System.Drawing.Color.Transparent;
            this.navDataStatusLabel.Image = global::QSP.Properties.Resources.GreenLight;
            this.navDataStatusLabel.Name = "navDataStatusLabel";
            this.navDataStatusLabel.Size = new System.Drawing.Size(148, 20);
            this.navDataStatusLabel.Text = "Nav Data: Loaded";
            this.navDataStatusLabel.ToolTipText = "Click to view options";
            // 
            // ToolStripStatusLabel3
            // 
            this.ToolStripStatusLabel3.BackColor = System.Drawing.Color.Transparent;
            this.ToolStripStatusLabel3.Name = "ToolStripStatusLabel3";
            this.ToolStripStatusLabel3.Size = new System.Drawing.Size(45, 20);
            this.ToolStripStatusLabel3.Text = "         ";
            // 
            // trackStatusLabel
            // 
            this.trackStatusLabel.BackColor = System.Drawing.Color.Transparent;
            this.trackStatusLabel.Image = global::QSP.Properties.Resources.YellowLight;
            this.trackStatusLabel.Name = "trackStatusLabel";
            this.trackStatusLabel.Size = new System.Drawing.Size(166, 20);
            this.trackStatusLabel.Text = "Tracks: Patially ready";
            // 
            // ToolStripStatusLabel2
            // 
            this.ToolStripStatusLabel2.BackColor = System.Drawing.Color.Transparent;
            this.ToolStripStatusLabel2.Name = "ToolStripStatusLabel2";
            this.ToolStripStatusLabel2.Size = new System.Drawing.Size(49, 20);
            this.ToolStripStatusLabel2.Text = "          ";
            // 
            // windDataStatusLabel
            // 
            this.windDataStatusLabel.BackColor = System.Drawing.Color.Transparent;
            this.windDataStatusLabel.Image = global::QSP.Properties.Resources.GreenLight;
            this.windDataStatusLabel.Name = "windDataStatusLabel";
            this.windDataStatusLabel.Size = new System.Drawing.Size(153, 20);
            this.windDataStatusLabel.Text = "Wind Aloft : Ready";
            // 
            // StatusStrip1
            // 
            this.StatusStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.StatusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.navDataStatusLabel,
            this.ToolStripStatusLabel3,
            this.trackStatusLabel,
            this.ToolStripStatusLabel2,
            this.windDataStatusLabel});
            this.StatusStrip1.Location = new System.Drawing.Point(0, 830);
            this.StatusStrip1.Name = "StatusStrip1";
            this.StatusStrip1.Size = new System.Drawing.Size(1182, 25);
            this.StatusStrip1.TabIndex = 40;
            this.StatusStrip1.Text = "StatusStrip1";
            // 
            // panel1
            // 
            this.panel1.AutoScroll = true;
            this.panel1.Controls.Add(this.tableLayoutPanel1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1182, 830);
            this.panel1.TabIndex = 41;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.AutoSize = true;
            this.tableLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.navBar, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.panel2, 1, 0);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(10, 10);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(125, 304);
            this.tableLayoutPanel1.TabIndex = 8;
            // 
            // navBar
            // 
            this.navBar.AutoSize = true;
            this.navBar.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.navBar.BackColor = System.Drawing.Color.White;
            this.navBar.Location = new System.Drawing.Point(0, 0);
            this.navBar.Margin = new System.Windows.Forms.Padding(0);
            this.navBar.Name = "navBar";
            this.navBar.Size = new System.Drawing.Size(120, 304);
            this.navBar.TabIndex = 9;
            // 
            // panel2
            // 
            this.panel2.AutoSize = true;
            this.panel2.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.panel2.Location = new System.Drawing.Point(125, 0);
            this.panel2.Margin = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(0, 0);
            this.panel2.TabIndex = 8;
            // 
            // QspForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(120F, 120F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.ClientSize = new System.Drawing.Size(1182, 855);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.StatusStrip1);
            this.DoubleBuffered = true;
            this.Name = "QspForm";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "QSimPlanner";
            this.StatusStrip1.ResumeLayout(false);
            this.StatusStrip1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        internal System.Windows.Forms.ToolStripStatusLabel navDataStatusLabel;
        internal System.Windows.Forms.ToolStripStatusLabel ToolStripStatusLabel3;
        internal System.Windows.Forms.ToolStripStatusLabel trackStatusLabel;
        internal System.Windows.Forms.ToolStripStatusLabel ToolStripStatusLabel2;
        internal System.Windows.Forms.ToolStripStatusLabel windDataStatusLabel;
        private System.Windows.Forms.StatusStrip StatusStrip1;
        private PanelSilentScrollbar panel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel panel2;
        private NavBar navBar;
    }
}