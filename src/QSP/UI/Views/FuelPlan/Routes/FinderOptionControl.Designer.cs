namespace QSP.UI.Views.FuelPlan.Routes
{
    partial class FinderOptionControl
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
            this.tableLayoutPanel5 = new System.Windows.Forms.TableLayoutPanel();
            this.icaoTxtBox = new System.Windows.Forms.TextBox();
            this.rwyComboBox = new QSP.UI.Views.ComboBoxWithBorder();
            this.procComboBox = new QSP.UI.Views.ComboBoxWithBorder();
            this.filterBtn = new System.Windows.Forms.Button();
            this.fromIdentLbl = new System.Windows.Forms.Label();
            this.fromRwyLbl = new System.Windows.Forms.Label();
            this.sidLbl = new System.Windows.Forms.Label();
            this.tableLayoutPanel5.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel5
            // 
            this.tableLayoutPanel5.AutoSize = true;
            this.tableLayoutPanel5.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel5.ColumnCount = 4;
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel5.Controls.Add(this.icaoTxtBox, 0, 1);
            this.tableLayoutPanel5.Controls.Add(this.rwyComboBox, 1, 1);
            this.tableLayoutPanel5.Controls.Add(this.procComboBox, 2, 1);
            this.tableLayoutPanel5.Controls.Add(this.filterBtn, 3, 1);
            this.tableLayoutPanel5.Controls.Add(this.fromIdentLbl, 0, 0);
            this.tableLayoutPanel5.Controls.Add(this.fromRwyLbl, 1, 0);
            this.tableLayoutPanel5.Controls.Add(this.sidLbl, 2, 0);
            this.tableLayoutPanel5.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel5.Margin = new System.Windows.Forms.Padding(0, 2, 24, 2);
            this.tableLayoutPanel5.Name = "tableLayoutPanel5";
            this.tableLayoutPanel5.RowCount = 2;
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel5.Size = new System.Drawing.Size(330, 48);
            this.tableLayoutPanel5.TabIndex = 1;
            // 
            // icaoTxtBox
            // 
            this.icaoTxtBox.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.icaoTxtBox.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.icaoTxtBox.Location = new System.Drawing.Point(3, 18);
            this.icaoTxtBox.Name = "icaoTxtBox";
            this.icaoTxtBox.Size = new System.Drawing.Size(74, 26);
            this.icaoTxtBox.TabIndex = 0;
            this.icaoTxtBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.icaoTxtBox.TextChanged += new System.EventHandler(this.IcaoChanged);
            // 
            // origRwyComboBox
            // 
            this.rwyComboBox.BorderColor = System.Drawing.Color.DimGray;
            this.rwyComboBox.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.rwyComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.rwyComboBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rwyComboBox.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rwyComboBox.FormattingEnabled = true;
            this.rwyComboBox.Location = new System.Drawing.Point(83, 18);
            this.rwyComboBox.Name = "origRwyComboBox";
            this.rwyComboBox.Size = new System.Drawing.Size(64, 27);
            this.rwyComboBox.TabIndex = 1;
            this.rwyComboBox.SelectedIndexChanged += new System.EventHandler(this.origRwyComboBox_SelectedIndexChanged);
            // 
            // procComboBox
            // 
            this.procComboBox.BorderColor = System.Drawing.Color.DimGray;
            this.procComboBox.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.procComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.procComboBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.procComboBox.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.procComboBox.FormattingEnabled = true;
            this.procComboBox.Location = new System.Drawing.Point(153, 18);
            this.procComboBox.Name = "procComboBox";
            this.procComboBox.Size = new System.Drawing.Size(123, 27);
            this.procComboBox.TabIndex = 2;
            // 
            // filterBtn
            // 
            this.filterBtn.AutoSize = true;
            this.filterBtn.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.filterBtn.BackColor = System.Drawing.Color.DarkSlateGray;
            this.filterBtn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.filterBtn.Cursor = System.Windows.Forms.Cursors.Default;
            this.filterBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.filterBtn.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.filterBtn.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.filterBtn.Location = new System.Drawing.Point(282, 18);
            this.filterBtn.Name = "filterBtn";
            this.filterBtn.Size = new System.Drawing.Size(45, 27);
            this.filterBtn.TabIndex = 3;
            this.filterBtn.Text = "Filter";
            this.filterBtn.UseVisualStyleBackColor = false;
            this.filterBtn.Click += new System.EventHandler(this.filterBtn_Click);
            // 
            // fromIdentLbl
            // 
            this.fromIdentLbl.AutoSize = true;
            this.fromIdentLbl.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.fromIdentLbl.Location = new System.Drawing.Point(2, 0);
            this.fromIdentLbl.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.fromIdentLbl.Name = "fromIdentLbl";
            this.fromIdentLbl.Size = new System.Drawing.Size(40, 15);
            this.fromIdentLbl.TabIndex = 50;
            this.fromIdentLbl.Text = "Origin";
            // 
            // fromRwyLbl
            // 
            this.fromRwyLbl.AutoSize = true;
            this.fromRwyLbl.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.fromRwyLbl.Location = new System.Drawing.Point(82, 0);
            this.fromRwyLbl.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.fromRwyLbl.Name = "fromRwyLbl";
            this.fromRwyLbl.Size = new System.Drawing.Size(49, 15);
            this.fromRwyLbl.TabIndex = 46;
            this.fromRwyLbl.Text = "Runway";
            // 
            // sidLbl
            // 
            this.sidLbl.AutoSize = true;
            this.sidLbl.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.sidLbl.Location = new System.Drawing.Point(152, 0);
            this.sidLbl.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.sidLbl.Name = "sidLbl";
            this.sidLbl.Size = new System.Drawing.Size(24, 15);
            this.sidLbl.TabIndex = 56;
            this.sidLbl.Text = "SID";
            // 
            // FinderOptionControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.Controls.Add(this.tableLayoutPanel5);
            this.Name = "FinderOptionControl";
            this.Size = new System.Drawing.Size(354, 50);
            this.tableLayoutPanel5.ResumeLayout(false);
            this.tableLayoutPanel5.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel5;
        public System.Windows.Forms.TextBox icaoTxtBox;
        public ComboBoxWithBorder rwyComboBox;
        public ComboBoxWithBorder procComboBox;
        private System.Windows.Forms.Button filterBtn;
        private System.Windows.Forms.Label fromIdentLbl;
        private System.Windows.Forms.Label fromRwyLbl;
        private System.Windows.Forms.Label sidLbl;
    }
}
