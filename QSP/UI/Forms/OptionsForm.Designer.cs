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
    partial class OptionsForm : System.Windows.Forms.Form
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OptionsForm));
            this.GroupBox1 = new System.Windows.Forms.GroupBox();
            this.browseFolderBtn = new System.Windows.Forms.Button();
            this.Label13 = new System.Windows.Forms.Label();
            this.Expired_Lbl = new System.Windows.Forms.Label();
            this.DBPath_TxtBox = new System.Windows.Forms.TextBox();
            this.AiracPeriod_Lbl = new System.Windows.Forms.Label();
            this.Label1 = new System.Windows.Forms.Label();
            this.Label5 = new System.Windows.Forms.Label();
            this.Label14 = new System.Windows.Forms.Label();
            this.Label6 = new System.Windows.Forms.Label();
            this.DBFound_Lbl = new System.Windows.Forms.Label();
            this.Airac_Lbl = new System.Windows.Forms.Label();
            this.FolderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.ok_button = new System.Windows.Forms.Button();
            this.cancel_button = new System.Windows.Forms.Button();
            this.GroupBox2 = new System.Windows.Forms.GroupBox();
            this.Button3 = new System.Windows.Forms.Button();
            this.CheckBox3 = new System.Windows.Forms.CheckBox();
            this.TextBox3 = new System.Windows.Forms.TextBox();
            this.Button2 = new System.Windows.Forms.Button();
            this.CheckBox2 = new System.Windows.Forms.CheckBox();
            this.TextBox2 = new System.Windows.Forms.TextBox();
            this.Button1 = new System.Windows.Forms.Button();
            this.CheckBox1 = new System.Windows.Forms.CheckBox();
            this.TextBox1 = new System.Windows.Forms.TextBox();
            this.GroupBox3 = new System.Windows.Forms.GroupBox();
            this.DoubleCheckWhenExit_CheckBox = new System.Windows.Forms.CheckBox();
            this.Options_TabControl = new System.Windows.Forms.TabControl();
            this.TabPage1 = new System.Windows.Forms.TabPage();
            this.TabPage2 = new System.Windows.Forms.TabPage();
            this.GroupBox4 = new System.Windows.Forms.GroupBox();
            this.AutoDLWind_CheckBox = new System.Windows.Forms.CheckBox();
            this.AutoDLNats_CheckBox = new System.Windows.Forms.CheckBox();
            this.TabPage3 = new System.Windows.Forms.TabPage();
            this.GroupBox1.SuspendLayout();
            this.GroupBox2.SuspendLayout();
            this.GroupBox3.SuspendLayout();
            this.Options_TabControl.SuspendLayout();
            this.TabPage1.SuspendLayout();
            this.TabPage2.SuspendLayout();
            this.GroupBox4.SuspendLayout();
            this.TabPage3.SuspendLayout();
            this.SuspendLayout();
            //
            //GroupBox1
            //
            this.GroupBox1.Controls.Add(this.browseFolderBtn);
            this.GroupBox1.Controls.Add(this.Label13);
            this.GroupBox1.Controls.Add(this.Expired_Lbl);
            this.GroupBox1.Controls.Add(this.DBPath_TxtBox);
            this.GroupBox1.Controls.Add(this.AiracPeriod_Lbl);
            this.GroupBox1.Controls.Add(this.Label1);
            this.GroupBox1.Controls.Add(this.Label5);
            this.GroupBox1.Controls.Add(this.Label14);
            this.GroupBox1.Controls.Add(this.Label6);
            this.GroupBox1.Controls.Add(this.DBFound_Lbl);
            this.GroupBox1.Controls.Add(this.Airac_Lbl);
            this.GroupBox1.Font = new System.Drawing.Font("Arial", 7.8f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
            this.GroupBox1.Location = new System.Drawing.Point(7, 7);
            this.GroupBox1.Margin = new System.Windows.Forms.Padding(4);
            this.GroupBox1.Name = "GroupBox1";
            this.GroupBox1.Padding = new System.Windows.Forms.Padding(4);
            this.GroupBox1.Size = new System.Drawing.Size(489, 272);
            this.GroupBox1.TabIndex = 0;
            this.GroupBox1.TabStop = false;
            this.GroupBox1.Text = "Database";
            //
            //browseFolderBtn
            //
            this.browseFolderBtn.BackColor = System.Drawing.SystemColors.Control;
            this.browseFolderBtn.Font = new System.Drawing.Font("Arial", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
            this.browseFolderBtn.ForeColor = System.Drawing.SystemColors.ControlText;
            this.browseFolderBtn.Location = new System.Drawing.Point(431, 156);
            this.browseFolderBtn.Margin = new System.Windows.Forms.Padding(4);
            this.browseFolderBtn.Name = "browseFolderBtn";
            this.browseFolderBtn.Size = new System.Drawing.Size(31, 28);
            this.browseFolderBtn.TabIndex = 4;
            this.browseFolderBtn.Text = "..";
            this.browseFolderBtn.UseVisualStyleBackColor = true;
            this.browseFolderBtn.Click += new System.EventHandler(this.browseFolderBtn_Click);
            //
            //Label13
            //
            this.Label13.AutoSize = true;
            this.Label13.Font = new System.Drawing.Font("Arial", 7.8f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
            this.Label13.Location = new System.Drawing.Point(8, 26);
            this.Label13.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.Label13.Name = "Label13";
            this.Label13.Size = new System.Drawing.Size(458, 80);
            this.Label13.TabIndex = 8;
            this.Label13.Text = resources.GetString("Label13.Text");
            //
            //Expired_Lbl
            //
            this.Expired_Lbl.AutoSize = true;
            this.Expired_Lbl.Font = new System.Drawing.Font("Arial", 7.8f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
            this.Expired_Lbl.Location = new System.Drawing.Point(277, 234);
            this.Expired_Lbl.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.Expired_Lbl.Name = "Expired_Lbl";
            this.Expired_Lbl.Size = new System.Drawing.Size(65, 16);
            this.Expired_Lbl.TabIndex = 21;
            this.Expired_Lbl.Text = "(Expired)";
            //
            //DBPath_TxtBox
            //
            this.DBPath_TxtBox.Font = new System.Drawing.Font("Arial", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
            this.DBPath_TxtBox.Location = new System.Drawing.Point(89, 158);
            this.DBPath_TxtBox.Margin = new System.Windows.Forms.Padding(4);
            this.DBPath_TxtBox.Name = "DBPath_TxtBox";
            this.DBPath_TxtBox.Size = new System.Drawing.Size(317, 27);
            this.DBPath_TxtBox.TabIndex = 3;
            this.DBPath_TxtBox.TextChanged += new System.EventHandler(this.DBPath_TxtBox_TextChanged);
            //
            //AiracPeriod_Lbl
            //
            this.AiracPeriod_Lbl.AutoSize = true;
            this.AiracPeriod_Lbl.Font = new System.Drawing.Font("Arial", 7.8f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
            this.AiracPeriod_Lbl.Location = new System.Drawing.Point(134, 234);
            this.AiracPeriod_Lbl.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.AiracPeriod_Lbl.Name = "AiracPeriod_Lbl";
            this.AiracPeriod_Lbl.Size = new System.Drawing.Size(119, 16);
            this.AiracPeriod_Lbl.TabIndex = 20;
            this.AiracPeriod_Lbl.Text = "OCT16NOV12/14";
            //
            //Label1
            //
            this.Label1.AutoSize = true;
            this.Label1.Font = new System.Drawing.Font("Arial", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
            this.Label1.Location = new System.Drawing.Point(16, 127);
            this.Label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.Label1.Name = "Label1";
            this.Label1.Size = new System.Drawing.Size(102, 19);
            this.Label1.TabIndex = 2;
            this.Label1.Text = "File Location";
            //
            //Label5
            //
            this.Label5.AutoSize = true;
            this.Label5.Font = new System.Drawing.Font("Arial", 7.8f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
            this.Label5.Location = new System.Drawing.Point(17, 202);
            this.Label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.Label5.Name = "Label5";
            this.Label5.Size = new System.Drawing.Size(117, 16);
            this.Label5.TabIndex = 9;
            this.Label5.Text = "Database Status:";
            //
            //Label14
            //
            this.Label14.AutoSize = true;
            this.Label14.Font = new System.Drawing.Font("Arial", 7.8f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
            this.Label14.Location = new System.Drawing.Point(17, 234);
            this.Label14.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.Label14.Name = "Label14";
            this.Label14.Size = new System.Drawing.Size(86, 16);
            this.Label14.TabIndex = 18;
            this.Label14.Text = "Valid period:";
            //
            //Label6
            //
            this.Label6.AutoSize = true;
            this.Label6.Font = new System.Drawing.Font("Arial", 7.8f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
            this.Label6.Location = new System.Drawing.Point(275, 202);
            this.Label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.Label6.Name = "Label6";
            this.Label6.Size = new System.Drawing.Size(53, 16);
            this.Label6.TabIndex = 10;
            this.Label6.Text = "AIRAC:";
            //
            //DBFound_Lbl
            //
            this.DBFound_Lbl.AutoSize = true;
            this.DBFound_Lbl.Font = new System.Drawing.Font("Arial", 7.8f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
            this.DBFound_Lbl.Location = new System.Drawing.Point(162, 202);
            this.DBFound_Lbl.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.DBFound_Lbl.Name = "DBFound_Lbl";
            this.DBFound_Lbl.Size = new System.Drawing.Size(49, 16);
            this.DBFound_Lbl.TabIndex = 17;
            this.DBFound_Lbl.Text = "Ready";
            //
            //Airac_Lbl
            //
            this.Airac_Lbl.AutoSize = true;
            this.Airac_Lbl.Font = new System.Drawing.Font("Arial", 7.8f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
            this.Airac_Lbl.Location = new System.Drawing.Point(371, 202);
            this.Airac_Lbl.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.Airac_Lbl.Name = "Airac_Lbl";
            this.Airac_Lbl.Size = new System.Drawing.Size(39, 16);
            this.Airac_Lbl.TabIndex = 15;
            this.Airac_Lbl.Text = "1411";
            //
            //ok_button
            //
            this.ok_button.BackColor = System.Drawing.SystemColors.ControlLight;
            this.ok_button.Font = new System.Drawing.Font("Arial", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
            this.ok_button.ForeColor = System.Drawing.SystemColors.ControlText;
            this.ok_button.Location = new System.Drawing.Point(119, 562);
            this.ok_button.Margin = new System.Windows.Forms.Padding(4);
            this.ok_button.Name = "ok_button";
            this.ok_button.Size = new System.Drawing.Size(100, 28);
            this.ok_button.TabIndex = 1;
            this.ok_button.Text = "OK";
            this.ok_button.UseVisualStyleBackColor = true;
            this.ok_button.Click += new System.EventHandler(this.ok_button_Click);
            //
            //cancel_button
            //
            this.cancel_button.BackColor = System.Drawing.SystemColors.Control;
            this.cancel_button.Font = new System.Drawing.Font("Arial", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
            this.cancel_button.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cancel_button.Location = new System.Drawing.Point(301, 562);
            this.cancel_button.Margin = new System.Windows.Forms.Padding(4);
            this.cancel_button.Name = "cancel_button";
            this.cancel_button.Size = new System.Drawing.Size(100, 28);
            this.cancel_button.TabIndex = 2;
            this.cancel_button.Text = "Cancel";
            this.cancel_button.UseVisualStyleBackColor = true;
            this.cancel_button.Click += new System.EventHandler(this.cancel_button_Click);
            //
            //GroupBox2
            //
            this.GroupBox2.Controls.Add(this.Button3);
            this.GroupBox2.Controls.Add(this.CheckBox3);
            this.GroupBox2.Controls.Add(this.TextBox3);
            this.GroupBox2.Controls.Add(this.Button2);
            this.GroupBox2.Controls.Add(this.CheckBox2);
            this.GroupBox2.Controls.Add(this.TextBox2);
            this.GroupBox2.Controls.Add(this.Button1);
            this.GroupBox2.Controls.Add(this.CheckBox1);
            this.GroupBox2.Controls.Add(this.TextBox1);
            this.GroupBox2.Font = new System.Drawing.Font("Arial", 7.8f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
            this.GroupBox2.Location = new System.Drawing.Point(13, 10);
            this.GroupBox2.Name = "GroupBox2";
            this.GroupBox2.Size = new System.Drawing.Size(487, 148);
            this.GroupBox2.TabIndex = 3;
            this.GroupBox2.TabStop = false;
            this.GroupBox2.Text = "Flight Plan Export";
            //
            //Button3
            //
            this.Button3.BackColor = System.Drawing.SystemColors.Control;
            this.Button3.Font = new System.Drawing.Font("Arial", 7.8f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
            this.Button3.ForeColor = System.Drawing.SystemColors.ControlText;
            this.Button3.Location = new System.Drawing.Point(443, 103);
            this.Button3.Margin = new System.Windows.Forms.Padding(4);
            this.Button3.Name = "Button3";
            this.Button3.Size = new System.Drawing.Size(29, 22);
            this.Button3.TabIndex = 29;
            this.Button3.Text = "..";
            this.Button3.UseVisualStyleBackColor = true;
            this.Button3.Click += new System.EventHandler(this.Button3_Click);
            //
            //CheckBox3
            //
            this.CheckBox3.AutoSize = true;
            this.CheckBox3.Font = new System.Drawing.Font("Arial", 7.8f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
            this.CheckBox3.Location = new System.Drawing.Point(20, 103);
            this.CheckBox3.Name = "CheckBox3";
            this.CheckBox3.Size = new System.Drawing.Size(99, 20);
            this.CheckBox3.TabIndex = 28;
            this.CheckBox3.Text = "PMDG 777";
            this.CheckBox3.UseVisualStyleBackColor = true;
            this.CheckBox3.CheckedChanged += new System.EventHandler(this.CheckBox3_CheckedChanged);
            //
            //TextBox3
            //
            this.TextBox3.Font = new System.Drawing.Font("Arial", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
            this.TextBox3.Location = new System.Drawing.Point(153, 99);
            this.TextBox3.Margin = new System.Windows.Forms.Padding(4);
            this.TextBox3.Name = "TextBox3";
            this.TextBox3.Size = new System.Drawing.Size(282, 27);
            this.TextBox3.TabIndex = 27;
            //
            //Button2
            //
            this.Button2.BackColor = System.Drawing.SystemColors.Control;
            this.Button2.Font = new System.Drawing.Font("Arial", 7.8f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
            this.Button2.ForeColor = System.Drawing.SystemColors.ControlText;
            this.Button2.Location = new System.Drawing.Point(443, 69);
            this.Button2.Margin = new System.Windows.Forms.Padding(4);
            this.Button2.Name = "Button2";
            this.Button2.Size = new System.Drawing.Size(29, 22);
            this.Button2.TabIndex = 26;
            this.Button2.Text = "..";
            this.Button2.UseVisualStyleBackColor = true;
            this.Button2.Click += new System.EventHandler(this.Button2_Click);
            //
            //CheckBox2
            //
            this.CheckBox2.AutoSize = true;
            this.CheckBox2.Font = new System.Drawing.Font("Arial", 7.8f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
            this.CheckBox2.Location = new System.Drawing.Point(20, 69);
            this.CheckBox2.Name = "CheckBox2";
            this.CheckBox2.Size = new System.Drawing.Size(103, 20);
            this.CheckBox2.TabIndex = 25;
            this.CheckBox2.Text = "PMDG NGX";
            this.CheckBox2.UseVisualStyleBackColor = true;
            this.CheckBox2.CheckedChanged += new System.EventHandler(this.CheckBox2_CheckedChanged);
            //
            //TextBox2
            //
            this.TextBox2.Font = new System.Drawing.Font("Arial", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
            this.TextBox2.Location = new System.Drawing.Point(153, 66);
            this.TextBox2.Margin = new System.Windows.Forms.Padding(4);
            this.TextBox2.Name = "TextBox2";
            this.TextBox2.Size = new System.Drawing.Size(282, 27);
            this.TextBox2.TabIndex = 24;
            //
            //Button1
            //
            this.Button1.BackColor = System.Drawing.SystemColors.Control;
            this.Button1.Font = new System.Drawing.Font("Arial", 7.8f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
            this.Button1.ForeColor = System.Drawing.SystemColors.ControlText;
            this.Button1.Location = new System.Drawing.Point(443, 34);
            this.Button1.Margin = new System.Windows.Forms.Padding(4);
            this.Button1.Name = "Button1";
            this.Button1.Size = new System.Drawing.Size(29, 22);
            this.Button1.TabIndex = 23;
            this.Button1.Text = "..";
            this.Button1.UseVisualStyleBackColor = true;
            this.Button1.Click += new System.EventHandler(this.Button1_Click);
            //
            //CheckBox1
            //
            this.CheckBox1.AutoSize = true;
            this.CheckBox1.Font = new System.Drawing.Font("Arial", 7.8f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
            this.CheckBox1.Location = new System.Drawing.Point(20, 38);
            this.CheckBox1.Name = "CheckBox1";
            this.CheckBox1.Size = new System.Drawing.Size(131, 20);
            this.CheckBox1.TabIndex = 23;
            this.CheckBox1.Text = "PMDG Common";
            this.CheckBox1.UseVisualStyleBackColor = true;
            this.CheckBox1.CheckedChanged += new System.EventHandler(this.CheckBox1_CheckedChanged);
            //
            //TextBox1
            //
            this.TextBox1.Font = new System.Drawing.Font("Arial", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
            this.TextBox1.Location = new System.Drawing.Point(153, 34);
            this.TextBox1.Margin = new System.Windows.Forms.Padding(4);
            this.TextBox1.Name = "TextBox1";
            this.TextBox1.Size = new System.Drawing.Size(282, 27);
            this.TextBox1.TabIndex = 22;
            //
            //GroupBox3
            //
            this.GroupBox3.Controls.Add(this.DoubleCheckWhenExit_CheckBox);
            this.GroupBox3.Font = new System.Drawing.Font("Arial", 7.8f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
            this.GroupBox3.Location = new System.Drawing.Point(7, 286);
            this.GroupBox3.Name = "GroupBox3";
            this.GroupBox3.Size = new System.Drawing.Size(487, 62);
            this.GroupBox3.TabIndex = 30;
            this.GroupBox3.TabStop = false;
            this.GroupBox3.Text = "Misc";
            //
            //DoubleCheckWhenExit_CheckBox
            //
            this.DoubleCheckWhenExit_CheckBox.AutoSize = true;
            this.DoubleCheckWhenExit_CheckBox.Font = new System.Drawing.Font("Arial", 7.8f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
            this.DoubleCheckWhenExit_CheckBox.Location = new System.Drawing.Point(37, 24);
            this.DoubleCheckWhenExit_CheckBox.Name = "DoubleCheckWhenExit_CheckBox";
            this.DoubleCheckWhenExit_CheckBox.Size = new System.Drawing.Size(218, 20);
            this.DoubleCheckWhenExit_CheckBox.TabIndex = 30;
            this.DoubleCheckWhenExit_CheckBox.Text = "Ask before closing application";
            this.DoubleCheckWhenExit_CheckBox.UseVisualStyleBackColor = true;
            //
            //Options_TabControl
            //
            this.Options_TabControl.Controls.Add(this.TabPage1);
            this.Options_TabControl.Controls.Add(this.TabPage2);
            this.Options_TabControl.Controls.Add(this.TabPage3);
            this.Options_TabControl.Location = new System.Drawing.Point(14, 12);
            this.Options_TabControl.Name = "Options_TabControl";
            this.Options_TabControl.SelectedIndex = 0;
            this.Options_TabControl.Size = new System.Drawing.Size(514, 536);
            this.Options_TabControl.TabIndex = 31;
            //
            //TabPage1
            //
            this.TabPage1.Controls.Add(this.GroupBox1);
            this.TabPage1.Controls.Add(this.GroupBox3);
            this.TabPage1.Location = new System.Drawing.Point(4, 25);
            this.TabPage1.Name = "TabPage1";
            this.TabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.TabPage1.Size = new System.Drawing.Size(506, 507);
            this.TabPage1.TabIndex = 0;
            this.TabPage1.Text = "General";
            this.TabPage1.UseVisualStyleBackColor = true;
            //
            //TabPage2
            //
            this.TabPage2.Controls.Add(this.GroupBox4);
            this.TabPage2.Location = new System.Drawing.Point(4, 25);
            this.TabPage2.Name = "TabPage2";
            this.TabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.TabPage2.Size = new System.Drawing.Size(506, 507);
            this.TabPage2.TabIndex = 1;
            this.TabPage2.Text = "Startup";
            this.TabPage2.UseVisualStyleBackColor = true;
            //
            //GroupBox4
            //
            this.GroupBox4.Controls.Add(this.AutoDLWind_CheckBox);
            this.GroupBox4.Controls.Add(this.AutoDLNats_CheckBox);
            this.GroupBox4.Font = new System.Drawing.Font("Arial", 7.8f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
            this.GroupBox4.Location = new System.Drawing.Point(6, 18);
            this.GroupBox4.Name = "GroupBox4";
            this.GroupBox4.Size = new System.Drawing.Size(487, 85);
            this.GroupBox4.TabIndex = 31;
            this.GroupBox4.TabStop = false;
            this.GroupBox4.Text = "Misc";
            //
            //AutoDLWind_CheckBox
            //
            this.AutoDLWind_CheckBox.AutoSize = true;
            this.AutoDLWind_CheckBox.Font = new System.Drawing.Font("Arial", 7.8f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
            this.AutoDLWind_CheckBox.Location = new System.Drawing.Point(44, 47);
            this.AutoDLWind_CheckBox.Name = "AutoDLWind_CheckBox";
            this.AutoDLWind_CheckBox.Size = new System.Drawing.Size(322, 20);
            this.AutoDLWind_CheckBox.TabIndex = 31;
            this.AutoDLWind_CheckBox.Text = "Download upper winds at startup automatically";
            this.AutoDLWind_CheckBox.UseVisualStyleBackColor = true;
            //
            //AutoDLNats_CheckBox
            //
            this.AutoDLNats_CheckBox.AutoSize = true;
            this.AutoDLNats_CheckBox.Font = new System.Drawing.Font("Arial", 7.8f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
            this.AutoDLNats_CheckBox.Location = new System.Drawing.Point(44, 21);
            this.AutoDLNats_CheckBox.Name = "AutoDLNats_CheckBox";
            this.AutoDLNats_CheckBox.Size = new System.Drawing.Size(277, 20);
            this.AutoDLNats_CheckBox.TabIndex = 30;
            this.AutoDLNats_CheckBox.Text = "Download NATs at startup automatically";
            this.AutoDLNats_CheckBox.UseVisualStyleBackColor = true;
            //
            //TabPage3
            //
            this.TabPage3.Controls.Add(this.GroupBox2);
            this.TabPage3.Location = new System.Drawing.Point(4, 25);
            this.TabPage3.Name = "TabPage3";
            this.TabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.TabPage3.Size = new System.Drawing.Size(506, 507);
            this.TabPage3.TabIndex = 2;
            this.TabPage3.Text = "Flight Plan Export";
            this.TabPage3.UseVisualStyleBackColor = true;
            //
            //OptionsForm
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(8f, 16f);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(545, 621);
            this.ControlBox = false;
            this.Controls.Add(this.Options_TabControl);
            this.Controls.Add(this.cancel_button);
            this.Controls.Add(this.ok_button);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.Name = "OptionsForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Options";
            this.GroupBox1.ResumeLayout(false);
            this.GroupBox1.PerformLayout();
            this.GroupBox2.ResumeLayout(false);
            this.GroupBox2.PerformLayout();
            this.GroupBox3.ResumeLayout(false);
            this.GroupBox3.PerformLayout();
            this.Options_TabControl.ResumeLayout(false);
            this.TabPage1.ResumeLayout(false);
            this.TabPage2.ResumeLayout(false);
            this.GroupBox4.ResumeLayout(false);
            this.GroupBox4.PerformLayout();
            this.TabPage3.ResumeLayout(false);
            this.ResumeLayout(false);

        }
        internal System.Windows.Forms.GroupBox GroupBox1;
        internal System.Windows.Forms.Label Label1;
        internal System.Windows.Forms.Button browseFolderBtn;
        internal System.Windows.Forms.TextBox DBPath_TxtBox;
        internal System.Windows.Forms.FolderBrowserDialog FolderBrowserDialog1;
        internal System.Windows.Forms.Button ok_button;
        internal System.Windows.Forms.Button cancel_button;
        internal System.Windows.Forms.Label Label6;
        internal System.Windows.Forms.Label Label5;
        internal System.Windows.Forms.Label Label14;
        internal System.Windows.Forms.Label DBFound_Lbl;
        internal System.Windows.Forms.Label Airac_Lbl;
        internal System.Windows.Forms.Label Expired_Lbl;
        internal System.Windows.Forms.Label AiracPeriod_Lbl;
        internal System.Windows.Forms.Label Label13;
        internal System.Windows.Forms.GroupBox GroupBox2;
        internal System.Windows.Forms.CheckBox CheckBox1;
        internal System.Windows.Forms.Button Button3;
        internal System.Windows.Forms.CheckBox CheckBox3;
        internal System.Windows.Forms.TextBox TextBox3;
        internal System.Windows.Forms.Button Button2;
        internal System.Windows.Forms.CheckBox CheckBox2;
        internal System.Windows.Forms.TextBox TextBox2;
        internal System.Windows.Forms.Button Button1;
        internal System.Windows.Forms.TextBox TextBox1;
        internal System.Windows.Forms.GroupBox GroupBox3;
        internal System.Windows.Forms.CheckBox DoubleCheckWhenExit_CheckBox;
        internal System.Windows.Forms.TabControl Options_TabControl;
        internal System.Windows.Forms.TabPage TabPage1;
        internal System.Windows.Forms.TabPage TabPage2;
        internal System.Windows.Forms.GroupBox GroupBox4;
        internal System.Windows.Forms.CheckBox AutoDLWind_CheckBox;
        internal System.Windows.Forms.CheckBox AutoDLNats_CheckBox;
        internal System.Windows.Forms.TabPage TabPage3;
    }
}
