using Godot;

public partial class Chase : EnemyState
{
	public Player Target;
	public override void _Ready()
    {
        Target = GetTree().GetFirstNodeInGroup("player") as Player;
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
		//Vector2 targetPos = GetTree().Root.GetMousePosition();
		Vector2 targetPos = Target.GlobalPosition;
		Vector2 direction = (targetPos - ActiveEnemy.GlobalPosition).Normalized();

		ActiveEnemy.Velocity = direction * ActiveEnemy.data.Speed;
		ActiveEnemy.MoveAndSlide();

		return null;
	}
}
