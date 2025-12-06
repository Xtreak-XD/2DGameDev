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

		// Get the camera's position to spawn stampede in world space
		Camera2D camera = GetViewport().GetCamera2D();
		Vector2 worldPosition = Vector2.Zero;

		if (camera != null)
		{
			// Use camera's global position as the center of the visible area
			worldPosition = camera.GetScreenCenterPosition();
			GD.Print($"Spawning stampede at camera position: {worldPosition}");
		}
		else
		{
			// Fallback: use Roary's position if no camera found
			worldPosition = ActiveEnemy.GlobalPosition;
			GD.Print($"No camera found, spawning stampede at Roary's position: {worldPosition}");
		}

		footballScene = (FootballScene)ActiveEnemy.footballCharge.Instantiate();
		ActiveEnemy.Owner.AddChild(footballScene);
		footballScene.GlobalPosition = worldPosition;
	}

	public override void ExitState()
    {
        GD.Print("==================== STAMPEDE EXIT STATE ====================");
		GD.Print($"Phase BEFORE AdvancePhase(): {ActiveEnemy.Phase}");
		GD.Print($"Health: {ActiveEnemy.GetHealthPercentage():F2}");
		
		ActiveEnemy.AdvancePhase();
		
		GD.Print($"Phase AFTER AdvancePhase(): {ActiveEnemy.Phase}");
		GD.Print($"AdvancePhase() completed successfully");
		GD.Print("=============================================================");
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
