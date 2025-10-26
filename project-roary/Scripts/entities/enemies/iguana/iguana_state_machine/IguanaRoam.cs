using Godot;

public partial class IguanaRoam : IguanaState
{
	public override void _Ready()
	{
    }

	public override void EnterState()
    {
        
    }

	// Called when the state is exited
	public override void ExitState()
	{
	}

	public override IguanaState Process(double delta)
	{
		if (ActiveEnemy.IsPlayerInChaseRange())
		{
			ActiveEnemy.stateMachine.ChangeState(ActiveEnemy.stateMachine.states.
			Find(state => state is IguanaChase));
			
			return null;
		}

		ActiveEnemy.Velocity = new Vector2(1, 0).Rotated(new RandomNumberGenerator().
		RandfRange(-Mathf.Pi / 2, Mathf.Pi / 2)) * ActiveEnemy.data.Speed * 2;
		ActiveEnemy.MoveAndSlide();
	
		return null;
    }
}
