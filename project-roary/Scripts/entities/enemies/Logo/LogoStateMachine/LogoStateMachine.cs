using Godot;
using System.Collections.Generic;

public partial class LogoStateMachine : Node
{

    private Logo Logo;
    public List<LogoState> States { get; private set; }
    public LogoState CurrentState { get; private set; }
    public LogoState PreviousState { get; private set; }

    public override void _Ready()
    {
        
        ProcessMode = ProcessModeEnum.Disabled;
    }

    public override void _Process(double delta)
    {
        if (CurrentState == null)
            return;

        var next = CurrentState.Process(delta);
        ChangeState(next);
    }

    public override void _PhysicsProcess(double delta)
    {
        if (CurrentState == null)
            return;

        var next = CurrentState.Physics(delta);
        ChangeState(next);
    }

    public void Initialize(Logo logo)
    {
        Logo = logo;

        States = new List<LogoState>();

        foreach (var child in GetChildren())
        {
            if (child is LogoState state)
            {
                state.Initialize(logo);
                States.Add(state);

                if (state is LogoIdleState idle) logo.IdleState = idle;
                if (state is RollState roll) logo.RollState = roll;
            }
        }

        if (States.Count == 0)
        {
            GD.PushError("LogoStateMachine: No LogoState nodes found!");
            return;
        }

        
        ChangeState(States[0]);

        
        ProcessMode = ProcessModeEnum.Inherit;
    }

    public void ChangeState(LogoState newState)
    {
        if (newState == null || newState == CurrentState)
            return;

        if (!States.Contains(newState))
        {
            GD.PushError($"LogoStateMachine: State \"{newState.Name}\" is not a child of this state machine.");
            return;
        }

        CurrentState?.ExitState();

        PreviousState = CurrentState;
        CurrentState = newState;

        CurrentState.EnterState();
    }
}