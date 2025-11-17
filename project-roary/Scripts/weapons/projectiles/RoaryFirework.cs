using System;
using Godot;

public partial class RoaryFirework : EnemyProjectile
{
	public Player target;
	public const float HOMING_RANGE = 750f;

    public override void Travel(double delta)
    {
		// Add a trail as well

		if(target != null)
        {
            Vector2 targetPos = target.GlobalPosition;
			Vector2 currentPos = GlobalPosition;

			sprite.LookAt(targetPos);

			if(currentPos.DistanceTo(targetPos) <= HOMING_RANGE)
            {
				float angle = currentPos.AngleTo(targetPos);
                Vector2 velocity = Velocity + new Vector2(Mathf.Cos(angle), MathF.Sin(angle) * data.speed);

				Velocity = velocity;
            }
        }

		base.Travel(delta);
    }

    public override void HitEntity(Area2D area)
    {
		// Add explosion here

        base.HitEntity(area);
    }
}
