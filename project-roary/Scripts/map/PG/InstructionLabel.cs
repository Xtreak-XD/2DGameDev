using Godot;

public partial class InstructionLabel : Label
{
	public DriveableCar player;
	public Camera2D camera;

	public override void _Ready()
    {
        player = GetParent().GetNode<DriveableCar>("DriveableCar");
		camera = player.GetViewport().GetCamera2D();
    }

	public override void _Process(double delta)
    {
		float height = camera.GetWindow().Size.Y;

        GlobalPosition = camera.GetScreenCenterPosition() - new Vector2(Size.X / 2, (height / 2) - 1200);
    }
}
