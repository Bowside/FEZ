using System;
using System.Threading;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;
using GHIElectronics.NETMF.FEZ;
using GHIElectronics.NETMF.Hardware;
namespace Servo
{
    public class Program
    {
        // Set a static variable to set the Pulse Width modulation frequency
        static int PWMFrequency = 15000;
        
        // Define two PWM ports to be used
        static PWM pwm1;
        static PWM pwm2;
        static PWM pwm3;

        // Define a reference for a new thread.
        static Thread brightFaderThread;

        public static void Main()
        {
            // Intialize the PWM Pins
            pwm1 = new PWM((PWM.Pin)FEZ_Pin.PWM.Di8);
            pwm2 = new PWM((PWM.Pin)FEZ_Pin.PWM.Di9);
            pwm3 = new PWM((PWM.Pin)FEZ_Pin.PWM.Di10);

            // Initialize the Special Fader Thread Delegate... note not started yet.
            brightFaderThread = new Thread(BrightFaderThread);
            
            // Hook and interupts onto 3 buttons
            InterruptPort Button1 = new InterruptPort((Cpu.Pin)FEZ_Pin.Interrupt.An0, true, Port.ResistorMode.PullUp, Port.InterruptMode.InterruptEdgeBoth);
            InterruptPort Button2 = new InterruptPort((Cpu.Pin)FEZ_Pin.Interrupt.An1, true, Port.ResistorMode.PullUp, Port.InterruptMode.InterruptEdgeBoth);
            InterruptPort Button3 = new InterruptPort((Cpu.Pin)FEZ_Pin.Interrupt.An2, true, Port.ResistorMode.PullUp, Port.InterruptMode.InterruptEdgeBoth);
            InterruptPort ButtonLDR = new InterruptPort((Cpu.Pin)FEZ_Pin.Interrupt.LDR, true, Port.ResistorMode.PullUp, Port.InterruptMode.InterruptEdgeBoth);
            Button1.OnInterrupt += new NativeEventHandler(Button1_onInterrupt);
            Button2.OnInterrupt += new NativeEventHandler(Button2_onInterrupt);
            Button3.OnInterrupt += new NativeEventHandler(Button3_onInterrupt);
            ButtonLDR.OnInterrupt += new NativeEventHandler(ButtonLDR_onInterrupt);

            // Set the startup values for the output ports
            PWMSetMidBright(pwm1);
            PWMSetMidBright(pwm2);
            PWMSetMidBright(pwm3);

            // Sleep the main thread
            Thread.Sleep(Timeout.Infinite);
        }

        static void Button1_onInterrupt(uint port, uint state, DateTime time)
        {
            // If the button was pressed
            if (state==0)
            { 
                //Make both outputs full brightness
                PWMSetFullBright(pwm1);
            }
            else 
            {   // Set us back to midium brightness
                PWMSetMidBright(pwm1);
            }
            
        }

        static void Button2_onInterrupt(uint port, uint state, DateTime time)
        {
            // If the button was pressed
            if (state == 0)
            {
                //Make both outputs full brightness
                PWMSetFullBright(pwm2);
            }
            else
            {   // Set us back to midium brightness
                PWMSetMidBright(pwm2);
            }

        }

        static void Button3_onInterrupt(uint port, uint state, DateTime time)
        {
            // If the button was pressed
            if (state == 0)
            {
                //Make both outputs full brightness
                PWMSetFullBright(pwm3);
            }
            else
            {   // Set us back to midium brightness
                PWMSetMidBright(pwm3);
            }

        }

        static void ButtonLDR_onInterrupt(uint port, uint state, DateTime time)
        {
            PWMBrightFade(pwm1);
        }

        static void PWMSetFullBright(PWM ipwm)
        {
            // Set the passed in PWM object to full duration
            ipwm.Set(PWMFrequency, 100);
        }
        static void PWMSetMidBright(PWM ipwm)
        {
            // Set the passed in PWM object to mid duration
            ipwm.Set(PWMFrequency, 5);

            // If the brightfaderthread is not suspend and the brightfaderthread has been started (not undstarted, easier to deal with)
            if (brightFaderThread.ThreadState != ThreadState.Suspended &&  brightFaderThread.ThreadState != ThreadState.Unstarted)
            {
                    // suspend the fader thread
                    brightFaderThread.Suspend();
            }
          
        }
        static void PWMBrightFade(PWM ipwm)
        {
            // If the thread is not currently started, start it
            if (brightFaderThread.ThreadState == ThreadState.Unstarted)
            {
                // start the thread
                brightFaderThread.Start();
            }
            else
            {
                // Thread was already started, so we can just resume it
                brightFaderThread.Resume();
            }
        }
        static void BrightFaderThread()
        {
            // specify a working duty
            byte duty = 10;
            sbyte dirr = 1;
            
            // loop endlessly
            while (true)
            {
                // Set the Starting duty
                pwm1.Set(PWMFrequency, duty);
                //pwm2.Set(PWMFrequency, duty);
                //pwm3.Set(PWMFrequency, duty);


                // Add the change to the duty
                duty = (byte)(duty + dirr);
                
                // If the duty is greater than 90 or less than 5
                //  set diir to a negative value
                //  thus we start going up, and then end up going down, then back up
                if (duty > 90 || duty < 5)
                { dirr *= -1; }
                
                // Sleep the thread for a bit between each iteration
                Thread.Sleep(10);
            }

        }
    }
}
