using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

using System.Diagnostics;
using InputManager;
using System.Windows.Forms;

namespace TetrisStart
{
    class Keypresses
    {
        public static int delay = 17;
        public static int holddelay = 1000;
        public Keypresses()
        {
            
        }

        public static bool executeInstructions(string[] moves, int delayhold)
        {
             
             if(delayhold >= 18 && delayhold < 22)
            {
                holddelay = 1100;
            }
            else if(delayhold < 17 && delayhold > 14)
            {
                holddelay = 1000;
            }
            else if (delayhold < 14 && delayhold > 10)
            {
                holddelay = 920;
            }
            else if (delayhold < 10 && delayhold > 7)
            {
                holddelay = 820;
            }
            else
            {
                holddelay = 720;
            }


            bool holdme = false;
            string[] movd = moves;
            for (int i = 0; i < movd.Length; i++)
            {
                switch (movd[i])
                {
                    case "NULL":
                        Debug.WriteLine("NULL");
                        continue;
                    case "L":
                        Debug.WriteLine("L");
                        SendLeft();
                        continue;
                    case "R":
                        Debug.WriteLine("R");
                        SendRight();
                        continue;
                    case "LL":
                        Debug.WriteLine("LL");
                        HoldLeft();
                        continue;
                    case "RR":
                        Debug.WriteLine("RR");
                        HoldRight();
                        continue;
                    case "D":
                        Debug.WriteLine("D");
                        SendDown();
                        continue;
                    case "DD":
                        Debug.WriteLine("DD");
                        HoldDown();
                        continue;
                    case "LSPIN":
                        Debug.WriteLine("LSPIN");
                        RotateCounterClock();
                        continue;
                    case "RSPIN":
                        Debug.WriteLine("RSPIN");
                        RotateClock();
                        continue;
                    case "DROP":
                        Debug.WriteLine("drop");
                        SendDrop();
                        continue;
                    case "HOLD":
                        Debug.WriteLine("hold");
                        HoldPiece();
                        holdme = true;
                        continue;
                    default:
                        Debug.WriteLine("BOOM");
                        break;

                }
                Thread.Sleep(100);
            }

            return holdme;

        }
        public static void SendRight()
        {
            Keyboard.KeyDown(Keys.Right);
            Thread.Sleep(delay);
            Keyboard.KeyUp(Keys.Right);
            Thread.Sleep(delay);

        }
        public static void SendLeft()
        {
            Keyboard.KeyDown(Keys.Left);
            Thread.Sleep(delay);
            Keyboard.KeyUp(Keys.Left);
            Thread.Sleep(delay);
        }

        public static void HoldLeft()
        {
            for(int i = 0; i < 6; i++)
            {
                SendLeft();
            }
            /*Keyboard.KeyDown(Keys.Left);
            Thread.Sleep(200);
            Keyboard.KeyUp(Keys.Left);
            Thread.Sleep(delay);*/


        }

        public static void HoldRight()
        {
            for (int i = 0; i < 6; i++)
            {
                SendRight();
            }
            /*Keyboard.KeyDown(Keys.Right);
            Thread.Sleep(200);
            Keyboard.KeyUp(Keys.Right);
            Thread.Sleep(delay);*/

        }
        public static void SendDown()
        {
            Keyboard.KeyDown(Keys.Down);
            Thread.Sleep(delay);
            Keyboard.KeyUp(Keys.Down);
            Thread.Sleep(delay);

        }

        public static void HoldDown()
        {
            Keyboard.KeyDown(Keys.Down);
            Thread.Sleep(50);
            Keyboard.KeyUp(Keys.Down);
            Thread.Sleep(delay);

        }
        public static void RotateCounterClock()
        {
            Keyboard.KeyDown(Keys.Y);
            Thread.Sleep(delay);
            Keyboard.KeyUp(Keys.Y);
            Thread.Sleep(delay);


        }
        public static void RotateClock()
        {
            Keyboard.KeyDown(Keys.Up);
            Thread.Sleep(delay);
            Keyboard.KeyUp(Keys.Up);
            Thread.Sleep(delay);


        }
        public static void SendDrop()
        {   
            Keyboard.KeyDown(Keys.Space);
            Thread.Sleep(delay);

            Keyboard.KeyUp(Keys.Space);
            Thread.Sleep(delay);



        }
        public static void HoldPiece()
        {
          
            Keyboard.KeyDown(Keys.LShiftKey);
            Thread.Sleep(delay);
            Keyboard.KeyUp(Keys.LShiftKey);
            Thread.Sleep(delay);


        }
        


    }
}
