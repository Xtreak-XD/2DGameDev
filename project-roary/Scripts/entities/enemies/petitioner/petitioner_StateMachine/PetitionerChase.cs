using System;
using Godot;

/// Simple horizontal patrol that flips at bounds. No player logic.
public partial class PetitionerChase : EnemyState
{
    [Export] public float Speed = 80f;             // px/s
    [Export] public float PatrolDistance = 200f;   // total width (start ± half)
    [Export] public float TurnPause = 0.08f;       // tiny pause at ends
    [Export] public bool  LockYToStart = true;     // keep to one “row”

    private float _leftX, _rightX;
    private float _rowY;
    private int _dir = +1;                         // +1 right, -1 left
    private float _pauseT = 0f;
    private bool _inited = false;

    public override void EnterState()
    {
        if (!_inited)
        {
            float startX = ActiveEnemy.GlobalPosition.X;
            _rowY = ActiveEnemy.GlobalPosition.Y;

            float half = PatrolDistance * 0.5f;
            _leftX  = startX - half;
            _rightX = startX + half;

            _inited = true;
        }

        _pauseT = 0f;
        ActiveEnemy.Velocity = Vector2.Zero;
    }

    public override EnemyState Physics(double delta)
    {
        // keep on one row (optional)
        if (LockYToStart && !Mathf.IsEqualApprox(ActiveEnemy.GlobalPosition.Y, _rowY))
            ActiveEnemy.GlobalPosition = new Vector2(ActiveEnemy.GlobalPosition.X, _rowY);

        // tiny pause when turning
        if (_pauseT > 0f)
        {
            _pauseT -= (float)delta;
            ActiveEnemy.Velocity = Vector2.Zero;
            ActiveEnemy.MoveAndSlide();
            return null;
        }

        // flip at bounds
        float x = ActiveEnemy.GlobalPosition.X;
        if (_dir > 0 && x >= _rightX) { _dir = -1; _pauseT = TurnPause; }
        else if (_dir < 0 && x <= _leftX) { _dir = +1; _pauseT = TurnPause; }

        // move horizontally
        ActiveEnemy.Velocity = new Vector2(_dir * Speed, 0f);
        ActiveEnemy.MoveAndSlide();

        // optional face-flip if you have a sprite called "petitionerface"
       // Try Sprite2D first
		var s = ActiveEnemy.GetNodeOrNull<Sprite2D>("petitionerface");
		if (s != null) {
    		s.FlipH = (_dir < 0);
		} else {
    		// Or AnimatedSprite2D if that's what you use
    		var a = ActiveEnemy.GetNodeOrNull<AnimatedSprite2D>("petitionerface");
    		if (a != null) a.FlipH = (_dir < 0);
		}

        return null; // stay in this state
    }

    public override void ExitState()
    {
        ActiveEnemy.Velocity = Vector2.Zero;
    }
}
