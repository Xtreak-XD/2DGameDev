using System;
using Godot;

public partial class GoToArenaCenter : RoaryState
{
	public Vector2 CENTER_POSITION = Vector2.Zero; // SET TO ACTUAL CENTER OF STADIUM

	public SummonFootballStampede SummonFootballStampede;
	public RoaryRoam Roam;
	public ThrowFootball ThrowFootball;
	public RoaryDash Dash;
	
	public override void _Ready()
    {
        SummonFootballStampede = GetParent()
		.GetNode<SummonFootballStampede>("FootballPlayerStampede");
		Roam = GetParent().GetNode<RoaryRoam>("RoaryRoam");
		Dash = GetParent().GetNode<RoaryDash>("RoaryDash");
		ThrowFootball = GetParent().GetNode<ThrowFootball>("ThrowFootball");
    }
	
	public override void EnterState()
	{
		GD.Print("Roary is now going to the center of the stadium.");
	}

	public override RoaryState Process(double delta)
    {
		if(ActiveEnemy.GlobalPosition.DistanceTo(CENTER_POSITION) <= 20)
        {
            ActiveEnemy.Velocity = Vector2.Zero;
			
			Random random = new();
			int num = random.Next(0, 3);

            return num switch
            {
                0 => Dash,
                1 => ThrowFootball,
                2 => SummonFootballStampede,
                _ => Roam,
            };
        }

        Vector2 direction = (CENTER_POSITION - ActiveEnemy.GlobalPosition).Normalized();
		//ActiveEnemy.animation(direction); COMMENTED OUT BECAUSE WE DO NOT HAVE ANIMATIONS
		ActiveEnemy.Velocity = direction * ActiveEnemy.data.Speed * 
		((float)delta * (float)ActiveEnemy.data.Accel);
		ActiveEnemy.MoveAndSlide();

		return null;
    }
}
