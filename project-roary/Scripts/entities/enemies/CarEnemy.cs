using System;
using Godot;

public partial class CarEnemy : Enemy
{
    [Export] int speed = 200;
    private enum directionChosen
    {
        North,
        East,
        South,
        West
    }

    [Export] private directionChosen currentDirection { get; set; } = directionChosen.North;

    public override void _Ready()
    {
    }

    public override void _Process(double delta)
    {
        switch (currentDirection)
        {
            case directionChosen.North:
                Velocity = Vector2.Up * speed * (float)delta;
                RotationDegrees = 270;
                break;
            case directionChosen.East:
                Velocity = Vector2.Right * speed * (float)delta;
                RotationDegrees = 0;
                break;
            case directionChosen.South:
                Velocity = Vector2.Down * speed * (float)delta;
                RotationDegrees = 90;
                break;
            case directionChosen.West:
                Velocity = Vector2.Left * speed * (float)delta;
                RotationDegrees = 180;
                break;
        }
    }

    public override void _PhysicsProcess(double delta)
    {
        MoveAndSlide();
    }
}
