using Godot;
using System;

public partial class RoaryDash : RoaryState
{
	public ThrowFootball ThrowFootball;
	public Timer dashTimer;

	bool EndChargeEarly = false;

	public GoToArenaCenter GoToCenter;
	public MoveTowardPlayer MoveTowardPlayer;

	public override void _Ready()
    {
        GoToCenter = GetParent().GetNode<GoToArenaCenter>("GoToArenaCenter");
		MoveTowardPlayer = GetParent().GetNode<MoveTowardPlayer>("MoveTowardPlayer");
		ThrowFootball = GetParent().GetNode<ThrowFootball>("ThrowFootball");
		
		dashTimer = GetParent().GetNode<Timer>("DashTimer");
    }

	public override void EnterState()
    {
        GD.Print("Roary is now dashing at the player");

		EndChargeEarly = false;

		dashTimer.Start();
    }

    public override void ExitState()
    {
        dashTimer.Stop();
    }

	public override RoaryState Process(double delta)
    {
		Vector2 targetPos = ActiveEnemy.target.GlobalPosition;
		Vector2 currentPos = ActiveEnemy.GlobalPosition;
		Vector2 targetVel = ActiveEnemy.target.Velocity;

		Vector2 velocity = currentPos.Lerp(targetPos + targetVel, 200).Normalized();

		//ActiveEnemy.animation(direction); COMMENTED OUT BECAUSE WE DO NOT HAVE ANIMATIONS
		ActiveEnemy.Velocity = velocity * ActiveEnemy.TrueSpeed() * 
		 2.5f * (ActiveEnemy.TrueAcceleration() * (float) delta);
		
		ActiveEnemy.MoveAndSlide();

		if(EndChargeEarly)
        {
			GD.Print("The player evaded Roary's dash");
            return ThrowFootball;
        }

		if(targetPos.DistanceTo(currentPos) <= 90) // Change for attack radius.
        {										  
            return InBetweenAttack();
        }

		return null;
	}

	public void SetEndChargeEarly()
    {
        EndChargeEarly = true;
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