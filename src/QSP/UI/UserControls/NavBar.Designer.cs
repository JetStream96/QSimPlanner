namespace QSP.UI.UserControls
{
    partial class NavBar
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
            this.acLbl = new System.Windows.Forms.Label();
            this.tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.manualLbl = new System.Windows.Forms.Label();
            this.fuelLbl = new System.Windows.Forms.Label();
            this.tolbl = new System.Windows.Forms.Label();
            this.ldgLbl = new System.Windows.Forms.Label();
            this.miscLbl = new System.Windows.Forms.Label();
            this.OptionLbl = new System.Windows.Forms.Label();
            this.aboutLbl = new System.Windows.Forms.Label();
            this.tableLayoutPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // acLbl
            // 
            this.acLbl.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.acLbl.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.acLbl.Location = new System.Drawing.Point(0, 0);
            this.acLbl.Margin = new System.Windows.Forms.Padding(0);
            this.acLbl.Name = "acLbl";
            this.acLbl.Size = new System.Drawing.Size(120, 38);
            this.acLbl.TabIndex = 0;
            this.acLbl.Text = "Aircraft";
            this.acLbl.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tableLayoutPanel
            // 
            this.tableLayoutPanel.BackColor = System.Drawing.Color.White;
            this.tableLayoutPanel.ColumnCount = 1;
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 120F));
            this.tableLayoutPanel.Controls.Add(this.manualLbl, 0, 6);
            this.tableLayoutPanel.Controls.Add(this.acLbl, 0, 0);
            this.tableLayoutPanel.Controls.Add(this.fuelLbl, 0, 1);
            this.tableLayoutPanel.Controls.Add(this.tolbl, 0, 2);
            this.tableLayoutPanel.Controls.Add(this.ldgLbl, 0, 3);
            this.tableLayoutPanel.Controls.Add(this.miscLbl, 0, 4);
            this.tableLayoutPanel.Controls.Add(this.OptionLbl, 0, 5);
            this.tableLayoutPanel.Controls.Add(this.aboutLbl, 0, 7);
            this.tableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel.Name = "tableLayoutPanel";
            this.tableLayoutPanel.RowCount = 8;
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 38F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 38F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 38F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 38F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 38F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 38F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 38F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 38F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel.Size = new System.Drawing.Size(120, 304);
            this.tableLayoutPanel.TabIndex = 9;
            // 
            // manualLbl
            // 
            this.manualLbl.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.manualLbl.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.manualLbl.Location = new System.Drawing.Point(0, 228);
            this.manualLbl.Margin = new System.Windows.Forms.Padding(0);
            this.manualLbl.Name = "manualLbl";
            this.manualLbl.Size = new System.Drawing.Size(120, 38);
            this.manualLbl.TabIndex = 6;
            this.manualLbl.Text = "Manual";
            this.manualLbl.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // fuelLbl
            // 
            this.fuelLbl.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(174)))), ((int)(((byte)(219)))));
            this.fuelLbl.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.fuelLbl.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.fuelLbl.Location = new System.Drawing.Point(0, 38);
            this.fuelLbl.Margin = new System.Windows.Forms.Padding(0);
            this.fuelLbl.Name = "fuelLbl";
            this.fuelLbl.Size = new System.Drawing.Size(120, 38);
            this.fuelLbl.TabIndex = 1;
            this.fuelLbl.Text = "Fuel";
            this.fuelLbl.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tolbl
            // 
            this.tolbl.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.tolbl.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tolbl.Location = new System.Drawing.Point(0, 76);
            this.tolbl.Margin = new System.Windows.Forms.Padding(0);
            this.tolbl.Name = "tolbl";
            this.tolbl.Size = new System.Drawing.Size(120, 38);
            this.tolbl.TabIndex = 2;
            this.tolbl.Text = "Takeoff";
            this.tolbl.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ldgLbl
            // 
            this.ldgLbl.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.ldgLbl.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ldgLbl.Location = new System.Drawing.Point(0, 114);
            this.ldgLbl.Margin = new System.Windows.Forms.Padding(0);
            this.ldgLbl.Name = "ldgLbl";
            this.ldgLbl.Size = new System.Drawing.Size(120, 38);
            this.ldgLbl.TabIndex = 3;
            this.ldgLbl.Text = "Landing";
            this.ldgLbl.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // miscLbl
            // 
            this.miscLbl.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.miscLbl.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.miscLbl.Location = new System.Drawing.Point(0, 152);
            this.miscLbl.Margin = new System.Windows.Forms.Padding(0);
            this.miscLbl.Name = "miscLbl";
            this.miscLbl.Size = new System.Drawing.Size(120, 38);
            this.miscLbl.TabIndex = 4;
            this.miscLbl.Text = "Misc";
            this.miscLbl.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // OptionLbl
            // 
            this.OptionLbl.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.OptionLbl.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.OptionLbl.Location = new System.Drawing.Point(0, 190);
            this.OptionLbl.Margin = new System.Windows.Forms.Padding(0);
            this.OptionLbl.Name = "OptionLbl";
            this.OptionLbl.Size = new System.Drawing.Size(120, 38);
            this.OptionLbl.TabIndex = 5;
            this.OptionLbl.Text = "Options";
            this.OptionLbl.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // aboutLbl
            // 
            this.aboutLbl.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.aboutLbl.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.aboutLbl.Location = new System.Drawing.Point(0, 266);
            this.aboutLbl.Margin = new System.Windows.Forms.Padding(0);
            this.aboutLbl.Name = "aboutLbl";
            this.aboutLbl.Size = new System.Drawing.Size(120, 38);
            this.aboutLbl.TabIndex = 7;
            this.aboutLbl.Text = "About";
            this.aboutLbl.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // NavBar
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(120F, 120F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.tableLayoutPanel);
            this.DoubleBuffered = true;
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "NavBar";
            this.Size = new System.Drawing.Size(120, 304);
            this.tableLayoutPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label acLbl;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
        private System.Windows.Forms.Label aboutLbl;
        private System.Windows.Forms.Label miscLbl;
        private System.Windows.Forms.Label ldgLbl;
        private System.Windows.Forms.Label tolbl;
        private System.Windows.Forms.Label fuelLbl;
        public System.Windows.Forms.Label OptionLbl;
        private System.Windows.Forms.Label manualLbl;
    }
}
