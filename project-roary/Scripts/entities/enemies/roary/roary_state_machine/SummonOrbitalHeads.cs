using Godot;
using System;

public partial class SummonOrbitalHeads : RoaryState
{
    public Marker2D projectileSource;
	public GoToArenaCenter GoToCenter;
	public MoveTowardPlayer MoveTowardPlayer;

	public Timer attackTimer;
	bool attackOver = false;

	public override void _Ready()
    {
		GoToCenter = GetParent().GetNode<GoToArenaCenter>("GoToArenaCenter");
		MoveTowardPlayer = GetParent().GetNode<MoveTowardPlayer>("MoveTowardPlayer");
		
		projectileSource = GetParent().GetParent().GetNode<Marker2D>("ProjectileSource");

		attackTimer = GetParent().GetNode<Timer>("OrbitalHeadTimer");
		attackTimer.Timeout += SetAttackOver;
	}

	public override void EnterState()
    {
        GD.Print("Roary has summoned his orbital heads");
		attackTimer.Start();
		attackOver = false;

		for(int i = 0; i <= 360; i += 90)
        {
            RoaryOrbitalHead orbitalHeadProjectile = (RoaryOrbitalHead)ActiveEnemy.orbitalHead.Instantiate();
			ActiveEnemy.Owner.AddChild(orbitalHeadProjectile);

			orbitalHeadProjectile.GlobalPosition = projectileSource.GlobalPosition;
			orbitalHeadProjectile.angle = i;

			orbitalHeadProjectile.data.Damage = (int)(ActiveEnemy.data.Damage 
			* ActiveEnemy.StatMultipler());
			orbitalHeadProjectile.data.knockback = ActiveEnemy.data.knockBackAmount;
			orbitalHeadProjectile.target = ActiveEnemy.target;
			orbitalHeadProjectile.parent = ActiveEnemy;
        }
    }

	public override RoaryState Process(double delta)
    {
		if(attackOver)
        {
            return InBetweenAttack();
        }

		Vector2 targetPos = ActiveEnemy.target.GlobalPosition;
		Vector2 currentPos = ActiveEnemy.GlobalPosition;
		Vector2 targetVel = ActiveEnemy.target.Velocity;

		Vector2 velocity = currentPos.Lerp(targetPos + targetVel, 200).Normalized();

		//ActiveEnemy.animation(direction); COMMENTED OUT BECAUSE WE DO NOT HAVE ANIMATIONS
		ActiveEnemy.Velocity = velocity * ActiveEnemy.TrueSpeed() * 
		 (ActiveEnemy.TrueAcceleration() * (float) delta);
		
		ActiveEnemy.MoveAndSlide();
		
		return null;
    }

	public void SetAttackOver()
    {
        attackOver = true;
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
