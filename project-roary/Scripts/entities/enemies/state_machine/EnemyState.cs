using Godot;

namespace state_machine;

public partial class EnemyState : Node
{
	public Enemy ActiveEnemy;

	public override void _Ready()
	{
	}

	// Called when the state is entered	
	public virtual void EnterState()
	{
	}

	// Called when the state is exited
	public virtual void ExitState()
	{
	}

	public virtual EnemyState Process(double delta)
	{
		return null;
	}
}
