using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Resources;

namespace FEZTouch.FontGenerator
{
    public partial class OutputConfigurationForm : Form
	{
		#region Member Fields

		// output configuration manager
        private OutputConfigurationManager outputConfigurationManager;

        // flag indicating whether user loaded a preset configration and then changed
        // the settings for it without saving
        private bool presetConfigurationModified = false;

        // whther or not we're currently loading a configuration
		private bool loadingOutputConfigurationToForm = false;

		#endregion

		#region Methods

		// populate the fields
        private void populateControls()
        {
            // set datasources
			this.paddingHorizontal.DataSource = Enum.GetNames(typeof(OutputConfiguration.PaddingRemoval));
			this.paddingVertical.DataSource = Enum.GetNames(typeof(OutputConfiguration.PaddingRemoval));
            
            // display string arrays
			foreach (string s in OutputConfiguration.RotationDisplayString)
			{
				this.charRotation.Items.Add(s);
			}

            // re-populate dropdown
            this.outputConfigurationManager.comboboxPopulate(this.outputConfigurations);
        }

        // output configuration to form
        private void loadOutputConfigurationToForm(OutputConfiguration outputConfig)
        {
            // set flag
            this.loadingOutputConfigurationToForm = true;
            
            // load combo boxes
			this.outputConfigurations.Text = outputConfig.displayName;
			this.paddingHorizontal.SelectedIndex = (int)outputConfig.paddingRemovalHorizontal;
			this.paddingVertical.SelectedIndex = (int)outputConfig.paddingRemovalVertical;
            this.charRotation.SelectedIndex = (int)outputConfig.rotation;

            // text boxes
			this.minCharWidth.Text = outputConfig.minCharacterWidth.ToString();
			this.minCharHeight.Text = outputConfig.minCharacterHeight.ToString();
			this.spaceCharPixels.Text = outputConfig.spaceCharacterWidth.ToString();
			this.interCharPixels.Text = outputConfig.interCharacterPixels.ToString();

            // load check boxes
            this.flipHorizontal.Checked = outputConfig.flipHorizontal;
            this.flipVertical.Checked = outputConfig.flipVertical;

            // clear flag
            this.loadingOutputConfigurationToForm = false;
        }

        // output configuration to form
        private void loadFormToOutputConfiguration(ref OutputConfiguration outputConfig)
        {
            // load combo boxes
            outputConfig.paddingRemovalHorizontal = (OutputConfiguration.PaddingRemoval)Enum.Parse(typeof(OutputConfiguration.PaddingRemoval), this.paddingHorizontal.Text);
            outputConfig.paddingRemovalVertical = (OutputConfiguration.PaddingRemoval)Enum.Parse(typeof(OutputConfiguration.PaddingRemoval), this.paddingVertical.Text);
            outputConfig.rotation = (OutputConfiguration.Rotation)Array.IndexOf(OutputConfiguration.RotationDisplayString, this.charRotation.Text);

            // text boxes
			outputConfig.minCharacterWidth = System.Convert.ToInt32(this.minCharWidth.Text, 10);
			outputConfig.minCharacterHeight = System.Convert.ToInt32(this.minCharHeight.Text, 10);
			outputConfig.spaceCharacterWidth = System.Convert.ToInt32(this.spaceCharPixels.Text, 10);
			outputConfig.interCharacterPixels = System.Convert.ToInt32(this.interCharPixels.Text, 10);

            // load check boxes
            outputConfig.flipHorizontal = this.flipHorizontal.Checked;
            outputConfig.flipVertical = this.flipVertical.Checked;
        }

        private void setControlTooltip(Control control, string tooltipString)
        {
            ToolTip tooltip = new ToolTip();
            tooltip.SetToolTip(control, tooltipString);
        }

        public OutputConfigurationForm(ref OutputConfigurationManager outputConfigurationManager)
        {
            // set ocm
            this.outputConfigurationManager = outputConfigurationManager;

            this.InitializeComponent();
            this.populateControls();

            // set tooltips
            this.setControlTooltip(btnUpdateConfig, "Save updated config to preset");
            this.setControlTooltip(btnSaveNewConfig, "Save as new preset");
            this.setControlTooltip(btnDeleteConfig, "Delete preset");
        }

        // populate an output configuration
        public int getOutputConfiguration(int displayedOutputConfigurationIndex)
        {
            // load no preset
            this.outputConfigurations.SelectedIndex = -1;

            // can never be modifying preset when just displayed
            this.modifyingPresetConfigurationExit();
            
            // check if we need to display an OC
            if (displayedOutputConfigurationIndex != -1)
            {
                // get the configuration
                OutputConfiguration oc = this.outputConfigurationManager.configurationGetAtIndex(displayedOutputConfigurationIndex);

                // copy the object from the repository to the working copy
                this.outputConfigurationManager.workingOutputConfiguration = oc.clone();
            }
            else
            {
                // clear out display name so that when this is loaded into cbx it doesn't
                // select a preset
                this.outputConfigurationManager.workingOutputConfiguration.displayName = "";
            }

            // set index in cbx
            this.outputConfigurations.SelectedIndex = displayedOutputConfigurationIndex;
            
            // load the configuration of the working output configuration
            this.loadOutputConfigurationToForm(this.outputConfigurationManager.workingOutputConfiguration);

            // show self
            this.ShowDialog();

            // load current state of form to working output configuration
            this.loadFormToOutputConfiguration(ref this.outputConfigurationManager.workingOutputConfiguration);

            // are we in modifying state?
            if (this.presetConfigurationModified == false)
            {
                // nope, simply return the preset index
                return this.outputConfigurations.SelectedIndex;
            }
            else
            {
                // user modified a preset and didn't save - switch back to no preset
                return -1;
            }
        }

