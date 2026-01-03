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
		SetRotation();
    }

	public void SetRotation()
	{
		if (StateMachine.currentState is ParkState || StateMachine.currentState is ReverseState)
		{
			return;
		}
		
		if (Velocity != Vector2.Zero)
		{
			Rotation = Velocity.Angle();
		}
	}

    public override void _ExitTree()
    {
        return;
    }

	public bool IsParked()
	{
		return Velocity.Length() == 0;
	}
}
