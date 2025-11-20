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
		chargeDuration.Start();
	}

	public override void EnterState()
	{
		GD.Print("Roary has summoned a stampede of football players.");

		Vector2 viewSize = GetViewport().GetVisibleRect().Size;

		// Football scene has some bugs still, so we have to put it part way into
		// the viewport.
		footballScene = (FootballScene)ActiveEnemy.footballCharge.Instantiate();
		ActiveEnemy.Owner.AddChild(footballScene);
		footballScene.Position = GetViewport().GetVisibleRect().Position;
	}

    public override RoaryState Process(double delta)
    {
		if(Change)
        {
			ActiveEnemy.AdvancePhase();
			
            return Roam;
        }

        return null;
    }

	public void SetChange()
    {
        Change = true;
    }
}
