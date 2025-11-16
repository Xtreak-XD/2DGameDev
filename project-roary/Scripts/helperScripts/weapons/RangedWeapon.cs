using Godot;

// Extend this to create a ranged weapon that shoots a projectile.
public partial class RangedWeapon : Weapon
{
	// DO NOT OVERRIDE ANY OF THIS
	[Export]
	public PackedScene projectile;
	public Marker2D projectileSource;

	// If you override this, remember to call base._Ready() at
	// the start. Ideally, don't override this.
	public override void _Ready()
	{
		base._Ready();

		projectileSource = GetNode<Marker2D>("ProjectileSource");

		if (projectile == null)
		{
			GD.PrintErr("No projectile set.");
		}

		if(projectileSource == null)
        {
            GD.PrintErr("No projectile source found.");
        }
	}

	// If you override this, remember to call base.Attack()
	// at the start of the override.
	public override void Attack(Vector2 pos)
	{
		base.Attack(pos);

		//GD.Print("A ranged weapon has shot");

		Projectile proj = (Projectile)projectile.Instantiate();
		Owner.AddChild(proj);
		
		proj.GlobalPosition = projectileSource.GlobalPosition;
		proj.sprite.LookAt(pos);
		proj.Velocity = (pos - proj.GlobalPosition).Normalized() 
		* proj.data.speed;

		//GD.Print($"Projectile launch velocity: {proj.Velocity}");

		proj.parentWeapon = this;
		proj.data.Damage = data.damage;
		proj.data.knockback = data.knockback;
	}
}
