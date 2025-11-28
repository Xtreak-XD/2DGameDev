using Godot;

public partial class ParkingSpot : Area2D
{
	public Node car;
	public Timer ParkingTimer;

	public override void _Ready()
	{
		car = GetTree().GetFirstNodeInGroup("player");
		ParkingTimer = GetParent().GetNode<Timer>("ParkingTimer");

		BodyEntered += EndMinigameWithSuccess;

		//eventbus.playerReachedParkingSpot += EndMinigameWithSuccess;
	}

	public override void _Process(double delta)
	{
	}
	
	public void EndMinigameWithSuccess(Node2D body)
	{
		if (body.IsInGroup("player"))
		{
			ParkingTimer.Paused = true;
			GD.Print("The player has reached the parking spot");
		}
    }
}
