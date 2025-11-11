using Godot;

public partial class RangedWeapon : Weapon
{
	// For this, in any child of RangedWeapon, you likely
	// need to say "new."
	// public new PackedScene projectile;
	[Export]
	public PackedScene projectile;

	// If you override this, remember to call base._Ready()
	public override void _Ready()
	{
		base._Ready();

		if (projectile == null)
		{
			GD.PrintErr("No projectile set.");
		}
	}

	// If you override this, remember to call base.Attack()
    public override void Attack(Vector2 pos)
    {
		base.Attack(pos);

		GD.Print("A ranged weapon has shot");

		Projectile proj = (Projectile)projectile.Instantiate();
		proj.Position = GlobalPosition;
		proj.Velocity = (pos - GlobalPosition).Normalized() * proj.data.speed;

		AddChild(proj);
    }
}
