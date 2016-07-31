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
            this.PicBoxPacots = new System.Windows.Forms.PictureBox();
            this.PicBoxAusots = new System.Windows.Forms.PictureBox();
            this.BtnNatsDn = new System.Windows.Forms.Button();
            this.BtnPacotsDn = new System.Windows.Forms.Button();
            this.PicBoxNats = new System.Windows.Forms.PictureBox();
            this.BtnAusotsDn = new System.Windows.Forms.Button();
            this.viewNatsBtn = new System.Windows.Forms.Button();
            this.viewPacotsBtn = new System.Windows.Forms.Button();
            this.viewAusotsBtn = new System.Windows.Forms.Button();
            this.ListView1 = new System.Windows.Forms.ListView();
            this.ColumnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ColMessage = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.txtRichTextBox = new System.Windows.Forms.RichTextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.importBtn = new System.Windows.Forms.Button();
            this.downloadAllBtn = new System.Windows.Forms.Button();
            this.saveBtn = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.CBoxAusotsEnabled = new QSP.UI.Controls.ComboBoxWithBorder();
            this.CBoxPacotsEnabled = new QSP.UI.Controls.ComboBoxWithBorder();
            this.CBoxNatsEnabled = new QSP.UI.Controls.ComboBoxWithBorder();
            ((System.ComponentModel.ISupportInitialize)(this.PicBoxPacots)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PicBoxAusots)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PicBoxNats)).BeginInit();
            this.tableLayoutPanel2.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // Label1
            // 
            this.Label1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.Label1.AutoSize = true;
            this.Label1.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label1.Location = new System.Drawing.Point(43, 8);
            this.Label1.Name = "Label1";
            this.Label1.Size = new System.Drawing.Size(52, 23);
            this.Label1.TabIndex = 1;
            this.Label1.Text = "NATS";
            // 
            // Label3
            // 
            this.Label3.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.Label3.AutoSize = true;
            this.Label3.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label3.Location = new System.Drawing.Point(33, 88);
            this.Label3.Name = "Label3";
            this.Label3.Size = new System.Drawing.Size(73, 23);
            this.Label3.TabIndex = 2;
            this.Label3.Text = "AUSOTS";
            // 
            // Label2
            // 
            this.Label2.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.Label2.AutoSize = true;
            this.Label2.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label2.Location = new System.Drawing.Point(33, 48);
            this.Label2.Name = "Label2";
            this.Label2.Size = new System.Drawing.Size(73, 23);
            this.Label2.TabIndex = 2;
            this.Label2.Text = "PACOTS";
            // 
            // PicBoxPacots
            // 
            this.PicBoxPacots.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.PicBoxPacots.Image = ((System.Drawing.Image)(resources.GetObject("PicBoxPacots.Image")));
            this.PicBoxPacots.Location = new System.Drawing.Point(3, 48);
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
            this.PicBoxAusots.Location = new System.Drawing.Point(3, 88);
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
            this.BtnNatsDn.Location = new System.Drawing.Point(507, 4);
            this.BtnNatsDn.Margin = new System.Windows.Forms.Padding(0);
            this.BtnNatsDn.Name = "BtnNatsDn";
            this.BtnNatsDn.Size = new System.Drawing.Size(140, 32);
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
            this.BtnPacotsDn.Location = new System.Drawing.Point(507, 44);
            this.BtnPacotsDn.Margin = new System.Windows.Forms.Padding(0);
            this.BtnPacotsDn.Name = "BtnPacotsDn";
            this.BtnPacotsDn.Size = new System.Drawing.Size(140, 32);
            this.BtnPacotsDn.TabIndex = 9;
            this.BtnPacotsDn.Text = "Download";
            this.BtnPacotsDn.UseVisualStyleBackColor = false;
            this.BtnPacotsDn.Click += new System.EventHandler(this.BtnPacotsDn_Click);
            // 
            // PicBoxNats
            // 
            this.PicBoxNats.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.PicBoxNats.Image = ((System.Drawing.Image)(resources.GetObject("PicBoxNats.Image")));
            this.PicBoxNats.Location = new System.Drawing.Point(3, 8);
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
            this.BtnAusotsDn.Location = new System.Drawing.Point(507, 84);
            this.BtnAusotsDn.Margin = new System.Windows.Forms.Padding(0);
            this.BtnAusotsDn.Name = "BtnAusotsDn";
            this.BtnAusotsDn.Size = new System.Drawing.Size(140, 32);
            this.BtnAusotsDn.TabIndex = 10;
            this.BtnAusotsDn.Text = "Download";
            this.BtnAusotsDn.UseVisualStyleBackColor = false;
            this.BtnAusotsDn.Click += new System.EventHandler(this.BtnAusotsDn_Click);
            // 
            // viewNatsBtn
            // 
            this.viewNatsBtn.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.viewNatsBtn.BackColor = System.Drawing.Color.DarkSlateGray;
            this.viewNatsBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.viewNatsBtn.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.viewNatsBtn.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.viewNatsBtn.Location = new System.Drawing.Point(344, 4);
            this.viewNatsBtn.Margin = new System.Windows.Forms.Padding(0);
            this.viewNatsBtn.Name = "viewNatsBtn";
            this.viewNatsBtn.Size = new System.Drawing.Size(117, 32);
            this.viewNatsBtn.TabIndex = 7;
            this.viewNatsBtn.Text = "View Text";
            this.viewNatsBtn.UseVisualStyleBackColor = false;
            // 
            // viewPacotsBtn
            // 
            this.viewPacotsBtn.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.viewPacotsBtn.BackColor = System.Drawing.Color.DarkSlateGray;
            this.viewPacotsBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.viewPacotsBtn.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.viewPacotsBtn.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.viewPacotsBtn.Location = new System.Drawing.Point(344, 44);
            this.viewPacotsBtn.Margin = new System.Windows.Forms.Padding(0);
            this.viewPacotsBtn.Name = "viewPacotsBtn";
            this.viewPacotsBtn.Size = new System.Drawing.Size(117, 32);
            this.viewPacotsBtn.TabIndex = 8;
            this.viewPacotsBtn.Text = "View Text";
            this.viewPacotsBtn.UseVisualStyleBackColor = false;
            // 
            // viewAusotsBtn
            // 
            this.viewAusotsBtn.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.viewAusotsBtn.BackColor = System.Drawing.Color.DarkSlateGray;
            this.viewAusotsBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.viewAusotsBtn.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.viewAusotsBtn.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.viewAusotsBtn.Location = new System.Drawing.Point(344, 84);
            this.viewAusotsBtn.Margin = new System.Windows.Forms.Padding(0);
            this.viewAusotsBtn.Name = "viewAusotsBtn";
            this.viewAusotsBtn.Size = new System.Drawing.Size(117, 32);
            this.viewAusotsBtn.TabIndex = 9;
            this.viewAusotsBtn.Text = "View Text";
            this.viewAusotsBtn.UseVisualStyleBackColor = false;
            // 
            // ListView1
            // 
            this.ListView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.ColumnHeader1,
            this.ColMessage});
            this.ListView1.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ListView1.FullRowSelect = true;
            this.ListView1.Location = new System.Drawing.Point(8, 25);
            this.ListView1.Name = "ListView1";
            this.ListView1.Size = new System.Drawing.Size(970, 183);
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
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.AutoSize = true;
            this.tableLayoutPanel2.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel2.ColumnCount = 1;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Controls.Add(this.groupBox2, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.groupBox3, 0, 2);
            this.tableLayoutPanel2.Controls.Add(this.groupBox1, 0, 0);
            this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 3;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(990, 698);
            this.tableLayoutPanel2.TabIndex = 8;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.ListView1);
            this.groupBox2.Location = new System.Drawing.Point(3, 218);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(984, 226);
            this.groupBox2.TabIndex = 10;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Messages";
            // 
            // groupBox3
            // 
            this.groupBox3.AutoSize = true;
            this.groupBox3.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.groupBox3.Controls.Add(this.txtRichTextBox);
            this.groupBox3.Location = new System.Drawing.Point(3, 450);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(984, 245);
            this.groupBox3.TabIndex = 11;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Text";
            // 
            // txtRichTextBox
            // 
            this.txtRichTextBox.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.txtRichTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtRichTextBox.DetectUrls = false;
            this.txtRichTextBox.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtRichTextBox.Location = new System.Drawing.Point(8, 21);
            this.txtRichTextBox.MinimumSize = new System.Drawing.Size(970, 200);
            this.txtRichTextBox.Name = "txtRichTextBox";
            this.txtRichTextBox.ReadOnly = true;
            this.txtRichTextBox.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.None;
            this.txtRichTextBox.Size = new System.Drawing.Size(970, 200);
            this.txtRichTextBox.TabIndex = 0;
            this.txtRichTextBox.Text = "";
            this.txtRichTextBox.ContentsResized += new System.Windows.Forms.ContentsResizedEventHandler(this.TxtRichTextBoxContentsResized);
            // 
            // groupBox1
            // 
            this.groupBox1.AutoSize = true;
            this.groupBox1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.groupBox1.Controls.Add(this.tableLayoutPanel1);
            this.groupBox1.Location = new System.Drawing.Point(3, 3);
            this.groupBox1.MinimumSize = new System.Drawing.Size(984, 190);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(984, 209);
            this.groupBox1.TabIndex = 9;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Tracks";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.AutoSize = true;
            this.tableLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel4, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.label4, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel3, 0, 2);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(9, 24);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel1.MinimumSize = new System.Drawing.Size(500, 160);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(965, 164);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.AutoSize = true;
            this.tableLayoutPanel4.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel4.ColumnCount = 3;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel4.Controls.Add(this.importBtn, 2, 0);
            this.tableLayoutPanel4.Controls.Add(this.downloadAllBtn, 0, 0);
            this.tableLayoutPanel4.Controls.Add(this.saveBtn, 1, 0);
            this.tableLayoutPanel4.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel4.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 1;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel4.Size = new System.Drawing.Size(480, 32);
            this.tableLayoutPanel4.TabIndex = 9;
            // 
            // importBtn
            // 
            this.importBtn.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.importBtn.BackColor = System.Drawing.Color.DarkSlateGray;
            this.importBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.importBtn.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.importBtn.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.importBtn.Location = new System.Drawing.Point(330, 0);
            this.importBtn.Margin = new System.Windows.Forms.Padding(10, 0, 10, 0);
            this.importBtn.Name = "importBtn";
            this.importBtn.Size = new System.Drawing.Size(140, 32);
            this.importBtn.TabIndex = 15;
            this.importBtn.Text = "Import";
            this.importBtn.UseVisualStyleBackColor = false;
            this.importBtn.Visible = false;
            // 
            // downloadAllBtn
            // 
            this.downloadAllBtn.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.downloadAllBtn.BackColor = System.Drawing.Color.DarkSlateGray;
            this.downloadAllBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.downloadAllBtn.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.downloadAllBtn.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.downloadAllBtn.Location = new System.Drawing.Point(10, 0);
            this.downloadAllBtn.Margin = new System.Windows.Forms.Padding(10, 0, 10, 0);
            this.downloadAllBtn.Name = "downloadAllBtn";
            this.downloadAllBtn.Size = new System.Drawing.Size(140, 32);
            this.downloadAllBtn.TabIndex = 13;
            this.downloadAllBtn.Text = "Download All";
            this.downloadAllBtn.UseVisualStyleBackColor = false;
            this.downloadAllBtn.Click += new System.EventHandler(this.DownloadAllBtnClick);
            // 
            // saveBtn
            // 
            this.saveBtn.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.saveBtn.BackColor = System.Drawing.Color.DarkSlateGray;
            this.saveBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.saveBtn.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.saveBtn.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.saveBtn.Location = new System.Drawing.Point(170, 0);
            this.saveBtn.Margin = new System.Windows.Forms.Padding(10, 0, 10, 0);
            this.saveBtn.Name = "saveBtn";
            this.saveBtn.Size = new System.Drawing.Size(140, 32);
            this.saveBtn.TabIndex = 14;
            this.saveBtn.Text = "Save";
            this.saveBtn.UseVisualStyleBackColor = false;
            this.saveBtn.Visible = false;
            // 
            // label4
            // 
            this.label4.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label4.Location = new System.Drawing.Point(5, 37);
            this.label4.Margin = new System.Windows.Forms.Padding(5);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(955, 2);
            this.label4.TabIndex = 10;
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.AutoSize = true;
            this.tableLayoutPanel3.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel3.ColumnCount = 5;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 175F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 175F));
            this.tableLayoutPanel3.Controls.Add(this.BtnAusotsDn, 4, 2);
            this.tableLayoutPanel3.Controls.Add(this.BtnPacotsDn, 4, 1);
            this.tableLayoutPanel3.Controls.Add(this.BtnNatsDn, 4, 0);
            this.tableLayoutPanel3.Controls.Add(this.PicBoxAusots, 0, 2);
            this.tableLayoutPanel3.Controls.Add(this.PicBoxPacots, 0, 1);
            this.tableLayoutPanel3.Controls.Add(this.PicBoxNats, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.viewAusotsBtn, 3, 2);
            this.tableLayoutPanel3.Controls.Add(this.viewPacotsBtn, 3, 1);
            this.tableLayoutPanel3.Controls.Add(this.viewNatsBtn, 3, 0);
            this.tableLayoutPanel3.Controls.Add(this.Label1, 1, 0);
            this.tableLayoutPanel3.Controls.Add(this.Label2, 1, 1);
            this.tableLayoutPanel3.Controls.Add(this.Label3, 1, 2);
            this.tableLayoutPanel3.Controls.Add(this.CBoxAusotsEnabled, 2, 2);
            this.tableLayoutPanel3.Controls.Add(this.CBoxPacotsEnabled, 2, 1);
            this.tableLayoutPanel3.Controls.Add(this.CBoxNatsEnabled, 2, 0);
            this.tableLayoutPanel3.Location = new System.Drawing.Point(0, 44);
            this.tableLayoutPanel3.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 3;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(665, 120);
            this.tableLayoutPanel3.TabIndex = 8;
            // 
            // panel1
            // 
            this.panel1.AutoScroll = true;
            this.panel1.Controls.Add(this.tableLayoutPanel2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1055, 710);
            this.panel1.TabIndex = 9;
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
            this.CBoxAusotsEnabled.Location = new System.Drawing.Point(112, 84);
            this.CBoxAusotsEnabled.Name = "CBoxAusotsEnabled";
            this.CBoxAusotsEnabled.Size = new System.Drawing.Size(200, 31);
            this.CBoxAusotsEnabled.TabIndex = 6;
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
            this.CBoxPacotsEnabled.Location = new System.Drawing.Point(112, 44);
            this.CBoxPacotsEnabled.Name = "CBoxPacotsEnabled";
            this.CBoxPacotsEnabled.Size = new System.Drawing.Size(200, 31);
            this.CBoxPacotsEnabled.TabIndex = 7;
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
            this.CBoxNatsEnabled.Location = new System.Drawing.Point(112, 4);
            this.CBoxNatsEnabled.Name = "CBoxNatsEnabled";
            this.CBoxNatsEnabled.Size = new System.Drawing.Size(200, 31);
            this.CBoxNatsEnabled.TabIndex = 8;
            // 
            // TracksForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(120F, 120F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.ClientSize = new System.Drawing.Size(1055, 710);
            this.Controls.Add(this.panel1);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Segoe UI", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "TracksForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Tracks Manager";
            ((System.ComponentModel.ISupportInitialize)(this.PicBoxPacots)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PicBoxAusots)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PicBoxNats)).EndInit();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.tableLayoutPanel4.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel3.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }
        private Label Label1;
        private Label Label3;
        private Label Label2;
        private Button BtnNatsDn;
        private Button BtnPacotsDn;
        private Button BtnAusotsDn;
        private Button viewNatsBtn;
        private Button viewPacotsBtn;
        private Button viewAusotsBtn;
        private QSP.UI.Controls.ComboBoxWithBorder CBoxPacotsEnabled;
        private QSP.UI.Controls.ComboBoxWithBorder CBoxAusotsEnabled;
        private QSP.UI.Controls.ComboBoxWithBorder CBoxNatsEnabled;
        private PictureBox PicBoxPacots;
        private PictureBox PicBoxAusots;
        private PictureBox PicBoxNats;
        private ListView ListView1;
        internal ColumnHeader ColumnHeader1;
        internal ColumnHeader ColMessage;
        private TableLayoutPanel tableLayoutPanel2;
        private Panel panel1;
        private GroupBox groupBox1;
        private TableLayoutPanel tableLayoutPanel3;
        private GroupBox groupBox2;
        private GroupBox groupBox3;
        private RichTextBox txtRichTextBox;
        private TableLayoutPanel tableLayoutPanel1;
        private TableLayoutPanel tableLayoutPanel4;
        private Button importBtn;
        private Button downloadAllBtn;
        private Button saveBtn;
        private Label label4;
    }
}
