using Godot;
using System;
using System.Collections.Generic;

// Should be a pivotal in-between for attacks in the first and second phase
public partial class MoveTowardPlayer : RoaryState
{
    public GoToArenaCenter GoToCenter;

	public RoaryDash Dash;
	public ThrowFootball ThrowFootball;
    public RoaryRoam Roam;
    public ThrowFirework ThrowFirework;

    public List<RoaryState> Attacks;

    private Vector2 stuckPosition = Vector2.Zero;
    private float stuckTimer = 0f;
    private const float STUCK_THRESHOLD = 0.5f; // If stuck for 0.5 seconds
    private bool backingIntoWall = false;

	public override void _Ready()
    {
        GoToCenter = GetParent().GetNode<GoToArenaCenter>("GoToArenaCenter");
        
        Roam = GetParent().GetNode<RoaryRoam>("RoaryRoam");
        Dash = GetParent().GetNode<RoaryDash>("RoaryDash");
		ThrowFootball = GetParent().GetNode<ThrowFootball>("ThrowFootball");
        ThrowFirework = GetParent().GetNode<ThrowFirework>("ThrowFirework");

		Attacks = [];

        Attacks.Add(Roam);
		Attacks.Add(Dash);
		Attacks.Add(ThrowFootball);
        Attacks.Add(ThrowFirework);
    }

    public override void EnterState()
    {
        GD.Print("Roary is moving towards the player.");
        stuckPosition = ActiveEnemy.GlobalPosition;
        stuckTimer = 0f;
        backingIntoWall = false;
    }

    public override RoaryState Process(double delta)
    {
        if(ActiveEnemy.target != null)
        {
            Vector2 currentPos = ActiveEnemy.GlobalPosition;
            Vector2 playerPos = ActiveEnemy.target.GlobalPosition;

            Vector2 direction = (playerPos - currentPos).Normalized();

            if(!ActiveEnemy.CanAttack)
            {
                ActiveEnemy.animation(direction);

                // Only try to back away if not already against a wall
                if(!backingIntoWall)
                {
                    ActiveEnemy.Velocity = -direction * ActiveEnemy.TrueSpeed() * 1.2f *
                    ((float)delta * ActiveEnemy.TrueAcceleration());
                }
                else
                {
                    // If against wall, move sideways along the wall
                    Vector2 perpendicular = new Vector2(-direction.Y, direction.X);
                    ActiveEnemy.Velocity = perpendicular * ActiveEnemy.TrueSpeed() * 0.8f *
                    ((float)delta * ActiveEnemy.TrueAcceleration());
                }

                ActiveEnemy.MoveAndSlide();

                // Check if backing into a wall
                backingIntoWall = false; // Reset each frame
                if(ActiveEnemy.GetSlideCollisionCount() > 0)
                {
                    for(int i = 0; i < ActiveEnemy.GetSlideCollisionCount(); i++)
                    {
                        var collision = ActiveEnemy.GetSlideCollision(i);
                        if(collision.GetCollider() is StaticBody2D)
                        {
                            backingIntoWall = true;
                            break;
                        }
                    }
                }

                return null;
            }

            ActiveEnemy.animation(direction);
            ActiveEnemy.Velocity = direction * ActiveEnemy.TrueSpeed() *
            ((float)delta * ActiveEnemy.TrueAcceleration());
            ActiveEnemy.MoveAndSlide();

            // Check if stuck on a wall
            if(ActiveEnemy.GlobalPosition.DistanceTo(stuckPosition) < 10f)
            {
                stuckTimer += (float)delta;
                if(stuckTimer >= STUCK_THRESHOLD)
                {
                    return GoToCenter;
                }
            }
            else
            {
                stuckPosition = ActiveEnemy.GlobalPosition;
                stuckTimer = 0f;
            }

            GD.Print($"Health: {ActiveEnemy.GetHealthPercentage():F2}, Phase: {ActiveEnemy.Phase}");
            if(ActiveEnemy.Phase == RoaryPhase.FIRST && ActiveEnemy.GetHealthPercentage() <= 0.75 && !ActiveEnemy.SummonedFirstStampede)
            {
                GD.Print("Phase 1 → 2 transition triggered at 75% HP");
                ActiveEnemy.SummonedFirstStampede = true; 
                return GoToCenter;
            }

            if(ActiveEnemy.Phase == RoaryPhase.SECOND && ActiveEnemy.GetHealthPercentage() <= 0.45 && !ActiveEnemy.SummonedSecondStampede)
            {
                GD.Print("Phase 2 → 3 transition triggered at 45% HP");
                ActiveEnemy.SummonedSecondStampede = true; 
                return GoToCenter;
            }

            if(ActiveEnemy.Phase == RoaryPhase.THIRD && ActiveEnemy.GetHealthPercentage() <= 0.25 && !ActiveEnemy.SummonedThirdStampede)
            {
                GD.Print("Final stampede triggered at 25% HP");
                ActiveEnemy.SummonedThirdStampede = true; 
                return GoToCenter;
            }

            if(currentPos.DistanceTo(playerPos) <= 1500)
            {
                return PickAttack();
            }
        }

        return null;
    }

	public RoaryState PickAttack()
    {
        ActiveEnemy.CanAttack = false;
        ActiveEnemy.GlobalAttackTimer.Start();

        return Attacks[new Random().Next(0, Attacks.Count)];
    }
}
