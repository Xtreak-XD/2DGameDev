using Godot;
using System;

public partial class HurtBox : Area2D
{
    public Eventbus eventbus;
    public GenericData data;
    public Node parent;
    public override void _Ready()
    {
        AddToGroup("hurtbox");
        eventbus = GetNode<Eventbus>("/root/Eventbus");
        eventbus.applyDamage += onApplyDmg;

        parent = GetParent();

        if (parent is Player playerParent)
        {
            if (playerParent.data != null)
            {
                data = playerParent.data;
                GD.Print($"connected hurtbox to {parent.Name}");
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
                GD.Print($"connected hurtbox to {parent.Name}");
            }
            else
            {
                GD.PushWarning("Parent's 'data' property is null");
            }
        }
        else
        {
            GD.Print("Hurtbox parent is not a 'Character' type!");
        }
    }

    public void onApplyDmg(Node dmgReceiver, Node dmgDealer, int dmg)
    {
        if (parent == null || parent != dmgReceiver) return;

        if (parent.IsInGroup("enemy") && dmgDealer.IsInGroup("enemy")) return;

        //Subracting health
        data.Health -= dmg;
        if (parent.IsInGroup("player"))
        {
            eventbus.EmitSignal("updateHealth", data.Health);
        }
        GD.Print($"{parent.Name} took {dmg} damage, from {dmgDealer},remaining health: {data.Health}");

        //Calls Die() function if health reaches 0 or below
        if (data.Health <= 0 && IsInstanceValid(parent))
        {
            if (Owner is Enemy enemy) { enemy.Die(); }
            /*
            else if(Owner is Player player) {player.Die();}
            -> We can implement this later. :). Don't feel like doing this right now.
            */
        }
    }

    public override void _ExitTree()
    {
        eventbus.applyDamage -= onApplyDmg;
    }

}
