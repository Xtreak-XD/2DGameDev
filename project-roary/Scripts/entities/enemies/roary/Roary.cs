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

    [Export]
    public PackedScene roarIndication;

    [Export]
    public PackedScene firework;

    [Export]
    public PackedScene orbitalHead;

    public bool CanAttack {get; set; } = true;

    public Timer GlobalAttackTimer;

	public const int ROAM_RANGE = 650;

    public bool SummonedFirstStampede { get; set; } = false;
    public bool SummonedSecondStampede { get; set; } = false;
    public bool SummonedThirdStampede { get; set; } = false;

    SaveManager saveManager;
	public override void _Ready()
    {
        saveManager = GetNode<SaveManager>("/root/SaveManager");

        stateMachine = GetNode<RoaryStateMachine>("RoaryStateMachine");
        stateMachine.Initialize(this);

		hitbox = GetNode<Hitbox>("Hitbox");
		hurtBox = GetNode<HurtBox>("HurtBox");
		projectileSource = GetNode<Marker2D>("ProjectileSource");
		anim = GetNode<AnimationPlayer>("AnimationPlayer");
        targetTimer = GetNode<Timer>("InitializePlayerTargetting");
        GlobalAttackTimer = GetNode<Timer>("RoaryGlobalAttackTimer");

        GlobalAttackTimer.Timeout += EnableAttack;

        targetTimer.Timeout += SetTarget;
        targetTimer.Start();

        // Initialize animation to sync collision shapes with sprite
        anim.Play("Walk_Right");
    }

    public override void _EnterTree()
    {
        AddToGroup("enemy");
    }

    public override void _ExitTree()
    {
        base._ExitTree();

        GlobalAttackTimer.Timeout -= EnableAttack;
        targetTimer.Timeout -= SetTarget;
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
        return (float) data.Health / (float) data.MaxHealth;
    }

    public float StatMultipler()
    {
        return 1 + ((1 - GetHealthPercentage()) * 0.4f);
    }

    public int TrueSpeed()
    {
        return (int)(data.Speed * StatMultipler());
    }

    public int TrueAcceleration()
    {
        return (int)(data.Accel * StatMultipler());
    }

    public RoaryState PreviousState()
    {
        return stateMachine.previousState;
    }

    public void EnableAttack()
    {
        CanAttack = true;
    }

    public void AdvancePhase()
    {
        if(Phase == RoaryPhase.THIRD)
    {
        GD.Print("Already at THIRD phase, not advancing");
        return;
    }
    
        if(Phase == RoaryPhase.FIRST)
        {
            Phase = RoaryPhase.SECOND;
            GD.Print("Advanced from FIRST to SECOND");
        }
        else if(Phase == RoaryPhase.SECOND)
        {
            Phase = RoaryPhase.THIRD;
            GD.Print("Advanced from SECOND to THIRD");
        }

            GD.Print("Roary has advanced phases");
        }

	public void animation(Vector2 direction)
    {
        // If direction is too small (essentially zero), don't change animation
        if (direction.LengthSquared() < 0.01f)
        {
            return;
        }

        if (Mathf.Abs(direction.X) > Mathf.Abs(direction.Y))
        {
            if (direction.X > 0)
            {
                if (!anim.IsPlaying() || anim.CurrentAnimation != "Walk_Right")
                {
                    anim.Play("Walk_Right");
                }
            }
            else if (direction.X < 0)
            {
                if (!anim.IsPlaying() || anim.CurrentAnimation != "Walk_Left")
                {
                    anim.Play("Walk_Left");
                }
            }
        }
        else
        {
            if (direction.Y > 0)
            {
                if (!anim.IsPlaying() || anim.CurrentAnimation != "Walk_Down")
                {
                    anim.Play("Walk_Down");
                }
            }
            else if (direction.Y < 0)
            {
                if (!anim.IsPlaying() || anim.CurrentAnimation != "Walk_Up")
                {
                    anim.Play("Walk_Up");
                }
            }
        }
    }

    public override void Die()
    {
        CallDeferred(MethodName.DeferredDie);
    }

    public void DeferredDie()
    {
        if(saveManager!= null)
        {
            saveManager.metaData.DefeatedRoary = true;
            saveManager.SaveNpcFlags();
        }
        else
        {
            GD.Print("error killing roary!");
        }
        eventbus?.EmitSignal(Eventbus.SignalName.beatRoary);
        base.Die();
    }
}
