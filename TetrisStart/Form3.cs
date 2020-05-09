﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TetrisStart
{
    public partial class Form3 : Form
    {
        int blockWidth = 32 * 10;
        int blockHeight = 32 * 4;
        private bool isDragged = false;
        private Point moveStartPoint;
        public Form3()
        {
            InitializeComponent();
            BackColor = Color.Green;
            this.FormBorderStyle = FormBorderStyle.None;
            MaximizeBox = false;
            MouseDown += overlay_MouseDown;
            MouseUp += overlay_MouseUp;
            MouseMove += overlay_MouseMove;
            Bounds = new Rectangle(0, 0, blockWidth, blockHeight);
            Opacity = 0.2;
            TopMost = true;
            Hide();
        }

        private void overlay_MouseDown(object sender, MouseEventArgs e)
        {
            moveStartPoint = new Point(e.X, e.Y);
            isDragged = true;
        }

        private void overlay_MouseUp(object sender, MouseEventArgs e)
        {
            isDragged = false;
        }

        private void overlay_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDragged)
            {
                Point p1 = new Point(e.X, e.Y);
                Point p2 = PointToScreen(p1);
                Point p3 =
                Location = new Point(p2.X - moveStartPoint.X,
                                     p2.Y - moveStartPoint.Y);
            }
        }

        private void Form3_Load(object sender, EventArgs e)
        {

        }
    }
}
