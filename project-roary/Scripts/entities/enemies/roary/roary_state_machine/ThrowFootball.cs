using Godot;

public partial class ThrowFootball : RoaryState
{
	public RoaryRoam Roam;
	public Marker2D projectileSource;

	public override void _Ready()
    {
		Roam = GetParent().GetNode<RoaryRoam>("RoaryRoam");
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
		footballProjectile.parent = GetParent().GetParent<Roary>();

		footballProjectile.data.Damage = ActiveEnemy.data.Damage;
		footballProjectile.data.knockback = ActiveEnemy.data.knockBackAmount;

		float finalSpeed = footballProjectile.data.speed *
		 ActiveEnemy.StatMultipler();

		footballProjectile.Velocity = direction * finalSpeed;
	}
	
	public override RoaryState Process(double delta)
    {
        return Roam;
    }
}
