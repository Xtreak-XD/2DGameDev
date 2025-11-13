using State = PlayerState;
using Godot;

public partial class WalkState: State
{
    public State idle;
    public State dodge;

    public override void _Ready()
    {
        idle = GetNode<IdleState>("../idle");
        dodge = GetNode<DodgeState>("../dodge");
    }

    public override void Enter()
    {
        // Walk animation goes here
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

        player.Velocity = player.direction.Normalized() * player.data.Speed * (float)(player.data.Accel * delta);

        if (player.SetDirection())
        {
            player.UpdateAnimation("walk"); 
        }

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