using Godot;

public partial class IguanaChase : IguanaState
{
	public override void _Ready()
    {
        
    }
	
	// Called when the state is entered
	public override void EnterState()
	{
        
    }

	// Called when the state is exited
	public override void ExitState()
	{
	}

	public override IguanaState Process(double delta)
	{
		Vector2 targetPos = ActiveEnemy.target.GlobalPosition;
		Vector2 direction = (targetPos - ActiveEnemy.GlobalPosition).Normalized();

		ActiveEnemy.Velocity = direction * ActiveEnemy.data.Speed;
		ActiveEnemy.MoveAndSlide();

		if(!ActiveEnemy.IsPlayerInChaseRange())
        {
			ActiveEnemy.stateMachine.ChangeState(ActiveEnemy.stateMachine.states.Find(state => state is IguanaRoam));
			return null;
        }

		if (ActiveEnemy.IsPLayerInAttackRange()) // SET THIS TO ACTUAL ATTACK RANGE
		{
			ActiveEnemy.stateMachine.ChangeState(ActiveEnemy.stateMachine.states.Find(state => state is IguanaAttack));
			return null;
		}

		return null;
	}
}
