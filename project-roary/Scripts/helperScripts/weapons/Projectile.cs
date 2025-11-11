using Godot;

public partial class Projectile : CharacterBody2D
{
	public Timer projectileTimer;
	public RangedWeapon parent;
	public int damage;
	public Vector2 spawn = Vector2.Zero;

	[Export]
	public ProjectileData data;

	// REMEMBER TO CALL base._Ready() if you override this
	public override void _Ready()
	{
		projectileTimer = GetNode<Timer>("LifespanTimer");

		if(projectileTimer == null)
        {
			GD.Print("Timer does not exist on ready.");
        }

		projectileTimer.WaitTime = data.lifeSpan;
		projectileTimer.Timeout += Kill;

		parent = GetParent<RangedWeapon>();
		damage = parent.data.damage;
	}

	// DO NOT OVERRIDE
    public override void _EnterTree()
    {
		spawn = GlobalPosition;

		if(projectileTimer == null)
        {
			GD.Print("Timer does not exist entering tree.");
        }

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

	// Controls special behavior of the projectile in flight
	// REMEMBER TO CALL base.Travel() if you override this
	public virtual void Travel(double delta)
    {
		MoveAndSlide();
    }

	// DO NOT OVERRIDE
	public void Kill()
	{
		GD.Print("Projectile is being destroyed.");
		QueueFree();
    }
}
