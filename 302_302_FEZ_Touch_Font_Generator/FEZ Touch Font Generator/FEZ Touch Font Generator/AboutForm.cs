/*
 * Copyright 2011, JASDev International
 *
  * The FEZ Touch Font Generator is based on the excellent font editor called The Dot Factory by Eran "Pavius" Duchan.
 * 
* The FEZ Touch Font Generator is free software: you can redistribute it and/or modify it 
 * under the terms of the GNU General Public License as published by the Free 
 * Software Foundation, either version 3 of the License, or (at your option) 
 * any later version. The FEZ Touch Font Generator is distributed in the hope that it will be 
 * useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY 
 * or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more 
 * details. You should have received a copy of the GNU General Public License along 
 * with the FEZ Touch Font Generator. If not, see http://www.gnu.org/licenses/.
 */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace FEZTouch.FontGenerator
{
    public partial class AboutForm : Form
    {
        public AboutForm()
        {
            InitializeComponent();
        }

        private void AboutForm_Load(object sender, EventArgs e)
        {
            // set text
            lblAppName.Text = String.Format("FEZ Touch Font Generator (ver {0})", MainForm.ApplicationVersion);
        }

		private void linkContact_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			// open JASDev email
			System.Diagnostics.Process.Start("mailto://jim@jasdev.com");
		}

        private void linkJASDev_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            // open JASDev site in web browser
            System.Diagnostics.Process.Start("http://www.jasdev.com");
        }

		private void linkPavius_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			// open pavius site in web browser
			System.Diagnostics.Process.Start("http://www.pavius.net");
		}
   }
}
