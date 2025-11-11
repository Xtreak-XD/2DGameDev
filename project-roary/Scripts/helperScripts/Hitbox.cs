using Godot;
using System;
using System.Collections.Generic;

public partial class Hitbox : Area2D
{
    public GenericData data;
    public Eventbus eventbus;

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
            GD.Print(parent.GetType());
            GD.PushWarning("Hitbox parent is not a 'Character' type!");
        }

        AreaEntered += onAreaEntered;
    }

    public void onAreaEntered(Area2D area)
    {
        if (area.IsInGroup("hurtbox") && !(area.GetParent() == GetParent()))
        {
            eventbus.EmitSignal("applyDamage", area.GetParent(), GetParent(), data.Damage);
        }
    }
    
    public override void _ExitTree()
    {
        AreaEntered -= onAreaEntered;
    }

}
