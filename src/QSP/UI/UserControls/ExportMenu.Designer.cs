namespace QSP.UI.UserControls
{
    partial class ExportMenu
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
            this.exportBtn = new System.Windows.Forms.Button();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.changePathBtn = new System.Windows.Forms.Button();
            this.formatTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.exportMenuRow1 = new QSP.UI.UserControls.ExportMenuRow();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.tableLayoutPanel1.SuspendLayout();
            this.formatTableLayoutPanel.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // exportBtn
            // 
            this.exportBtn.AutoSize = true;
            this.exportBtn.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.exportBtn.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.exportBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.exportBtn.Font = new System.Drawing.Font("Segoe UI", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.exportBtn.ForeColor = System.Drawing.SystemColors.ControlText;
            this.exportBtn.Location = new System.Drawing.Point(4, 4);
            this.exportBtn.Margin = new System.Windows.Forms.Padding(4);
            this.exportBtn.Name = "exportBtn";
            this.exportBtn.Size = new System.Drawing.Size(221, 37);
            this.exportBtn.TabIndex = 6;
            this.exportBtn.Text = "Export selected formats";
            this.exportBtn.UseVisualStyleBackColor = false;
            this.exportBtn.Click += new System.EventHandler(this.ExportFiles);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.AutoSize = true;
            this.tableLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.changePathBtn, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.exportBtn, 0, 0);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 43);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 45F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(461, 45);
            this.tableLayoutPanel1.TabIndex = 7;
            // 
            // changePathBtn
            // 
            this.changePathBtn.AutoSize = true;
            this.changePathBtn.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.changePathBtn.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.changePathBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.changePathBtn.Font = new System.Drawing.Font("Segoe UI", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.changePathBtn.ForeColor = System.Drawing.SystemColors.ControlText;
            this.changePathBtn.Location = new System.Drawing.Point(233, 4);
            this.changePathBtn.Margin = new System.Windows.Forms.Padding(4);
            this.changePathBtn.Name = "changePathBtn";
            this.changePathBtn.Size = new System.Drawing.Size(224, 37);
            this.changePathBtn.TabIndex = 7;
            this.changePathBtn.Text = "Change simulator paths";
            this.changePathBtn.UseVisualStyleBackColor = false;
            this.changePathBtn.Click += new System.EventHandler(this.ChangeSimPaths);
            // 
            // formatTableLayoutPanel
            // 
            this.formatTableLayoutPanel.AutoSize = true;
            this.formatTableLayoutPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.formatTableLayoutPanel.ColumnCount = 1;
            this.formatTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.formatTableLayoutPanel.Controls.Add(this.exportMenuRow1, 0, 0);
            this.formatTableLayoutPanel.Location = new System.Drawing.Point(3, 94);
            this.formatTableLayoutPanel.Name = "formatTableLayoutPanel";
            this.formatTableLayoutPanel.RowCount = 1;
            this.formatTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.formatTableLayoutPanel.Size = new System.Drawing.Size(772, 48);
            this.formatTableLayoutPanel.TabIndex = 8;
            // 
            // exportMenuRow1
            // 
            this.exportMenuRow1.AutoSize = true;
            this.exportMenuRow1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.exportMenuRow1.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.exportMenuRow1.Location = new System.Drawing.Point(3, 3);
            this.exportMenuRow1.Name = "exportMenuRow1";
            this.exportMenuRow1.Size = new System.Drawing.Size(766, 42);
            this.exportMenuRow1.TabIndex = 0;
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.AutoSize = true;
            this.tableLayoutPanel3.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel3.ColumnCount = 1;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.tableLayoutPanel1, 0, 1);
            this.tableLayoutPanel3.Controls.Add(this.formatTableLayoutPanel, 0, 2);
            this.tableLayoutPanel3.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 3;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(778, 145);
            this.tableLayoutPanel3.TabIndex = 9;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Padding = new System.Windows.Forms.Padding(0, 0, 0, 20);
            this.label1.Size = new System.Drawing.Size(671, 40);
            this.label1.TabIndex = 10;
            this.label1.Text = "Select the flight plan formats you want to export, and then click \'Export selecte" +
    "d formats\'.";
            // 
            // ExportMenu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(120F, 120F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.Controls.Add(this.tableLayoutPanel3);
            this.Name = "ExportMenu";
            this.Size = new System.Drawing.Size(781, 148);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.formatTableLayoutPanel.ResumeLayout(false);
            this.formatTableLayoutPanel.PerformLayout();
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel3.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.Button exportBtn;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        public System.Windows.Forms.Button changePathBtn;
        private System.Windows.Forms.TableLayoutPanel formatTableLayoutPanel;
        private ExportMenuRow exportMenuRow1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.Label label1;
    }
}
