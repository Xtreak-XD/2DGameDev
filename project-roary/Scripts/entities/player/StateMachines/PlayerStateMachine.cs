using State = PlayerState;
using Godot;
using System;
using System.Collections;
using System.Collections.Generic;

public partial class PlayerStateMachine : Node
{
	public List<State> states;
	public State prevState;
	public State currState;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
    {
        ProcessMode = Node.ProcessModeEnum.Disabled;
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		ChangeState(currState?.Process(delta));
	}
	
	public override void _PhysicsProcess(double delta)
    {
		ChangeState(currState?.Physics(delta));
    }


    public override void _Input(InputEvent @event)
    {
        ChangeState(currState?.HandleInput(@event));
    }


	public void Initialize(Player player)
	{
		states = new List<State>();

		foreach (Node c in GetChildren())
		{
			if (c is State stateNode)
			{
				states.Add(stateNode);
			}
		}

		if (states.Count > 0)
		{
			State.player = player;
			ChangeState(states[0]);
			ProcessMode = Node.ProcessModeEnum.Inherit;
		}
	}

	public void ChangeState(State newState)
	{
		if (newState == null || newState == currState)
		{
			return;
		}

		if (currState != null)
		{
			currState.Exit();
		}

		prevState = currState;
		currState = newState;
		currState.Enter();
	}
}