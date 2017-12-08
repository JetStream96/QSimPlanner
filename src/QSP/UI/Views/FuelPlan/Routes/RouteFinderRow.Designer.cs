namespace QSP.UI.Views.FuelPlan.Routes
{
    partial class RouteFinderRow
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
            this.typeLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.typeComboBox = new QSP.UI.Views.ComboBoxWithBorder();
            this.fromToLbl = new System.Windows.Forms.Label();
            this.OptionControl = new QSP.UI.Views.FuelPlan.Routes.FinderOptionControl();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.wptLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.identTxtBox = new System.Windows.Forms.TextBox();
            this.wptComboBox = new QSP.UI.Views.ComboBoxWithBorder();
            this.label2 = new System.Windows.Forms.Label();
            this.fromIdentLbl = new System.Windows.Forms.Label();
            this.typeLayoutPanel.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.wptLayoutPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // typeLayoutPanel
            // 
            this.typeLayoutPanel.AutoSize = true;
            this.typeLayoutPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.typeLayoutPanel.ColumnCount = 1;
            this.typeLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.typeLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.typeLayoutPanel.Controls.Add(this.typeComboBox, 0, 1);
            this.typeLayoutPanel.Controls.Add(this.fromToLbl, 0, 0);
            this.typeLayoutPanel.Location = new System.Drawing.Point(3, 3);
            this.typeLayoutPanel.Name = "typeLayoutPanel";
            this.typeLayoutPanel.RowCount = 2;
            this.typeLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.typeLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.typeLayoutPanel.Size = new System.Drawing.Size(100, 46);
            this.typeLayoutPanel.TabIndex = 28;
            // 
            // typeComboBox
            // 
            this.typeComboBox.BorderColor = System.Drawing.Color.DimGray;
            this.typeComboBox.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.typeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.typeComboBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.typeComboBox.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.typeComboBox.FormattingEnabled = true;
            this.typeComboBox.Items.AddRange(new object[] {
            "Airport",
            "Waypoint"});
            this.typeComboBox.Location = new System.Drawing.Point(2, 17);
            this.typeComboBox.Margin = new System.Windows.Forms.Padding(2);
            this.typeComboBox.Name = "typeComboBox";
            this.typeComboBox.Size = new System.Drawing.Size(96, 27);
            this.typeComboBox.TabIndex = 2;
            this.typeComboBox.SelectedIndexChanged += new System.EventHandler(this.typeComboBox_SelectedIndexChanged);
            // 
            // fromToLbl
            // 
            this.fromToLbl.AutoSize = true;
            this.fromToLbl.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.fromToLbl.Location = new System.Drawing.Point(2, 0);
            this.fromToLbl.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.fromToLbl.Name = "fromToLbl";
            this.fromToLbl.Size = new System.Drawing.Size(35, 15);
            this.fromToLbl.TabIndex = 1;
            this.fromToLbl.Text = "From";
            // 
            // finderOptionControl
            // 
            this.OptionControl.AutoSize = true;
            this.OptionControl.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.OptionControl.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.OptionControl.Icao = "";
            this.OptionControl.Location = new System.Drawing.Point(109, 3);
            this.OptionControl.Name = "finderOptionControl";
            this.OptionControl.SelectedProcedureText = "";
            this.OptionControl.SelectedRwy = "";
            this.OptionControl.Size = new System.Drawing.Size(354, 50);
            this.OptionControl.TabIndex = 29;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.AutoSize = true;
            this.tableLayoutPanel2.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel2.ColumnCount = 3;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Controls.Add(this.wptLayoutPanel, 2, 0);
            this.tableLayoutPanel2.Controls.Add(this.typeLayoutPanel, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.OptionControl, 1, 0);
            this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 56F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(784, 56);
            this.tableLayoutPanel2.TabIndex = 30;
            // 
            // wptLayoutPanel
            // 
            this.wptLayoutPanel.AutoSize = true;
            this.wptLayoutPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.wptLayoutPanel.ColumnCount = 2;
            this.wptLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.wptLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.wptLayoutPanel.Controls.Add(this.identTxtBox, 0, 1);
            this.wptLayoutPanel.Controls.Add(this.wptComboBox, 1, 1);
            this.wptLayoutPanel.Controls.Add(this.label2, 1, 0);
            this.wptLayoutPanel.Controls.Add(this.fromIdentLbl, 0, 0);
            this.wptLayoutPanel.Location = new System.Drawing.Point(469, 3);
            this.wptLayoutPanel.Name = "wptLayoutPanel";
            this.wptLayoutPanel.RowCount = 2;
            this.wptLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.wptLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.wptLayoutPanel.Size = new System.Drawing.Size(312, 46);
            this.wptLayoutPanel.TabIndex = 31;
            // 
            // identTxtBox
            // 
            this.identTxtBox.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.identTxtBox.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.identTxtBox.Location = new System.Drawing.Point(2, 17);
            this.identTxtBox.Margin = new System.Windows.Forms.Padding(2);
            this.identTxtBox.Name = "identTxtBox";
            this.identTxtBox.Size = new System.Drawing.Size(74, 26);
            this.identTxtBox.TabIndex = 21;
            this.identTxtBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.identTxtBox.TextChanged += new System.EventHandler(this.identTxtBox_TextChanged);
            // 
            // wptComboBox
            // 
            this.wptComboBox.BorderColor = System.Drawing.Color.DimGray;
            this.wptComboBox.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.wptComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.wptComboBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.wptComboBox.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.wptComboBox.FormattingEnabled = true;
            this.wptComboBox.Location = new System.Drawing.Point(80, 17);
            this.wptComboBox.Margin = new System.Windows.Forms.Padding(2);
            this.wptComboBox.Name = "wptComboBox";
            this.wptComboBox.Size = new System.Drawing.Size(230, 27);
            this.wptComboBox.TabIndex = 6;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(80, 0);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(58, 15);
            this.label2.TabIndex = 1;
            this.label2.Text = "Waypoint";
            // 
            // fromIdentLbl
            // 
            this.fromIdentLbl.AutoSize = true;
            this.fromIdentLbl.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.fromIdentLbl.Location = new System.Drawing.Point(2, 0);
            this.fromIdentLbl.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.fromIdentLbl.Name = "fromIdentLbl";
            this.fromIdentLbl.Size = new System.Drawing.Size(34, 15);
            this.fromIdentLbl.TabIndex = 20;
            this.fromIdentLbl.Text = "Ident";
            // 
            // RouteFinderRow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.Controls.Add(this.tableLayoutPanel2);
            this.Name = "RouteFinderRow";
            this.Size = new System.Drawing.Size(787, 59);
            this.typeLayoutPanel.ResumeLayout(false);
            this.typeLayoutPanel.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.wptLayoutPanel.ResumeLayout(false);
            this.wptLayoutPanel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel typeLayoutPanel;
        private System.Windows.Forms.Label fromToLbl;
        private ComboBoxWithBorder typeComboBox;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.TableLayoutPanel wptLayoutPanel;
        private System.Windows.Forms.Label label2;
        private ComboBoxWithBorder wptComboBox;
        private System.Windows.Forms.Label fromIdentLbl;
        private System.Windows.Forms.TextBox identTxtBox;
        public FinderOptionControl OptionControl;
    }
}
