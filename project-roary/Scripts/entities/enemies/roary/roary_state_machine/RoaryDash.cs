using Godot;

public partial class RoaryDash : RoaryState
{
	public GoToArenaCenter GoToCenter;
	public ThrowFootball ThrowFootball;
	public Vector2 dashStartPos;

	public override void _Ready()
    {
        GoToCenter = GetParent().GetNode<GoToArenaCenter>("GoToArenaCenter");
		ThrowFootball = GetParent().GetNode<ThrowFootball>("ThrowFootball");
    }

	public override void EnterState()
    {
        GD.Print("Roary is now dashing at the player");
		dashStartPos = ActiveEnemy.GlobalPosition;
    }

	public override RoaryState Process(double delta)
    {
		Vector2 targetPos = ActiveEnemy.target.GlobalPosition;
		Vector2 currentPos = ActiveEnemy.GlobalPosition;
		Vector2 targetVel = ActiveEnemy.target.Velocity;

		Vector2 velocity = currentPos.Lerp(targetPos + targetVel, 200).Normalized();

		//ActiveEnemy.animation(direction); COMMENTED OUT BECAUSE WE DO NOT HAVE ANIMATIONS
		ActiveEnemy.Velocity = velocity * ActiveEnemy.TrueSpeed() * 
		 2.5f * (ActiveEnemy.TrueAcceleration() * (float) delta);
		
		ActiveEnemy.MoveAndSlide();

		if(currentPos.DistanceTo(dashStartPos) >= 1500)
        {
			GD.Print("The player evaded Roary's dash");
            return ThrowFootball;
        }

		if(targetPos.DistanceTo(currentPos) <= 90) // Change for attack radius.
        {										  
            return GoToCenter;
        }

		return null;
	}
}