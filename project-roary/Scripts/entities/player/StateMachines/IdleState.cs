using State = PlayerState;
using Godot;

public partial class IdleState: State
{
    public State walk;

    public override void _Ready()
    {
        walk = GetNode<WalkState>("../walk");
    }

// what happens when player enters their new state
    public override void Enter()
    {
        player.UpdateAnimation("idle");
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

        if (player.SetDirection()){ player.UpdateAnimation("idle");}

        return null;
    }

    public override State Physics(double delta)
    {
        return null;
    }

    public override State HandleInput(InputEvent @event)
    {
        return null;
    }

}