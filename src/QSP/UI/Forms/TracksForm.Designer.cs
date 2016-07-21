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
    partial class TracksForm : System.Windows.Forms.Form
    {
        //Form overrides dispose to clean up the component list.
        [System.Diagnostics.DebuggerNonUserCode()]
        protected override void Dispose(bool disposing)
        {
            try
            {
                if (disposing && components != null)
                {
                    components.Dispose();
                }
            }
            finally
            {
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TracksForm));
            this.Label1 = new System.Windows.Forms.Label();
            this.Label3 = new System.Windows.Forms.Label();
            this.Label2 = new System.Windows.Forms.Label();
            this.TableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.PicBoxPacots = new System.Windows.Forms.PictureBox();
            this.PicBoxAusots = new System.Windows.Forms.PictureBox();
            this.BtnNatsDn = new System.Windows.Forms.Button();
            this.BtnPacotsDn = new System.Windows.Forms.Button();
            this.PicBoxNats = new System.Windows.Forms.PictureBox();
            this.BtnAusotsDn = new System.Windows.Forms.Button();
            this.Button1 = new System.Windows.Forms.Button();
            this.Button2 = new System.Windows.Forms.Button();
            this.Button3 = new System.Windows.Forms.Button();
            this.CBoxPacotsEnabled = new QSP.UI.Controls.ComboBoxWithBorder();
            this.CBoxAusotsEnabled = new QSP.UI.Controls.ComboBoxWithBorder();
            this.CBoxNatsEnabled = new QSP.UI.Controls.ComboBoxWithBorder();
            this.ListView1 = new System.Windows.Forms.ListView();
            this.ColumnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ColMessage = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.GroupBox1 = new System.Windows.Forms.GroupBox();
            this.GroupBox2 = new System.Windows.Forms.GroupBox();
            this.TableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PicBoxPacots)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PicBoxAusots)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PicBoxNats)).BeginInit();
            this.GroupBox1.SuspendLayout();
            this.GroupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // Label1
            // 
            this.Label1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.Label1.AutoSize = true;
            this.Label1.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label1.Location = new System.Drawing.Point(75, 8);
            this.Label1.Name = "Label1";
            this.Label1.Size = new System.Drawing.Size(50, 23);
            this.Label1.TabIndex = 1;
            this.Label1.Text = "NATs";
            // 
            // Label3
            // 
            this.Label3.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.Label3.AutoSize = true;
            this.Label3.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label3.Location = new System.Drawing.Point(64, 88);
            this.Label3.Name = "Label3";
            this.Label3.Size = new System.Drawing.Size(71, 23);
            this.Label3.TabIndex = 2;
            this.Label3.Text = "AUSOTs";
            // 
            // Label2
            // 
            this.Label2.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.Label2.AutoSize = true;
            this.Label2.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label2.Location = new System.Drawing.Point(64, 48);
            this.Label2.Name = "Label2";
            this.Label2.Size = new System.Drawing.Size(71, 23);
            this.Label2.TabIndex = 2;
            this.Label2.Text = "PACOTs";
            // 
            // TableLayoutPanel1
            // 
            this.TableLayoutPanel1.ColumnCount = 5;
            this.TableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.TableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 120F));
            this.TableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 120F));
            this.TableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 144F));
            this.TableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 289F));
            this.TableLayoutPanel1.Controls.Add(this.PicBoxPacots, 0, 1);
            this.TableLayoutPanel1.Controls.Add(this.PicBoxAusots, 0, 2);
            this.TableLayoutPanel1.Controls.Add(this.BtnNatsDn, 4, 0);
            this.TableLayoutPanel1.Controls.Add(this.BtnPacotsDn, 4, 1);
            this.TableLayoutPanel1.Controls.Add(this.PicBoxNats, 0, 0);
            this.TableLayoutPanel1.Controls.Add(this.BtnAusotsDn, 4, 2);
            this.TableLayoutPanel1.Controls.Add(this.Button1, 3, 0);
            this.TableLayoutPanel1.Controls.Add(this.Button2, 3, 1);
            this.TableLayoutPanel1.Controls.Add(this.Button3, 3, 2);
            this.TableLayoutPanel1.Controls.Add(this.CBoxPacotsEnabled, 2, 1);
            this.TableLayoutPanel1.Controls.Add(this.CBoxAusotsEnabled, 2, 2);
            this.TableLayoutPanel1.Controls.Add(this.CBoxNatsEnabled, 2, 0);
            this.TableLayoutPanel1.Controls.Add(this.Label1, 1, 0);
            this.TableLayoutPanel1.Controls.Add(this.Label2, 1, 1);
            this.TableLayoutPanel1.Controls.Add(this.Label3, 1, 2);
            this.TableLayoutPanel1.Location = new System.Drawing.Point(11, 21);
            this.TableLayoutPanel1.Name = "TableLayoutPanel1";
            this.TableLayoutPanel1.RowCount = 3;
            this.TableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.TableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.TableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.TableLayoutPanel1.Size = new System.Drawing.Size(713, 120);
            this.TableLayoutPanel1.TabIndex = 6;
            // 
            // PicBoxPacots
            // 
            this.PicBoxPacots.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.PicBoxPacots.Image = ((System.Drawing.Image)(resources.GetObject("PicBoxPacots.Image")));
            this.PicBoxPacots.Location = new System.Drawing.Point(8, 48);
            this.PicBoxPacots.Name = "PicBoxPacots";
            this.PicBoxPacots.Size = new System.Drawing.Size(24, 24);
            this.PicBoxPacots.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.PicBoxPacots.TabIndex = 11;
            this.PicBoxPacots.TabStop = false;
            // 
            // PicBoxAusots
            // 
            this.PicBoxAusots.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.PicBoxAusots.Image = ((System.Drawing.Image)(resources.GetObject("PicBoxAusots.Image")));
            this.PicBoxAusots.Location = new System.Drawing.Point(8, 88);
            this.PicBoxAusots.Name = "PicBoxAusots";
            this.PicBoxAusots.Size = new System.Drawing.Size(24, 24);
            this.PicBoxAusots.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.PicBoxAusots.TabIndex = 12;
            this.PicBoxAusots.TabStop = false;
            // 
            // BtnNatsDn
            // 
            this.BtnNatsDn.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.BtnNatsDn.BackColor = System.Drawing.Color.DarkSlateGray;
            this.BtnNatsDn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnNatsDn.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnNatsDn.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.BtnNatsDn.Location = new System.Drawing.Point(510, 4);
            this.BtnNatsDn.Margin = new System.Windows.Forms.Padding(0);
            this.BtnNatsDn.Name = "BtnNatsDn";
            this.BtnNatsDn.Size = new System.Drawing.Size(116, 32);
            this.BtnNatsDn.TabIndex = 8;
            this.BtnNatsDn.Text = "Download";
            this.BtnNatsDn.UseVisualStyleBackColor = false;
            this.BtnNatsDn.Click += new System.EventHandler(this.BtnNatsDn_Click);
            // 
            // BtnPacotsDn
            // 
            this.BtnPacotsDn.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.BtnPacotsDn.BackColor = System.Drawing.Color.DarkSlateGray;
            this.BtnPacotsDn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnPacotsDn.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnPacotsDn.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.BtnPacotsDn.Location = new System.Drawing.Point(510, 44);
            this.BtnPacotsDn.Margin = new System.Windows.Forms.Padding(0);
            this.BtnPacotsDn.Name = "BtnPacotsDn";
            this.BtnPacotsDn.Size = new System.Drawing.Size(116, 32);
            this.BtnPacotsDn.TabIndex = 9;
            this.BtnPacotsDn.Text = "Download";
            this.BtnPacotsDn.UseVisualStyleBackColor = false;
            this.BtnPacotsDn.Click += new System.EventHandler(this.BtnPacotsDn_Click);
            // 
            // PicBoxNats
            // 
            this.PicBoxNats.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.PicBoxNats.Image = ((System.Drawing.Image)(resources.GetObject("PicBoxNats.Image")));
            this.PicBoxNats.Location = new System.Drawing.Point(8, 8);
            this.PicBoxNats.Name = "PicBoxNats";
            this.PicBoxNats.Size = new System.Drawing.Size(24, 24);
            this.PicBoxNats.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.PicBoxNats.TabIndex = 10;
            this.PicBoxNats.TabStop = false;
            // 
            // BtnAusotsDn
            // 
            this.BtnAusotsDn.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.BtnAusotsDn.BackColor = System.Drawing.Color.DarkSlateGray;
            this.BtnAusotsDn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnAusotsDn.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnAusotsDn.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.BtnAusotsDn.Location = new System.Drawing.Point(510, 84);
            this.BtnAusotsDn.Margin = new System.Windows.Forms.Padding(0);
            this.BtnAusotsDn.Name = "BtnAusotsDn";
            this.BtnAusotsDn.Size = new System.Drawing.Size(116, 32);
            this.BtnAusotsDn.TabIndex = 10;
            this.BtnAusotsDn.Text = "Download";
            this.BtnAusotsDn.UseVisualStyleBackColor = false;
            this.BtnAusotsDn.Click += new System.EventHandler(this.BtnAusotsDn_Click);
            // 
            // Button1
            // 
            this.Button1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.Button1.BackColor = System.Drawing.Color.DarkSlateGray;
            this.Button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Button1.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Button1.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.Button1.Location = new System.Drawing.Point(293, 4);
            this.Button1.Margin = new System.Windows.Forms.Padding(0);
            this.Button1.Name = "Button1";
            this.Button1.Size = new System.Drawing.Size(117, 32);
            this.Button1.TabIndex = 7;
            this.Button1.Text = "View Text";
            this.Button1.UseVisualStyleBackColor = false;
            // 
            // Button2
            // 
            this.Button2.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.Button2.BackColor = System.Drawing.Color.DarkSlateGray;
            this.Button2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Button2.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Button2.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.Button2.Location = new System.Drawing.Point(293, 44);
            this.Button2.Margin = new System.Windows.Forms.Padding(0);
            this.Button2.Name = "Button2";
            this.Button2.Size = new System.Drawing.Size(117, 32);
            this.Button2.TabIndex = 8;
            this.Button2.Text = "View Text";
            this.Button2.UseVisualStyleBackColor = false;
            // 
            // Button3
            // 
            this.Button3.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.Button3.BackColor = System.Drawing.Color.DarkSlateGray;
            this.Button3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Button3.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Button3.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.Button3.Location = new System.Drawing.Point(293, 84);
            this.Button3.Margin = new System.Windows.Forms.Padding(0);
            this.Button3.Name = "Button3";
            this.Button3.Size = new System.Drawing.Size(117, 32);
            this.Button3.TabIndex = 9;
            this.Button3.Text = "View Text";
            this.Button3.UseVisualStyleBackColor = false;
            // 
            // CBoxPacotsEnabled
            // 
            this.CBoxPacotsEnabled.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.CBoxPacotsEnabled.BorderColor = System.Drawing.Color.DimGray;
            this.CBoxPacotsEnabled.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.CBoxPacotsEnabled.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CBoxPacotsEnabled.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.CBoxPacotsEnabled.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CBoxPacotsEnabled.FormattingEnabled = true;
            this.CBoxPacotsEnabled.Items.AddRange(new object[] {
            "Enabled",
            "Disabled"});
            this.CBoxPacotsEnabled.Location = new System.Drawing.Point(163, 44);
            this.CBoxPacotsEnabled.Name = "CBoxPacotsEnabled";
            this.CBoxPacotsEnabled.Size = new System.Drawing.Size(114, 31);
            this.CBoxPacotsEnabled.TabIndex = 7;
            // 
            // CBoxAusotsEnabled
            // 
            this.CBoxAusotsEnabled.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.CBoxAusotsEnabled.BorderColor = System.Drawing.Color.DimGray;
            this.CBoxAusotsEnabled.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.CBoxAusotsEnabled.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CBoxAusotsEnabled.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.CBoxAusotsEnabled.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CBoxAusotsEnabled.FormattingEnabled = true;
            this.CBoxAusotsEnabled.Items.AddRange(new object[] {
            "Enabled",
            "Disabled"});
            this.CBoxAusotsEnabled.Location = new System.Drawing.Point(163, 84);
            this.CBoxAusotsEnabled.Name = "CBoxAusotsEnabled";
            this.CBoxAusotsEnabled.Size = new System.Drawing.Size(114, 31);
            this.CBoxAusotsEnabled.TabIndex = 6;
            // 
            // CBoxNatsEnabled
            // 
            this.CBoxNatsEnabled.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.CBoxNatsEnabled.BorderColor = System.Drawing.Color.DimGray;
            this.CBoxNatsEnabled.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.CBoxNatsEnabled.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CBoxNatsEnabled.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.CBoxNatsEnabled.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CBoxNatsEnabled.FormattingEnabled = true;
            this.CBoxNatsEnabled.Items.AddRange(new object[] {
            "Enabled",
            "Disabled"});
            this.CBoxNatsEnabled.Location = new System.Drawing.Point(163, 4);
            this.CBoxNatsEnabled.Name = "CBoxNatsEnabled";
            this.CBoxNatsEnabled.Size = new System.Drawing.Size(114, 31);
            this.CBoxNatsEnabled.TabIndex = 8;
            // 
            // ListView1
            // 
            this.ListView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.ColumnHeader1,
            this.ColMessage});
            this.ListView1.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ListView1.FullRowSelect = true;
            this.ListView1.Location = new System.Drawing.Point(17, 21);
            this.ListView1.Name = "ListView1";
            this.ListView1.Size = new System.Drawing.Size(713, 183);
            this.ListView1.TabIndex = 7;
            this.ListView1.UseCompatibleStateImageBehavior = false;
            this.ListView1.View = System.Windows.Forms.View.Details;
            // 
            // ColumnHeader1
            // 
            this.ColumnHeader1.Text = "";
            this.ColumnHeader1.Width = 100;
            // 
            // ColMessage
            // 
            this.ColMessage.Text = "Message";
            this.ColMessage.Width = 609;
            // 
            // GroupBox1
            // 
            this.GroupBox1.Controls.Add(this.ListView1);
            this.GroupBox1.Font = new System.Drawing.Font("Segoe UI", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.GroupBox1.Location = new System.Drawing.Point(72, 216);
            this.GroupBox1.Name = "GroupBox1";
            this.GroupBox1.Size = new System.Drawing.Size(736, 210);
            this.GroupBox1.TabIndex = 8;
            this.GroupBox1.TabStop = false;
            this.GroupBox1.Text = "Message";
            // 
            // GroupBox2
            // 
            this.GroupBox2.Controls.Add(this.TableLayoutPanel1);
            this.GroupBox2.Font = new System.Drawing.Font("Segoe UI", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.GroupBox2.Location = new System.Drawing.Point(72, 49);
            this.GroupBox2.Name = "GroupBox2";
            this.GroupBox2.Size = new System.Drawing.Size(736, 161);
            this.GroupBox2.TabIndex = 9;
            this.GroupBox2.TabStop = false;
            this.GroupBox2.Text = "Tracks";
            // 
            // TracksForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(120F, 120F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.ClientSize = new System.Drawing.Size(893, 466);
            this.Controls.Add(this.GroupBox2);
            this.Controls.Add(this.GroupBox1);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Segoe UI", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "TracksForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Tracks Manager";
            this.TableLayoutPanel1.ResumeLayout(false);
            this.TableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PicBoxPacots)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PicBoxAusots)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PicBoxNats)).EndInit();
            this.GroupBox1.ResumeLayout(false);
            this.GroupBox2.ResumeLayout(false);
            this.ResumeLayout(false);

        }
        internal TableLayoutPanel TableLayoutPanel1;
        internal ColumnHeader ColMessage;
        internal ColumnHeader ColumnHeader1;
        private Label Label1;
        private Label Label3;
        private Label Label2;
        private Button BtnNatsDn;
        private Button BtnPacotsDn;
        private Button BtnAusotsDn;
        private Button Button1;
        private Button Button2;
        private Button Button3;
        private QSP.UI.Controls.ComboBoxWithBorder CBoxPacotsEnabled;
        private QSP.UI.Controls.ComboBoxWithBorder CBoxAusotsEnabled;
        private QSP.UI.Controls.ComboBoxWithBorder CBoxNatsEnabled;
        private ListView ListView1;
        private GroupBox GroupBox1;
        private GroupBox GroupBox2;
        private PictureBox PicBoxPacots;
        private PictureBox PicBoxAusots;
        private PictureBox PicBoxNats;
    }
}
