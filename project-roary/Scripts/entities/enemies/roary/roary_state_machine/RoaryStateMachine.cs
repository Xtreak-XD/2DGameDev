using Godot;
using System.Collections.Generic;

public partial class RoaryStateMachine : Node
{
	public List<RoaryState> states;
    public RoaryState previousState;
    public RoaryState currentState;

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

    public void Initialize(Roary enemy)
    {
        states = new List<RoaryState>();

        foreach (Node node in GetChildren())
        {
            if (node is RoaryState state)
            {
                states.Add(state);
            }
        }

        if (states.Count > 0)
        {
            RoaryState.ActiveEnemy = enemy;
            ChangeState(states[0]);
            ProcessMode = ProcessModeEnum.Inherit;
        }
    }

    public void ChangeState(RoaryState newState)
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
