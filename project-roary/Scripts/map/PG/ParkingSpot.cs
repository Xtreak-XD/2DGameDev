using System;
using Godot;

public partial class ParkingSpot : Area2D
{
	public Node car;
	public Timer ParkingTimer;
	public Timer FlashingTimer;
	public SceneManager sceneManager;
	public float score;

	public override void _Ready()
	{
		car = GetTree().GetFirstNodeInGroup("player");
		ParkingTimer = GetParent().GetNode<Timer>("ParkingTimer");
		FlashingTimer = GetParent().GetNode<Timer>("FlashTime");
		sceneManager = GetNode<SceneManager>("/root/SceneManager");
		BodyEntered += EndMinigameWithSuccess;
		score = 0;

		//eventbus.playerReachedParkingSpot += EndMinigameWithSuccess;
	}

	public override void _Process(double delta)
    {
        if(car is DriveableCar driveableCar)
        {
            if(driveableCar.Velocity.Length() >= driveableCar.stats.TopSpeed * 0.99)
            {
                score += 4;
            }
        }
	}
	
	public async void EndMinigameWithSuccess(Node2D body)
	{
		if(body.IsInGroup("player") || body is DriveableCar)
		{
			if(body.RotationDegrees >= 240 && body.RotationDegrees <= 300
			 || body.RotationDegrees >= 60 && body.RotationDegrees <= 120)
            {
				ParkingTimer.Paused = true;
				FlashingTimer.Paused = true;

				GD.Print("The player has reached the parking spot");
				GD.Print("Speed Based Score: " + score);
				GD.Print("Time Based Score: " + Math.Round(ParkingTimer.TimeLeft * 7000, 2));

				score += (float)ParkingTimer.TimeLeft * 7000;
				GD.Print("Total Score: " + Math.Round(score, 2));
				
				int cashAward = (int)(score / 200);
				GD.Print("Cash Award: $" + cashAward);
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
