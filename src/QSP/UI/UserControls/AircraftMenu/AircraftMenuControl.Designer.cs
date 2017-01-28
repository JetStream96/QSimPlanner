namespace QSP.UI.UserControls.AircraftMenu
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
            this.selectionGroupBox = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.editBtn = new System.Windows.Forms.Button();
            this.deleteBtn = new System.Windows.Forms.Button();
            this.newBtn = new System.Windows.Forms.Button();
            this.acListView = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.registrationTxtBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.acTypeComboBox = new QSP.UI.Controls.ComboBoxWithBorder();
            this.label1 = new System.Windows.Forms.Label();
            this.propertyGroupBox = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel6 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel5 = new System.Windows.Forms.TableLayoutPanel();
            this.saveBtn = new System.Windows.Forms.Button();
            this.cancelBtn = new System.Windows.Forms.Button();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.label14 = new System.Windows.Forms.Label();
            this.biasPercentTxtBox = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.maxFuelTxtBox = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.wtUnitLbl1 = new System.Windows.Forms.Label();
            this.wtUnitLbl2 = new System.Windows.Forms.Label();
            this.wtUnitLbl3 = new System.Windows.Forms.Label();
            this.wtUnitLbl4 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.fuelProfileComboBox = new QSP.UI.Controls.ComboBoxWithBorder();
            this.label9 = new System.Windows.Forms.Label();
            this.maxZfwTxtBox = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.maxLdgWtTxtBox = new System.Windows.Forms.TextBox();
            this.maxTOWtTxtBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.oewTxtBox = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.ldgProfileComboBox = new QSP.UI.Controls.ComboBoxWithBorder();
            this.toProfileComboBox = new QSP.UI.Controls.ComboBoxWithBorder();
            this.label4 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.wtUnitComboBox = new QSP.UI.Controls.ComboBoxWithBorder();
            this.wtUnitLbl5 = new System.Windows.Forms.Label();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.selectionGroupBox.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.propertyGroupBox.SuspendLayout();
            this.tableLayoutPanel6.SuspendLayout();
            this.tableLayoutPanel5.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            this.SuspendLayout();
            // 
            // selectionGroupBox
            // 
            this.selectionGroupBox.AutoSize = true;
            this.selectionGroupBox.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.selectionGroupBox.Controls.Add(this.tableLayoutPanel3);
            this.selectionGroupBox.Controls.Add(this.acListView);
            this.selectionGroupBox.Font = new System.Drawing.Font("Segoe UI Semibold", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.selectionGroupBox.Location = new System.Drawing.Point(2, 2);
            this.selectionGroupBox.Margin = new System.Windows.Forms.Padding(2);
            this.selectionGroupBox.Name = "selectionGroupBox";
            this.selectionGroupBox.Padding = new System.Windows.Forms.Padding(2);
            this.selectionGroupBox.Size = new System.Drawing.Size(400, 513);
            this.selectionGroupBox.TabIndex = 0;
            this.selectionGroupBox.TabStop = false;
            this.selectionGroupBox.Text = "Aircraft";
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 3;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel3.Controls.Add(this.editBtn, 1, 0);
            this.tableLayoutPanel3.Controls.Add(this.deleteBtn, 2, 0);
            this.tableLayoutPanel3.Controls.Add(this.newBtn, 0, 0);
            this.tableLayoutPanel3.Location = new System.Drawing.Point(42, 441);
            this.tableLayoutPanel3.Margin = new System.Windows.Forms.Padding(2);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 1;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(315, 45);
            this.tableLayoutPanel3.TabIndex = 1;
            // 
            // editBtn
            // 
            this.editBtn.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.editBtn.AutoSize = true;
            this.editBtn.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.editBtn.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.editBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.editBtn.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.editBtn.ForeColor = System.Drawing.Color.Black;
            this.editBtn.Location = new System.Drawing.Point(118, 5);
            this.editBtn.Margin = new System.Windows.Forms.Padding(2);
            this.editBtn.MaximumSize = new System.Drawing.Size(78, 125);
            this.editBtn.MinimumSize = new System.Drawing.Size(78, 0);
            this.editBtn.Name = "editBtn";
            this.editBtn.Size = new System.Drawing.Size(78, 35);
            this.editBtn.TabIndex = 1;
            this.editBtn.Text = "Edit";
            this.editBtn.UseVisualStyleBackColor = false;
            // 
            // deleteBtn
            // 
            this.deleteBtn.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.deleteBtn.AutoSize = true;
            this.deleteBtn.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.deleteBtn.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.deleteBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.deleteBtn.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.deleteBtn.ForeColor = System.Drawing.Color.Black;
            this.deleteBtn.Location = new System.Drawing.Point(223, 5);
            this.deleteBtn.Margin = new System.Windows.Forms.Padding(2);
            this.deleteBtn.MaximumSize = new System.Drawing.Size(78, 125);
            this.deleteBtn.MinimumSize = new System.Drawing.Size(78, 0);
            this.deleteBtn.Name = "deleteBtn";
            this.deleteBtn.Size = new System.Drawing.Size(78, 35);
            this.deleteBtn.TabIndex = 2;
            this.deleteBtn.Text = "Delete";
            this.deleteBtn.UseVisualStyleBackColor = false;
            // 
            // newBtn
            // 
            this.newBtn.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.newBtn.AutoSize = true;
            this.newBtn.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.newBtn.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.newBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.newBtn.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.newBtn.ForeColor = System.Drawing.Color.Black;
            this.newBtn.Location = new System.Drawing.Point(13, 5);
            this.newBtn.Margin = new System.Windows.Forms.Padding(2);
            this.newBtn.MaximumSize = new System.Drawing.Size(78, 125);
            this.newBtn.MinimumSize = new System.Drawing.Size(78, 0);
            this.newBtn.Name = "newBtn";
            this.newBtn.Size = new System.Drawing.Size(78, 35);
            this.newBtn.TabIndex = 0;
            this.newBtn.Text = "New";
            this.newBtn.UseVisualStyleBackColor = false;
            // 
            // acListView
            // 
            this.acListView.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.acListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2});
            this.acListView.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.acListView.FullRowSelect = true;
            this.acListView.Location = new System.Drawing.Point(6, 28);
            this.acListView.Margin = new System.Windows.Forms.Padding(2);
            this.acListView.MultiSelect = false;
            this.acListView.Name = "acListView";
            this.acListView.Size = new System.Drawing.Size(390, 408);
            this.acListView.TabIndex = 0;
            this.acListView.UseCompatibleStateImageBehavior = false;
            this.acListView.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Aircraft Type";
            this.columnHeader1.Width = 180;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Registration";
            this.columnHeader2.Width = 210;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.AutoSize = true;
            this.tableLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.registrationTxtBox, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.label2, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.acTypeComboBox, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(2, 2);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(2);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(338, 69);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // registrationTxtBox
            // 
            this.registrationTxtBox.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.registrationTxtBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.registrationTxtBox.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.registrationTxtBox.Location = new System.Drawing.Point(112, 37);
            this.registrationTxtBox.Margin = new System.Windows.Forms.Padding(2);
            this.registrationTxtBox.Name = "registrationTxtBox";
            this.registrationTxtBox.Size = new System.Drawing.Size(223, 30);
            this.registrationTxtBox.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(2, 40);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(101, 23);
            this.label2.TabIndex = 2;
            this.label2.Text = "Registration";
            // 
            // acTypeComboBox
            // 
            this.acTypeComboBox.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.acTypeComboBox.BorderColor = System.Drawing.Color.DimGray;
            this.acTypeComboBox.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.acTypeComboBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.acTypeComboBox.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.acTypeComboBox.FormattingEnabled = true;
            this.acTypeComboBox.Location = new System.Drawing.Point(112, 2);
            this.acTypeComboBox.Margin = new System.Windows.Forms.Padding(2);
            this.acTypeComboBox.Name = "acTypeComboBox";
            this.acTypeComboBox.Size = new System.Drawing.Size(224, 31);
            this.acTypeComboBox.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(2, 6);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(106, 23);
            this.label1.TabIndex = 1;
            this.label1.Text = "Aircraft Type";
            // 
            // propertyGroupBox
            // 
            this.propertyGroupBox.AutoSize = true;
            this.propertyGroupBox.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.propertyGroupBox.Controls.Add(this.tableLayoutPanel6);
            this.propertyGroupBox.Font = new System.Drawing.Font("Segoe UI Semibold", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.propertyGroupBox.Location = new System.Drawing.Point(406, 2);
            this.propertyGroupBox.Margin = new System.Windows.Forms.Padding(2);
            this.propertyGroupBox.Name = "propertyGroupBox";
            this.propertyGroupBox.Padding = new System.Windows.Forms.Padding(2);
            this.propertyGroupBox.Size = new System.Drawing.Size(533, 546);
            this.propertyGroupBox.TabIndex = 1;
            this.propertyGroupBox.TabStop = false;
            this.propertyGroupBox.Text = "Properties";
            // 
            // tableLayoutPanel6
            // 
            this.tableLayoutPanel6.AutoSize = true;
            this.tableLayoutPanel6.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel6.ColumnCount = 1;
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel6.Controls.Add(this.tableLayoutPanel5, 0, 2);
            this.tableLayoutPanel6.Controls.Add(this.tableLayoutPanel1, 0, 0);
            this.tableLayoutPanel6.Controls.Add(this.tableLayoutPanel2, 0, 1);
            this.tableLayoutPanel6.Location = new System.Drawing.Point(6, 28);
            this.tableLayoutPanel6.Margin = new System.Windows.Forms.Padding(4);
            this.tableLayoutPanel6.Name = "tableLayoutPanel6";
            this.tableLayoutPanel6.RowCount = 3;
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel6.Size = new System.Drawing.Size(521, 489);
            this.tableLayoutPanel6.TabIndex = 0;
            // 
            // tableLayoutPanel5
            // 
            this.tableLayoutPanel5.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.tableLayoutPanel5.AutoSize = true;
            this.tableLayoutPanel5.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel5.ColumnCount = 2;
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel5.Controls.Add(this.saveBtn, 0, 0);
            this.tableLayoutPanel5.Controls.Add(this.cancelBtn, 1, 0);
            this.tableLayoutPanel5.Location = new System.Drawing.Point(138, 446);
            this.tableLayoutPanel5.Margin = new System.Windows.Forms.Padding(4, 19, 4, 4);
            this.tableLayoutPanel5.MaximumSize = new System.Drawing.Size(500, 62500);
            this.tableLayoutPanel5.Name = "tableLayoutPanel5";
            this.tableLayoutPanel5.RowCount = 1;
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel5.Size = new System.Drawing.Size(244, 39);
            this.tableLayoutPanel5.TabIndex = 2;
            // 
            // saveBtn
            // 
            this.saveBtn.AutoSize = true;
            this.saveBtn.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.saveBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.saveBtn.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.saveBtn.ForeColor = System.Drawing.Color.ForestGreen;
            this.saveBtn.Location = new System.Drawing.Point(22, 2);
            this.saveBtn.Margin = new System.Windows.Forms.Padding(22, 2, 22, 2);
            this.saveBtn.MaximumSize = new System.Drawing.Size(78, 125);
            this.saveBtn.MinimumSize = new System.Drawing.Size(78, 0);
            this.saveBtn.Name = "saveBtn";
            this.saveBtn.Size = new System.Drawing.Size(78, 35);
            this.saveBtn.TabIndex = 0;
            this.saveBtn.Text = "Save";
            this.saveBtn.UseVisualStyleBackColor = true;
            // 
            // cancelBtn
            // 
            this.cancelBtn.AutoSize = true;
            this.cancelBtn.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.cancelBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cancelBtn.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cancelBtn.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.cancelBtn.Location = new System.Drawing.Point(144, 2);
            this.cancelBtn.Margin = new System.Windows.Forms.Padding(22, 2, 22, 2);
            this.cancelBtn.MaximumSize = new System.Drawing.Size(78, 125);
            this.cancelBtn.MinimumSize = new System.Drawing.Size(78, 0);
            this.cancelBtn.Name = "cancelBtn";
            this.cancelBtn.Size = new System.Drawing.Size(78, 35);
            this.cancelBtn.TabIndex = 1;
            this.cancelBtn.Text = "Cancel";
            this.cancelBtn.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.AutoSize = true;
            this.tableLayoutPanel2.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel2.ColumnCount = 3;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Controls.Add(this.label14, 2, 9);
            this.tableLayoutPanel2.Controls.Add(this.biasPercentTxtBox, 1, 9);
            this.tableLayoutPanel2.Controls.Add(this.label13, 0, 9);
            this.tableLayoutPanel2.Controls.Add(this.maxFuelTxtBox, 1, 8);
            this.tableLayoutPanel2.Controls.Add(this.label11, 0, 8);
            this.tableLayoutPanel2.Controls.Add(this.wtUnitLbl1, 2, 4);
            this.tableLayoutPanel2.Controls.Add(this.wtUnitLbl2, 2, 5);
            this.tableLayoutPanel2.Controls.Add(this.wtUnitLbl3, 2, 6);
            this.tableLayoutPanel2.Controls.Add(this.wtUnitLbl4, 2, 7);
            this.tableLayoutPanel2.Controls.Add(this.label10, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.fuelProfileComboBox, 1, 1);
            this.tableLayoutPanel2.Controls.Add(this.label9, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.maxZfwTxtBox, 1, 7);
            this.tableLayoutPanel2.Controls.Add(this.label5, 0, 4);
            this.tableLayoutPanel2.Controls.Add(this.label8, 0, 7);
            this.tableLayoutPanel2.Controls.Add(this.maxLdgWtTxtBox, 1, 6);
            this.tableLayoutPanel2.Controls.Add(this.maxTOWtTxtBox, 1, 5);
            this.tableLayoutPanel2.Controls.Add(this.label3, 0, 3);
            this.tableLayoutPanel2.Controls.Add(this.oewTxtBox, 1, 4);
            this.tableLayoutPanel2.Controls.Add(this.label7, 0, 6);
            this.tableLayoutPanel2.Controls.Add(this.ldgProfileComboBox, 1, 3);
            this.tableLayoutPanel2.Controls.Add(this.toProfileComboBox, 1, 2);
            this.tableLayoutPanel2.Controls.Add(this.label4, 0, 2);
            this.tableLayoutPanel2.Controls.Add(this.label6, 0, 5);
            this.tableLayoutPanel2.Controls.Add(this.wtUnitComboBox, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.wtUnitLbl5, 2, 8);
            this.tableLayoutPanel2.Location = new System.Drawing.Point(2, 75);
            this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(2);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 10;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(517, 350);
            this.tableLayoutPanel2.TabIndex = 1;
            // 
            // label14
            // 
            this.label14.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label14.AutoSize = true;
            this.label14.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label14.Location = new System.Drawing.Point(483, 321);
            this.label14.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(24, 23);
            this.label14.TabIndex = 15;
            this.label14.Text = "%";
            // 
            // biasPercentTxtBox
            // 
            this.biasPercentTxtBox.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.biasPercentTxtBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.biasPercentTxtBox.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.biasPercentTxtBox.Location = new System.Drawing.Point(205, 317);
            this.biasPercentTxtBox.Margin = new System.Windows.Forms.Padding(2);
            this.biasPercentTxtBox.Name = "biasPercentTxtBox";
            this.biasPercentTxtBox.Size = new System.Drawing.Size(273, 30);
            this.biasPercentTxtBox.TabIndex = 14;
            this.biasPercentTxtBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label13
            // 
            this.label13.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label13.Location = new System.Drawing.Point(2, 321);
            this.label13.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(76, 23);
            this.label13.TabIndex = 15;
            this.label13.Text = "Fuel Bias";
            // 
            // maxFuelTxtBox
            // 
            this.maxFuelTxtBox.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.maxFuelTxtBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.maxFuelTxtBox.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.maxFuelTxtBox.Location = new System.Drawing.Point(205, 282);
            this.maxFuelTxtBox.Margin = new System.Windows.Forms.Padding(2);
            this.maxFuelTxtBox.Name = "maxFuelTxtBox";
            this.maxFuelTxtBox.Size = new System.Drawing.Size(273, 30);
            this.maxFuelTxtBox.TabIndex = 12;
            this.maxFuelTxtBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label11
            // 
            this.label11.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.Location = new System.Drawing.Point(2, 286);
            this.label11.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(142, 23);
            this.label11.TabIndex = 12;
            this.label11.Text = "Max fuel capacity";
            // 
            // wtUnitLbl1
            // 
            this.wtUnitLbl1.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.wtUnitLbl1.AutoSize = true;
            this.wtUnitLbl1.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.wtUnitLbl1.Location = new System.Drawing.Point(483, 146);
            this.wtUnitLbl1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.wtUnitLbl1.Name = "wtUnitLbl1";
            this.wtUnitLbl1.Size = new System.Drawing.Size(32, 23);
            this.wtUnitLbl1.TabIndex = 5;
            this.wtUnitLbl1.Text = "KG";
            // 
            // wtUnitLbl2
            // 
            this.wtUnitLbl2.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.wtUnitLbl2.AutoSize = true;
            this.wtUnitLbl2.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.wtUnitLbl2.Location = new System.Drawing.Point(483, 181);
            this.wtUnitLbl2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.wtUnitLbl2.Name = "wtUnitLbl2";
            this.wtUnitLbl2.Size = new System.Drawing.Size(32, 23);
            this.wtUnitLbl2.TabIndex = 7;
            this.wtUnitLbl2.Text = "KG";
            // 
            // wtUnitLbl3
            // 
            this.wtUnitLbl3.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.wtUnitLbl3.AutoSize = true;
            this.wtUnitLbl3.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.wtUnitLbl3.Location = new System.Drawing.Point(483, 216);
            this.wtUnitLbl3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.wtUnitLbl3.Name = "wtUnitLbl3";
            this.wtUnitLbl3.Size = new System.Drawing.Size(32, 23);
            this.wtUnitLbl3.TabIndex = 9;
            this.wtUnitLbl3.Text = "KG";
            // 
            // wtUnitLbl4
            // 
            this.wtUnitLbl4.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.wtUnitLbl4.AutoSize = true;
            this.wtUnitLbl4.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.wtUnitLbl4.Location = new System.Drawing.Point(483, 251);
            this.wtUnitLbl4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.wtUnitLbl4.Name = "wtUnitLbl4";
            this.wtUnitLbl4.Size = new System.Drawing.Size(32, 23);
            this.wtUnitLbl4.TabIndex = 11;
            this.wtUnitLbl4.Text = "KG";
            // 
            // label10
            // 
            this.label10.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(2, 6);
            this.label10.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(100, 23);
            this.label10.TabIndex = 5;
            this.label10.Text = "Weight unit";
            // 
            // fuelProfileComboBox
            // 
            this.fuelProfileComboBox.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.fuelProfileComboBox.BorderColor = System.Drawing.Color.DimGray;
            this.fuelProfileComboBox.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.fuelProfileComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.fuelProfileComboBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.fuelProfileComboBox.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.fuelProfileComboBox.FormattingEnabled = true;
            this.fuelProfileComboBox.Location = new System.Drawing.Point(205, 37);
            this.fuelProfileComboBox.Margin = new System.Windows.Forms.Padding(2);
            this.fuelProfileComboBox.Name = "fuelProfileComboBox";
            this.fuelProfileComboBox.Size = new System.Drawing.Size(274, 31);
            this.fuelProfileComboBox.TabIndex = 1;
            // 
            // label9
            // 
            this.label9.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(2, 41);
            this.label9.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(199, 23);
            this.label9.TabIndex = 10;
            this.label9.Text = "Fuel consumption profile";
            // 
            // maxZfwTxtBox
            // 
            this.maxZfwTxtBox.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.maxZfwTxtBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.maxZfwTxtBox.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.maxZfwTxtBox.Location = new System.Drawing.Point(205, 247);
            this.maxZfwTxtBox.Margin = new System.Windows.Forms.Padding(2);
            this.maxZfwTxtBox.Name = "maxZfwTxtBox";
            this.maxZfwTxtBox.Size = new System.Drawing.Size(273, 30);
            this.maxZfwTxtBox.TabIndex = 10;
            this.maxZfwTxtBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label5
            // 
            this.label5.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(2, 146);
            this.label5.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(196, 23);
            this.label5.TabIndex = 4;
            this.label5.Text = "Operating empty weight";
            // 
            // label8
            // 
            this.label8.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(2, 251);
            this.label8.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(169, 23);
            this.label8.TabIndex = 8;
            this.label8.Text = "Max zero fuel weight";
            // 
            // maxLdgWtTxtBox
            // 
            this.maxLdgWtTxtBox.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.maxLdgWtTxtBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.maxLdgWtTxtBox.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.maxLdgWtTxtBox.Location = new System.Drawing.Point(205, 212);
            this.maxLdgWtTxtBox.Margin = new System.Windows.Forms.Padding(2);
            this.maxLdgWtTxtBox.Name = "maxLdgWtTxtBox";
            this.maxLdgWtTxtBox.Size = new System.Drawing.Size(273, 30);
            this.maxLdgWtTxtBox.TabIndex = 8;
            this.maxLdgWtTxtBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // maxTOWtTxtBox
            // 
            this.maxTOWtTxtBox.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.maxTOWtTxtBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.maxTOWtTxtBox.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.maxTOWtTxtBox.Location = new System.Drawing.Point(205, 177);
            this.maxTOWtTxtBox.Margin = new System.Windows.Forms.Padding(2);
            this.maxTOWtTxtBox.Name = "maxTOWtTxtBox";
            this.maxTOWtTxtBox.Size = new System.Drawing.Size(273, 30);
            this.maxTOWtTxtBox.TabIndex = 6;
            this.maxTOWtTxtBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label3
            // 
            this.label3.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(2, 111);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(124, 23);
            this.label3.TabIndex = 2;
            this.label3.Text = "Landing profile";
            // 
            // oewTxtBox
            // 
            this.oewTxtBox.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.oewTxtBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.oewTxtBox.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.oewTxtBox.Location = new System.Drawing.Point(205, 142);
            this.oewTxtBox.Margin = new System.Windows.Forms.Padding(2);
            this.oewTxtBox.Name = "oewTxtBox";
            this.oewTxtBox.Size = new System.Drawing.Size(273, 30);
            this.oewTxtBox.TabIndex = 4;
            this.oewTxtBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label7
            // 
            this.label7.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(2, 216);
            this.label7.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(160, 23);
            this.label7.TabIndex = 6;
            this.label7.Text = "Max landing weight";
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
            this.ldgProfileComboBox.Location = new System.Drawing.Point(205, 107);
            this.ldgProfileComboBox.Margin = new System.Windows.Forms.Padding(2);
            this.ldgProfileComboBox.Name = "ldgProfileComboBox";
            this.ldgProfileComboBox.Size = new System.Drawing.Size(274, 31);
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
            this.toProfileComboBox.Location = new System.Drawing.Point(205, 72);
            this.toProfileComboBox.Margin = new System.Windows.Forms.Padding(2);
            this.toProfileComboBox.Name = "toProfileComboBox";
            this.toProfileComboBox.Size = new System.Drawing.Size(274, 31);
            this.toProfileComboBox.TabIndex = 2;
            // 
            // label4
            // 
            this.label4.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(2, 76);
            this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(118, 23);
            this.label4.TabIndex = 1;
            this.label4.Text = "Takeoff profile";
            // 
            // label6
            // 
            this.label6.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(2, 181);
            this.label6.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(155, 23);
            this.label6.TabIndex = 5;
            this.label6.Text = "Max takeoff weight";
            // 
            // wtUnitComboBox
            // 
            this.wtUnitComboBox.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.wtUnitComboBox.BorderColor = System.Drawing.Color.DimGray;
            this.wtUnitComboBox.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.wtUnitComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.wtUnitComboBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.wtUnitComboBox.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.wtUnitComboBox.FormattingEnabled = true;
            this.wtUnitComboBox.Location = new System.Drawing.Point(205, 2);
            this.wtUnitComboBox.Margin = new System.Windows.Forms.Padding(2);
            this.wtUnitComboBox.Name = "wtUnitComboBox";
            this.wtUnitComboBox.Size = new System.Drawing.Size(274, 31);
            this.wtUnitComboBox.TabIndex = 0;
            // 
            // wtUnitLbl5
            // 
            this.wtUnitLbl5.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.wtUnitLbl5.AutoSize = true;
            this.wtUnitLbl5.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.wtUnitLbl5.Location = new System.Drawing.Point(483, 286);
            this.wtUnitLbl5.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.wtUnitLbl5.Name = "wtUnitLbl5";
            this.wtUnitLbl5.Size = new System.Drawing.Size(32, 23);
            this.wtUnitLbl5.TabIndex = 13;
            this.wtUnitLbl5.Text = "KG";
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.AutoSize = true;
            this.tableLayoutPanel4.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel4.ColumnCount = 2;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel4.Controls.Add(this.selectionGroupBox, 0, 0);
            this.tableLayoutPanel4.Controls.Add(this.propertyGroupBox, 1, 0);
            this.tableLayoutPanel4.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel4.Margin = new System.Windows.Forms.Padding(4);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 1;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel4.Size = new System.Drawing.Size(941, 550);
            this.tableLayoutPanel4.TabIndex = 4;
            // 
            // AircraftMenuControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(120F, 120F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.Controls.Add(this.tableLayoutPanel4);
            this.DoubleBuffered = true;
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "AircraftMenuControl";
            this.Size = new System.Drawing.Size(945, 554);
            this.selectionGroupBox.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel3.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.propertyGroupBox.ResumeLayout(false);
            this.propertyGroupBox.PerformLayout();
            this.tableLayoutPanel6.ResumeLayout(false);
            this.tableLayoutPanel6.PerformLayout();
            this.tableLayoutPanel5.ResumeLayout(false);
            this.tableLayoutPanel5.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.tableLayoutPanel4.ResumeLayout(false);
            this.tableLayoutPanel4.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox selectionGroupBox;
        private System.Windows.Forms.Label label1;
        private Controls.ComboBoxWithBorder acTypeComboBox;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button deleteBtn;
        private System.Windows.Forms.Button newBtn;
        private System.Windows.Forms.GroupBox propertyGroupBox;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private Controls.ComboBoxWithBorder ldgProfileComboBox;
        private System.Windows.Forms.Label label3;
        private Controls.ComboBoxWithBorder toProfileComboBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox maxLdgWtTxtBox;
        private System.Windows.Forms.TextBox maxTOWtTxtBox;
        private System.Windows.Forms.TextBox oewTxtBox;
        private Controls.ComboBoxWithBorder wtUnitComboBox;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button cancelBtn;
        private System.Windows.Forms.Button saveBtn;
        private System.Windows.Forms.ListView acListView;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.TextBox registrationTxtBox;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.Button editBtn;
        private System.Windows.Forms.TextBox maxZfwTxtBox;
        private System.Windows.Forms.Label label8;
        private Controls.ComboBoxWithBorder fuelProfileComboBox;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label wtUnitLbl1;
        private System.Windows.Forms.Label wtUnitLbl2;
        private System.Windows.Forms.Label wtUnitLbl3;
        private System.Windows.Forms.Label wtUnitLbl4;
        private System.Windows.Forms.TextBox maxFuelTxtBox;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label wtUnitLbl5;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.TextBox biasPercentTxtBox;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel6;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel5;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
    }
}
