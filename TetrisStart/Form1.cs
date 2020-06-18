using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using MisaMinoNET;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;


namespace TetrisStart
{
    public partial class Form1 : Form
    {
        private delegate void SafeCallDelegate(string text);
        Form2 overlay;
        Form3 capturer;
        NextPieceOverlay nextPieceChecker;
        private Board tetrisBoard;
        bool overlayHidden = true;
        private Rectangle overlayArea;
        private Rectangle capturerArea;
        private Rectangle nextPieceArea;
        public static int blockSize = 32;
        
        int pieceNext = 3;

        public static int[] queue = { };
        public static int heightBoard = 21;
        public int currentPiece = 99;
        public int previousPiece = 99;
        public static int? hold = null;
        public int[,] tetrisMap;
        public bool holdAllowed = true;
        public int maxBuildHeight = 21;
        /*
           public int heightBoard = 20;
          public int currentPiece = 99;
          public int previousPiece = 99;
          public static int[] queue = { };
          public static int? hold = null;
          public int[,] tetrisMap;
                  bool executeins = false;    
        string test = "";
        public string[] currentInstructions = { "zzz" };
        public string[] previousInstructions = { "zzz" };
        
        bool loadCurrent = true;
               */
        //bool gameStarted = false;
        string test = "";
        public string[] currentInstructions = { "zzz" };
        public string[] previousInstructions = { "zzz" };

        //bool loadCurrent = true;
        //ultra
        //public static MisaMinoParameters newparams = new MisaMinoParameters(16, 9,11, 23,20,1, 39, 2, 12, 19, 7, 24, 32, 16, 1, 19, 500, 0,63,0,100);
        //normal
        //public static MisaMinoParameters newparams = new MisaMinoParameters(13, 9, 17, 10, 29, 25, 39, 2, 12, 19, 7, 24, 21, 16, 1, 19, 0, 30, 0, 24, 100);
        //normal+b2b
        //public static MisaMinoParameters newparams = new MisaMinoParameters(16,9,11,17,17,25,39,2,12,19,7,24,18,7,14,19,99,14,19,0,0);
        //    idk  

        public static MisaMinoParameters newparams = new MisaMinoParameters(13, 9, 17, 10, 29, 25, 39, 2, 12, 19, 7, 24, 21, 16, 1, 19, 99, 30, 0,24,0);


        bool isLocked = true;

        //Default settings,values
        int searchDepth = 100;
        bool allSpin = false;
        bool tsd = false;
        int wide4 = 0;

        public Form1()
        {
            InitializeComponent();
            // pictureBox1.CreateGraphics();
            KeyboardIntercept keyboardIntercept = new KeyboardIntercept();
            keyboardIntercept.KeyIntercepted += new EventHandler<KeyEventArgs>(Form1_KeyDown);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            overlay = new Form2();
            capturer = new Form3();
            //capturer.Width = 1;
            nextPieceChecker = new NextPieceOverlay();
            capturer.StartPosition = FormStartPosition.Manual;
            nextPieceChecker.StartPosition = FormStartPosition.Manual;
            //overlay.Show();
            TopMost = true;
            MoveWindow.Start();



            //default values settings
            Intelligence.Text = searchDepth.ToString();
            textBox1.Text = Keypresses.delay.ToString();
            textBox2.Text = blockSize.ToString();
            textBox3.Text = pieceNext.ToString();
        }
        int pieceNext2;

