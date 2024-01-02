using System;
using System.Threading;

using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;

using GHIElectronics.NETMF.FEZ;

namespace SimonSays
{
    public class Program
    {
        //Declare variables
        public static int[] ActualSolution = new int[100];
        public static int[] UserSolution = new int[100];
        public static int ButtonPressCount = 0;
        public static int Level = 1;
        public static bool gledState = false;
        public static bool yledState = false;
        public static bool rledState = false;
        public static bool[] SERPinState = new bool[8];

        //Declare ports and use
        public static OutputPort gled = new OutputPort((Cpu.Pin)FEZ_Pin.Digital.Di20, gledState);
        public static OutputPort yled = new OutputPort((Cpu.Pin)FEZ_Pin.Digital.Di21, yledState);
        public static OutputPort rled = new OutputPort((Cpu.Pin)FEZ_Pin.Digital.Di22, rledState);
        public static OutputPort SER_Pin = new OutputPort((Cpu.Pin)FEZ_Pin.Digital.Di50, false); //pin 14 on the 75HC595
        public static OutputPort RCLK_Pin = new OutputPort((Cpu.Pin)FEZ_Pin.Digital.Di51, false);  //pin 12 on the 75HC595
        public static OutputPort SRCLK_Pin = new OutputPort((Cpu.Pin)FEZ_Pin.Digital.Di52, false); //pin 11 on the 75HC595
        public static InterruptPort gbutton = new InterruptPort((Cpu.Pin)FEZ_Pin.Interrupt.Di40, true, Port.ResistorMode.PullUp, Port.InterruptMode.InterruptEdgeHigh);
        public static InterruptPort ybutton = new InterruptPort((Cpu.Pin)FEZ_Pin.Interrupt.Di41, true, Port.ResistorMode.PullUp, Port.InterruptMode.InterruptEdgeHigh);
        public static InterruptPort rbutton = new InterruptPort((Cpu.Pin)FEZ_Pin.Interrupt.Di42, true, Port.ResistorMode.PullUp, Port.InterruptMode.InterruptEdgeHigh);

        public static void Main()
        {
            //Assign interrupts
            gbutton.OnInterrupt += new NativeEventHandler(gbutton_OnInterrupt);
            ybutton.OnInterrupt += new NativeEventHandler(ybutton_OnInterrupt);
            rbutton.OnInterrupt += new NativeEventHandler(rbutton_OnInterrupt);

            ClearRegister();

            Display(3);
            Thread.Sleep(1000);
            ClearRegister();
            Thread.Sleep(250);
            Display(2);
            Thread.Sleep(1000);

            /*
            ClearRegister();
            Thread.Sleep(250);
            Display(1);

            
            Thread.Sleep(1000);
            ClearRegister();
            Thread.Sleep(250);

   
            //start the game
            StartGame();

            //Wait for player to do something
            Thread.Sleep(Timeout.Infinite);
             * */
        }

        static void StartGame()
        {
            //Build the random sequence
            int i;
            Random Rnd = new Random();

            for (i = 0; i < Level; i++)
            {
                ActualSolution[i] = Rnd.Next(3);
            }

            //Display Level
            Display(Level);

            //Show User the Sequence
            for (i = 0; i < Level; i++)
            {
                IlluminateLED(ActualSolution[i]);
                Thread.Sleep(500);
                //we need to reset back to no values
                IlluminateLED(3);
                Thread.Sleep(500);
            }

            //Reset Counters
            ButtonPressCount = 0;

            //Tell user they can start with one flash
            IlluminateLED(4);
            Thread.Sleep(250);
            IlluminateLED(3);


        }

        static void EndGame()
        {
            IlluminateLED(1);
            Thread.Sleep(Timeout.Infinite);
        }

        static void gbutton_OnInterrupt(uint port, uint state, DateTime time)
        {
            UserSolution[ButtonPressCount] = 0;
            ButtonPressCount = ButtonPressCount + 1;
            CheckSequence();
        }

        static void ybutton_OnInterrupt(uint port, uint state, DateTime time)
        {
            UserSolution[ButtonPressCount] = 1;
            ButtonPressCount = ButtonPressCount + 1;
            CheckSequence();
        }

        static void rbutton_OnInterrupt(uint port, uint state, DateTime time)
        {
                UserSolution[ButtonPressCount] = 2;
                ButtonPressCount = ButtonPressCount + 1;
                CheckSequence();
        }

