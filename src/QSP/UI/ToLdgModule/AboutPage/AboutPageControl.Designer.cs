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
            this.githubBtn = new System.Windows.Forms.Button();
            this.siteBtn = new System.Windows.Forms.Button();
            this.licenseBtn = new System.Windows.Forms.Button();
            this.versionLbl = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtLbl
            // 
            this.txtLbl.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.txtLbl.AutoSize = true;
            this.txtLbl.BackColor = System.Drawing.Color.Transparent;
            this.txtLbl.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtLbl.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtLbl.Location = new System.Drawing.Point(6, 112);
            this.txtLbl.Name = "txtLbl";
            this.txtLbl.Size = new System.Drawing.Size(533, 94);
            this.txtLbl.TabIndex = 0;
            this.txtLbl.Text = "This is an open-source software, distributed under the MIT License. \r\nFor quesion" +
    "s, bug reports and suggestions, you can leave a message\r\non our webpage.\r\n\r\n";
            // 
            // panel1
            // 
            this.panel1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.panel1.Controls.Add(this.githubBtn);
            this.panel1.Controls.Add(this.siteBtn);
            this.panel1.Controls.Add(this.licenseBtn);
            this.panel1.Controls.Add(this.versionLbl);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.txtLbl);
            this.panel1.Location = new System.Drawing.Point(257, 95);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(543, 337);
            this.panel1.TabIndex = 1;
            // 
            // githubBtn
            // 
            this.githubBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.githubBtn.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.githubBtn.Location = new System.Drawing.Point(299, 243);
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
            this.siteBtn.Location = new System.Drawing.Point(219, 243);
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
            this.licenseBtn.Location = new System.Drawing.Point(139, 243);
            this.licenseBtn.Name = "licenseBtn";
            this.licenseBtn.Size = new System.Drawing.Size(81, 33);
            this.licenseBtn.TabIndex = 3;
            this.licenseBtn.Text = "License";
            this.licenseBtn.UseVisualStyleBackColor = true;
            this.licenseBtn.Click += new System.EventHandler(this.licenseBtn_Click);
            // 
            // versionLbl
            // 
            this.versionLbl.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.versionLbl.AutoSize = true;
            this.versionLbl.BackColor = System.Drawing.Color.Transparent;
            this.versionLbl.Font = new System.Drawing.Font("Segoe UI", 10.8F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.versionLbl.Location = new System.Drawing.Point(338, 54);
            this.versionLbl.Name = "versionLbl";
            this.versionLbl.Size = new System.Drawing.Size(45, 25);
            this.versionLbl.TabIndex = 2;
            this.versionLbl.Text = "v0.1";
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 19.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(186, 38);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(142, 45);
            this.label1.TabIndex = 1;
            this.label1.Text = "QSP Lite";
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
            this.Size = new System.Drawing.Size(1024, 610);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label txtLbl;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label versionLbl;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button githubBtn;
        private System.Windows.Forms.Button siteBtn;
        private System.Windows.Forms.Button licenseBtn;
    }
}
