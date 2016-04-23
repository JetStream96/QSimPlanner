namespace QSP.UI.ToLdgModule.Forms
{
    partial class QspLiteForm
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
            this.acConfigBtn = new System.Windows.Forms.Button();
            this.toBtn = new System.Windows.Forms.Button();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.airportBtn = new System.Windows.Forms.Button();
            this.ldgBtn = new System.Windows.Forms.Button();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // acConfigBtn
            // 
            this.acConfigBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.acConfigBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.acConfigBtn.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.acConfigBtn.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.acConfigBtn.Location = new System.Drawing.Point(1, 1);
            this.acConfigBtn.Margin = new System.Windows.Forms.Padding(0);
            this.acConfigBtn.Name = "acConfigBtn";
            this.acConfigBtn.Size = new System.Drawing.Size(150, 38);
            this.acConfigBtn.TabIndex = 0;
            this.acConfigBtn.Text = "Aircraft Config";
            this.acConfigBtn.UseVisualStyleBackColor = false;
            // 
            // toBtn
            // 
            this.toBtn.BackColor = System.Drawing.Color.DarkOrange;
            this.toBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.toBtn.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.toBtn.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.toBtn.Location = new System.Drawing.Point(152, 1);
            this.toBtn.Margin = new System.Windows.Forms.Padding(0);
            this.toBtn.Name = "toBtn";
            this.toBtn.Size = new System.Drawing.Size(150, 38);
            this.toBtn.TabIndex = 1;
            this.toBtn.Text = "Take off";
            this.toBtn.UseVisualStyleBackColor = false;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            this.tableLayoutPanel1.ColumnCount = 4;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 150F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 150F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 150F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 150F));
            this.tableLayoutPanel1.Controls.Add(this.airportBtn, 3, 0);
            this.tableLayoutPanel1.Controls.Add(this.ldgBtn, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.acConfigBtn, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.toBtn, 1, 0);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(12, 12);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(605, 41);
            this.tableLayoutPanel1.TabIndex = 2;
            // 
            // airportBtn
            // 
            this.airportBtn.BackColor = System.Drawing.Color.DodgerBlue;
            this.airportBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.airportBtn.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.airportBtn.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.airportBtn.Location = new System.Drawing.Point(454, 1);
            this.airportBtn.Margin = new System.Windows.Forms.Padding(0);
            this.airportBtn.Name = "airportBtn";
            this.airportBtn.Size = new System.Drawing.Size(150, 38);
            this.airportBtn.TabIndex = 3;
            this.airportBtn.Text = "Airports";
            this.airportBtn.UseVisualStyleBackColor = false;
            // 
            // ldgBtn
            // 
            this.ldgBtn.BackColor = System.Drawing.Color.ForestGreen;
            this.ldgBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ldgBtn.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ldgBtn.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.ldgBtn.Location = new System.Drawing.Point(303, 1);
            this.ldgBtn.Margin = new System.Windows.Forms.Padding(0);
            this.ldgBtn.Name = "ldgBtn";
            this.ldgBtn.Size = new System.Drawing.Size(150, 38);
            this.ldgBtn.TabIndex = 4;
            this.ldgBtn.Text = "Landing";
            this.ldgBtn.UseVisualStyleBackColor = false;
            // 
            // QspLiteForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(120F, 120F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(1160, 713);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "QspLiteForm";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "QSP Lite";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button acConfigBtn;
        private System.Windows.Forms.Button toBtn;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button airportBtn;
        private System.Windows.Forms.Button ldgBtn;
    }
}