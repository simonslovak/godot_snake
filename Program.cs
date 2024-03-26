using Godot;
using System;
using System.Collections.Generic;

public partial class SnakeGame : Node2D
{
    private const int screenWidth = 32;
    private const int screenHeight = 16;

    private Random randomNumber = new Random();
    private Movement movement = Movement.Right;
    private int score = 5;
    private bool isGameOver = false;

    private Pixel snakeHead = new Pixel(screenWidth / 2, screenHeight / 2, Colors.Red);
    private List<Pixel> snakeBody = new List<Pixel>();
    private Pixel berry = new Pixel();

    public override void _Ready()
    {
        GenerateBerry();
    }

    public override void _Process(double delta)
    {
        ProcessInput();
    }

    public override void _PhysicsProcess(double delta)
    {
        MoveSnake();
    }

    private void GenerateBerry()
    {
        berry.XPos = randomNumber.Next(1, screenWidth - 2);
        berry.YPos = randomNumber.Next(1, screenHeight - 2);
    }

    private void ProcessInput()
    {
        if (Input.IsActionPressed("ui_up") && movement != Movement.Down)
            movement = Movement.Up;
        else if (Input.IsActionPressed("ui_down") && movement != Movement.Up)
            movement = Movement.Down;
        else if (Input.IsActionPressed("ui_left") && movement != Movement.Right)
            movement = Movement.Left;
        else if (Input.IsActionPressed("ui_right") && movement != Movement.Left)
            movement = Movement.Right;
    }

    private void MoveSnake()
    {
        snakeBody.Insert(0, new Pixel(snakeHead.XPos, snakeHead.YPos, Colors.Green));

        switch (movement)
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

        if (snakeHead.XPos == berry.XPos && snakeHead.YPos == berry.YPos)
        {
            score++;
            GenerateBerry();
        }
        else if (IsSnakeCollidingWithBody(snakeHead))
        {
            isGameOver = true;
        }

        if (snakeBody.Count > score)
            snakeBody.RemoveAt(snakeBody.Count - 1);
    }

    private bool IsSnakeCollidingWithBody(Pixel snakeHead)
    {
        foreach (var bodyPart in snakeBody)
        {
            if (bodyPart.XPos == snakeHead.XPos && bodyPart.YPos == snakeHead.YPos)
                return true;
        }
        return false;
    }

    public override void _Draw()
    {
        DrawBorder();

        foreach (var bodyPart in snakeBody)
        {
            DrawPixel(bodyPart);
        }

        DrawPixel(snakeHead);
        DrawPixel(berry);
    }

    private void DrawBorder()
    {
        // Draw top border
        DrawRect(new Rect2(0, 0, screenWidth, 1), Colors.White);
        // Draw bottom border
        DrawRect(new Rect2(0, screenHeight - 1, screenWidth, 1), Colors.White);
        // Draw left border
        DrawRect(new Rect2(0, 0, 1, screenHeight), Colors.White);
        // Draw right border
        DrawRect(new Rect2(screenWidth - 1, 0, 1, screenHeight), Colors.White);
    }

    private void DrawPixel(Pixel pixel)
    {
        DrawRect(new Rect2(pixel.XPos, pixel.YPos, 1, 1), pixel.Color);
    }

    private class Pixel
    {
        public int XPos { get; set; }
        public int YPos { get; set; }
        public Color Color { get; set; }

        public Pixel()
        {
        }

        public Pixel(int xPos, int yPos, Color color)
        {
            XPos = xPos;
            YPos = yPos;
            Color = color;
        }
    }

    public enum Movement
    {
        Left,
        Right,
        Up,
        Down
    }
}
