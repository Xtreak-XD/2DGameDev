using Godot;
using System;

public enum RoaryPhase {
    FIRST,
    SECOND,
    THIRD
}

public partial class Roary : Enemy
{
	public new RoaryStateMachine stateMachine;
	public Hitbox hitbox;
	public HurtBox hurtBox;
	public Marker2D projectileSource;
	public Player target;
	public AnimationPlayer anim;
    public Timer targetTimer;

    public RoaryPhase Phase { get; set; } = RoaryPhase.FIRST;

    [Export]
    public PackedScene footballCharge;
    
    [Export]
	public PackedScene football;

    [Export]
	public PackedScene shadowPaw;

	public const int ROAM_RANGE = 650;

	public override void _Ready()
    {
        stateMachine = GetNode<RoaryStateMachine>("RoaryStateMachine");
        stateMachine.Initialize(this);

		hitbox = GetNode<Hitbox>("Hitbox");
		hurtBox = GetNode<HurtBox>("HurtBox");
		projectileSource = GetNode<Marker2D>("ProjectileSource");
		anim = GetNode<AnimationPlayer>("AnimationPlayer");
        targetTimer = GetNode<Timer>("InitializePlayerTargetting");

        targetTimer.Timeout += SetTarget;
        targetTimer.Start();
    }

    public override void _EnterTree()
    {
        AddToGroup("enemy");
    }

    public void SetTarget()
    {
        target = (Player)GetTree().GetFirstNodeInGroup("player");

        if(target == null)
        {
            GD.Print("Player could not be found.");
        }
    }

	public Vector2 GetRandomPositionInRoamRange()
	{
		Random random = new Random();

		float roamRange = ROAM_RANGE;

		float randomX = random.Next((int)-roamRange, (int)roamRange + 1);
		float randomY = random.Next((int)-roamRange, (int)roamRange + 1);

		return GlobalPosition + new Vector2(randomX, randomY);
	}

    public float GetHealthPercentage()
    {
        return data.Health / data.MaxHealth;
    }

    public float StatMultipler()
    {
        return 1 + ((1 - GetHealthPercentage()) / 2);
    }

    public int TrueSpeed()
    {
        return (int)(data.Speed * StatMultipler());
    }

    public int TrueAcceleration()
    {
        return (int)(data.Accel * StatMultipler());
    }

    public int TrueDamage()
    {
        return (int)(data.Damage * StatMultipler());
    }

    public RoaryState PreviousState()
    {
        return stateMachine.previousState;
    }

    public void AdvancePhase()
    {
        if(Phase == RoaryPhase.THIRD)
        {
            return;
        }
        
        if(Phase == RoaryPhase.FIRST)
        {
            Phase = RoaryPhase.SECOND;
        }
        else if(Phase == RoaryPhase.SECOND)
        {
            Phase = RoaryPhase.THIRD;
        }

        GD.Print("Roary has advanced phases");
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
