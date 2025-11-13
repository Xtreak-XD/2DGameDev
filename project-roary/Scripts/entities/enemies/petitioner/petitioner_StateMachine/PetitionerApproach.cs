using Godot;
using System;
public partial class PetitionerApproach : EnemyState
{
    [Export] public float ApproachSpeed = 110f;
    [Export] public bool  HorizontalOnly = false;

    // NEW: keep a little gap so we don’t stick to the player
    [Export] public float StopDistance = 14f;   // pixels center-to-center
    [Export] public float Decel = 900f;         // how fast we slow to a stop
    [Export] public float ExitDelay = 0.15f;    // anti-bounce when leaving zone

    public Vector2 to;
    private CharacterBody2D _player;
    private Area2D _det;
    private EnemyStateMachine _fsm;
    private float _leaveT;
    public bool inApproach = false;
    public override void EnterState()
    {
        inApproach = true;
        _fsm  ??= GetParent<EnemyStateMachine>();
        _det  ??= ActiveEnemy.GetNodeOrNull<Area2D>("DetectionArea");
        _player ??= ActiveEnemy.GetTree().GetFirstNodeInGroup("player") as CharacterBody2D;

        _leaveT = ExitDelay;
        ActiveEnemy.Velocity = Vector2.Zero;
    }

    public override void ExitState()
    {
        inApproach = false;
        ActiveEnemy.Velocity = Vector2.Zero;
    }

    // ---- signals (you already connected these from petitioner.cs) ----
    public void OnDetectionBodyEntered(Node body)
    {
        if (!body.IsInGroup("player")) return;
        _player = body as CharacterBody2D;
        _fsm?.ChangeState(this);
    }

    public void OnDetectionBodyExited(Node body)
    {
        if (!body.IsInGroup("player")) return;
        _leaveT = ExitDelay;
    }

    public override EnemyState Physics(double delta)
    {
        if (_player == null)
            return GetParent().GetNodeOrNull<EnemyState>("movement");

        // leave after a brief cooldown if player no longer overlaps
        if (_det != null && !_det.OverlapsBody(_player))
        {
            _leaveT -= (float)delta;
            if (_leaveT <= 0f)
                return GetParent().GetNodeOrNull<EnemyState>("movement");
        }
        else
        {
            _leaveT = ExitDelay;
        }

        // === chase WITHOUT attaching ===
        if (HorizontalOnly)
        {
            float dx = _player.GlobalPosition.X - ActiveEnemy.GlobalPosition.X;
            if (Mathf.Abs(dx) > StopDistance)
            {
                float sx = Mathf.Sign(dx);
                ActiveEnemy.Velocity = new Vector2(sx * ApproachSpeed, 0f);
            }
            else
            {
                // within cushion -> smoothly stop so we don't glue to the player
                ActiveEnemy.Velocity = ActiveEnemy.Velocity.MoveToward(Vector2.Zero, Decel * (float)delta);
            }
        }
        else
        {
            to = _player.GlobalPosition - ActiveEnemy.GlobalPosition;
            float d = to.Length();
            if (d > StopDistance)
            {
                ActiveEnemy.Velocity = to / d * ApproachSpeed;
            }
            else
            {
                ActiveEnemy.Velocity = ActiveEnemy.Velocity.MoveToward(Vector2.Zero, Decel * (float)delta);
            }
        }

        // flip without changing size
        var face = ActiveEnemy.GetNodeOrNull<Sprite2D>("petitionerface");
        if (face != null) face.FlipH = (ActiveEnemy.Velocity.X < 0f);

        ActiveEnemy.MoveAndSlide();
        return null;
    }
}