        public static void CheckSequence()
        {
            if (ButtonPressCount == Level)
            {
               int i;
               for (i = 0; i < Level; i++)
                   if (ActualSolution[i] == UserSolution[i])
                   {
                       IlluminateLED(0);
                   }
                    else
                   {
                       IlluminateLED(2);
                       i = Level;
                       EndGame();
                   }
               Thread.Sleep(2000);
               IlluminateLED(4);
               Thread.Sleep(250);
               IlluminateLED(3);
               Thread.Sleep(2000);
               //Next Level
               Level = Level + 1;
               StartGame();
            }
        }

        public static void IlluminateLED(int IlluminateLEDOption)
        {
            //All off
            if (IlluminateLEDOption == 3)
            {
                gled.Write(false);
                yled.Write(false);
                rled.Write(false);
            }
            //All On
            if (IlluminateLEDOption == 4)
            {
                gled.Write(true);
                yled.Write(true);
                rled.Write(true);
            }
            //Green Only On
            if (IlluminateLEDOption == 0)
            {
                gled.Write(true);
                yled.Write(false);
                rled.Write(false);
            }
            //Yellow Only On
            if (IlluminateLEDOption == 1)
            {
                gled.Write(false);
                yled.Write(true);
                rled.Write(false);
            }
            //Red Only On
            if (IlluminateLEDOption == 2)
            {
                gled.Write(false);
                yled.Write(false);
                rled.Write(true);
            }
        }

        static void ClearRegister()
        {
            for (int i = 0; i < 7; i++)
            {
                SERPinState[i] = false;
            }
        }

        static void WriteRegister()
        {
            // Prepare display data
            RCLK_Pin.Write(false);
            //write data
            for (int i = 0; i < 7; i++)
            {
                //Prepare to accept data
                SRCLK_Pin.Write(false);
                //data
                SER_Pin.Write(SERPinState[i]);
                //accept Data
                SRCLK_Pin.Write(true);
            }
            //
            //Display Data
            RCLK_Pin.Write(true);
        }

        static void Display(int DisplayOption)
        {
    
            //Display 0
            if (DisplayOption == 0)
            {
                SERPinState[2] = true;
                SERPinState[3] = true;
                SERPinState[4] = true;
                SERPinState[5] = true;
                SERPinState[6] = true;
                SERPinState[7] = true;
            }
            //Display 1
            if (DisplayOption == 1)
            {
                SERPinState[4] = true;
                SERPinState[7] = true;
            }
            //Display 2
            if (DisplayOption == 2)
            {
                SERPinState[1] = true;
                SERPinState[3] = true;
                SERPinState[4] = true;
                SERPinState[5] = true;
                SERPinState[6] = true;
            }
            //Display 3
            if (DisplayOption == 3)
            {
                SERPinState[1] = true;
                SERPinState[3] = true;
                SERPinState[4] = true;
                SERPinState[6] = true;
                SERPinState[7] = true;
            }
            //Display 4
            if (DisplayOption == 4)
            {
                SERPinState[1] = true;
                SERPinState[2] = true;
                SERPinState[4] = true;
                SERPinState[7] = true;
            }
            //Display 5
            if (DisplayOption == 5)
            {
                SERPinState[1] = true;
                SERPinState[2] = true;
                SERPinState[3] = true;
                SERPinState[6] = true;
                SERPinState[7] = true;
            }
            //Display 6
            if (DisplayOption == 6)
            {
                SERPinState[1] = true;
                SERPinState[2] = true;
                SERPinState[3] = true;
                SERPinState[5] = true;
                SERPinState[6] = true;
                SERPinState[7] = true;
            }
            //Display 7
            if (DisplayOption == 7)
            {
                SERPinState[3] = true;
                SERPinState[4] = true;
                SERPinState[7] = true;
            }
            //Display 8
            if (DisplayOption == 8)
            {
                SERPinState[1] = true;
                SERPinState[2] = true;
                SERPinState[3] = true;
                SERPinState[4] = true;
                SERPinState[5] = true;
                SERPinState[6] = true;
                SERPinState[7] = true;
            }
            //Display 9
            if (DisplayOption == 9)
            {
                SERPinState[1] = true;
                SERPinState[2] = true;
                SERPinState[3] = true;
                SERPinState[4] = true;
                SERPinState[6] = true;
                SERPinState[7] = true;
            }

            //write 
            WriteRegister();

        }

    }
}
