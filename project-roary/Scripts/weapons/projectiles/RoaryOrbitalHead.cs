using Godot;
using System;

public partial class RoaryOrbitalHead : EnemyProjectile
{
	public float angle;
	public Player target;
    bool homing = false;

    public override void _Ready()
    {
        base._Ready();
		angle = 0;
    }

    public override void Travel(double delta)
    {   
        if(!homing)
        {
            angle += 5;

            if(angle >= 360)
            {
                angle = 0;
            }
            

            Vector2 posOffset = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle))
             .Normalized() * 300;
            GlobalPosition = parent.GlobalPosition + posOffset;

            sprite.LookAt(parent.GlobalPosition);
        }
		
		Vector2 currentPos = GlobalPosition;
		Vector2 targetPos = target.GlobalPosition;

		if(currentPos.DistanceTo(targetPos) <= 500 && !homing)
        {
            homing = true;
        }

        if(homing)
        {
            Velocity = (targetPos - currentPos).Normalized() * data.speed;
            sprite.LookAt(targetPos);
        }

		base.Travel(delta);
    }
}
