using Godot;

public partial class WalkState: State
{
    [Export]
    public State idle;

    public override void _Ready()
    {
        idle = GetNode<IdleState>("../idle");
    }

    public override void Enter()
    {
        GD.Print("entered walk");
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

        player.Velocity = player.direction.Normalized() * player.data.Speed;

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
        return null;
    }

}