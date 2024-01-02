using System;
using System.Threading;
using Microsoft.SPOT;
using GHIElectronics.NETMF.FEZ;


namespace HouseWatch
{
    class MenuPage1
    {
        public static void DrawMenuPage1()
        {
            //initialise graphics
            Graphics.InitGraphics();

            Graphics.CheckBox Menu_AlarmStatus = new Graphics.CheckBox(10, 10, Graphics.fezTouch, "Alarm Armed");
            Graphics.CheckBox Menu_Zone1Status = new Graphics.CheckBox(30, 40, Graphics.fezTouch, "Zone 1 Active");
            Graphics.CheckBox Menu_Zone2Status = new Graphics.CheckBox(30, 70, Graphics.fezTouch, "Zone 2 Active");
            Graphics.CheckBox Menu_Zone3Status = new Graphics.CheckBox(30, 100, Graphics.fezTouch, "Zone 3 Active");
            Graphics.CheckBox Menu_Zone4Status = new Graphics.CheckBox(30, 130, Graphics.fezTouch, "Zone 4 Active");
            Graphics.CheckBox Menu_SendTestStatus = new Graphics.CheckBox(10, 175, Graphics.fezTouch, "SMS");
            Graphics.MomentaryButton Menu_SetTextNumbers = new Graphics.MomentaryButton(130, 165, Graphics.fezTouch, 100, 45, FEZ_Components.FEZTouch.Color.LightSeaGreen, FEZ_Components.FEZTouch.Color.Teal, "Numbers");
            Graphics.MomentaryButton Menu_SetPasscode = new Graphics.MomentaryButton(10, 220, Graphics.fezTouch, 100, 45, FEZ_Components.FEZTouch.Color.LightSeaGreen, FEZ_Components.FEZTouch.Color.Teal, "Set Code");
            Graphics.MomentaryButton Menu_SetTime = new Graphics.MomentaryButton(130, 220, Graphics.fezTouch, 100, 45, FEZ_Components.FEZTouch.Color.LightSeaGreen, FEZ_Components.FEZTouch.Color.Teal, "Set Time");
            
            //Save
            Graphics.MomentaryButton Menu_Save = new Graphics.MomentaryButton(130, 275, Graphics.fezTouch, 100, 45, FEZ_Components.FEZTouch.Color.LightSeaGreen, FEZ_Components.FEZTouch.Color.Teal, "Save");
            Menu_Save.Released += new Graphics.MomentaryButton.ButtonReleasedDelegate(Menu_Save_btn_Released);
        }

        static void Menu_Save_btn_Released()
        {
            Graphics.fezTouch.Dispose();
            Keypad.DrawKeypad(true, "");
         }
    }
}
