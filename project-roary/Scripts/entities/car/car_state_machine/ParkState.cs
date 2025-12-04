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