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
	
	public override void EnterState()
	{
		GD.Print("Alligator has grabbed the player.");
	}

	public override AlligatorState Process(double delta)
	{
		Vector2 direction = (ActiveEnemy.homePosition - ActiveEnemy.GlobalPosition).Normalized();
		ActiveEnemy.Velocity = direction * ActiveEnemy.data.Speed * 0.66f;

		ActiveEnemy.MoveAndSlide();

		ActiveEnemy.target.GlobalPosition = ActiveEnemy.GlobalPosition;
		ActiveEnemy.target.Velocity = Vector2.Zero;

		GD.Print("Distance to home: " +
		 ActiveEnemy.GlobalPosition.DistanceTo(ActiveEnemy.homePosition));

		if (ActiveEnemy.GlobalPosition.DistanceTo(ActiveEnemy.homePosition) <= 20)
		{
			return AlligatorDeathRoll;
		}
		
		// Player needs to be able to dodge out of this state
		// The alligator will return to chase if the player dodges successfully

		return null;
    }
}