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
                GetNode<Sprite2D>("Sprite2D").RotationDegrees = 0;
                GetNode<Area2D>("Hitbox").RotationDegrees = 0;
                GetNode<CollisionShape2D>("CollisionShape2D").RotationDegrees = 0;
                break;
            case directionChosen.East:
                Velocity = Vector2.Right * speed * (float)delta;
                GetNode<Sprite2D>("Sprite2D").RotationDegrees = 90;
                GetNode<Area2D>("Hitbox").RotationDegrees = 90;
                GetNode<CollisionShape2D>("CollisionShape2D").RotationDegrees = 90;
                break;
            case directionChosen.South:
                Velocity = Vector2.Down * speed * (float)delta;
                GetNode<Sprite2D>("Sprite2D").RotationDegrees = 180;
                GetNode<Area2D>("Hitbox").RotationDegrees = 180;
                GetNode<CollisionShape2D>("CollisionShape2D").RotationDegrees = 180;
                break;
            case directionChosen.West:
                Velocity = Vector2.Left * speed * (float)delta;
                GetNode<Sprite2D>("Sprite2D").RotationDegrees = 270;
                GetNode<Area2D>("Hitbox").RotationDegrees = 270;
                GetNode<CollisionShape2D>("CollisionShape2D").RotationDegrees = 270;
                break;
        }

        MoveAndSlide();
    }
}
