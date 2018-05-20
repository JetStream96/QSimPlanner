namespace QSP.UI.UserControls
{
    partial class WindControl
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.statusPicBox = new System.Windows.Forms.PictureBox();
            this.statusLbl = new System.Windows.Forms.Label();
            this.downloadBtn = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.loadFileBtn = new System.Windows.Forms.Button();
            this.saveFileBtn = new System.Windows.Forms.Button();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.statusPicBox)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.AutoSize = true;
            this.tableLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.downloadBtn, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.loadFileBtn, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.saveFileBtn, 0, 4);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 5;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(458, 176);
            this.tableLayoutPanel1.TabIndex = 30;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.AutoSize = true;
            this.tableLayoutPanel2.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Controls.Add(this.statusPicBox, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.statusLbl, 1, 0);
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(96, 23);
            this.tableLayoutPanel2.TabIndex = 31;
            // 
            // statusPicBox
            // 
            this.statusPicBox.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.statusPicBox.BackgroundImage = global::QSP.Properties.Resources.GreenLight;
            this.statusPicBox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.statusPicBox.Location = new System.Drawing.Point(0, 1);
            this.statusPicBox.Margin = new System.Windows.Forms.Padding(0);
            this.statusPicBox.Name = "statusPicBox";
            this.statusPicBox.Size = new System.Drawing.Size(20, 20);
            this.statusPicBox.TabIndex = 30;
            this.statusPicBox.TabStop = false;
            // 
            // statusLbl
            // 
            this.statusLbl.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.statusLbl.AutoSize = true;
            this.statusLbl.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.statusLbl.Location = new System.Drawing.Point(23, 0);
            this.statusLbl.Name = "statusLbl";
            this.statusLbl.Size = new System.Drawing.Size(70, 23);
            this.statusLbl.TabIndex = 0;
            this.statusLbl.Text = "Status : ";
            // 
            // downloadBtn
            // 
            this.downloadBtn.AutoSize = true;
            this.downloadBtn.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.downloadBtn.BackColor = System.Drawing.Color.DarkSlateGray;
            this.downloadBtn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.downloadBtn.Cursor = System.Windows.Forms.Cursors.Default;
            this.downloadBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.downloadBtn.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.downloadBtn.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.downloadBtn.Location = new System.Drawing.Point(5, 46);
            this.downloadBtn.Margin = new System.Windows.Forms.Padding(5);
            this.downloadBtn.MinimumSize = new System.Drawing.Size(200, 0);
            this.downloadBtn.Name = "downloadBtn";
            this.downloadBtn.Size = new System.Drawing.Size(200, 35);
            this.downloadBtn.TabIndex = 0;
            this.downloadBtn.Text = "Download / Refresh";
            this.downloadBtn.UseVisualStyleBackColor = false;
            // 
            // label1
            // 
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label1.Location = new System.Drawing.Point(3, 34);
            this.label1.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(452, 2);
            this.label1.TabIndex = 32;
            // 
            // loadFileBtn
            // 
            this.loadFileBtn.AutoSize = true;
            this.loadFileBtn.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.loadFileBtn.BackColor = System.Drawing.Color.DarkSlateGray;
            this.loadFileBtn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.loadFileBtn.Cursor = System.Windows.Forms.Cursors.Default;
            this.loadFileBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.loadFileBtn.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.loadFileBtn.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.loadFileBtn.Location = new System.Drawing.Point(5, 91);
            this.loadFileBtn.Margin = new System.Windows.Forms.Padding(5);
            this.loadFileBtn.MinimumSize = new System.Drawing.Size(200, 0);
            this.loadFileBtn.Name = "loadFileBtn";
            this.loadFileBtn.Size = new System.Drawing.Size(200, 35);
            this.loadFileBtn.TabIndex = 1;
            this.loadFileBtn.Text = "Load wind data";
            this.loadFileBtn.UseVisualStyleBackColor = false;
            // 
            // saveFileBtn
            // 
            this.saveFileBtn.AutoSize = true;
            this.saveFileBtn.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.saveFileBtn.BackColor = System.Drawing.Color.DarkSlateGray;
            this.saveFileBtn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.saveFileBtn.Cursor = System.Windows.Forms.Cursors.Default;
            this.saveFileBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.saveFileBtn.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.saveFileBtn.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.saveFileBtn.Location = new System.Drawing.Point(5, 136);
            this.saveFileBtn.Margin = new System.Windows.Forms.Padding(5);
            this.saveFileBtn.MinimumSize = new System.Drawing.Size(200, 0);
            this.saveFileBtn.Name = "saveFileBtn";
            this.saveFileBtn.Size = new System.Drawing.Size(200, 35);
            this.saveFileBtn.TabIndex = 2;
            this.saveFileBtn.Text = "Save wind data";
            this.saveFileBtn.UseVisualStyleBackColor = false;
            // 
            // WindControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(120F, 120F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "WindControl";
            this.Size = new System.Drawing.Size(461, 179);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.statusPicBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.PictureBox statusPicBox;
        private System.Windows.Forms.Label statusLbl;
        private System.Windows.Forms.Button downloadBtn;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button loadFileBtn;
        private System.Windows.Forms.Button saveFileBtn;
    }
}
