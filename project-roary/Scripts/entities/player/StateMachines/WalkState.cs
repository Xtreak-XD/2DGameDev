using State = PlayerState;
using Godot;

public partial class WalkState: State
{
    public State idle;
    public State dodge;
    public State attack;


    public override void _Ready()
    {
        idle = GetNode<IdleState>("../idle");
        dodge = GetNode<DodgeState>("../dodge");
        attack = GetNode<Attack>("../attack");
    }

    public override void Enter()
    {
        player.UpdateAnimation("walk");
    }

// what happens when player exits their current state
    public override void Exit()
    {
        
    }

    public override State Process(double delta)
    {
        if (player.direction == Vector2.Zero)
        {
            return idle;
        }

        player.lastDirection = player.direction;

        if (player.SetDirection())
        {
            player.UpdateAnimation("walk"); 
        }

        return null;
    }

    public override State Physics(double delta)
    {
        Vector2 peakVelocity = player.direction.Normalized() * player.data.Speed;

        player.Velocity = peakVelocity * (float)(player.data.Accel * delta);

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