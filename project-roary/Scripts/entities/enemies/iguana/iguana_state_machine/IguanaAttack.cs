using Godot;
using System;

public partial class IguanaAttack : IguanaState
{
	public Timer timer;

	public override void _Ready()
    {
        timer = GetParent().GetNode<Timer>("AttackTimer");
    }
	
	// Called when the state is entered
	public override void EnterState()
	{
        timer.Start();
    }

	// Called when the state is exited
	public override void ExitState()
	{
		timer.Stop();
		timer.WaitTime = 0.5;
		
		ActiveEnemy.stateMachine.ChangeState(ActiveEnemy.stateMachine.states.
		Find(state => state is IguanaChase));
    }

	public override IguanaState Process(double delta)
	{
		return null;
	}
}
