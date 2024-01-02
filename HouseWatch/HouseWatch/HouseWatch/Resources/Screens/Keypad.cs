using System;
using System.Threading;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;
using GHIElectronics.NETMF.FEZ;
using GHIElectronics.NETMF.Hardware;


namespace HouseWatch
{
    class Keypad
    {
        //Declare variables
        public static Graphics.Label KeypadEntry;
        public static bool bool_IsKeypadDialler;
        public static bool bool_IsTimeSet = false;
        const int IDLE_TIME = 10000;
        static int idleTimer;
        static FEZ_Components.FEZTouch.DisplayMode displayMode;

        public static void DrawKeypad(bool bool_IsKeyPadDialler, string str_Error)
        {
            bool_IsKeypadDialler = bool_IsKeyPadDialler;

            //initialise graphics
            Graphics.InitGraphics();

            //Draw Error Label
            Graphics.Label Keypad_error = new Graphics.Label(10, 10, Graphics.fezTouch, str_Error);

            //Draw entry label
            KeypadEntry = new Graphics.Label(10, 35, Graphics.fezTouch, "");
            KeypadEntry.Text = "";

            //Draw button Pad
            //7
            Graphics.MomentaryButton Keypad_7 = new Graphics.MomentaryButton(20, 60, Graphics.fezTouch, 50, 50, FEZ_Components.FEZTouch.Color.LightSeaGreen, FEZ_Components.FEZTouch.Color.Teal, "7");
            Keypad_7.Released += new Graphics.MomentaryButton.ButtonReleasedDelegate(Keypad_7_btn_Released);

            //8
            Graphics.MomentaryButton Keypad_8 = new Graphics.MomentaryButton(90, 60, Graphics.fezTouch, 50, 50, FEZ_Components.FEZTouch.Color.LightSeaGreen, FEZ_Components.FEZTouch.Color.Teal, "8");
            Keypad_8.Released += new Graphics.MomentaryButton.ButtonReleasedDelegate(Keypad_8_btn_Released);

            //9
            Graphics.MomentaryButton Keypad_9 = new Graphics.MomentaryButton(160, 60, Graphics.fezTouch, 50, 50, FEZ_Components.FEZTouch.Color.LightSeaGreen, FEZ_Components.FEZTouch.Color.Teal, "9");
            Keypad_9.Released += new Graphics.MomentaryButton.ButtonReleasedDelegate(Keypad_9_btn_Released);

            //4
            Graphics.MomentaryButton Keypad_4 = new Graphics.MomentaryButton(20, 130, Graphics.fezTouch, 50, 50, FEZ_Components.FEZTouch.Color.LightSeaGreen, FEZ_Components.FEZTouch.Color.Teal, "4");
            Keypad_4.Released += new Graphics.MomentaryButton.ButtonReleasedDelegate(Keypad_4_btn_Released);

            //5
            Graphics.MomentaryButton Keypad_5 = new Graphics.MomentaryButton(90, 130, Graphics.fezTouch, 50, 50, FEZ_Components.FEZTouch.Color.LightSeaGreen, FEZ_Components.FEZTouch.Color.Teal, "5");
            Keypad_5.Released += new Graphics.MomentaryButton.ButtonReleasedDelegate(Keypad_5_btn_Released);

            //6
            Graphics.MomentaryButton Keypad_6 = new Graphics.MomentaryButton(160, 130, Graphics.fezTouch, 50, 50, FEZ_Components.FEZTouch.Color.LightSeaGreen, FEZ_Components.FEZTouch.Color.Teal, "6");
            Keypad_6.Released += new Graphics.MomentaryButton.ButtonReleasedDelegate(Keypad_6_btn_Released);

            //1
            Graphics.MomentaryButton Keypad_1 = new Graphics.MomentaryButton(20, 200, Graphics.fezTouch, 50, 50, FEZ_Components.FEZTouch.Color.LightSeaGreen, FEZ_Components.FEZTouch.Color.Teal, "1");
            Keypad_1.Released += new Graphics.MomentaryButton.ButtonReleasedDelegate(Keypad_1_btn_Released);

            //2
            Graphics.MomentaryButton Keypad_2 = new Graphics.MomentaryButton(90, 200, Graphics.fezTouch, 50, 50, FEZ_Components.FEZTouch.Color.LightSeaGreen, FEZ_Components.FEZTouch.Color.Teal, "2");
            Keypad_2.Released += new Graphics.MomentaryButton.ButtonReleasedDelegate(Keypad_2_btn_Released);

            //3
            Graphics.MomentaryButton Keypad_3 = new Graphics.MomentaryButton(160, 200, Graphics.fezTouch, 50, 50, FEZ_Components.FEZTouch.Color.LightSeaGreen, FEZ_Components.FEZTouch.Color.Teal, "3");
            Keypad_3.Released += new Graphics.MomentaryButton.ButtonReleasedDelegate(Keypad_3_btn_Released);

            //0
            Graphics.MomentaryButton Keypad_0 = new Graphics.MomentaryButton(20, 270, Graphics.fezTouch, 50, 50, FEZ_Components.FEZTouch.Color.LightSeaGreen, FEZ_Components.FEZTouch.Color.Teal, "0");
            Keypad_0.Released += new Graphics.MomentaryButton.ButtonReleasedDelegate(Keypad_0_btn_Released);

            //Clear
            Graphics.MomentaryButton Keypad_clear = new Graphics.MomentaryButton(90, 270, Graphics.fezTouch, 50, 50, FEZ_Components.FEZTouch.Color.LightSeaGreen, FEZ_Components.FEZTouch.Color.Teal, "Clr");
            Keypad_clear.Released += new Graphics.MomentaryButton.ButtonReleasedDelegate(Keypad_clear_btn_Released);

            //Enter
            Graphics.MomentaryButton Keypad_enter = new Graphics.MomentaryButton(160, 270, Graphics.fezTouch, 50, 50, FEZ_Components.FEZTouch.Color.LightSeaGreen, FEZ_Components.FEZTouch.Color.Teal, "Etr");
            Keypad_enter.Released += new Graphics.MomentaryButton.ButtonReleasedDelegate(Keypad_enter_btn_Released);

            // dim, and then turn off, the display if not interrupted within idleTime
            displayMode = FEZ_Components.FEZTouch.DisplayMode.Normal;
            idleTimer = IDLE_TIME;
            while (true)
            {
                switch (displayMode)
                {
                    case FEZ_Components.FEZTouch.DisplayMode.Normal:
                        Graphics.fezTouch.SetDisplayMode(displayMode);
                        displayMode = FEZ_Components.FEZTouch.DisplayMode.Dim;
                        break;
                    case FEZ_Components.FEZTouch.DisplayMode.Dim:
                        Graphics.fezTouch.SetDisplayMode(displayMode);
                        displayMode = FEZ_Components.FEZTouch.DisplayMode.Off;
                        break;
                    case FEZ_Components.FEZTouch.DisplayMode.Off:
                        Graphics.fezTouch.SetDisplayMode(displayMode);
                        displayMode = FEZ_Components.FEZTouch.DisplayMode.Off;
                        
                        break;
                }
                Thread.Sleep(idleTimer);
            }
        }

