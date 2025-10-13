using Godot;

public partial class State: Node
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

    public virtual State Process(double delta)
    {
        return null;
    }

    public virtual State Physics(double delta)
    {
        return null;
    }

    public virtual State HandleInput(InputEvent @event)
    {
        return null;
    }

}