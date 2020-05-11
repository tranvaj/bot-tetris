using System;
using System.Collections.Generic;
using System.Diagnostics;

using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using MisaMinoNET;

namespace TetrisStart
{
    class Calculation
    {


        public static int? holdPiece = null;
        private static int currentPiece;
        private static int moveDelay;


        public static void bestMove(int[] queue, int current, int? hold, int height, int[,] field, int combo, int b2b, int garbage, int xdelay)
        {
            

            MisaMino.Finished += MisaMino_Finished;
            currentPiece = current;
            moveDelay = xdelay;
            


            MisaMino.FindMove(queue, current, hold, height, field, combo, b2b, garbage);
        }

        private static void MisaMino_Finished(bool success)
        {


            Console.WriteLine(success + MisaMino.LastSolution.ToString());
            executeInstructions(40);
                //Form1.timer2.Start();

            
        }

        public static string[] getInstructions()
        {
            string lold = String.Join(",", MisaMino.LastSolution.Instructions); // MisaMino.LastSolution.Instructions

            char[] seperate = { ',' };
            string[] strlist = lold.Split(seperate);
            return strlist;
        }

        private static void executeInstructions(int xdelay)
        {
            if (Keypresses.executeInstructions(getInstructions()))
            {
                holdPiece = currentPiece;

            };
        }

        public int? getHoldPiece()
        {
            return holdPiece;
        }



    }



}
