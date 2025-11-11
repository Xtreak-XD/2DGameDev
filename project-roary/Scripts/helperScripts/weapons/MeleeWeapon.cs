using Godot;

// Extend this to create a melee weapon that does NOT shoot a projectile.
public partial class MeleeWeapon : Weapon
{

    // If you override this, remember to call base.Attack()
    public override void Attack(Vector2 pos)
    {
        base.Attack(pos);

        GD.Print("Attack was from a melee weapon.");
    }
}
