using Godot;
using System;

public partial class BirdPoop : CharacterBody2D
{
    [Export] public float FallSpeed = 200f;
    [Export] public float GroundTime = 7.5f;

    private AnimatedSprite2D _sprite;
    private bool _exploded = false;

    private float _explosionY;

    public override void _Ready()
    {
        _sprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
        _sprite.Play("PooFall");

        // Get screen height
        float screenBottom = GetViewportRect().Size.Y - 50f; // 50 px margin

        // Clamp explosion point: always **below starting Y**, but not below bottom
        float minY = GlobalPosition.Y + 40f; // just below bird
        float maxY = screenBottom;
        if (minY > maxY)
            minY = maxY - 10f; // small fallback if bird is too low

        _explosionY = (float)GD.RandRange(minY, maxY);
    }

    public override void _PhysicsProcess(double delta)
    {
        if (_exploded) return;

        // move down
        Velocity = new Vector2(0, FallSpeed);
        MoveAndSlide();

        // Trigger explosion when reaching random Y
        if (GlobalPosition.Y >= _explosionY)
        {
            Explode();
        }
    }

    private void Explode()
    {
        if (_exploded) return;
        _exploded = true;

        _sprite.Play("PooLand");

        Timer t = new Timer();
        t.WaitTime = 0.25f;
        t.OneShot = true;
        t.Timeout += BecomeGround;
        AddChild(t);
        t.Start();
    }

    private void BecomeGround()
    {
        _sprite.Play("PooGround");

        Timer deleteTimer = new Timer();
        deleteTimer.WaitTime = GroundTime;
        deleteTimer.OneShot = true;
        deleteTimer.Timeout += () => QueueFree();
        AddChild(deleteTimer);
        deleteTimer.Start();
    }
}