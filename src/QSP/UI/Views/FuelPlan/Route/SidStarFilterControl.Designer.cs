namespace QSP.UI.Views.FuelPlan.Route
{
    partial class SidStarFilterControl
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
            this.procListView = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.showSelectedCheckBox = new System.Windows.Forms.CheckBox();
            this.okBtn = new System.Windows.Forms.Button();
            this.cancelBtn = new System.Windows.Forms.Button();
            this.listTypeComboBox = new QSP.UI.Views.ComboBoxWithBorder();
            this.SuspendLayout();
            // 
            // procListView
            // 
            this.procListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1});
            this.procListView.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.procListView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.procListView.Location = new System.Drawing.Point(6, 44);
            this.procListView.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.procListView.Name = "procListView";
            this.procListView.Size = new System.Drawing.Size(168, 226);
            this.procListView.TabIndex = 1;
            this.procListView.UseCompatibleStateImageBehavior = false;
            this.procListView.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Width = 205;
            // 
            // showSelectedCheckBox
            // 
            this.showSelectedCheckBox.AutoSize = true;
            this.showSelectedCheckBox.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.showSelectedCheckBox.Location = new System.Drawing.Point(77, 274);
            this.showSelectedCheckBox.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.showSelectedCheckBox.Name = "showSelectedCheckBox";
            this.showSelectedCheckBox.Size = new System.Drawing.Size(96, 19);
            this.showSelectedCheckBox.TabIndex = 2;
            this.showSelectedCheckBox.Text = "Show Seleted";
            this.showSelectedCheckBox.UseVisualStyleBackColor = true;
            // 
            // okBtn
            // 
            this.okBtn.BackColor = System.Drawing.Color.Green;
            this.okBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.okBtn.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.okBtn.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.okBtn.Location = new System.Drawing.Point(18, 307);
            this.okBtn.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.okBtn.Name = "okBtn";
            this.okBtn.Size = new System.Drawing.Size(60, 24);
            this.okBtn.TabIndex = 3;
            this.okBtn.Text = "OK";
            this.okBtn.UseVisualStyleBackColor = false;
            // 
            // cancelBtn
            // 
            this.cancelBtn.BackColor = System.Drawing.SystemColors.ControlLight;
            this.cancelBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cancelBtn.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cancelBtn.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.cancelBtn.Location = new System.Drawing.Point(92, 307);
            this.cancelBtn.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.cancelBtn.Name = "cancelBtn";
            this.cancelBtn.Size = new System.Drawing.Size(60, 24);
            this.cancelBtn.TabIndex = 4;
            this.cancelBtn.Text = "Cancel";
            this.cancelBtn.UseVisualStyleBackColor = false;
            // 
            // listTypeComboBox
            // 
            this.listTypeComboBox.BorderColor = System.Drawing.Color.DimGray;
            this.listTypeComboBox.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.listTypeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.listTypeComboBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.listTypeComboBox.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.listTypeComboBox.FormattingEnabled = true;
            this.listTypeComboBox.Location = new System.Drawing.Point(6, 6);
            this.listTypeComboBox.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.listTypeComboBox.Name = "listTypeComboBox";
            this.listTypeComboBox.Size = new System.Drawing.Size(168, 27);
            this.listTypeComboBox.TabIndex = 0;
            // 
            // SidStarFilterControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.cancelBtn);
            this.Controls.Add(this.okBtn);
            this.Controls.Add(this.showSelectedCheckBox);
            this.Controls.Add(this.procListView);
            this.Controls.Add(this.listTypeComboBox);
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Name = "SidStarFilterControl";
            this.Size = new System.Drawing.Size(180, 343);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ComboBoxWithBorder listTypeComboBox;
        private System.Windows.Forms.ListView procListView;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.CheckBox showSelectedCheckBox;
        private System.Windows.Forms.Button okBtn;
        private System.Windows.Forms.Button cancelBtn;
    }
}
