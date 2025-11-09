using Godot;
using System.Collections.Generic;

public partial class AlligatorStateMachine : Node
{
    public List<AlligatorState> states;
    public AlligatorState previousState;
    public AlligatorState currentState;

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

    public void Initialize(Alligator enemy)
    {
        states = new List<AlligatorState>();

        GD.Print(GetChildCount());
        foreach (Node node in GetChildren())
        {
            if (node is AlligatorState state)
            {
                states.Add(state);
            }
        }

        if (states.Count > 0)
        {
            AlligatorState.ActiveEnemy = enemy;
            ChangeState(states[0]);
            ProcessMode = ProcessModeEnum.Inherit;
        }
    }

    public void ChangeState(AlligatorState newState)
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
