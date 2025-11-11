using Godot;

public partial class AlligatorState : Node
{
    public static Alligator ActiveEnemy;

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

    public virtual AlligatorState Process(double delta)
    {
        return null;
    }

    public virtual AlligatorState Physics(double delta)
    {
        return null;
    }
}