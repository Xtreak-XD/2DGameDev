using Godot;
using System;

public partial class LateralDash : RoaryState
{
    public enum Direction {
        East,
        West
    }

    public GoToArenaCenter GoToCenter;
    public MoveTowardPlayer MoveTowardPlayer;
    public Timer dashTimer;

    bool ChargeOver = false;

    Vector2 direction = Vector2.Zero;

    Direction chargeDirection;

    public override void _Ready()
    {
        GoToCenter = GetParent().GetNode<GoToArenaCenter>("GoToArenaCenter");
        MoveTowardPlayer = GetParent().GetNode<MoveTowardPlayer>("MoveTowardPlayer");

        dashTimer = GetParent().GetNode<Timer>("LateralDashTimer");
        dashTimer.Timeout += SetDashOver;
    }

    public override void EnterState()
	{
		GD.Print("Roary is dashing the player from the side");

        if(ActiveEnemy.target != null)
        {
            ChargeOver = false;
            dashTimer.Start();

            if(new Random().Next(2) == 1)
            {
                chargeDirection = Direction.West;
            } 
            else
            {
                chargeDirection = Direction.East;
            }

            Vector2 playerPos = ActiveEnemy.target.GlobalPosition;

            if(chargeDirection == Direction.East)
            {
                ActiveEnemy.GlobalPosition = playerPos + new Vector2(-750, 0);
            } 
            else
            {
                ActiveEnemy.GlobalPosition = playerPos + new Vector2(750, 0);
            }

            direction = (playerPos - ActiveEnemy.GlobalPosition).Normalized();
        }
	}

    public override void ExitState()
    {
        dashTimer.Stop();
    }

    public override RoaryState Process(double delta)
    {
        if(ChargeOver)
        {
            return InBetweenAttack();
        }

        ActiveEnemy.animation(direction);
		ActiveEnemy.Velocity = direction * ActiveEnemy.TrueSpeed() *
		 1.8f * (ActiveEnemy.TrueAcceleration() * (float) delta);

        ActiveEnemy.MoveAndSlide();

        // Check if Roary hit a wall and bounce off
        if(ActiveEnemy.GetSlideCollisionCount() > 0)
        {
            for(int i = 0; i < ActiveEnemy.GetSlideCollisionCount(); i++)
            {
                var collision = ActiveEnemy.GetSlideCollision(i);
                if(collision.GetCollider() is StaticBody2D) // Hit a wall
                {
                    // Bounce off the wall using the collision normal
                    Vector2 normal = collision.GetNormal();
                    direction = direction.Bounce(normal);
                    chargeDirection = (chargeDirection == Direction.East) ? Direction.West : Direction.East;
                    break;
                }
            }
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

    public void SetDashOver()
    {
        ChargeOver = true;
    }
}