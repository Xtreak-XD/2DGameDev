using System;
using Godot;

public partial class DriveableCar : Player
{
	public CarStateMachine StateMachine;

	[Export]
	public CarStats stats;
	public Vector2 Direction = Vector2.Zero;

	public override void _Ready()
	{
		AddToGroup("player");
		sceneManager = GetNode<SceneManager>("/root/SceneManager");
		StateMachine = GetNode<CarStateMachine>("DriveableCarStateMachine");
		StateMachine.Initialize(this);
	}

	public override void _Process(double delta)
	{
	}

	public override void _PhysicsProcess(double delta)
	{
		if(HasThrottle())
        {
            float turn = Input.GetAxis("Left", "Right");
			float turnAngle = Mathf.DegToRad(turn * stats.SteeringSpeed);
			Vector2 accelerationVector = Vector2.Zero;

			if(Input.IsActionPressed("Up"))
			{
				accelerationVector = Transform.X * stats.Acceleration;
			}

			Velocity += accelerationVector * (float) delta;
			Velocity = Velocity.Rotated(turnAngle);

			if(Velocity.Length() > stats.TopSpeed)
            {
                Velocity = Velocity.Normalized()
                * stats.TopSpeed;
            }

			SetRotation();
			MoveAndSlide();
        }
    }

	public bool HasThrottle()
	{	
		return Input.IsActionPressed("Up");
	}

	public void SetRotation()
	{
		if (StateMachine.currentState is ParkState)
		{
			return;
		}
		
		if (Velocity != Vector2.Zero)
		{
			Rotation = Velocity.Angle();
		}
	}

	public bool IsParked()
	{
		return Velocity.Length() == 0;
	}
}
