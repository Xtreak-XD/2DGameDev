using Godot;

public partial class CoffeeShot : Node2D
{
    //Fires a large wave of coffee as knockback
    [Export] public float Speed = 1000f;
    [Export] public float Lifetime = 2f;

    private float _timer = 0f;
    private Vector2 _velocity = Vector2.Zero;

    public override void _PhysicsProcess(double delta)
    {
        _timer += (float)delta;

        if (_timer >= Lifetime)
        {
            QueueFree();
            return;
        }

        GlobalPosition += _velocity * (float)delta;
    }

    public void SetDirection(Vector2 dir)
    {
        _velocity = dir.Normalized() * Speed;
    }

    public static readonly Vector2[] CardinalDirections = 
    {
        Vector2.Up,
        Vector2.Down,
        Vector2.Left,
        Vector2.Right
    };

    public void Fire(Logo logo)
    {
        int index = (int)(GD.Randi() % CardinalDirections.Length);
        Vector2 dir = CardinalDirections[index];

        SetDirection(dir);

        GlobalPosition = logo.GlobalPosition;
    }
}
