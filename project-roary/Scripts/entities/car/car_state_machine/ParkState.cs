using Godot;

public partial class ParkState : CarState
{
    public DriveState DriveState;
    public ReverseState ReverseState;
    public DecelerateState DecelerateState;
    public override void _Ready()
    {
        DriveState = GetParent().GetNode<DriveState>("Drive");
        ReverseState = GetParent().GetNode<ReverseState>("Reverse");
    }

    public override void EnterState()
    {
        ActiveCar.Velocity = Vector2.Zero;
        GD.Print("Car has parked.");
    }

    public override void ExitState()
    {
        GD.Print("Car has resumed driving.");
    }

    public override CarState Process(double delta)
    {
        return null;
    }

    public override CarState HandleInput(InputEvent @event)
    {
        bool throttlePressed = Input.GetActionStrength("throttle") > 0.1f || Input.IsActionPressed("Up");
        
        // Check for brake trigger or S key
        bool brakePressed = Input.GetActionStrength("break") > 0.1f || Input.IsActionPressed("Down");

        if (throttlePressed)
        {
            return DriveState;
        }

        if (brakePressed)
        {
            return ReverseState;
        }

        return null;
    }
}