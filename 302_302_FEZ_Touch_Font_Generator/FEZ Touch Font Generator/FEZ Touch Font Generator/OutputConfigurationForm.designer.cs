
namespace FEZTouch.FontGenerator
{
    partial class OutputConfigurationForm
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OutputConfigurationForm));
			this.gbxPadding = new System.Windows.Forms.GroupBox();
			this.paddingHorizontal = new System.Windows.Forms.ComboBox();
			this.label17 = new System.Windows.Forms.Label();
			this.paddingVertical = new System.Windows.Forms.ComboBox();
			this.label16 = new System.Windows.Forms.Label();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.flipVertical = new System.Windows.Forms.CheckBox();
			this.flipHorizontal = new System.Windows.Forms.CheckBox();
			this.charRotation = new System.Windows.Forms.ComboBox();
			this.outputConfigurations = new System.Windows.Forms.ComboBox();
			this.label15 = new System.Windows.Forms.Label();
			this.btnSaveNewConfig = new System.Windows.Forms.Button();
			this.btnApply = new System.Windows.Forms.Button();
			this.label4 = new System.Windows.Forms.Label();
			this.groupBox7 = new System.Windows.Forms.GroupBox();
			this.label3 = new System.Windows.Forms.Label();
			this.minCharHeight = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.minCharWidth = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.spaceCharPixels = new System.Windows.Forms.TextBox();
			this.label6 = new System.Windows.Forms.Label();
			this.interCharPixels = new System.Windows.Forms.TextBox();
			this.btnDeleteConfig = new System.Windows.Forms.Button();
			this.btnUpdateConfig = new System.Windows.Forms.Button();
			this.gbxPadding.SuspendLayout();
			this.groupBox1.SuspendLayout();
			this.groupBox7.SuspendLayout();
			this.SuspendLayout();
			// 
			// gbxPadding
			// 
			this.gbxPadding.Controls.Add(this.paddingHorizontal);
			this.gbxPadding.Controls.Add(this.label17);
			this.gbxPadding.Controls.Add(this.paddingVertical);
			this.gbxPadding.Controls.Add(this.label16);
			this.gbxPadding.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.gbxPadding.Location = new System.Drawing.Point(125, 58);
			this.gbxPadding.Name = "gbxPadding";
			this.gbxPadding.Size = new System.Drawing.Size(202, 92);
			this.gbxPadding.TabIndex = 21;
			this.gbxPadding.TabStop = false;
			this.gbxPadding.Text = "Character size";
			// 
			// paddingHorizontal
			// 
			this.paddingHorizontal.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.paddingHorizontal.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.paddingHorizontal.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
			this.paddingHorizontal.FormattingEnabled = true;
			this.paddingHorizontal.Location = new System.Drawing.Point(79, 23);
			this.paddingHorizontal.Name = "paddingHorizontal";
			this.paddingHorizontal.Size = new System.Drawing.Size(103, 21);
			this.paddingHorizontal.TabIndex = 24;
			this.paddingHorizontal.SelectedIndexChanged += new System.EventHandler(this.onOutputConfigurationFormChange);
			// 
			// label17
			// 
			this.label17.AutoSize = true;
			this.label17.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
			this.label17.Location = new System.Drawing.Point(22, 26);
			this.label17.Name = "label17";
			this.label17.Size = new System.Drawing.Size(54, 13);
			this.label17.TabIndex = 23;
			this.label17.Text = "Width (X):";
			// 
			// paddingVertical
			// 
			this.paddingVertical.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.paddingVertical.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.paddingVertical.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
			this.paddingVertical.FormattingEnabled = true;
			this.paddingVertical.Location = new System.Drawing.Point(79, 57);
			this.paddingVertical.Name = "paddingVertical";
			this.paddingVertical.Size = new System.Drawing.Size(103, 21);
			this.paddingVertical.TabIndex = 22;
			this.paddingVertical.SelectedIndexChanged += new System.EventHandler(this.onOutputConfigurationFormChange);
			// 
			// label16
			// 
			this.label16.AutoSize = true;
			this.label16.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
			this.label16.Location = new System.Drawing.Point(19, 60);
			this.label16.Name = "label16";
			this.label16.Size = new System.Drawing.Size(57, 13);
			this.label16.TabIndex = 21;
			this.label16.Text = "Height (Y):";
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.flipVertical);
			this.groupBox1.Controls.Add(this.flipHorizontal);
			this.groupBox1.Controls.Add(this.charRotation);
			this.groupBox1.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.groupBox1.Location = new System.Drawing.Point(17, 58);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(101, 92);
			this.groupBox1.TabIndex = 20;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Flip/ Rotate";
			// 
			// flipVertical
			// 
			this.flipVertical.AutoSize = true;
			this.flipVertical.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.flipVertical.Location = new System.Drawing.Point(15, 41);
			this.flipVertical.Name = "flipVertical";
			this.flipVertical.Size = new System.Drawing.Size(52, 17);
			this.flipVertical.TabIndex = 28;
			this.flipVertical.Text = "Flip Y";
			this.flipVertical.UseVisualStyleBackColor = true;
			this.flipVertical.CheckedChanged += new System.EventHandler(this.onOutputConfigurationFormChange);
			// 
			// flipHorizontal
			// 
			this.flipHorizontal.AutoSize = true;
			this.flipHorizontal.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.flipHorizontal.Location = new System.Drawing.Point(15, 23);
			this.flipHorizontal.Name = "flipHorizontal";
			this.flipHorizontal.Size = new System.Drawing.Size(52, 17);
			this.flipHorizontal.TabIndex = 27;
			this.flipHorizontal.Text = "Flip X";
			this.flipHorizontal.UseVisualStyleBackColor = true;
			this.flipHorizontal.CheckedChanged += new System.EventHandler(this.onOutputConfigurationFormChange);
			// 
			// charRotation
			// 
			this.charRotation.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.charRotation.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.charRotation.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
			this.charRotation.FormattingEnabled = true;
			this.charRotation.Location = new System.Drawing.Point(15, 62);
			this.charRotation.Name = "charRotation";
			this.charRotation.Size = new System.Drawing.Size(59, 21);
			this.charRotation.TabIndex = 23;
			this.charRotation.SelectedIndexChanged += new System.EventHandler(this.onOutputConfigurationFormChange);
			// 
			// outputConfigurations
			// 
			this.outputConfigurations.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.outputConfigurations.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.outputConfigurations.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.outputConfigurations.FormattingEnabled = true;
			this.outputConfigurations.Location = new System.Drawing.Point(61, 12);
			this.outputConfigurations.Name = "outputConfigurations";
			this.outputConfigurations.Size = new System.Drawing.Size(250, 21);
			this.outputConfigurations.TabIndex = 36;
			this.outputConfigurations.SelectedIndexChanged += new System.EventHandler(this.outputConfigurations_SelectedIndexChanged);
			// 
			// label15
			// 
			this.label15.AutoSize = true;
			this.label15.Location = new System.Drawing.Point(19, 15);
			this.label15.Name = "label15";
			this.label15.Size = new System.Drawing.Size(40, 13);
			this.label15.TabIndex = 35;
			this.label15.Text = "Preset:";
			// 
			// btnSaveNewConfig
			// 
			this.btnSaveNewConfig.FlatAppearance.BorderSize = 0;
			this.btnSaveNewConfig.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnSaveNewConfig.Image = ((System.Drawing.Image)(resources.GetObject("btnSaveNewConfig.Image")));
			this.btnSaveNewConfig.Location = new System.Drawing.Point(352, 12);
			this.btnSaveNewConfig.Name = "btnSaveNewConfig";
			this.btnSaveNewConfig.Size = new System.Drawing.Size(26, 23);
			this.btnSaveNewConfig.TabIndex = 37;
			this.btnSaveNewConfig.UseVisualStyleBackColor = true;
			this.btnSaveNewConfig.Click += new System.EventHandler(this.btnSaveNewConfig_Click);
			// 
			// btnApply
			// 
			this.btnApply.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.btnApply.Image = ((System.Drawing.Image)(resources.GetObject("btnApply.Image")));
			this.btnApply.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.btnApply.Location = new System.Drawing.Point(345, 58);
			this.btnApply.Name = "btnApply";
			this.btnApply.Size = new System.Drawing.Size(64, 23);
			this.btnApply.TabIndex = 40;
			this.btnApply.Text = "Apply ";
			this.btnApply.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.btnApply.UseVisualStyleBackColor = true;
			this.btnApply.Click += new System.EventHandler(this.button1_Click);
			// 
			// label4
			// 
			this.label4.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.label4.Location = new System.Drawing.Point(18, 44);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(391, 2);
			this.label4.TabIndex = 42;
			// 
			// groupBox7
			// 
			this.groupBox7.Controls.Add(this.label3);
			this.groupBox7.Controls.Add(this.minCharHeight);
			this.groupBox7.Controls.Add(this.label2);
			this.groupBox7.Controls.Add(this.minCharWidth);
			this.groupBox7.Controls.Add(this.label1);
			this.groupBox7.Controls.Add(this.spaceCharPixels);
			this.groupBox7.Controls.Add(this.label6);
			this.groupBox7.Controls.Add(this.interCharPixels);
			this.groupBox7.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.groupBox7.Location = new System.Drawing.Point(17, 166);
			this.groupBox7.Name = "groupBox7";
			this.groupBox7.Size = new System.Drawing.Size(310, 148);
			this.groupBox7.TabIndex = 44;
			this.groupBox7.TabStop = false;
			this.groupBox7.Text = "Character spacing and size";
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
			this.label3.Location = new System.Drawing.Point(55, 59);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(127, 13);
			this.label3.TabIndex = 55;
			this.label3.Text = "minimum character height";
			// 
			// minCharHeight
			// 
			this.minCharHeight.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.minCharHeight.Location = new System.Drawing.Point(15, 56);
			this.minCharHeight.Name = "minCharHeight";
			this.minCharHeight.Size = new System.Drawing.Size(36, 21);
			this.minCharHeight.TabIndex = 54;
			this.minCharHeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.minCharHeight.TextChanged += new System.EventHandler(this.onOutputConfigurationFormChange);
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
			this.label2.Location = new System.Drawing.Point(55, 32);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(123, 13);
			this.label2.TabIndex = 53;
			this.label2.Text = "minimum character width";
			// 
			// minCharWidth
			// 
			this.minCharWidth.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.minCharWidth.Location = new System.Drawing.Point(15, 29);
			this.minCharWidth.Name = "minCharWidth";
			this.minCharWidth.Size = new System.Drawing.Size(36, 21);
			this.minCharWidth.TabIndex = 52;
			this.minCharWidth.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.minCharWidth.TextChanged += new System.EventHandler(this.onOutputConfigurationFormChange);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
			this.label1.Location = new System.Drawing.Point(55, 113);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(115, 13);
			this.label1.TabIndex = 51;
			this.label1.Text = "inter-character spacing";
			// 
			// spaceCharPixels
			// 
			this.spaceCharPixels.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.spaceCharPixels.Location = new System.Drawing.Point(15, 83);
			this.spaceCharPixels.Name = "spaceCharPixels";
			this.spaceCharPixels.Size = new System.Drawing.Size(36, 21);
			this.spaceCharPixels.TabIndex = 48;
			this.spaceCharPixels.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.spaceCharPixels.TextChanged += new System.EventHandler(this.onOutputConfigurationFormChange);
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
			this.label6.Location = new System.Drawing.Point(55, 86);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(112, 13);
			this.label6.TabIndex = 49;
			this.label6.Text = "space character width";
			// 
			// interCharPixels
			// 
			this.interCharPixels.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.interCharPixels.Location = new System.Drawing.Point(15, 110);
			this.interCharPixels.Name = "interCharPixels";
			this.interCharPixels.Size = new System.Drawing.Size(36, 21);
			this.interCharPixels.TabIndex = 50;
			this.interCharPixels.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.interCharPixels.TextChanged += new System.EventHandler(this.onOutputConfigurationFormChange);
			// 
			// btnDeleteConfig
			// 
			this.btnDeleteConfig.FlatAppearance.BorderSize = 0;
			this.btnDeleteConfig.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnDeleteConfig.Image = ((System.Drawing.Image)(resources.GetObject("btnDeleteConfig.Image")));
			this.btnDeleteConfig.Location = new System.Drawing.Point(381, 12);
			this.btnDeleteConfig.Name = "btnDeleteConfig";
			this.btnDeleteConfig.Size = new System.Drawing.Size(26, 23);
			this.btnDeleteConfig.TabIndex = 45;
			this.btnDeleteConfig.UseVisualStyleBackColor = true;
			this.btnDeleteConfig.Click += new System.EventHandler(this.btnDeleteConfig_Click);
			// 
			// btnUpdateConfig
			// 
			this.btnUpdateConfig.Enabled = false;
			this.btnUpdateConfig.FlatAppearance.BorderSize = 0;
			this.btnUpdateConfig.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnUpdateConfig.Image = ((System.Drawing.Image)(resources.GetObject("btnUpdateConfig.Image")));
			this.btnUpdateConfig.Location = new System.Drawing.Point(323, 12);
			this.btnUpdateConfig.Name = "btnUpdateConfig";
			this.btnUpdateConfig.Size = new System.Drawing.Size(26, 23);
			this.btnUpdateConfig.TabIndex = 46;
			this.btnUpdateConfig.UseVisualStyleBackColor = true;
			this.btnUpdateConfig.Click += new System.EventHandler(this.btnUpdateConfig_Click);
			// 
			// OutputConfigurationForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(425, 333);
			this.Controls.Add(this.btnUpdateConfig);
			this.Controls.Add(this.btnDeleteConfig);
			this.Controls.Add(this.groupBox7);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.btnApply);
			this.Controls.Add(this.btnSaveNewConfig);
			this.Controls.Add(this.outputConfigurations);
			this.Controls.Add(this.label15);
			this.Controls.Add(this.gbxPadding);
			this.Controls.Add(this.groupBox1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.Name = "OutputConfigurationForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = " Modify Output Configuration";
			this.gbxPadding.ResumeLayout(false);
			this.gbxPadding.PerformLayout();
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.groupBox7.ResumeLayout(false);
			this.groupBox7.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

		private System.Windows.Forms.GroupBox gbxPadding;
        private System.Windows.Forms.ComboBox paddingHorizontal;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.ComboBox paddingVertical;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ComboBox outputConfigurations;
        private System.Windows.Forms.Label label15;
		private System.Windows.Forms.Button btnSaveNewConfig;
		private System.Windows.Forms.Button btnApply;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox charRotation;
        private System.Windows.Forms.CheckBox flipVertical;
		private System.Windows.Forms.CheckBox flipHorizontal;
        private System.Windows.Forms.GroupBox groupBox7;
        private System.Windows.Forms.Label label6;
		private System.Windows.Forms.TextBox spaceCharPixels;
        private System.Windows.Forms.Button btnDeleteConfig;
		private System.Windows.Forms.Button btnUpdateConfig;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox interCharPixels;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox minCharHeight;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox minCharWidth;
    }
}