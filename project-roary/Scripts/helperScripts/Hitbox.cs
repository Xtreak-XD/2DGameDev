using Godot;
using System;

public partial class Hitbox : Area2D
{
    public GenericData data;
    public Eventbus eventbus;

    public ProjectileData projectileData;

    public WeaponData meleeData;

    public override void _EnterTree()
    {
        eventbus = GetNode<Eventbus>("/root/Eventbus");
        AreaEntered += onAreaEntered;
    }


    public override void _Ready()
    {
        
        AddToGroup("hitbox");
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
        else if (parent is Projectile projectileParent)
        {
            if (projectileParent.data != null)
            {
                projectileData = projectileParent.data;
            }
            else
            {
                GD.PushWarning("Parent's 'data' property is null");
            }
        }
        else if (parent is EnemyProjectile enemyProjectileParent)
        {
            if (enemyProjectileParent.data != null)
            {
                projectileData = enemyProjectileParent.data;
            }
            else
            {
                GD.PushWarning("Parent's 'data' property is null");
            }
        }
         else if (parent is MeleeWeapon MelleeParent)
        {
            if (MelleeParent.data != null)
            {
                meleeData = MelleeParent.data;
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
    }

    public void onAreaEntered(Area2D area)
    {
        if (area.IsInGroup("hurtbox") && !(area.GetParent() == GetParent()) && !(GetParent() is Projectile) && !(GetParent() is MeleeWeapon))
        {
            eventbus.EmitSignal("applyDamage", area.GetParent(), GetParent(), data.Damage);
            eventbus.EmitSignal("hitStop", 0.05); //set duration for hitstop

            if (data.dealKnockback)
            {
                eventbus.EmitSignal("knockBack", (CharacterBody2D)area.GetParent(), data.knockBackAmount * 5, GlobalPosition);
            }
        }
        else if (area.IsInGroup("hurtbox") && area.GetParent().IsInGroup("enemy") && !(area.GetParent() == GetParent()) && (GetParent() is Projectile))
        {
            eventbus.EmitSignal("applyDamage", area.GetParent(), GetParent(), projectileData.Damage);
            eventbus.EmitSignal("hitStop", 0.05);

            eventbus.EmitSignal("knockBack", (CharacterBody2D)area.GetParent(), projectileData.knockback * 5, GlobalPosition);
        }
        else if (area.IsInGroup("hurtbox") && area.GetParent().IsInGroup("enemy") && !(area.GetParent() == GetParent()) && (GetParent() is MeleeWeapon))
        {
            eventbus.EmitSignal("applyDamage", area.GetParent(), GetParent(), meleeData.damage);
            eventbus.EmitSignal("hitStop", 0.05);

            eventbus.EmitSignal("knockBack", (CharacterBody2D)area.GetParent(), meleeData.knockback * 5, GlobalPosition);
        }
         else if (area.IsInGroup("hurtbox") && area.GetParent().IsInGroup("player") && !(area.GetParent() == GetParent()) && (GetParent() is EnemyProjectile))
        {
            eventbus.EmitSignal("applyDamage", area.GetParent(), GetParent(), projectileData.Damage);
            eventbus.EmitSignal("hitStop", 0.05);

            eventbus.EmitSignal("knockBack", (CharacterBody2D)area.GetParent(), projectileData.knockback * 5, GlobalPosition);
        }
    }
    
    public override void _ExitTree()
    {
        AreaEntered -= onAreaEntered;
    }

}
