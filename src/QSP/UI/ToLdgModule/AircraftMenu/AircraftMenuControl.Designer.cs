namespace QSP.UI.ToLdgModule.AircraftMenu
{
    partial class AircraftMenuControl
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.acListView = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.deleteBtn = new System.Windows.Forms.Button();
            this.newBtn = new System.Windows.Forms.Button();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.registrationTxtBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.resetBtn = new System.Windows.Forms.Button();
            this.saveBtn = new System.Windows.Forms.Button();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.maxLdgWtTxtBox = new System.Windows.Forms.TextBox();
            this.maxTOWtTxtBox = new System.Windows.Forms.TextBox();
            this.zfwTxtBox = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.acTypeComboBox = new QSP.UI.Controls.ComboBoxWithBorder();
            this.maxLdgWtUnitComboBox = new QSP.UI.Controls.ComboBoxWithBorder();
            this.zfwUnitComboBox = new QSP.UI.Controls.ComboBoxWithBorder();
            this.ldgProfileComboBox = new QSP.UI.Controls.ComboBoxWithBorder();
            this.toProfileComboBox = new QSP.UI.Controls.ComboBoxWithBorder();
            this.maxTOWtUnitComboBox = new QSP.UI.Controls.ComboBoxWithBorder();
            this.groupBox1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.acListView);
            this.groupBox1.Controls.Add(this.deleteBtn);
            this.groupBox1.Controls.Add(this.newBtn);
            this.groupBox1.Font = new System.Drawing.Font("Segoe UI", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(6, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(399, 296);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Aircraft";
            // 
            // acListView
            // 
            this.acListView.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.acListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2});
            this.acListView.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.acListView.FullRowSelect = true;
            this.acListView.Location = new System.Drawing.Point(6, 27);
            this.acListView.MultiSelect = false;
            this.acListView.Name = "acListView";
            this.acListView.Size = new System.Drawing.Size(390, 205);
            this.acListView.TabIndex = 4;
            this.acListView.UseCompatibleStateImageBehavior = false;
            this.acListView.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Aircraft Type";
            this.columnHeader1.Width = 200;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Registration";
            this.columnHeader2.Width = 190;
            // 
            // deleteBtn
            // 
            this.deleteBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.deleteBtn.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.deleteBtn.Location = new System.Drawing.Point(223, 247);
            this.deleteBtn.Name = "deleteBtn";
            this.deleteBtn.Size = new System.Drawing.Size(78, 31);
            this.deleteBtn.TabIndex = 2;
            this.deleteBtn.Text = "Delete";
            this.deleteBtn.UseVisualStyleBackColor = true;
            // 
            // newBtn
            // 
            this.newBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.newBtn.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.newBtn.Location = new System.Drawing.Point(68, 247);
            this.newBtn.Name = "newBtn";
            this.newBtn.Size = new System.Drawing.Size(78, 31);
            this.newBtn.TabIndex = 1;
            this.newBtn.Text = "New";
            this.newBtn.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 36.67712F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 63.32288F));
            this.tableLayoutPanel1.Controls.Add(this.registrationTxtBox, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.label2, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.acTypeComboBox, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(15, 18);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(309, 70);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // registrationTxtBox
            // 
            this.registrationTxtBox.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.registrationTxtBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.registrationTxtBox.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.registrationTxtBox.Location = new System.Drawing.Point(126, 38);
            this.registrationTxtBox.Name = "registrationTxtBox";
            this.registrationTxtBox.Size = new System.Drawing.Size(170, 30);
            this.registrationTxtBox.TabIndex = 5;
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(3, 41);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(101, 23);
            this.label2.TabIndex = 2;
            this.label2.Text = "Registration";
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(3, 6);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(106, 23);
            this.label1.TabIndex = 1;
            this.label1.Text = "Aircraft Type";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.resetBtn);
            this.groupBox2.Controls.Add(this.tableLayoutPanel1);
            this.groupBox2.Controls.Add(this.saveBtn);
            this.groupBox2.Controls.Add(this.tableLayoutPanel2);
            this.groupBox2.Font = new System.Drawing.Font("Segoe UI", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox2.Location = new System.Drawing.Point(411, 3);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(521, 339);
            this.groupBox2.TabIndex = 3;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Properties";
            // 
            // resetBtn
            // 
            this.resetBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.resetBtn.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.resetBtn.Location = new System.Drawing.Point(287, 282);
            this.resetBtn.Name = "resetBtn";
            this.resetBtn.Size = new System.Drawing.Size(78, 31);
            this.resetBtn.TabIndex = 4;
            this.resetBtn.Text = "Reset";
            this.resetBtn.UseVisualStyleBackColor = true;
            // 
            // saveBtn
            // 
            this.saveBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.saveBtn.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.saveBtn.Location = new System.Drawing.Point(138, 282);
            this.saveBtn.Name = "saveBtn";
            this.saveBtn.Size = new System.Drawing.Size(78, 31);
            this.saveBtn.TabIndex = 3;
            this.saveBtn.Text = "Save";
            this.saveBtn.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 3;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 52.89855F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 47.10145F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 97F));
            this.tableLayoutPanel2.Controls.Add(this.maxLdgWtTxtBox, 1, 4);
            this.tableLayoutPanel2.Controls.Add(this.maxTOWtTxtBox, 1, 3);
            this.tableLayoutPanel2.Controls.Add(this.zfwTxtBox, 1, 2);
            this.tableLayoutPanel2.Controls.Add(this.maxLdgWtUnitComboBox, 2, 4);
            this.tableLayoutPanel2.Controls.Add(this.zfwUnitComboBox, 2, 2);
            this.tableLayoutPanel2.Controls.Add(this.label7, 0, 4);
            this.tableLayoutPanel2.Controls.Add(this.ldgProfileComboBox, 1, 1);
            this.tableLayoutPanel2.Controls.Add(this.label3, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.toProfileComboBox, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.label4, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.label5, 0, 2);
            this.tableLayoutPanel2.Controls.Add(this.label6, 0, 3);
            this.tableLayoutPanel2.Controls.Add(this.maxTOWtUnitComboBox, 2, 3);
            this.tableLayoutPanel2.Location = new System.Drawing.Point(15, 88);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 5;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(484, 175);
            this.tableLayoutPanel2.TabIndex = 1;
            // 
            // maxLdgWtTxtBox
            // 
            this.maxLdgWtTxtBox.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.maxLdgWtTxtBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.maxLdgWtTxtBox.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.maxLdgWtTxtBox.Location = new System.Drawing.Point(210, 143);
            this.maxLdgWtTxtBox.Name = "maxLdgWtTxtBox";
            this.maxLdgWtTxtBox.Size = new System.Drawing.Size(170, 30);
            this.maxLdgWtTxtBox.TabIndex = 2;
            this.maxLdgWtTxtBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // maxTOWtTxtBox
            // 
            this.maxTOWtTxtBox.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.maxTOWtTxtBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.maxTOWtTxtBox.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.maxTOWtTxtBox.Location = new System.Drawing.Point(210, 108);
            this.maxTOWtTxtBox.Name = "maxTOWtTxtBox";
            this.maxTOWtTxtBox.Size = new System.Drawing.Size(170, 30);
            this.maxTOWtTxtBox.TabIndex = 3;
            this.maxTOWtTxtBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // zfwTxtBox
            // 
            this.zfwTxtBox.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.zfwTxtBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.zfwTxtBox.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.zfwTxtBox.Location = new System.Drawing.Point(210, 73);
            this.zfwTxtBox.Name = "zfwTxtBox";
            this.zfwTxtBox.Size = new System.Drawing.Size(170, 30);
            this.zfwTxtBox.TabIndex = 4;
            this.zfwTxtBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label7
            // 
            this.label7.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(3, 146);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(160, 23);
            this.label7.TabIndex = 6;
            this.label7.Text = "Max landing weight";
            // 
            // label3
            // 
            this.label3.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(3, 41);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(172, 23);
            this.label3.TabIndex = 2;
            this.label3.Text = "Landing profile name";
            // 
            // label4
            // 
            this.label4.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(3, 6);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(166, 23);
            this.label4.TabIndex = 1;
            this.label4.Text = "Takeoff profile name";
            // 
            // label5
            // 
            this.label5.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(3, 76);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(44, 23);
            this.label5.TabIndex = 4;
            this.label5.Text = "ZFW";
            // 
            // label6
            // 
            this.label6.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(3, 111);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(155, 23);
            this.label6.TabIndex = 5;
            this.label6.Text = "Max takeoff weight";
            // 
            // acTypeComboBox
            // 
            this.acTypeComboBox.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.acTypeComboBox.BorderColor = System.Drawing.Color.DimGray;
            this.acTypeComboBox.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.acTypeComboBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.acTypeComboBox.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.acTypeComboBox.FormattingEnabled = true;
            this.acTypeComboBox.Location = new System.Drawing.Point(126, 3);
            this.acTypeComboBox.Name = "acTypeComboBox";
            this.acTypeComboBox.Size = new System.Drawing.Size(170, 31);
            this.acTypeComboBox.TabIndex = 1;
            // 
            // maxLdgWtUnitComboBox
            // 
            this.maxLdgWtUnitComboBox.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.maxLdgWtUnitComboBox.BorderColor = System.Drawing.Color.DimGray;
            this.maxLdgWtUnitComboBox.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.maxLdgWtUnitComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.maxLdgWtUnitComboBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.maxLdgWtUnitComboBox.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.maxLdgWtUnitComboBox.FormattingEnabled = true;
            this.maxLdgWtUnitComboBox.Location = new System.Drawing.Point(398, 143);
            this.maxLdgWtUnitComboBox.Name = "maxLdgWtUnitComboBox";
            this.maxLdgWtUnitComboBox.Size = new System.Drawing.Size(73, 31);
            this.maxLdgWtUnitComboBox.TabIndex = 8;
            // 
            // zfwUnitComboBox
            // 
            this.zfwUnitComboBox.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.zfwUnitComboBox.BorderColor = System.Drawing.Color.DimGray;
            this.zfwUnitComboBox.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.zfwUnitComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.zfwUnitComboBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.zfwUnitComboBox.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.zfwUnitComboBox.FormattingEnabled = true;
            this.zfwUnitComboBox.Location = new System.Drawing.Point(398, 73);
            this.zfwUnitComboBox.Name = "zfwUnitComboBox";
            this.zfwUnitComboBox.Size = new System.Drawing.Size(73, 31);
            this.zfwUnitComboBox.TabIndex = 4;
            // 
            // ldgProfileComboBox
            // 
            this.ldgProfileComboBox.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.ldgProfileComboBox.BorderColor = System.Drawing.Color.DimGray;
            this.ldgProfileComboBox.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.ldgProfileComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ldgProfileComboBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ldgProfileComboBox.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ldgProfileComboBox.FormattingEnabled = true;
            this.ldgProfileComboBox.Location = new System.Drawing.Point(210, 38);
            this.ldgProfileComboBox.Name = "ldgProfileComboBox";
            this.ldgProfileComboBox.Size = new System.Drawing.Size(170, 31);
            this.ldgProfileComboBox.TabIndex = 3;
            // 
            // toProfileComboBox
            // 
            this.toProfileComboBox.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.toProfileComboBox.BorderColor = System.Drawing.Color.DimGray;
            this.toProfileComboBox.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.toProfileComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.toProfileComboBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.toProfileComboBox.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.toProfileComboBox.FormattingEnabled = true;
            this.toProfileComboBox.Location = new System.Drawing.Point(210, 3);
            this.toProfileComboBox.Name = "toProfileComboBox";
            this.toProfileComboBox.Size = new System.Drawing.Size(170, 31);
            this.toProfileComboBox.TabIndex = 1;
            // 
            // maxTOWtUnitComboBox
            // 
            this.maxTOWtUnitComboBox.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.maxTOWtUnitComboBox.BorderColor = System.Drawing.Color.DimGray;
            this.maxTOWtUnitComboBox.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.maxTOWtUnitComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.maxTOWtUnitComboBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.maxTOWtUnitComboBox.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.maxTOWtUnitComboBox.FormattingEnabled = true;
            this.maxTOWtUnitComboBox.Location = new System.Drawing.Point(398, 108);
            this.maxTOWtUnitComboBox.Name = "maxTOWtUnitComboBox";
            this.maxTOWtUnitComboBox.Size = new System.Drawing.Size(73, 31);
            this.maxTOWtUnitComboBox.TabIndex = 7;
            // 
            // AircraftMenuControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(120F, 120F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "AircraftMenuControl";
            this.Size = new System.Drawing.Size(1024, 676);
            this.groupBox1.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label1;
        private Controls.ComboBoxWithBorder acTypeComboBox;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button deleteBtn;
        private System.Windows.Forms.Button newBtn;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private Controls.ComboBoxWithBorder ldgProfileComboBox;
        private System.Windows.Forms.Label label3;
        private Controls.ComboBoxWithBorder toProfileComboBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox maxLdgWtTxtBox;
        private System.Windows.Forms.TextBox maxTOWtTxtBox;
        private System.Windows.Forms.TextBox zfwTxtBox;
        private Controls.ComboBoxWithBorder maxLdgWtUnitComboBox;
        private Controls.ComboBoxWithBorder zfwUnitComboBox;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private Controls.ComboBoxWithBorder maxTOWtUnitComboBox;
        private System.Windows.Forms.Button resetBtn;
        private System.Windows.Forms.Button saveBtn;
        private System.Windows.Forms.ListView acListView;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.TextBox registrationTxtBox;
    }
}
