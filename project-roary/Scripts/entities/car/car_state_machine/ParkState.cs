using Godot;

public partial class ParkState : CarState
{
    public DriveState DriveState;

    public override void _Ready()
    {
        DriveState = GetParent().GetNode<DriveState>("Drive");
    }

    public override void EnterState()
    {
        GD.Print("Car has parked.");
    }

    public override void ExitState()
    {
        GD.Print("Car has resumed driving.");
    }

    public override CarState Process(double delta)
    {
        if (ActiveCar.HasThrottle() || Input.IsActionPressed("Down"))
        {
            return DriveState;
        }

        return null;
    }
}