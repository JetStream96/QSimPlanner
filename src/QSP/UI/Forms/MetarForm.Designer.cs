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
            this.resultRichTxtBox = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // Label1
            // 
            this.Label1.AutoSize = true;
            this.Label1.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label1.Location = new System.Drawing.Point(31, 30);
            this.Label1.Name = "Label1";
            this.Label1.Size = new System.Drawing.Size(92, 23);
            this.Label1.TabIndex = 0;
            this.Label1.Text = "ICAO code";
            // 
            // icaoTxtBox
            // 
            this.icaoTxtBox.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.icaoTxtBox.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.icaoTxtBox.Location = new System.Drawing.Point(159, 27);
            this.icaoTxtBox.Name = "icaoTxtBox";
            this.icaoTxtBox.Size = new System.Drawing.Size(90, 30);
            this.icaoTxtBox.TabIndex = 1;
            // 
            // resultRichTxtBox
            // 
            this.resultRichTxtBox.Font = new System.Drawing.Font("Courier New", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.resultRichTxtBox.Location = new System.Drawing.Point(35, 86);
            this.resultRichTxtBox.Name = "resultRichTxtBox";
            this.resultRichTxtBox.Size = new System.Drawing.Size(1103, 394);
            this.resultRichTxtBox.TabIndex = 4;
            this.resultRichTxtBox.Text = "";
            // 
            // MetarForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(120F, 120F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1182, 508);
            this.Controls.Add(this.resultRichTxtBox);
            this.Controls.Add(this.icaoTxtBox);
            this.Controls.Add(this.Label1);
            this.Font = new System.Drawing.Font("Segoe UI", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "MetarForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "METAR";
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        internal System.Windows.Forms.Label Label1;
        internal System.Windows.Forms.TextBox icaoTxtBox;
        internal System.Windows.Forms.RichTextBox resultRichTxtBox;
    }
}
