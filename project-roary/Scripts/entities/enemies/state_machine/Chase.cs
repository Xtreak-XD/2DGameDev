using Godot;

public partial class Chase : EnemyState
{
	[Export]
	public int Speed;

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
