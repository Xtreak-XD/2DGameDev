using Godot;
using System.Collections.Generic;

public partial class MermaidStateMachine : Node
{
	public List<MermaidState> states;
    public MermaidState previousState;
    public MermaidState currentState;

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

    public void Initialize(Mermaid enemy)
    {
        states = new List<MermaidState>();

        foreach (Node node in GetChildren())
        {
            if (node is MermaidState state)
            {
                states.Add(state);
            }
        }

        if (states.Count > 0)
        {
            MermaidState.ActiveEnemy = enemy;
            ChangeState(states[0]);
            ProcessMode = ProcessModeEnum.Inherit;
        }
    }

    public void ChangeState(MermaidState newState)
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
