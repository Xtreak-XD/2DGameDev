using Godot;

public partial class BookEnemy : CharacterBody2D
{
    [Export] public float Gravity = 800f;
    [Export] public float MaxFallSpeed = 400f;
    [Export] public float HorizontalSpeed = 1;
    [Export] public float BackoffSpeed = 250f;
    [Export] public float DetectionRange = 400f;
    [Export] public float BackoffDuration = 1.0f;
    [Export] public string FlapAnimationName = "flapping";
    [Export] public float FlapStrength = -400f;
    [Export] public float FlapInterval = 0.5f;
    [Export] public float TargetPointRadius = 80f;
    [Export] public float TargetPointReachedThreshold = 10f;

    private Node2D _player;
    private bool _isBackingOff = false;
    private float _backoffTimer = 0f;
    private bool _hasDealtDamage = false;
    private float _flapTimer = 0f;
    private Area2D _damageArea;
    private AnimatedSprite2D _sprite;
    private bool _chasingPlayerDirectly = false;
    private Vector2 _currentTargetPoint;

    public override void _Ready()
    {
        _player = GetTree().GetFirstNodeInGroup("player") as Node2D;
        if (_player == null)
        {
            GD.PrintErr("⚠️ BookEnemy: player not found in 'player' group!");
        }

        _damageArea = GetNodeOrNull<Area2D>("DamageArea");
        if (_damageArea != null)
        {
            _damageArea.BodyEntered += OnBodyEntered;
        }
        else
        {
            GD.PrintErr("❌ DamageArea not found!");
        }

        _sprite = GetNodeOrNull<AnimatedSprite2D>("AnimatedSprite2D");
        if (_sprite == null)
        {
            GD.PrintErr("❌ AnimatedSprite2D node not found!");
        }
    }

    public float MinimumChaseDistance = 50f;
    public float MinimumSafeDistance = 15f;

    public override void _PhysicsProcess(double deltaDouble)
    {
        float delta = (float)deltaDouble;
        if (_player == null) return;

        Vector2 velocity = Velocity;

        if (_isBackingOff)
        {
            _backoffTimer -= delta;
            if (_backoffTimer <= 0f)
            {
                _isBackingOff = false;
                _hasDealtDamage = false;
            }
            else
            {
                Vector2 retreatDir = (GlobalPosition - _player.GlobalPosition).Normalized();
                velocity = retreatDir * BackoffSpeed;

                velocity.Y += Gravity * delta;
                velocity.Y = Mathf.Min(velocity.Y, MaxFallSpeed);

                Velocity = velocity;
                MoveAndSlide();

                PlayFlapAnimation(retreatDir);
                return;
            }
        }

        float distance = GlobalPosition.DistanceTo(_player.GlobalPosition);
        Vector2 direction = (_player.GlobalPosition - GlobalPosition).Normalized();

        if (distance > MinimumChaseDistance)
        {
            velocity = direction * HorizontalSpeed;
            velocity.Y += Gravity * delta;
        }
        else if (distance > MinimumSafeDistance)
        {
            float speedFactor = (distance - MinimumSafeDistance) / (MinimumChaseDistance - MinimumSafeDistance);
            velocity = direction * HorizontalSpeed * speedFactor;
            velocity.Y += Gravity * delta;
        }
        else
        {
            velocity = Vector2.Zero;
        }

        velocity.Y = Mathf.Min(velocity.Y, MaxFallSpeed);

        Velocity = velocity;
        MoveAndSlide();

        PlayFlapAnimation(direction);
    }




    private void PlayFlapAnimation(Vector2 direction)
    {
        if (_sprite == null) return;

        if (Mathf.Abs(direction.X) > Mathf.Abs(direction.Y))
        {
            if (direction.X > 0)
            {
                if (!_sprite.IsPlaying() || _sprite.Animation != "flapping_right")
                {
                    _sprite.Play("flapping_right");
                }
            }
            else if (direction.X < 0)
            {
                if (!_sprite.IsPlaying() || _sprite.Animation != "flapping_left")
                {
                    _sprite.Play("flapping_left");
                }
            }
        }
        else if (Mathf.Abs(direction.Y) > 0)
        {
            if (direction.Y > 0)
            {
                if (!_sprite.IsPlaying() || _sprite.Animation != "flapping_down")
                {
                    _sprite.Play("flapping_down");
                }
            }
            else if (direction.Y < 0)
            {
                if (!_sprite.IsPlaying() || _sprite.Animation != "flapping_up")
                {
                    _sprite.Play("flapping_up");
                }
            }
        }
        else
        {
            // Not moving, stop animation
            if (_sprite.IsPlaying())
                _sprite.Stop();
        }
    }

    private void OnBodyEntered(Node body)
    {
        if (body.IsInGroup("player") && !_hasDealtDamage)
        {
            _hasDealtDamage = true;
            GD.Print("📕 BookEnemy hit the player!");

            _isBackingOff = true;
            _backoffTimer = BackoffDuration;

            // TODO: apply damage to player here
            // ((Player)body).TakeDamage(10);
        }
    }
}
