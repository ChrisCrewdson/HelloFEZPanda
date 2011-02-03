using System;
using System.Threading;

using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;

using GHIElectronics.NETMF.FEZ;

namespace HelloFEZPanda
{
    public class Program
    {
        public static void Main()
        {
            //The LCD requires 6 signals to work (E, RS, D4, D5, D6,
            //D7) connect pin #3 (contrast) to ground. Also, pin RW is
            //not needed and it must be connected to ground
            //pin connection list:
            //GND- ground
            //VDD- connect to 5V
            //Vo- contrast voltage, connect to ground
            //RS- connect to any digital pin on FEZ
            //RW- not needed, connect to ground
            //D0-leave unconnected
            //D1-leave unconnected
            //D2-leave unconnected
            //D3-leave unconnected
            //D4-connect to any digital pin on FEZ
            //D5-connect to any digital pin on FEZ
            //D6-connect to any digital pin on FEZ
            //D7-connect to any digital pin on FEZ
            //BL_A- backlight, connect to 5V
            //BL_K- backlight, connect to ground
            var lcd = new FEZ_Components.LCD2x16(FEZ_Pin.Digital.Di5, FEZ_Pin.Digital.Di6, FEZ_Pin.Digital.Di7,
            FEZ_Pin.Digital.Di8, FEZ_Pin.Digital.Di3, FEZ_Pin.Digital.Di2);
            lcd.Clear();
            int i = 0;
            while (true)
            {
                lcd.CursorHome();
                lcd.Print("Counter " + i++);
                Thread.Sleep(100);
            }

            //var servo0 = new FEZ_Components.ServoMotor(FEZ_Pin.Digital.Di0);
            //var servo1 = new FEZ_Components.ServoMotor(FEZ_Pin.Digital.Di1);
            
            //for (byte ii = 0; ii < 3; ii++)
            //{
            //    // slowly translates from one end to the other, then jumps back and starts again.
            //    for (byte jj = 0; jj <= 180; jj += 10)
            //    {
            //        servo0.SetPosition(jj);
            //        servo1.SetPosition(jj);
            //        Thread.Sleep(2);
            //    }
            //}
            
            //servo0.Dispose();
            //servo1.Dispose();

            // Blink board LED
            //bool ledState = false;
            //OutputPort led = new OutputPort((Cpu.Pin)FEZ_Pin.Digital.LED, ledState);

            //while (true)
            //{
            //    // Sleep for 500 milliseconds
            //    Thread.Sleep(500);

            //    // toggle LED state
            //    ledState = !ledState;
            //    led.Write(ledState);
            //}
        }

    }
}
