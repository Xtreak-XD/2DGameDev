using Godot;
using System;

public partial class AlligatorRoam : AlligatorState
{
	public Timer timer;
	public AlligatorChase AlligatorChase;
	public Vector2 newPos;

	public override void _Ready()
	{
		timer = GetParent().GetNode<Timer>("AlligatorRoamTimer");
		AlligatorChase = GetParent().GetNode<AlligatorChase>("AlligatorChase");

		timer.Timeout += PickPosition;
	}
	
	public override void EnterState()
	{
		timer.Start();

		newPos = ActiveEnemy.Position;
	}

	public override AlligatorState Process(double delta)
    {
        Vector2 direction = (newPos - ActiveEnemy.Position).Normalized();
		ActiveEnemy.Velocity = direction * ActiveEnemy.data.Speed;
		ActiveEnemy.MoveAndSlide();

		if (ActiveEnemy.IsPlayerInChaseRange())
		{
			return AlligatorChase;
		}

		return null;
    }

	public void PickPosition()
	{
		if (ActiveEnemy.Position.DistanceTo(newPos) <= 50)
		{
			newPos = ActiveEnemy.GetRandomPositionInRoamRange();
		}
		
		timer.Start();
    }
}
