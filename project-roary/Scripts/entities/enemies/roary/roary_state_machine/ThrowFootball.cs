using System;
using Godot;

public partial class ThrowFootball : RoaryState
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
		GD.Print("Roary is throwing a football at the player");

		Vector2 currentPos = projectileSource.GlobalPosition;
		Vector2 targetPos = ActiveEnemy.target.GlobalPosition;

		Vector2 direction = (targetPos - currentPos).Normalized();

		RoaryFootball footballProjectile = (RoaryFootball)ActiveEnemy.football.Instantiate();
		ActiveEnemy.Owner.AddChild(footballProjectile);

		footballProjectile.GlobalPosition = currentPos;
		footballProjectile.sprite.LookAt(targetPos);
		footballProjectile.parent = ActiveEnemy;

		footballProjectile.data.Damage = (int)(ActiveEnemy.data.Damage 
		 * ActiveEnemy.StatMultipler());
		footballProjectile.data.knockback = ActiveEnemy.data.knockBackAmount;

		float finalSpeed = footballProjectile.data.speed *
		 ActiveEnemy.StatMultipler();

		footballProjectile.Velocity = direction * finalSpeed;
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
