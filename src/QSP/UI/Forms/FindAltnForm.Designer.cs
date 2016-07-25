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

namespace QSP.UI.Forms
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
            this.lengthUnitComboBox = new QSP.UI.Controls.ComboBoxWithBorder();
            ((System.ComponentModel.ISupportInitialize)(this.DataGrid)).BeginInit();
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
            this.DataGrid.Location = new System.Drawing.Point(16, 96);
            this.DataGrid.MultiSelect = false;
            this.DataGrid.Name = "DataGrid";
            this.DataGrid.ReadOnly = true;
            this.DataGrid.RowHeadersVisible = false;
            this.DataGrid.RowTemplate.Height = 24;
            this.DataGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.DataGrid.Size = new System.Drawing.Size(538, 250);
            this.DataGrid.TabIndex = 0;
            // 
            // Label1
            // 
            this.Label1.AutoSize = true;
            this.Label1.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label1.Location = new System.Drawing.Point(12, 24);
            this.Label1.Name = "Label1";
            this.Label1.Size = new System.Drawing.Size(162, 23);
            this.Label1.TabIndex = 1;
            this.Label1.Text = "Search Alternate for";
            // 
            // OkBtn
            // 
            this.OkBtn.BackColor = System.Drawing.Color.DarkGreen;
            this.OkBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.OkBtn.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.OkBtn.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.OkBtn.Location = new System.Drawing.Point(133, 369);
            this.OkBtn.Name = "OkBtn";
            this.OkBtn.Size = new System.Drawing.Size(110, 33);
            this.OkBtn.TabIndex = 2;
            this.OkBtn.Text = "OK";
            this.OkBtn.UseVisualStyleBackColor = false;
            this.OkBtn.Click += new System.EventHandler(this.OkBtn_Click);
            // 
            // icaoTxtbox
            // 
            this.icaoTxtbox.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.icaoTxtbox.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.icaoTxtbox.Location = new System.Drawing.Point(227, 21);
            this.icaoTxtbox.Name = "icaoTxtbox";
            this.icaoTxtbox.Size = new System.Drawing.Size(100, 30);
            this.icaoTxtbox.TabIndex = 25;
            // 
            // Label2
            // 
            this.Label2.AutoSize = true;
            this.Label2.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label2.Location = new System.Drawing.Point(12, 61);
            this.Label2.Name = "Label2";
            this.Label2.Size = new System.Drawing.Size(212, 23);
            this.Label2.TabIndex = 26;
            this.Label2.Text = "Runway length more than:";
            // 
            // minRwyLengthTxtbox
            // 
            this.minRwyLengthTxtbox.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.minRwyLengthTxtbox.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.minRwyLengthTxtbox.Location = new System.Drawing.Point(227, 58);
            this.minRwyLengthTxtbox.Name = "minRwyLengthTxtbox";
            this.minRwyLengthTxtbox.Size = new System.Drawing.Size(100, 30);
            this.minRwyLengthTxtbox.TabIndex = 27;
            // 
            // findBtn
            // 
            this.findBtn.BackColor = System.Drawing.Color.DarkSlateGray;
            this.findBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.findBtn.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.findBtn.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.findBtn.Location = new System.Drawing.Point(444, 59);
            this.findBtn.Name = "findBtn";
            this.findBtn.Size = new System.Drawing.Size(110, 31);
            this.findBtn.TabIndex = 29;
            this.findBtn.Text = "Find";
            this.findBtn.UseVisualStyleBackColor = false;
            this.findBtn.Click += new System.EventHandler(this.FindBtnClick);
            // 
            // CancelBtn
            // 
            this.CancelBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.CancelBtn.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CancelBtn.Location = new System.Drawing.Point(326, 369);
            this.CancelBtn.Name = "CancelBtn";
            this.CancelBtn.Size = new System.Drawing.Size(110, 33);
            this.CancelBtn.TabIndex = 30;
            this.CancelBtn.Text = "Cancel";
            this.CancelBtn.UseVisualStyleBackColor = true;
            this.CancelBtn.Click += new System.EventHandler(this.CancelBtn_Click);
            // 
            // lengthUnitComboBox
            // 
            this.lengthUnitComboBox.BorderColor = System.Drawing.Color.DimGray;
            this.lengthUnitComboBox.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.lengthUnitComboBox.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lengthUnitComboBox.FormattingEnabled = true;
            this.lengthUnitComboBox.Items.AddRange(new object[] {
            "M",
            "FT"});
            this.lengthUnitComboBox.Location = new System.Drawing.Point(333, 59);
            this.lengthUnitComboBox.Name = "lengthUnitComboBox";
            this.lengthUnitComboBox.Size = new System.Drawing.Size(66, 31);
            this.lengthUnitComboBox.TabIndex = 31;
            // 
            // FindAltnForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(120F, 120F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.ClientSize = new System.Drawing.Size(573, 417);
            this.Controls.Add(this.lengthUnitComboBox);
            this.Controls.Add(this.CancelBtn);
            this.Controls.Add(this.findBtn);
            this.Controls.Add(this.minRwyLengthTxtbox);
            this.Controls.Add(this.Label2);
            this.Controls.Add(this.icaoTxtbox);
            this.Controls.Add(this.OkBtn);
            this.Controls.Add(this.Label1);
            this.Controls.Add(this.DataGrid);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Segoe UI", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "FindAltnForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Find Alternate";
            ((System.ComponentModel.ISupportInitialize)(this.DataGrid)).EndInit();
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
        private UI.Controls.ComboBoxWithBorder lengthUnitComboBox;
    }
}
