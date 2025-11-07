using Godot;
using System;

public partial class AlligatorChase : AlligatorState
{
	public AlligatorRoam AlligatorRoam;

	public override void _Ready()
	{
		AlligatorRoam = GetParent().GetNode<AlligatorRoam>("AlligatorRoam");
	}
	
	public override AlligatorState Process(double delta)
	{
		Vector2 targetPos = ActiveEnemy.target.GlobalPosition;
		Vector2 direction = (targetPos - ActiveEnemy.GlobalPosition).Normalized();

		ActiveEnemy.Velocity = direction * ActiveEnemy.data.Speed;
		ActiveEnemy.MoveAndSlide();

		if (!ActiveEnemy.IsPlayerInChaseRange())
		{
			return AlligatorRoam;
		}

		if(ActiveEnemy.IsPlayerInAttackRange())
		{
			// Switch to attack state
		}

		return null;
	}
}
