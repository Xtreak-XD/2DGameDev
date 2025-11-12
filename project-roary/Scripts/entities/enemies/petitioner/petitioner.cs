using Godot;
using System;

public partial class Petitioner : Enemy
{
    // expose patrol tuning on the root for designers
    [Export] public float PatrolSpeed = 60f;
    [Export] public float PatrolDistance = 200f;
    [Export] public float PatrolTurnPause = 0.1f;

    public override void _Ready()
    {
        base._Ready();

        // state machine already in your scene
        var fsm = GetNode<EnemyStateMachine>("EnemyStateMachine");
        fsm.Initialize(this);

        // child state is named "movement" in your screenshot
        var move = fsm.GetNodeOrNull<PetitionerChase>("movement");
        if (move != null)
        {
            move.Speed = PatrolSpeed;
            move.PatrolDistance = PatrolDistance;
            move.TurnPause = PatrolTurnPause;
        }

        // (Optional) If your Hitbox is present but you don’t want contact damage now:
        // var hb = GetNodeOrNull<Hitbox>("Hitbox");
        // if (hb != null) { hb.ContinuousDamage = false; hb.Monitoring = false; }
    }
}

