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
			this.OK_Btn = new System.Windows.Forms.Button();
			this.Dest_Txtbox = new System.Windows.Forms.TextBox();
			this.Label2 = new System.Windows.Forms.Label();
			this.MinRwyLength_Txtbox = new System.Windows.Forms.TextBox();
			this.m_ft = new System.Windows.Forms.ComboBox();
			this.Find_Btn = new System.Windows.Forms.Button();
			this.Button1 = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)this.DataGrid).BeginInit();
			this.SuspendLayout();
			//
			//DataGrid
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
			//Label1
			//
			this.Label1.AutoSize = true;
			this.Label1.Font = new System.Drawing.Font("Arial", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
			this.Label1.Location = new System.Drawing.Point(40, 27);
			this.Label1.Name = "Label1";
			this.Label1.Size = new System.Drawing.Size(155, 19);
			this.Label1.TabIndex = 1;
			this.Label1.Text = "Search Alternate for";
			//
			//OK_Btn
			//
			this.OK_Btn.Font = new System.Drawing.Font("Arial", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
			this.OK_Btn.Location = new System.Drawing.Point(133, 369);
			this.OK_Btn.Name = "OK_Btn";
			this.OK_Btn.Size = new System.Drawing.Size(110, 27);
			this.OK_Btn.TabIndex = 2;
			this.OK_Btn.Text = "OK";
			this.OK_Btn.UseVisualStyleBackColor = true;
            this.OK_Btn.Click += new System.EventHandler(this.OK_Btn_Click);
            //
            //Dest_Txtbox
            //
            this.Dest_Txtbox.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
			this.Dest_Txtbox.Font = new System.Drawing.Font("Arial", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
			this.Dest_Txtbox.Location = new System.Drawing.Point(207, 24);
			this.Dest_Txtbox.Name = "Dest_Txtbox";
			this.Dest_Txtbox.Size = new System.Drawing.Size(123, 27);
			this.Dest_Txtbox.TabIndex = 25;
			//
			//Label2
			//
			this.Label2.AutoSize = true;
			this.Label2.Font = new System.Drawing.Font("Arial", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
			this.Label2.Location = new System.Drawing.Point(40, 64);
			this.Label2.Name = "Label2";
			this.Label2.Size = new System.Drawing.Size(198, 19);
			this.Label2.TabIndex = 26;
			this.Label2.Text = "Runway length more than:";
			//
			//MinRwyLength_Txtbox
			//
			this.MinRwyLength_Txtbox.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
			this.MinRwyLength_Txtbox.Font = new System.Drawing.Font("Arial", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
			this.MinRwyLength_Txtbox.Location = new System.Drawing.Point(238, 61);
			this.MinRwyLength_Txtbox.Name = "MinRwyLength_Txtbox";
			this.MinRwyLength_Txtbox.Size = new System.Drawing.Size(89, 27);
			this.MinRwyLength_Txtbox.TabIndex = 27;
			//
			//m_ft
			//
			this.m_ft.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.m_ft.Font = new System.Drawing.Font("Arial", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
			this.m_ft.FormattingEnabled = true;
			this.m_ft.Items.AddRange(new object[] {
				"M",
				"FT"
			});
			this.m_ft.Location = new System.Drawing.Point(339, 64);
			this.m_ft.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
			this.m_ft.Name = "m_ft";
			this.m_ft.Size = new System.Drawing.Size(64, 27);
			this.m_ft.TabIndex = 28;
			//
			//Find_Btn
			//
			this.Find_Btn.Font = new System.Drawing.Font("Arial", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
			this.Find_Btn.Location = new System.Drawing.Point(444, 63);
			this.Find_Btn.Name = "Find_Btn";
			this.Find_Btn.Size = new System.Drawing.Size(110, 27);
			this.Find_Btn.TabIndex = 29;
			this.Find_Btn.Text = "Find";
			this.Find_Btn.UseVisualStyleBackColor = true;
            this.Find_Btn.Click += new System.EventHandler(this. Find_Btn_Click);
            //
            //Button1
            //
            this.Button1.Font = new System.Drawing.Font("Arial", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
			this.Button1.Location = new System.Drawing.Point(326, 369);
			this.Button1.Name = "Button1";
			this.Button1.Size = new System.Drawing.Size(110, 27);
			this.Button1.TabIndex = 30;
			this.Button1.Text = "Cancel";
			this.Button1.UseVisualStyleBackColor = true;
            this.Button1. Click += new System.EventHandler(this. Button1_Click);
            //
            //AltnFinder
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(8f, 16f);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(584, 417);
			this.ControlBox = false;
			this.Controls.Add(this.Button1);
			this.Controls.Add(this.Find_Btn);
			this.Controls.Add(this.m_ft);
			this.Controls.Add(this.MinRwyLength_Txtbox);
			this.Controls.Add(this.Label2);
			this.Controls.Add(this.Dest_Txtbox);
			this.Controls.Add(this.OK_Btn);
			this.Controls.Add(this.Label1);
			this.Controls.Add(this.DataGrid);
			this.Name = "AltnFinder";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Find Alternate";
			this.TopMost = true;
			((System.ComponentModel.ISupportInitialize)this.DataGrid).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		internal System.Windows.Forms.DataGridView DataGrid;
		internal System.Windows.Forms.Label Label1;
        internal System.Windows.Forms.Button OK_Btn;
		internal System.Windows.Forms.TextBox Dest_Txtbox;
		internal System.Windows.Forms.Label Label2;
		internal System.Windows.Forms.TextBox MinRwyLength_Txtbox;
		internal System.Windows.Forms.ComboBox m_ft;
        internal System.Windows.Forms.Button Find_Btn;
        internal System.Windows.Forms.Button Button1;
	}
}
