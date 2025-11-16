using System;
using System.Collections.Generic;
using Godot;

public partial class RoaryRoam : RoaryState
{
	public Timer roamTimer;
	public Vector2 newPos;
	public Timer attackTimer;
	public bool ShouldPickAttack = false;

	public List<RoaryState> Moves;
	
	public GoToArenaCenter GoToCenter;
	public MoveTowardPlayer MoveTowardPlayer;

	public override void _Ready()
	{
		roamTimer = GetParent().GetNode<Timer>("RoaryRoamTimer");
		roamTimer.Timeout += PickPosition;

		Moves = [];

		attackTimer = GetParent().GetNode<Timer>("RoaryAttackTimer");
		attackTimer.Timeout += SetShouldPickAttack;
		attackTimer.Start();

		GoToCenter = GetParent().GetNode<GoToArenaCenter>("GoToArenaCenter");
		MoveTowardPlayer = GetParent().GetNode<MoveTowardPlayer>("MoveTowardPlayer");
		
		Moves.Add(GoToCenter);
		Moves.Add(MoveTowardPlayer);
	}
	
	public override void EnterState()
	{
		roamTimer.Start();
		attackTimer.Start();
		
		ShouldPickAttack = false;
		newPos = ActiveEnemy.Position;

		GD.Print("Roary is now roaming");
	}

    public override void ExitState()
    {
		attackTimer.Stop();
		ShouldPickAttack = false;
    }

	public override RoaryState Process(double delta)
    {
		if(ShouldPickAttack)
        {
            return PickAttack();
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

	public RoaryState PickAttack()
    {
        return Moves[new Random().Next(0, Moves.Count)];
    }

	public void SetShouldPickAttack()
    {
        ShouldPickAttack = true;
    }
}
