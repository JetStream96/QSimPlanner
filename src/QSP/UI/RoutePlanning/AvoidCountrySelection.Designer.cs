namespace QSP.UI.RoutePlanning
{
    partial class AvoidCountrySelection
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
            this.listView = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.showSelectedCheckBox = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // listView
            // 
            this.listView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2});
            this.listView.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.listView.Location = new System.Drawing.Point(64, 125);
            this.listView.Name = "listView";
            this.listView.Size = new System.Drawing.Size(370, 254);
            this.listView.TabIndex = 0;
            this.listView.UseCompatibleStateImageBehavior = false;
            this.listView.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Letter Code";
            this.columnHeader1.Width = 120;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Country Name";
            this.columnHeader2.Width = 225;
            // 
            // showSelectedCheckBox
            // 
            this.showSelectedCheckBox.AutoSize = true;
            this.showSelectedCheckBox.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.showSelectedCheckBox.Location = new System.Drawing.Point(306, 95);
            this.showSelectedCheckBox.Name = "showSelectedCheckBox";
            this.showSelectedCheckBox.Size = new System.Drawing.Size(128, 24);
            this.showSelectedCheckBox.TabIndex = 1;
            this.showSelectedCheckBox.Text = "Show Selected";
            this.showSelectedCheckBox.UseVisualStyleBackColor = true;
            // 
            // AvoidCountrySelection
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(120F, 120F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.Controls.Add(this.showSelectedCheckBox);
            this.Controls.Add(this.listView);
            this.Name = "AvoidCountrySelection";
            this.Size = new System.Drawing.Size(646, 484);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView listView;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.CheckBox showSelectedCheckBox;
    }
}