        // 
        public int getDisplayStringIndex(ref string[] displayStrings, string selectedText)
        {
            return 0;
        }

		#endregion

		#region Event Handlers

		private void button1_Click(object sender, EventArgs e)
        {
            // close self
            Close();
        }

		private void outputConfigurations_SelectedIndexChanged(object sender, EventArgs e)
		{
			// check that we haven't reverted to no selection
			if (this.outputConfigurations.SelectedIndex != -1)
			{
				// get the configuration
				OutputConfiguration oc = this.outputConfigurationManager.configurationGetAtIndex(this.outputConfigurations.SelectedIndex);

				// copy the object from the repository to the working copy
				this.outputConfigurationManager.workingOutputConfiguration = oc.clone();

				// load to form
				loadOutputConfigurationToForm(this.outputConfigurationManager.workingOutputConfiguration);
			}
		}

		private void btnUpdateConfig_Click(object sender, EventArgs e)
		{
			// no focus
			this.gbxPadding.Focus();

			// exit modifying
			this.modifyingPresetConfigurationExit();

			// get the configuration reference at index
			OutputConfiguration updatedOutputConfiguration = this.outputConfigurationManager.configurationGetAtIndex(this.outputConfigurations.SelectedIndex);

			// load current form to the configuration
			this.loadFormToOutputConfiguration(ref updatedOutputConfiguration);

			// re-save 
			this.outputConfigurationManager.saveToFile(Properties.Resources.ConfigFileName);
		}

        private void btnSaveNewConfig_Click(object sender, EventArgs e)
        {
            // no focus
            this.gbxPadding.Focus();
            
            // exit modifying
            this.modifyingPresetConfigurationExit();
            
            // get name of new configuration
            InputBoxDialog ib = new InputBoxDialog();
                ib.FormPrompt = "Enter preset name";
                ib.FormCaption = "New preset configuration";
                ib.DefaultValue = "";

            // show the dialog
            if (ib.ShowDialog() == DialogResult.OK)
            {
                // close dialog
                ib.Close();

                // create a new output configuration
                OutputConfiguration oc = new OutputConfiguration();

                // load current form to config
                this.loadFormToOutputConfiguration(ref oc);

                // set display name
                oc.displayName = ib.InputResponse;

                // save new configuration to end of list
                this.outputConfigurationManager.configurationAdd(ref oc);

                // re-populate dropdown
                this.outputConfigurationManager.comboboxPopulate(this.outputConfigurations);

                // set selected index
                this.outputConfigurations.SelectedIndex = this.outputConfigurations.Items.Count    - 1;

                // re-save 
                this.outputConfigurationManager.saveToFile(Properties.Resources.ConfigFileName);
            }
        }

        private void btnDeleteConfig_Click(object sender, EventArgs e)
        {
            // no focus
            this.gbxPadding.Focus();

            // remove current 
            this.outputConfigurationManager.configurationDelete(this.outputConfigurations.SelectedIndex);

            // re-populate dropdown
            this.outputConfigurationManager.comboboxPopulate(this.outputConfigurations);

            // re-save 
            this.outputConfigurationManager.saveToFile(Properties.Resources.ConfigFileName);

            // check if any configurations left in manager
            if (this.outputConfigurationManager.configurationCountGet() > 0)
            {
                // just get the first
                OutputConfiguration oc = this.outputConfigurationManager.configurationGetAtIndex(0);

                // to form
                this.loadOutputConfigurationToForm(oc);
            }
            else
            {
                // clear text
                this.outputConfigurations.Text = "";
            }
        }

        // enter modifying state
        private void modifyingPresetConfigurationEnter()
        {
            // enter modified state
            this.presetConfigurationModified = true;

            // enable edit button, to allow user to modify his changes
            this.btnUpdateConfig.Enabled = true;

            // update name of preset to indicate modified
            this.outputConfigurations.Font = new Font(this.outputConfigurations.Font, FontStyle.Italic);
        }

        // exit modifying state
        private void modifyingPresetConfigurationExit()
        {
            // enter modified state
            this.presetConfigurationModified = false;

            // enable edit button, to allow user to modify his changes
            this.btnUpdateConfig.Enabled = false;

            // update name of preset to indicate modified
            this.outputConfigurations.Font = new Font(this.outputConfigurations.Font, FontStyle.Regular);
        }

        private void onOutputConfigurationFormChange(object sender, EventArgs e)
        {
            // check if a preset is selected
            if (this.loadingOutputConfigurationToForm == false && this.outputConfigurations.SelectedIndex != -1)
            {
                // when user has changed a preset, enter modifying state
                this.modifyingPresetConfigurationEnter();
            }
		}

		#endregion
	}
}
