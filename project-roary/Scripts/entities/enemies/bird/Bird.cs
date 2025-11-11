using Godot;
using System;

public partial class Bird : CharacterBody2D
{
    public enum FlyDirection { Right, Left }

    [Export] public BirdData BirdProperties;
    [Export] public FlyDirection Direction = FlyDirection.Right;

    private float _timeUntilDrop;
    private bool _hasDropped = false;
    private AnimatedSprite2D _sprite;

    public override void _EnterTree()
    {
        float ySpawn = (float)GD.RandRange(30f, 120f);

        if (Direction == FlyDirection.Right)
            GlobalPosition = new Vector2(-50f, ySpawn);
        else
            GlobalPosition = new Vector2(GetViewportRect().Size.X + 50f, ySpawn);
    }

    public override void _Ready()
    {
        if (BirdProperties == null)
        {
            GD.PrintErr("BirdProperties resource not assigned!");
            return;
        }

        _sprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");

        // Play appropriate animation
        if (Direction == FlyDirection.Right)
            _sprite.Play(BirdProperties.FlyRightAnimation);
        else
            _sprite.Play(BirdProperties.FlyLeftAnimation);

        // Set first random drop time
        _timeUntilDrop = (float)GD.RandRange(BirdProperties.MinDropTime, BirdProperties.MaxDropTime);
    }

    public override void _PhysicsProcess(double delta)
    {
        if (BirdProperties == null) return;

        // Move bird
        Velocity = new Vector2(BirdProperties.Speed * (Direction == FlyDirection.Right ? 1 : -1), 0);
        MoveAndSlide();

        // Drop poop randomly once
        if (!_hasDropped)
        {
            _timeUntilDrop -= (float)delta;
            if (_timeUntilDrop <= 0f)
            {
                DropPoop();
                _hasDropped = true;
            }
        }

        // Remove bird if off-screen
        if ((Direction == FlyDirection.Right && GlobalPosition.X > GetViewportRect().Size.X + 60) ||
            (Direction == FlyDirection.Left && GlobalPosition.X < -60))
            QueueFree();
    }

    private void DropPoop()
    {
        if (BirdProperties?.PoopScene == null)
        {
            GD.Print("BirdProperties or PoopScene not set!");
            return;
        }

        var poop = (BirdPoop)BirdProperties.PoopScene.Instantiate();
        GD.Print("Poop instantiated!");

        // Offset slightly below bird
        float xOffset = -80f; 
        float yOffset = 60f;

        // Small horizontal randomness
        xOffset += (float)GD.RandRange(-5f, 5f);

        poop.GlobalPosition = GlobalPosition + new Vector2(xOffset, yOffset);

        // Add to main scene so poop falls independently
        GetTree().Root.GetChild(0).AddChild(poop);
    }
}