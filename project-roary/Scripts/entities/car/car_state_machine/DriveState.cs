using System;
using Godot;

public partial class DriveState : CarState
{
    public DecelerateState DecelerateState;
    Vector2 accelerationVector = Vector2.Zero;
    float smoothTurn;
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
        if(accelerationVector != Vector2.Zero || turnAngle != 0f)
        {
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
        if (Math.Abs(turn) < 0.30f) { turn = 0f; }

        smoothTurn = Mathf.Lerp(smoothTurn, turn, 10f * (float)delta);
        turnAngle = Mathf.DegToRad(smoothTurn * ActiveCar.stats.SteeringSpeed);

        bool isAccelerating = Input.IsActionPressed("Up") || Input.GetActionStrength("throttle") > 0.1f;
        if (isAccelerating)
        {
            accelerationVector = ActiveCar.Transform.X * ActiveCar.stats.Acceleration;
        }
        else
        {
            accelerationVector = Vector2.Zero;
        }

        return null;
    }

    public override CarState HandleInput(InputEvent @event)
    {
        bool brakePressed = Input.GetActionStrength("break") > 0.1f || Input.IsActionPressed("Down");
        bool throttleReleased = !Input.IsActionPressed("Up") && Input.GetActionStrength("throttle") <= 0.1f;

        if(brakePressed || throttleReleased)
        {
            return DecelerateState;
        }
        return null;
    }
}