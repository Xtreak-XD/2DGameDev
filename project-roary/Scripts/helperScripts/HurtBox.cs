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
        eventbus.Connect("applyDamage", new Callable(this, nameof(onApplyDmg)));

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

    public void onApplyDmg(string dmgReceiverName, string dmgDealerName, int dmg)
    {
        if (Owner == null || Owner.Name != dmgReceiverName) return;

        //Subracting health
        data.Health -= dmg;
        GD.Print($"{Owner.Name} took {dmg} damage, remaining health: {data.Health}");

        //Calls Die() function if health reaches 0 or below
        if(data.Health <= 0 && IsInstanceValid(Owner))
        {
            if (Owner is Enemy enemy) { enemy.Die(); }
            /*
            else if(Owner is Player player) {player.Die();}
            -> We can implement this later. :). Don't feel like doing this right now.
            */
        }
    }
}
