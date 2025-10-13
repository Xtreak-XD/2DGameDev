using Godot;

public partial class Enemy : CharacterBody2D
{
	[Export]
	public Resource data;
	public EnemyStateMachine stateMachine = new EnemyStateMachine();
	
	public override void _Ready()
	{
		stateMachine.Initialize(this);
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
    {
    }
}
