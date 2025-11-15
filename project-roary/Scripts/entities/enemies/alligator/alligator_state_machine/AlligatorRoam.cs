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
        Vector2 direction = (newPos - ActiveEnemy.GlobalPosition).Normalized();
		ActiveEnemy.animation(direction);
		ActiveEnemy.Velocity = direction * ActiveEnemy.data.Speed * ((float)delta * (float)ActiveEnemy.data.Accel);
		ActiveEnemy.MoveAndSlide();

		if (ActiveEnemy.IsPlayerInChaseRange())
		{
			return AlligatorChase;
		}

		return null;
    }

	public void PickPosition()
	{
		if (ActiveEnemy.GlobalPosition.DistanceTo(newPos) <= 50)
		{
			newPos = ActiveEnemy.GetRandomPositionInRoamRange();
		}
		
		timer.Start();
    }
}
