using System;
using Godot;


public partial class EnemyState : Node
{
	public Enemy ActiveEnemy;

	public override void _Ready()
	{

	}

	// Called when the state is entered	
	public virtual void EnterState()
	{
	}

	// Called when the state is exited
	public virtual void ExitState()
	{
	}

	public virtual EnemyState Process(double delta)
	{
		return null;
	}

	public virtual EnemyState Physics(double delta)
    {
		return null;
    }
}
