using Godot;
using System;
using System.Collections.Generic;

public partial class MoveTowardPlayer : RoaryState
{
	public List<RoaryState> Attacks;

	public RoaryDash Dash;
	public ThrowFootball ThrowFootball;

	public override void _Ready()
    {
        Dash = GetParent().GetNode<RoaryDash>("RoaryDash");
		ThrowFootball = GetParent().GetNode<ThrowFootball>("ThrowFootball");

		Attacks = [];

		Attacks.Add(Dash);
		Attacks.Add(ThrowFootball);
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

		//ActiveEnemy.animation(direction); COMMENTED OUT BECAUSE WE DO NOT HAVE ANIMATIONS
		ActiveEnemy.Velocity = direction * ActiveEnemy.TrueSpeed() * 
		((float)delta * ActiveEnemy.TrueAcceleration());
		ActiveEnemy.MoveAndSlide();

		if(currentPos.DistanceTo(playerPos) <= 1000)
        {
            return PickAttack();
        }

        return null;
    }

	public RoaryState PickAttack()
    {
        return Attacks[new Random().Next(0, Attacks.Count)];
    }
}
