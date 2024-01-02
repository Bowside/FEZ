using System;
using System.Threading;

using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;

using GHIElectronics.NETMF.FEZ;

namespace Shift_Register
{
    public class Program
    {
        public static bool[] SERPinState = new bool[8];
        public static OutputPort SER_Pin = new OutputPort((Cpu.Pin)FEZ_Pin.Digital.Di8, false); //pin 14 on the 75HC595
        public static OutputPort RCLK_Pin = new OutputPort((Cpu.Pin)FEZ_Pin.Digital.Di9, false);  //pin 12 on the 75HC595
        public static OutputPort SRCLK_Pin = new OutputPort((Cpu.Pin)FEZ_Pin.Digital.Di10, false); //pin 11 on the 75HC595

        public static void Main()
        {
            ClearRegister();
 
            Alternate();
           
 
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


        static void Alternate()
        {
            while (true) 
            {

            SERPinState[0] = true;
            SERPinState[1] = false;
            SERPinState[2] = true;
            SERPinState[3] = false;
            SERPinState[4] = true;
            SERPinState[5] = false;
            SERPinState[6] = true;
            SERPinState[7] = false;

            WriteRegister();
            Thread.Sleep(500);

            SERPinState[0] = false;
            SERPinState[1] = true;
            SERPinState[2] = false;
            SERPinState[3] = true;
            SERPinState[4] = false;
            SERPinState[5] = true;
            SERPinState[6] = false;
            SERPinState[7] = true;

            WriteRegister();
            Thread.Sleep(500);
            }



        }


    }
}
