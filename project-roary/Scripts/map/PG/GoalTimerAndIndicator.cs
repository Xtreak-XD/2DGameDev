using Godot;

public partial class GoalTimerAndIndicator : Node2D
{
	public Timer timer;
	public Timer flashTimer;
	public Sprite2D sprite;
	public SceneManager sceneManager;

	public override void _Ready()
	{
		sceneManager = GetNode<SceneManager>("/root/SceneManager");
		timer = GetNode<Timer>("ParkingTimer");
		flashTimer = GetNode<Timer>("FlashTime");
		sprite = GetNode<Sprite2D>("Sprite2D");

		timer.Timeout += TimeOut;
		timer.Start();

		flashTimer.Timeout += Flash;
		flashTimer.Start();

		GD.Print("Timer has " + timer.TimeLeft + " seconds.");
	}
    public override void _ExitTree()
    {
        timer.Timeout -= TimeOut;
		flashTimer.Timeout -= Flash;
    }

	public void Flash()
    {
        sprite.Visible = !sprite.Visible;
    }
	
	public void TimeOut()
	{
		GD.Print("Player has run out of time.");
		var deadScene = "res://Scenes/ui/died_screen.tscn";
		sceneManager.goToScene(GetParent(), deadScene, false);
	}
}
