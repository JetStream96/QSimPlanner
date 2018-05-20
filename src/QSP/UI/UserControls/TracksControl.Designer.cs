namespace QSP.UI.UserControls
{
    partial class TracksControl
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
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.ListView1 = new System.Windows.Forms.ListView();
            this.ColumnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ColMessage = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.txtRichTextBox = new System.Windows.Forms.RichTextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.importBtn = new System.Windows.Forms.Button();
            this.downloadAllBtn = new System.Windows.Forms.Button();
            this.saveBtn = new System.Windows.Forms.Button();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.BtnAusotsDn = new System.Windows.Forms.Button();
            this.BtnPacotsDn = new System.Windows.Forms.Button();
            this.BtnNatsDn = new System.Windows.Forms.Button();
            this.viewAusotsBtn = new System.Windows.Forms.Button();
            this.viewPacotsBtn = new System.Windows.Forms.Button();
            this.viewNatsBtn = new System.Windows.Forms.Button();
            this.Label1 = new System.Windows.Forms.Label();
            this.Label2 = new System.Windows.Forms.Label();
            this.Label3 = new System.Windows.Forms.Label();
            this.PicBoxAusots = new System.Windows.Forms.PictureBox();
            this.PicBoxPacots = new System.Windows.Forms.PictureBox();
            this.PicBoxNats = new System.Windows.Forms.PictureBox();
            this.CBoxAusotsEnabled = new QSP.UI.Views.ComboBoxWithBorder();
            this.CBoxPacotsEnabled = new QSP.UI.Views.ComboBoxWithBorder();
            this.CBoxNatsEnabled = new QSP.UI.Views.ComboBoxWithBorder();
            this.tableLayoutPanel2.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PicBoxAusots)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PicBoxPacots)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PicBoxNats)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.AutoSize = true;
            this.tableLayoutPanel2.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel2.ColumnCount = 1;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Controls.Add(this.groupBox2, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.groupBox3, 0, 2);
            this.tableLayoutPanel2.Controls.Add(this.groupBox1, 0, 0);
            this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 3;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(988, 685);
            this.tableLayoutPanel2.TabIndex = 9;
            // 
            // groupBox2
            // 
            this.groupBox2.AutoSize = true;
            this.groupBox2.Controls.Add(this.ListView1);
            this.groupBox2.Location = new System.Drawing.Point(2, 212);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(2);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(2);
            this.groupBox2.Size = new System.Drawing.Size(982, 227);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Messages";
            // 
            // ListView1
            // 
            this.ListView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.ColumnHeader1,
            this.ColMessage});
            this.ListView1.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ListView1.FullRowSelect = true;
            this.ListView1.Location = new System.Drawing.Point(8, 25);
            this.ListView1.Margin = new System.Windows.Forms.Padding(2);
            this.ListView1.Name = "ListView1";
            this.ListView1.Scrollable = false;
            this.ListView1.Size = new System.Drawing.Size(970, 183);
            this.ListView1.TabIndex = 7;
            this.ListView1.UseCompatibleStateImageBehavior = false;
            this.ListView1.View = System.Windows.Forms.View.Details;
            // 
            // ColumnHeader1
            // 
            this.ColumnHeader1.Text = "";
            this.ColumnHeader1.Width = 100;
            // 
            // ColMessage
            // 
            this.ColMessage.Text = "Message";
            this.ColMessage.Width = 609;
            // 
            // groupBox3
            // 
            this.groupBox3.AutoSize = true;
            this.groupBox3.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.groupBox3.Controls.Add(this.txtRichTextBox);
            this.groupBox3.Location = new System.Drawing.Point(2, 443);
            this.groupBox3.Margin = new System.Windows.Forms.Padding(2);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Padding = new System.Windows.Forms.Padding(2);
            this.groupBox3.Size = new System.Drawing.Size(982, 240);
            this.groupBox3.TabIndex = 2;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Text";
            // 
            // txtRichTextBox
            // 
            this.txtRichTextBox.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.txtRichTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtRichTextBox.DetectUrls = false;
            this.txtRichTextBox.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtRichTextBox.HideSelection = false;
            this.txtRichTextBox.Location = new System.Drawing.Point(8, 21);
            this.txtRichTextBox.Margin = new System.Windows.Forms.Padding(2);
            this.txtRichTextBox.MinimumSize = new System.Drawing.Size(970, 200);
            this.txtRichTextBox.Name = "txtRichTextBox";
            this.txtRichTextBox.ReadOnly = true;
            this.txtRichTextBox.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.None;
            this.txtRichTextBox.Size = new System.Drawing.Size(970, 200);
            this.txtRichTextBox.TabIndex = 0;
            this.txtRichTextBox.Text = "";
            this.txtRichTextBox.ContentsResized += new System.Windows.Forms.ContentsResizedEventHandler(this.TxtRichTextBoxContentsResized);
            // 
            // groupBox1
            // 
            this.groupBox1.AutoSize = true;
            this.groupBox1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.groupBox1.Controls.Add(this.tableLayoutPanel1);
            this.groupBox1.Location = new System.Drawing.Point(2, 2);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(2);
            this.groupBox1.MaximumSize = new System.Drawing.Size(984, 1000);
            this.groupBox1.MinimumSize = new System.Drawing.Size(984, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(2);
            this.groupBox1.Size = new System.Drawing.Size(984, 206);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Tracks";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.AutoSize = true;
            this.tableLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel4, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel3, 0, 1);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(9, 24);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel1.MinimumSize = new System.Drawing.Size(500, 160);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(660, 165);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.AutoSize = true;
            this.tableLayoutPanel4.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel4.ColumnCount = 3;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel4.Controls.Add(this.importBtn, 2, 0);
            this.tableLayoutPanel4.Controls.Add(this.downloadAllBtn, 0, 0);
            this.tableLayoutPanel4.Controls.Add(this.saveBtn, 1, 0);
            this.tableLayoutPanel4.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel4.Margin = new System.Windows.Forms.Padding(0, 0, 10, 10);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 1;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel4.Size = new System.Drawing.Size(404, 35);
            this.tableLayoutPanel4.TabIndex = 0;
            // 
            // importBtn
            // 
            this.importBtn.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.importBtn.AutoSize = true;
            this.importBtn.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.importBtn.BackColor = System.Drawing.Color.DarkSlateGray;
            this.importBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.importBtn.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.importBtn.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.importBtn.Location = new System.Drawing.Point(290, 0);
            this.importBtn.Margin = new System.Windows.Forms.Padding(10, 0, 10, 0);
            this.importBtn.Name = "importBtn";
            this.importBtn.Padding = new System.Windows.Forms.Padding(15, 0, 15, 0);
            this.importBtn.Size = new System.Drawing.Size(104, 35);
            this.importBtn.TabIndex = 2;
            this.importBtn.Text = "Import";
            this.importBtn.UseVisualStyleBackColor = false;
            this.importBtn.Click += new System.EventHandler(this.ImportBtnClick);
            // 
            // downloadAllBtn
            // 
            this.downloadAllBtn.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.downloadAllBtn.AutoSize = true;
            this.downloadAllBtn.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.downloadAllBtn.BackColor = System.Drawing.Color.DarkSlateGray;
            this.downloadAllBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.downloadAllBtn.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.downloadAllBtn.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.downloadAllBtn.Location = new System.Drawing.Point(10, 0);
            this.downloadAllBtn.Margin = new System.Windows.Forms.Padding(10, 0, 10, 0);
            this.downloadAllBtn.Name = "downloadAllBtn";
            this.downloadAllBtn.Padding = new System.Windows.Forms.Padding(15, 0, 15, 0);
            this.downloadAllBtn.Size = new System.Drawing.Size(153, 35);
            this.downloadAllBtn.TabIndex = 0;
            this.downloadAllBtn.Text = "Download All";
            this.downloadAllBtn.UseVisualStyleBackColor = false;
            // 
            // saveBtn
            // 
            this.saveBtn.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.saveBtn.AutoSize = true;
            this.saveBtn.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.saveBtn.BackColor = System.Drawing.Color.DarkSlateGray;
            this.saveBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.saveBtn.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.saveBtn.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.saveBtn.Location = new System.Drawing.Point(183, 0);
            this.saveBtn.Margin = new System.Windows.Forms.Padding(10, 0, 10, 0);
            this.saveBtn.Name = "saveBtn";
            this.saveBtn.Padding = new System.Windows.Forms.Padding(15, 0, 15, 0);
            this.saveBtn.Size = new System.Drawing.Size(87, 35);
            this.saveBtn.TabIndex = 1;
            this.saveBtn.Text = "Save";
            this.saveBtn.UseVisualStyleBackColor = false;
            this.saveBtn.Click += new System.EventHandler(this.SaveBtnClick);
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.AutoSize = true;
            this.tableLayoutPanel3.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel3.ColumnCount = 5;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 175F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 175F));
            this.tableLayoutPanel3.Controls.Add(this.BtnAusotsDn, 4, 2);
            this.tableLayoutPanel3.Controls.Add(this.BtnPacotsDn, 4, 1);
            this.tableLayoutPanel3.Controls.Add(this.BtnNatsDn, 4, 0);
            this.tableLayoutPanel3.Controls.Add(this.PicBoxAusots, 0, 2);
            this.tableLayoutPanel3.Controls.Add(this.PicBoxPacots, 0, 1);
            this.tableLayoutPanel3.Controls.Add(this.PicBoxNats, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.viewAusotsBtn, 3, 2);
            this.tableLayoutPanel3.Controls.Add(this.viewPacotsBtn, 3, 1);
            this.tableLayoutPanel3.Controls.Add(this.viewNatsBtn, 3, 0);
            this.tableLayoutPanel3.Controls.Add(this.Label1, 1, 0);
            this.tableLayoutPanel3.Controls.Add(this.Label2, 1, 1);
            this.tableLayoutPanel3.Controls.Add(this.Label3, 1, 2);
            this.tableLayoutPanel3.Controls.Add(this.CBoxAusotsEnabled, 2, 2);
            this.tableLayoutPanel3.Controls.Add(this.CBoxPacotsEnabled, 2, 1);
            this.tableLayoutPanel3.Controls.Add(this.CBoxNatsEnabled, 2, 0);
            this.tableLayoutPanel3.Location = new System.Drawing.Point(0, 45);
            this.tableLayoutPanel3.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 3;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(660, 120);
            this.tableLayoutPanel3.TabIndex = 8;
            // 
            // BtnAusotsDn
            // 
            this.BtnAusotsDn.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.BtnAusotsDn.AutoSize = true;
            this.BtnAusotsDn.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BtnAusotsDn.BackColor = System.Drawing.Color.DarkSlateGray;
            this.BtnAusotsDn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnAusotsDn.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnAusotsDn.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.BtnAusotsDn.Location = new System.Drawing.Point(508, 82);
            this.BtnAusotsDn.Margin = new System.Windows.Forms.Padding(0);
            this.BtnAusotsDn.Name = "BtnAusotsDn";
            this.BtnAusotsDn.Padding = new System.Windows.Forms.Padding(15, 0, 15, 0);
            this.BtnAusotsDn.Size = new System.Drawing.Size(129, 35);
            this.BtnAusotsDn.TabIndex = 8;
            this.BtnAusotsDn.Text = "Download";
            this.BtnAusotsDn.UseVisualStyleBackColor = false;
            // 
            // BtnPacotsDn
            // 
            this.BtnPacotsDn.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.BtnPacotsDn.AutoSize = true;
            this.BtnPacotsDn.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BtnPacotsDn.BackColor = System.Drawing.Color.DarkSlateGray;
            this.BtnPacotsDn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnPacotsDn.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnPacotsDn.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.BtnPacotsDn.Location = new System.Drawing.Point(508, 42);
            this.BtnPacotsDn.Margin = new System.Windows.Forms.Padding(0);
            this.BtnPacotsDn.Name = "BtnPacotsDn";
            this.BtnPacotsDn.Padding = new System.Windows.Forms.Padding(15, 0, 15, 0);
            this.BtnPacotsDn.Size = new System.Drawing.Size(129, 35);
            this.BtnPacotsDn.TabIndex = 5;
            this.BtnPacotsDn.Text = "Download";
            this.BtnPacotsDn.UseVisualStyleBackColor = false;
            // 
            // BtnNatsDn
            // 
            this.BtnNatsDn.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.BtnNatsDn.AutoSize = true;
            this.BtnNatsDn.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BtnNatsDn.BackColor = System.Drawing.Color.DarkSlateGray;
            this.BtnNatsDn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnNatsDn.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnNatsDn.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.BtnNatsDn.Location = new System.Drawing.Point(508, 2);
            this.BtnNatsDn.Margin = new System.Windows.Forms.Padding(0);
            this.BtnNatsDn.Name = "BtnNatsDn";
            this.BtnNatsDn.Padding = new System.Windows.Forms.Padding(15, 0, 15, 0);
            this.BtnNatsDn.Size = new System.Drawing.Size(129, 35);
            this.BtnNatsDn.TabIndex = 2;
            this.BtnNatsDn.Text = "Download";
            this.BtnNatsDn.UseVisualStyleBackColor = false;
            // 
            // viewAusotsBtn
            // 
            this.viewAusotsBtn.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.viewAusotsBtn.AutoSize = true;
            this.viewAusotsBtn.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.viewAusotsBtn.BackColor = System.Drawing.Color.DarkSlateGray;
            this.viewAusotsBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.viewAusotsBtn.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.viewAusotsBtn.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.viewAusotsBtn.Location = new System.Drawing.Point(336, 82);
            this.viewAusotsBtn.Margin = new System.Windows.Forms.Padding(0);
            this.viewAusotsBtn.Name = "viewAusotsBtn";
            this.viewAusotsBtn.Padding = new System.Windows.Forms.Padding(15, 0, 15, 0);
            this.viewAusotsBtn.Size = new System.Drawing.Size(123, 35);
            this.viewAusotsBtn.TabIndex = 7;
            this.viewAusotsBtn.Text = "View Text";
            this.viewAusotsBtn.UseVisualStyleBackColor = false;
            // 
            // viewPacotsBtn
            // 
            this.viewPacotsBtn.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.viewPacotsBtn.AutoSize = true;
            this.viewPacotsBtn.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.viewPacotsBtn.BackColor = System.Drawing.Color.DarkSlateGray;
            this.viewPacotsBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.viewPacotsBtn.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.viewPacotsBtn.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.viewPacotsBtn.Location = new System.Drawing.Point(336, 42);
            this.viewPacotsBtn.Margin = new System.Windows.Forms.Padding(0);
            this.viewPacotsBtn.Name = "viewPacotsBtn";
            this.viewPacotsBtn.Padding = new System.Windows.Forms.Padding(15, 0, 15, 0);
            this.viewPacotsBtn.Size = new System.Drawing.Size(123, 35);
            this.viewPacotsBtn.TabIndex = 4;
            this.viewPacotsBtn.Text = "View Text";
            this.viewPacotsBtn.UseVisualStyleBackColor = false;
            // 
            // viewNatsBtn
            // 
            this.viewNatsBtn.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.viewNatsBtn.AutoSize = true;
            this.viewNatsBtn.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.viewNatsBtn.BackColor = System.Drawing.Color.DarkSlateGray;
            this.viewNatsBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.viewNatsBtn.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.viewNatsBtn.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.viewNatsBtn.Location = new System.Drawing.Point(336, 2);
            this.viewNatsBtn.Margin = new System.Windows.Forms.Padding(0);
            this.viewNatsBtn.Name = "viewNatsBtn";
            this.viewNatsBtn.Padding = new System.Windows.Forms.Padding(15, 0, 15, 0);
            this.viewNatsBtn.Size = new System.Drawing.Size(123, 35);
            this.viewNatsBtn.TabIndex = 1;
            this.viewNatsBtn.Text = "View Text";
            this.viewNatsBtn.UseVisualStyleBackColor = false;
            // 
            // Label1
            // 
            this.Label1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.Label1.AutoSize = true;
            this.Label1.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label1.Location = new System.Drawing.Point(42, 8);
            this.Label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.Label1.Name = "Label1";
            this.Label1.Size = new System.Drawing.Size(51, 23);
            this.Label1.TabIndex = 1;
            this.Label1.Text = "NATS";
            // 
            // Label2
            // 
            this.Label2.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.Label2.AutoSize = true;
            this.Label2.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label2.Location = new System.Drawing.Point(32, 48);
            this.Label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.Label2.Name = "Label2";
            this.Label2.Size = new System.Drawing.Size(71, 23);
            this.Label2.TabIndex = 2;
            this.Label2.Text = "PACOTS";
            // 
            // Label3
            // 
            this.Label3.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.Label3.AutoSize = true;
            this.Label3.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label3.Location = new System.Drawing.Point(32, 88);
            this.Label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.Label3.Name = "Label3";
            this.Label3.Size = new System.Drawing.Size(72, 23);
            this.Label3.TabIndex = 2;
            this.Label3.Text = "AUSOTS";
            // 
            // PicBoxAusots
            // 
            this.PicBoxAusots.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.PicBoxAusots.Location = new System.Drawing.Point(2, 87);
            this.PicBoxAusots.Margin = new System.Windows.Forms.Padding(2);
            this.PicBoxAusots.Name = "PicBoxAusots";
            this.PicBoxAusots.Size = new System.Drawing.Size(26, 26);
            this.PicBoxAusots.TabIndex = 12;
            this.PicBoxAusots.TabStop = false;
            // 
            // PicBoxPacots
            // 
            this.PicBoxPacots.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.PicBoxPacots.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.PicBoxPacots.Location = new System.Drawing.Point(2, 47);
            this.PicBoxPacots.Margin = new System.Windows.Forms.Padding(2);
            this.PicBoxPacots.Name = "PicBoxPacots";
            this.PicBoxPacots.Size = new System.Drawing.Size(26, 26);
            this.PicBoxPacots.TabIndex = 11;
            this.PicBoxPacots.TabStop = false;
            // 
            // PicBoxNats
            // 
            this.PicBoxNats.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.PicBoxNats.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.PicBoxNats.Location = new System.Drawing.Point(2, 7);
            this.PicBoxNats.Margin = new System.Windows.Forms.Padding(2);
            this.PicBoxNats.Name = "PicBoxNats";
            this.PicBoxNats.Size = new System.Drawing.Size(26, 26);
            this.PicBoxNats.TabIndex = 10;
            this.PicBoxNats.TabStop = false;
            // 
            // CBoxAusotsEnabled
            // 
            this.CBoxAusotsEnabled.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.CBoxAusotsEnabled.BorderColor = System.Drawing.Color.DimGray;
            this.CBoxAusotsEnabled.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.CBoxAusotsEnabled.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CBoxAusotsEnabled.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.CBoxAusotsEnabled.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CBoxAusotsEnabled.FormattingEnabled = true;
            this.CBoxAusotsEnabled.Items.AddRange(new object[] {
            "Enabled",
            "Disabled"});
            this.CBoxAusotsEnabled.Location = new System.Drawing.Point(108, 84);
            this.CBoxAusotsEnabled.Margin = new System.Windows.Forms.Padding(2);
            this.CBoxAusotsEnabled.Name = "CBoxAusotsEnabled";
            this.CBoxAusotsEnabled.Size = new System.Drawing.Size(200, 31);
            this.CBoxAusotsEnabled.TabIndex = 6;
            // 
            // CBoxPacotsEnabled
            // 
            this.CBoxPacotsEnabled.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.CBoxPacotsEnabled.BorderColor = System.Drawing.Color.DimGray;
            this.CBoxPacotsEnabled.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.CBoxPacotsEnabled.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CBoxPacotsEnabled.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.CBoxPacotsEnabled.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CBoxPacotsEnabled.FormattingEnabled = true;
            this.CBoxPacotsEnabled.Items.AddRange(new object[] {
            "Enabled",
            "Disabled"});
            this.CBoxPacotsEnabled.Location = new System.Drawing.Point(108, 44);
            this.CBoxPacotsEnabled.Margin = new System.Windows.Forms.Padding(2);
            this.CBoxPacotsEnabled.Name = "CBoxPacotsEnabled";
            this.CBoxPacotsEnabled.Size = new System.Drawing.Size(200, 31);
            this.CBoxPacotsEnabled.TabIndex = 3;
            // 
            // CBoxNatsEnabled
            // 
            this.CBoxNatsEnabled.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.CBoxNatsEnabled.BorderColor = System.Drawing.Color.DimGray;
            this.CBoxNatsEnabled.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.CBoxNatsEnabled.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CBoxNatsEnabled.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.CBoxNatsEnabled.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CBoxNatsEnabled.FormattingEnabled = true;
            this.CBoxNatsEnabled.Items.AddRange(new object[] {
            "Enabled",
            "Disabled"});
            this.CBoxNatsEnabled.Location = new System.Drawing.Point(108, 4);
            this.CBoxNatsEnabled.Margin = new System.Windows.Forms.Padding(2);
            this.CBoxNatsEnabled.Name = "CBoxNatsEnabled";
            this.CBoxNatsEnabled.Size = new System.Drawing.Size(200, 31);
            this.CBoxNatsEnabled.TabIndex = 0;
            // 
            // TracksControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.Controls.Add(this.tableLayoutPanel2);
            this.Name = "TracksControl";
            this.Size = new System.Drawing.Size(988, 685);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.tableLayoutPanel4.ResumeLayout(false);
            this.tableLayoutPanel4.PerformLayout();
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PicBoxAusots)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PicBoxPacots)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PicBoxNats)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ListView ListView1;
        internal System.Windows.Forms.ColumnHeader ColumnHeader1;
        internal System.Windows.Forms.ColumnHeader ColMessage;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.RichTextBox txtRichTextBox;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private System.Windows.Forms.Button importBtn;
        private System.Windows.Forms.Button downloadAllBtn;
        private System.Windows.Forms.Button saveBtn;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.Button BtnAusotsDn;
        private System.Windows.Forms.Button BtnPacotsDn;
        private System.Windows.Forms.Button BtnNatsDn;
        private System.Windows.Forms.PictureBox PicBoxAusots;
        private System.Windows.Forms.PictureBox PicBoxPacots;
        private System.Windows.Forms.PictureBox PicBoxNats;
        private System.Windows.Forms.Button viewAusotsBtn;
        private System.Windows.Forms.Button viewPacotsBtn;
        private System.Windows.Forms.Button viewNatsBtn;
        private System.Windows.Forms.Label Label1;
        private System.Windows.Forms.Label Label2;
        private System.Windows.Forms.Label Label3;
        private Views.ComboBoxWithBorder CBoxAusotsEnabled;
        private Views.ComboBoxWithBorder CBoxPacotsEnabled;
        private Views.ComboBoxWithBorder CBoxNatsEnabled;
    }
}
