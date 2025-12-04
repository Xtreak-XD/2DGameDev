using Godot;
using System;
public partial class ReverseState : CarState
{
    public DecelerateState DecelerateState;
    public DriveState DriveState;
    Vector2 accelerationVector = Vector2.Zero;
    float turnAngle;
    
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
        turnAngle = Mathf.DegToRad(turn * ActiveCar.stats.SteeringSpeed);
        
        accelerationVector = -ActiveCar.Transform.X * ActiveCar.stats.Acceleration;
        
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

            // Use lower max speed for reverse
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
        if (@event.IsActionReleased("Down"))
        {
            return DecelerateState;
        }
        return null;
    }
}