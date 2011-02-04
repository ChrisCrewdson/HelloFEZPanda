/*
Copyright 2010 GHI Electronics LLC
Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License. You may obtain a copy of the License at
http://www.apache.org/licenses/LICENSE-2.0
Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. See the License for the specific language governing permissions and limitations under the License. 
*/

using System;
using System.Threading;
using Microsoft.SPOT.Hardware;
using GHIElectronics.NETMF.FEZ;

namespace GHIElectronics.NETMF.FEZ
{
    public static partial class FEZ_Components
    {
        //http://www.microframeworkprojects.com/index.php?title=HD44780
        //The LCD requires 6 signals to work (E, RS, D4, D5, D6,
        //D7) connect pin #3 (contrast) to ground. Also, pin RW is
        //not needed and it must be connected to ground
        //pin connection list:
        //GND- ground
        //VDD- connect to 5V
        //Vo- contrast voltage, connect to ground
        //RS- Register Select - connect to any digital pin on FEZ
        //RW- not needed, connect to ground
        //E - Clock (Enable) - Digital pin
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
        public class LCD2x16 : IDisposable
        {
            static OutputPort _LCDRegisterSelect;
            static OutputPort _LCDEnable;
            static OutputPort _LCDBit0;
            static OutputPort _LCDBit1;
            static OutputPort _LCDBit2;
            static OutputPort _LCDBit3;

            const byte DISP_ON      = 0xC;  //Turn visible LCD on
            const byte CLR_DISP     = 1;    //Clear display
            const byte CUR_HOME     = 2;    //Move cursor home and clear screen memory
            const byte SET_CURSOR   = 0x80; //SET_CURSOR + X : Sets cursor position to X

            public void Dispose()
            {
                _LCDBit0.Dispose();
                _LCDBit1.Dispose();
                _LCDBit2.Dispose();
                _LCDBit3.Dispose();
                _LCDRegisterSelect.Dispose();
                _LCDEnable.Dispose();
            }

            public LCD2x16(
                FEZ_Pin.Digital bit0, 
                FEZ_Pin.Digital bit1, 
                FEZ_Pin.Digital bit2, 
                FEZ_Pin.Digital bit3, 
                FEZ_Pin.Digital enable, 
                FEZ_Pin.Digital registerSelect)
            {
                _LCDRegisterSelect = new OutputPort((Cpu.Pin)registerSelect, false);
                _LCDEnable  = new OutputPort((Cpu.Pin)enable, false);
                _LCDBit0 = new OutputPort((Cpu.Pin)bit0, false);
                _LCDBit1 = new OutputPort((Cpu.Pin)bit1, false);
                _LCDBit2 = new OutputPort((Cpu.Pin)bit2, false);
                _LCDBit3 = new OutputPort((Cpu.Pin)bit3, false);

                _LCDRegisterSelect.Write(false);

                // 4 bit data communication
                Thread.Sleep(50);

                _LCDBit3.Write(false);
                _LCDBit2.Write(false);
                _LCDBit1.Write(true);
                _LCDBit0.Write(true);

                _LCDEnable.Write(true);
                _LCDEnable.Write(false);

                Thread.Sleep(50);

                _LCDBit3.Write(false);
                _LCDBit2.Write(false);
                _LCDBit1.Write(true);
                _LCDBit0.Write(true);

                _LCDEnable.Write(true);
                _LCDEnable.Write(false);

                Thread.Sleep(50);

                _LCDBit3.Write(false);
                _LCDBit2.Write(false);
                _LCDBit1.Write(true);
                _LCDBit0.Write(true);

                _LCDEnable.Write(true);
                _LCDEnable.Write(false);

                Thread.Sleep(50);

                _LCDBit3.Write(false);
                _LCDBit2.Write(false);
                _LCDBit1.Write(true);
                _LCDBit0.Write(false);

                _LCDEnable.Write(true);
                _LCDEnable.Write(false);

                SendCmd(DISP_ON);
                SendCmd(CLR_DISP);
            }

            //Sends an ASCII character to the LCD
            void Putc(byte c)
            {
                _LCDBit3.Write((c & 0x80) != 0);
                _LCDBit2.Write((c & 0x40) != 0);
                _LCDBit1.Write((c & 0x20) != 0);
                _LCDBit0.Write((c & 0x10) != 0);
                _LCDEnable.Write(true); _LCDEnable.Write(false); //Toggle the Enable Pin

                _LCDBit3.Write((c & 0x08) != 0);
                _LCDBit2.Write((c & 0x04) != 0);
                _LCDBit1.Write((c & 0x02) != 0);
                _LCDBit0.Write((c & 0x01) != 0);
                _LCDEnable.Write(true); _LCDEnable.Write(false); //Toggle the Enable Pin
                //Thread.Sleep(1);
            }

            //Sends an LCD command
            void SendCmd(byte c)
            {
                _LCDRegisterSelect.Write(false); //set LCD to data mode

                _LCDBit3.Write((c & 0x80) != 0);
                _LCDBit2.Write((c & 0x40) != 0);
                _LCDBit1.Write((c & 0x20) != 0);
                _LCDBit0.Write((c & 0x10) != 0);
                _LCDEnable.Write(true); _LCDEnable.Write(false); //Toggle the Enable Pin

                _LCDBit3.Write((c & 0x08) != 0);
                _LCDBit2.Write((c & 0x04) != 0);
                _LCDBit1.Write((c & 0x02) != 0);
                _LCDBit0.Write((c & 0x01) != 0);
                _LCDEnable.Write(true); _LCDEnable.Write(false); //Toggle the Enable Pin

                Thread.Sleep(1);
                _LCDRegisterSelect.Write(true); //set LCD to data mode
            }

            public void Print(string str)
            {
                for (int i = 0; i < str.Length; i++)
                    Putc((byte)str[i]);
            }

            public void Clear()
            {
                SendCmd(CLR_DISP);
            }

            public void CursorHome()
            {
                SendCmd(CUR_HOME);
            }

            public void SetCursor(byte row, byte col)
            {
                SendCmd((byte)(SET_CURSOR | row << 6 | col));
            }
        }
    }
}