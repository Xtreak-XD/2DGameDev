using Godot;

public partial class DriveState : CarState
{
    public DecelerateState DecelerateState;
    Vector2 accelerationVector = Vector2.Zero;
    float turnAngle;
    public override void _Ready()
    {
        DecelerateState = GetParent().GetNode<DecelerateState>("Decelerate");
    }

    public override void EnterState()
    {
        GD.Print("Car is driving.");
    }

    public override void ExitState()
    {
        accelerationVector = Vector2.Zero;
        turnAngle = 0f;
    }

    public override CarState Physics(double delta)
	{
        if(accelerationVector != Vector2.Zero || turnAngle != 0f){
            ActiveCar.Velocity += accelerationVector * (float)delta;
            ActiveCar.Velocity = ActiveCar.Velocity.Rotated(turnAngle);

            if (ActiveCar.Velocity.Length() > ActiveCar.stats.TopSpeed)
            {
                ActiveCar.Velocity = ActiveCar.Velocity.Normalized() * ActiveCar.stats.TopSpeed;
            }
        }

        ActiveCar.MoveAndSlide();
		return null;
	}

    public override CarState Process(double delta)
    {
        float turn = Input.GetAxis("Left", "Right");
        turnAngle = Mathf.DegToRad(turn * ActiveCar.stats.SteeringSpeed);

        accelerationVector = ActiveCar.Transform.X * ActiveCar.stats.Acceleration;

        return null;
    }

    public override CarState HandleInput(InputEvent @event)
    {
        if(@event.IsActionPressed("Down") || @event.IsActionReleased("Up"))
        {
            return DecelerateState;
        }
        return null;
    }
}