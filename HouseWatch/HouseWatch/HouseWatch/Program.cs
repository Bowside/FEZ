using System;
using System.IO;
using System.Threading;
using System.IO.Ports;
using System.Text;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;
using Microsoft.SPOT.IO;
using GHIElectronics.NETMF.FEZ;
using GHIElectronics.NETMF.Hardware;
using GHIElectronics.NETMF.IO;


namespace HouseWatch
{
    class Program
    {
        //Set-up Sensor ports
        public static InterruptPort Sensor_Motion1 = new InterruptPort((Cpu.Pin)FEZ_Pin.Interrupt.Di37, true, Port.ResistorMode.Disabled, Port.InterruptMode.InterruptEdgeLow);
        public static InterruptPort Sensor_Motion2 = new InterruptPort((Cpu.Pin)FEZ_Pin.Interrupt.Di39, true, Port.ResistorMode.Disabled, Port.InterruptMode.InterruptEdgeLow);
        public static InterruptPort Sensor_Motion3 = new InterruptPort((Cpu.Pin)FEZ_Pin.Interrupt.Di43, true, Port.ResistorMode.Disabled, Port.InterruptMode.InterruptEdgeLow);
        public static InterruptPort Sensor_Motion4 = new InterruptPort((Cpu.Pin)FEZ_Pin.Interrupt.Di41, true, Port.ResistorMode.Disabled, Port.InterruptMode.InterruptEdgeLow);
        public static InterruptPort Sensor_Motion5 = new InterruptPort((Cpu.Pin)FEZ_Pin.Interrupt.Di30, true, Port.ResistorMode.Disabled, Port.InterruptMode.InterruptEdgeLow);
        public static InterruptPort Sensor_BoxCover = new InterruptPort((Cpu.Pin)FEZ_Pin.Interrupt.Di32, true, Port.ResistorMode.Disabled, Port.InterruptMode.InterruptEdgeLow);

        //Set Up board LED and switch it off
        public static OutputPort LED = new OutputPort((Cpu.Pin)FEZ_Pin.Digital.LED, false);

        //Set-up Memory card
        static PersistentStorage ps;

        //Set-up variables
        public static bool bool_AlarmActive;
        public static bool bool_IsTimeSet = false;
        public static string str_ErrorText = "";
        

        //public static

        public static void Main()
        {
        // Load up variables from memory card
        // Assumes SD card is inserted
            // Create a new storage device
            ps = new PersistentStorage("SD");
            ps.MountFileSystem();

            // Subscribe to RemovableMedia events
            //RemovableMedia.Insert += RemovableMedia_Insert;
            //RemovableMedia.Eject += RemovableMedia_Eject;

            CheckTime();

            Keypad.DrawKeypad(false, str_ErrorText);


            // Sleep forever
            Thread.Sleep(Timeout.Infinite);

        }

        static void CheckTime()
        {
            //Check whether time is set
            byte[] SetTimeFlag_helper = new byte[4];

            if (SetTimeFlag_helper[0] == 1)
                bool_IsTimeSet = true;

            if (bool_IsTimeSet)// The time and date are still set correctly in the Real Time Clock and we can use it in our application
            {
                // Set system's clock
                Utility.SetLocalTime(RealTimeClock.GetTime());
            }
            else
            {
                str_ErrorText = "Date/Time not set";
            }


            //RealTimeClock.SetTime(new DateTime(2012, 04, 12, 9, 50, 00, 0));
            //Utility.SetLocalTime(RealTimeClock.GetTime());
            // We will save a flag in RTC Battery RAM indicating that we set the time
        }


        static void Sensor_Motion1_OnInterrupt(uint port, uint state, DateTime time)
        {
            //GPRS.GPRS seed = new GPRS.GPRS();
            //seed.SendSMS("+27723000206", "Alarm are triggered");
            //seed.placeCall("+27" + KeypadEntry); // Replace this with your number
            Debug.Print("Test");
        }

        static void RemovableMedia_Insert(object sender, MediaEventArgs e)
        {
            //e.Volume.Format(0, true);
            Debug.Print("Storage \"" + e.Volume.RootDirectory + "\" is inserted.");
            Debug.Print("Getting files and folders:");
            if (e.Volume.IsFormatted)
            {
                //string[] files = Directory.GetFiles(e.Volume.RootDirectory);
                //string[] folders = Directory.GetDirectories(e.Volume.RootDirectory);

                //Debug.Print("Files available on " + e.Volume.RootDirectory + ":");
                //for (int i = 0; i < files.Length; i++)
                //    Debug.Print(files[i]);

                //Debug.Print("Folders available on " + e.Volume.RootDirectory + ":");
                //for (int i = 0; i < folders.Length; i++)
                //    Debug.Print(folders[i]);
                // Read configuration file.
                //http://mikedodaro.net/2012/03/25/saving-settings-to-xml-configuration-files/
                string filePath = "Application.config";
                using (Stream configStream = File.OpenRead(filePath))
                {
                    // Loads settings.
                    ConfigurationManager.Load(configStream);
                }

                // Retrieve some values.
                var host = ConfigurationManager.GetAppSetting("hostName");
                var port = ConfigurationManager.GetAppSetting("port");
                var userName = ConfigurationManager.GetAppSetting("userName", "anonymous");

                Debug.Print("HostName: " + host);
                Debug.Print("Port: " + port);
                Debug.Print("Username: " + userName);
            }
            else
            {
                Debug.Print("Storage is not formatted. Format on PC with FAT32/FAT16 first.");
            }

            // We do not need it any more
            ps.Dispose();
        }

        static void RemovableMedia_Eject(object sender, MediaEventArgs e)
        {
            Debug.Print("Storage \"" + e.Volume.RootDirectory + "\" is ejected.");
        }


        }
    }

      

    

