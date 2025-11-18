using Godot;

public partial class RoaryFirework : EnemyProjectile
{
	public Player target;
    public const float PROPORTIONAL = 3;
    public const float ACCEL = 3;

    public Vector2 previousTargetPos;
    public Vector2 previousLocation;

    public override void Travel(double delta)
    {
		// Add a trail

		if(target != null)
        {
            Vector2 targetPos = target.GlobalPosition;
			Vector2 currentPos = GlobalPosition;

			sprite.LookAt(targetPos);

			Vector2 currentPosDelta = targetPos - currentPos;
            Vector2 prevPosDelta = previousTargetPos - previousLocation;

            Vector2 losDelta = Vector2.Zero;
            float losRate = 0;

            currentPosDelta.Normalized();
            prevPosDelta.Normalized();

            if(prevPosDelta.Length() != 0)
            {
                losDelta = currentPosDelta - prevPosDelta;
                losRate = losDelta.Length();
            }

            float closeRate = -losRate;

            Vector2 intermediate1 = currentPosDelta * (PROPORTIONAL * closeRate * losRate);
            Vector2 intermediate2 = intermediate1 + (losDelta * PROPORTIONAL * 9.8f * 0.5f);
            
            Vector2 adjustedVel = Velocity.Normalized() * ACCEL * (float)delta;

            Vector2 finalAccel = intermediate2 + adjustedVel;

            Velocity += finalAccel;

            Velocity *= -1;
            
            if(Velocity.Length() > data.speed)
            {
                Velocity = Velocity.Normalized() * data.speed;
            }

            GD.Print($"Velocity: {Velocity}");

            previousLocation = GlobalPosition;
            previousTargetPos = targetPos;
        }

		base.Travel(delta);
    }

    public override void HitEntity(Area2D area)
    {
		// Add explosion here

        base.HitEntity(area);
    }

    public override void Kill()
    {
        // Add explosion here

        base.Kill();
    }
}
