using System.Collections.Generic;
using Godot;

public partial class IguanaStateMachine : Node
{
	public List<IguanaState> states;
	public IguanaState previousState;
	public IguanaState currentState;

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

	public void Initialize(Iguana enemy)
	{
		states = new List<IguanaState>();
		
		GD.Print(GetChildCount());
		foreach (Node node in GetChildren())
		{
			if(node is IguanaState state)
			{
				states.Add(state);
			}
		}

		if (states.Count > 0)
		{
			IguanaState.ActiveEnemy = enemy;
			ChangeState(states[0]);
			ProcessMode = ProcessModeEnum.Inherit;
		}
	}

	public void ChangeState(IguanaState newState)
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
