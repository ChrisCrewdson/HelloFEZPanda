using System.Threading;
using GHIElectronics.NETMF.Hardware;

using GHIElectronics.NETMF.FEZ;

namespace HelloFEZPanda
{
    public class Program
    {
        public static void Main()
        {
            var lcd = new FEZ_Components.LCD2x16(
                FEZ_Pin.Digital.Di3, FEZ_Pin.Digital.Di2, FEZ_Pin.Digital.Di1, FEZ_Pin.Digital.Di0, FEZ_Pin.Digital.Di7, FEZ_Pin.Digital.Di6);
            lcd.Clear();

            using (var servo0 = new FEZ_Components.ServoMotor(FEZ_Pin.Digital.Di13))
            {
                servo0.SetPosition(0);
                Thread.Sleep(1000);
            }

            for (int ii = 10; ii >= 0; ii--)
            {
                lcd.CursorHome();
                lcd.Print("Downcount: " + ii + " ");
                Thread.Sleep(1000);
            }

            var piezo = new PWM((PWM.Pin) FEZ_Pin.PWM.Di10);

            using (var servo0 = new FEZ_Components.ServoMotor(FEZ_Pin.Digital.Di13))
            {
                servo0.SetPosition(180);
                Thread.Sleep(1000);
            }

            piezo.Set(5000, 50);
            Thread.Sleep(500);
            piezo.Set(1000, 50);
            Thread.Sleep(500);
            piezo.Set(500, 50);
            Thread.Sleep(500);
            piezo.Set(50, 50);
            Thread.Sleep(500);
            piezo.Set(0, 0);

            using (var servo0 = new FEZ_Components.ServoMotor(FEZ_Pin.Digital.Di13))
            {
                servo0.SetPosition(0);
                Thread.Sleep(1000);
            }

            lcd.Clear();

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
