using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TetrisStart
{

    class Tetromino
    {
        public static List<int[,]> list = new List<int[,]>();
        public static int[,] ttoI = new int[10, 2]
        {
            {255, 255},
             {255, 255},
              {255, 255},
             {2, 255},
             {2, 255},
             {2, 255},
             {2, 255},
          {255, 255},
               {255, 255},
                {255, 255},


        };
        public static int[,] ttoJ = new int[10, 2]
        {
           {255, 255},
               {255, 255},
               {255, 255},
             {2, 2},
             {2, 255},
             {2, 255},
             {255, 255},
        {255, 255},
               {255, 255},
               {255, 255},

          };
        public static int[,] ttoL = new int[10, 2]
          {
        {255, 255},
               {255, 255},
               {255, 255},
             {2, 255},
             {2, 255},
             {2, 2},
             {255, 255},
          {255, 255},
               {255, 255},
               {255, 255},
          };
        public static int[,] ttoO = new int[10, 2]
             {
                {255, 255},
               {255, 255},
               {255, 255},
               {255, 255},
             {2, 2},
             {2, 2},
             {255, 255},
            {255, 255},
               {255, 255},
               {255, 255},


              };
        public static int[,] ttoS = new int[10, 2]
                  {
          {255, 255},
               {255, 255},
               {255, 255},
             {2, 255},
             {2, 2},
             {255, 2},
             {255, 255},
             {255, 255},
               {255, 255},
               {255, 255},

             };
        public static int[,] ttoT = new int[10, 2]
            {
      {255, 255},
               {255, 255},
               {255, 255},
             {2, 255},
             {2, 2},
             {2, 255},
             {255, 255},
      {255, 255},
               {255, 255},
               {255, 255},

         };
        public static int[,] ttoZ = new int[10, 2]
           {
               {255, 255},
                {255, 255},
                {255, 255},
             {255, 2},
             {2, 2},
             {2, 255},
             {255, 255},
               {255, 255},
               {255, 255},
               {255, 255},

        };

        public Tetromino()
        {
            
        }
        public static List<int[,]> getList()
        {
            list.Add(ttoI);
            list.Add(ttoJ);
            list.Add(ttoS);
            list.Add(ttoL);
            list.Add(ttoZ);
            list.Add(ttoT);
            list.Add(ttoO);
            return list;
        }


    }
}
