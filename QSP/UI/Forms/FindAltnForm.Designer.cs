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
namespace QSP
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
            this.DestTxtbox = new System.Windows.Forms.TextBox();
            this.Label2 = new System.Windows.Forms.Label();
            this.MinRwyLengthTxtbox = new System.Windows.Forms.TextBox();
            this.lengthUnitComboBox = new System.Windows.Forms.ComboBox();
            this.FindBtn = new System.Windows.Forms.Button();
            this.CancelBtn = new System.Windows.Forms.Button();
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
            this.DataGrid.Location = new System.Drawing.Point(34, 99);
            this.DataGrid.MultiSelect = false;
            this.DataGrid.Name = "DataGrid";
            this.DataGrid.ReadOnly = true;
            this.DataGrid.RowHeadersVisible = false;
            this.DataGrid.RowTemplate.Height = 24;
            this.DataGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.DataGrid.Size = new System.Drawing.Size(520, 250);
            this.DataGrid.TabIndex = 0;
            // 
            // Label1
            // 
            this.Label1.AutoSize = true;
            this.Label1.Font = new System.Drawing.Font("Arial", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label1.Location = new System.Drawing.Point(40, 27);
            this.Label1.Name = "Label1";
            this.Label1.Size = new System.Drawing.Size(155, 19);
            this.Label1.TabIndex = 1;
            this.Label1.Text = "Search Alternate for";
            // 
            // OkBtn
            // 
            this.OkBtn.Font = new System.Drawing.Font("Arial", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.OkBtn.Location = new System.Drawing.Point(133, 369);
            this.OkBtn.Name = "OkBtn";
            this.OkBtn.Size = new System.Drawing.Size(110, 27);
            this.OkBtn.TabIndex = 2;
            this.OkBtn.Text = "OK";
            this.OkBtn.UseVisualStyleBackColor = true;
            this.OkBtn.Click += new System.EventHandler(this.OkBtn_Click);
            // 
            // DestTxtbox
            // 
            this.DestTxtbox.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.DestTxtbox.Font = new System.Drawing.Font("Arial", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DestTxtbox.Location = new System.Drawing.Point(207, 24);
            this.DestTxtbox.Name = "DestTxtbox";
            this.DestTxtbox.Size = new System.Drawing.Size(123, 27);
            this.DestTxtbox.TabIndex = 25;
            // 
            // Label2
            // 
            this.Label2.AutoSize = true;
            this.Label2.Font = new System.Drawing.Font("Arial", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label2.Location = new System.Drawing.Point(40, 64);
            this.Label2.Name = "Label2";
            this.Label2.Size = new System.Drawing.Size(198, 19);
            this.Label2.TabIndex = 26;
            this.Label2.Text = "Runway length more than:";
            // 
            // MinRwyLengthTxtbox
            // 
            this.MinRwyLengthTxtbox.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.MinRwyLengthTxtbox.Font = new System.Drawing.Font("Arial", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MinRwyLengthTxtbox.Location = new System.Drawing.Point(238, 61);
            this.MinRwyLengthTxtbox.Name = "MinRwyLengthTxtbox";
            this.MinRwyLengthTxtbox.Size = new System.Drawing.Size(89, 27);
            this.MinRwyLengthTxtbox.TabIndex = 27;
            // 
            // lengthUnitComboBox
            // 
            this.lengthUnitComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.lengthUnitComboBox.Font = new System.Drawing.Font("Arial", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lengthUnitComboBox.FormattingEnabled = true;
            this.lengthUnitComboBox.Items.AddRange(new object[] {
            "M",
            "FT"});
            this.lengthUnitComboBox.Location = new System.Drawing.Point(339, 64);
            this.lengthUnitComboBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.lengthUnitComboBox.Name = "lengthUnitComboBox";
            this.lengthUnitComboBox.Size = new System.Drawing.Size(64, 27);
            this.lengthUnitComboBox.TabIndex = 28;
            // 
            // FindBtn
            // 
            this.FindBtn.Font = new System.Drawing.Font("Arial", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FindBtn.Location = new System.Drawing.Point(444, 63);
            this.FindBtn.Name = "FindBtn";
            this.FindBtn.Size = new System.Drawing.Size(110, 27);
            this.FindBtn.TabIndex = 29;
            this.FindBtn.Text = "Find";
            this.FindBtn.UseVisualStyleBackColor = true;
            this.FindBtn.Click += new System.EventHandler(this.FindBtn_Click);
            // 
            // CancelBtn
            // 
            this.CancelBtn.Font = new System.Drawing.Font("Arial", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CancelBtn.Location = new System.Drawing.Point(326, 369);
            this.CancelBtn.Name = "CancelBtn";
            this.CancelBtn.Size = new System.Drawing.Size(110, 27);
            this.CancelBtn.TabIndex = 30;
            this.CancelBtn.Text = "Cancel";
            this.CancelBtn.UseVisualStyleBackColor = true;
            this.CancelBtn.Click += new System.EventHandler(this.CancelBtn_Click);
            // 
            // FindAltnForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(120F, 120F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(584, 417);
            this.ControlBox = false;
            this.Controls.Add(this.CancelBtn);
            this.Controls.Add(this.FindBtn);
            this.Controls.Add(this.lengthUnitComboBox);
            this.Controls.Add(this.MinRwyLengthTxtbox);
            this.Controls.Add(this.Label2);
            this.Controls.Add(this.DestTxtbox);
            this.Controls.Add(this.OkBtn);
            this.Controls.Add(this.Label1);
            this.Controls.Add(this.DataGrid);
            this.Name = "FindAltnForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Find Alternate";
            this.TopMost = true;
            ((System.ComponentModel.ISupportInitialize)(this.DataGrid)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		internal System.Windows.Forms.DataGridView DataGrid;
		internal System.Windows.Forms.Label Label1;
        internal System.Windows.Forms.Button OkBtn;
		internal System.Windows.Forms.TextBox DestTxtbox;
		internal System.Windows.Forms.Label Label2;
		internal System.Windows.Forms.TextBox MinRwyLengthTxtbox;
		internal System.Windows.Forms.ComboBox lengthUnitComboBox;
        internal System.Windows.Forms.Button FindBtn;
        internal System.Windows.Forms.Button CancelBtn;
	}
}
