using Godot;

public partial class IguanaChase : IguanaState
{
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

	public override IguanaState Process(double delta)
	{
		Vector2 targetPos = ActiveEnemy.target.GlobalPosition;
		//Vector2 targetPos = state.Target.GlobalPosition; // USE THIS LATER INSTEAD OF MOUSE POS
		Vector2 direction = (targetPos - ActiveEnemy.GlobalPosition).Normalized();

		ActiveEnemy.Velocity = direction * ActiveEnemy.data.Speed;
		ActiveEnemy.MoveAndSlide();

		return null;
	}
}
