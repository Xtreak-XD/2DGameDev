using Godot;
using System;

public partial class HurtBox : Area2D
{
    public Eventbus eventbus;
    public GenericData data;
    public override void _Ready()
    {
        AddToGroup("hurtbox");
        eventbus = GetNode<Eventbus>("/root/Eventbus");
        eventbus.applyDamage += onApplyDmg;

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
            GD.PushWarning("Hurtbox parent is not a 'Character' type!");
        }
    }

    public void onApplyDmg(string dmgReceiverName,string dmgDealerName, int dmg)
    {
        GD.Print($"incoming dmg: {dmg} dealing to {dmgReceiverName} by {dmgDealerName}");
        //add functionality here!
    }

}
