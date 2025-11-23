using Godot;

public partial class GoalTimerAndIndicator : Node2D
{
	public Timer timer;

	public override void _Ready()
	{
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
