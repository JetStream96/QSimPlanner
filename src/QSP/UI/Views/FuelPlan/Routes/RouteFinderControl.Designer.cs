namespace QSP.UI.Views.FuelPlan.Routes
{
    partial class RouteFinderControl
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
            this.routeRichTxtBox = new System.Windows.Forms.RichTextBox();
            this.routeSummaryLbl = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.routeOptionBtn = new System.Windows.Forms.Button();
            this.showRouteActionsBtn = new System.Windows.Forms.Button();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.origRow = new QSP.UI.Views.FuelPlan.Routes.RouteFinderRow();
            this.destRow = new QSP.UI.Views.FuelPlan.Routes.RouteFinderRow();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.routeStatusLbl = new System.Windows.Forms.Label();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            this.SuspendLayout();
            // 
            // routeRichTxtBox
            // 
            this.routeRichTxtBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.routeRichTxtBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.routeRichTxtBox.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.routeRichTxtBox.ForeColor = System.Drawing.Color.Gray;
            this.routeRichTxtBox.Location = new System.Drawing.Point(2, 217);
            this.routeRichTxtBox.Margin = new System.Windows.Forms.Padding(2);
            this.routeRichTxtBox.MaximumSize = new System.Drawing.Size(10000, 81);
            this.routeRichTxtBox.MinimumSize = new System.Drawing.Size(4, 81);
            this.routeRichTxtBox.Name = "routeRichTxtBox";
            this.routeRichTxtBox.Size = new System.Drawing.Size(797, 81);
            this.routeRichTxtBox.TabIndex = 14;
            this.routeRichTxtBox.Text = "";
            this.routeRichTxtBox.TextChanged += new System.EventHandler(this.routeRichTxtBox_TextChanged);
            // 
            // routeSummaryLbl
            // 
            this.routeSummaryLbl.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.routeSummaryLbl.AutoSize = true;
            this.routeSummaryLbl.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.routeSummaryLbl.Location = new System.Drawing.Point(628, 181);
            this.routeSummaryLbl.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.routeSummaryLbl.Name = "routeSummaryLbl";
            this.routeSummaryLbl.Size = new System.Drawing.Size(171, 19);
            this.routeSummaryLbl.TabIndex = 18;
            this.routeSummaryLbl.Text = "Distance: 459 NM (+3.5%)";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(2, 200);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(38, 15);
            this.label3.TabIndex = 26;
            this.label3.Text = "Route";
            // 
            // routeOptionBtn
            // 
            this.routeOptionBtn.AutoSize = true;
            this.routeOptionBtn.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.routeOptionBtn.BackColor = System.Drawing.Color.DarkSlateGray;
            this.routeOptionBtn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.routeOptionBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.routeOptionBtn.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.routeOptionBtn.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.routeOptionBtn.Location = new System.Drawing.Point(2, 2);
            this.routeOptionBtn.Margin = new System.Windows.Forms.Padding(2);
            this.routeOptionBtn.Name = "routeOptionBtn";
            this.routeOptionBtn.Size = new System.Drawing.Size(70, 31);
            this.routeOptionBtn.TabIndex = 12;
            this.routeOptionBtn.Text = "Options";
            this.routeOptionBtn.UseVisualStyleBackColor = false;
            // 
            // showRouteActionsBtn
            // 
            this.showRouteActionsBtn.AutoSize = true;
            this.showRouteActionsBtn.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.showRouteActionsBtn.BackColor = System.Drawing.Color.DarkSlateGray;
            this.showRouteActionsBtn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.showRouteActionsBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.showRouteActionsBtn.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.showRouteActionsBtn.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.showRouteActionsBtn.Location = new System.Drawing.Point(76, 2);
            this.showRouteActionsBtn.Margin = new System.Windows.Forms.Padding(2);
            this.showRouteActionsBtn.Name = "showRouteActionsBtn";
            this.showRouteActionsBtn.Size = new System.Drawing.Size(136, 31);
            this.showRouteActionsBtn.TabIndex = 13;
            this.showRouteActionsBtn.Text = "Select an action  ▼";
            this.showRouteActionsBtn.UseVisualStyleBackColor = false;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.AutoSize = true;
            this.tableLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.origRow, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.destRow, 0, 1);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(795, 134);
            this.tableLayoutPanel1.TabIndex = 29;
            // 
            // origRow
            // 
            this.origRow.AutoSize = true;
            this.origRow.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.origRow.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.origRow.Location = new System.Drawing.Point(4, 4);
            this.origRow.Margin = new System.Windows.Forms.Padding(4);
            this.origRow.Name = "origRow";
            this.origRow.Size = new System.Drawing.Size(787, 59);
            this.origRow.TabIndex = 0;
            this.origRow.WaypointOptionEnabled = true;
            // 
            // destRow
            // 
            this.destRow.AutoSize = true;
            this.destRow.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.destRow.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.destRow.Location = new System.Drawing.Point(4, 71);
            this.destRow.Margin = new System.Windows.Forms.Padding(4);
            this.destRow.Name = "destRow";
            this.destRow.Size = new System.Drawing.Size(787, 59);
            this.destRow.TabIndex = 1;
            this.destRow.WaypointOptionEnabled = true;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.AutoSize = true;
            this.tableLayoutPanel2.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Controls.Add(this.routeOptionBtn, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.showRouteActionsBtn, 1, 0);
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 143);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(214, 35);
            this.tableLayoutPanel2.TabIndex = 30;
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.AutoSize = true;
            this.tableLayoutPanel4.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel4.ColumnCount = 1;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel4.Controls.Add(this.routeStatusLbl, 0, 5);
            this.tableLayoutPanel4.Controls.Add(this.label3, 0, 3);
            this.tableLayoutPanel4.Controls.Add(this.routeSummaryLbl, 0, 2);
            this.tableLayoutPanel4.Controls.Add(this.tableLayoutPanel1, 0, 0);
            this.tableLayoutPanel4.Controls.Add(this.routeRichTxtBox, 0, 4);
            this.tableLayoutPanel4.Controls.Add(this.tableLayoutPanel2, 0, 1);
            this.tableLayoutPanel4.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 6;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel4.Size = new System.Drawing.Size(801, 320);
            this.tableLayoutPanel4.TabIndex = 32;
            // 
            // routeStatusLbl
            // 
            this.routeStatusLbl.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.routeStatusLbl.AutoSize = true;
            this.routeStatusLbl.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.routeStatusLbl.ForeColor = System.Drawing.Color.Gray;
            this.routeStatusLbl.Location = new System.Drawing.Point(556, 300);
            this.routeStatusLbl.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.routeStatusLbl.Name = "routeStatusLbl";
            this.routeStatusLbl.Size = new System.Drawing.Size(243, 20);
            this.routeStatusLbl.TabIndex = 31;
            this.routeStatusLbl.Text = "This route has not been analyzed.";
            // 
            // RouteFinderControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.Controls.Add(this.tableLayoutPanel4);
            this.DoubleBuffered = true;
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "RouteFinderControl";
            this.Size = new System.Drawing.Size(804, 323);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.tableLayoutPanel4.ResumeLayout(false);
            this.tableLayoutPanel4.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.RichTextBox routeRichTxtBox;
        private System.Windows.Forms.Label routeSummaryLbl;
        private System.Windows.Forms.Label label3;
        public System.Windows.Forms.Button routeOptionBtn;
        public System.Windows.Forms.Button showRouteActionsBtn;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private RouteFinderRow origRow;
        private RouteFinderRow destRow;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private System.Windows.Forms.Label routeStatusLbl;
    }
}
