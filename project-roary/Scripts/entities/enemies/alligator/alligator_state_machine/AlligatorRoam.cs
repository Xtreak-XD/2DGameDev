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
		Vector2 offset = newPos - ActiveEnemy.GlobalPosition;

		if (offset.Length() < 5f)
		{
			ActiveEnemy.Velocity = Vector2.Zero;
			return null;
		}

		Vector2 direction = offset.Normalized();
		ActiveEnemy.animation(direction);
		ActiveEnemy.Velocity = direction * ActiveEnemy.data.Speed;
		ActiveEnemy.MoveAndSlide();

		if(ActiveEnemy.IsPlayerInChaseRange())
		{
			return AlligatorChase;
		}

		return null;
    }

	public void PickPosition()
	{
		newPos = ActiveEnemy.GetRandomPositionInRoamRange();
		timer.Start();
    }
}
