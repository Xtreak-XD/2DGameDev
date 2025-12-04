using Godot;

public partial class DecelerateState : CarState
{
    public DriveState DriveState;
    public ReverseState ReverseState;
    public ParkState ParkState;
    public Timer parkTimer;
    private bool isParked = false;

    public override void _Ready()
    {
        DriveState = GetParent().GetNode<DriveState>("Drive");
        ParkState = GetParent().GetNode<ParkState>("Park");
        parkTimer = GetParent().GetNode<Timer>("ParkTimer");
        ReverseState = GetParent().GetNode<ReverseState>("Reverse");
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
        if (ActiveCar.IsParked() && parkTimer.IsStopped())
        {
            parkTimer.Start();
        }

        if(isParked)
        {
            return ParkState;
        }

        Vector2 currentVelocity = ActiveCar.Velocity;
        float decelerationAmount = ActiveCar.stats.Acceleration * 5 * (float)delta;

        if (currentVelocity.Length() > decelerationAmount)
        {
            Vector2 decelerationVector = currentVelocity.Normalized() * decelerationAmount ;
            ActiveCar.Velocity -= decelerationVector;
        }
        else
        {
            ActiveCar.Velocity = Vector2.Zero;
        }

        ActiveCar.MoveAndSlide();
        return null;
    }
    
    public void SetParked()
    {
        if(ActiveCar.IsParked())
        {
            isParked = true;
        }
    }

    public override CarState HandleInput(InputEvent @event)
    {
         if (@event.IsActionPressed("Up"))
        {
            return DriveState;
        }
        if (@event.IsActionPressed("Down"))
        {
            return ReverseState;
        }
        return null;
    }
}