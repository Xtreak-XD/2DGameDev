using Godot;
using Godot.Collections;

// Extend this to create a melee weapon that does NOT shoot a projectile.
public partial class MeleeWeapon : Weapon
{
    public Hitbox hitbox;

    public override void _Ready()
    {
        base._Ready();

        hitbox = GetNode<Hitbox>("Hitbox");
    }

    // If you override this, remember to call base.Attack()
    // at the start.
    // at the start of the override
    public override void Attack(Vector2 pos)
    {
        base.Attack(pos);

        GD.Print("Attack was from a melee weapon.");

        Array<Node2D> overlapping = hitbox.GetOverlappingBodies();

        foreach(Node2D node in overlapping)
        {
            if (node is HurtBox hurtBox)
            {
                if (hurtBox.GetParent().GetChildren().Contains(this))
                {
                    continue;
                }
                
                GD.Print("Hurtbox hit by melee weapon.");
                // Add damage logic here
            }
        }
    }
}
