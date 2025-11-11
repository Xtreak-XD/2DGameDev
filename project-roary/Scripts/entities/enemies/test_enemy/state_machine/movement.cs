using Godot;
using System;

public partial class movement : EnemyState
{
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
		return null;
	}
    public override void _EnterTree()
	{
		Vector2 direction = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");
		ActiveEnemy.Velocity = direction * ActiveEnemy.petitionerData.moveSpeed;
    }

	public override EnemyState Physics(double delta)
    {
		GetInput();
		ActiveEnemy.MoveAndSlide();
    }
}
