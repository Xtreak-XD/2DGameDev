using Godot;
using System;

public partial class petitioner : Enemy
{
    // expose patrol tuning on the root for designers
    [Export] public float PatrolSpeed = 60f;
    [Export] public float PatrolDistance = 200f;
    [Export] public float PatrolTurnPause = 0.1f;

    public override void _Ready()
    {
        base._Ready();

        var fsm = GetNode<EnemyStateMachine>("EnemyStateMachine");
        fsm.Initialize(this);

         // tune movement state
        var move = fsm.GetNodeOrNull<PetitionerChase>("movement");
        if (move != null)
        {
            move.Speed = PatrolSpeed;
            move.PatrolDistance = PatrolDistance;
            move.TurnPause = PatrolTurnPause;
        }

        // --- connect DetectionArea → approach state signals ---
        var det = GetNodeOrNull<Area2D>("DetectionArea");
        var approach = fsm.GetNodeOrNull<PetitionerApproach>("approach");
        if (det != null && approach != null)
        {
            // avoid duplicate connections on respawn
            det.BodyEntered -= approach.OnDetectionBodyEntered;
            det.BodyExited  -= approach.OnDetectionBodyExited;
            det.BodyEntered += approach.OnDetectionBodyEntered;
            det.BodyExited  += approach.OnDetectionBodyExited;
        }
    }

}

