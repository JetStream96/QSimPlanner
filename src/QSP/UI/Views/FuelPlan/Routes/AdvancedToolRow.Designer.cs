namespace QSP.UI.Views.FuelPlan.Routes
{
    partial class AdvancedToolRow
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.typeComboBox = new QSP.UI.Views.ComboBoxWithBorder();
            this.label1 = new System.Windows.Forms.Label();
            this.finderOptionControl = new QSP.UI.Views.FuelPlan.Routes.FinderOptionControl();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.identTxtBox = new System.Windows.Forms.TextBox();
            this.wptComboBox = new QSP.UI.Views.ComboBoxWithBorder();
            this.label2 = new System.Windows.Forms.Label();
            this.fromIdentLbl = new System.Windows.Forms.Label();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.AutoSize = true;
            this.tableLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Controls.Add(this.typeComboBox, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(100, 46);
            this.tableLayoutPanel1.TabIndex = 28;
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
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(2, 0);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 15);
            this.label1.TabIndex = 1;
            this.label1.Text = "From";
            // 
            // finderOptionControl
            // 
            this.finderOptionControl.AutoSize = true;
            this.finderOptionControl.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.finderOptionControl.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.finderOptionControl.Icao = "";
            this.finderOptionControl.Location = new System.Drawing.Point(109, 3);
            this.finderOptionControl.Name = "finderOptionControl";
            this.finderOptionControl.SelectedProcedureText = "";
            this.finderOptionControl.SelectedRwy = "";
            this.finderOptionControl.Size = new System.Drawing.Size(354, 50);
            this.finderOptionControl.TabIndex = 29;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.AutoSize = true;
            this.tableLayoutPanel2.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel2.ColumnCount = 3;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Controls.Add(this.tableLayoutPanel3, 2, 0);
            this.tableLayoutPanel2.Controls.Add(this.tableLayoutPanel1, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.finderOptionControl, 1, 0);
            this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 56F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(784, 56);
            this.tableLayoutPanel2.TabIndex = 30;
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.AutoSize = true;
            this.tableLayoutPanel3.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel3.ColumnCount = 2;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Controls.Add(this.identTxtBox, 0, 1);
            this.tableLayoutPanel3.Controls.Add(this.wptComboBox, 1, 1);
            this.tableLayoutPanel3.Controls.Add(this.label2, 1, 0);
            this.tableLayoutPanel3.Controls.Add(this.fromIdentLbl, 0, 0);
            this.tableLayoutPanel3.Location = new System.Drawing.Point(469, 3);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 2;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(312, 46);
            this.tableLayoutPanel3.TabIndex = 31;
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
            // AdvancedToolRow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.Controls.Add(this.tableLayoutPanel2);
            this.Name = "AdvancedToolRow";
            this.Size = new System.Drawing.Size(787, 59);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel3.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label label1;
        private ComboBoxWithBorder typeComboBox;
        private FinderOptionControl finderOptionControl;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.Label label2;
        private ComboBoxWithBorder wptComboBox;
        private System.Windows.Forms.Label fromIdentLbl;
        private System.Windows.Forms.TextBox identTxtBox;
    }
}
