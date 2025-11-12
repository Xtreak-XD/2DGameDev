using Godot;
using System;
using System.Collections.Generic;

public partial class Hitbox : Area2D
{
    public GenericData data;
    public Eventbus eventbus;
    private List<Area2D> targetsInRange = new List<Area2D>();

    public override void _Ready()
    {
        
        AddToGroup("hitbox");
        eventbus = GetNode<Eventbus>("/root/Eventbus");
        Node parent = GetParent();

        if (parent is Player playerParent)
        {
            if (playerParent.data != null)
            {
                data = playerParent.data;
            }
            else
            {
                GD.PushWarning("Parent's 'data' property is null");
            }
        }
        else if (parent is Enemy enemyParent)
        {
            if (enemyParent.data != null)
            {
                data = enemyParent.data;
            }
            else
            {
                GD.PushWarning("Parent's 'data' property is null");
            }
        }
        else
        {
            GD.PushWarning("Hitbox parent is not a 'Character' type!");
        }

        AreaEntered += onAreaEntered;
        AreaExited += OnAreaExited;
    }

    public void onAreaEntered(Area2D area)
    {
        if (area.IsInGroup("hurtbox") && !(area.GetParent() == GetParent()))
        {
            targetsInRange.Add(area);
        }
    }

    public void OnAreaExited(Area2D area)
    {
        if (area.IsInGroup("hurtbox") && targetsInRange.Contains(area))
        {
            targetsInRange.Remove(area);
        }
    }
    public override void _Input(InputEvent @event)
    {
        if (@event.IsActionPressed("attack"))
        {
            foreach (var area in targetsInRange)
            {
                if (area != null && GodotObject.IsInstanceValid(area) && area.IsInsideTree())
                {
                    eventbus.EmitSignal("applyDamage", area.GetParent().Name,GetParent().Name,data.Damage);
                }
            }
        }
    }

}
