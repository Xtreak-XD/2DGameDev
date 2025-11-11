using System;
using Godot;

// Extend this to create a projectile that is shot from a RangedWeapon
public partial class Projectile : CharacterBody2D
{
	public Timer projectileTimer;
	public Hitbox hitbox;
	public RangedWeapon parent;

	public Vector2 spawn = Vector2.Zero;

	// For this, in any child of Projectile, you likely
	// need to say "new."
	// public new ProjectileData data;
	[Export]
	public ProjectileData data;

	// REMEMBER TO CALL base._Ready() if you override this
	public override void _Ready()
	{
		projectileTimer = GetNode<Timer>("LifespanTimer");

		projectileTimer.WaitTime = data.lifeSpan;
		projectileTimer.Timeout += Kill;

		parent = GetParent().GetParent<RangedWeapon>();

		hitbox = GetNode<Hitbox>("Hitbox");

		spawn = GlobalPosition;

		hitbox.BodyEntered += HitEntity;

		projectileTimer.Start();
		GD.Print($"Projectile spawned at: {spawn}");
	}

	// DO NOT OVERRIDE
	public override void _PhysicsProcess(double delta)
	{
		if (GlobalPosition.DistanceTo(spawn) >= data.maxDistance)
		{
			Kill();
		}

		Travel(delta);
	}

	// Controls special behavior of the projectile in flight.
	// REMEMBER TO CALL base.Travel() if you override this.
	// You have to call base.Travel() at the end of your override.
	public virtual void Travel(double delta)
	{
		MoveAndSlide();
	}

	// Override to add special effects when this
	// projectile hits an entity
	// REMEMBER TO CALL base.HitEntity() if you override this.
	public virtual void HitEntity(Node2D body)
	{
		if (body is not HurtBox)
		{
			return;
		}
		
		GD.Print("Projectile has hit a hurtbox.");
		QueueFree();
    }

	// DO NOT OVERRIDE
	public void Kill()
	{
		GD.Print($"Projectile has been destroyed with " +
		$"{Math.Round(projectileTimer.TimeLeft, 2)} seconds left.");
		QueueFree();
	}
}
