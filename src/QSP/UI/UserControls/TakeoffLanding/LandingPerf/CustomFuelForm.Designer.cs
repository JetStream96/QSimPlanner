namespace QSP.UI.UserControls.TakeoffLanding.LandingPerf
{
    partial class CustomFuelForm
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.Label95 = new System.Windows.Forms.Label();
            this.cancelBtn = new System.Windows.Forms.Button();
            this.importFuelBtn = new System.Windows.Forms.Button();
            this.okBtn = new System.Windows.Forms.Button();
            this.landingFuelTxtBox = new System.Windows.Forms.TextBox();
            this.wtUnitLbl = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // Label95
            // 
            this.Label95.AutoSize = true;
            this.Label95.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label95.Location = new System.Drawing.Point(15, 47);
            this.Label95.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.Label95.Name = "Label95";
            this.Label95.Size = new System.Drawing.Size(126, 20);
            this.Label95.TabIndex = 55;
            this.Label95.Text = "Enter landing fuel";
            // 
            // cancelBtn
            // 
            this.cancelBtn.BackColor = System.Drawing.SystemColors.Control;
            this.cancelBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cancelBtn.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cancelBtn.Location = new System.Drawing.Point(159, 83);
            this.cancelBtn.Name = "cancelBtn";
            this.cancelBtn.Size = new System.Drawing.Size(72, 32);
            this.cancelBtn.TabIndex = 3;
            this.cancelBtn.Text = "Cancel";
            this.cancelBtn.UseVisualStyleBackColor = false;
            this.cancelBtn.Click += new System.EventHandler(this.cancelBtn_Click);
            // 
            // importFuelBtn
            // 
            this.importFuelBtn.AutoSize = true;
            this.importFuelBtn.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.importFuelBtn.BackColor = System.Drawing.SystemColors.Control;
            this.importFuelBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.importFuelBtn.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.importFuelBtn.Location = new System.Drawing.Point(8, 3);
            this.importFuelBtn.Name = "importFuelBtn";
            this.importFuelBtn.Size = new System.Drawing.Size(196, 32);
            this.importFuelBtn.TabIndex = 0;
            this.importFuelBtn.Text = "Use predicted landing fuel";
            this.importFuelBtn.UseVisualStyleBackColor = false;
            this.importFuelBtn.Click += new System.EventHandler(this.importFuelBtn_Click);
            // 
            // okBtn
            // 
            this.okBtn.BackColor = System.Drawing.SystemColors.Control;
            this.okBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.okBtn.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.okBtn.Location = new System.Drawing.Point(80, 83);
            this.okBtn.Name = "okBtn";
            this.okBtn.Size = new System.Drawing.Size(72, 32);
            this.okBtn.TabIndex = 2;
            this.okBtn.Text = "OK";
            this.okBtn.UseVisualStyleBackColor = false;
            this.okBtn.Click += new System.EventHandler(this.okBtn_Click);
            // 
            // landingFuelTxtBox
            // 
            this.landingFuelTxtBox.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.landingFuelTxtBox.Location = new System.Drawing.Point(158, 43);
            this.landingFuelTxtBox.Name = "landingFuelTxtBox";
            this.landingFuelTxtBox.Size = new System.Drawing.Size(86, 27);
            this.landingFuelTxtBox.TabIndex = 1;
            this.landingFuelTxtBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // wtUnitLbl
            // 
            this.wtUnitLbl.AutoSize = true;
            this.wtUnitLbl.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.wtUnitLbl.Location = new System.Drawing.Point(249, 46);
            this.wtUnitLbl.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.wtUnitLbl.Name = "wtUnitLbl";
            this.wtUnitLbl.Size = new System.Drawing.Size(28, 20);
            this.wtUnitLbl.TabIndex = 52;
            this.wtUnitLbl.Text = "KG";
            // 
            // CustomFuelForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(120F, 120F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.Bisque;
            this.ClientSize = new System.Drawing.Size(300, 120);
            this.Controls.Add(this.Label95);
            this.Controls.Add(this.cancelBtn);
            this.Controls.Add(this.importFuelBtn);
            this.Controls.Add(this.okBtn);
            this.Controls.Add(this.landingFuelTxtBox);
            this.Controls.Add(this.wtUnitLbl);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "CustomFuelForm";
            this.Text = "CustomFuelForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        internal System.Windows.Forms.Label Label95;
        internal System.Windows.Forms.Button cancelBtn;
        internal System.Windows.Forms.Button importFuelBtn;
        internal System.Windows.Forms.Button okBtn;
        internal System.Windows.Forms.TextBox landingFuelTxtBox;
        internal System.Windows.Forms.Label wtUnitLbl;
    }
}