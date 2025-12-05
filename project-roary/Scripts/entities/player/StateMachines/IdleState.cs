using State = PlayerState;
using Godot;

public partial class IdleState: State
{
    public State walk;
    public State dodge;
    public State attack;

    public override void _Ready()
    {
        walk = GetNode<WalkState>("../walk");
        dodge = GetNode<DodgeState>("../dodge");
        attack = GetNode<Attack>("../attack");
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
        if (@event.IsActionPressed("dodge"))
        {
            return dodge;
        }
        if (@event.IsActionPressed("attack"))
        {
            return attack;
        }
        return null;
    }

}