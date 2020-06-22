﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snake
{
    public enum Direction { Up, Down, Left, Right };


    public class Settings
    {
        public static int Width { get; set; }
        public static int Height { get; set; }
        public static int Speed { get; set; }
        public static int Score { get; set; }
        public static int Points { get; set; }
        public static bool GameOver { get; set; }
        public static Direction direction { get; set; }

        public Settings()
        {
            Width = 16; //16 пиксела
            Height = 16;
            Speed = 16;
            Score = 0; //в началото на играта
            Points = 100; //всеки път след изяждане на храната
            GameOver = false;
            direction = Direction.Down;
        }
    }
}