        private void button1_Click(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
            {
                newparams = new MisaMinoParameters(13, 9, 17, 10, 29, 25, 39, 2, 12, 19, 7, 24, 21, 16, 1, 19, 0, 30, 0, 24, wide4);
                label11.Text = "tspin";
                //MessageBox.Show("1");
            }
            if (radioButton2.Checked)
            {
                newparams = new MisaMinoParameters(16, 9, 11, 17, 17, 25, 39, 2, 12, 19, 7, 24, 18, 7, 14, 19, 99, 14, 19, 0, wide4);
                label11.Text = "tspin + b2b";
                //MessageBox.Show("2");
            }
            if (radioButton3.Checked)
            {
                //newparams = new MisaMinoParameters(16, 9, 11, 23, 20, 25, 39, 2, 12, 19, 7,24, 32, 16, 1, 19, 10, 40, 20, 0, wide4);
                newparams = new MisaMinoParameters(13, 9, 17, 10, 29, 25, 39, 2, 12, 19, 7, 24, 21, 16, 1, 19, 5, 30, 5, 24, wide4);
                //new MisaMinoParameters(16,9,11,17,17,25,39,2,12,19,7,24,18,7,14,19,99,14,19,0,0);
                //newparams = new MisaMinoParameters(16, 9,11, 23,20,1, 39, 2, 12, 19, 7, 24, 32, 16, 1, 19, 500, 0,63,0,100);
                label11.Text = "ultra";
                
            }

            //load values from boxes
            // searchDepth = Intelligence.Text.To;
            if (!(Int32.TryParse(Intelligence.Text, out searchDepth)))
            {
                MessageBox.Show("Wrong value input.");

            }
            //textbox1 is keypress delay
            if (!(Int32.TryParse(textBox1.Text, out Keypresses.delay)))
            {
                MessageBox.Show("Wrong value input.");

            }
            if (!(Int32.TryParse(textBox3.Text, out pieceNext)))
            {
                MessageBox.Show("Wrong value input.");

            }
            else
            {
                pieceNext2 = pieceNext;
            }

            if (Int32.Parse(textBox2.Text) % 2 == 0)
            {
                if(!(Int32.TryParse(textBox2.Text, out blockSize))){
                    MessageBox.Show("Wrong value input.");

                }
                else
                {
                    overlay.Width = blockSize * 10;
                    overlay.Height = blockSize * heightBoard;
                    capturer.Height = blockSize * 20;
                    //Board.setTetrisFieldHeight(heightBoard);
                    
                   // nextPieceChecker.Width = blockSize * 5;
                    //nextPieceChecker.Height = blockSize * 3;
                }

            }
            else
            {
                MessageBox.Show("Even numbers only. Value not changed.");
               
            }





            if (overlayHidden)
            {
                overlay.Show();
                capturer.Show();
                nextPieceChecker.Show();
                MoveWindow.Start();
                timer1.Stop();
                timer2.Stop();
                MisaMino.Reset();
                resetVars();
                overlayHidden = false;
            }
            else
            {
                
               

                MisaMino.Configure(newparams, true, allSpin, tsd, searchDepth);

                MoveWindow.Stop();
                overlayArea = new Rectangle(overlay.Location.X,
                          overlay.Location.Y,
                          overlay.Width,
                          overlay.Height);
                capturerArea = new Rectangle(capturer.Location.X,
                                        capturer.Location.Y,
                                       capturer.Width,
                                       capturer.Height);
                nextPieceArea = new Rectangle(nextPieceChecker.Location.X,
                                       nextPieceChecker.Location.Y,
                                      nextPieceChecker.Width,
                                      nextPieceChecker.Height);
                overlay.Hide();
                nextPieceChecker.Hide();
                capturer.Hide();
                shouldStart = false;
               
                using (Bitmap bmp = ScreenCap.Grab(overlayArea))
                {
                    tetrisBoard = new Board(blockSize, bmp);
                    tetrisBoard.GetGrid(bmp);
                    tetrisMap = TetrisStart.Board.tetrisField;
                    Board.Text = TetrisStart.Board.Print2DArray(TetrisStart.Board.tetrisField);
                    
                    //pictureBox1.Image = ScreenCap.Grab(overlayArea);
                   //pictureBox2.Image = ScreenCap.Grab(capturerArea);
                }
                timer1.Start();
                timer2.Start();

                overlayHidden = true;

                

            }
        }

        private static void resetVars()
        {
            b2b = 0;
            TetrisStart.Board.combo = 0;
            TetrisStart.Board.previouscombo = 0;
            TetrisStart.Board.currentAmount = 0;
            TetrisStart.Board.previousAmount = 0;
            hold = null;
    }
        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }


