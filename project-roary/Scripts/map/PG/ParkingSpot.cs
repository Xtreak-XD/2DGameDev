using Godot;

public partial class ParkingSpot : Area2D
{
	public Node car;

	public override void _Ready()
	{
		car = GetTree().GetFirstNodeInGroup("player");

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
