using System;
using Godot;

public partial class TimerLabel : Label
{
	public GoalTimerAndIndicator goalTimerAndIndicator;
	private double time;
	public DriveableCar player;
	public float timeTotal;
	public Camera2D camera;

	public override void _Ready()
    {
		LabelSettings.FontColor = Colors.White;
        goalTimerAndIndicator = GetParent().GetNode<GoalTimerAndIndicator>("GoalTimerAndIndicator");
		player = GetParent().GetNode<DriveableCar>("DriveableCar");

		timeTotal = (float) goalTimerAndIndicator.timer.WaitTime;
		camera = player.GetViewport().GetCamera2D();
    }

	public override void _Process(double delta)
    {
		time = goalTimerAndIndicator.timer.TimeLeft;
        Text = $"Find Parking | Time Left: {Math.Round(time, 2)} seconds";

		if(time < timeTotal / 4)
		{
			LabelSettings.FontColor = Colors.Red;
		}
		else if(time < timeTotal / 2)
        {
            LabelSettings.FontColor = Colors.Yellow;
        }
		float height = camera.GetWindow().Size.Y;

		GlobalPosition = camera.GetScreenCenterPosition() - new Vector2(Size.X / 2, height + 400);
    }
}