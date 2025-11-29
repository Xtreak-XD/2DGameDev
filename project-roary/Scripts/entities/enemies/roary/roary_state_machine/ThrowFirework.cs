using Godot;
using System;

public partial class ThrowFirework : RoaryState
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
		GD.Print("Roary is throwing a firework at the player");

		if(ActiveEnemy.target != null)
        {
			Vector2 currentPos = projectileSource.GlobalPosition;
			Vector2 targetPos = ActiveEnemy.target.GlobalPosition;

			Vector2 direction = (targetPos - currentPos).Normalized();

			RoaryFirework fireworkProjectile = (RoaryFirework)ActiveEnemy.firework.Instantiate();
			ActiveEnemy.Owner.AddChild(fireworkProjectile);

			fireworkProjectile.GlobalPosition = currentPos;
			fireworkProjectile.sprite.LookAt(targetPos);
			fireworkProjectile.parent = ActiveEnemy;
			fireworkProjectile.target = ActiveEnemy.target;

			fireworkProjectile.data.Damage = (int)(ActiveEnemy.data.Damage 
			* ActiveEnemy.StatMultipler());
			fireworkProjectile.data.knockback = ActiveEnemy.data.knockBackAmount;

			float finalSpeed = fireworkProjectile.data.speed *
			ActiveEnemy.StatMultipler();

			fireworkProjectile.Velocity = direction * finalSpeed;
        }
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
