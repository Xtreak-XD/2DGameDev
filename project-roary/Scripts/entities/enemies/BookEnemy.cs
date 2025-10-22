using Godot;

public partial class BookEnemy : Enemy
{
    [Export] public float Gravity = 800f;
    [Export] public float FlapStrength = -400f;
    [Export] public float MaxFallSpeed = 400f;
    [Export] public float HorizontalSpeed = 200f;
    [Export] public float FlapInterval = 0.5f;

    private double _flapTimer = 0;
    private Node2D _player;

    public override void _Ready()
    {
        base._Ready(); // Call parent _Ready() to set up EnemyStateMachine and group

        // Find the player in the scene
        _player = GetTree().GetFirstNodeInGroup("player") as Node2D;

        if (_player == null)
            GD.Print("⚠️ BookEnemy couldn't find a player in the 'player' group.");
    }

    public override void _PhysicsProcess(double delta)
    {
        if (_player == null)
        {
            // Try to find the player again if lost
            _player = GetTree().GetFirstNodeInGroup("player") as Node2D;
            return;
        }

        // Gravity
        Velocity = new Vector2(Velocity.X, Mathf.Min(Velocity.Y + Gravity * (float)delta, MaxFallSpeed));

        // Flap periodically
        _flapTimer += delta;
        if (_flapTimer >= FlapInterval)
        {
            Flap();
            _flapTimer = 0;
        }

        // Move toward player horizontally
        float dirX = Mathf.Sign(_player.GlobalPosition.X - GlobalPosition.X);
        Velocity = new Vector2(dirX * HorizontalSpeed, Velocity.Y);

        // Optional: flip sprite if needed
        if (HasNode("AnimatedSprite2D"))
        {
            var sprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
            sprite.FlipH = dirX < 0;
        }

        MoveAndSlide();
    }

    private void Flap()
    {
        Velocity = new Vector2(Velocity.X, FlapStrength);

        // Play flap animation
        if (HasNode("AnimatedSprite2D"))
        {
            var sprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
            sprite.Play("flap");
        }
    }
}
