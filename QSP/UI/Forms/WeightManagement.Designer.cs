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
    partial class WtManagement : System.Windows.Forms.Form
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
            this.TableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.MaxZFW_Label = new System.Windows.Forms.Label();
            this.OEW_Label = new System.Windows.Forms.Label();
            this.MaxPayload_Label = new System.Windows.Forms.Label();
            this.Label5 = new System.Windows.Forms.Label();
            this.Label6 = new System.Windows.Forms.Label();
            this.Label4 = new System.Windows.Forms.Label();
            this.Label3 = new System.Windows.Forms.Label();
            this.Label2 = new System.Windows.Forms.Label();
            this.Label1 = new System.Windows.Forms.Label();
            this.GroupBox1 = new System.Windows.Forms.GroupBox();
            this.PayloadTrackBar = new System.Windows.Forms.TrackBar();
            this.WtUnit_ComboBox = new System.Windows.Forms.ComboBox();
            this.Label13 = new System.Windows.Forms.Label();
            this.TableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.Label10 = new System.Windows.Forms.Label();
            this.Label11 = new System.Windows.Forms.Label();
            this.Label12 = new System.Windows.Forms.Label();
            this.TableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.OEW_TextBox = new System.Windows.Forms.TextBox();
            this.Payload_TextBox = new System.Windows.Forms.TextBox();
            this.ZFW_TextBox = new System.Windows.Forms.TextBox();
            this.TableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.Label16 = new System.Windows.Forms.Label();
            this.Label17 = new System.Windows.Forms.Label();
            this.Label18 = new System.Windows.Forms.Label();
            this.OK_Button = new System.Windows.Forms.Button();
            this.Cancel_Button = new System.Windows.Forms.Button();
            this.TableLayoutPanel1.SuspendLayout();
            this.GroupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)this.PayloadTrackBar).BeginInit();
            this.TableLayoutPanel4.SuspendLayout();
            this.TableLayoutPanel3.SuspendLayout();
            this.TableLayoutPanel2.SuspendLayout();
            this.SuspendLayout();
            //
            //TableLayoutPanel1
            //
            this.TableLayoutPanel1.ColumnCount = 3;
            this.TableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 300f));
            this.TableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 80f));
            this.TableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 45f));
            this.TableLayoutPanel1.Controls.Add(this.MaxZFW_Label, 1, 2);
            this.TableLayoutPanel1.Controls.Add(this.OEW_Label, 1, 0);
            this.TableLayoutPanel1.Controls.Add(this.MaxPayload_Label, 1, 1);
            this.TableLayoutPanel1.Controls.Add(this.Label5, 2, 2);
            this.TableLayoutPanel1.Controls.Add(this.Label6, 2, 1);
            this.TableLayoutPanel1.Controls.Add(this.Label4, 2, 0);
            this.TableLayoutPanel1.Controls.Add(this.Label3, 0, 2);
            this.TableLayoutPanel1.Controls.Add(this.Label2, 0, 1);
            this.TableLayoutPanel1.Controls.Add(this.Label1, 0, 0);
            this.TableLayoutPanel1.Location = new System.Drawing.Point(19, 23);
            this.TableLayoutPanel1.Name = "TableLayoutPanel1";
            this.TableLayoutPanel1.RowCount = 3;
            this.TableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35f));
            this.TableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35f));
            this.TableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35f));
            this.TableLayoutPanel1.Size = new System.Drawing.Size(425, 105);
            this.TableLayoutPanel1.TabIndex = 0;
            //
            //MaxZFW_Label
            //
            this.MaxZFW_Label.AutoSize = true;
            this.MaxZFW_Label.Font = new System.Drawing.Font("Arial", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
            this.MaxZFW_Label.Location = new System.Drawing.Point(303, 70);
            this.MaxZFW_Label.Name = "MaxZFW_Label";
            this.MaxZFW_Label.Size = new System.Drawing.Size(54, 19);
            this.MaxZFW_Label.TabIndex = 4;
            this.MaxZFW_Label.Text = "12345";
            //
            //OEW_Label
            //
            this.OEW_Label.AutoSize = true;
            this.OEW_Label.Font = new System.Drawing.Font("Arial", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
            this.OEW_Label.Location = new System.Drawing.Point(303, 0);
            this.OEW_Label.Name = "OEW_Label";
            this.OEW_Label.Size = new System.Drawing.Size(54, 19);
            this.OEW_Label.TabIndex = 5;
            this.OEW_Label.Text = "12345";
            //
            //MaxPayload_Label
            //
            this.MaxPayload_Label.AutoSize = true;
            this.MaxPayload_Label.Font = new System.Drawing.Font("Arial", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
            this.MaxPayload_Label.Location = new System.Drawing.Point(303, 35);
            this.MaxPayload_Label.Name = "MaxPayload_Label";
            this.MaxPayload_Label.Size = new System.Drawing.Size(54, 19);
            this.MaxPayload_Label.TabIndex = 3;
            this.MaxPayload_Label.Text = "12345";
            //
            //Label5
            //
            this.Label5.AutoSize = true;
            this.Label5.Font = new System.Drawing.Font("Arial", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
            this.Label5.Location = new System.Drawing.Point(383, 70);
            this.Label5.Name = "Label5";
            this.Label5.Size = new System.Drawing.Size(32, 19);
            this.Label5.TabIndex = 3;
            this.Label5.Text = "KG";
            //
            //Label6
            //
            this.Label6.AutoSize = true;
            this.Label6.Font = new System.Drawing.Font("Arial", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
            this.Label6.Location = new System.Drawing.Point(383, 35);
            this.Label6.Name = "Label6";
            this.Label6.Size = new System.Drawing.Size(32, 19);
            this.Label6.TabIndex = 4;
            this.Label6.Text = "KG";
            //
            //Label4
            //
            this.Label4.AutoSize = true;
            this.Label4.Font = new System.Drawing.Font("Arial", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
            this.Label4.Location = new System.Drawing.Point(383, 0);
            this.Label4.Name = "Label4";
            this.Label4.Size = new System.Drawing.Size(32, 19);
            this.Label4.TabIndex = 2;
            this.Label4.Text = "KG";
            //
            //Label3
            //
            this.Label3.AutoSize = true;
            this.Label3.Font = new System.Drawing.Font("Arial", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
            this.Label3.Location = new System.Drawing.Point(3, 70);
            this.Label3.Name = "Label3";
            this.Label3.Size = new System.Drawing.Size(118, 19);
            this.Label3.TabIndex = 3;
            this.Label3.Text = "Maximum ZFW";
            //
            //Label2
            //
            this.Label2.AutoSize = true;
            this.Label2.Font = new System.Drawing.Font("Arial", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
            this.Label2.Location = new System.Drawing.Point(3, 35);
            this.Label2.Name = "Label2";
            this.Label2.Size = new System.Drawing.Size(141, 19);
            this.Label2.TabIndex = 2;
            this.Label2.Text = "Maximum Payload";
            //
            //Label1
            //
            this.Label1.AutoSize = true;
            this.Label1.Font = new System.Drawing.Font("Arial", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
            this.Label1.Location = new System.Drawing.Point(3, 0);
            this.Label1.Name = "Label1";
            this.Label1.Size = new System.Drawing.Size(245, 19);
            this.Label1.TabIndex = 1;
            this.Label1.Text = "Operating Empty Weight (OEW)";
            //
            //GroupBox1
            //
            this.GroupBox1.Controls.Add(this.PayloadTrackBar);
            this.GroupBox1.Controls.Add(this.WtUnit_ComboBox);
            this.GroupBox1.Controls.Add(this.Label13);
            this.GroupBox1.Controls.Add(this.TableLayoutPanel4);
            this.GroupBox1.Controls.Add(this.TableLayoutPanel3);
            this.GroupBox1.Controls.Add(this.TableLayoutPanel2);
            this.GroupBox1.Controls.Add(this.TableLayoutPanel1);
            this.GroupBox1.Location = new System.Drawing.Point(12, 12);
            this.GroupBox1.Name = "GroupBox1";
            this.GroupBox1.Size = new System.Drawing.Size(666, 296);
            this.GroupBox1.TabIndex = 1;
            this.GroupBox1.TabStop = false;
            this.GroupBox1.Text = "Weight Management";
            //
            //PayloadTrackBar
            //
            this.PayloadTrackBar.Location = new System.Drawing.Point(386, 192);
            this.PayloadTrackBar.Maximum = 100;
            this.PayloadTrackBar.Name = "PayloadTrackBar";
            this.PayloadTrackBar.Size = new System.Drawing.Size(239, 56);
            this.PayloadTrackBar.TabIndex = 4;
            this.PayloadTrackBar.TickStyle = System.Windows.Forms.TickStyle.None;
            this.PayloadTrackBar.Scroll += new System.EventHandler(this.Payload_TrackBar_Scroll);
            //
            //WtUnit_ComboBox
            //
            this.WtUnit_ComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.WtUnit_ComboBox.Font = new System.Drawing.Font("Arial", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
            this.WtUnit_ComboBox.FormattingEnabled = true;
            this.WtUnit_ComboBox.Items.AddRange(new object[] {
                "KG",
                "LB"
            });
            this.WtUnit_ComboBox.Location = new System.Drawing.Point(525, 58);
            this.WtUnit_ComboBox.Name = "WtUnit_ComboBox";
            this.WtUnit_ComboBox.Size = new System.Drawing.Size(90, 27);
            this.WtUnit_ComboBox.TabIndex = 8;
            this.WtUnit_ComboBox.SelectedIndexChanged += new System.EventHandler(this.WtUnit_ComboBox_SelectedIndexChanged);
            //
            //Label13
            //
            this.Label13.AutoSize = true;
            this.Label13.Font = new System.Drawing.Font("Arial", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
            this.Label13.Location = new System.Drawing.Point(521, 34);
            this.Label13.Name = "Label13";
            this.Label13.Size = new System.Drawing.Size(94, 19);
            this.Label13.TabIndex = 7;
            this.Label13.Text = "Weight Unit";
            //
            //TableLayoutPanel4
            //
            this.TableLayoutPanel4.ColumnCount = 1;
            this.TableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100f));
            this.TableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20f));
            this.TableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20f));
            this.TableLayoutPanel4.Controls.Add(this.Label10, 0, 2);
            this.TableLayoutPanel4.Controls.Add(this.Label11, 0, 1);
            this.TableLayoutPanel4.Controls.Add(this.Label12, 0, 0);
            this.TableLayoutPanel4.Location = new System.Drawing.Point(276, 154);
            this.TableLayoutPanel4.Name = "TableLayoutPanel4";
            this.TableLayoutPanel4.RowCount = 3;
            this.TableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35f));
            this.TableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35f));
            this.TableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35f));
            this.TableLayoutPanel4.Size = new System.Drawing.Size(50, 105);
            this.TableLayoutPanel4.TabIndex = 3;
            //
            //Label10
            //
            this.Label10.AutoSize = true;
            this.Label10.Font = new System.Drawing.Font("Arial", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
            this.Label10.Location = new System.Drawing.Point(3, 70);
            this.Label10.Name = "Label10";
            this.Label10.Size = new System.Drawing.Size(32, 19);
            this.Label10.TabIndex = 3;
            this.Label10.Text = "KG";
            //
            //Label11
            //
            this.Label11.AutoSize = true;
            this.Label11.Font = new System.Drawing.Font("Arial", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
            this.Label11.Location = new System.Drawing.Point(3, 35);
            this.Label11.Name = "Label11";
            this.Label11.Size = new System.Drawing.Size(32, 19);
            this.Label11.TabIndex = 2;
            this.Label11.Text = "KG";
            //
            //Label12
            //
            this.Label12.AutoSize = true;
            this.Label12.Font = new System.Drawing.Font("Arial", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
            this.Label12.Location = new System.Drawing.Point(3, 0);
            this.Label12.Name = "Label12";
            this.Label12.Size = new System.Drawing.Size(32, 19);
            this.Label12.TabIndex = 1;
            this.Label12.Text = "KG";
            //
            //TableLayoutPanel3
            //
            this.TableLayoutPanel3.ColumnCount = 1;
            this.TableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100f));
            this.TableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20f));
            this.TableLayoutPanel3.Controls.Add(this.OEW_TextBox, 0, 0);
            this.TableLayoutPanel3.Controls.Add(this.Payload_TextBox, 0, 1);
            this.TableLayoutPanel3.Controls.Add(this.ZFW_TextBox, 0, 2);
            this.TableLayoutPanel3.Location = new System.Drawing.Point(139, 154);
            this.TableLayoutPanel3.Name = "TableLayoutPanel3";
            this.TableLayoutPanel3.RowCount = 3;
            this.TableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35f));
            this.TableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35f));
            this.TableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35f));
            this.TableLayoutPanel3.Size = new System.Drawing.Size(128, 105);
            this.TableLayoutPanel3.TabIndex = 2;
            //
            //OEW_TextBox
            //
            this.OEW_TextBox.Enabled = false;
            this.OEW_TextBox.Font = new System.Drawing.Font("Arial", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
            this.OEW_TextBox.Location = new System.Drawing.Point(3, 3);
            this.OEW_TextBox.Name = "OEW_TextBox";
            this.OEW_TextBox.Size = new System.Drawing.Size(100, 27);
            this.OEW_TextBox.TabIndex = 3;
            //
            //Payload_TextBox
            //
            this.Payload_TextBox.Font = new System.Drawing.Font("Arial", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
            this.Payload_TextBox.Location = new System.Drawing.Point(3, 38);
            this.Payload_TextBox.Name = "Payload_TextBox";
            this.Payload_TextBox.Size = new System.Drawing.Size(100, 27);
            this.Payload_TextBox.TabIndex = 4;
            this.Payload_TextBox.TextChanged += new System.EventHandler(this.Payload_TextBox_TextChanged);
            //
            //ZFW_TextBox
            //
            this.ZFW_TextBox.Font = new System.Drawing.Font("Arial", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
            this.ZFW_TextBox.Location = new System.Drawing.Point(3, 73);
            this.ZFW_TextBox.Name = "ZFW_TextBox";
            this.ZFW_TextBox.Size = new System.Drawing.Size(100, 27);
            this.ZFW_TextBox.TabIndex = 5;
            this.ZFW_TextBox.TextChanged += new System.EventHandler(this.ZFW_TextBox_TextChanged);
            //
            //TableLayoutPanel2
            //
            this.TableLayoutPanel2.ColumnCount = 1;
            this.TableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100f));
            this.TableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20f));
            this.TableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20f));
            this.TableLayoutPanel2.Controls.Add(this.Label16, 0, 2);
            this.TableLayoutPanel2.Controls.Add(this.Label17, 0, 1);
            this.TableLayoutPanel2.Controls.Add(this.Label18, 0, 0);
            this.TableLayoutPanel2.Location = new System.Drawing.Point(19, 154);
            this.TableLayoutPanel2.Name = "TableLayoutPanel2";
            this.TableLayoutPanel2.RowCount = 3;
            this.TableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35f));
            this.TableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35f));
            this.TableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35f));
            this.TableLayoutPanel2.Size = new System.Drawing.Size(100, 105);
            this.TableLayoutPanel2.TabIndex = 1;
            //
            //Label16
            //
            this.Label16.AutoSize = true;
            this.Label16.Font = new System.Drawing.Font("Arial", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
            this.Label16.Location = new System.Drawing.Point(3, 70);
            this.Label16.Name = "Label16";
            this.Label16.Size = new System.Drawing.Size(45, 19);
            this.Label16.TabIndex = 3;
            this.Label16.Text = "ZFW";
            //
            //Label17
            //
            this.Label17.AutoSize = true;
            this.Label17.Font = new System.Drawing.Font("Arial", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
            this.Label17.Location = new System.Drawing.Point(3, 35);
            this.Label17.Name = "Label17";
            this.Label17.Size = new System.Drawing.Size(68, 19);
            this.Label17.TabIndex = 2;
            this.Label17.Text = "Payload";
            //
            //Label18
            //
            this.Label18.AutoSize = true;
            this.Label18.Font = new System.Drawing.Font("Arial", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
            this.Label18.Location = new System.Drawing.Point(3, 0);
            this.Label18.Name = "Label18";
            this.Label18.Size = new System.Drawing.Size(49, 19);
            this.Label18.TabIndex = 1;
            this.Label18.Text = "OEW";
            //
            //OK_Button
            //
            this.OK_Button.Font = new System.Drawing.Font("Arial", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
            this.OK_Button.Location = new System.Drawing.Point(193, 344);
            this.OK_Button.Name = "OK_Button";
            this.OK_Button.Size = new System.Drawing.Size(86, 28);
            this.OK_Button.TabIndex = 2;
            this.OK_Button.Text = "OK";
            this.OK_Button.UseVisualStyleBackColor = true;
            this.OK_Button.Click += new System.EventHandler(this.OK_Button_Click);
            //
            //Cancel_Button
            //
            this.Cancel_Button.Font = new System.Drawing.Font("Arial", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
            this.Cancel_Button.Location = new System.Drawing.Point(432, 344);
            this.Cancel_Button.Name = "Cancel_Button";
            this.Cancel_Button.Size = new System.Drawing.Size(86, 28);
            this.Cancel_Button.TabIndex = 3;
            this.Cancel_Button.Text = "Cancel";
            this.Cancel_Button.UseVisualStyleBackColor = true;
            this.Cancel_Button.Click += new System.EventHandler(this.Cancel_Button_Click);
            //
            //WtManagement
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(8f, 16f);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.ClientSize = new System.Drawing.Size(712, 414);
            this.Controls.Add(this.Cancel_Button);
            this.Controls.Add(this.OK_Button);
            this.Controls.Add(this.GroupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "WtManagement";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Weight Management";
            this.TableLayoutPanel1.ResumeLayout(false);
            this.TableLayoutPanel1.PerformLayout();
            this.GroupBox1.ResumeLayout(false);
            this.GroupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)this.PayloadTrackBar).EndInit();
            this.TableLayoutPanel4.ResumeLayout(false);
            this.TableLayoutPanel4.PerformLayout();
            this.TableLayoutPanel3.ResumeLayout(false);
            this.TableLayoutPanel3.PerformLayout();
            this.TableLayoutPanel2.ResumeLayout(false);
            this.TableLayoutPanel2.PerformLayout();
            this.ResumeLayout(false);

        }
        internal System.Windows.Forms.TableLayoutPanel TableLayoutPanel1;
        internal System.Windows.Forms.Label Label3;
        internal System.Windows.Forms.Label Label1;
        internal System.Windows.Forms.Label Label2;
        internal System.Windows.Forms.GroupBox GroupBox1;
        internal System.Windows.Forms.Label MaxZFW_Label;
        internal System.Windows.Forms.Label OEW_Label;
        internal System.Windows.Forms.Label MaxPayload_Label;
        internal System.Windows.Forms.Label Label5;
        internal System.Windows.Forms.Label Label6;
        internal System.Windows.Forms.Label Label4;
        internal System.Windows.Forms.TrackBar PayloadTrackBar;
        internal System.Windows.Forms.TableLayoutPanel TableLayoutPanel4;
        internal System.Windows.Forms.Label Label10;
        internal System.Windows.Forms.Label Label11;
        internal System.Windows.Forms.Label Label12;
        internal System.Windows.Forms.TableLayoutPanel TableLayoutPanel3;
        internal System.Windows.Forms.TextBox OEW_TextBox;
        internal System.Windows.Forms.TextBox Payload_TextBox;
        internal System.Windows.Forms.TextBox ZFW_TextBox;
        internal System.Windows.Forms.TableLayoutPanel TableLayoutPanel2;
        internal System.Windows.Forms.Label Label16;
        internal System.Windows.Forms.Label Label17;
        internal System.Windows.Forms.Label Label18;
        internal System.Windows.Forms.Button OK_Button;
        internal System.Windows.Forms.Button Cancel_Button;
        internal System.Windows.Forms.ComboBox WtUnit_ComboBox;
        internal System.Windows.Forms.Label Label13;
    }
}
