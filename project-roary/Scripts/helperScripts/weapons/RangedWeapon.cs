using Godot;

// Extend this to create a ranged weapon that shoots a projectile.
public partial class RangedWeapon : Weapon
{
	// DO NOT OVERRIDE ANY OF THIS
	[Export]
	public PackedScene projectile;
	public Node projectileSource;

	// If you override this, remember to call base._Ready() at
	// the start.
	// Ideally, don't override this
	public override void _Ready()
	{
		base._Ready();

		projectileSource = GetNode<Node>("ProjectileSource");

		if (projectile == null)
		{
			GD.PrintErr("No projectile set.");
		}
	}

	// If you override this, remember to call base.Attack()
	// at the start of the override.
	public override void Attack(Vector2 pos)
	{
		base.Attack(pos);

		GD.Print("A ranged weapon has shot");

		Projectile proj = (Projectile)projectile.Instantiate();
		proj.data.damage = data.damage;
		projectileSource.AddChild(proj);

		proj.Position = GlobalPosition;
		proj.Rotation = Rotation;
		proj.Velocity = (pos - GlobalPosition).Normalized() * proj.data.speed;
	}
}
