using Godot;

public partial class IguanaChase : IguanaState
{
	public IguanaRoam IguanaRoam;
	public IguanaAttack IguanaAttack;

	public override void _Ready()
    {
        IguanaRoam = GetParent().GetNode<IguanaRoam>("IguanaRoam");
		IguanaAttack = GetParent().GetNode<IguanaAttack>("IguanaAttack");
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

		ActiveEnemy.animation(direction);

		ActiveEnemy.Velocity = direction * ActiveEnemy.data.Speed;
		ActiveEnemy.MoveAndSlide();

		if(!ActiveEnemy.IsPlayerInChaseRange())
        {
			return IguanaRoam;
        }

		if (ActiveEnemy.IsPlayerInAttackRange())
		{
			return IguanaAttack;
		}

		return null;
	}
}
