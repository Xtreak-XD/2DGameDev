using Godot;

public partial class ParkingSpot : Area2D
{
	public Node2D car;

	public override void _Ready()
	{
		car = GetTree().GetNodesInGroup("player")[0] as Node2D;

		GD.Print(car.Name);

		if (car == null)
		{
			GD.Print("The car could not be found.");
		}

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
			GD.Print("The player has reached the parking spot");
		}
    }
}
