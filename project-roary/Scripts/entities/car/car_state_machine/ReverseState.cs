using Godot;
using System;
public partial class ReverseState : CarState
{
    public DecelerateState DecelerateState;
    public DriveState DriveState;
    Vector2 accelerationVector = Vector2.Zero;
    float turnAngle;
    float smoothTurn;
    public override void _Ready()
    {
        DriveState = GetParent().GetNode<DriveState>("Drive");
        DecelerateState = GetParent().GetNode<DecelerateState>("Decelerate");
    }
    
    public override void EnterState()
    {
        GD.Print("reversing");
    }
    
    public override void ExitState()
    {
        accelerationVector = Vector2.Zero;
        turnAngle = 0f;
    }
    
    public override CarState Process(double delta)
    {
        float turn = Input.GetAxis("Left", "Right");
        if (Math.Abs(turn) < 0.30f) { turn = 0f; }
        
        smoothTurn = Mathf.Lerp(smoothTurn, turn, 10f * (float)delta);
        turnAngle = Mathf.DegToRad(smoothTurn * ActiveCar.stats.SteeringSpeed);

        bool isReversing = Input.IsActionPressed("Down") || Input.GetActionStrength("break") > 0.1f;
        if (isReversing)
        {
            accelerationVector = -ActiveCar.Transform.X * ActiveCar.stats.Acceleration;
        }
        else
        {
            accelerationVector = Vector2.Zero;
        }
        
        return null;
    }
    
    public override CarState Physics(double delta)
    {
        if(accelerationVector != Vector2.Zero || turnAngle != 0f)
        {
            ActiveCar.Velocity += accelerationVector * (float)delta;
            if (turnAngle != 0f && ActiveCar.Velocity.Length() > 0.1f)
            {
                ActiveCar.Rotation += turnAngle * 5f * (float)delta;
            }
            float maxReverseSpeed = ActiveCar.stats.TopSpeed * 0.5f;
            if (ActiveCar.Velocity.Length() > maxReverseSpeed)
            {
                ActiveCar.Velocity = ActiveCar.Velocity.Normalized() * maxReverseSpeed;
            }
        }
        
        ActiveCar.MoveAndSlide();
        
        return null;
    }
    
    public override CarState HandleInput(InputEvent @event)
    {
        bool brakeReleased = !Input.IsActionPressed("Down") && Input.GetActionStrength("break") <= 0.1f;
        
        if (brakeReleased)
        {
            return DecelerateState;
        }
        return null;
    }
}