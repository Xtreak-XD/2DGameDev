using Godot;

public partial class ParkingSpot : Area2D
{
	public Eventbus eventbus;
	public DriveableCar car;

	public override void _Ready()
	{
		eventbus = GetNode<Eventbus>("/root/Eventbus");
		car = GetTree().GetNodesInGroup("player")[0] as DriveableCar;

		if(car == null)
        {
			GD.Print("The car could not be found.");
        }

		eventbus.playerReachedParkingSpot += EndMinigameWithSuccess;
	}

	public override void _Process(double delta)
	{
		if (GetOverlappingBodies().Contains(car))
		{
			eventbus.EmitSignal("playerReachedParkingSpot");
		}
	}
	
	public void EndMinigameWithSuccess()
    {
		GD.Print("The player has reached the parking spot");
    }
}
