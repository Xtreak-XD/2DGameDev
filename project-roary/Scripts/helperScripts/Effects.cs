using Godot;
using System;

public partial class Effects : Node2D
{
    public Eventbus eventbus;

    public override void _Ready()
    {
        eventbus = GetNode<Eventbus>("/root/Eventbus");

        eventbus.hitStop += hitstop;
        eventbus.screenShake += screenShake;
        eventbus.knockBack += knockBack;
    }

    //Hitstop code
    public void hitstop(float duration)
    {
        GetTree().Paused = true;

        SceneTreeTimer timer = GetTree().CreateTimer(duration);

        timer.Timeout += () =>
        {
            GetTree().Paused = false;
        };
    }

    //screenshake
    public void screenShake(float intensity)
    {
        //we can add this later if we want, but it can have nice effects like if an enemy does a stump or something
    }

    //knockback
    public void knockBack(CharacterBody2D target, float strength, Vector2 sourcePosition)
    {
        if (!IsInstanceValid(target))
        {
            return;
        }

        Vector2 direction = (target.GlobalPosition - sourcePosition).Normalized();
        if (target is Enemy enemy)
        {
            enemy.ApplyKnockBack(direction, strength);
        }
        else if (target is Player player){ player.ApplyKnockBack(direction, strength); }
        
    }

    public override void _ExitTree()
    {
        eventbus.hitStop -= hitstop;
        eventbus.screenShake -= screenShake;
        eventbus.knockBack -= knockBack;
    }


}
