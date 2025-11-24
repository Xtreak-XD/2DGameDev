using Godot;

public partial class SpiralBubble : Node2D
{
    //Fires bursts of bubbles each idlestate
    [Export] public float Speed = 200f;
    [Export] public float Lifetime = 3f;

    private float _lifeTimer = 0f;
    private Vector2 _velocity = Vector2.Zero;

    public override void _PhysicsProcess(double delta)
    {
        _lifeTimer += (float)delta;
        
        if (_lifeTimer >= Lifetime)
        {
            QueueFree();
            return;
        }

        Position += _velocity * (float)delta;
    }

    public void SetVelocity(Vector2 v)
    {
        _velocity = v.Normalized() * Speed;
    }
}