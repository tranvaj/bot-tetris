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
        public int heightBoard = 20;
        public int currentPiece = 99;
        public int previousPiece = 99;
        public static int? hold = null;
        public int[,] tetrisMap;
        public bool holdAllowed = true;
        public int maxBuildHeight = 10;
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
        public static MisaMinoParameters newparams = new MisaMinoParameters(13, 9,17, 10, 29,25, 39, 2, 12, 19, 7, 24, 21, 16, 1, 19, 0, 30,0,24,0);
        bool isLocked = true;

        //Default settings,values
        int searchDepth = 100;


        public Form1()
        {
            InitializeComponent();
            // pictureBox1.CreateGraphics();
            KeyboardIntercept keyboardIntercept = new KeyboardIntercept();
            keyboardIntercept.KeyIntercepted += new EventHandler<KeyEventArgs>(Form1_KeyDown);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            overlay = new Form2();
            capturer = new Form3();
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


        private void button1_Click(object sender, EventArgs e)
        {
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

            if (Int32.Parse(textBox2.Text) % 2 == 0)
            {
                if(!(Int32.TryParse(textBox2.Text, out blockSize))){
                    MessageBox.Show("Wrong value input.");

                }
                {
                    overlay.Width = blockSize * 10;
                    overlay.Height = blockSize * 20;
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
                //capturer.Show();
                nextPieceChecker.Show();
                MoveWindow.Start();
                timer1.Stop();
                timer2.Stop();
                overlayHidden = false;
            }
            else
            {
                
                MisaMino.Configure(newparams, true, true, false, searchDepth);

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


        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }


        public static int fromNextPiece;
        bool shouldStart = false;
        public  bool spinUsed;
        public bool success;
        //List<Instruction> misaInstr = new List<Instruction>(15);
        private void checkIfReady()
        {
            //int imageDetectPiece;

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
            //PerfectClear.Find(tetrisMap, queue, currentPiece,hold,holdAllowed,maxBuildHeight, false,0,0,false);
            /*misaInstr = MisaMino.FindPath(tetrisMap, 
                heightBoard, 
                currentPiece, 
                PerfectClear.LastSolution[0].X, 
                PerfectClear.LastSolution[0].Y, 
                PerfectClear.LastSolution[0].R, 
                currentPiece != PerfectClear.LastSolution[0].Piece, ref spinUsed, out success
                               
                );*/
            MisaMino.FindMove(queue, currentPiece, hold, heightBoard, tetrisMap, 0, 0, 0);


            fromNextPiece = queue[0];
            here:
            return;

        }
            private void timer1_Tick(object sender, EventArgs e)
            {
            

            using (Bitmap bmp = ScreenCap.Grab(overlayArea))
            {
                tetrisBoard.GetGrid(bmp);
                tetrisMap = TetrisStart.Board.tetrisField;
                Board.Text = TetrisStart.Board.Print2DArray(TetrisStart.Board.tetrisField);

            }
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
        private void timer2_Tick(object sender, EventArgs e)
        {

          
            string loff = "";
            foreach (var item in getNextPieces(pieceNext))
            {
                loff += MisaMino.ToChar[item] + " ";
            }
            label2.Text =  loff;


            /*
                        if (gameStarted)
                        {

                        }
                        else
                        {
                            using (Bitmap bmp = ScreenCap.Grab(capturerArea))
                            {

                                currentPiece = tetrisBoard.GetCurrent(bmp);
                                //label5.Text = currentPiece.ToString();

                            }
                        }
                        */
            //label4.Text = Board.Print2DArray(Board.FlipArray(Board.capturerField));
            if (shouldStart)
                {
                    //gameStarted = true;
                    //loadCurrent = false;
                 
                    checkIfReady();
                MisaMino_Finished();
                //PerfectClear_Finished();     
                HELPMELOL(MisaMino.LastSolution.Instructions);

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
            overlay.Hide();
            capturer.Hide();
            nextPieceChecker.Hide();
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
                   int xdelay = MisaMino.LastSolution.FinalY;

                   if (Keypresses.executeInstructions(getInstructions(instruct),xdelay) == true)
                   {
                       hold = currentPiece;

                   };
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
                capturer.Left = overlay.Left; //+ ((overlay.Width / 10) * 3)
                capturer.Top = overlay.Top - capturer.Height; //
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
                TetrisStart.Board.jstris = true;
                TetrisStart.Board.lightBlue = Color.FromArgb(15, 155, 215);
                TetrisStart.Board.pink = Color.FromArgb(175, 41, 138);
                TetrisStart.Board.yellow = Color.FromArgb(227, 159, 2);
                TetrisStart.Board.purple = Color.FromArgb(33, 65, 198);

                TetrisStart.Board.orange = Color.FromArgb(227, 91, 2);
                TetrisStart.Board.green = Color.FromArgb(89, 177, 1);
                TetrisStart.Board.red = Color.FromArgb(215, 15, 55);
                TetrisStart.Board.garbage = Color.FromArgb(153, 153, 153);
                heightBoard = 19;
            }
            else
            {
                TetrisStart.Board.jstris = false;
                 TetrisStart.Board.lightBlue = Color.FromArgb(61, 191, 144);
                TetrisStart.Board.pink = Color.FromArgb(188, 84, 173);
                     TetrisStart.Board.yellow = Color.FromArgb(196, 172, 67);
                     TetrisStart.Board.purple = Color.FromArgb(79, 62, 155);
       
                     TetrisStart.Board.orange = Color.FromArgb(194, 112, 63);
                     TetrisStart.Board.green = Color.FromArgb(131, 175, 58);
                  TetrisStart.Board.red = Color.FromArgb(203, 88, 94);
                     TetrisStart.Board.garbage = Color.FromArgb(70, 70, 70);
                heightBoard = 20;
            }
        }
    }
}
