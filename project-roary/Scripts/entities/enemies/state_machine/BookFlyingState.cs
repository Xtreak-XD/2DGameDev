using Godot;

public partial class BookFlyingState : EnemyState
{
    [Export] public float Gravity = 800f;
    [Export] public float FlapStrength = -400f;
    [Export] public float MaxFallSpeed = 400f;
    [Export] public float HorizontalSpeed = 200f;
    [Export] public float FlapInterval = 0.5f;

    private double _flapTimer = 0;
    private Node2D _player;

    public override void EnterState()
    {
        // Find the player once
        _player = GetTree().GetFirstNodeInGroup("player") as Node2D;

        if (_player == null)
            GD.Print("⚠️ BookFlyingState couldn't find player!");
    }

    public override EnemyState Physics(double delta)
    {
        GD.Print("in flying state");
        if (_player == null)
        {
            _player = GetTree().GetFirstNodeInGroup("player") as Node2D;
            return null;
        }

        // Gravity
        ActiveEnemy.Velocity = new Vector2(ActiveEnemy.Velocity.X, Mathf.Min(ActiveEnemy.Velocity.Y + Gravity * (float)delta, MaxFallSpeed));

        // Flap periodically
        _flapTimer += delta;
        if (_flapTimer >= FlapInterval)
        {
            ActiveEnemy.Velocity = new Vector2(ActiveEnemy.Velocity.X, FlapStrength);

            // Play flap animation if available
            // if (ActiveEnemy.HasNode("AnimatedSprite2D"))
            // {
            //     var sprite = ActiveEnemy.GetNode<AnimatedSprite2D>("AnimatedSprite2D");
            //     sprite.Play("flapping");
            // }

            _flapTimer = 0;
        }

        // Move toward player horizontally
        float dirX = Mathf.Sign(_player.GlobalPosition.X - ActiveEnemy.GlobalPosition.X);
        ActiveEnemy.Velocity = new Vector2(dirX * HorizontalSpeed, ActiveEnemy.Velocity.Y);

        // // Flip sprite
        // if (ActiveEnemy.HasNode("AnimationPlayer"))
        //     ActiveEnemy.GetNode<AnimationPlayer>("AnimationPlayer").FlipH = dirX < 0;

        ActiveEnemy.MoveAndSlide();

        return null;
    }
}
