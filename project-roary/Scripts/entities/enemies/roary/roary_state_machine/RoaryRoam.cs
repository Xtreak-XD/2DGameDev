using System;
using Godot;

public partial class RoaryRoam : RoaryState
{
	public Timer roamTimer;
	public Vector2 newPos;
	public Timer attackTimer;
	public bool ShouldAdvance = false;
	
	public GoToArenaCenter GoToCenter;
	public MoveTowardPlayer MoveTowardPlayer;

	public override void _Ready()
	{
		GoToCenter = GetParent().GetNode<GoToArenaCenter>("GoToArenaCenter");
		MoveTowardPlayer = GetParent().GetNode<MoveTowardPlayer>("MoveTowardPlayer");

		roamTimer = GetParent().GetNode<Timer>("RoaryRoamTimer");
		roamTimer.Timeout += PickPosition;

		attackTimer = GetParent().GetNode<Timer>("RoaryAttackTimer");
		attackTimer.Timeout += SetShouldPickAttack;
		attackTimer.Start();
	}
	
	public override void EnterState()
	{
		roamTimer.Start();
		attackTimer.Start();
		
		ShouldAdvance = false;
		newPos = ActiveEnemy.Position;

		GD.Print("Roary is now roaming");
	}

    public override void ExitState()
    {
		attackTimer.Stop();
		ShouldAdvance = false;
    }

	public override RoaryState Process(double delta)
    {
		if(ShouldAdvance)
        {
            return InBetweenAttack();
        }

        Vector2 direction = (newPos - ActiveEnemy.GlobalPosition).Normalized();
		//ActiveEnemy.animation(direction); COMMENTED OUT BECAUSE WE DO NOT HAVE ANIMATIONS
		ActiveEnemy.Velocity = direction * ActiveEnemy.TrueSpeed() * 
		((float)delta * ActiveEnemy.TrueAcceleration());
		ActiveEnemy.MoveAndSlide();

		return null;
    }

	public void PickPosition()
	{
		if (ActiveEnemy.GlobalPosition.DistanceTo(newPos) <= 30)
		{
			newPos = ActiveEnemy.GetRandomPositionInRoamRange();
		}
		
		roamTimer.Start();
    }

	public void SetShouldPickAttack()
    {
        ShouldAdvance = true;
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
}
