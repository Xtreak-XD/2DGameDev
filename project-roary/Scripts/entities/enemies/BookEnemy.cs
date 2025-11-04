using Godot;

public partial class BookEnemy : Enemy
{
    [Export] public float Gravity = 800f;
    [Export] public float FlapStrength = -400f;
    [Export] public float MaxFallSpeed = 400f;
    [Export] public float HorizontalSpeed = 200f;
    [Export] public float FlapInterval = 0.5f;
    [Export] public string FlapAnimationName = "flapping"; // Name of your animation

    private double _flapTimer = 0;
    private Node2D _player;

    // Backoff variables
    private bool _isBackingOff = false;
    private float _backoffTimer = 0f;
    private float _backoffDuration = 0.5f;

    private bool _hasDealtDamage = false;

    public override void _Ready()
    {
        base._Ready(); // Calls Enemy _Ready() for state machine & groups

        // Find the player
        _player = GetTree().GetFirstNodeInGroup("player") as Node2D;
        if (_player == null)
            GD.Print("⚠️ BookEnemy couldn't find a player in the 'player' group.");

        // Connect the damage area signal
        var area = GetNode<Area2D>("Area2D");
        area.BodyEntered += OnBodyEntered;
    }

    public override void _PhysicsProcess(double delta)
    {
        // Ensure player reference exists
        if (_player == null)
        {
            _player = GetTree().GetFirstNodeInGroup("player") as Node2D;
            if (_player == null) return;
        }

        // Handle backoff
        if (_isBackingOff)
        {
            _backoffTimer -= (float)delta;
            if (_backoffTimer <= 0)
            {
                _isBackingOff = false;
                _hasDealtDamage = false; // Allow next hit after backoff
            }
            else
            {
                Vector2 awayDir = (GlobalPosition - _player.GlobalPosition).Normalized();
                Velocity = awayDir * HorizontalSpeed * 1.2f;

                PlayFlapAnimation(awayDir.X);
                MoveAndSlide();
                return; // Skip normal chasing
            }
        }

        // Gravity (optional)
        Velocity = new Vector2(Velocity.X, Mathf.Min(Velocity.Y + Gravity * (float)delta, MaxFallSpeed));

        // Flap timer (optional for vertical movement)
        _flapTimer += delta;
        if (_flapTimer >= FlapInterval)
        {
            Flap();
            _flapTimer = 0;
        }

        // Normal chasing
        Vector2 direction = (_player.GlobalPosition - GlobalPosition).Normalized();
        Velocity = direction * HorizontalSpeed;

        PlayFlapAnimation(direction.X);
        MoveAndSlide();
    }

    private void Flap()
    {
        Velocity = new Vector2(Velocity.X, FlapStrength);
    }

    private void PlayFlapAnimation(float dirX)
    {
        if (!HasNode("AnimatedSprite2D")) return;

        var sprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
        sprite.FlipH = dirX < 0;

        if (!sprite.IsPlaying() || sprite.Animation != FlapAnimationName)
            sprite.Play(FlapAnimationName);
    }

    private void OnBodyEntered(Node body)
    {
        if (body.IsInGroup("player") && !_hasDealtDamage)
        {
            if (body is Node2D playerbody)
            {
                _hasDealtDamage = true;
                GD.Print("📕 BookEnemy hit the player!");

                // TODO: apply damage
                // ((Player)body).TakeDamage(10);

                // Start backoff
                _isBackingOff = true;
                _backoffTimer = _backoffDuration;
            }
        }
    }
}
