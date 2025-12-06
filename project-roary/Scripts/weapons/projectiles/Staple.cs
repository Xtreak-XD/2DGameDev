using Godot;

public partial class Staple : Projectile
{
	public override void HitEntity(Area2D body)
    {
		base.HitEntity(body);

		// Add stun effect if this hit a hurtbox.
    }
}
