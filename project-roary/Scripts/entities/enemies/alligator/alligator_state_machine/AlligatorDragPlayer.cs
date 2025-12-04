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
		// The player should probably take damage upon being grabbed
		GD.Print("Alligator has grabbed the player.");
	}

	public override AlligatorState Process(double delta)
	{
		Vector2 direction = (ActiveEnemy.homePosition - ActiveEnemy.GlobalPosition)
		.Normalized();
		ActiveEnemy.Velocity = direction * ActiveEnemy.data.Speed;

		ActiveEnemy.MoveAndSlide();

        ActiveEnemy.target.GlobalPosition = ActiveEnemy.hitbox.GlobalPosition;
		ActiveEnemy.target.Velocity = Vector2.Zero;

		GD.Print($"Distance to home at ({ActiveEnemy.homePosition.X}, " +
		 $"{ActiveEnemy.homePosition.Y}): " +
		 ActiveEnemy.GlobalPosition.DistanceTo(ActiveEnemy.homePosition));
	
		// Insurance against glitches
		if(!ActiveEnemy.IsPlayerInDeathRollRange())
		{
			ActiveEnemy.target.GlobalPosition = ActiveEnemy.hitbox.GlobalPosition
			+ direction * 50;
			return AlligatorChase;
        }

		if (ActiveEnemy.GlobalPosition.DistanceTo(ActiveEnemy.homePosition) <= 70)
		{
			return AlligatorDeathRoll;
		}
		
		// Player needs to be able to dodge out of this state
		// The alligator will return to chase if the player dodges successfully

		return null;
    }
}