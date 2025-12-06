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
        if(ActiveEnemy.target != null)
        {
            Vector2 targetPos = ActiveEnemy.target.GlobalPosition;
            Vector2 currentPos = ActiveEnemy.GlobalPosition;
            Vector2 targetVel = ActiveEnemy.target.Velocity;

            Vector2 predictedPos = targetPos + (targetVel * .5f);
            Vector2 velocity = (predictedPos + targetVel).Normalized();

            Vector2 targetDir = (targetPos - currentPos).Normalized();
            velocity = velocity.Lerp(targetDir,.2f);

            ActiveEnemy.animation(velocity);
            ActiveEnemy.Velocity = velocity * ActiveEnemy.TrueSpeed() *
            2.5f * (ActiveEnemy.TrueAcceleration() * (float) delta);

            ActiveEnemy.MoveAndSlide();

            // Check if Roary hit a wall
            if(ActiveEnemy.GetSlideCollisionCount() > 0)
            {
                for(int i = 0; i < ActiveEnemy.GetSlideCollisionCount(); i++)
                {
                    var collision = ActiveEnemy.GetSlideCollision(i);
                    if(collision.GetCollider() is StaticBody2D) // Hit a wall
                    {
                        dashTimer.Stop();
                        return InBetweenAttack();
                    }
                }
            }

            if(EndChargeEarly)
            {
                GD.Print("The player evaded Roary's dash");
                return ThrowFootball;
            }

            if(targetPos.DistanceTo(currentPos) <= 90) // Change for attack radius.
            {
                dashTimer.Stop();
                return InBetweenAttack();
            }
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