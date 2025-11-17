using System;
using Godot;
using System.Collections.Generic;

// Should be a pivotal in-between for attacks in the second and last phase
public partial class GoToArenaCenter : RoaryState
{
	public Vector2 CENTER_POSITION = Vector2.Zero; // SET TO ACTUAL CENTER OF STADIUM

	public SummonFootballStampede SummonFootballStampede;

	public RoaryDash Dash;
	public ShadowPaw ShadowPaw;
	public ThunderousRoar Roar;

	public List<RoaryState> Attacks;
	
	public override void _Ready()
    {
		Attacks = [];

		SummonFootballStampede = GetParent().
		GetNode<SummonFootballStampede>("FootballPlayerStampede");

		Dash = GetParent().GetNode<RoaryDash>("RoaryDash");
		ShadowPaw = GetParent().GetNode<ShadowPaw>("ShadowPaw");
		Roar = GetParent().GetNode<ThunderousRoar>("ThunderousRoar");

		// Add a couple good attacks here
		Attacks.Add(Dash);
		Attacks.Add(ShadowPaw);
		Attacks.Add(Roar);
    }
	
	public override void EnterState()
	{
		GD.Print("Roary is now going to the center of the stadium.");
	}

	public override RoaryState Process(double delta)
    {
		if(ActiveEnemy.GlobalPosition.DistanceTo(CENTER_POSITION) <= 20)
        {
            ActiveEnemy.Velocity = Vector2.Zero;

			if(ActiveEnemy.Phase == RoaryPhase.FIRST && ActiveEnemy.GetHealthPercentage() <= 0.65)
            {
                return SummonFootballStampede;
            }

			if(ActiveEnemy.Phase == RoaryPhase.SECOND && ActiveEnemy.GetHealthPercentage() <= 0.35)
            {
                return SummonFootballStampede;
            }

			if(ActiveEnemy.Phase == RoaryPhase.THIRD && ActiveEnemy.GetHealthPercentage() <= 0.1)
            {
                return SummonFootballStampede;
            }

            return PickAttack();
        }

        Vector2 direction = (CENTER_POSITION - ActiveEnemy.GlobalPosition).Normalized();
		//ActiveEnemy.animation(direction); COMMENTED OUT BECAUSE WE DO NOT HAVE ANIMATIONS
		ActiveEnemy.Velocity = direction * ActiveEnemy.data.Speed * 
		((float)delta * (float)ActiveEnemy.data.Accel);
		ActiveEnemy.MoveAndSlide();

		return null;
    }

	public RoaryState PickAttack()
    {
        return Attacks[new Random().Next(0, Attacks.Count)];
    }
}
