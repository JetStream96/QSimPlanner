using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using System.Linq;
using System.Xml.Linq;

namespace QSP.UI.Views.FuelPlan.Route
{
	partial class FindAltnForm : System.Windows.Forms.Form
	{

		//Form overrides dispose to clean up the component list.
		[System.Diagnostics.DebuggerNonUserCode()]
		protected override void Dispose(bool disposing)
		{
			try {
				if (disposing && components != null) {
					components.Dispose();
				}
			} finally {
				base.Dispose(disposing);
			}
		}

		//Required by the Windows Form Designer

		private System.ComponentModel.IContainer components=null;
		//NOTE: The following procedure is required by the Windows Form Designer
		//It can be modified using the Windows Form Designer.  
		//Do not modify it using the code editor.
		[System.Diagnostics.DebuggerStepThrough()]
		private void InitializeComponent()
		{
            this.DataGrid = new System.Windows.Forms.DataGridView();
            this.Label1 = new System.Windows.Forms.Label();
            this.OkBtn = new System.Windows.Forms.Button();
            this.icaoTxtbox = new System.Windows.Forms.TextBox();
            this.Label2 = new System.Windows.Forms.Label();
            this.minRwyLengthTxtbox = new System.Windows.Forms.TextBox();
            this.findBtn = new System.Windows.Forms.Button();
            this.CancelBtn = new System.Windows.Forms.Button();
            this.lengthUnitComboBox = new ComboBoxWithBorder();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            ((System.ComponentModel.ISupportInitialize)(this.DataGrid)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // DataGrid
            // 
            this.DataGrid.AllowUserToAddRows = false;
            this.DataGrid.AllowUserToDeleteRows = false;
            this.DataGrid.AllowUserToResizeColumns = false;
            this.DataGrid.AllowUserToResizeRows = false;
            this.DataGrid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.DataGrid.BackgroundColor = System.Drawing.SystemColors.ButtonHighlight;
            this.DataGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DataGrid.Location = new System.Drawing.Point(16, 106);
            this.DataGrid.Margin = new System.Windows.Forms.Padding(2);
            this.DataGrid.MultiSelect = false;
            this.DataGrid.Name = "DataGrid";
            this.DataGrid.ReadOnly = true;
            this.DataGrid.RowHeadersVisible = false;
            this.DataGrid.RowTemplate.Height = 24;
            this.DataGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.DataGrid.Size = new System.Drawing.Size(538, 250);
            this.DataGrid.TabIndex = 1;
            // 
            // Label1
            // 
            this.Label1.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.Label1.AutoSize = true;
            this.Label1.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label1.Location = new System.Drawing.Point(2, 5);
            this.Label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.Label1.Name = "Label1";
            this.Label1.Size = new System.Drawing.Size(162, 23);
            this.Label1.TabIndex = 1;
            this.Label1.Text = "Search Alternate for";
            // 
            // OkBtn
            // 
            this.OkBtn.AutoSize = true;
            this.OkBtn.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.OkBtn.BackColor = System.Drawing.Color.DarkGreen;
            this.OkBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.OkBtn.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.OkBtn.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.OkBtn.Location = new System.Drawing.Point(132, 388);
            this.OkBtn.Margin = new System.Windows.Forms.Padding(2);
            this.OkBtn.MinimumSize = new System.Drawing.Size(112, 38);
            this.OkBtn.Name = "OkBtn";
            this.OkBtn.Size = new System.Drawing.Size(112, 38);
            this.OkBtn.TabIndex = 2;
            this.OkBtn.Text = "OK";
            this.OkBtn.UseVisualStyleBackColor = false;
            this.OkBtn.Click += new System.EventHandler(this.OkBtn_Click);
            // 
            // icaoTxtbox
            // 
            this.icaoTxtbox.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.icaoTxtbox.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.icaoTxtbox.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.icaoTxtbox.Location = new System.Drawing.Point(218, 2);
            this.icaoTxtbox.Margin = new System.Windows.Forms.Padding(2);
            this.icaoTxtbox.Name = "icaoTxtbox";
            this.icaoTxtbox.Size = new System.Drawing.Size(100, 30);
            this.icaoTxtbox.TabIndex = 0;
            // 
            // Label2
            // 
            this.Label2.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.Label2.AutoSize = true;
            this.Label2.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label2.Location = new System.Drawing.Point(2, 42);
            this.Label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.Label2.Name = "Label2";
            this.Label2.Size = new System.Drawing.Size(212, 23);
            this.Label2.TabIndex = 26;
            this.Label2.Text = "Runway length more than:";
            // 
            // minRwyLengthTxtbox
            // 
            this.minRwyLengthTxtbox.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.minRwyLengthTxtbox.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.minRwyLengthTxtbox.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.minRwyLengthTxtbox.Location = new System.Drawing.Point(218, 38);
            this.minRwyLengthTxtbox.Margin = new System.Windows.Forms.Padding(2);
            this.minRwyLengthTxtbox.Name = "minRwyLengthTxtbox";
            this.minRwyLengthTxtbox.Size = new System.Drawing.Size(100, 30);
            this.minRwyLengthTxtbox.TabIndex = 1;
            // 
            // findBtn
            // 
            this.findBtn.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.findBtn.AutoSize = true;
            this.findBtn.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.findBtn.BackColor = System.Drawing.Color.DarkSlateGray;
            this.findBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.findBtn.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.findBtn.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.findBtn.Location = new System.Drawing.Point(429, 36);
            this.findBtn.Margin = new System.Windows.Forms.Padding(10, 2, 10, 2);
            this.findBtn.Name = "findBtn";
            this.findBtn.Padding = new System.Windows.Forms.Padding(15, 0, 15, 0);
            this.findBtn.Size = new System.Drawing.Size(84, 35);
            this.findBtn.TabIndex = 3;
            this.findBtn.Text = "Find";
            this.findBtn.UseVisualStyleBackColor = false;
            this.findBtn.Click += new System.EventHandler(this.FindBtnClick);
            // 
            // CancelBtn
            // 
            this.CancelBtn.AutoSize = true;
            this.CancelBtn.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.CancelBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.CancelBtn.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CancelBtn.Location = new System.Drawing.Point(326, 388);
            this.CancelBtn.Margin = new System.Windows.Forms.Padding(2);
            this.CancelBtn.MinimumSize = new System.Drawing.Size(112, 38);
            this.CancelBtn.Name = "CancelBtn";
            this.CancelBtn.Size = new System.Drawing.Size(112, 38);
            this.CancelBtn.TabIndex = 3;
            this.CancelBtn.Text = "Cancel";
            this.CancelBtn.UseVisualStyleBackColor = true;
            this.CancelBtn.Click += new System.EventHandler(this.CancelBtn_Click);
            // 
            // lengthUnitComboBox
            // 
            this.lengthUnitComboBox.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lengthUnitComboBox.BorderColor = System.Drawing.Color.DimGray;
            this.lengthUnitComboBox.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.lengthUnitComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.lengthUnitComboBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lengthUnitComboBox.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lengthUnitComboBox.FormattingEnabled = true;
            this.lengthUnitComboBox.Items.AddRange(new object[] {
            "M",
            "FT"});
            this.lengthUnitComboBox.Location = new System.Drawing.Point(330, 38);
            this.lengthUnitComboBox.Margin = new System.Windows.Forms.Padding(10, 2, 10, 2);
            this.lengthUnitComboBox.Name = "lengthUnitComboBox";
            this.lengthUnitComboBox.Size = new System.Drawing.Size(79, 31);
            this.lengthUnitComboBox.TabIndex = 2;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.AutoSize = true;
            this.tableLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel1.ColumnCount = 4;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.Label1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.lengthUnitComboBox, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.findBtn, 3, 1);
            this.tableLayoutPanel1.Controls.Add(this.icaoTxtbox, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.Label2, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.minRwyLengthTxtbox, 1, 1);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(16, 12);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(2);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(523, 73);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // FindAltnForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(120F, 120F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.ClientSize = new System.Drawing.Size(572, 439);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.CancelBtn);
            this.Controls.Add(this.OkBtn);
            this.Controls.Add(this.DataGrid);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Segoe UI", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "FindAltnForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Find Alternate";
            ((System.ComponentModel.ISupportInitialize)(this.DataGrid)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

        private DataGridView DataGrid;
        private Label Label1;
        private Button OkBtn;
        private TextBox icaoTxtbox;
        private Label Label2;
        private TextBox minRwyLengthTxtbox;
        private Button findBtn;
        private Button CancelBtn;
        private ComboBoxWithBorder lengthUnitComboBox;
        private TableLayoutPanel tableLayoutPanel1;
    }
}
