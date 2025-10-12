using System.Collections.Generic;
using Godot;

namespace state_machine;

public partial class EnemyStateMachine : Node
{
	public List<EnemyState> states;
	public EnemyState previousState;
	public EnemyState currentState;

	public override void _Ready()
	{
		ProcessMode = ProcessModeEnum.Disabled;
	}

	public override void _Process(double delta)
    {
        ChangeState(currentState.Process(delta));
    }

	public void Initialize(Enemy enemy)
	{
		states = [];

		foreach (Node node in GetChildren())
		{
			if (node is EnemyState state)
			{
				states.Add(state);
			}
		}

		if (states.Count > 0)
		{
			states[0].ActiveEnemy = enemy;
			ChangeState(states[0]);
			ProcessMode = ProcessModeEnum.Inherit;
		}
	}

	public void ChangeState(EnemyState newState)
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
			previousState = currentState;
		}

		currentState = newState;
		currentState.EnterState();
	}
}
