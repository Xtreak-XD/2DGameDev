using System;
using Godot;

public partial class DriveableCar : CharacterBody2D
{
	public CarStateMachine stateMachine;
	public Vector2 direction = Vector2.Zero;

	[Export]
	public CarStats stats;

	public override void _Ready()
	{
		AddToGroup("player");

		stateMachine = GetNode<CarStateMachine>("DriveableCarStateMachine");
		stateMachine.Initialize(this);
	}

	public override void _Process(double delta)
	{
		// TODO: FIGURE OUT HOW TO SMOOTH OUT THE MOVEMENT INPUT AND ROTATION
		float upInput = Input.GetActionStrength("Up");
		float leftInput = Input.GetActionStrength("Left");
		float rightInput = Input.GetActionStrength("Right");
		int downInput = (int)Input.GetActionStrength("Down");

		direction.X += upInput;
		direction.X = Mathf.Clamp(direction.X, -1, 1);

		if (ShouldReverse())
		{
			direction.X *= -1;
		}

		if (!(stateMachine.currentState is ParkState))
		{
			if (rightInput != 0 || leftInput != 0)
			{
				Rotation += Mathf.DegToRad((rightInput - leftInput) * stats.SteeringSpeed * (float)delta);
				direction += Vector2.Right.Rotated(Rotation);
			}
		}
		
		direction = direction.Normalized();
	}

	public bool ShouldReverse()
    {
		int downActionStrength = (int)Input.GetActionStrength("Down");
		int upActionStrength = (int)Input.GetActionStrength("Up");
		return downActionStrength != 0 && upActionStrength == 0;
    }

	public bool HasThrottle()
	{
		/*
		int rightActionStrength = (int)Input.GetActionStrength("Right");
		int leftActionStrength = (int)Input.GetActionStrength("Left");
		int upActionStrength = (int)Input.GetActionStrength("Up");
		int downActionStrength = (int)Input.GetActionStrength("Down");

		return rightActionStrength != 0 ||
			   leftActionStrength != 0 ||
			   upActionStrength != 0 ||
			   downActionStrength != 0;
			   */

		int upActionStrength = (int)Input.GetActionStrength("Up");
		int downActionStrength = (int)Input.GetActionStrength("Down");
		return upActionStrength != 0 || downActionStrength != 0;
	}

	public void SetRotation()
	{
		// NOT SURE IF THIS IS THE BEST WAY TO DO IT
		Rotation = direction.Angle();
    }
	
	public bool IsParked()
	{
		return Velocity.Length() == 0;
	}
}
