using Godot;

public partial class DriveableCar : CharacterBody2D
{
	public CarStateMachine stateMachine;
	public Vector2 direction = Vector2.Zero;

	[Export]
	public CarStats stats;

	public override void _Ready()
	{
		AddToGroup("player");

		stateMachine = GetNode<CarStateMachine>("DriveableCarStateMachine");
		stateMachine.Initialize(this);
	}

	public override void _Process(double delta)
	{	
		// TODO: FIGURE OUT HOW TO SMOOTH OUT THE MOVEMENT INPUT AND ROTATION
		float leftInput = Input.GetActionStrength("Left");
		float rightInput = Input.GetActionStrength("Right");
		float upINput = Input.GetActionStrength("Up");
		float downInput = Input.GetActionStrength("Down");

		direction.X += leftInput - rightInput;
		direction.Y += upINput - downInput;

		direction.X = Mathf.Clamp(direction.X, -1, 1f);
		direction.Y = Mathf.Clamp(direction.Y, -1, 1f);
    }

	public bool HasThrottle()
	{
		int rightActionStrength = (int)Input.GetActionStrength("Right");
		int leftActionStrength = (int)Input.GetActionStrength("Left");
		int upActionStrength = (int)Input.GetActionStrength("Up");
		int downActionStrength = (int)Input.GetActionStrength("Down");

		return rightActionStrength != 0 ||
			   leftActionStrength != 0 ||
			   upActionStrength != 0 ||
			   downActionStrength != 0;
	}

	public void SetRotation()
    {
		Rotation = Velocity.Angle();
    }
	
	public bool IsParked()
	{
		return Velocity.Length() == 0;
	}
}
