using Godot;

public partial class Iguana : CharacterBody2D
{
	[Export]
	public GenericData data;
	public IguanaStateMachine stateMachine;
	
	public override void _Ready()
	{
		AddToGroup("enemy");
		stateMachine = GetNode<IguanaStateMachine>("IguanaStateMachine");
		stateMachine.Initialize(this);
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
    {
    }
}