        static void Keypad_0_btn_Released()
        {
            if (displayMode != FEZ_Components.FEZTouch.DisplayMode.Off)
            {
                KeypadEntry.Text = KeypadEntry.Text.ToString() + "0";
            }
            displayMode = FEZ_Components.FEZTouch.DisplayMode.Normal;
            Graphics.fezTouch.SetDisplayMode(displayMode);
            
        }
        static void Keypad_1_btn_Released()
        {
            if (displayMode != FEZ_Components.FEZTouch.DisplayMode.Off)
            {
                KeypadEntry.Text = KeypadEntry.Text.ToString() + "1";
            }
            displayMode = FEZ_Components.FEZTouch.DisplayMode.Normal;
            Graphics.fezTouch.SetDisplayMode(displayMode);
        }
        static void Keypad_2_btn_Released()
        {
            if (displayMode != FEZ_Components.FEZTouch.DisplayMode.Off)
            {
                KeypadEntry.Text = KeypadEntry.Text.ToString() + "2";
            }
            displayMode = FEZ_Components.FEZTouch.DisplayMode.Normal;
            Graphics.fezTouch.SetDisplayMode(displayMode);
        }
        static void Keypad_3_btn_Released()
        {
            if (displayMode != FEZ_Components.FEZTouch.DisplayMode.Off)
            {
                KeypadEntry.Text = KeypadEntry.Text.ToString() + "3";
            }
            displayMode = FEZ_Components.FEZTouch.DisplayMode.Normal;
            Graphics.fezTouch.SetDisplayMode(displayMode);
        }
        static void Keypad_4_btn_Released()
        {
            if (displayMode != FEZ_Components.FEZTouch.DisplayMode.Off)
            {
                KeypadEntry.Text = KeypadEntry.Text.ToString() + "4";
            }
            displayMode = FEZ_Components.FEZTouch.DisplayMode.Normal;
            Graphics.fezTouch.SetDisplayMode(displayMode);
        }
        static void Keypad_5_btn_Released()
        {
            if (displayMode != FEZ_Components.FEZTouch.DisplayMode.Off)
            {
                KeypadEntry.Text = KeypadEntry.Text.ToString() + "5";
            }
            displayMode = FEZ_Components.FEZTouch.DisplayMode.Normal;
            Graphics.fezTouch.SetDisplayMode(displayMode);
        }
        static void Keypad_6_btn_Released()
        {
            if (displayMode != FEZ_Components.FEZTouch.DisplayMode.Off)
            {
                KeypadEntry.Text = KeypadEntry.Text.ToString() + "6";
            }
            displayMode = FEZ_Components.FEZTouch.DisplayMode.Normal;
            Graphics.fezTouch.SetDisplayMode(displayMode);
        }
        static void Keypad_7_btn_Released()
        {
            if (displayMode != FEZ_Components.FEZTouch.DisplayMode.Off)
            {
                KeypadEntry.Text = KeypadEntry.Text.ToString() + "7";
            }
            displayMode = FEZ_Components.FEZTouch.DisplayMode.Normal;
            Graphics.fezTouch.SetDisplayMode(displayMode);
        }
        static void Keypad_8_btn_Released()
        {
            if (displayMode != FEZ_Components.FEZTouch.DisplayMode.Off)
            {
                KeypadEntry.Text = KeypadEntry.Text.ToString() + "8";
            }
            displayMode = FEZ_Components.FEZTouch.DisplayMode.Normal;
            Graphics.fezTouch.SetDisplayMode(displayMode);
        }
        static void Keypad_9_btn_Released()
        {
            if (displayMode != FEZ_Components.FEZTouch.DisplayMode.Off)
            {
                KeypadEntry.Text = KeypadEntry.Text.ToString() + "9";
            }
            displayMode = FEZ_Components.FEZTouch.DisplayMode.Normal;
            Graphics.fezTouch.SetDisplayMode(displayMode);
        }
        static void Keypad_clear_btn_Released()
        {
            if (displayMode != FEZ_Components.FEZTouch.DisplayMode.Off)
            {
                KeypadEntry.Text = "";
            }
            displayMode = FEZ_Components.FEZTouch.DisplayMode.Normal;
            Graphics.fezTouch.SetDisplayMode(displayMode);
        }
        static void Keypad_enter_btn_Released()
        {
            displayMode = FEZ_Components.FEZTouch.DisplayMode.Normal;
            Graphics.fezTouch.SetDisplayMode(displayMode);
            if (bool_IsKeypadDialler == false)
            {
                //We should check the password here
                Graphics.fezTouch.Dispose();
                MenuPage1.DrawMenuPage1();
            }
            else
            {
                //No need to check password
                Graphics.fezTouch.Dispose();
                MenuPage1.DrawMenuPage1();
            }
        }
    }
}
