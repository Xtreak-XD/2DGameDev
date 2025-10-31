using Godot;

public partial class IguanaRoam : IguanaState
{
	public Timer timer;

	public override void _Ready()
	{
		timer = GetParent().GetNode<Timer>("AttackTimer");
    }

	public override void EnterState()
    {
		timer.Start();

		timer.Timeout += () =>
        {
            ActiveEnemy.Velocity = new Vector2(1, 0).Rotated(new RandomNumberGenerator().
		RandfRange(-Mathf.Pi / 2, Mathf.Pi / 2)) * ActiveEnemy.data.Speed * 2;
			ActiveEnemy.MoveAndSlide();

			timer.WaitTime = 0.5;
        };
    }

	// Called when the state is exited
	public override void ExitState()
    {
        timer.Stop();
		timer.WaitTime = 0.5;
    }

	public override IguanaState Process(double delta)
	{
		if (ActiveEnemy.IsPlayerInChaseRange())
		{
			ActiveEnemy.stateMachine.ChangeState(ActiveEnemy.stateMachine.states.
			Find(state => state is IguanaChase));
			
			return null;
		}
	
		return null;
    }
}
