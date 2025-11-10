using Godot;

public partial class DriveState : CarState
{
    public DecelerateState DecelerateState;

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
    }

    public override CarState Process(double delta)
    {
        if (!ActiveCar.HasThrottle())
        {
            return DecelerateState;
        }

        GD.Print($"Car Velocity: ({ActiveCar.Velocity.X}," +
        $"{ActiveCar.Velocity.Y})");
        GD.Print("Car Speed: " + ActiveCar.GetVelocity().Length());
        GD.Print("Car Position: " + ActiveCar.Position);
        
        /*
        ActiveCar.Velocity = ActiveCar.Velocity.Lerp(ActiveCar.direction
         * ActiveCar.stats.Acceleration * (float)delta,
        ActiveCar.stats.TopSpeed);
        */

        if (ActiveCar.Velocity.Length() > ActiveCar.stats.TopSpeed)
        {
            ActiveCar.Velocity = ActiveCar.Velocity.Normalized()
             * ActiveCar.stats.TopSpeed;
        }

        return null;
    }
}