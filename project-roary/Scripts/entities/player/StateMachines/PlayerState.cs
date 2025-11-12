using Godot;

public partial class PlayerState: Node
{
    public static Player player;

    public override void _Ready()
    {
        
    }

// what happens when player enters their new state
    public virtual void Enter()
    {
        
    }

// what happens when player exits their current state
    public virtual void Exit()
    {
        
    }

    public virtual PlayerState Process(double delta)
    {
        return null;
    }

    public virtual PlayerState Physics(double delta)
    {
        return null;
    }

    public virtual PlayerState HandleInput(InputEvent @event)
    {
        return null;
    }

}