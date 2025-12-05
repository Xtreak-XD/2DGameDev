using Godot;

public partial class AlligatorChase : AlligatorState
{
	public AlligatorRoam AlligatorRoam;
	public AlligatorDragPlayer AlligatorDragPlayer;
	public AlligatorLunge AlligatorLunge;

	public override void _Ready()
	{
		AlligatorRoam = GetParent().GetNode<AlligatorRoam>("AlligatorRoam");
		AlligatorDragPlayer = GetParent().GetNode<AlligatorDragPlayer>("AlligatorDragPlayer");
		AlligatorLunge = GetParent().GetNode<AlligatorLunge>("AlligatorLunge");
	}
	
	public override AlligatorState Process(double delta)
	{
		Vector2 targetPos = ActiveEnemy.target.GlobalPosition;
		Vector2 direction = (targetPos - ActiveEnemy.GlobalPosition).Normalized();
		ActiveEnemy.animation(direction);

		ActiveEnemy.Velocity = direction * ActiveEnemy.data.Speed * ((float)delta * (float)ActiveEnemy.data.Accel);
		ActiveEnemy.MoveAndSlide();

		if (!ActiveEnemy.IsPlayerInChaseRange())
		{
			return AlligatorRoam;
		}

		if(ActiveEnemy.IsPlayerInDeathRollRange())
		{
			if(ActiveEnemy.IsPlayerInChompRange())
            {
                return AlligatorDragPlayer;
            }
		}
		else if(ActiveEnemy.IsPlayerInAttackRange())
		{	
			if(ActiveEnemy.IsPlayerInLungeRange())
			{
				return AlligatorLunge;
			}
		}

		return null;
	}
}
