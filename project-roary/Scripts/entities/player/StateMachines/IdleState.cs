using Godot;

public partial class IdleState: State
{
    public State walk;
    public State dodge;

    public override void _Ready()
    {
        walk = GetNode<WalkState>("../walk");
        dodge = GetNode<DodgeState>("../dodge");
    }

// what happens when player enters their new state
    public override void Enter()
    {
        // idle animation goes here
    }

// what happens when player exits their current state
    public override void Exit()
    {
        
    }

    public override State Process(double delta)
    {
        if (player.direction != Vector2.Zero)
        {
            return walk;
        }

        player.Velocity = Vector2.Zero;

        return null;
    }

    public override State Physics(double delta)
    {
        return null;
    }

    public override State HandleInput(InputEvent @event)
    {
        if (@event.IsActionPressed("dodge"))
        {
            return dodge;
        }
        return null;
    }

}