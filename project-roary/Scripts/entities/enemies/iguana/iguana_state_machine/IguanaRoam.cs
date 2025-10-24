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
		if (ActiveEnemy.IsPlayerInRange())
		{
			ActiveEnemy.stateMachine.ChangeState(ActiveEnemy.stateMachine.states.Find(state => state is IguanaChase));
			return null;
		}

		// FIX TO MAKE THE IGUANA NOT HAVE A SEIZURE AND BUG OUT
		Vector2 currentPos = ActiveEnemy.GlobalPosition;

		Vector2 newPos = currentPos + new Vector2(
				new RandomNumberGenerator().RandfRange(-500f, 500.0f),
				new RandomNumberGenerator().RandfRange(-500.0f, 500.0f)
			);

		Vector2 direction = newPos - currentPos;
		ActiveEnemy.Velocity = direction;

		ActiveEnemy.MoveAndSlide();

		return null;
    }
}
