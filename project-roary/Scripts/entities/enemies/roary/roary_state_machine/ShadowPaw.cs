using Godot;
using System;

public partial class ShadowPaw : RoaryState
{
	public Marker2D projectileSource;
	public GoToArenaCenter GoToCenter;
	public MoveTowardPlayer MoveTowardPlayer;

	public override void _Ready()
    {
		GoToCenter = GetParent().GetNode<GoToArenaCenter>("GoToArenaCenter");
		MoveTowardPlayer = GetParent().GetNode<MoveTowardPlayer>("MoveTowardPlayer");
		
		projectileSource = GetParent().GetParent().GetNode<Marker2D>("ProjectileSource");
	}

	public override void EnterState()
    {
		GD.Print("Roary summoned his shadow paw at the player");

		Vector2 targetPos = ActiveEnemy.target.GlobalPosition;
        
		RoaryShadowPaw shadowPawProjectile = (RoaryShadowPaw)ActiveEnemy.shadowPaw.Instantiate();
		ActiveEnemy.Owner.AddChild(shadowPawProjectile);

		float angle = (float)new RandomNumberGenerator().RandfRange(0, (float)(2 * Math.PI));
		Vector2 offset = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)).Normalized() * 600;
		Vector2 startPosition = targetPos + offset;

		shadowPawProjectile.GlobalPosition = startPosition;
		shadowPawProjectile.sprite.LookAt(targetPos);
		shadowPawProjectile.parent = GetParent().GetParent<Roary>();

		shadowPawProjectile.data.Damage = ActiveEnemy.data.Damage;
		shadowPawProjectile.data.knockback = ActiveEnemy.data.knockBackAmount;

		float finalSpeed = shadowPawProjectile.data.speed *
		 ActiveEnemy.StatMultipler();

		Vector2 direction = (targetPos - shadowPawProjectile.GlobalPosition).Normalized();

		shadowPawProjectile.Velocity = direction * finalSpeed;
	}
	
	public override RoaryState Process(double delta)
    {
		return InBetweenAttack();	
    }

	public RoaryState InBetweenAttack()
    {
        if(ActiveEnemy.Phase == RoaryPhase.FIRST)
        {
            return MoveTowardPlayer;
        }

		if(ActiveEnemy.Phase == RoaryPhase.SECOND)
        {
			if(new Random().Next(2) == 1)
            {
                return GoToCenter;
            }
			
            return MoveTowardPlayer;
        }

        return GoToCenter;
    }
}