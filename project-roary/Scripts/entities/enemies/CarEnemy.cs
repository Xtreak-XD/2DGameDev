using System;
using Godot;

public partial class CarEnemy : Enemy
{
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
                Velocity = Vector2.Up * data.Speed * ((float)delta * (float)data.Accel);
                RotationDegrees = 270;
                break;
            case directionChosen.East:
                Velocity = Vector2.Right * data.Speed * ((float)delta * (float)data.Accel);
                RotationDegrees = 0;
                break;
            case directionChosen.South:
                Velocity = Vector2.Down * data.Speed * ((float)delta * (float)data.Accel);
                RotationDegrees = 90;
                break;
            case directionChosen.West:
                Velocity = Vector2.Left * data.Speed * ((float)delta * (float)data.Accel);
                RotationDegrees = 180;
                break;
        }
    }

    public override void _PhysicsProcess(double delta)
    {
        MoveAndSlide();
    }
}
