using Godot;

public partial class EnemyMovement : CharacterBody2D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
    {
		Vector2 targetPos = GetGlobalMousePosition();
		//Vector2 targetPos = state.Target.GlobalPosition; // USE THIS LATER INSTEAD OF MOUSE POS
		Vector2 direction = (targetPos - GlobalPosition).Normalized();

		Velocity = direction * 100;
		MoveAndSlide();
	}
}