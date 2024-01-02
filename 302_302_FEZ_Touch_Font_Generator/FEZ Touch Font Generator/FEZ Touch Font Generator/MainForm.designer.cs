
namespace FEZTouch.FontGenerator
{
    partial class MainForm
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
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
			this.menuStrip1 = new System.Windows.Forms.MenuStrip();
			this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.copySourceToClipboardToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.saveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
			this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.aboutToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
			this.statusStrip1 = new System.Windows.Forms.StatusStrip();
			this.splitContainer1 = new System.Windows.Forms.SplitContainer();
			this.tcInput = new System.Windows.Forms.TabControl();
			this.tabPage1 = new System.Windows.Forms.TabPage();
			this.panel5 = new System.Windows.Forms.Panel();
			this.txtInputText = new System.Windows.Forms.TextBox();
			this.panel10 = new System.Windows.Forms.Panel();
			this.panel6 = new System.Windows.Forms.Panel();
			this.btnInsertText = new System.Windows.Forms.Button();
			this.btnFontSelect = new System.Windows.Forms.Button();
			this.textInsert = new System.Windows.Forms.ComboBox();
			this.label12 = new System.Windows.Forms.Label();
			this.lblFont = new System.Windows.Forms.Label();
			this.inputFont = new System.Windows.Forms.TextBox();
			this.panel3 = new System.Windows.Forms.Panel();
			this.panel1 = new System.Windows.Forms.Panel();
			this.label1 = new System.Windows.Forms.Label();
			this.panel7 = new System.Windows.Forms.Panel();
			this.outputTextDisplay = new System.Windows.Forms.RichTextBox();
			this.ctxMenuSource = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.tsmCopySource = new System.Windows.Forms.ToolStripMenuItem();
			this.label6 = new System.Windows.Forms.Label();
			this.panel4 = new System.Windows.Forms.Panel();
			this.label4 = new System.Windows.Forms.Label();
			this.btnOutputConfig = new System.Windows.Forms.Button();
			this.btnGenerate = new System.Windows.Forms.Button();
			this.outputConfiguration = new System.Windows.Forms.ComboBox();
			this.label15 = new System.Windows.Forms.Label();
			this.panel2 = new System.Windows.Forms.Panel();
			this.label2 = new System.Windows.Forms.Label();
			this.ctxMenuHeader = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.tsmCopyHeader = new System.Windows.Forms.ToolStripMenuItem();
			this.fontDlgInputFont = new System.Windows.Forms.FontDialog();
			this.label8 = new System.Windows.Forms.Label();
			this.label9 = new System.Windows.Forms.Label();
			this.textBox2 = new System.Windows.Forms.TextBox();
			this.button1 = new System.Windows.Forms.Button();
			this.label10 = new System.Windows.Forms.Label();
			this.textBox3 = new System.Windows.Forms.TextBox();
			this.dlgOpenFile = new System.Windows.Forms.OpenFileDialog();
			this.label14 = new System.Windows.Forms.Label();
			this.button2 = new System.Windows.Forms.Button();
			this.button3 = new System.Windows.Forms.Button();
			this.comboBox1 = new System.Windows.Forms.ComboBox();
			this.label16 = new System.Windows.Forms.Label();
			this.dlgSaveAs = new System.Windows.Forms.SaveFileDialog();
			this.menuStrip1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.Panel2.SuspendLayout();
			this.splitContainer1.SuspendLayout();
			this.tcInput.SuspendLayout();
			this.tabPage1.SuspendLayout();
			this.panel5.SuspendLayout();
			this.panel6.SuspendLayout();
			this.panel1.SuspendLayout();
			this.panel7.SuspendLayout();
			this.ctxMenuSource.SuspendLayout();
			this.panel4.SuspendLayout();
			this.panel2.SuspendLayout();
			this.ctxMenuHeader.SuspendLayout();
			this.SuspendLayout();
			// 
			// menuStrip1
			// 
			this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.aboutToolStripMenuItem});
			this.menuStrip1.Location = new System.Drawing.Point(0, 0);
			this.menuStrip1.Name = "menuStrip1";
			this.menuStrip1.Size = new System.Drawing.Size(989, 24);
			this.menuStrip1.TabIndex = 0;
			this.menuStrip1.Text = "menuStrip1";
			// 
			// fileToolStripMenuItem
			// 
			this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.copySourceToClipboardToolStripMenuItem,
            this.saveAsToolStripMenuItem,
            this.toolStripMenuItem1,
            this.exitToolStripMenuItem});
			this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
			this.fileToolStripMenuItem.Size = new System.Drawing.Size(35, 20);
			this.fileToolStripMenuItem.Text = "File";
			// 
			// copySourceToClipboardToolStripMenuItem
			// 
			this.copySourceToClipboardToolStripMenuItem.Name = "copySourceToClipboardToolStripMenuItem";
			this.copySourceToClipboardToolStripMenuItem.Size = new System.Drawing.Size(196, 22);
			this.copySourceToClipboardToolStripMenuItem.Text = "Copy Source to Clipboard";
			this.copySourceToClipboardToolStripMenuItem.Click += new System.EventHandler(this.copySourceMenuItem_Click);
			// 
			// saveAsToolStripMenuItem
			// 
			this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
			this.saveAsToolStripMenuItem.Size = new System.Drawing.Size(196, 22);
			this.saveAsToolStripMenuItem.Text = "Save as files ...";
			this.saveAsToolStripMenuItem.Click += new System.EventHandler(this.saveAsMenuItem_Click);
			// 
			// toolStripMenuItem1
			// 
			this.toolStripMenuItem1.Name = "toolStripMenuItem1";
			this.toolStripMenuItem1.Size = new System.Drawing.Size(193, 6);
			// 
			// exitToolStripMenuItem
			// 
			this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
			this.exitToolStripMenuItem.Size = new System.Drawing.Size(196, 22);
			this.exitToolStripMenuItem.Text = "Exit";
			this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitMenuItem_Click);
			// 
			// aboutToolStripMenuItem
			// 
			this.aboutToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem1});
			this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
			this.aboutToolStripMenuItem.Size = new System.Drawing.Size(40, 20);
			this.aboutToolStripMenuItem.Text = "Help";
			// 
			// aboutToolStripMenuItem1
			// 
			this.aboutToolStripMenuItem1.Name = "aboutToolStripMenuItem1";
			this.aboutToolStripMenuItem1.Size = new System.Drawing.Size(103, 22);
			this.aboutToolStripMenuItem1.Text = "About";
			this.aboutToolStripMenuItem1.Click += new System.EventHandler(this.aboutMenuItem1_Click);
			// 
			// statusStrip1
			// 
			this.statusStrip1.Location = new System.Drawing.Point(0, 646);
			this.statusStrip1.Name = "statusStrip1";
			this.statusStrip1.Size = new System.Drawing.Size(989, 22);
			this.statusStrip1.TabIndex = 3;
			this.statusStrip1.Text = "statusStrip1";
			// 
			// splitContainer1
			// 
			this.splitContainer1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainer1.Location = new System.Drawing.Point(0, 24);
			this.splitContainer1.Name = "splitContainer1";
			// 
			// splitContainer1.Panel1
			// 
			this.splitContainer1.Panel1.Controls.Add(this.tcInput);
			this.splitContainer1.Panel1.Controls.Add(this.panel3);
			this.splitContainer1.Panel1.Controls.Add(this.panel1);
			// 
			// splitContainer1.Panel2
			// 
			this.splitContainer1.Panel2.Controls.Add(this.panel7);
			this.splitContainer1.Panel2.Controls.Add(this.panel4);
			this.splitContainer1.Panel2.Controls.Add(this.panel2);
			this.splitContainer1.Size = new System.Drawing.Size(989, 622);
			this.splitContainer1.SplitterDistance = 488;
			this.splitContainer1.TabIndex = 4;
			this.splitContainer1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.splitContainer1_MouseUp);
			// 
			// tcInput
			// 
			this.tcInput.Controls.Add(this.tabPage1);
			this.tcInput.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tcInput.Location = new System.Drawing.Point(0, 31);
			this.tcInput.Name = "tcInput";
			this.tcInput.SelectedIndex = 0;
			this.tcInput.Size = new System.Drawing.Size(484, 587);
			this.tcInput.TabIndex = 7;
			// 
			// tabPage1
			// 
			this.tabPage1.Controls.Add(this.panel5);
			this.tabPage1.Controls.Add(this.panel10);
			this.tabPage1.Controls.Add(this.panel6);
			this.tabPage1.Location = new System.Drawing.Point(4, 22);
			this.tabPage1.Name = "tabPage1";
			this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage1.Size = new System.Drawing.Size(476, 561);
			this.tabPage1.TabIndex = 0;
			this.tabPage1.Text = "Text";
			this.tabPage1.UseVisualStyleBackColor = true;
			// 
			// panel5
			// 
			this.panel5.Controls.Add(this.txtInputText);
			this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
			this.panel5.Location = new System.Drawing.Point(3, 81);
			this.panel5.Name = "panel5";
			this.panel5.Size = new System.Drawing.Size(470, 129);
			this.panel5.TabIndex = 8;
			// 
			// txtInputText
			// 
			this.txtInputText.Dock = System.Windows.Forms.DockStyle.Fill;
			this.txtInputText.Location = new System.Drawing.Point(0, 0);
			this.txtInputText.Multiline = true;
			this.txtInputText.Name = "txtInputText";
			this.txtInputText.Size = new System.Drawing.Size(470, 129);
			this.txtInputText.TabIndex = 9;
			// 
			// panel10
			// 
			this.panel10.Dock = System.Windows.Forms.DockStyle.Top;
			this.panel10.Location = new System.Drawing.Point(3, 76);
			this.panel10.Name = "panel10";
			this.panel10.Size = new System.Drawing.Size(470, 5);
			this.panel10.TabIndex = 7;
			// 
			// panel6
			// 
			this.panel6.Controls.Add(this.btnInsertText);
			this.panel6.Controls.Add(this.btnFontSelect);
			this.panel6.Controls.Add(this.textInsert);
			this.panel6.Controls.Add(this.label12);
			this.panel6.Controls.Add(this.lblFont);
			this.panel6.Controls.Add(this.inputFont);
			this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
			this.panel6.Location = new System.Drawing.Point(3, 3);
			this.panel6.Name = "panel6";
			this.panel6.Size = new System.Drawing.Size(470, 73);
			this.panel6.TabIndex = 3;
			// 
			// btnInsertText
			// 
			this.btnInsertText.FlatAppearance.BorderSize = 0;
			this.btnInsertText.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnInsertText.Image = ((System.Drawing.Image)(resources.GetObject("btnInsertText.Image")));
			this.btnInsertText.Location = new System.Drawing.Point(261, 38);
			this.btnInsertText.Name = "btnInsertText";
			this.btnInsertText.Size = new System.Drawing.Size(26, 23);
			this.btnInsertText.TabIndex = 15;
			this.btnInsertText.UseVisualStyleBackColor = true;
			this.btnInsertText.Click += new System.EventHandler(this.btnInsertText_Click);
			// 
			// btnFontSelect
			// 
			this.btnFontSelect.FlatAppearance.BorderSize = 0;
			this.btnFontSelect.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnFontSelect.Image = ((System.Drawing.Image)(resources.GetObject("btnFontSelect.Image")));
			this.btnFontSelect.Location = new System.Drawing.Point(379, 11);
			this.btnFontSelect.Name = "btnFontSelect";
			this.btnFontSelect.Size = new System.Drawing.Size(26, 23);
			this.btnFontSelect.TabIndex = 14;
			this.btnFontSelect.UseVisualStyleBackColor = true;
			this.btnFontSelect.Click += new System.EventHandler(this.btnFontSelect_Click);
			// 
			// textInsert
			// 
			this.textInsert.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.textInsert.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.textInsert.FormattingEnabled = true;
			this.textInsert.Location = new System.Drawing.Point(80, 39);
			this.textInsert.Name = "textInsert";
			this.textInsert.Size = new System.Drawing.Size(175, 21);
			this.textInsert.TabIndex = 13;
			// 
			// label12
			// 
			this.label12.AutoSize = true;
			this.label12.Location = new System.Drawing.Point(18, 42);
			this.label12.Name = "label12";
			this.label12.Size = new System.Drawing.Size(56, 13);
			this.label12.TabIndex = 12;
			this.label12.Text = "Insert text:";
			// 
			// lblFont
			// 
			this.lblFont.AutoSize = true;
			this.lblFont.Location = new System.Drawing.Point(43, 16);
			this.lblFont.Name = "lblFont";
			this.lblFont.Size = new System.Drawing.Size(31, 13);
			this.lblFont.TabIndex = 1;
			this.lblFont.Text = "Font:";
			// 
			// inputFont
			// 
			this.inputFont.Location = new System.Drawing.Point(80, 13);
			this.inputFont.Name = "inputFont";
			this.inputFont.Size = new System.Drawing.Size(293, 20);
			this.inputFont.TabIndex = 0;
			// 
			// panel3
			// 
			this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
			this.panel3.Location = new System.Drawing.Point(0, 21);
			this.panel3.Name = "panel3";
			this.panel3.Size = new System.Drawing.Size(484, 10);
			this.panel3.TabIndex = 6;
			// 
			// panel1
			// 
			this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(142)))), ((int)(((byte)(142)))), ((int)(((byte)(142)))));
			this.panel1.Controls.Add(this.label1);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
			this.panel1.Location = new System.Drawing.Point(0, 0);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(484, 21);
			this.panel1.TabIndex = 0;
			// 
			// label1
			// 
			this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.label1.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label1.ForeColor = System.Drawing.Color.White;
			this.label1.Location = new System.Drawing.Point(0, 0);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(484, 21);
			this.label1.TabIndex = 0;
			this.label1.Text = "Input";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// panel7
			// 
			this.panel7.Controls.Add(this.outputTextDisplay);
			this.panel7.Controls.Add(this.label6);
			this.panel7.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel7.Location = new System.Drawing.Point(0, 68);
			this.panel7.Margin = new System.Windows.Forms.Padding(10);
			this.panel7.Name = "panel7";
			this.panel7.Padding = new System.Windows.Forms.Padding(5);
			this.panel7.Size = new System.Drawing.Size(493, 550);
			this.panel7.TabIndex = 6;
			// 
			// outputTextDisplay
			// 
			this.outputTextDisplay.ContextMenuStrip = this.ctxMenuSource;
			this.outputTextDisplay.Dock = System.Windows.Forms.DockStyle.Fill;
			this.outputTextDisplay.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.outputTextDisplay.Location = new System.Drawing.Point(5, 21);
			this.outputTextDisplay.Name = "outputTextDisplay";
			this.outputTextDisplay.Size = new System.Drawing.Size(483, 524);
			this.outputTextDisplay.TabIndex = 16;
			this.outputTextDisplay.Text = "";
			this.outputTextDisplay.WordWrap = false;
			// 
			// ctxMenuSource
			// 
			this.ctxMenuSource.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmCopySource});
			this.ctxMenuSource.Name = "ctxMenuSource";
			this.ctxMenuSource.Size = new System.Drawing.Size(100, 26);
			// 
			// tsmCopySource
			// 
			this.tsmCopySource.Name = "tsmCopySource";
			this.tsmCopySource.Size = new System.Drawing.Size(99, 22);
			this.tsmCopySource.Text = "Copy";
			// 
			// label6
			// 
			this.label6.Dock = System.Windows.Forms.DockStyle.Top;
			this.label6.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label6.Location = new System.Drawing.Point(5, 5);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(483, 16);
			this.label6.TabIndex = 0;
			this.label6.Text = "FEZ Touch Font";
			// 
			// panel4
			// 
			this.panel4.Controls.Add(this.label4);
			this.panel4.Controls.Add(this.btnOutputConfig);
			this.panel4.Controls.Add(this.btnGenerate);
			this.panel4.Controls.Add(this.outputConfiguration);
			this.panel4.Controls.Add(this.label15);
			this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
			this.panel4.Location = new System.Drawing.Point(0, 21);
			this.panel4.Name = "panel4";
			this.panel4.Size = new System.Drawing.Size(493, 47);
			this.panel4.TabIndex = 2;
			// 
			// label4
			// 
			this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.label4.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.label4.Location = new System.Drawing.Point(5, 42);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(483, 2);
			this.label4.TabIndex = 33;
			this.label4.Text = "label4";
			// 
			// btnOutputConfig
			// 
			this.btnOutputConfig.Anchor = System.Windows.Forms.AnchorStyles.Right;
			this.btnOutputConfig.FlatAppearance.BorderSize = 0;
			this.btnOutputConfig.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnOutputConfig.Image = ((System.Drawing.Image)(resources.GetObject("btnOutputConfig.Image")));
			this.btnOutputConfig.Location = new System.Drawing.Point(342, 9);
			this.btnOutputConfig.Name = "btnOutputConfig";
			this.btnOutputConfig.Size = new System.Drawing.Size(26, 23);
			this.btnOutputConfig.TabIndex = 32;
			this.btnOutputConfig.UseVisualStyleBackColor = true;
			this.btnOutputConfig.Click += new System.EventHandler(this.btnOutputConfig_Click);
			// 
			// btnGenerate
			// 
			this.btnGenerate.Anchor = System.Windows.Forms.AnchorStyles.Right;
			this.btnGenerate.FlatAppearance.BorderSize = 0;
			this.btnGenerate.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.btnGenerate.Image = ((System.Drawing.Image)(resources.GetObject("btnGenerate.Image")));
			this.btnGenerate.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.btnGenerate.Location = new System.Drawing.Point(407, 10);
			this.btnGenerate.Name = "btnGenerate";
			this.btnGenerate.Size = new System.Drawing.Size(76, 23);
			this.btnGenerate.TabIndex = 31;
			this.btnGenerate.Text = "Generate";
			this.btnGenerate.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.btnGenerate.UseVisualStyleBackColor = true;
			this.btnGenerate.Click += new System.EventHandler(this.btnGenerate_Click);
			// 
			// outputConfiguration
			// 
			this.outputConfiguration.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.outputConfiguration.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.outputConfiguration.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.outputConfiguration.FormattingEnabled = true;
			this.outputConfiguration.Location = new System.Drawing.Point(60, 11);
			this.outputConfiguration.Name = "outputConfiguration";
			this.outputConfiguration.Size = new System.Drawing.Size(276, 21);
			this.outputConfiguration.TabIndex = 30;
			this.outputConfiguration.SelectedIndexChanged += new System.EventHandler(this.outputConfiguration_SelectedIndexChanged);
			// 
			// label15
			// 
			this.label15.AutoSize = true;
			this.label15.Location = new System.Drawing.Point(17, 14);
			this.label15.Name = "label15";
			this.label15.Size = new System.Drawing.Size(40, 13);
			this.label15.TabIndex = 29;
			this.label15.Text = "Preset:";
			// 
			// panel2
			// 
			this.panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(142)))), ((int)(((byte)(142)))), ((int)(((byte)(142)))));
			this.panel2.Controls.Add(this.label2);
			this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
			this.panel2.Location = new System.Drawing.Point(0, 0);
			this.panel2.Name = "panel2";
			this.panel2.Size = new System.Drawing.Size(493, 21);
			this.panel2.TabIndex = 1;
			// 
			// label2
			// 
			this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.label2.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label2.ForeColor = System.Drawing.Color.White;
			this.label2.Location = new System.Drawing.Point(0, 0);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(493, 21);
			this.label2.TabIndex = 0;
			this.label2.Text = "Output";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// ctxMenuHeader
			// 
			this.ctxMenuHeader.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmCopyHeader});
			this.ctxMenuHeader.Name = "ctxMenuSource";
			this.ctxMenuHeader.Size = new System.Drawing.Size(100, 26);
			// 
			// tsmCopyHeader
			// 
			this.tsmCopyHeader.Name = "tsmCopyHeader";
			this.tsmCopyHeader.Size = new System.Drawing.Size(99, 22);
			this.tsmCopyHeader.Text = "Copy";
			// 
			// label8
			// 
			this.label8.AutoSize = true;
			this.label8.Location = new System.Drawing.Point(122, 41);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(33, 13);
			this.label8.TabIndex = 11;
			this.label8.Text = "pixels";
			// 
			// label9
			// 
			this.label9.AutoSize = true;
			this.label9.Location = new System.Drawing.Point(12, 41);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(62, 13);
			this.label9.TabIndex = 10;
			this.label9.Text = "Space size:";
			// 
			// textBox2
			// 
			this.textBox2.Location = new System.Drawing.Point(80, 39);
			this.textBox2.Name = "textBox2";
			this.textBox2.Size = new System.Drawing.Size(36, 20);
			this.textBox2.TabIndex = 9;
			this.textBox2.Text = "2";
			this.textBox2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			// 
			// button1
			// 
			this.button1.Anchor = System.Windows.Forms.AnchorStyles.Right;
			this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.button1.Location = new System.Drawing.Point(408, 11);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(28, 23);
			this.button1.TabIndex = 4;
			this.button1.Text = "...";
			this.button1.UseVisualStyleBackColor = true;
			// 
			// label10
			// 
			this.label10.AutoSize = true;
			this.label10.Location = new System.Drawing.Point(43, 16);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(31, 13);
			this.label10.TabIndex = 1;
			this.label10.Text = "Font:";
			// 
			// textBox3
			// 
			this.textBox3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.textBox3.Location = new System.Drawing.Point(80, 13);
			this.textBox3.Name = "textBox3";
			this.textBox3.Size = new System.Drawing.Size(322, 20);
			this.textBox3.TabIndex = 0;
			// 
			// dlgOpenFile
			// 
			this.dlgOpenFile.FileName = "openFileDialog1";
			// 
			// label14
			// 
			this.label14.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.label14.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.label14.Location = new System.Drawing.Point(20, 42);
			this.label14.Name = "label14";
			this.label14.Size = new System.Drawing.Size(466, 2);
			this.label14.TabIndex = 33;
			this.label14.Text = "label4";
			// 
			// button2
			// 
			this.button2.Anchor = System.Windows.Forms.AnchorStyles.Right;
			this.button2.FlatAppearance.BorderSize = 0;
			this.button2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.button2.Image = ((System.Drawing.Image)(resources.GetObject("button2.Image")));
			this.button2.Location = new System.Drawing.Point(342, 9);
			this.button2.Name = "button2";
			this.button2.Size = new System.Drawing.Size(26, 23);
			this.button2.TabIndex = 32;
			this.button2.UseVisualStyleBackColor = true;
			// 
			// button3
			// 
			this.button3.Anchor = System.Windows.Forms.AnchorStyles.Right;
			this.button3.FlatAppearance.BorderSize = 0;
			this.button3.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.button3.Image = ((System.Drawing.Image)(resources.GetObject("button3.Image")));
			this.button3.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.button3.Location = new System.Drawing.Point(407, 10);
			this.button3.Name = "button3";
			this.button3.Size = new System.Drawing.Size(76, 23);
			this.button3.TabIndex = 31;
			this.button3.Text = "Generate";
			this.button3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.button3.UseVisualStyleBackColor = true;
			// 
			// comboBox1
			// 
			this.comboBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboBox1.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.comboBox1.FormattingEnabled = true;
			this.comboBox1.Location = new System.Drawing.Point(60, 11);
			this.comboBox1.Name = "comboBox1";
			this.comboBox1.Size = new System.Drawing.Size(276, 21);
			this.comboBox1.TabIndex = 30;
			// 
			// label16
			// 
			this.label16.AutoSize = true;
			this.label16.Location = new System.Drawing.Point(17, 14);
			this.label16.Name = "label16";
			this.label16.Size = new System.Drawing.Size(40, 13);
			this.label16.TabIndex = 29;
			this.label16.Text = "Preset:";
			// 
			// dlgSaveAs
			// 
			this.dlgSaveAs.DefaultExt = "cs";
			this.dlgSaveAs.Filter = "\"C-sharp files (*.cs)|*.cs|All files (*.*)|*.*\" ";
			this.dlgSaveAs.Title = "Save font class file";
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(989, 668);
			this.Controls.Add(this.splitContainer1);
			this.Controls.Add(this.statusStrip1);
			this.Controls.Add(this.menuStrip1);
			this.MainMenuStrip = this.menuStrip1;
			this.Name = "MainForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Load += new System.EventHandler(this.Form1_Load);
			this.menuStrip1.ResumeLayout(false);
			this.menuStrip1.PerformLayout();
			this.splitContainer1.Panel1.ResumeLayout(false);
			this.splitContainer1.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
			this.splitContainer1.ResumeLayout(false);
			this.tcInput.ResumeLayout(false);
			this.tabPage1.ResumeLayout(false);
			this.panel5.ResumeLayout(false);
			this.panel5.PerformLayout();
			this.panel6.ResumeLayout(false);
			this.panel6.PerformLayout();
			this.panel1.ResumeLayout(false);
			this.panel7.ResumeLayout(false);
			this.ctxMenuSource.ResumeLayout(false);
			this.panel4.ResumeLayout(false);
			this.panel4.PerformLayout();
			this.panel2.ResumeLayout(false);
			this.ctxMenuHeader.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.FontDialog fontDlgInputFont;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.OpenFileDialog dlgOpenFile;
        private System.Windows.Forms.Panel panel7;
		private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem1;
        private System.Windows.Forms.Label label6;
		private System.Windows.Forms.RichTextBox outputTextDisplay;
        private System.Windows.Forms.Button btnGenerate;
        private System.Windows.Forms.ComboBox outputConfiguration;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Button btnOutputConfig;
		private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.ContextMenuStrip ctxMenuSource;
        private System.Windows.Forms.ToolStripMenuItem tsmCopySource;
        private System.Windows.Forms.ContextMenuStrip ctxMenuHeader;
        private System.Windows.Forms.ToolStripMenuItem tsmCopyHeader;
        private System.Windows.Forms.ToolStripMenuItem saveAsToolStripMenuItem;
        private System.Windows.Forms.SaveFileDialog dlgSaveAs;
		private System.Windows.Forms.ToolStripMenuItem copySourceToClipboardToolStripMenuItem;
		private System.Windows.Forms.TabControl tcInput;
		private System.Windows.Forms.TabPage tabPage1;
		private System.Windows.Forms.Panel panel5;
		private System.Windows.Forms.TextBox txtInputText;
		private System.Windows.Forms.Panel panel10;
		private System.Windows.Forms.Panel panel6;
		private System.Windows.Forms.Button btnInsertText;
		private System.Windows.Forms.Button btnFontSelect;
		private System.Windows.Forms.ComboBox textInsert;
		private System.Windows.Forms.Label label12;
		private System.Windows.Forms.Label lblFont;
		private System.Windows.Forms.TextBox inputFont;

    }
}

