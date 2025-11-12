using Godot;

public partial class GoalTimerAndIndicator : Node2D
{
	public Eventbus eventbus;
	public Timer timer;

	public override void _Ready()
	{
		eventbus = GetNode<Eventbus>("/root/Eventbus");
		timer = GetNode<Timer>("ParkingTimer");

		timer.Timeout += TimeOut;
		timer.Start();

		GD.Print("Timer has " + timer.TimeLeft + " seconds.");
	}
	
	public void TimeOut()
	{
		GD.Print("Player has run out of time.");
	}
}
