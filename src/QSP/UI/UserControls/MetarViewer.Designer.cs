namespace QSP.UI.UserControls
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
            this.button1 = new System.Windows.Forms.Button();
            this.metarToFindTxtBox = new System.Windows.Forms.TextBox();
            this.metarLastUpdatedLbl = new System.Windows.Forms.Label();
            this.downloadAllBtn = new System.Windows.Forms.Button();
            this.matarTafRichTxtBox = new System.Windows.Forms.RichTextBox();
            this.tableLayoutPanel5.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel5
            // 
            this.tableLayoutPanel5.AutoSize = true;
            this.tableLayoutPanel5.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel5.ColumnCount = 1;
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel5.Controls.Add(this.tableLayoutPanel4, 0, 0);
            this.tableLayoutPanel5.Controls.Add(this.matarTafRichTxtBox, 0, 1);
            this.tableLayoutPanel5.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel5.Margin = new System.Windows.Forms.Padding(2);
            this.tableLayoutPanel5.Name = "tableLayoutPanel5";
            this.tableLayoutPanel5.RowCount = 2;
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel5.Size = new System.Drawing.Size(1074, 754);
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
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel4.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel4.Controls.Add(this.button1, 1, 0);
            this.tableLayoutPanel4.Controls.Add(this.metarToFindTxtBox, 1, 0);
            this.tableLayoutPanel4.Controls.Add(this.metarLastUpdatedLbl, 4, 0);
            this.tableLayoutPanel4.Controls.Add(this.downloadAllBtn, 2, 0);
            this.tableLayoutPanel4.Location = new System.Drawing.Point(2, 2);
            this.tableLayoutPanel4.Margin = new System.Windows.Forms.Padding(2);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 1;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel4.Size = new System.Drawing.Size(785, 36);
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
            this.label1.Size = new System.Drawing.Size(50, 23);
            this.label1.TabIndex = 83;
            this.label1.Text = "ICAO";
            // 
            // button1
            // 
            this.button1.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.button1.AutoSize = true;
            this.button1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.button1.BackColor = System.Drawing.SystemColors.Control;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.Location = new System.Drawing.Point(160, 2);
            this.button1.Margin = new System.Windows.Forms.Padding(2);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(165, 32);
            this.button1.TabIndex = 82;
            this.button1.Text = "View Selected Airport";
            this.button1.UseVisualStyleBackColor = false;
            // 
            // metarToFindTxtBox
            // 
            this.metarToFindTxtBox.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.metarToFindTxtBox.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.metarToFindTxtBox.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.metarToFindTxtBox.Location = new System.Drawing.Point(56, 3);
            this.metarToFindTxtBox.Margin = new System.Windows.Forms.Padding(2);
            this.metarToFindTxtBox.Name = "metarToFindTxtBox";
            this.metarToFindTxtBox.Size = new System.Drawing.Size(100, 30);
            this.metarToFindTxtBox.TabIndex = 77;
            this.metarToFindTxtBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // metarLastUpdatedLbl
            // 
            this.metarLastUpdatedLbl.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.metarLastUpdatedLbl.AutoSize = true;
            this.metarLastUpdatedLbl.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.metarLastUpdatedLbl.Location = new System.Drawing.Point(493, 6);
            this.metarLastUpdatedLbl.Margin = new System.Windows.Forms.Padding(2);
            this.metarLastUpdatedLbl.Name = "metarLastUpdatedLbl";
            this.metarLastUpdatedLbl.Size = new System.Drawing.Size(290, 23);
            this.metarLastUpdatedLbl.TabIndex = 81;
            this.metarLastUpdatedLbl.Text = "Last Updated : 7/18/2016 6:55:48 PM";
            // 
            // downloadAllBtn
            // 
            this.downloadAllBtn.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.downloadAllBtn.AutoSize = true;
            this.downloadAllBtn.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.downloadAllBtn.BackColor = System.Drawing.SystemColors.Control;
            this.downloadAllBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.downloadAllBtn.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.downloadAllBtn.Location = new System.Drawing.Point(329, 2);
            this.downloadAllBtn.Margin = new System.Windows.Forms.Padding(2);
            this.downloadAllBtn.Name = "downloadAllBtn";
            this.downloadAllBtn.Size = new System.Drawing.Size(160, 32);
            this.downloadAllBtn.TabIndex = 78;
            this.downloadAllBtn.Text = "View DEP/ARR/ATLN";
            this.downloadAllBtn.UseVisualStyleBackColor = false;
            // 
            // matarTafRichTxtBox
            // 
            this.matarTafRichTxtBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.matarTafRichTxtBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.matarTafRichTxtBox.Font = new System.Drawing.Font("Courier New", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.matarTafRichTxtBox.Location = new System.Drawing.Point(2, 42);
            this.matarTafRichTxtBox.Margin = new System.Windows.Forms.Padding(2);
            this.matarTafRichTxtBox.MaximumSize = new System.Drawing.Size(1070, 10000);
            this.matarTafRichTxtBox.MinimumSize = new System.Drawing.Size(1070, 0);
            this.matarTafRichTxtBox.Name = "matarTafRichTxtBox";
            this.matarTafRichTxtBox.Size = new System.Drawing.Size(1070, 710);
            this.matarTafRichTxtBox.TabIndex = 0;
            this.matarTafRichTxtBox.Text = "";
            // 
            // MetarViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(120F, 120F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.Controls.Add(this.tableLayoutPanel5);
            this.DoubleBuffered = true;
            this.Name = "MetarViewer";
            this.Size = new System.Drawing.Size(1076, 756);
            this.tableLayoutPanel5.ResumeLayout(false);
            this.tableLayoutPanel5.PerformLayout();
            this.tableLayoutPanel4.ResumeLayout(false);
            this.tableLayoutPanel4.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel5;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private System.Windows.Forms.Label label1;
        internal System.Windows.Forms.Button button1;
        internal System.Windows.Forms.TextBox metarToFindTxtBox;
        private System.Windows.Forms.Label metarLastUpdatedLbl;
        internal System.Windows.Forms.Button downloadAllBtn;
        internal System.Windows.Forms.RichTextBox matarTafRichTxtBox;
    }
}
