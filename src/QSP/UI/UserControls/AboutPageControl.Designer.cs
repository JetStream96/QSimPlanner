namespace QSP.UI.UserControls
{
    partial class AboutPageControl
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
            this.txtLbl = new System.Windows.Forms.Label();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.versionLbl = new System.Windows.Forms.Label();
            this.appNameLbl = new System.Windows.Forms.Label();
            this.githubBtn = new System.Windows.Forms.Button();
            this.siteBtn = new System.Windows.Forms.Button();
            this.licenseBtn = new System.Windows.Forms.Button();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtLbl
            // 
            this.txtLbl.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.txtLbl.AutoSize = true;
            this.txtLbl.BackColor = System.Drawing.Color.Transparent;
            this.txtLbl.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtLbl.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtLbl.Location = new System.Drawing.Point(12, 104);
            this.txtLbl.Margin = new System.Windows.Forms.Padding(12, 38, 12, 38);
            this.txtLbl.Name = "txtLbl";
            this.txtLbl.Size = new System.Drawing.Size(526, 94);
            this.txtLbl.TabIndex = 0;
            this.txtLbl.Text = "This is an open-source software, released under MIT license.\r\nFor quesions, bug r" +
    "eports and suggestions, please leave a message \r\non our webpage.\r\n\r\n";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.tableLayoutPanel1.AutoSize = true;
            this.tableLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.versionLbl, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.appNameLbl, 0, 0);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(144, 19);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(2, 19, 2, 2);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(261, 45);
            this.tableLayoutPanel1.TabIndex = 6;
            // 
            // versionLbl
            // 
            this.versionLbl.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.versionLbl.AutoSize = true;
            this.versionLbl.BackColor = System.Drawing.Color.Transparent;
            this.versionLbl.Font = new System.Drawing.Font("Segoe UI", 10.8F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.versionLbl.Location = new System.Drawing.Point(214, 20);
            this.versionLbl.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.versionLbl.Name = "versionLbl";
            this.versionLbl.Size = new System.Drawing.Size(45, 25);
            this.versionLbl.TabIndex = 2;
            this.versionLbl.Text = "v0.1";
            // 
            // appNameLbl
            // 
            this.appNameLbl.AutoSize = true;
            this.appNameLbl.BackColor = System.Drawing.Color.Transparent;
            this.appNameLbl.Font = new System.Drawing.Font("Segoe UI", 19.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.appNameLbl.Location = new System.Drawing.Point(2, 0);
            this.appNameLbl.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.appNameLbl.Name = "appNameLbl";
            this.appNameLbl.Size = new System.Drawing.Size(208, 45);
            this.appNameLbl.TabIndex = 1;
            this.appNameLbl.Text = "QSimPlanner";
            // 
            // githubBtn
            // 
            this.githubBtn.AutoSize = true;
            this.githubBtn.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.githubBtn.Cursor = System.Windows.Forms.Cursors.Hand;
            this.githubBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.githubBtn.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.githubBtn.Location = new System.Drawing.Point(170, 2);
            this.githubBtn.Margin = new System.Windows.Forms.Padding(0, 2, 0, 2);
            this.githubBtn.MinimumSize = new System.Drawing.Size(85, 0);
            this.githubBtn.Name = "githubBtn";
            this.githubBtn.Size = new System.Drawing.Size(85, 32);
            this.githubBtn.TabIndex = 5;
            this.githubBtn.Text = "Github";
            this.githubBtn.UseVisualStyleBackColor = true;
            this.githubBtn.Click += new System.EventHandler(this.githubBtn_Click);
            // 
            // siteBtn
            // 
            this.siteBtn.AutoSize = true;
            this.siteBtn.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.siteBtn.Cursor = System.Windows.Forms.Cursors.Hand;
            this.siteBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.siteBtn.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.siteBtn.Location = new System.Drawing.Point(85, 2);
            this.siteBtn.Margin = new System.Windows.Forms.Padding(0, 2, 0, 2);
            this.siteBtn.MinimumSize = new System.Drawing.Size(85, 0);
            this.siteBtn.Name = "siteBtn";
            this.siteBtn.Size = new System.Drawing.Size(85, 32);
            this.siteBtn.TabIndex = 4;
            this.siteBtn.Text = "Website";
            this.siteBtn.UseVisualStyleBackColor = true;
            this.siteBtn.Click += new System.EventHandler(this.siteBtn_Click);
            // 
            // licenseBtn
            // 
            this.licenseBtn.AutoSize = true;
            this.licenseBtn.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.licenseBtn.Cursor = System.Windows.Forms.Cursors.Hand;
            this.licenseBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.licenseBtn.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.licenseBtn.Location = new System.Drawing.Point(0, 2);
            this.licenseBtn.Margin = new System.Windows.Forms.Padding(0, 2, 0, 2);
            this.licenseBtn.MinimumSize = new System.Drawing.Size(85, 0);
            this.licenseBtn.Name = "licenseBtn";
            this.licenseBtn.Size = new System.Drawing.Size(85, 32);
            this.licenseBtn.TabIndex = 3;
            this.licenseBtn.Text = "License";
            this.licenseBtn.UseVisualStyleBackColor = true;
            this.licenseBtn.Click += new System.EventHandler(this.licenseBtn_Click);
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.AutoSize = true;
            this.tableLayoutPanel3.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel3.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.tableLayoutPanel3.ColumnCount = 1;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Controls.Add(this.tableLayoutPanel1, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.tableLayoutPanel4, 0, 2);
            this.tableLayoutPanel3.Controls.Add(this.txtLbl, 0, 1);
            this.tableLayoutPanel3.Location = new System.Drawing.Point(292, 172);
            this.tableLayoutPanel3.Margin = new System.Windows.Forms.Padding(4);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 3;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(550, 295);
            this.tableLayoutPanel3.TabIndex = 2;
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.tableLayoutPanel4.AutoSize = true;
            this.tableLayoutPanel4.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel4.ColumnCount = 3;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 85F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 85F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 85F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel4.Controls.Add(this.githubBtn, 2, 0);
            this.tableLayoutPanel4.Controls.Add(this.licenseBtn, 0, 0);
            this.tableLayoutPanel4.Controls.Add(this.siteBtn, 1, 0);
            this.tableLayoutPanel4.Location = new System.Drawing.Point(147, 240);
            this.tableLayoutPanel4.Margin = new System.Windows.Forms.Padding(4, 4, 4, 19);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 1;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel4.Size = new System.Drawing.Size(255, 36);
            this.tableLayoutPanel4.TabIndex = 9;
            // 
            // AboutPageControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(120F, 120F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.BackgroundImage = global::QSP.Properties.Resources.chartFaa;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.Controls.Add(this.tableLayoutPanel3);
            this.DoubleBuffered = true;
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "AboutPageControl";
            this.Size = new System.Drawing.Size(1124, 670);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel3.PerformLayout();
            this.tableLayoutPanel4.ResumeLayout(false);
            this.tableLayoutPanel4.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label txtLbl;
        private System.Windows.Forms.Label versionLbl;
        private System.Windows.Forms.Label appNameLbl;
        private System.Windows.Forms.Button githubBtn;
        private System.Windows.Forms.Button siteBtn;
        private System.Windows.Forms.Button licenseBtn;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
    }
}
