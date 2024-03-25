using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace Snake
{
    class Program
    {
        private static int screenWidth = Console.WindowWidth;
        private static int screenHeight = Console.WindowHeight;
            
        private static Random randomNumber = new Random();
        private static Movement movement = Movement.Right;
        private static int score = 5;
        private static bool isGameOver = false;

        private static Pixel snakeHead = new Pixel(screenWidth / 2,screenHeight / 2,ConsoleColor.Red);
        private static List<Pixel> snakeBody = new List<Pixel>();
        private static Pixel berry = new Pixel(randomNumber.Next(1, screenWidth - 2),randomNumber.Next(1, screenHeight - 2),ConsoleColor.Cyan);

        static void Main(string[] args)
        {
            SetConsole();
            
            while (!isGameOver)
            {
                Console.Clear();
                
                if (IsSnakeOutOfBounds(snakeHead, screenWidth, screenHeight) || IsSnakeCollidingWithBody(snakeHead, snakeBody))
                {
                    isGameOver = true;
                }
                
                DrawBorder();
                Console.ForegroundColor = ConsoleColor.Green;
                
                if (IsSnakeEatingBerry(snakeHead, berry))
                {
                    score++;
                    berry = new Pixel(randomNumber.Next(1, screenWidth - 2), randomNumber.Next(1, screenHeight - 2), ConsoleColor.Cyan);
                }
                foreach (var bodyPart in snakeBody)
                {
                    DrawPixel(bodyPart);

                    if (bodyPart.XPos == snakeHead.XPos && bodyPart.YPos == snakeHead.YPos)
                    {
                        isGameOver = true;
                    }
                }
                if (isGameOver)
                {
                    break;
                }
                
                DrawPixel(snakeHead);
                DrawPixel(berry);

                ProsessInput();
                
                snakeBody.Add(new Pixel(snakeHead.XPos, snakeHead.YPos, ConsoleColor.Green));
                UpdateSnakeHeadPosition(ref snakeHead, movement);
  
                if (snakeBody.Count() > score)
                {
                    snakeBody.RemoveAt(0);
                }
            }
            Console.SetCursorPosition(screenWidth / 5, screenHeight / 2);
            Console.WriteLine("Game over, Score: " + score);
            Console.SetCursorPosition(screenWidth / 5, screenHeight / 2 + 1);
        }
        
        private static bool IsSnakeOutOfBounds(Pixel snakeHead, int screenWidth, int screenHeight)
        {
            return snakeHead.XPos == screenWidth - 1 || snakeHead.XPos == 0 || snakeHead.YPos == screenHeight - 1 || snakeHead.YPos == 0;
        }

        private static bool IsSnakeCollidingWithBody(Pixel snakeHead, List<Pixel> snakeBody)
        {
            foreach (var bodyPart in snakeBody)
            {
                if (bodyPart.XPos == snakeHead.XPos && bodyPart.YPos == snakeHead.YPos)
                {
                    return true;
                }
            }
            return false;
        }

        private static void ProsessInput()
        {
            DateTime timeOfLoopEntry = DateTime.Now;
            bool buttonpressed = false;
            while (true)
            {
                DateTime timeInLoop = DateTime.Now;
                if (timeInLoop.Subtract(timeOfLoopEntry).TotalMilliseconds > 500) { break; }
                if (Console.KeyAvailable)
                {
                    ConsoleKey pressedKey = Console.ReadKey(true).Key;
                    if (pressedKey.Equals(ConsoleKey.UpArrow) && movement != Movement.Down && !buttonpressed)
                    {
                        movement = Movement.Up;
                        buttonpressed = true;
                    }
                    if (pressedKey.Equals(ConsoleKey.DownArrow) && movement != Movement.Up && !buttonpressed)
                    {
                        movement = Movement.Down;
                        buttonpressed = true;
                    }
                    if (pressedKey.Equals(ConsoleKey.LeftArrow) && movement != Movement.Right && !buttonpressed)
                    {
                        movement = Movement.Left;
                        buttonpressed = true;
                    }
                    if (pressedKey.Equals(ConsoleKey.RightArrow) && movement != Movement.Left && !buttonpressed)
                    {
                        movement = Movement.Right;
                        buttonpressed = true;
                    }
                }
            }
        }
        private static bool IsSnakeEatingBerry(Pixel snakeHead, Pixel berry)
        {
            return snakeHead.XPos == berry.XPos && snakeHead.YPos == berry.YPos;
        }
        
        private static void UpdateSnakeHeadPosition(ref Pixel snakeHead, Movement movementDirection)
        {
            switch (movementDirection)
            {
                case Movement.Up:
                    snakeHead.YPos--;
                    break;
                case Movement.Down:
                    snakeHead.YPos++;
                    break;
                case Movement.Left:
                    snakeHead.XPos--;
                    break;
                case Movement.Right:
                    snakeHead.XPos++;
                    break;
            }
        }

        private static void DrawBorder()
        {
            for (int i = 0; i < Console.WindowWidth; i++)
            {
                Console.SetCursorPosition(i, 0);
                Console.Write("■");
                Console.SetCursorPosition(i, Console.WindowHeight - 1);
                Console.Write("■");
            }
            for (int i = 0; i < Console.WindowHeight; i++)
            {
                Console.SetCursorPosition(0, i);
                Console.Write("■");
                Console.SetCursorPosition(Console.WindowWidth - 1, i);
                Console.Write("■");
            }
        }

        private static void DrawPixel(Pixel pixel)
        {
            Console.SetCursorPosition(pixel.XPos, pixel.YPos);
            Console.ForegroundColor = pixel.Color;
            Console.Write("■");
        }

        private static void SetConsole()
        {
            Console.WindowHeight = 16;
            Console.WindowWidth = 32;
            Console.CursorVisible = false;
        }

        class Pixel
        {
            public Pixel(int xPos, int yPos, ConsoleColor color)
            {
                XPos = xPos;
                YPos = yPos;
                Color = color;
            }

            public int XPos { get; set; }
            public int YPos { get; set; }
            public ConsoleColor Color { get; set; }
        }
        public enum Movement
        {
            Left,
            Right,
            Up,
            Down
        }
    }
}