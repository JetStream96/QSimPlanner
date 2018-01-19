namespace QSP.UI.Views.MiscInfo
{
    partial class MetarViewer
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tableLayoutPanel5 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.lastUpdatedLbl = new System.Windows.Forms.Label();
            this.downloadAllBtn = new System.Windows.Forms.Button();
            this.IcaoTxtBox = new System.Windows.Forms.TextBox();
            this.statusPicBox = new System.Windows.Forms.PictureBox();
            this.metarTafRichTxtBox = new System.Windows.Forms.RichTextBox();
            this.tableLayoutPanel5.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.statusPicBox)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel5
            // 
            this.tableLayoutPanel5.AutoSize = true;
            this.tableLayoutPanel5.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel5.ColumnCount = 1;
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel5.Controls.Add(this.tableLayoutPanel4, 0, 0);
            this.tableLayoutPanel5.Controls.Add(this.metarTafRichTxtBox, 0, 1);
            this.tableLayoutPanel5.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel5.Margin = new System.Windows.Forms.Padding(2);
            this.tableLayoutPanel5.Name = "tableLayoutPanel5";
            this.tableLayoutPanel5.RowCount = 2;
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 16F));
            this.tableLayoutPanel5.Size = new System.Drawing.Size(860, 88);
            this.tableLayoutPanel5.TabIndex = 81;
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.AutoSize = true;
            this.tableLayoutPanel4.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel4.ColumnCount = 5;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel4.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel4.Controls.Add(this.lastUpdatedLbl, 4, 0);
            this.tableLayoutPanel4.Controls.Add(this.downloadAllBtn, 3, 0);
            this.tableLayoutPanel4.Controls.Add(this.IcaoTxtBox, 1, 0);
            this.tableLayoutPanel4.Controls.Add(this.statusPicBox, 2, 0);
            this.tableLayoutPanel4.Location = new System.Drawing.Point(2, 2);
            this.tableLayoutPanel4.Margin = new System.Windows.Forms.Padding(2);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 1;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel4.Size = new System.Drawing.Size(536, 31);
            this.tableLayoutPanel4.TabIndex = 79;
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(2, 6);
            this.label1.Margin = new System.Windows.Forms.Padding(2);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(42, 19);
            this.label1.TabIndex = 83;
            this.label1.Text = "ICAO";
            // 
            // lastUpdatedLbl
            // 
            this.lastUpdatedLbl.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lastUpdatedLbl.AutoSize = true;
            this.lastUpdatedLbl.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lastUpdatedLbl.Location = new System.Drawing.Point(291, 6);
            this.lastUpdatedLbl.Margin = new System.Windows.Forms.Padding(2);
            this.lastUpdatedLbl.Name = "lastUpdatedLbl";
            this.lastUpdatedLbl.Size = new System.Drawing.Size(243, 19);
            this.lastUpdatedLbl.TabIndex = 0;
            this.lastUpdatedLbl.Text = "Last Updated : 7/18/2016 6:55:48 PM";
            // 
            // downloadAllBtn
            // 
            this.downloadAllBtn.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.downloadAllBtn.AutoSize = true;
            this.downloadAllBtn.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.downloadAllBtn.BackColor = System.Drawing.SystemColors.Control;
            this.downloadAllBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.downloadAllBtn.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.downloadAllBtn.Location = new System.Drawing.Point(158, 2);
            this.downloadAllBtn.Margin = new System.Windows.Forms.Padding(2);
            this.downloadAllBtn.Name = "downloadAllBtn";
            this.downloadAllBtn.Size = new System.Drawing.Size(129, 27);
            this.downloadAllBtn.TabIndex = 1;
            this.downloadAllBtn.Text = "View DEP/ARR/ATLN";
            this.downloadAllBtn.UseVisualStyleBackColor = false;
            // 
            // IcaoTxtBox
            // 
            this.IcaoTxtBox.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.IcaoTxtBox.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.IcaoTxtBox.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IcaoTxtBox.Location = new System.Drawing.Point(48, 2);
            this.IcaoTxtBox.Margin = new System.Windows.Forms.Padding(2);
            this.IcaoTxtBox.Name = "IcaoTxtBox";
            this.IcaoTxtBox.Size = new System.Drawing.Size(81, 26);
            this.IcaoTxtBox.TabIndex = 0;
            this.IcaoTxtBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // statusPicBox
            // 
            this.statusPicBox.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.statusPicBox.Location = new System.Drawing.Point(133, 5);
            this.statusPicBox.Margin = new System.Windows.Forms.Padding(2);
            this.statusPicBox.Name = "statusPicBox";
            this.statusPicBox.Size = new System.Drawing.Size(21, 21);
            this.statusPicBox.TabIndex = 84;
            this.statusPicBox.TabStop = false;
            // 
            // metarTafRichTxtBox
            // 
            this.metarTafRichTxtBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.metarTafRichTxtBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.metarTafRichTxtBox.Font = new System.Drawing.Font("Courier New", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.metarTafRichTxtBox.Location = new System.Drawing.Point(2, 37);
            this.metarTafRichTxtBox.Margin = new System.Windows.Forms.Padding(2);
            this.metarTafRichTxtBox.MaximumSize = new System.Drawing.Size(856, 8000);
            this.metarTafRichTxtBox.MinimumSize = new System.Drawing.Size(856, 0);
            this.metarTafRichTxtBox.Name = "metarTafRichTxtBox";
            this.metarTafRichTxtBox.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.None;
            this.metarTafRichTxtBox.Size = new System.Drawing.Size(856, 49);
            this.metarTafRichTxtBox.TabIndex = 0;
            this.metarTafRichTxtBox.Text = "";
            // 
            // MetarViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.Controls.Add(this.tableLayoutPanel5);
            this.DoubleBuffered = true;
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "MetarViewer";
            this.Size = new System.Drawing.Size(862, 90);
            this.Load += new System.EventHandler(this.MetarViewer_Load);
            this.tableLayoutPanel5.ResumeLayout(false);
            this.tableLayoutPanel5.PerformLayout();
            this.tableLayoutPanel4.ResumeLayout(false);
            this.tableLayoutPanel4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.statusPicBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel5;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private System.Windows.Forms.Label label1;
        internal System.Windows.Forms.TextBox IcaoTxtBox;
        private System.Windows.Forms.Label lastUpdatedLbl;
        internal System.Windows.Forms.RichTextBox metarTafRichTxtBox;
        private System.Windows.Forms.PictureBox statusPicBox;
        internal System.Windows.Forms.Button downloadAllBtn;
    }
}
