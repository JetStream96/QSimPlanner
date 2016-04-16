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
    partial class MetarForm : System.Windows.Forms.Form
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

        private System.ComponentModel.IContainer components = null;
        //NOTE: The following procedure is required by the Windows Form Designer
        //It can be modified using the Windows Form Designer.  
        //Do not modify it using the code editor.
        [System.Diagnostics.DebuggerStepThrough()]
        private void InitializeComponent()
        {
            this.Label1 = new System.Windows.Forms.Label();
            this.icaoTxtBox = new System.Windows.Forms.TextBox();
            this.getMetarCheckBox = new System.Windows.Forms.CheckBox();
            this.getTafCheckBox = new System.Windows.Forms.CheckBox();
            this.resultRichTxtBox = new System.Windows.Forms.RichTextBox();
            this.downloadBtn = new System.Windows.Forms.Button();
            this.sendBtn = new System.Windows.Forms.Button();
            this.PicBox = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.PicBox)).BeginInit();
            this.SuspendLayout();
            // 
            // Label1
            // 
            this.Label1.AutoSize = true;
            this.Label1.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label1.Location = new System.Drawing.Point(65, 46);
            this.Label1.Name = "Label1";
            this.Label1.Size = new System.Drawing.Size(92, 23);
            this.Label1.TabIndex = 0;
            this.Label1.Text = "ICAO code";
            // 
            // icaoTxtBox
            // 
            this.icaoTxtBox.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.icaoTxtBox.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.icaoTxtBox.Location = new System.Drawing.Point(193, 43);
            this.icaoTxtBox.Name = "icaoTxtBox";
            this.icaoTxtBox.Size = new System.Drawing.Size(90, 30);
            this.icaoTxtBox.TabIndex = 1;
            // 
            // getMetarCheckBox
            // 
            this.getMetarCheckBox.AutoSize = true;
            this.getMetarCheckBox.Enabled = false;
            this.getMetarCheckBox.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.getMetarCheckBox.Location = new System.Drawing.Point(379, 47);
            this.getMetarCheckBox.Name = "getMetarCheckBox";
            this.getMetarCheckBox.Size = new System.Drawing.Size(88, 27);
            this.getMetarCheckBox.TabIndex = 2;
            this.getMetarCheckBox.Text = "MATAR";
            this.getMetarCheckBox.UseVisualStyleBackColor = true;
            // 
            // getTafCheckBox
            // 
            this.getTafCheckBox.AutoSize = true;
            this.getTafCheckBox.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.getTafCheckBox.Location = new System.Drawing.Point(510, 47);
            this.getTafCheckBox.Name = "getTafCheckBox";
            this.getTafCheckBox.Size = new System.Drawing.Size(60, 27);
            this.getTafCheckBox.TabIndex = 3;
            this.getTafCheckBox.Text = "TAF";
            this.getTafCheckBox.UseVisualStyleBackColor = true;
            // 
            // resultRichTxtBox
            // 
            this.resultRichTxtBox.Font = new System.Drawing.Font("Courier New", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.resultRichTxtBox.Location = new System.Drawing.Point(69, 102);
            this.resultRichTxtBox.Name = "resultRichTxtBox";
            this.resultRichTxtBox.Size = new System.Drawing.Size(1103, 394);
            this.resultRichTxtBox.TabIndex = 4;
            this.resultRichTxtBox.Text = "";
            // 
            // downloadBtn
            // 
            this.downloadBtn.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.downloadBtn.Location = new System.Drawing.Point(627, 39);
            this.downloadBtn.Name = "downloadBtn";
            this.downloadBtn.Size = new System.Drawing.Size(209, 36);
            this.downloadBtn.TabIndex = 5;
            this.downloadBtn.Text = "Download / Refresh";
            this.downloadBtn.UseVisualStyleBackColor = true;
            this.downloadBtn.Click += new System.EventHandler(this.downloadBtn_Click);
            // 
            // sendBtn
            // 
            this.sendBtn.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.sendBtn.Location = new System.Drawing.Point(889, 39);
            this.sendBtn.Name = "sendBtn";
            this.sendBtn.Size = new System.Drawing.Size(235, 36);
            this.sendBtn.TabIndex = 6;
            this.sendBtn.Text = "Send Weather";
            this.sendBtn.UseVisualStyleBackColor = true;
            this.sendBtn.Click += new System.EventHandler(this.sendBtn_Click);
            // 
            // PicBox
            // 
            this.PicBox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.PicBox.Image = global::QSP.Properties.Resources.checkIconLarge;
            this.PicBox.Location = new System.Drawing.Point(1131, 39);
            this.PicBox.Name = "PicBox";
            this.PicBox.Size = new System.Drawing.Size(37, 36);
            this.PicBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.PicBox.TabIndex = 1;
            this.PicBox.TabStop = false;
            // 
            // MetarForm
            // 
            this.AcceptButton = this.downloadBtn;
            this.AutoScaleDimensions = new System.Drawing.SizeF(120F, 120F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1242, 508);
            this.Controls.Add(this.PicBox);
            this.Controls.Add(this.sendBtn);
            this.Controls.Add(this.downloadBtn);
            this.Controls.Add(this.resultRichTxtBox);
            this.Controls.Add(this.getTafCheckBox);
            this.Controls.Add(this.getMetarCheckBox);
            this.Controls.Add(this.icaoTxtBox);
            this.Controls.Add(this.Label1);
            this.Font = new System.Drawing.Font("Segoe UI", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "MetarForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "METAR";
            ((System.ComponentModel.ISupportInitialize)(this.PicBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        internal System.Windows.Forms.Label Label1;
        internal System.Windows.Forms.TextBox icaoTxtBox;
        internal System.Windows.Forms.CheckBox getMetarCheckBox;
        internal System.Windows.Forms.CheckBox getTafCheckBox;
        internal System.Windows.Forms.RichTextBox resultRichTxtBox;
        internal System.Windows.Forms.Button downloadBtn;
        internal System.Windows.Forms.Button sendBtn;
        internal System.Windows.Forms.PictureBox PicBox;
    }
}