        public static int fromNextPiece;
        bool shouldStart = false;
        public  bool spinUsed;
        public bool success;
        private int realCombo()
        {
            int realcombo = 0;

            realcombo = TetrisStart.Board.combo - 1;
            if (realcombo < 0)
            {
                realcombo = 0;
            }
            return realcombo;
        }
        //List<Instruction> misaInstr = new List<Instruction>(15);
        private void checkIfReady()
        {
            //legit mode
            /* if (legit)
             {
                 Random rnd = new Random();

                 int druh = 1;

                 int dice = rnd.Next(druh, pieceNext2); // creates a number between 1 and 5

                 pieceNext = dice;
                 Debug.WriteLine(pieceNext);

             }*/




            using (Bitmap bmp = ScreenCap.Grab(overlayArea))
            {
                tetrisBoard.GetGrid(bmp);
                               tetrisMap = TetrisStart.Board.tetrisField;
                Board.Text = TetrisStart.Board.Print2DArray(TetrisStart.Board.tetrisField);

            }
            queue = getNextPieces(pieceNext);
            if(queue.Length == 0)
            {
                goto here;
            }
            currentPiece = fromNextPiece;
           
            
            MisaMino.FindMove(queue, currentPiece, hold, maxBuildHeight, tetrisMap, realCombo(), b2b, garbageupcoming);

            //label10.Text = currentPiece.ToString() + "\n" +heightBoard ;

            fromNextPiece = queue[0];
            here:
            return;

        }
        public static int garbageupcoming = 0;
            private void timer1_Tick(object sender, EventArgs e)
            {

            

            /*   using (Bitmap bmp = ScreenCap.Grab(overlayArea))
               {
                   tetrisBoard.GetGrid(bmp);
                   tetrisMap = TetrisStart.Board.tetrisField;
                   Board.Text = TetrisStart.Board.Print2DArray(TetrisStart.Board.tetrisField);

               }*/
        }


        private int[] getNextPieces(int howmany)
        {
            //SZJLTOI
            //0123456
           
            int[] empty = { };
            int[] lol = new int[howmany];
            Rectangle ourpiece = new Rectangle(nextPieceChecker.Location.X,
                                          nextPieceChecker.Location.Y,
                                         nextPieceChecker.Width,
                                         nextPieceChecker.Height);
            //ourpiece = nextPieceArea;
            for (int i = 0; i < howmany; i++)
            {
                
                //ourpiece.Y += ourpiece.Height * i;
                ourpiece = new Rectangle(ourpiece.Location.X, nextPieceChecker.Location.Y + (i*ourpiece.Height), ourpiece.Width, ourpiece.Height);
                using (Bitmap bmp = ScreenCap.Grab(ourpiece))
                {
                    lol[i] = tetrisBoard.GetCurrent(bmp);
                    if(lol[i] > 100)
                    {
                       return empty;
                    }
                }
            }

            return lol;
            
        }
        public static int b2b = 0;
        public static bool executingmoves = false;
        private void timer2_Tick(object sender, EventArgs e)
        {
            // int legitdelay = 0;
            
            string loff = "";
            foreach (var item in getNextPieces(pieceNext))
            {
                loff += MisaMino.ToChar[item] + " ";
            }
            label2.Text =  loff;
            
            if (shouldStart)
                {
                using (Bitmap bmp = ScreenCap.Grab(capturerArea))
                {
                    garbageupcoming = tetrisBoard.garbageDetect(bmp);
                    label12.Text = "Incoming garbage:" + garbageupcoming.ToString();
                    //pictureBox1.Image = ScreenCap.Grab(capturerArea);

                }
                if (legit)
                {
                    int depthcopy = searchDepth;

                    Random rnd = new Random();



                    //int dice = rnd.Next(depthcopy/3, depthcopy+(depthcopy/2)); // creates a number between 1 and 5
                    int dice = rnd.Next(1, 20);
                    if(dice == 20)
                    {
                        //rMisaMino.Configure(newparams, true, allSpin, tsd, depthcopy + (depthcopy / 2));
                        Console.WriteLine("rng");
                    }


                }
                
                


                checkIfReady();
                    MisaMino_Finished();
                //Thread.Sleep(legitdelay);
                HELPMELOL(MisaMino.LastSolution.Instructions);
                using (Bitmap bmp = ScreenCap.Grab(overlayArea))
                {
                    tetrisBoard.GetGrid(bmp);
                    tetrisBoard.getCombo();
                    comboaa.Text = realCombo().ToString() + " current:" + TetrisStart.Board.currentAmount + "rpevious " + TetrisStart.Board.previousAmount;
                    tetrisMap = TetrisStart.Board.tetrisField;
                    Board.Text = TetrisStart.Board.Print2DArray(TetrisStart.Board.tetrisField);

                }
                b2b = MisaMino.LastSolution.B2B;
                backtoback.Text = "b2b: " + b2b.ToString();

            }
            else
            {
                int[] queueToRead = getNextPieces(pieceNext);
                if(!(queueToRead.Length == 0))
                {
                fromNextPiece = queueToRead[0];

                }
                else
                {
                   // fromNextPiece = 999;
                }
            }


        }


      
        private void DisableProgram()
        {
            Debug.WriteLine("escp");
            timer1.Stop();
            timer2.Stop();
            overlayHidden = false;
            b2b = 0;
            TetrisStart.Board.combo = 0;
            overlay.Hide();
            capturer.Hide();
            nextPieceChecker.Hide();
            MisaMino.Reset();
            resetVars();
            //Thread.Sleep(500);
        }
        private void PerfectClear_Finished()
        {

            /*if (misaInstr.Count > 0) {
                     currentInstructions = getInstructions(misaInstr);
                string lold = String.Join(",", misaInstr);
                        test = lold;
                        //executeins = true;
                        SetText(test);

                    }*/


        }

