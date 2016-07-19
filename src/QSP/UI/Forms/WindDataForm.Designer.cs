namespace QSP.UI.Forms
{
    partial class WindDataForm
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
            this.statusLbl = new System.Windows.Forms.Label();
            this.downlaodBtn = new System.Windows.Forms.Button();
            this.saveFileBtn = new System.Windows.Forms.Button();
            this.loadFileBtn = new System.Windows.Forms.Button();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // statusLbl
            // 
            this.statusLbl.AutoSize = true;
            this.statusLbl.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.statusLbl.Location = new System.Drawing.Point(3, 0);
            this.statusLbl.Name = "statusLbl";
            this.statusLbl.Size = new System.Drawing.Size(71, 23);
            this.statusLbl.TabIndex = 0;
            this.statusLbl.Text = "Status : ";
            // 
            // downlaodBtn
            // 
            this.downlaodBtn.BackColor = System.Drawing.Color.DarkSlateGray;
            this.downlaodBtn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.downlaodBtn.Cursor = System.Windows.Forms.Cursors.Default;
            this.downlaodBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.downlaodBtn.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.downlaodBtn.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.downlaodBtn.Location = new System.Drawing.Point(0, 35);
            this.downlaodBtn.Margin = new System.Windows.Forms.Padding(0);
            this.downlaodBtn.Name = "downlaodBtn";
            this.downlaodBtn.Size = new System.Drawing.Size(200, 32);
            this.downlaodBtn.TabIndex = 26;
            this.downlaodBtn.Text = "Download / Refresh";
            this.downlaodBtn.UseVisualStyleBackColor = false;
            // 
            // saveFileBtn
            // 
            this.saveFileBtn.BackColor = System.Drawing.Color.DarkSlateGray;
            this.saveFileBtn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.saveFileBtn.Cursor = System.Windows.Forms.Cursors.Default;
            this.saveFileBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.saveFileBtn.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.saveFileBtn.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.saveFileBtn.Location = new System.Drawing.Point(0, 70);
            this.saveFileBtn.Margin = new System.Windows.Forms.Padding(0);
            this.saveFileBtn.Name = "saveFileBtn";
            this.saveFileBtn.Size = new System.Drawing.Size(200, 32);
            this.saveFileBtn.TabIndex = 27;
            this.saveFileBtn.Text = "Save wind data";
            this.saveFileBtn.UseVisualStyleBackColor = false;
            // 
            // loadFileBtn
            // 
            this.loadFileBtn.BackColor = System.Drawing.Color.DarkSlateGray;
            this.loadFileBtn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.loadFileBtn.Cursor = System.Windows.Forms.Cursors.Default;
            this.loadFileBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.loadFileBtn.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.loadFileBtn.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.loadFileBtn.Location = new System.Drawing.Point(0, 105);
            this.loadFileBtn.Margin = new System.Windows.Forms.Padding(0);
            this.loadFileBtn.Name = "loadFileBtn";
            this.loadFileBtn.Size = new System.Drawing.Size(200, 32);
            this.loadFileBtn.TabIndex = 28;
            this.loadFileBtn.Text = "Load wind data";
            this.loadFileBtn.UseVisualStyleBackColor = false;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.statusLbl, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.loadFileBtn, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.downlaodBtn, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.saveFileBtn, 0, 2);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(50, 33);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 4;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(408, 145);
            this.tableLayoutPanel1.TabIndex = 29;
            // 
            // WindDataForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(120F, 120F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.ClientSize = new System.Drawing.Size(620, 212);
            this.Controls.Add(this.tableLayoutPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "WindDataForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "WindDataForm";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label statusLbl;
        private System.Windows.Forms.Button downlaodBtn;
        private System.Windows.Forms.Button saveFileBtn;
        private System.Windows.Forms.Button loadFileBtn;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
    }
}