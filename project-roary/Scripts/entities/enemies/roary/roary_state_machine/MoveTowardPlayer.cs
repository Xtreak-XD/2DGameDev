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
    }

    public override RoaryState Process(double delta)
    {
        Vector2 currentPos = ActiveEnemy.GlobalPosition;
		Vector2 playerPos = ActiveEnemy.target.GlobalPosition;

        Vector2 direction = (playerPos - currentPos).Normalized();

        if(!ActiveEnemy.CanAttack)
        {
            //ActiveEnemy.animation(direction); COMMENTED OUT BECAUSE WE DO NOT HAVE ANIMATIONS
		    ActiveEnemy.Velocity = -direction * ActiveEnemy.TrueSpeed() * 1.2f *
		     ((float)delta * ActiveEnemy.TrueAcceleration());

		    ActiveEnemy.MoveAndSlide();

            return null;
        }

		//ActiveEnemy.animation(direction); COMMENTED OUT BECAUSE WE DO NOT HAVE ANIMATIONS
		ActiveEnemy.Velocity = direction * ActiveEnemy.TrueSpeed() * 
		((float)delta * ActiveEnemy.TrueAcceleration());
		ActiveEnemy.MoveAndSlide();

        if(ActiveEnemy.Phase == RoaryPhase.FIRST && ActiveEnemy.GetHealthPercentage() <= 0.65)
        {
            return GoToCenter;
        }

        if(ActiveEnemy.Phase == RoaryPhase.SECOND && ActiveEnemy.GetHealthPercentage() <= 0.35)
        {
            return GoToCenter;
        }

        if(ActiveEnemy.Phase == RoaryPhase.THIRD && ActiveEnemy.GetHealthPercentage() <= 0.1)
        {
            return GoToCenter;
        }

		if(currentPos.DistanceTo(playerPos) <= 1500)
        {
            return PickAttack();
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
