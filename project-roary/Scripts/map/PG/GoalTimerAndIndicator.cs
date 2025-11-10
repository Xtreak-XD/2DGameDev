using Godot;

public partial class GoalTimerAndIndicator : Node2D
{
	public Eventbus eventbus;
	public Timer timer;

	public override void _Ready()
	{
		eventbus = GetNode<Eventbus>("/root/Eventbus");
		timer = GetNode<Timer>("ParkingTimer");

		eventbus.playerOutOfTime += EndMinigameWithTimeOut;

		timer.Timeout += TimeOut;
		timer.Start();
	}
	
	public void TimeOut()
	{
		eventbus.EmitSignal("playerOutOfTime");
	}
	
	public void EndMinigameWithTimeOut()
    {
		GD.Print("Player has run out of time.");
    }
}
