using Godot;
using System;

public partial class MeleeWeapon : Weapon
{
    
    // If you override this, remember to call base.Attack()
	public override void Attack(Vector2 pos)
    {
        base.Attack(pos);

        GD.Print("Attack was from a melee weapon.");
    }
}
