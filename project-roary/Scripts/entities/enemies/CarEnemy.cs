using System;
using Godot;

public partial class CarEnemy : Enemy
{
    public enum directionChosen
    {
        North,
        East,
        South,
        West
    }

    [Export] public directionChosen currentDirection { get; set; } = directionChosen.North;

    public AnimationPlayer anim;

    private float despawnTimer;

    public override void _Ready()
    {
        anim = GetNode<AnimationPlayer>("AnimationPlayer");
        despawnTimer = OffScreenDespawnTime;
    }

    public override void _Process(double delta)
    {
        switch (currentDirection)
        {
            case directionChosen.North:
                Velocity = Vector2.Up * data.Speed * ((float)delta * (float)data.Accel);
                anim.Play("north");
                break;
            case directionChosen.East:
                Velocity = Vector2.Right * data.Speed * ((float)delta * (float)data.Accel);
                anim.Play("east");
                break;
            case directionChosen.South:
                Velocity = Vector2.Down * data.Speed * ((float)delta * (float)data.Accel);
                anim.Play("south");
                break;
            case directionChosen.West:
                Velocity = Vector2.Left * data.Speed * ((float)delta * (float)data.Accel);
                anim.Play("west");
                break;
        }
        despawnTimer -= (float)delta;
        if (despawnTimer <= 0f)
        {
            QueueFree();
            return;
        }
    }

    public override void _PhysicsProcess(double delta)
    {
        MoveAndSlide();
    }
}
