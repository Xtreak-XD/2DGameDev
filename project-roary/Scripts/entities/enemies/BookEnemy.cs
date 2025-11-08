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
    [Export] public float TargetPointReachThreshold = 10f;
    [Export] public float TargetUpdateInterval = 0.3f;
    [Export] public float DirectChaseDuration = 3.0f;


    private Node2D _player;
    private bool _isBackingOff = false;
    private float _backoffTimer = 0f;
    private bool _hasDealtDamage = false;
    private float _flapTimer = 0f;
    private Area2D _damageArea;
    private AnimatedSprite2D _sprite;
    private bool _chasingPlayerDirectly = false;
    private Vector2 _currentTargetPoint;
    private float _targetUpdateTimer = 0f;
    private float _directChaseTimer = 0f;

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
        AssignNewRandomTargetPoint();
    }

    public float MinimumChaseDistance = 50f;
    public float MinimumSafeDistance = 15f;

    public override void _PhysicsProcess(double deltaDouble)
    {
        float delta = (float)deltaDouble;
        if (_player == null) return;

        // Reset timer if just switched to direct chase
        if (_chasingPlayerDirectly && _directChaseTimer <= 0f)
        {
            _directChaseTimer = DirectChaseDuration;
        }

        if (_chasingPlayerDirectly)
        {
            _directChaseTimer -= delta;
            if (_directChaseTimer <= 0f)
            {
                // Time to stop chasing player directly and pick a new random target
                AssignNewRandomTargetPoint();
                _chasingPlayerDirectly = false;
            }
        }
        else
        {
            // Periodically update the random target point while not directly chasing
            _targetUpdateTimer -= delta;
            if (_targetUpdateTimer <= 0f)
            {
                AssignNewRandomTargetPoint();
                _targetUpdateTimer = TargetUpdateInterval;
            }
        }

        Vector2 targetPosition = _chasingPlayerDirectly ? _player.GlobalPosition : _currentTargetPoint;
        float distanceToTarget = GlobalPosition.DistanceTo(targetPosition);
        Vector2 direction = (targetPosition - GlobalPosition).Normalized();

        if (!_chasingPlayerDirectly && distanceToTarget <= TargetPointReachThreshold)
        {
            _chasingPlayerDirectly = true;
            _directChaseTimer = DirectChaseDuration; 
        }

        float minDistance = _chasingPlayerDirectly ? MinimumChaseDistance : TargetPointReachThreshold;
        float safeDistance = _chasingPlayerDirectly ? MinimumSafeDistance : TargetPointReachThreshold / 2f;

        Vector2 velocity;

        if (distanceToTarget > minDistance)
        {
            velocity = direction * HorizontalSpeed;
        }
        else if (distanceToTarget > safeDistance)
        {
            float speedFactor = (distanceToTarget - safeDistance) / (minDistance - safeDistance);
            velocity = direction * HorizontalSpeed * speedFactor;
        }
        else
        {
            velocity = Vector2.Zero;
        }

        velocity.Y += Gravity * delta;
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

    private void AssignNewRandomTargetPoint()
    {
        if (_player == null) return;

        var randomOffset = new Vector2(
            (float)GD.RandRange(-TargetPointRadius, TargetPointRadius),
            (float)GD.RandRange(-TargetPointRadius, TargetPointRadius));

        _currentTargetPoint = _player.GlobalPosition + randomOffset;
        _chasingPlayerDirectly = false;
    }
}
