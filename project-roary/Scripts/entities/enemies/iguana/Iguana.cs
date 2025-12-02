using System;
using Godot;

public partial class Iguana : Enemy
{
    public new IguanaStateMachine stateMachine;

    [Export] public Player target;

    public Area2D chaseDetector;  // Larger area for detecting player to chase
    public Area2D attackDetector; // Smaller area for attack range
    public AnimationPlayer anim;

    private bool playerInChaseRange = false;
    private bool playerInAttackRange = false;

    public const float ROAM_RANGE = 150f;
    private Vector2 homePos;
    private RandomNumberGenerator rng = new RandomNumberGenerator();

    public override void _Ready()
    {
        if (data != null)
        {
            data = (GenericData)data.Duplicate();
        }

        AddToGroup("enemy");

        // Get player reference
        var playerNode = GetTree().GetFirstNodeInGroup("player");
        if (playerNode != null)
        {
            target = (Player)playerNode;
        }

        anim = GetNode<AnimationPlayer>("AnimationPlayer");
        stateMachine = GetNode<IguanaStateMachine>("IguanaStateMachine");

        // Get the detection areas
        chaseDetector = GetNode<Area2D>("ChaseDetector");
        attackDetector = GetNode<Area2D>("AttackDetector");

        // Connect signals
        chaseDetector.BodyEntered += OnChaseAreaEntered;
        chaseDetector.BodyExited += OnChaseAreaExited;
        attackDetector.BodyEntered += OnAttackAreaEntered;
        attackDetector.BodyExited += OnAttackAreaExited;

        homePos = GlobalPosition;
        rng.Randomize();

        stateMachine.Initialize(this);
    }

    public override void _ExitTree()
    {
        // Disconnect signals to prevent memory leaks
        if (chaseDetector != null)
        {
            chaseDetector.BodyEntered -= OnChaseAreaEntered;
            chaseDetector.BodyExited -= OnChaseAreaExited;
        }
        if (attackDetector != null)
        {
            attackDetector.BodyEntered -= OnAttackAreaEntered;
            attackDetector.BodyExited -= OnAttackAreaExited;
        }
    }

    private void OnChaseAreaEntered(Node2D body)
    {
        if (body == target)
        {
            playerInChaseRange = true;
        }
    }

    private void OnChaseAreaExited(Node2D body)
    {
        if (body == target)
        {
            playerInChaseRange = false;
        }
    }

    private void OnAttackAreaEntered(Node2D body)
    {
        if (body == target)
        {
            playerInAttackRange = true;
        }
    }

    private void OnAttackAreaExited(Node2D body)
    {
        if (body == target)
        {
            playerInAttackRange = false;
        }
    }

    public bool IsPlayerInChaseRange()
    {
        return playerInChaseRange;
    }

    public bool IsPlayerInAttackRange()
    {
        return playerInAttackRange;
    }

    public Vector2 GetRandomPositionInRoamRange()
    {
        Vector2 center = homePos != Vector2.Zero ? homePos : GlobalPosition;
        float randomX = rng.RandfRange(-ROAM_RANGE, ROAM_RANGE);
        float randomY = rng.RandfRange(-ROAM_RANGE, ROAM_RANGE);
        return center + new Vector2(randomX, randomY);
    }

    public void AttackPlayer(Vector2 direction)
    {
        if (direction.Length() < 0.1f)
        {
            if (anim.IsPlaying())
                anim.Stop();
            return;
        }

        if (Mathf.Abs(direction.X) > Mathf.Abs(direction.Y))
        {
            if (direction.X > 0)
            {
                if (!anim.IsPlaying() || anim.CurrentAnimation != "walk_right_attack")
                {
                    anim.Play("walk_right_attack");
                }
            }
            else if (direction.X < 0)
            {
                if (!anim.IsPlaying() || anim.CurrentAnimation != "walk_left_attack")
                {
                    anim.Play("walk_left_attack");
                }
            }
        }
        else if (Mathf.Abs(direction.Y) > 0)
        {
            if (direction.Y > 0)
            {
                if (!anim.IsPlaying() || anim.CurrentAnimation != "walk_down_attack")
                {
                    anim.Play("walk_down_attack");
                }
            }
            else if (direction.Y < 0)
            {
                if (!anim.IsPlaying() || anim.CurrentAnimation != "walk_up_attack")
                {
                    anim.Play("walk_up_attack");
                }
            }
        }
    }

    public void animation(Vector2 direction)
    {
        if (direction.Length() < 0.1f)
        {
            if (anim.IsPlaying())
                anim.Stop();
            return;
        }

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
    }
}