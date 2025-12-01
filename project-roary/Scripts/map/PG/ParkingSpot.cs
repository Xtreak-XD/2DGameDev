using Godot;

public partial class ParkingSpot : Area2D
{
	public Node car;
	public Timer ParkingTimer;
	public Timer FlashingTimer;

	public override void _Ready()
	{
		car = GetTree().GetFirstNodeInGroup("player");
		ParkingTimer = GetParent().GetNode<Timer>("ParkingTimer");
		FlashingTimer = GetParent().GetNode<Timer>("FlashTime");

		BodyEntered += EndMinigameWithSuccess;

		//eventbus.playerReachedParkingSpot += EndMinigameWithSuccess;
	}

	public override void _Process(double delta)
	{
	}
	
	public void EndMinigameWithSuccess(Node2D body)
	{
		if(body.IsInGroup("player") && body is DriveableCar car)
		{
			if(body.RotationDegrees >= 240 && body.RotationDegrees <= 300
			 || body.RotationDegrees >= 60 && body.RotationDegrees <= 120)
            {
				ParkingTimer.Paused = true;
				FlashingTimer.Paused = true;
				GD.Print("The player has reached the parking spot");
            }
		}
		else
        {
			GD.Print("A non-player entered the parking spot");
        }
    }
}
