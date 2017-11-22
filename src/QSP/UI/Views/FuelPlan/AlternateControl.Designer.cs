namespace QSP.UI.Views.FuelPlan
{
    partial class AlternateControl
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
            this.layoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.addRemoveAltnBtnLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.addAltnBtn = new System.Windows.Forms.Button();
            this.removeAltnBtn = new System.Windows.Forms.Button();
            this.layoutPanel.SuspendLayout();
            this.addRemoveAltnBtnLayoutPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // layoutPanel
            // 
            this.layoutPanel.AutoSize = true;
            this.layoutPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.layoutPanel.ColumnCount = 1;
            this.layoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 21F));
            this.layoutPanel.Controls.Add(this.addRemoveAltnBtnLayoutPanel, 0, 0);
            this.layoutPanel.Location = new System.Drawing.Point(0, 0);
            this.layoutPanel.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.layoutPanel.Name = "layoutPanel";
            this.layoutPanel.RowCount = 1;
            this.layoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 43F));
            this.layoutPanel.Size = new System.Drawing.Size(153, 39);
            this.layoutPanel.TabIndex = 76;
            // 
            // addRemoveAltnBtnLayoutPanel
            // 
            this.addRemoveAltnBtnLayoutPanel.AutoSize = true;
            this.addRemoveAltnBtnLayoutPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.addRemoveAltnBtnLayoutPanel.ColumnCount = 2;
            this.addRemoveAltnBtnLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.addRemoveAltnBtnLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.addRemoveAltnBtnLayoutPanel.Controls.Add(this.addAltnBtn, 0, 0);
            this.addRemoveAltnBtnLayoutPanel.Controls.Add(this.removeAltnBtn, 1, 0);
            this.addRemoveAltnBtnLayoutPanel.Location = new System.Drawing.Point(3, 2);
            this.addRemoveAltnBtnLayoutPanel.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.addRemoveAltnBtnLayoutPanel.Name = "addRemoveAltnBtnLayoutPanel";
            this.addRemoveAltnBtnLayoutPanel.RowCount = 1;
            this.addRemoveAltnBtnLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.addRemoveAltnBtnLayoutPanel.Size = new System.Drawing.Size(147, 35);
            this.addRemoveAltnBtnLayoutPanel.TabIndex = 76;
            // 
            // addAltnBtn
            // 
            this.addAltnBtn.AutoSize = true;
            this.addAltnBtn.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.addAltnBtn.BackColor = System.Drawing.Color.DarkSlateGray;
            this.addAltnBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.addAltnBtn.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.addAltnBtn.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.addAltnBtn.Location = new System.Drawing.Point(0, 0);
            this.addAltnBtn.Margin = new System.Windows.Forms.Padding(0);
            this.addAltnBtn.Name = "addAltnBtn";
            this.addAltnBtn.Size = new System.Drawing.Size(53, 35);
            this.addAltnBtn.TabIndex = 0;
            this.addAltnBtn.Text = "Add";
            this.addAltnBtn.UseVisualStyleBackColor = false;
            this.addAltnBtn.Click += new System.EventHandler(this.addAltnBtn_Click);
            // 
            // removeAltnBtn
            // 
            this.removeAltnBtn.AutoSize = true;
            this.removeAltnBtn.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.removeAltnBtn.BackColor = System.Drawing.Color.DarkSlateGray;
            this.removeAltnBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.removeAltnBtn.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.removeAltnBtn.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.removeAltnBtn.Location = new System.Drawing.Point(64, 0);
            this.removeAltnBtn.Margin = new System.Windows.Forms.Padding(11, 0, 0, 0);
            this.removeAltnBtn.Name = "removeAltnBtn";
            this.removeAltnBtn.Size = new System.Drawing.Size(83, 35);
            this.removeAltnBtn.TabIndex = 1;
            this.removeAltnBtn.Text = "Remove";
            this.removeAltnBtn.UseVisualStyleBackColor = false;
            this.removeAltnBtn.Click += new System.EventHandler(this.removeAltnBtn_Click);
            // 
            // AlternateControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.Controls.Add(this.layoutPanel);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "AlternateControl";
            this.Size = new System.Drawing.Size(156, 41);
            this.layoutPanel.ResumeLayout(false);
            this.layoutPanel.PerformLayout();
            this.addRemoveAltnBtnLayoutPanel.ResumeLayout(false);
            this.addRemoveAltnBtnLayoutPanel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel layoutPanel;
        private System.Windows.Forms.TableLayoutPanel addRemoveAltnBtnLayoutPanel;
        internal System.Windows.Forms.Button addAltnBtn;
        internal System.Windows.Forms.Button removeAltnBtn;
    }
}
