using System;
using Godot;

// Extend this to create a projectile that is shot from a RangedWeapon
public partial class Projectile : CharacterBody2D
{
	// DO NOT OVERRIDE ANY OF THIS
	public Timer projectileTimer;
	public Hitbox hitbox;
	public Vector2 spawn;
	public RangedWeapon parent;
	public Timer setSpawn;

	[Export]
	public ProjectileData data;

	// REMEMBER TO CALL base._Ready() at the start if
	// you override this.
	// Ideally, don't override this
	public override void _Ready()
	{
		projectileTimer = GetNode<Timer>("LifespanTimer");

		projectileTimer.WaitTime = data.lifeSpan;
		projectileTimer.Timeout += Kill;

		hitbox = GetNode<Hitbox>("Hitbox");
		parent = GetParent().GetParent<RangedWeapon>();

		hitbox.BodyEntered += HitEntity;

		spawn = GlobalPosition;
		GD.Print($"Projectile spawned at: {spawn}");

		projectileTimer.Start();
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
	// If you override this, call base.HitEntity() at the start
	// of your override
	public virtual void HitEntity(Node2D body)
	{
		if (body is not HurtBox)
		{
			return;
		}

		HurtBox hurtbox = (HurtBox)body;
		if(hurtbox.GetParent().GetChildren().Contains(parent))
        {
			return;
        }

		GD.Print("Projectile has hit a hurtbox.");
		
		// Add damage logic here

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
