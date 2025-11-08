using Godot;

public partial class AlligatorDragPlayer : AlligatorState
{
	public AlligatorChase AlligatorChase;
	public AlligatorDeathRoll AlligatorDeathRoll;

	public override void _Ready()
    {
		AlligatorChase = GetParent().GetNode<AlligatorChase>("AlligatorChase");
		AlligatorDeathRoll = GetParent().GetNode<AlligatorDeathRoll>("AlligatorDeathRoll");
    }

	public override AlligatorState Process(double delta)
	{
		Vector2 direction = (ActiveEnemy.homePosition - ActiveEnemy.Position).Normalized();
		ActiveEnemy.Velocity = direction * ActiveEnemy.data.Speed * 0.66f;

		ActiveEnemy.MoveAndSlide();

		ActiveEnemy.target.Position = ActiveEnemy.Position;
		ActiveEnemy.target.Velocity = Vector2.Zero;

		if (ActiveEnemy.Position.DistanceTo(ActiveEnemy.homePosition) <= 40)
		{
			return AlligatorDeathRoll;
		}
		
		// Player needs to be able to dodge out of this state
		// The alligator will return to chase if the player dodges successfully

		return null;
    }
}