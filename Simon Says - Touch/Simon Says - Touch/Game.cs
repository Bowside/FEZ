using System;
using System.Threading;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;
using GHIElectronics.NETMF.FEZ;
using GHIElectronics.NETMF.Hardware;
//using System.IO.Ports;
using System.Text;

namespace Simon_Says
{
    public class Game
    {
        private static string var_ActualPassword;
        private static string var_EnteredPassword;
        private static Button pad_0;
        private static Button pad_1;
        private static Button pad_2;
        private static Button pad_3;



        public static void Main()
        {
            Graphics.InitGraphics();
            Graphics.TextArea myText = new Graphics.TextArea(0, 0, Graphics.fezTouch.ScreenWidth, Graphics.fezTouch.ScreenHeight, Graphics.fezTouch);
            Graphics.fezTouch.TouchUpEvent += new FEZ_Components.FEZTouch.TouchEventHandler(fezTouch_TouchUpEvent);

            myText.WriteLine("I'd like to play a game");
            myText.WriteLine("That is so much fun,");
            myText.WriteLine("And it's not so very hard to do,");
            myText.WriteLine("The name of the game is Simple Simon says,");
            myText.WriteLine("And I would like for you to play it too...");
        
            
            //DrawForm();  
        }
        public static void DrawForm()
        {
            //Clear screen
            Graphics.fezTouch.ClearScreen();

            //1
            Graphics.MomentaryButton pad_1 = new Graphics.MomentaryButton(20, 160, Graphics.fezTouch, 50, 50, FEZ_Components.FEZTouch.Color.LightSeaGreen, FEZ_Components.FEZTouch.Color.Teal, "1");
            pad_1.Released += new Graphics.MomentaryButton.ButtonReleasedDelegate(pad_1_btn_Released);

            //2
            Graphics.MomentaryButton pad_2 = new Graphics.MomentaryButton(90, 160, Graphics.fezTouch, 50, 50, FEZ_Components.FEZTouch.Color.LightSeaGreen, FEZ_Components.FEZTouch.Color.Teal, "2");
            pad_2.Released += new Graphics.MomentaryButton.ButtonReleasedDelegate(pad_2_btn_Released);

            //3
            Graphics.MomentaryButton pad_3 = new Graphics.MomentaryButton(160, 160, Graphics.fezTouch, 50, 50, FEZ_Components.FEZTouch.Color.LightSeaGreen, FEZ_Components.FEZTouch.Color.Teal, "3");
            pad_3.Released += new Graphics.MomentaryButton.ButtonReleasedDelegate(pad_3_btn_Released);

            //0
            Graphics.MomentaryButton pad_0 = new Graphics.MomentaryButton(20, 230, Graphics.fezTouch, 120, 50, FEZ_Components.FEZTouch.Color.LightSeaGreen, FEZ_Components.FEZTouch.Color.Teal, "0");
            pad_0.Released += new Graphics.MomentaryButton.ButtonReleasedDelegate(pad_0_btn_Released);
        }

        static void pad_0_btn_Released()
        {
            var_EnteredPassword = var_EnteredPassword + "0";
        }
        static void pad_1_btn_Released()
        {
            var_EnteredPassword = var_EnteredPassword + "1";
        }
        static void pad_2_btn_Released()
        {
            var_EnteredPassword = var_EnteredPassword + "2";
        }
        static void pad_3_btn_Released()
        {
            var_EnteredPassword = var_EnteredPassword + "3";
        }
  
        static void pad_enter_btn_Released()
        {
            if (var_ActualPassword == var_EnteredPassword)
            {
                //Complete delete the cache
                Graphics.fezTouch.Dispose();
                //start affresh
                Graphics.InitGraphics();
                Graphics.Label mylabel = new Graphics.Label(10, 10, Graphics.fezTouch, "Correct");
            }
            var_EnteredPassword = "";

            //GPRS.GPRS seed = new GPRS.GPRS();
            //seed.SendSMS("+27824805221", "TEST 1234556");
            ////seed.placeCall("+27" + var_EnteredPassword); // Replace this with your number
            //Thread.Sleep(Timeout.Infinite);
        }
        static void fezTouch_TouchUpEvent(int x, int y)
        {
            Graphics.fezTouch.ClearScreen();
        }

    }
}
