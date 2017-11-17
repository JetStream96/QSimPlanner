namespace QSP.UI.Views.Route
{
    partial class AirportSelectionControl
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
            this.origTxtBox = new System.Windows.Forms.TextBox();
            this.origRwyComboBox = new QSP.UI.Views.ComboBoxWithBorder();
            this.sidComboBox = new QSP.UI.Views.ComboBoxWithBorder();
            this.filterSidBtn = new System.Windows.Forms.Button();
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
            this.tableLayoutPanel5.Controls.Add(this.origTxtBox, 0, 1);
            this.tableLayoutPanel5.Controls.Add(this.origRwyComboBox, 1, 1);
            this.tableLayoutPanel5.Controls.Add(this.sidComboBox, 2, 1);
            this.tableLayoutPanel5.Controls.Add(this.filterSidBtn, 3, 1);
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
            // origTxtBox
            // 
            this.origTxtBox.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.origTxtBox.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.origTxtBox.Location = new System.Drawing.Point(3, 18);
            this.origTxtBox.Name = "origTxtBox";
            this.origTxtBox.Size = new System.Drawing.Size(74, 26);
            this.origTxtBox.TabIndex = 0;
            this.origTxtBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // origRwyComboBox
            // 
            this.origRwyComboBox.BorderColor = System.Drawing.Color.DimGray;
            this.origRwyComboBox.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.origRwyComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.origRwyComboBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.origRwyComboBox.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.origRwyComboBox.FormattingEnabled = true;
            this.origRwyComboBox.Location = new System.Drawing.Point(83, 18);
            this.origRwyComboBox.Name = "origRwyComboBox";
            this.origRwyComboBox.Size = new System.Drawing.Size(64, 27);
            this.origRwyComboBox.TabIndex = 1;
            // 
            // sidComboBox
            // 
            this.sidComboBox.BorderColor = System.Drawing.Color.DimGray;
            this.sidComboBox.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.sidComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.sidComboBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.sidComboBox.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.sidComboBox.FormattingEnabled = true;
            this.sidComboBox.Location = new System.Drawing.Point(153, 18);
            this.sidComboBox.Name = "sidComboBox";
            this.sidComboBox.Size = new System.Drawing.Size(123, 27);
            this.sidComboBox.TabIndex = 2;
            // 
            // filterSidBtn
            // 
            this.filterSidBtn.AutoSize = true;
            this.filterSidBtn.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.filterSidBtn.BackColor = System.Drawing.Color.DarkSlateGray;
            this.filterSidBtn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.filterSidBtn.Cursor = System.Windows.Forms.Cursors.Default;
            this.filterSidBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.filterSidBtn.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.filterSidBtn.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.filterSidBtn.Location = new System.Drawing.Point(282, 18);
            this.filterSidBtn.Name = "filterSidBtn";
            this.filterSidBtn.Size = new System.Drawing.Size(45, 27);
            this.filterSidBtn.TabIndex = 3;
            this.filterSidBtn.Text = "Filter";
            this.filterSidBtn.UseVisualStyleBackColor = false;
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
            // AirportSelectionControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.Controls.Add(this.tableLayoutPanel5);
            this.Name = "AirportSelectionControl";
            this.Size = new System.Drawing.Size(354, 50);
            this.tableLayoutPanel5.ResumeLayout(false);
            this.tableLayoutPanel5.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel5;
        public System.Windows.Forms.TextBox origTxtBox;
        public ComboBoxWithBorder origRwyComboBox;
        public ComboBoxWithBorder sidComboBox;
        private System.Windows.Forms.Button filterSidBtn;
        private System.Windows.Forms.Label fromIdentLbl;
        private System.Windows.Forms.Label fromRwyLbl;
        private System.Windows.Forms.Label sidLbl;
    }
}
