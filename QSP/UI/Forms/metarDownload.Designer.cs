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
    partial class METAR : System.Windows.Forms.Form
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

        private System.ComponentModel.IContainer components;
        //NOTE: The following procedure is required by the Windows Form Designer
        //It can be modified using the Windows Form Designer.  
        //Do not modify it using the code editor.
        [System.Diagnostics.DebuggerStepThrough()]
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.Label1 = new System.Windows.Forms.Label();
            this.icao_code = new System.Windows.Forms.TextBox();
            this.metar_download = new System.Windows.Forms.CheckBox();
            this.TAF_download = new System.Windows.Forms.CheckBox();
            this.result_box = new System.Windows.Forms.RichTextBox();
            this.download_btn = new System.Windows.Forms.Button();
            this.send_btn = new System.Windows.Forms.Button();
            this.PicBox = new System.Windows.Forms.PictureBox();
            this.Timer1 = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.PicBox)).BeginInit();
            this.SuspendLayout();
            // 
            // Label1
            // 
            this.Label1.AutoSize = true;
            this.Label1.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label1.Location = new System.Drawing.Point(65, 46);
            this.Label1.Name = "Label1";
            this.Label1.Size = new System.Drawing.Size(100, 22);
            this.Label1.TabIndex = 0;
            this.Label1.Text = "ICAO code";
            // 
            // icao_code
            // 
            this.icao_code.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.icao_code.Font = new System.Drawing.Font("Times New Roman", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.icao_code.Location = new System.Drawing.Point(193, 43);
            this.icao_code.Name = "icao_code";
            this.icao_code.Size = new System.Drawing.Size(90, 27);
            this.icao_code.TabIndex = 1;
            // 
            // metar_download
            // 
            this.metar_download.AutoSize = true;
            this.metar_download.Enabled = false;
            this.metar_download.Font = new System.Drawing.Font("Times New Roman", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.metar_download.Location = new System.Drawing.Point(380, 47);
            this.metar_download.Name = "metar_download";
            this.metar_download.Size = new System.Drawing.Size(85, 23);
            this.metar_download.TabIndex = 2;
            this.metar_download.Text = "MATAR";
            this.metar_download.UseVisualStyleBackColor = true;
            // 
            // TAF_download
            // 
            this.TAF_download.AutoSize = true;
            this.TAF_download.Font = new System.Drawing.Font("Times New Roman", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TAF_download.Location = new System.Drawing.Point(510, 47);
            this.TAF_download.Name = "TAF_download";
            this.TAF_download.Size = new System.Drawing.Size(59, 23);
            this.TAF_download.TabIndex = 3;
            this.TAF_download.Text = "TAF";
            this.TAF_download.UseVisualStyleBackColor = true;
            // 
            // result_box
            // 
            this.result_box.Font = new System.Drawing.Font("Courier New", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.result_box.Location = new System.Drawing.Point(69, 102);
            this.result_box.Name = "result_box";
            this.result_box.Size = new System.Drawing.Size(1104, 394);
            this.result_box.TabIndex = 4;
            this.result_box.Text = "";
            // 
            // download_btn
            // 
            this.download_btn.Font = new System.Drawing.Font("Times New Roman", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.download_btn.Location = new System.Drawing.Point(627, 39);
            this.download_btn.Name = "download_btn";
            this.download_btn.Size = new System.Drawing.Size(209, 36);
            this.download_btn.TabIndex = 5;
            this.download_btn.Text = "Download / Refresh";
            this.download_btn.UseVisualStyleBackColor = true;
            this.download_btn.Click += new System.EventHandler(this.download_btn_Click);
            // 
            // send_btn
            // 
            this.send_btn.Font = new System.Drawing.Font("Times New Roman", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.send_btn.Location = new System.Drawing.Point(931, 39);
            this.send_btn.Name = "send_btn";
            this.send_btn.Size = new System.Drawing.Size(236, 36);
            this.send_btn.TabIndex = 6;
            this.send_btn.Text = "Send Weather";
            this.send_btn.UseVisualStyleBackColor = true;
            this.send_btn.Click += new System.EventHandler(this.send_btn_Click);
            // 
            // PicBox
            // 
            this.PicBox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.PicBox.Image = global::QSP.Properties.Resources.checkIconLarge;
            this.PicBox.Location = new System.Drawing.Point(1173, 39);
            this.PicBox.Name = "PicBox";
            this.PicBox.Size = new System.Drawing.Size(36, 36);
            this.PicBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.PicBox.TabIndex = 1;
            this.PicBox.TabStop = false;
            // 
            // Timer1
            // 
            this.Timer1.Interval = 20;
            // 
            // METAR
            // 
            this.AcceptButton = this.download_btn;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1242, 508);
            this.Controls.Add(this.PicBox);
            this.Controls.Add(this.send_btn);
            this.Controls.Add(this.download_btn);
            this.Controls.Add(this.result_box);
            this.Controls.Add(this.TAF_download);
            this.Controls.Add(this.metar_download);
            this.Controls.Add(this.icao_code);
            this.Controls.Add(this.Label1);
            this.Name = "METAR";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "METAR";
            ((System.ComponentModel.ISupportInitialize)(this.PicBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        internal System.Windows.Forms.Label Label1;
        internal System.Windows.Forms.TextBox icao_code;
        internal System.Windows.Forms.CheckBox metar_download;
        internal System.Windows.Forms.CheckBox TAF_download;
        internal System.Windows.Forms.RichTextBox result_box;
        internal System.Windows.Forms.Button download_btn;
        internal System.Windows.Forms.Button send_btn;
        internal System.Windows.Forms.PictureBox PicBox;
        internal System.Windows.Forms.Timer Timer1;
    }
}
