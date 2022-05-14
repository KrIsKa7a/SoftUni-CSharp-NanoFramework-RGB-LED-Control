using nanoFramework.Hardware.Esp32;

using System;
using System.Diagnostics;
using System.Device.Pwm;
using System.Threading;

namespace NFApp1
{
    public class Program
    {
        //Before you start examing the code I suggest you looking up the following URLs:
        //https://www.analogictips.com/pulse-width-modulation-pwm/
        //https://learn.sparkfun.com/tutorials/pulse-width-modulation/all

        //Here we configure our PIN numbers on the MCU and the default frequency of the PWM signal
        private const int RED_PIN = 21;
        private const int GREEN_PIN = 22;
        private const int BLUE_PIN = 23;
        private const int DEF_FREQ = 40000;

        public static void Main()
        {
            //Just for debugging tasks
            Debug.WriteLine("Hello from nanoFramework!");

            //Here we initialize the starter values for the duty cycle of PWM for every pin
            //The values should be between 0 to 100%
            float redValue = .95f;
            float greenValue = .00f;
            float blueValue = .00f;

            //Here we configure out PINs that they will be used for PWM
            //Then we need to create a PwmChannel in order to code the process of PWM 
            Configuration.SetPinFunction(RED_PIN, DeviceFunction.PWM1);
            PwmChannel redPin = PwmChannel.CreateFromPin(RED_PIN, DEF_FREQ, redValue);

            Configuration.SetPinFunction(22, DeviceFunction.PWM2);
            PwmChannel greenPin = PwmChannel.CreateFromPin(GREEN_PIN, DEF_FREQ, greenValue);

            Configuration.SetPinFunction(23, DeviceFunction.PWM3);
            PwmChannel bluePin = PwmChannel.CreateFromPin(BLUE_PIN, DEF_FREQ, blueValue);

            //Here we open the channels and we say that we will start doing our PWM
            redPin.Start();
            greenPin.Start();
            bluePin.Start();

            //This is just counter that can adjust the seconds you want your RGB to be on
            int cnt = 1;
            while (cnt <= 50)
            {
                //With this loop we decrease the RED PIN duty cycle
                //Also with this we increaste the BLUE PIN duty cycle
                //In one word - the RGB is going from RED to BLUE with all colours
                while (redValue >= 0.10)
                {
                    //Change the duty cycle variables values
                    redValue -= 0.05f;
                    blueValue += 0.05f;

                    //Pass the new values into the PWM channels
                    redPin.DutyCycle = redValue;
                    bluePin.DutyCycle = blueValue;

                    //Sleeps the thread in order the change to be smooth and not so fast
                    Thread.Sleep(50);
                }

                //Setting variable values to start change from BLUE to GREEN
                redValue = .00f;
                blueValue = .95f;
                greenValue = .00f;

                //With this loop we decrease the BLUE PIN duty cycle
                //Also with this we increaste the GREEN PIN duty cycle
                //In one word - the RGB is going from BLUE to GREEN with all colours
                while (blueValue > 0.10)
                {
                    blueValue -= 0.05f;
                    greenValue += 0.05f;

                    bluePin.DutyCycle = blueValue;
                    greenPin.DutyCycle = greenValue;

                    Thread.Sleep(50);
                }

                //Setting variable values to start change from GREEN to RED (close the colour loop)
                redValue = .00f;
                blueValue = .00f;
                greenValue = .95f;

                //With this loop we decrease the GREEN PIN duty cycle
                //Also with this we increaste the RED PIN duty cycle
                //In one word - the RGB is going from GREEN to RED with all colours
                while (greenValue > 0.10)
                {
                    greenValue -= 0.05f;
                    redValue += 0.05f;

                    greenPin.DutyCycle = greenValue;
                    redPin.DutyCycle = redValue;

                    Thread.Sleep(50);
                }

                cnt++;
            }

            //Here we close the PWM channels
            redPin.Stop();
            greenPin.Stop();
            bluePin.Stop();

            //Here we stop our thread
            Thread.Sleep(Timeout.Infinite);

            //Enjoy the colour effects and try yourself other combinations :)
        }
    }
}
