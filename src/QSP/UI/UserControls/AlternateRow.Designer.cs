namespace QSP.UI.UserControls
{
    partial class AlternateRowItems
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
            this.ShowMoreBtn = new System.Windows.Forms.Button();
            this.DisLbl = new System.Windows.Forms.Label();
            this.RouteTxtBox = new System.Windows.Forms.TextBox();
            this.RouteLbl = new System.Windows.Forms.Label();
            this.RwyComboBox = new QSP.UI.Controls.ComboBoxWithBorder();
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
            this.tableLayoutPanel1.Controls.Add(this.ShowMoreBtn, 7, 0);
            this.tableLayoutPanel1.Controls.Add(this.DisLbl, 6, 0);
            this.tableLayoutPanel1.Controls.Add(this.RouteTxtBox, 5, 0);
            this.tableLayoutPanel1.Controls.Add(this.RouteLbl, 4, 0);
            this.tableLayoutPanel1.Controls.Add(this.RwyComboBox, 3, 0);
            this.tableLayoutPanel1.Controls.Add(this.IcaoTxtBox, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.FindBtn, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.RwyLbl, 2, 0);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1045, 39);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // ShowMoreBtn
            // 
            this.ShowMoreBtn.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.ShowMoreBtn.BackgroundImage = global::QSP.Properties.Resources.add_icon;
            this.ShowMoreBtn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.ShowMoreBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ShowMoreBtn.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ShowMoreBtn.ForeColor = System.Drawing.SystemColors.ControlDark;
            this.ShowMoreBtn.Location = new System.Drawing.Point(1009, 3);
            this.ShowMoreBtn.Name = "ShowMoreBtn";
            this.ShowMoreBtn.Size = new System.Drawing.Size(33, 33);
            this.ShowMoreBtn.TabIndex = 77;
            this.ShowMoreBtn.UseVisualStyleBackColor = false;
            // 
            // DisLbl
            // 
            this.DisLbl.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.DisLbl.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DisLbl.Location = new System.Drawing.Point(833, 8);
            this.DisLbl.Margin = new System.Windows.Forms.Padding(3);
            this.DisLbl.Name = "DisLbl";
            this.DisLbl.Size = new System.Drawing.Size(170, 23);
            this.DisLbl.TabIndex = 78;
            // 
            // RouteTxtBox
            // 
            this.RouteTxtBox.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.RouteTxtBox.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RouteTxtBox.Location = new System.Drawing.Point(347, 3);
            this.RouteTxtBox.Name = "RouteTxtBox";
            this.RouteTxtBox.Size = new System.Drawing.Size(480, 30);
            this.RouteTxtBox.TabIndex = 76;
            // 
            // RouteLbl
            // 
            this.RouteLbl.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.RouteLbl.AutoSize = true;
            this.RouteLbl.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RouteLbl.Location = new System.Drawing.Point(286, 8);
            this.RouteLbl.Margin = new System.Windows.Forms.Padding(3);
            this.RouteLbl.Name = "RouteLbl";
            this.RouteLbl.Size = new System.Drawing.Size(55, 23);
            this.RouteLbl.TabIndex = 78;
            this.RouteLbl.Text = "Route";
            // 
            // RwyComboBox
            // 
            this.RwyComboBox.BorderColor = System.Drawing.Color.DimGray;
            this.RwyComboBox.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.RwyComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.RwyComboBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.RwyComboBox.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RwyComboBox.FormattingEnabled = true;
            this.RwyComboBox.Location = new System.Drawing.Point(220, 3);
            this.RwyComboBox.Name = "RwyComboBox";
            this.RwyComboBox.Size = new System.Drawing.Size(60, 31);
            this.RwyComboBox.TabIndex = 74;
            // 
            // IcaoTxtBox
            // 
            this.IcaoTxtBox.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.IcaoTxtBox.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IcaoTxtBox.Location = new System.Drawing.Point(3, 3);
            this.IcaoTxtBox.Name = "IcaoTxtBox";
            this.IcaoTxtBox.Size = new System.Drawing.Size(70, 30);
            this.IcaoTxtBox.TabIndex = 75;
            this.IcaoTxtBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // FindBtn
            // 
            this.FindBtn.BackColor = System.Drawing.Color.DarkSlateGray;
            this.FindBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.FindBtn.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FindBtn.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.FindBtn.Location = new System.Drawing.Point(79, 3);
            this.FindBtn.Name = "FindBtn";
            this.FindBtn.Size = new System.Drawing.Size(60, 33);
            this.FindBtn.TabIndex = 76;
            this.FindBtn.Text = "Find";
            this.FindBtn.UseVisualStyleBackColor = false;
            // 
            // RwyLbl
            // 
            this.RwyLbl.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.RwyLbl.AutoSize = true;
            this.RwyLbl.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RwyLbl.Location = new System.Drawing.Point(145, 8);
            this.RwyLbl.Margin = new System.Windows.Forms.Padding(3);
            this.RwyLbl.Name = "RwyLbl";
            this.RwyLbl.Size = new System.Drawing.Size(69, 23);
            this.RwyLbl.TabIndex = 77;
            this.RwyLbl.Text = "Runway";
            // 
            // AlternateRowControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(120F, 120F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "AlternateRowControl";
            this.Size = new System.Drawing.Size(1048, 42);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        public Controls.ComboBoxWithBorder RwyComboBox;
        public System.Windows.Forms.TextBox IcaoTxtBox;
        public System.Windows.Forms.TextBox RouteTxtBox;
        public System.Windows.Forms.Label RouteLbl;
        public System.Windows.Forms.Button FindBtn;
        public System.Windows.Forms.Label RwyLbl;
        public System.Windows.Forms.Label DisLbl;
        public System.Windows.Forms.Button ShowMoreBtn;
    }
}
