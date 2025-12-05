using System;
using Godot;
using System.Collections.Generic;

// Should be a pivotal in-between for attacks in the second and last phase
public partial class GoToArenaCenter : RoaryState
{
	public Vector2 CENTER_POSITION;

	public SummonFootballStampede SummonFootballStampede;

	public ShadowPaw ShadowPaw;
	public ThunderousRoar Roar;
	public ThrowFirework ThrowFirework;
	public LateralDash LateralDash;
	public SummonOrbitalHeads OrbitalHeads;

	public List<RoaryState> Attacks;

	private Vector2 stuckPosition = Vector2.Zero;
	private float stuckTimer = 0f;
	private const float STUCK_THRESHOLD = 0.5f;
	private Vector2 direction = Vector2.Zero;

	public override void _Ready()
    {
		Attacks = [];

		CENTER_POSITION = new Vector2(2472.0f, 2584.0f);

		SummonFootballStampede = GetParent().
		GetNode<SummonFootballStampede>("FootballPlayerStampede");

		ShadowPaw = GetParent().GetNode<ShadowPaw>("ShadowPaw");
		Roar = GetParent().GetNode<ThunderousRoar>("ThunderousRoar");
		ThrowFirework = GetParent().GetNode<ThrowFirework>("ThrowFirework");
		LateralDash = GetParent().GetNode<LateralDash>("LateralDash");
		OrbitalHeads = GetParent().GetNode<SummonOrbitalHeads>("SummonOrbitalHeads");

		Attacks.Add(ShadowPaw);
		Attacks.Add(Roar);
		Attacks.Add(ThrowFirework);
		Attacks.Add(LateralDash);
		Attacks.Add(OrbitalHeads);
    }
	
	public override void EnterState()
	{
		GD.Print("Roary is now going to the center of the stadium.");
		stuckPosition = ActiveEnemy.GlobalPosition;
		stuckTimer = 0f;
		direction = Vector2.Zero;
	}

	public override RoaryState Process(double delta)
    {
		if(ActiveEnemy.GlobalPosition.DistanceTo(CENTER_POSITION) <= 20)
        {
            ActiveEnemy.Velocity = Vector2.Zero;
			    
			GD.Print($"At center - Phase: {ActiveEnemy.Phase}, HP: {ActiveEnemy.GetHealthPercentage():F2}");
			GD.Print($"Flags - First: {ActiveEnemy.SummonedFirstStampede}, Second: {ActiveEnemy.SummonedSecondStampede}, Third: {ActiveEnemy.SummonedThirdStampede}");

			// Check stampede conditions FIRST - these are phase transitions and should override attack cooldowns
			if(ActiveEnemy.Phase == RoaryPhase.FIRST && ActiveEnemy.GetHealthPercentage() <= 0.75 && ActiveEnemy.SummonedFirstStampede)
            {
                return SummonFootballStampede;
            }

			if(ActiveEnemy.Phase == RoaryPhase.SECOND && ActiveEnemy.GetHealthPercentage() <= 0.45 && ActiveEnemy.SummonedSecondStampede)
            {
                return SummonFootballStampede;
            }

			if(ActiveEnemy.Phase == RoaryPhase.THIRD && ActiveEnemy.GetHealthPercentage() <= 0.25 && ActiveEnemy.SummonedThirdStampede)
            {
                return SummonFootballStampede;
            }

			// Only check CanAttack for normal attacks
			if(!ActiveEnemy.CanAttack)
			{
				return null;
			}

            return PickAttack();
        }

		// Calculate direction towards center (only if direction is zero or we're far from center)
		if (direction == Vector2.Zero)
		{
			direction = (CENTER_POSITION - ActiveEnemy.GlobalPosition).Normalized();
		}

		ActiveEnemy.animation(direction);
		ActiveEnemy.Velocity = direction * ActiveEnemy.data.Speed;
		ActiveEnemy.MoveAndSlide();

		// Handle wall collisions
		if(ActiveEnemy.GetSlideCollisionCount() > 0)
		{
			for(int i = 0; i < ActiveEnemy.GetSlideCollisionCount(); i++)
			{
				var collision = ActiveEnemy.GetSlideCollision(i);
				if(collision.GetCollider() is StaticBody2D)
				{
					// Reflect the direction off the wall normal
					Vector2 normal = collision.GetNormal();
					direction = direction.Bounce(normal);
					break;
				}
			}
		}
		else
		{
			// If not colliding, recalculate direction towards center
			direction = (CENTER_POSITION - ActiveEnemy.GlobalPosition).Normalized();
		}

		// Check if stuck on a wall - if stuck, teleport to center
		if(ActiveEnemy.GlobalPosition.DistanceTo(stuckPosition) < 10f)
{
    stuckTimer += (float)delta;
    if(stuckTimer >= STUCK_THRESHOLD)
    {
        GD.Print("Roary stuck, teleporting to center");
        ActiveEnemy.GlobalPosition = CENTER_POSITION;
        
        // After teleporting, check for stampede FIRST
        if(ActiveEnemy.Phase == RoaryPhase.FIRST && ActiveEnemy.SummonedFirstStampede)
        {
            GD.Print("Triggering first stampede after teleport!");
            return SummonFootballStampede;
        }

        if(ActiveEnemy.Phase == RoaryPhase.SECOND && ActiveEnemy.SummonedSecondStampede)
        {
            GD.Print("Triggering second stampede after teleport!");
            return SummonFootballStampede;
        }

        if(ActiveEnemy.Phase == RoaryPhase.THIRD && ActiveEnemy.SummonedThirdStampede)
        {
            GD.Print("Triggering third stampede after teleport!");
            return SummonFootballStampede;
        }

        // Only pick normal attack if no stampede pending
        return PickAttack();
    }
}
else
{
    stuckPosition = ActiveEnemy.GlobalPosition;
    stuckTimer = 0f;
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
