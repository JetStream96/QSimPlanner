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
            this.expiredLbl = new System.Windows.Forms.Label();
            this.navDataPathTxtBox = new System.Windows.Forms.TextBox();
            this.airacPeriodLbl = new System.Windows.Forms.Label();
            this.Label1 = new System.Windows.Forms.Label();
            this.Label5 = new System.Windows.Forms.Label();
            this.Label14 = new System.Windows.Forms.Label();
            this.Label6 = new System.Windows.Forms.Label();
            this.navDataFoundLbl = new System.Windows.Forms.Label();
            this.airacLbl = new System.Windows.Forms.Label();
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
            this.PromptBeforeExit = new System.Windows.Forms.CheckBox();
            this.Options_TabControl = new System.Windows.Forms.TabControl();
            this.TabPage1 = new System.Windows.Forms.TabPage();
            this.TabPage2 = new System.Windows.Forms.TabPage();
            this.GroupBox4 = new System.Windows.Forms.GroupBox();
            this.AutoDLWindCheckBox = new System.Windows.Forms.CheckBox();
            this.AutoDLTracksCheckBox = new System.Windows.Forms.CheckBox();
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
            // GroupBox1
            // 
            this.GroupBox1.Controls.Add(this.browseFolderBtn);
            this.GroupBox1.Controls.Add(this.Label13);
            this.GroupBox1.Controls.Add(this.expiredLbl);
            this.GroupBox1.Controls.Add(this.navDataPathTxtBox);
            this.GroupBox1.Controls.Add(this.airacPeriodLbl);
            this.GroupBox1.Controls.Add(this.Label1);
            this.GroupBox1.Controls.Add(this.Label5);
            this.GroupBox1.Controls.Add(this.Label14);
            this.GroupBox1.Controls.Add(this.Label6);
            this.GroupBox1.Controls.Add(this.navDataFoundLbl);
            this.GroupBox1.Controls.Add(this.airacLbl);
            this.GroupBox1.Font = new System.Drawing.Font("Arial", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.GroupBox1.Location = new System.Drawing.Point(7, 7);
            this.GroupBox1.Margin = new System.Windows.Forms.Padding(4);
            this.GroupBox1.Name = "GroupBox1";
            this.GroupBox1.Padding = new System.Windows.Forms.Padding(4);
            this.GroupBox1.Size = new System.Drawing.Size(489, 272);
            this.GroupBox1.TabIndex = 0;
            this.GroupBox1.TabStop = false;
            this.GroupBox1.Text = "Database";
            // 
            // browseFolderBtn
            // 
            this.browseFolderBtn.BackColor = System.Drawing.SystemColors.Control;
            this.browseFolderBtn.Font = new System.Drawing.Font("Arial", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
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
            // Label13
            // 
            this.Label13.AutoSize = true;
            this.Label13.Font = new System.Drawing.Font("Arial", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label13.Location = new System.Drawing.Point(8, 26);
            this.Label13.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.Label13.Name = "Label13";
            this.Label13.Size = new System.Drawing.Size(458, 80);
            this.Label13.TabIndex = 8;
            this.Label13.Text = resources.GetString("Label13.Text");
            // 
            // expiredLbl
            // 
            this.expiredLbl.AutoSize = true;
            this.expiredLbl.Font = new System.Drawing.Font("Arial", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.expiredLbl.Location = new System.Drawing.Point(277, 234);
            this.expiredLbl.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.expiredLbl.Name = "expiredLbl";
            this.expiredLbl.Size = new System.Drawing.Size(65, 16);
            this.expiredLbl.TabIndex = 21;
            this.expiredLbl.Text = "(Expired)";
            // 
            // navDataPathTxtBox
            // 
            this.navDataPathTxtBox.Font = new System.Drawing.Font("Arial", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.navDataPathTxtBox.Location = new System.Drawing.Point(89, 158);
            this.navDataPathTxtBox.Margin = new System.Windows.Forms.Padding(4);
            this.navDataPathTxtBox.Name = "navDataPathTxtBox";
            this.navDataPathTxtBox.Size = new System.Drawing.Size(317, 27);
            this.navDataPathTxtBox.TabIndex = 3;
            this.navDataPathTxtBox.TextChanged += new System.EventHandler(this.navDataPathTxtBox_TextChanged);
            // 
            // airacPeriodLbl
            // 
            this.airacPeriodLbl.AutoSize = true;
            this.airacPeriodLbl.Font = new System.Drawing.Font("Arial", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.airacPeriodLbl.Location = new System.Drawing.Point(134, 234);
            this.airacPeriodLbl.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.airacPeriodLbl.Name = "airacPeriodLbl";
            this.airacPeriodLbl.Size = new System.Drawing.Size(119, 16);
            this.airacPeriodLbl.TabIndex = 20;
            this.airacPeriodLbl.Text = "OCT16NOV12/14";
            // 
            // Label1
            // 
            this.Label1.AutoSize = true;
            this.Label1.Font = new System.Drawing.Font("Arial", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label1.Location = new System.Drawing.Point(16, 127);
            this.Label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.Label1.Name = "Label1";
            this.Label1.Size = new System.Drawing.Size(102, 19);
            this.Label1.TabIndex = 2;
            this.Label1.Text = "File Location";
            // 
            // Label5
            // 
            this.Label5.AutoSize = true;
            this.Label5.Font = new System.Drawing.Font("Arial", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label5.Location = new System.Drawing.Point(17, 202);
            this.Label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.Label5.Name = "Label5";
            this.Label5.Size = new System.Drawing.Size(117, 16);
            this.Label5.TabIndex = 9;
            this.Label5.Text = "Database Status:";
            // 
            // Label14
            // 
            this.Label14.AutoSize = true;
            this.Label14.Font = new System.Drawing.Font("Arial", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label14.Location = new System.Drawing.Point(17, 234);
            this.Label14.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.Label14.Name = "Label14";
            this.Label14.Size = new System.Drawing.Size(86, 16);
            this.Label14.TabIndex = 18;
            this.Label14.Text = "Valid period:";
            // 
            // Label6
            // 
            this.Label6.AutoSize = true;
            this.Label6.Font = new System.Drawing.Font("Arial", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label6.Location = new System.Drawing.Point(275, 202);
            this.Label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.Label6.Name = "Label6";
            this.Label6.Size = new System.Drawing.Size(53, 16);
            this.Label6.TabIndex = 10;
            this.Label6.Text = "AIRAC:";
            // 
            // navDataFoundLbl
            // 
            this.navDataFoundLbl.AutoSize = true;
            this.navDataFoundLbl.Font = new System.Drawing.Font("Arial", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.navDataFoundLbl.Location = new System.Drawing.Point(162, 202);
            this.navDataFoundLbl.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.navDataFoundLbl.Name = "navDataFoundLbl";
            this.navDataFoundLbl.Size = new System.Drawing.Size(49, 16);
            this.navDataFoundLbl.TabIndex = 17;
            this.navDataFoundLbl.Text = "Ready";
            // 
            // airacLbl
            // 
            this.airacLbl.AutoSize = true;
            this.airacLbl.Font = new System.Drawing.Font("Arial", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.airacLbl.Location = new System.Drawing.Point(371, 202);
            this.airacLbl.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.airacLbl.Name = "airacLbl";
            this.airacLbl.Size = new System.Drawing.Size(39, 16);
            this.airacLbl.TabIndex = 15;
            this.airacLbl.Text = "1411";
            // 
            // ok_button
            // 
            this.ok_button.BackColor = System.Drawing.SystemColors.ControlLight;
            this.ok_button.Font = new System.Drawing.Font("Arial", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ok_button.ForeColor = System.Drawing.SystemColors.ControlText;
            this.ok_button.Location = new System.Drawing.Point(119, 562);
            this.ok_button.Margin = new System.Windows.Forms.Padding(4);
            this.ok_button.Name = "ok_button";
            this.ok_button.Size = new System.Drawing.Size(100, 28);
            this.ok_button.TabIndex = 1;
            this.ok_button.Text = "OK";
            this.ok_button.UseVisualStyleBackColor = true;
            this.ok_button.Click += new System.EventHandler(this.OkBtnClick);
            // 
            // cancel_button
            // 
            this.cancel_button.BackColor = System.Drawing.SystemColors.Control;
            this.cancel_button.Font = new System.Drawing.Font("Arial", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cancel_button.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cancel_button.Location = new System.Drawing.Point(301, 562);
            this.cancel_button.Margin = new System.Windows.Forms.Padding(4);
            this.cancel_button.Name = "cancel_button";
            this.cancel_button.Size = new System.Drawing.Size(100, 28);
            this.cancel_button.TabIndex = 2;
            this.cancel_button.Text = "Cancel";
            this.cancel_button.UseVisualStyleBackColor = true;
            this.cancel_button.Click += new System.EventHandler(this.CancelBtnClick);
            // 
            // GroupBox2
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
            this.GroupBox2.Font = new System.Drawing.Font("Arial", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.GroupBox2.Location = new System.Drawing.Point(13, 10);
            this.GroupBox2.Name = "GroupBox2";
            this.GroupBox2.Size = new System.Drawing.Size(487, 148);
            this.GroupBox2.TabIndex = 3;
            this.GroupBox2.TabStop = false;
            this.GroupBox2.Text = "Flight Plan Export";
            // 
            // Button3
            // 
            this.Button3.BackColor = System.Drawing.SystemColors.Control;
            this.Button3.Font = new System.Drawing.Font("Arial", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Button3.ForeColor = System.Drawing.SystemColors.ControlText;
            this.Button3.Location = new System.Drawing.Point(443, 103);
            this.Button3.Margin = new System.Windows.Forms.Padding(4);
            this.Button3.Name = "Button3";
            this.Button3.Size = new System.Drawing.Size(29, 22);
            this.Button3.TabIndex = 29;
            this.Button3.Text = "..";
            this.Button3.UseVisualStyleBackColor = true;
            // 
            // CheckBox3
            // 
            this.CheckBox3.AutoSize = true;
            this.CheckBox3.Font = new System.Drawing.Font("Arial", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CheckBox3.Location = new System.Drawing.Point(20, 103);
            this.CheckBox3.Name = "CheckBox3";
            this.CheckBox3.Size = new System.Drawing.Size(99, 20);
            this.CheckBox3.TabIndex = 28;
            this.CheckBox3.Text = "PMDG 777";
            this.CheckBox3.UseVisualStyleBackColor = true;
            // 
            // TextBox3
            // 
            this.TextBox3.Font = new System.Drawing.Font("Arial", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TextBox3.Location = new System.Drawing.Point(153, 99);
            this.TextBox3.Margin = new System.Windows.Forms.Padding(4);
            this.TextBox3.Name = "TextBox3";
            this.TextBox3.Size = new System.Drawing.Size(282, 27);
            this.TextBox3.TabIndex = 27;
            // 
            // Button2
            // 
            this.Button2.BackColor = System.Drawing.SystemColors.Control;
            this.Button2.Font = new System.Drawing.Font("Arial", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Button2.ForeColor = System.Drawing.SystemColors.ControlText;
            this.Button2.Location = new System.Drawing.Point(443, 69);
            this.Button2.Margin = new System.Windows.Forms.Padding(4);
            this.Button2.Name = "Button2";
            this.Button2.Size = new System.Drawing.Size(29, 22);
            this.Button2.TabIndex = 26;
            this.Button2.Text = "..";
            this.Button2.UseVisualStyleBackColor = true;
            // 
            // CheckBox2
            // 
            this.CheckBox2.AutoSize = true;
            this.CheckBox2.Font = new System.Drawing.Font("Arial", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CheckBox2.Location = new System.Drawing.Point(20, 69);
            this.CheckBox2.Name = "CheckBox2";
            this.CheckBox2.Size = new System.Drawing.Size(103, 20);
            this.CheckBox2.TabIndex = 25;
            this.CheckBox2.Text = "PMDG NGX";
            this.CheckBox2.UseVisualStyleBackColor = true;
            // 
            // TextBox2
            // 
            this.TextBox2.Font = new System.Drawing.Font("Arial", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TextBox2.Location = new System.Drawing.Point(153, 66);
            this.TextBox2.Margin = new System.Windows.Forms.Padding(4);
            this.TextBox2.Name = "TextBox2";
            this.TextBox2.Size = new System.Drawing.Size(282, 27);
            this.TextBox2.TabIndex = 24;
            // 
            // Button1
            // 
            this.Button1.BackColor = System.Drawing.SystemColors.Control;
            this.Button1.Font = new System.Drawing.Font("Arial", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Button1.ForeColor = System.Drawing.SystemColors.ControlText;
            this.Button1.Location = new System.Drawing.Point(443, 34);
            this.Button1.Margin = new System.Windows.Forms.Padding(4);
            this.Button1.Name = "Button1";
            this.Button1.Size = new System.Drawing.Size(29, 22);
            this.Button1.TabIndex = 23;
            this.Button1.Text = "..";
            this.Button1.UseVisualStyleBackColor = true;
            // 
            // CheckBox1
            // 
            this.CheckBox1.AutoSize = true;
            this.CheckBox1.Font = new System.Drawing.Font("Arial", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CheckBox1.Location = new System.Drawing.Point(20, 38);
            this.CheckBox1.Name = "CheckBox1";
            this.CheckBox1.Size = new System.Drawing.Size(131, 20);
            this.CheckBox1.TabIndex = 23;
            this.CheckBox1.Text = "PMDG Common";
            this.CheckBox1.UseVisualStyleBackColor = true;
            // 
            // TextBox1
            // 
            this.TextBox1.Font = new System.Drawing.Font("Arial", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TextBox1.Location = new System.Drawing.Point(153, 34);
            this.TextBox1.Margin = new System.Windows.Forms.Padding(4);
            this.TextBox1.Name = "TextBox1";
            this.TextBox1.Size = new System.Drawing.Size(282, 27);
            this.TextBox1.TabIndex = 22;
            // 
            // GroupBox3
            // 
            this.GroupBox3.Controls.Add(this.PromptBeforeExit);
            this.GroupBox3.Font = new System.Drawing.Font("Arial", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.GroupBox3.Location = new System.Drawing.Point(7, 286);
            this.GroupBox3.Name = "GroupBox3";
            this.GroupBox3.Size = new System.Drawing.Size(487, 62);
            this.GroupBox3.TabIndex = 30;
            this.GroupBox3.TabStop = false;
            this.GroupBox3.Text = "Misc";
            // 
            // PromptBeforeExit
            // 
            this.PromptBeforeExit.AutoSize = true;
            this.PromptBeforeExit.Font = new System.Drawing.Font("Arial", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.PromptBeforeExit.Location = new System.Drawing.Point(37, 24);
            this.PromptBeforeExit.Name = "PromptBeforeExit";
            this.PromptBeforeExit.Size = new System.Drawing.Size(218, 20);
            this.PromptBeforeExit.TabIndex = 30;
            this.PromptBeforeExit.Text = "Ask before closing application";
            this.PromptBeforeExit.UseVisualStyleBackColor = true;
            // 
            // Options_TabControl
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
            // TabPage1
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
            // TabPage2
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
            // GroupBox4
            // 
            this.GroupBox4.Controls.Add(this.AutoDLWindCheckBox);
            this.GroupBox4.Controls.Add(this.AutoDLTracksCheckBox);
            this.GroupBox4.Font = new System.Drawing.Font("Arial", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.GroupBox4.Location = new System.Drawing.Point(6, 18);
            this.GroupBox4.Name = "GroupBox4";
            this.GroupBox4.Size = new System.Drawing.Size(487, 85);
            this.GroupBox4.TabIndex = 31;
            this.GroupBox4.TabStop = false;
            this.GroupBox4.Text = "Misc";
            // 
            // AutoDLWindCheckBox
            // 
            this.AutoDLWindCheckBox.AutoSize = true;
            this.AutoDLWindCheckBox.Font = new System.Drawing.Font("Arial", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.AutoDLWindCheckBox.Location = new System.Drawing.Point(44, 47);
            this.AutoDLWindCheckBox.Name = "AutoDLWindCheckBox";
            this.AutoDLWindCheckBox.Size = new System.Drawing.Size(322, 20);
            this.AutoDLWindCheckBox.TabIndex = 31;
            this.AutoDLWindCheckBox.Text = "Download upper winds at startup automatically";
            this.AutoDLWindCheckBox.UseVisualStyleBackColor = true;
            // 
            // AutoDLTracksCheckBox
            // 
            this.AutoDLTracksCheckBox.AutoSize = true;
            this.AutoDLTracksCheckBox.Font = new System.Drawing.Font("Arial", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.AutoDLTracksCheckBox.Location = new System.Drawing.Point(44, 21);
            this.AutoDLTracksCheckBox.Name = "AutoDLTracksCheckBox";
            this.AutoDLTracksCheckBox.Size = new System.Drawing.Size(284, 20);
            this.AutoDLTracksCheckBox.TabIndex = 30;
            this.AutoDLTracksCheckBox.Text = "Download tracks at startup automatically";
            this.AutoDLTracksCheckBox.UseVisualStyleBackColor = true;
            // 
            // TabPage3
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
            // OptionsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(525, 601);
            this.ControlBox = false;
            this.Controls.Add(this.Options_TabControl);
            this.Controls.Add(this.cancel_button);
            this.Controls.Add(this.ok_button);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
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
        internal System.Windows.Forms.TextBox navDataPathTxtBox;
        internal System.Windows.Forms.Button ok_button;
        internal System.Windows.Forms.Button cancel_button;
        internal System.Windows.Forms.Label Label6;
        internal System.Windows.Forms.Label Label5;
        internal System.Windows.Forms.Label Label14;
        internal System.Windows.Forms.Label navDataFoundLbl;
        internal System.Windows.Forms.Label airacLbl;
        internal System.Windows.Forms.Label expiredLbl;
        internal System.Windows.Forms.Label airacPeriodLbl;
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
        internal System.Windows.Forms.CheckBox PromptBeforeExit;
        internal System.Windows.Forms.TabControl Options_TabControl;
        internal System.Windows.Forms.TabPage TabPage1;
        internal System.Windows.Forms.TabPage TabPage2;
        internal System.Windows.Forms.GroupBox GroupBox4;
        internal System.Windows.Forms.CheckBox AutoDLWindCheckBox;
        internal System.Windows.Forms.CheckBox AutoDLTracksCheckBox;
        internal System.Windows.Forms.TabPage TabPage3;
    }
}
