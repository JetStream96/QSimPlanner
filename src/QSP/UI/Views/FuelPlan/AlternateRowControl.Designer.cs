namespace QSP.UI.Views.FuelPlan
{
    partial class AlternateRowControl
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
            this.ActionBtn = new System.Windows.Forms.Button();
            this.DisLbl = new System.Windows.Forms.Label();
            this.RouteTxtBox = new System.Windows.Forms.TextBox();
            this.RouteLbl = new System.Windows.Forms.Label();
            this.RwyComboBox = new QSP.UI.Views.ComboBoxWithBorder();
            this.IcaoTxtBox = new System.Windows.Forms.TextBox();
            this.FindBtn = new System.Windows.Forms.Button();
            this.RwyLbl = new System.Windows.Forms.Label();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.AutoSize = true;
            this.tableLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel1.ColumnCount = 8;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.ActionBtn, 7, 0);
            this.tableLayoutPanel1.Controls.Add(this.DisLbl, 6, 0);
            this.tableLayoutPanel1.Controls.Add(this.RouteTxtBox, 5, 0);
            this.tableLayoutPanel1.Controls.Add(this.RouteLbl, 4, 0);
            this.tableLayoutPanel1.Controls.Add(this.RwyComboBox, 3, 0);
            this.tableLayoutPanel1.Controls.Add(this.IcaoTxtBox, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.FindBtn, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.RwyLbl, 2, 0);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(2);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(858, 35);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // ActionBtn
            // 
            this.ActionBtn.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.ActionBtn.AutoSize = true;
            this.ActionBtn.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ActionBtn.BackColor = System.Drawing.Color.DarkSlateGray;
            this.ActionBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ActionBtn.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ActionBtn.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.ActionBtn.Location = new System.Drawing.Point(790, 2);
            this.ActionBtn.Margin = new System.Windows.Forms.Padding(2);
            this.ActionBtn.Name = "ActionBtn";
            this.ActionBtn.Size = new System.Drawing.Size(66, 31);
            this.ActionBtn.TabIndex = 7;
            this.ActionBtn.Text = "Actions";
            this.ActionBtn.UseVisualStyleBackColor = false;
            this.ActionBtn.Click += new System.EventHandler(this.ActionBtn_Click);
            // 
            // DisLbl
            // 
            this.DisLbl.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.DisLbl.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DisLbl.Location = new System.Drawing.Point(642, 8);
            this.DisLbl.Margin = new System.Windows.Forms.Padding(2);
            this.DisLbl.Name = "DisLbl";
            this.DisLbl.Size = new System.Drawing.Size(144, 18);
            this.DisLbl.TabIndex = 6;
            // 
            // RouteTxtBox
            // 
            this.RouteTxtBox.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.RouteTxtBox.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.RouteTxtBox.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RouteTxtBox.Location = new System.Drawing.Point(277, 4);
            this.RouteTxtBox.Margin = new System.Windows.Forms.Padding(2);
            this.RouteTxtBox.Name = "RouteTxtBox";
            this.RouteTxtBox.Size = new System.Drawing.Size(361, 26);
            this.RouteTxtBox.TabIndex = 5;
            // 
            // RouteLbl
            // 
            this.RouteLbl.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.RouteLbl.AutoSize = true;
            this.RouteLbl.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RouteLbl.Location = new System.Drawing.Point(228, 8);
            this.RouteLbl.Margin = new System.Windows.Forms.Padding(2);
            this.RouteLbl.Name = "RouteLbl";
            this.RouteLbl.Size = new System.Drawing.Size(45, 19);
            this.RouteLbl.TabIndex = 4;
            this.RouteLbl.Text = "Route";
            // 
            // RwyComboBox
            // 
            this.RwyComboBox.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.RwyComboBox.BorderColor = System.Drawing.Color.DimGray;
            this.RwyComboBox.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.RwyComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.RwyComboBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.RwyComboBox.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RwyComboBox.FormattingEnabled = true;
            this.RwyComboBox.Location = new System.Drawing.Point(175, 4);
            this.RwyComboBox.Margin = new System.Windows.Forms.Padding(2);
            this.RwyComboBox.Name = "RwyComboBox";
            this.RwyComboBox.Size = new System.Drawing.Size(49, 27);
            this.RwyComboBox.TabIndex = 3;
            // 
            // IcaoTxtBox
            // 
            this.IcaoTxtBox.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.IcaoTxtBox.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.IcaoTxtBox.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IcaoTxtBox.Location = new System.Drawing.Point(2, 4);
            this.IcaoTxtBox.Margin = new System.Windows.Forms.Padding(2);
            this.IcaoTxtBox.Name = "IcaoTxtBox";
            this.IcaoTxtBox.Size = new System.Drawing.Size(57, 26);
            this.IcaoTxtBox.TabIndex = 0;
            this.IcaoTxtBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.IcaoTxtBox.TextChanged += new System.EventHandler(this.IcaoTxtBox_TextChanged);
            // 
            // FindBtn
            // 
            this.FindBtn.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.FindBtn.AutoSize = true;
            this.FindBtn.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.FindBtn.BackColor = System.Drawing.Color.DarkSlateGray;
            this.FindBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.FindBtn.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FindBtn.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.FindBtn.Location = new System.Drawing.Point(63, 2);
            this.FindBtn.Margin = new System.Windows.Forms.Padding(2);
            this.FindBtn.Name = "FindBtn";
            this.FindBtn.Size = new System.Drawing.Size(47, 31);
            this.FindBtn.TabIndex = 1;
            this.FindBtn.Text = "Find";
            this.FindBtn.UseVisualStyleBackColor = false;
            this.FindBtn.Click += new System.EventHandler(this.FindBtnClick);
            // 
            // RwyLbl
            // 
            this.RwyLbl.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.RwyLbl.AutoSize = true;
            this.RwyLbl.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RwyLbl.Location = new System.Drawing.Point(114, 8);
            this.RwyLbl.Margin = new System.Windows.Forms.Padding(2);
            this.RwyLbl.Name = "RwyLbl";
            this.RwyLbl.Size = new System.Drawing.Size(57, 19);
            this.RwyLbl.TabIndex = 2;
            this.RwyLbl.Text = "Runway";
            // 
            // AlternateRowControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "AlternateRowControl";
            this.Size = new System.Drawing.Size(860, 37);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        public ComboBoxWithBorder RwyComboBox;
        public System.Windows.Forms.TextBox IcaoTxtBox;
        public System.Windows.Forms.Label RouteLbl;
        public System.Windows.Forms.Button FindBtn;
        public System.Windows.Forms.Label RwyLbl;
        public System.Windows.Forms.Label DisLbl;
        public System.Windows.Forms.Button ActionBtn;
        public System.Windows.Forms.TextBox RouteTxtBox;
    }
}
