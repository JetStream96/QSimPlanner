namespace QSP.UI.Views.MiscInfo
{
    partial class DescentForcastControl
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
            this.tableLayoutPanel7 = new System.Windows.Forms.TableLayoutPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.destIcaoLbl = new System.Windows.Forms.Label();
            this.updateDesForcastBtn = new System.Windows.Forms.Button();
            this.lastUpdatedLbl = new System.Windows.Forms.Label();
            this.desForcastRichTxtBox = new System.Windows.Forms.RichTextBox();
            this.tableLayoutPanel7.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel7
            // 
            this.tableLayoutPanel7.ColumnCount = 1;
            this.tableLayoutPanel7.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel7.Controls.Add(this.panel1, 0, 0);
            this.tableLayoutPanel7.Controls.Add(this.desForcastRichTxtBox, 0, 1);
            this.tableLayoutPanel7.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel7.Margin = new System.Windows.Forms.Padding(2);
            this.tableLayoutPanel7.Name = "tableLayoutPanel7";
            this.tableLayoutPanel7.RowCount = 2;
            this.tableLayoutPanel7.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel7.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel7.Size = new System.Drawing.Size(513, 321);
            this.tableLayoutPanel7.TabIndex = 3;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.destIcaoLbl);
            this.panel1.Controls.Add(this.updateDesForcastBtn);
            this.panel1.Controls.Add(this.lastUpdatedLbl);
            this.panel1.Location = new System.Drawing.Point(2, 2);
            this.panel1.Margin = new System.Windows.Forms.Padding(2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(508, 42);
            this.panel1.TabIndex = 1;
            // 
            // destIcaoLbl
            // 
            this.destIcaoLbl.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.destIcaoLbl.AutoSize = true;
            this.destIcaoLbl.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.destIcaoLbl.Location = new System.Drawing.Point(2, 10);
            this.destIcaoLbl.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.destIcaoLbl.Name = "destIcaoLbl";
            this.destIcaoLbl.Size = new System.Drawing.Size(122, 19);
            this.destIcaoLbl.TabIndex = 0;
            this.destIcaoLbl.Text = "Destination : RCTP";
            // 
            // updateDesForcastBtn
            // 
            this.updateDesForcastBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.updateDesForcastBtn.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.updateDesForcastBtn.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.updateDesForcastBtn.Location = new System.Drawing.Point(423, 2);
            this.updateDesForcastBtn.Margin = new System.Windows.Forms.Padding(2);
            this.updateDesForcastBtn.Name = "updateDesForcastBtn";
            this.updateDesForcastBtn.Size = new System.Drawing.Size(83, 33);
            this.updateDesForcastBtn.TabIndex = 80;
            this.updateDesForcastBtn.Text = "Refresh";
            this.updateDesForcastBtn.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.updateDesForcastBtn.UseVisualStyleBackColor = true;
            // 
            // lastUpdatedLbl
            // 
            this.lastUpdatedLbl.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lastUpdatedLbl.AutoSize = true;
            this.lastUpdatedLbl.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lastUpdatedLbl.Location = new System.Drawing.Point(174, 10);
            this.lastUpdatedLbl.Margin = new System.Windows.Forms.Padding(2);
            this.lastUpdatedLbl.Name = "lastUpdatedLbl";
            this.lastUpdatedLbl.Size = new System.Drawing.Size(243, 19);
            this.lastUpdatedLbl.TabIndex = 81;
            this.lastUpdatedLbl.Text = "Last Updated : 7/18/2016 6:55:48 PM";
            // 
            // desForcastRichTxtBox
            // 
            this.desForcastRichTxtBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.desForcastRichTxtBox.Font = new System.Drawing.Font("Courier New", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.desForcastRichTxtBox.Location = new System.Drawing.Point(2, 48);
            this.desForcastRichTxtBox.Margin = new System.Windows.Forms.Padding(2);
            this.desForcastRichTxtBox.Name = "desForcastRichTxtBox";
            this.desForcastRichTxtBox.Size = new System.Drawing.Size(509, 271);
            this.desForcastRichTxtBox.TabIndex = 1;
            this.desForcastRichTxtBox.Text = "";
            // 
            // DescentForcastControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.Controls.Add(this.tableLayoutPanel7);
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Name = "DescentForcastControl";
            this.Size = new System.Drawing.Size(515, 323);
            this.Load += new System.EventHandler(this.DescentForcastControl_Load);
            this.tableLayoutPanel7.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel7;
        private System.Windows.Forms.Panel panel1;
        internal System.Windows.Forms.Label destIcaoLbl;
        internal System.Windows.Forms.Button updateDesForcastBtn;
        private System.Windows.Forms.Label lastUpdatedLbl;
        internal System.Windows.Forms.RichTextBox desForcastRichTxtBox;
    }
}
