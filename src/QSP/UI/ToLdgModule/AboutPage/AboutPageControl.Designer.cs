namespace QSP.UI.ToLdgModule.AboutPage
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.manualBtn = new System.Windows.Forms.Button();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.versionLbl = new System.Windows.Forms.Label();
            this.appNameLbl = new System.Windows.Forms.Label();
            this.githubBtn = new System.Windows.Forms.Button();
            this.siteBtn = new System.Windows.Forms.Button();
            this.licenseBtn = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtLbl
            // 
            this.txtLbl.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.txtLbl.AutoSize = true;
            this.txtLbl.BackColor = System.Drawing.Color.Transparent;
            this.txtLbl.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtLbl.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtLbl.Location = new System.Drawing.Point(8, 112);
            this.txtLbl.Name = "txtLbl";
            this.txtLbl.Size = new System.Drawing.Size(526, 94);
            this.txtLbl.TabIndex = 0;
            this.txtLbl.Text = "This is an open-source software, released under MIT license.\r\nFor quesions, bug r" +
    "eports and suggestions, please leave a message \r\non our webpage.\r\n\r\n";
            // 
            // panel1
            // 
            this.panel1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.panel1.Controls.Add(this.manualBtn);
            this.panel1.Controls.Add(this.tableLayoutPanel2);
            this.panel1.Controls.Add(this.githubBtn);
            this.panel1.Controls.Add(this.siteBtn);
            this.panel1.Controls.Add(this.licenseBtn);
            this.panel1.Controls.Add(this.txtLbl);
            this.panel1.Location = new System.Drawing.Point(295, 125);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(543, 337);
            this.panel1.TabIndex = 1;
            // 
            // manualBtn
            // 
            this.manualBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.manualBtn.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.manualBtn.Location = new System.Drawing.Point(106, 243);
            this.manualBtn.Name = "manualBtn";
            this.manualBtn.Size = new System.Drawing.Size(81, 33);
            this.manualBtn.TabIndex = 8;
            this.manualBtn.Text = "Manual";
            this.manualBtn.UseVisualStyleBackColor = true;
            this.manualBtn.Click += new System.EventHandler(this.manualBtn_Click);
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 1;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Controls.Add(this.tableLayoutPanel1, 0, 0);
            this.tableLayoutPanel2.Location = new System.Drawing.Point(6, 37);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(533, 66);
            this.tableLayoutPanel2.TabIndex = 7;
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
            this.tableLayoutPanel1.Location = new System.Drawing.Point(134, 3);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(265, 45);
            this.tableLayoutPanel1.TabIndex = 6;
            // 
            // versionLbl
            // 
            this.versionLbl.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.versionLbl.AutoSize = true;
            this.versionLbl.BackColor = System.Drawing.Color.Transparent;
            this.versionLbl.Font = new System.Drawing.Font("Segoe UI", 10.8F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.versionLbl.Location = new System.Drawing.Point(217, 20);
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
            this.appNameLbl.Location = new System.Drawing.Point(3, 0);
            this.appNameLbl.Name = "appNameLbl";
            this.appNameLbl.Size = new System.Drawing.Size(208, 45);
            this.appNameLbl.TabIndex = 1;
            this.appNameLbl.Text = "QSimPlanner";
            // 
            // githubBtn
            // 
            this.githubBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.githubBtn.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.githubBtn.Location = new System.Drawing.Point(346, 243);
            this.githubBtn.Name = "githubBtn";
            this.githubBtn.Size = new System.Drawing.Size(81, 33);
            this.githubBtn.TabIndex = 5;
            this.githubBtn.Text = "Github";
            this.githubBtn.UseVisualStyleBackColor = true;
            this.githubBtn.Click += new System.EventHandler(this.githubBtn_Click);
            // 
            // siteBtn
            // 
            this.siteBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.siteBtn.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.siteBtn.Location = new System.Drawing.Point(266, 243);
            this.siteBtn.Name = "siteBtn";
            this.siteBtn.Size = new System.Drawing.Size(81, 33);
            this.siteBtn.TabIndex = 4;
            this.siteBtn.Text = "Website";
            this.siteBtn.UseVisualStyleBackColor = true;
            this.siteBtn.Click += new System.EventHandler(this.siteBtn_Click);
            // 
            // licenseBtn
            // 
            this.licenseBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.licenseBtn.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.licenseBtn.Location = new System.Drawing.Point(186, 243);
            this.licenseBtn.Name = "licenseBtn";
            this.licenseBtn.Size = new System.Drawing.Size(81, 33);
            this.licenseBtn.TabIndex = 3;
            this.licenseBtn.Text = "License";
            this.licenseBtn.UseVisualStyleBackColor = true;
            this.licenseBtn.Click += new System.EventHandler(this.licenseBtn_Click);
            // 
            // AboutPageControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(120F, 120F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.BackgroundImage = global::QSP.Properties.Resources.chart3Faa;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.Controls.Add(this.panel1);
            this.DoubleBuffered = true;
            this.Name = "AboutPageControl";
            this.Size = new System.Drawing.Size(1100, 670);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label txtLbl;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label versionLbl;
        private System.Windows.Forms.Label appNameLbl;
        private System.Windows.Forms.Button githubBtn;
        private System.Windows.Forms.Button siteBtn;
        private System.Windows.Forms.Button licenseBtn;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Button manualBtn;
    }
}
