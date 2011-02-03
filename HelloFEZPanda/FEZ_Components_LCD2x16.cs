/*
Copyright 2010 GHI Electronics LLC
Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License. You may obtain a copy of the License at
http://www.apache.org/licenses/LICENSE-2.0
Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. See the License for the specific language governing permissions and limitations under the License. 
*/

using System;
using System.Runtime.CompilerServices;
using System.Threading;
using Microsoft.SPOT.Hardware;
using GHIElectronics.NETMF.FEZ;

namespace GHIElectronics.NETMF.FEZ
{
    public static partial class FEZ_Components
    {
        public class LCD2x16 : IDisposable
        {
            static OutputPort LCD_RS;
            static OutputPort LCD_E;
            static OutputPort LCD_D4;
            static OutputPort LCD_D5;
            static OutputPort LCD_D6;
            static OutputPort LCD_D7;

            const byte DISP_ON = 0xC;    //Turn visible LCD on
            const byte CLR_DISP = 1;      //Clear display
            const byte CUR_HOME = 2;      //Move cursor home and clear screen memory
            const byte SET_CURSOR = 0x80;   //SET_CURSOR + X : Sets cursor position to X

            public void Dispose()
            {
                LCD_D4.Dispose();
                LCD_D5.Dispose();
                LCD_D6.Dispose();
                LCD_D7.Dispose();
                LCD_RS.Dispose();
                LCD_E.Dispose();
            }

            public LCD2x16(FEZ_Pin.Digital D4, FEZ_Pin.Digital D5, FEZ_Pin.Digital D6, FEZ_Pin.Digital D7, FEZ_Pin.Digital E, FEZ_Pin.Digital RS)
            {
                LCD_RS = new OutputPort((Cpu.Pin)RS, false);
                LCD_E = new OutputPort((Cpu.Pin)E, false);
                LCD_D4 = new OutputPort((Cpu.Pin)D4, false);
                LCD_D5 = new OutputPort((Cpu.Pin)D5, false);
                LCD_D6 = new OutputPort((Cpu.Pin)D6, false);
                LCD_D7 = new OutputPort((Cpu.Pin)D7, false);

                LCD_RS.Write(false);

                // 4 bit data communication
                Thread.Sleep(50);

                LCD_D7.Write(false);
                LCD_D6.Write(false);
                LCD_D5.Write(true);
                LCD_D4.Write(true);

                LCD_E.Write(true);
                LCD_E.Write(false);

                Thread.Sleep(50);

                LCD_D7.Write(false);
                LCD_D6.Write(false);
                LCD_D5.Write(true);
                LCD_D4.Write(true);

                LCD_E.Write(true);
                LCD_E.Write(false);

                Thread.Sleep(50);

                LCD_D7.Write(false);
                LCD_D6.Write(false);
                LCD_D5.Write(true);
                LCD_D4.Write(true);

                LCD_E.Write(true);
                LCD_E.Write(false);

                Thread.Sleep(50);

                LCD_D7.Write(false);
                LCD_D6.Write(false);
                LCD_D5.Write(true);
                LCD_D4.Write(false);

                LCD_E.Write(true);
                LCD_E.Write(false);

                SendCmd(DISP_ON);
                SendCmd(CLR_DISP);
            }

            //Sends an ASCII character to the LCD
            void Putc(byte c)
            {
                LCD_D7.Write((c & 0x80) != 0);
                LCD_D6.Write((c & 0x40) != 0);
                LCD_D5.Write((c & 0x20) != 0);
                LCD_D4.Write((c & 0x10) != 0);
                LCD_E.Write(true); LCD_E.Write(false); //Toggle the Enable Pin

                LCD_D7.Write((c & 0x08) != 0);
                LCD_D6.Write((c & 0x04) != 0);
                LCD_D5.Write((c & 0x02) != 0);
                LCD_D4.Write((c & 0x01) != 0);
                LCD_E.Write(true); LCD_E.Write(false); //Toggle the Enable Pin
                //Thread.Sleep(1);
            }

            //Sends an LCD command
            void SendCmd(byte c)
            {
                LCD_RS.Write(false); //set LCD to data mode

                LCD_D7.Write((c & 0x80) != 0);
                LCD_D6.Write((c & 0x40) != 0);
                LCD_D5.Write((c & 0x20) != 0);
                LCD_D4.Write((c & 0x10) != 0);
                LCD_E.Write(true); LCD_E.Write(false); //Toggle the Enable Pin

                LCD_D7.Write((c & 0x08) != 0);
                LCD_D6.Write((c & 0x04) != 0);
                LCD_D5.Write((c & 0x02) != 0);
                LCD_D4.Write((c & 0x01) != 0);
                LCD_E.Write(true); LCD_E.Write(false); //Toggle the Enable Pin

                Thread.Sleep(1);
                LCD_RS.Write(true); //set LCD to data mode
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