        private void MisaMino_Finished()
                {

                    if (!(MisaMino.LastSolution.Empty)) {
                     currentInstructions = getInstructions(MisaMino.LastSolution.Instructions);
                        test = MisaMino.LastSolution.ToString();
                        //executeins = true;
                        SetText(test);

                    }

                }


        public static string[] getInstructions(List<Instruction> instruct)
        {
            string lold = String.Join(",", instruct); // MisaMino.LastSolution.Instructions

            char[] seperate = { ',' };
            string[] strlist = lold.Split(seperate);
            return strlist;
        }




        private void HELPMELOL(List<Instruction> instruct)
               {
            //int xdelay = MisaMino.LastSolution.FinalY;
            //bool xdelay = 0;
            /* if (legit)
             {
                 Random rnd = new Random();

                 int dice = rnd.Next(30, 80); // creates a number between 1 and 5
                 xdelay = dice;
                 Debug.WriteLine(xdelay);

             }*/
            //executingmoves = true;
            if (Keypresses.executeInstructions(getInstructions(instruct)) == true)
                   {
                       hold = currentPiece;

                   };
            //executingmoves = false;
               }

       





        private void WriteTextSafe(string text)
                 {
                     if (label3.InvokeRequired)
                     {
                         var d = new SafeCallDelegate(WriteTextSafe);
                         label3.Invoke(d, new object[] { text });
                     }
                     else
                     {
                         label3.Text = text;
                     }
                 }
                 private void SetText(string a)
                 {
                     WriteTextSafe(a);
                 }


                     


        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void MoveWindow_Tick(object sender, EventArgs e)
        {
            
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (isLocked)
            {
                MoveWindow.Stop();
                isLocked = false;
            }
            else
            {
                MoveWindow.Start();
                //capturer.Left = overlay.Left; //+ ((overlay.Width / 10) * 3)
                //capturer.Top = overlay.Top - capturer.Height; //
                capturer.Left = overlay.Left - capturer.Width;
                capturer.Top = overlay.Top + blockSize;
                Console.WriteLine(capturer.Width);
                nextPieceChecker.Left = overlay.Left + ((overlay.Width / 10) * 10);
                nextPieceChecker.Top = overlay.Top + ((overlay.Height / 20) * 1 - 4);
                isLocked = true;
            }
        }

