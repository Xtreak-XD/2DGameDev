using Godot;

public partial class Enemy : CharacterBody2D
{
	[Export]
	public GenericData Data;
	public EnemyStateMachine stateMachine;
	
	public override void _Ready()
	{
		stateMachine = GetNode<EnemyStateMachine>("EnemyStateMachine");
		stateMachine.Initialize(this);
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
    {
    }
}
