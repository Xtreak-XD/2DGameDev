using System;
using Godot;

public partial class Projectile : CharacterBody2D
{
	public Timer projectileTimer;
	public int damage = 0;
	public Vector2 spawn = Vector2.Zero;

	[Export]
	public ProjectileData data;

	// REMEMBER TO CALL base._Ready() if you override this
	public override void _Ready()
	{
		projectileTimer = GetNode<Timer>("LifespanTimer");
		
		projectileTimer.WaitTime = data.lifeSpan;
		projectileTimer.Timeout += Kill;

		spawn = GlobalPosition;

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
		GD.Print($"Projectile has been destroyed with " +
		$"{Math.Round(projectileTimer.TimeLeft, 2)} seconds left.");
		QueueFree();
    }
}
