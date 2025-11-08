using Godot;
using System;

public partial class AlligatorChase : AlligatorState
{
	public AlligatorRoam AlligatorRoam;
	public AlligatorDragPlayer AlligatorDragPlayer;

	public override void _Ready()
	{
		AlligatorRoam = GetParent().GetNode<AlligatorRoam>("AlligatorRoam");
		AlligatorDragPlayer = GetParent().GetNode<AlligatorDragPlayer>("AlligatorDragPlayer");
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
			if (ActiveEnemy.IsPlayerInDeathRollRange())
			{
				return AlligatorDragPlayer;
			}
			else
			{
				// Go to other attack
			}
		}

		return null;
	}
}
