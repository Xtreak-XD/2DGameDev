using Godot;

public partial class ParkingSpot : Area2D
{
	public Node car;
	public Timer ParkingTimer;
	public Timer FlashingTimer;
	public SceneManager sceneManager;

	public override void _Ready()
	{
		car = GetTree().GetFirstNodeInGroup("player");
		ParkingTimer = GetParent().GetNode<Timer>("ParkingTimer");
		FlashingTimer = GetParent().GetNode<Timer>("FlashTime");
		sceneManager = GetNode<SceneManager>("/root/SceneManager");
		BodyEntered += EndMinigameWithSuccess;

		//eventbus.playerReachedParkingSpot += EndMinigameWithSuccess;
	}

	public override void _Process(double delta)
	{
	}
	
	public async void EndMinigameWithSuccess(Node2D body)
	{
		if(body.IsInGroup("player") && body is DriveableCar car)
		{
			if(body.RotationDegrees >= 240 && body.RotationDegrees <= 300
			 || body.RotationDegrees >= 60 && body.RotationDegrees <= 120)
            {
				ParkingTimer.Paused = true;
				FlashingTimer.Paused = true;
				GD.Print("The player has reached the parking spot");
				//add some animation story thing here!

				//wait 3 secs
				await ToSignal(GetTree().CreateTimer(3.0), SceneTreeTimer.SignalName.Timeout);
				string path = "res://Scenes/map/Overworld/Overworld.tscn";
				if (!ResourceLoader.Exists(path))
				{
					GD.PrintErr($"First scene not found: {path}");
					return;
				}
				sceneManager.goToScene(GetParent().GetParent(), path, false);
			}
		}
		else
        {
			GD.Print("A non-player entered the parking spot");
        }
    }
}
