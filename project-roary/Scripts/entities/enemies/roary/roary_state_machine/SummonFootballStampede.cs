using Godot;

public partial class SummonFootballStampede : RoaryState
{
	public RoaryRoam Roam;
	public Timer chargeDuration;
	bool Change = false;
	private FootballScene footballScene;

    public override void _Ready()
	{
		Roam = GetParent().GetNode<RoaryRoam>("RoaryRoam");
		chargeDuration = GetParent().GetNode<Timer>("FootballChargeDuration");

		chargeDuration.Timeout += SetChange;
	}

	public override void EnterState()
	{
		GD.Print("Roary has summoned a stampede of football players.");

		// Stop all movement during stampede
		ActiveEnemy.Velocity = Vector2.Zero;
		Change = false;

		// Start the timer when entering this state
		chargeDuration.Start();

		Vector2 viewSize = GetViewport().GetVisibleRect().Size;

		// Football scene has some bugs still, so we have to put it part way into
		// the viewport.
		footballScene = (FootballScene)ActiveEnemy.footballCharge.Instantiate();
		ActiveEnemy.Owner.AddChild(footballScene);
		footballScene.Position = GetViewport().GetVisibleRect().Position;
	}

	public override void ExitState()
    {
        ActiveEnemy.AdvancePhase();
		chargeDuration.Stop();
    }

    public override RoaryState Process(double delta)
    {
		// Keep Roary still during stampede
		ActiveEnemy.Velocity = Vector2.Zero;
		ActiveEnemy.MoveAndSlide();

		if(Change)
        {
            return Roam;
        }

        return null;
    }

	public void SetChange()
    {
        Change = true;
    }
}
