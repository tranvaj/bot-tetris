using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Runtime.Remoting.Messaging;
using System.Threading;
using System.Windows.Forms;

namespace TetrisStart
{
    class Board
    {

        public static int[,] tetrisField = new int[10, Form1.heightBoard];
        public static int[,] capturerField = new int[10, 2];
        public int blockSize;
        public Bitmap bmp;
        public Board(int blockSize2, Bitmap bmp2)
        {
            blockSize = blockSize2;
            bmp = bmp2;
        }

        public static void setTetrisFieldHeight(int a)
        {
            tetrisField = new int[10, a];
        }
        public static int[,] FlipArray(int[,] tetrisField)
        {
            int[,] arrayToFlip = tetrisField;
            int rows = arrayToFlip.GetLength(0);
            int columns = arrayToFlip.GetLength(1);
            int[,] flippedArray = new int[rows, columns];
            int temp;
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    temp = arrayToFlip[i, j];

                    flippedArray[i, j] = arrayToFlip[i, columns - j - 1];
                    flippedArray[i, columns - j - 1] = temp;
                }
            }
            return flippedArray;
        }
        public static string Print2DArray(int[,] smth)
        {
            int[,] matrix = smth;
            string lol = "";
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                //lol += matrix[i, j] + "\t";
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    lol += matrix[i, j];
                }
                lol += "\n";

            }
            return lol;
        }

        public static int combo = 0;
        public static int previouscombo = 0;
        public static int currentAmount = 0;
        public static int previousAmount = 0;
        public void getCombo() {
            int b = 0;
            foreach(int item in tetrisField)
            {
                if(item == 100) 
                {
                    b++;
                }
            }
            previouscombo = combo;
            previousAmount = currentAmount;
            currentAmount = b;
          
            if(previousAmount > currentAmount)
            {
                combo++;
                
            }
            /* else if(previousAmount <= currentAmount)
             {
                 combo = 0;
             }*/
             if((currentAmount - previousAmount) > 0)
            {
                combo = 0;
            }
        }
        public void GetGrid(Bitmap tetrisBmp)
        {

            int blockSizeHalf = blockSize / 2;
            Color emptyField = Color.FromArgb(255, 0, 0, 0);
            Color border = Color.FromArgb(255, 255, 255, 255);
            Color warning = Color.FromArgb(255, 255, 0, 0);
            for (int i = 1; i <= tetrisBmp.Width; i++)
            {
                
                if (i * blockSize > tetrisBmp.Width)
                {
                    continue;
                }

                for (int j = 1; j <= tetrisBmp.Height; j++)
                {
                    if (j * blockSize > tetrisBmp.Height)
                    {
                        continue;
                    }

                    Color pixel = tetrisBmp.GetPixel((i * blockSize) - blockSizeHalf, (j * blockSize) - blockSizeHalf);
                    /*if (pixel.Equals(emptyField) || pixel.Equals(border) || pixel.Equals(warning))
                    {
                        tetrisField[i - 1, j - 1] = 255;
                    }
                    else
                    {
                        tetrisField[i - 1, j - 1] = 100;                      
                    }*/
                   
                    if (InList(pixel))
                    {
                        tetrisField[i - 1, j - 1] = 100;

                        //goto here;
                    }
                    else
                    {
                        tetrisField[i - 1, j - 1] = 255;
                        //Debug.WriteLine("What is this color" + pixel.R + " " + pixel.G + " " + pixel.B);
                    }

                }


            }

            tetrisField = FlipArray(tetrisField);

        }


    
        //increase color tolerance - bool init
        public int GetCurrent(Bitmap tetrisBmp)
        {

            int blockSizeHalf = blockSize / 2;
            Color background = Color.FromArgb(255, 0, 0, 0);
            for (int i = 1; i <= tetrisBmp.Width; i++)
            {
                if (i * blockSizeHalf > tetrisBmp.Width)
                   {
                       continue;
                   }

                for (int j = 1; j <= tetrisBmp.Height; j++)
                {
                    if (j * blockSizeHalf > tetrisBmp.Height)
                    {
                        continue;
                    }

                    Color pixel = tetrisBmp.GetPixel((i * blockSizeHalf) - blockSizeHalf, (j * blockSizeHalf) - blockSizeHalf);
                    if (pixel.Equals(background) || pixel.Equals(Color.FromArgb(255, 255, 255, 255)) || pixel.Equals(Color.FromArgb(255, 255, 0, 0)))
                    {
                        //capturerField[i - 1, j - 1] = 255;   
                        
                    }
                    else
                    {
                        //capturerField[i - 1, j - 1] = 2;
                     
                        if (InList(pixel))
                        {
                            //Debug.WriteLine(pixel.ToString());
                            return getIndexByRBG(pixel);

                            //goto here;


                        }
                        else
                        {

                            //Debug.Write("I THINK NOT");
                        }

                    }
                }
            }
           
            return 8989;
        }



        public int garbageDetect(Bitmap tetrisBmp)
        {
            
            int blockSizeHalf = blockSize / 2;
            int garbagelines = 0;
            //Console.WriteLine("width:" + tetrisBmp.Width);


            for (int j = 1; j <= 20; j++)
                {
                   
                    Color pixel = tetrisBmp.GetPixel(tetrisBmp.Width/4, (blockSize * j) - blockSizeHalf);
                    //Console.WriteLine("X: " + tetrisBmp.Width/4 + ",Y: " + ((blockSize * j) - blockSizeHalf));
                //Console.WriteLine(pixel + " distance: " + getColorDistance(pixel, upcomgar));
                    if (getColorDistance(pixel, upcomgar) < 85)
                    {
                        garbagelines++;
                    //Console.WriteLine("increment: " + garbagelines);

                }
            }
            

           
            //int lol = garbagelines / 2;
            
            return garbagelines;// 1;
        }



        public static Color lightBlue = Color.FromArgb(61, 191, 144);
        public static Color pink = Color.FromArgb(188, 84, 173);
        public static Color yellow = Color.FromArgb(196, 172, 67);
        public static Color purple = Color.FromArgb(79, 62, 155);
        //rgb(181,101,52)219
        public static Color orange = Color.FromArgb(194,112,63);
        public static Color green = Color.FromArgb(131, 175, 58);
        public static Color red = Color.FromArgb(203, 88, 94);
        public static Color garbage = Color.FromArgb(70, 70, 70);
        public static Color garbage2 = Color.FromArgb(106, 106, 106);


        //jstris
        /* public static Color lightBlue2 = Color.FromArgb(15, 155, 215);
         public static Color pink2 = Color.FromArgb(175, 41, 138);
         public static Color yellow2 = Color.FromArgb(227, 159, 2);
         public static Color purple2 = Color.FromArgb(33, 65, 198);

         public static Color orange2 = Color.FromArgb(227, 91, 2);
         public static Color green2 = Color.FromArgb(89, 177, 1);
         public static Color red2 = Color.FromArgb(215, 15, 55);
         public static Color garbage2 = Color.FromArgb(153,153,153);*/
        public static Color red2 = Color.FromArgb(179, 58, 59);
        public static Color upcomgar = Color.FromArgb(255,0,0);

        public static bool jstris = false;
        public static bool InList(Color rgb)
        {
            int colorDistance = 80;
           
            List<Color> ColorList = new List<Color>();
            ColorList.Add(lightBlue);
            ColorList.Add(orange);
            ColorList.Add(pink);
            ColorList.Add(purple);
            ColorList.Add(yellow);
            ColorList.Add(green);
            ColorList.Add(red);
            ColorList.Add(red2);
            ColorList.Add(garbage);
            //ColorList.Add(upcomgar);
            ColorList.Add(garbage2);
            foreach (var item in ColorList)
            {
                if(getColorDistance(item, rgb) > colorDistance)
                {
                    //Debug.WriteLine(getColorDistance(item, rgb));
                }
                else
                {
                    return true;
                }
             
            }

            return false;
           


        }


        /* private static double getColorDistance2(Color c1, Color c2)
         {
             double distanceColor = (c1.R - c2.R) * (c1.R - c2.R)
                     + (c1.G - c2.G) * (c1.G - c2.G)
                     + (c1.B - c2.B) * (c1.B - c2.B);
             return Math.Sqrt(distanceColor);
         }*/
        // ColorMine.ColorSpaces.Comparisons.Cie94Comparison.Application.

        private static double getColorDistance(Color e1, Color e2)
        {
            long rmean = ((long)e1.R + (long)e2.R) / 2;
            long r = (long)e1.R - (long)e2.R;
            long g = (long)e1.G - (long)e2.G;
            long b = (long)e1.B - (long)e2.B;
            return Math.Sqrt((((512 + rmean) * r * r) >> 8) + 4 * g * g + (((767 - rmean) * b * b) >> 8));

        }

        public static int getIndexByRBG(Color rgb)
        {
            /// <para>Pieces should be formatted with numbers from 0 to 6 in the order of SZJLTOI. Empty state on the field should be formatted with 255.</para>
            double[] dists = new double[] { getColorDistance(green, rgb), getColorDistance(red, rgb), getColorDistance(purple, rgb), getColorDistance(orange, rgb), getColorDistance(pink, rgb), getColorDistance(yellow, rgb), getColorDistance(lightBlue, rgb) };
            int result = 0;
            for (int i = 0; i < dists.Length; i++)
            {
                if (dists[i] < dists[result])
                {
                    result = i;

                }

            }

            return result;

        }


    }
}