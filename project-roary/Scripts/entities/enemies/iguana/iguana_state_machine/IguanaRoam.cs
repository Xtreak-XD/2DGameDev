using Godot;

public partial class IguanaRoam : IguanaState
{
	public Timer timer;
	public IguanaAttack IguanaAttack;
	public IguanaChase IguanaChase;
	public Vector2 newPos;

	public override void _Ready()
	{
		timer = GetParent().GetNode<Timer>("RoamTimer");
		IguanaAttack = GetParent().GetNode<IguanaAttack>("IguanaAttack");
		IguanaChase = GetParent().GetNode<IguanaChase>("IguanaChase");
    }

	public override void EnterState()
    {
		timer.Start();

		timer.Timeout += PickPosition;

		newPos = ActiveEnemy.Position;
    }

	// Called when the state is exited
	public override void ExitState()
    {
    }

	public override IguanaState Process(double delta)
	{
		Vector2 direction = (newPos - ActiveEnemy.Position).Normalized();
		ActiveEnemy.Velocity = direction * ActiveEnemy.data.Speed;
		ActiveEnemy.MoveAndSlide();

		if (ActiveEnemy.IsPlayerInChaseRange())
		{
			return IguanaChase;
		}

		if (ActiveEnemy.IsPlayerInAttackRange())
		{
			return IguanaAttack;
		}

		return null;
	}
	
	public void PickPosition()
	{
		if (ActiveEnemy.Position.DistanceTo(newPos) <= 50)
		{
			//GD.Print("Reached roam position.");
			newPos = ActiveEnemy.GetRandomPositionInRoamRange();
			//GD.Print("New Roam Position: " + newPos);
		}
		
		timer.Start();
    }
}
