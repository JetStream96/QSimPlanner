namespace QSP.UI.ToLdgModule.LandingPerf
{
    partial class CustomFuelControl
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
            this.Label95 = new System.Windows.Forms.Label();
            this.cancelBtn = new System.Windows.Forms.Button();
            this.okBtn = new System.Windows.Forms.Button();
            this.wtUnitLbl = new System.Windows.Forms.Label();
            this.landingFuelTxtBox = new System.Windows.Forms.TextBox();
            this.importFuelBtn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // Label95
            // 
            this.Label95.AutoSize = true;
            this.Label95.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label95.Location = new System.Drawing.Point(10, 47);
            this.Label95.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.Label95.Name = "Label95";
            this.Label95.Size = new System.Drawing.Size(126, 20);
            this.Label95.TabIndex = 49;
            this.Label95.Text = "Enter landing fuel";
            // 
            // cancelBtn
            // 
            this.cancelBtn.BackColor = System.Drawing.SystemColors.Control;
            this.cancelBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cancelBtn.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cancelBtn.Location = new System.Drawing.Point(154, 83);
            this.cancelBtn.Name = "cancelBtn";
            this.cancelBtn.Size = new System.Drawing.Size(72, 32);
            this.cancelBtn.TabIndex = 48;
            this.cancelBtn.Text = "Cancel";
            this.cancelBtn.UseVisualStyleBackColor = false;
            // 
            // okBtn
            // 
            this.okBtn.BackColor = System.Drawing.SystemColors.Control;
            this.okBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.okBtn.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.okBtn.Location = new System.Drawing.Point(75, 83);
            this.okBtn.Name = "okBtn";
            this.okBtn.Size = new System.Drawing.Size(72, 32);
            this.okBtn.TabIndex = 47;
            this.okBtn.Text = "OK";
            this.okBtn.UseVisualStyleBackColor = false;
            // 
            // wtUnitLbl
            // 
            this.wtUnitLbl.AutoSize = true;
            this.wtUnitLbl.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.wtUnitLbl.Location = new System.Drawing.Point(244, 46);
            this.wtUnitLbl.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.wtUnitLbl.Name = "wtUnitLbl";
            this.wtUnitLbl.Size = new System.Drawing.Size(28, 20);
            this.wtUnitLbl.TabIndex = 46;
            this.wtUnitLbl.Text = "KG";
            // 
            // landingFuelTxtBox
            // 
            this.landingFuelTxtBox.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.landingFuelTxtBox.Location = new System.Drawing.Point(153, 43);
            this.landingFuelTxtBox.Name = "landingFuelTxtBox";
            this.landingFuelTxtBox.Size = new System.Drawing.Size(86, 27);
            this.landingFuelTxtBox.TabIndex = 41;
            this.landingFuelTxtBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // importFuelBtn
            // 
            this.importFuelBtn.BackColor = System.Drawing.SystemColors.Control;
            this.importFuelBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.importFuelBtn.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.importFuelBtn.Location = new System.Drawing.Point(3, 3);
            this.importFuelBtn.Name = "importFuelBtn";
            this.importFuelBtn.Size = new System.Drawing.Size(145, 32);
            this.importFuelBtn.TabIndex = 39;
            this.importFuelBtn.Text = "Use predicted fuel";
            this.importFuelBtn.UseVisualStyleBackColor = false;
            // 
            // CustomFuelControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Bisque;
            this.Controls.Add(this.Label95);
            this.Controls.Add(this.cancelBtn);
            this.Controls.Add(this.importFuelBtn);
            this.Controls.Add(this.okBtn);
            this.Controls.Add(this.landingFuelTxtBox);
            this.Controls.Add(this.wtUnitLbl);
            this.Name = "CustomFuelControl";
            this.Size = new System.Drawing.Size(300, 120);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        internal System.Windows.Forms.Label Label95;
        internal System.Windows.Forms.Button cancelBtn;
        internal System.Windows.Forms.Button okBtn;
        internal System.Windows.Forms.Label wtUnitLbl;
        internal System.Windows.Forms.TextBox landingFuelTxtBox;
        internal System.Windows.Forms.Button importFuelBtn;
    }
}
