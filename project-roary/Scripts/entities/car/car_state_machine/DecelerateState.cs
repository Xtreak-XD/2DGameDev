using Godot;

public partial class DecelerateState : CarState
{
    public DriveState DriveState;
    public ParkState ParkState;
    public Timer parkTimer;
    private bool isParked = false;

    public override void _Ready()
    {
        DriveState = GetParent().GetNode<DriveState>("Drive");
        ParkState = GetParent().GetNode<ParkState>("Park");
        parkTimer = GetParent().GetNode<Timer>("ParkTimer");

        parkTimer.Timeout += SetParked;
    }

    public override void EnterState()
    {
        isParked = false;
        GD.Print("Car is decelerating.");
    }

    public override void ExitState()
    {
    }

    public override CarState Process(double delta)
    {
        if (ActiveCar.HasThrottle())
        {
            return DriveState;
        }

        if (ActiveCar.IsParked() && parkTimer.IsStopped())
        {
            parkTimer.Start();
        }

        if(isParked)
        {
            return ParkState;
        }

        GD.Print($"Car Velocity: ({ActiveCar.Velocity.X}," +
        $"{ActiveCar.Velocity.Y})");
        GD.Print("Car Speed: " + ActiveCar.GetVelocity().Length());

        Vector2 currentVelocity = ActiveCar.Velocity;
        float decelerationAmount = ActiveCar.stats.Acceleration * (float)delta;

        if (currentVelocity.Length() > decelerationAmount)
        {
            Vector2 decelerationVector = currentVelocity.Normalized()
             * decelerationAmount;
            ActiveCar.Velocity -= decelerationVector;
        }
        else
        {
            ActiveCar.Velocity = Vector2.Zero;
        }

        ActiveCar.SetRotation();
        ActiveCar.MoveAndCollide(ActiveCar.Velocity);
        return null;
    }
    
    public void SetParked()
    {
        if(ActiveCar.IsParked())
        {
            isParked = true;
        }
    }
}