using Godot;
using System;

public partial class CarState : Node
{
	public static DriveableCar ActiveCar;

	public override void _Ready()
	{

	}

	// Called when the state is entered	
	public virtual void EnterState()
	{
	}

	// Called when the state is exited
	public virtual void ExitState()
	{
	}

	public virtual CarState Process(double delta)
	{
		return null;
	}

	public virtual CarState Physics(double delta)
	{
		return null;
	}
}
