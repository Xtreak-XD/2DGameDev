using Godot;
using System;

public partial class ThunderousRoar : RoaryState
{
    public Marker2D projectileSource;
	public GoToArenaCenter GoToCenter;
	public MoveTowardPlayer MoveTowardPlayer;
    
    public Timer ActivationTimer;

    public bool Activate;

    public override void _Ready()
    {
		GoToCenter = GetParent().GetNode<GoToArenaCenter>("GoToArenaCenter");
		MoveTowardPlayer = GetParent().GetNode<MoveTowardPlayer>("MoveTowardPlayer");

		projectileSource = GetParent().GetParent().GetNode<Marker2D>("ProjectileSource");

        ActivationTimer = GetParent().GetNode<Timer>("RoarActivationTimer");
        ActivationTimer.Timeout += SetActivate;

        Activate = false;
	}
    
    public override void EnterState()
    {
        Activate = false;
        ActivationTimer.Start();

        GD.Print("Roary is about to roar");
    }

    public override void ExitState()
    {
        Activate = false;
        ActivationTimer.Stop();
    }

    public override RoaryState Process(double delta)
    {
        if(Activate)
        {
            Vector2 currentPos = projectileSource.GlobalPosition;

            RoaryRoarIndicator roaryRoarIndicator = (RoaryRoarIndicator)
             ActiveEnemy.roarIndication.Instantiate();
            ActiveEnemy.Owner.AddChild(roaryRoarIndicator);

            roaryRoarIndicator.GlobalPosition = currentPos;
            roaryRoarIndicator.parent = ActiveEnemy;

            roaryRoarIndicator.data.Damage = (int)(ActiveEnemy.data.Damage * 2
		     * ActiveEnemy.StatMultipler());
		    roaryRoarIndicator.data.knockback = ActiveEnemy.data.knockBackAmount;

            return InBetweenAttack();
        }

        return null;
    }

    public RoaryState InBetweenAttack()
    {
        if(ActiveEnemy.Phase == RoaryPhase.FIRST)
        {
            return MoveTowardPlayer;
        }

		if(ActiveEnemy.Phase == RoaryPhase.SECOND)
        {
			if(new Random().Next(2) == 1)
            {
                return GoToCenter;
            }
			
            return MoveTowardPlayer;
        }

        return GoToCenter;
    }

    public void SetActivate()
    {
        Activate = true;
    }
}