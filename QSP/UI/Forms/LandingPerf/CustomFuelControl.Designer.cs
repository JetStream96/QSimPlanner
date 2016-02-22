namespace QSP.UI.Forms.LandingPerf
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
            this.Cancel_Btn = new System.Windows.Forms.Button();
            this.OK_Btn = new System.Windows.Forms.Button();
            this.WtUnit_Req_lbl = new System.Windows.Forms.Label();
            this.ActualLDGFuel = new System.Windows.Forms.TextBox();
            this.Predict_Btn = new System.Windows.Forms.Button();
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
            // Cancel_Btn
            // 
            this.Cancel_Btn.BackColor = System.Drawing.SystemColors.Control;
            this.Cancel_Btn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Cancel_Btn.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Cancel_Btn.Location = new System.Drawing.Point(154, 83);
            this.Cancel_Btn.Name = "Cancel_Btn";
            this.Cancel_Btn.Size = new System.Drawing.Size(72, 32);
            this.Cancel_Btn.TabIndex = 48;
            this.Cancel_Btn.Text = "Cancel";
            this.Cancel_Btn.UseVisualStyleBackColor = false;
            // 
            // OK_Btn
            // 
            this.OK_Btn.BackColor = System.Drawing.SystemColors.Control;
            this.OK_Btn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.OK_Btn.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.OK_Btn.Location = new System.Drawing.Point(75, 83);
            this.OK_Btn.Name = "OK_Btn";
            this.OK_Btn.Size = new System.Drawing.Size(72, 32);
            this.OK_Btn.TabIndex = 47;
            this.OK_Btn.Text = "OK";
            this.OK_Btn.UseVisualStyleBackColor = false;
            // 
            // WtUnit_Req_lbl
            // 
            this.WtUnit_Req_lbl.AutoSize = true;
            this.WtUnit_Req_lbl.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.WtUnit_Req_lbl.Location = new System.Drawing.Point(244, 46);
            this.WtUnit_Req_lbl.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.WtUnit_Req_lbl.Name = "WtUnit_Req_lbl";
            this.WtUnit_Req_lbl.Size = new System.Drawing.Size(28, 20);
            this.WtUnit_Req_lbl.TabIndex = 46;
            this.WtUnit_Req_lbl.Text = "KG";
            // 
            // ActualLDGFuel
            // 
            this.ActualLDGFuel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ActualLDGFuel.Location = new System.Drawing.Point(153, 43);
            this.ActualLDGFuel.Name = "ActualLDGFuel";
            this.ActualLDGFuel.Size = new System.Drawing.Size(86, 27);
            this.ActualLDGFuel.TabIndex = 41;
            this.ActualLDGFuel.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // Predict_Btn
            // 
            this.Predict_Btn.BackColor = System.Drawing.SystemColors.Control;
            this.Predict_Btn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Predict_Btn.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Predict_Btn.Location = new System.Drawing.Point(3, 3);
            this.Predict_Btn.Name = "Predict_Btn";
            this.Predict_Btn.Size = new System.Drawing.Size(145, 32);
            this.Predict_Btn.TabIndex = 39;
            this.Predict_Btn.Text = "Use predicted fuel";
            this.Predict_Btn.UseVisualStyleBackColor = false;
            // 
            // LandingFuelControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Bisque;
            this.Controls.Add(this.Label95);
            this.Controls.Add(this.Cancel_Btn);
            this.Controls.Add(this.Predict_Btn);
            this.Controls.Add(this.OK_Btn);
            this.Controls.Add(this.ActualLDGFuel);
            this.Controls.Add(this.WtUnit_Req_lbl);
            this.Name = "LandingFuelControl";
            this.Size = new System.Drawing.Size(300, 120);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        internal System.Windows.Forms.Label Label95;
        internal System.Windows.Forms.Button Cancel_Btn;
        internal System.Windows.Forms.Button OK_Btn;
        internal System.Windows.Forms.Label WtUnit_Req_lbl;
        internal System.Windows.Forms.TextBox ActualLDGFuel;
        internal System.Windows.Forms.Button Predict_Btn;
    }
}