        private void Form1_MouseClick(object sender, MouseEventArgs e)
        {
            //label6.Text = "X =" + e.X + " Y=" + e.Y;
        }

        

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Home)
            {
                DisableProgram();
                
                Debug.WriteLine("dsad");
            }
            if(e.KeyCode == Keys.Space)
            {
                if (shouldStart == false) Thread.Sleep(200);

                shouldStart = true;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void Intelligence_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) &&
                    (e.KeyChar != '.'))
            {
                e.Handled = true;
            }

            // only allow one decimal point
            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) &&
                  (e.KeyChar != '.'))
            {
                e.Handled = true;
            }

            // only allow one decimal point
            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }
        }
        bool resizelocked = true;
        private void allowResize_Click(object sender, EventArgs e)
        {
            if (resizelocked)
            {
                nextPieceChecker.FormBorderStyle = FormBorderStyle.Sizable;

                resizelocked = false;
            }
            else
            {
                nextPieceChecker.FormBorderStyle = FormBorderStyle.None;
                resizelocked = true;
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            bool jstrison = TetrisStart.Board.jstris;
            if (!(jstrison))
              {
                TetrisStart.Board.setTetrisFieldHeight(20);
                TetrisStart.Board.jstris = true;
                TetrisStart.Board.lightBlue = Color.FromArgb(15, 155, 215);
                TetrisStart.Board.pink = Color.FromArgb(175, 41, 138);
                TetrisStart.Board.yellow = Color.FromArgb(227, 159, 2);
                TetrisStart.Board.purple = Color.FromArgb(33, 65, 198);

                TetrisStart.Board.orange = Color.FromArgb(227, 91, 2);
                TetrisStart.Board.green = Color.FromArgb(89, 177, 1);
                TetrisStart.Board.red = Color.FromArgb(215, 15, 55);
                TetrisStart.Board.garbage = Color.FromArgb(153, 153, 153);
                heightBoard = 20;
                maxBuildHeight = 19;
            }
            else
            {
                TetrisStart.Board.setTetrisFieldHeight(21);
                TetrisStart.Board.jstris = false;
                 TetrisStart.Board.lightBlue = Color.FromArgb(61, 191, 144);
                TetrisStart.Board.pink = Color.FromArgb(188, 84, 173);
                     TetrisStart.Board.yellow = Color.FromArgb(196, 172, 67);
                     TetrisStart.Board.purple = Color.FromArgb(79, 62, 155);
       
                     TetrisStart.Board.orange = Color.FromArgb(194, 112, 63);
                     TetrisStart.Board.green = Color.FromArgb(131, 175, 58);
                  TetrisStart.Board.red = Color.FromArgb(203, 88, 94);
                     TetrisStart.Board.garbage = Color.FromArgb(70, 70, 70);
                heightBoard = 21;
                maxBuildHeight = 21;
            }
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            if (!(allSpin))
            {
                allSpin = true;
            }
            else
            {
                
                allSpin = false;
            }
            Debug.WriteLine(allSpin);
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            if (!(tsd))
            {
                tsd = true;
            }
            else
            {

                tsd = false;
            }
            Debug.WriteLine(tsd);
        }
         bool legit = false;
        private void checkBox5_CheckedChanged(object sender, EventArgs e)
        {
            if (!(legit))
            {
                legit = true;
                Keypresses.legitM = true;
            }
            else
            {
                legit = false;
                Keypresses.legitM = false;
            }
            Debug.WriteLine(legit);

        }
        bool fourwideOn = false;
        private void backtoback_Click(object sender, EventArgs e)
        {

        }

        private void checkBox6_CheckedChanged(object sender, EventArgs e)
        {
            if (fourwideOn)
            {
                fourwideOn = false;
                wide4 = 0;
            } else 
            {
                fourwideOn = true;
                wide4 = 99;
            }
            Debug.WriteLine(wide4);
        }
    }
}
