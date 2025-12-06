using Godot;
using System;

public partial class petitioner : Enemy
{
    // expose patrol tuning on the root for designers
    [Export] public float PatrolSpeed = 60f;
    [Export] public float PatrolDistance = 200f;
    [Export] public float PatrolTurnPause = 0.1f;

    public PetitionerChase roam;
    public PetitionerApproach chase;

    public AnimationPlayer anim;

    public override void _Ready()
    {
        if (data != null)
        {
            data = (GenericData)data.Duplicate();
        }
        anim = GetNode<AnimationPlayer>("AnimationPlayer");

        var fsm = GetNode<EnemyStateMachine>("EnemyStateMachine");
        fsm.Initialize(this);
        roam = GetNode<PetitionerChase>("EnemyStateMachine/movement");
        chase = GetNode<PetitionerApproach>("EnemyStateMachine/approach");
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
            det.BodyEntered += approach.OnDetectionBodyEntered;
            det.BodyExited  += approach.OnDetectionBodyExited;
        }
    }

    public override void _Process(double delta)
    {
        if (roam.inChase)
        {
            if(roam._dir == 1)
            {
                animation(new Vector2(1,0));
            }
            else if(roam._dir == -1)
            {
                animation(new Vector2(-1,0));
            }
        }
        else if (chase.inApproach)
        {
            animation(chase.to);
        }
    }

    

    public void animation(Vector2 direction)
    {
        if (Mathf.Abs(direction.X) > Mathf.Abs(direction.Y))
        {
            if (direction.X > 0)
            {
                if (!anim.IsPlaying() || anim.CurrentAnimation != "walk_right")
                {
                    anim.Play("walk_right");
                }
            }
            else if (direction.X < 0)
            {
                if (!anim.IsPlaying() || anim.CurrentAnimation != "walk_left")
                {
                    anim.Play("walk_left");
                }
            }
        }
        else if (Mathf.Abs(direction.Y) > 0)
        {
            if (direction.Y > 0)
            {
                if (!anim.IsPlaying() || anim.CurrentAnimation != "walk_down")
                {
                    anim.Play("walk_down");
                }
            }
            else if (direction.Y < 0)
            {
                if (!anim.IsPlaying() || anim.CurrentAnimation != "walk_up")
                {
                    anim.Play("walk_up");
                }
            }
        }
        else
        {
            // Not moving, stop animation
            if (anim.IsPlaying())
                anim.Stop();
        }
    }

}

