
namespace FEZTouch.FontGenerator
{
    partial class AboutForm
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
			this.panel2 = new System.Windows.Forms.Panel();
			this.linkPavius = new System.Windows.Forms.LinkLabel();
			this.label3 = new System.Windows.Forms.Label();
			this.linkContact = new System.Windows.Forms.LinkLabel();
			this.linkJASDev = new System.Windows.Forms.LinkLabel();
			this.lblAppName = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.panel2.SuspendLayout();
			this.SuspendLayout();
			// 
			// panel2
			// 
			this.panel2.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
			this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.panel2.Controls.Add(this.linkPavius);
			this.panel2.Controls.Add(this.label3);
			this.panel2.Controls.Add(this.linkContact);
			this.panel2.Controls.Add(this.linkJASDev);
			this.panel2.Location = new System.Drawing.Point(9, 44);
			this.panel2.Name = "panel2";
			this.panel2.Size = new System.Drawing.Size(253, 59);
			this.panel2.TabIndex = 3;
			// 
			// linkPavius
			// 
			this.linkPavius.AutoSize = true;
			this.linkPavius.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
			this.linkPavius.Location = new System.Drawing.Point(142, 33);
			this.linkPavius.Name = "linkPavius";
			this.linkPavius.Size = new System.Drawing.Size(83, 13);
			this.linkPavius.TabIndex = 4;
			this.linkPavius.TabStop = true;
			this.linkPavius.Text = "www.pavius.net";
			this.linkPavius.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkPavius_LinkClicked);
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(3, 33);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(143, 13);
			this.label3.TabIndex = 3;
			this.label3.Text = "Based on TheDotFactory by ";
			// 
			// linkContact
			// 
			this.linkContact.AutoSize = true;
			this.linkContact.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
			this.linkContact.Location = new System.Drawing.Point(3, 5);
			this.linkContact.Name = "linkContact";
			this.linkContact.Size = new System.Drawing.Size(84, 13);
			this.linkContact.TabIndex = 2;
			this.linkContact.TabStop = true;
			this.linkContact.Text = "jim@jasdev.com";
			this.linkContact.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkContact_LinkClicked);
			// 
			// linkJASDev
			// 
			this.linkJASDev.AutoSize = true;
			this.linkJASDev.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
			this.linkJASDev.Location = new System.Drawing.Point(3, 19);
			this.linkJASDev.Name = "linkJASDev";
			this.linkJASDev.Size = new System.Drawing.Size(88, 13);
			this.linkJASDev.TabIndex = 1;
			this.linkJASDev.TabStop = true;
			this.linkJASDev.Text = "www.jasdev.com";
			this.linkJASDev.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkJASDev_LinkClicked);
			// 
			// lblAppName
			// 
			this.lblAppName.AutoSize = true;
			this.lblAppName.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblAppName.Location = new System.Drawing.Point(13, 7);
			this.lblAppName.Name = "lblAppName";
			this.lblAppName.Size = new System.Drawing.Size(172, 16);
			this.lblAppName.TabIndex = 6;
			this.lblAppName.Text = "FEZ Touch Font Generator";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(48, 23);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(184, 13);
			this.label2.TabIndex = 5;
			this.label2.Text = "Copyright 2011, JASDev International";
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(43, 113);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(182, 13);
			this.label1.TabIndex = 7;
			this.label1.Text = "Released under GPL (see license.txt)";
			// 
			// AboutForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(274, 135);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.lblAppName);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.panel2);
			this.Name = "AboutForm";
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "About";
			this.Load += new System.EventHandler(this.AboutForm_Load);
			this.panel2.ResumeLayout(false);
			this.panel2.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.LinkLabel linkContact;
        private System.Windows.Forms.LinkLabel linkJASDev;
        private System.Windows.Forms.Label lblAppName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.LinkLabel linkPavius;
        private System.Windows.Forms.Label label1;

    }
}