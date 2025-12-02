using System;
using Godot;

/// Simple horizontal patrol that flips at bounds. No player logic.
public partial class PetitionerChase : EnemyState
{
    [Export] public float Speed = 800f;             // px/s
    [Export] public float PatrolDistance = 200f;   // total width (start ± half)
    [Export] public float TurnPause = 0.08f;       // tiny pause at ends
    [Export] public bool  LockYToStart = true;     // keep to one "row"
    [Export] public float ReturnSpeed   = 140f;    // how fast to slide back to row
    [Export] public float ReturnEpsilon = 2f;      // snap when very close
    
    private float _leftX, _rightX;
    private float _rowY;
    public int _dir = +1;                          // +1 right, -1 left
    private float _pauseT = 0f;
    private bool _inited = false;
    
    private Area2D _det;
    private CharacterBody2D _player;
    public bool inChase = false;
    private petitioner petitioner;

    public override void EnterState()
    {
        inChase = true;
        
        _det    = ActiveEnemy.GetNodeOrNull<Area2D>("DetectionArea");
        _player = ActiveEnemy.GetTree().GetFirstNodeInGroup("player") as CharacterBody2D;
        
        _pauseT = 0f;
        ActiveEnemy.Velocity = Vector2.Zero;
    }

    public override EnemyState Physics(double delta)
    {
        // Initialize on first physics frame (when position is correct)
        if (!_inited)
        {
            float startX = ActiveEnemy.GlobalPosition.X;
            _rowY = ActiveEnemy.GlobalPosition.Y;
            float half = PatrolDistance * 0.5f;
            _leftX = startX - half;
            _rightX = startX + half;
            _inited = true;
            
            //GD.Print($"Petitioner patrol initialized at X:{startX}, Y:{_rowY}, bounds:[{_leftX}, {_rightX}]");
        }
        
        // 1) Switch to approach if player in zone
        if (_det != null && _player != null && _det.OverlapsBody(_player))
            return GetParent().GetNodeOrNull<EnemyState>("approach");

        // 2) Turn-around pause
        if (_pauseT > 0f)
        {
            _pauseT -= (float)delta;
            ActiveEnemy.Velocity = Vector2.Zero;
            ActiveEnemy.MoveAndSlide();
            return null;
        }

        // 3) Flip direction at bounds
        float x = ActiveEnemy.GlobalPosition.X;
        if (_dir > 0 && x >= _rightX) { _dir = -1; _pauseT = TurnPause; }
        else if (_dir < 0 && x <= _leftX) { _dir = +1; _pauseT = TurnPause; }

        // 4) Compute desired horizontal velocity
        float vx = _dir * Speed;

        // 5) SMOOTH rejoin to the patrol row (no teleport)
        float vy = 0f;
        if (LockYToStart)
        {
            float dy = _rowY - ActiveEnemy.GlobalPosition.Y;
            if (Mathf.Abs(dy) > ReturnEpsilon)
            {
                vy = Mathf.Sign(dy) * ReturnSpeed; // glide back to row
            }
            else
            {
                // tiny snap when within epsilon to kill float drift
                ActiveEnemy.GlobalPosition = new Vector2(ActiveEnemy.GlobalPosition.X, _rowY);
                vy = 0f;
            }
        }

        // 6) Apply both components
        ActiveEnemy.Velocity = new Vector2(vx, vy);
        ActiveEnemy.MoveAndSlide();
        return null;
    }

    public override void ExitState()
    {
        ActiveEnemy.Velocity = Vector2.Zero;
        inChase = false;
    }
}