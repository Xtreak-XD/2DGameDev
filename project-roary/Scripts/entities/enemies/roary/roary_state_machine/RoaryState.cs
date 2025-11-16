using Godot;

public partial class RoaryState : Node
{
    public static Roary ActiveEnemy;

    public override void _Ready()
    {

    }

    // Called when the state is entered	
    public virtual void EnterState()
    {
    }

    // Called when the state is exited
    public virtual void ExitState()
    {
    }

    public virtual RoaryState Process(double delta)
    {
        return null;
    }

    public virtual RoaryState Physics(double delta)
    {
        return null;
    }
}