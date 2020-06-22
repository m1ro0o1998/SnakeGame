using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Snake
{
    public partial class Form1 : Form
    {
        private List<Circle> Snake = new List<Circle>();
        private Circle food = new Circle();

        public Form1()
        {
            InitializeComponent();

            //задава настройките по default
            new Settings();

            //задава скоростта на играта и стартира таймер
            GameTimer.Interval = 1000 / Settings.Speed; //1000 милисекунди делено на настройките за скорост
            GameTimer.Tick += UpdateScreen;
            GameTimer.Start();

            //нова игра
            StartGame();
        }

        private EventHandler UpdateScreen()
        {
            throw new NotImplementedException();
        }

        private void StartGame()
        {
            lblGameOver.Visible = false;

            //set settings to default
            new Settings();

            //създаване на нова змия
            Snake.Clear(); //маха змията от предишна игра
            Circle head = new Circle {X = 10, Y = 5};
            Snake.Add(head);

            lblScore.Text = Settings.Score.ToString();
            GenerateFood();

        }
        //генерира храна на произволно място
        private void GenerateFood()
        {
            int maxXPos = pbCanvas.Size.Width / Settings.Width;
            int maxYPos = pbCanvas.Size.Height / Settings.Height;

            Random random = new Random();
            food = new Circle();
            food.X = random.Next(0, maxXPos);
            food.Y = random.Next(0, maxYPos);
        }

        private void UpdateScreen(object sender, EventArgs e)
        {
            //check for game over
            if (Settings.GameOver == true)
            {
                //check if enter is pressed
                if (Input.KeyPressed(Keys.Enter))
                {
                    StartGame();
                }
            }
            else
            {
                if (Input.KeyPressed(Keys.Right) && Settings.direction != Direction.Left)
                    Settings.direction = Direction.Right;
                else if (Input.KeyPressed(Keys.Left) && Settings.direction != Direction.Right)
                    Settings.direction = Direction.Left;
                else if (Input.KeyPressed(Keys.Up) && Settings.direction != Direction.Down)
                    Settings.direction = Direction.Up;
                else if (Input.KeyPressed(Keys.Down) && Settings.direction != Direction.Up)
                    Settings.direction = Direction.Down;

                MovePlayer();
            }

            pbCanvas.Invalidate();
        }

        private void pbCanvas_Paint(object sender, PaintEventArgs e)
        {
            Graphics canvas = e.Graphics;

            if (!Settings.GameOver)
            {
                //задаване на цветове
                Brush snakeColour;

                //рисуване на змия
                for (int i = 0; i < Snake.Count; i++)
                {
                    if (i == 0)
                        snakeColour = Brushes.Black;    //глава
                    else snakeColour = Brushes.Green;   //тялото
                    canvas.FillEllipse(snakeColour,
                        new Rectangle(Snake[i].X * Settings.Width,
                                      Snake[i].Y * Settings.Height,
                                      Settings.Width, Settings.Height));

                    //рисува храна
                    canvas.FillEllipse(Brushes.Red,
                        new Rectangle(food.X * Settings.Width,
                            food.Y * Settings.Height, Settings.Width, Settings.Height));
                }
            }
            else
            {
                string GameOver = "Game Over \nYour final score is: " + Settings.Score + "\nPress Enter to try again";
                lblGameOver.Text = GameOver;
                lblGameOver.Visible = true;
            }
        }

        private void MovePlayer()
        {
            for (int i = Snake.Count - 1; i >= 0; i--)
            {
                //движение
                if (i == 0)
                {
                    switch (Settings.direction)
                    {
                        case Direction.Right:       //кара тялото да следва главата на змията
                            Snake[i].X++;
                            break;
                        case Direction.Left:
                            Snake[i].X--;
                            break;
                        case Direction.Up:
                            Snake[i].Y--;
                            break;
                        case Direction.Down:
                            Snake[i].Y++;
                            break;
                    }

                    //get maximum X and Y pos
                    int maxXPos = pbCanvas.Size.Width / Settings.Width;
                    int maxYPos = pbCanvas.Size.Height / Settings.Height;

                    //засива сблъсък със стените
                    if (Snake[i].X < 0 || Snake[i].Y < 0
                        || Snake[i].X >= maxXPos || Snake[i].Y >= maxYPos)
                    {
                        Die();
                    }

                    //засича сблъсък с тялото
                    for (int j = 1; j < Snake.Count; j++)
                    {
                        if (Snake[i].X == Snake[j].X &&
                            Snake[i].Y == Snake[j].Y)
                        {
                            Die();
                        }
                    }

                    //засича сблъсък с храна
                    if (Snake[0].X == food.X && Snake[0].Y == food.Y)
                    {
                        Eat();
                    }

                }
                else
                {
                    Snake[i].X = Snake[i - 1].X;
                    Snake[i].Y = Snake[i - 1].Y;
                }
            }
        }

        private void Eat()
        {
            //растене на змията
            Circle food = new Circle();
            food.X = Snake[Snake.Count - 1].X;
            food.Y = Snake[Snake.Count - 1].Y;

            Snake.Add(food);

            //прибавяне на нови точки
            Settings.Score += Settings.Points;
            lblScore.Text = Settings.Score.ToString();

            GenerateFood(); //след изядена храна извежда нова на екрана
        }

        private void Die()
        {
            Settings.GameOver = true;
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            Input.ChangeState (e.KeyCode, true);

        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            Input.ChangeState (e.KeyCode, false);
        }
    }
}
