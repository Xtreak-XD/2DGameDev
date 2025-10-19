using Godot;
using System;

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
            GD.PushWarning("Hitbox parent is not a 'Character' type!");
        }

        AreaEntered += onAreaEntered;
    }

    public void onAreaEntered(Area2D area)
    {
        if (area.IsInGroup("hurtbox") && !(area.GetParent() == GetParent()))
        {
            eventbus.EmitSignal("applyDamage", area.GetParent().Name,GetParent().Name, data.Damage);
            GD.Print("apply dmg event emitted!");
        }
    }

}
