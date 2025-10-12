using Godot;

public partial class EnemyMovement : CharacterBody2D
{
	public EnemyState state;

	//public Player Target {get; set; }; USE THIS LATER

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		state = GetNode<EnemyState>("EnemyState");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
    {
		Vector2 targetPos = GetGlobalMousePosition();
		//Vector2 targetPos = Target.GlobalPosition; // USE THIS LATER INSTEAD OF MOUSE POS
		Vector2 direction = (targetPos - GlobalPosition).Normalized();

		Velocity = direction * state.Speed;
		MoveAndSlide();
	}
}