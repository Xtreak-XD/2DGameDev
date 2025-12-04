using Godot;
using System.Collections.Generic;

public partial class CarStateMachine : Node
{
	public List<CarState> states;
	public CarState previousState;
	public CarState currentState;

	public override void _Ready()
	{
		ProcessMode = ProcessModeEnum.Disabled;
	}

	public override void _Process(double delta)
	{
		ChangeState(currentState?.Process(delta));
	}

	public override void _PhysicsProcess(double delta)
	{
		ChangeState(currentState?.Physics(delta));
	}

	public override void _UnhandledInput(InputEvent @event)
    {
        ChangeState(currentState?.HandleInput(@event));
    }

	public void Initialize(DriveableCar car)
	{
		states = new List<CarState>();
		
		foreach (Node node in GetChildren())
		{
			if(node is CarState state)
			{
				states.Add(state);
			}
		}

		if (states.Count > 0)
		{
			CarState.ActiveCar = car;
			ChangeState(states[0]);
			ProcessMode = ProcessModeEnum.Inherit;
		}
	}

	public void ChangeState(CarState newState)
	{
		if (newState == null || newState == currentState)
			return;

		if (!states.Contains(newState))
		{
			GD.PrintErr("State not found in state machine.");
			return;
		}

		if (currentState != null)
		{
			currentState.ExitState();
		}

		previousState = currentState;
		currentState = newState;
		currentState.EnterState();
	}
}
