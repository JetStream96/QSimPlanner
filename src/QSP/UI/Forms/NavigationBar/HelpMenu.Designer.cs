namespace QSP.UI.Forms.NavigationBar
{
    partial class HelpMenu
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
            this.ManualLbl = new System.Windows.Forms.Label();
            this.tableLayoutPanel5 = new System.Windows.Forms.TableLayoutPanel();
            this.AboutLbl = new System.Windows.Forms.Label();
            this.tableLayoutPanel5.SuspendLayout();
            this.SuspendLayout();
            // 
            // ManualLbl
            // 
            this.ManualLbl.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.ManualLbl.Font = new System.Drawing.Font("Segoe UI", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ManualLbl.Location = new System.Drawing.Point(0, 0);
            this.ManualLbl.Margin = new System.Windows.Forms.Padding(0);
            this.ManualLbl.Name = "ManualLbl";
            this.ManualLbl.Size = new System.Drawing.Size(120, 38);
            this.ManualLbl.TabIndex = 8;
            this.ManualLbl.Text = "Manual";
            this.ManualLbl.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.ManualLbl.Click += new System.EventHandler(this.ManualLbl_Click);
            // 
            // tableLayoutPanel5
            // 
            this.tableLayoutPanel5.ColumnCount = 1;
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 120F));
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel5.Controls.Add(this.AboutLbl, 0, 1);
            this.tableLayoutPanel5.Controls.Add(this.ManualLbl, 0, 0);
            this.tableLayoutPanel5.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel5.Name = "tableLayoutPanel5";
            this.tableLayoutPanel5.RowCount = 2;
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 38F));
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 38F));
            this.tableLayoutPanel5.Size = new System.Drawing.Size(120, 76);
            this.tableLayoutPanel5.TabIndex = 10;
            // 
            // AboutLbl
            // 
            this.AboutLbl.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.AboutLbl.Font = new System.Drawing.Font("Segoe UI", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.AboutLbl.Location = new System.Drawing.Point(0, 38);
            this.AboutLbl.Margin = new System.Windows.Forms.Padding(0);
            this.AboutLbl.Name = "AboutLbl";
            this.AboutLbl.Size = new System.Drawing.Size(120, 38);
            this.AboutLbl.TabIndex = 20;
            this.AboutLbl.Text = "About";
            this.AboutLbl.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // HelpMenu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.Controls.Add(this.tableLayoutPanel5);
            this.Name = "HelpMenu";
            this.Size = new System.Drawing.Size(123, 79);
            this.Load += new System.EventHandler(this.HelpMenu_Load);
            this.tableLayoutPanel5.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel5;
        public System.Windows.Forms.Label ManualLbl;
        public System.Windows.Forms.Label AboutLbl;
    }
}
