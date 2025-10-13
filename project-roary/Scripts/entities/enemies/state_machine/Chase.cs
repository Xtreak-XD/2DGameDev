using Godot;

public partial class Chase : EnemyState
{
	[Export]
	public int Speed;
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

	public override EnemyState Process(double delta)
	{
		Vector2 targetPos = GetTree().Root.GetMousePosition();
		//Vector2 targetPos = state.Target.GlobalPosition; // USE THIS LATER INSTEAD OF MOUSE POS
		Vector2 direction = (targetPos - ActiveEnemy.GlobalPosition).Normalized();

		ActiveEnemy.Velocity = direction * 100;
		ActiveEnemy.MoveAndSlide();

		return null;
	}
}
