using Godot;

// Extend this to create a projectile that is shot from a RangedWeapon
public partial class Projectile : CharacterBody2D
{
	// DO NOT OVERRIDE ANY OF THIS
	public Timer projectileTimer;
	public Hitbox hitbox;
	public Vector2 spawn;
	public RangedWeapon parentWeapon;
	public Timer setSpawn;
	public Sprite2D sprite;

	[Export]
	public ProjectileData data;

	// REMEMBER TO CALL base._Ready() at the start if
	// you override this.
	// Ideally, don't override this
	public override void _Ready()
	{
		projectileTimer = GetNode<Timer>("LifespanTimer");
		sprite = GetNode<Sprite2D>("Sprite2D");

		projectileTimer.WaitTime = data.lifeSpan;
		projectileTimer.Timeout += Kill;

		hitbox = GetNode<Hitbox>("Hitbox");
		//parent = GetParent().GetParent<RangedWeapon>();

		hitbox.AreaEntered += HitEntity;

		spawn = GlobalPosition;
		//GD.Print($"Projectile spawned at: {spawn}");

		projectileTimer.Start();
	}

	// DO NOT OVERRIDE
	public override void _PhysicsProcess(double delta)
	{
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
	public virtual void HitEntity(Area2D area)
	{
		if (area.GetParent().IsInGroup("enemy"))
        {
            QueueFree();
        }
    }

	// If you override this, remember to
	// call base.Kill() at the end of your
	// override.
	public virtual void Kill()
	{
		//GD.Print($"Projectile has been destroyed with " +
		//$"{Math.Round(projectileTimer.TimeLeft, 2)} seconds left.");
		QueueFree();
	}
}
