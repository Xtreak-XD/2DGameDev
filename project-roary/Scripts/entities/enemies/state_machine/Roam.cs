using Godot;

public partial class Roam : EnemyState
{

	public EnemyState chase;
	public override void _Ready()
	{
		chase = GetNode<EnemyState>("../Chase");
    }

	public override void EnterState()
    {
        
    }

	// Called when the state is exited
	public override void ExitState()
	{
	}

	public override EnemyState Process(double delta)
	{
		//returning chase for testing. Logic should change!
		return chase;
		
		//return null;
	}
}
