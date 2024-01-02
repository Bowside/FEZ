using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.IO;
using System.Windows.Forms;

namespace FEZTouch.FontGenerator
{
    // an output configuration preset
    public class OutputConfiguration
	{
		#region Enumerations

		// padding removal type
        public enum PaddingRemoval
        {
            None,               // no padding removal
            Tightest,            // remove padding as much as possible, per bitmap
            Fixed               // remove padding as much as the bitmap with least padding
        }

        // rotation
        public enum Rotation
        {
            RotateZero,
            RotateNinety,
            RotateOneEighty,
            RotateTwoSeventy
        }

		#endregion

		#region Constants

		// rotation display string
        public static readonly string[] RotationDisplayString = new string[]
        {
            "0°",
            "90°",
            "180°",
            "270°"
        };

		#endregion

		#region Properties

		// leading strings
		public const string ByteLeadingStringBinary = "0b";
		public const string ByteLeadingStringHex = "0x";

		// rotation
		public Rotation rotation = Rotation.RotateZero;

		// flip
		public bool flipHorizontal = false;
		public bool flipVertical = false;

		// padding removal
		public PaddingRemoval paddingRemovalHorizontal = PaddingRemoval.Fixed;
		public PaddingRemoval paddingRemovalVertical = PaddingRemoval.Tightest;

		// character widths and heights
		public int minCharacterWidth = 0;
		public int minCharacterHeight = 0;
		public int spaceCharacterWidth = 6;
		public int interCharacterPixels = 2;

		// display name
		public string displayName = "";

		#endregion

		#region Methods

		// clone self
        public OutputConfiguration clone() { return (OutputConfiguration)this.MemberwiseClone(); }

		#endregion
    }

    // the output configuration manager
    public class OutputConfigurationManager
	{
		#region Properties

		// a working copy configuration, used for when there are no presets and 
		// during editing
		public OutputConfiguration workingOutputConfiguration = new OutputConfiguration();

		#endregion

		#region Methods

		// add a configuration
        public int configurationAdd(ref OutputConfiguration configToAdd)
        {
            // add to list
           this.outputConfigurationList.Add(configToAdd);

            // return the index of the new item
            return this.outputConfigurationList.Count - 1;
        }

        // delete a configuration
        public void configurationDelete(int configIdxToRemove)
        {
            // check if in bounds
            if (configIdxToRemove >= 0 && configIdxToRemove < configurationCountGet())
            {
                // delete it
                this.outputConfigurationList.RemoveAt(configIdxToRemove);
            }
        }
        
        // get number of configurations
        public int configurationCountGet()
        {
            // get number of items
            return this.outputConfigurationList.Count;
        }

        // get configuration at index
        public OutputConfiguration configurationGetAtIndex(int index)
        {
            // return the configuration
            return this.outputConfigurationList[index];
        }

        // save to file
        public void saveToFile(string fileName)
        {
            // create serailizer and text writer
            XmlSerializer serializer = new XmlSerializer(this.outputConfigurationList.GetType());
            TextWriter textWriter = new StreamWriter(fileName);
            
            // serialize to xml
            serializer.Serialize(textWriter, this.outputConfigurationList);
            
            // close and flush the stream
            textWriter.Close();
        }

        // load from file
        public void loadFromFile(string fileName)
        {
            // create serailizer and text writer
            XmlSerializer serializer = new XmlSerializer(this.outputConfigurationList.GetType());

            // catch exceptions (especially file not found)
            try
            {
                // read text
                TextReader textReader = new StreamReader(fileName);

                // serialize to xml
                this.outputConfigurationList = (List<OutputConfiguration>)serializer.Deserialize(textReader);

                // close and flush the stream
                textReader.Close();
            }
            catch (IOException)
            {
            }
        }

        // populate the cbx
        public void comboboxPopulate(ComboBox combobox)
        {
            // clear all items
            combobox.Items.Clear();

            // iterate through items
            foreach (OutputConfiguration oc in this.outputConfigurationList)
            {
                // get the name
                combobox.Items.Add(oc.displayName);
            }
        }

		#endregion

		#region Member Fields

		// the output configuration
		private List<OutputConfiguration> outputConfigurationList = new List<OutputConfiguration>();

		#endregion
	}
}
