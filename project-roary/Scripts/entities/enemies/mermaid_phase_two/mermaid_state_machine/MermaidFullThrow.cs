using Godot;

public partial class MermaidFullThrow : MermaidState
{
	public MermaidDash MermaidDash;

	public override void _Ready()
    {
		MermaidDash = GetParent().GetNode<MermaidDash>("MermaidDash");
    }

	 public override void EnterState()
    {
		GD.Print("The mermaid is attempting to throw her trident and shield");

		if(ActiveEnemy.target != null)
        {
            Vector2 currentPos = ActiveEnemy.projectileSource.GlobalPosition;
			Vector2 targetPos = ActiveEnemy.target.GlobalPosition;

			Vector2 direction = (targetPos - currentPos).Normalized();

			if(ActiveEnemy.HasTrident)
			{
				MermaidTrident tridentProjectile = (MermaidTrident)ActiveEnemy.trident.Instantiate();
				Owner.AddChild(tridentProjectile);

				tridentProjectile.GlobalPosition = ActiveEnemy.projectileSource.GlobalPosition;
				tridentProjectile.sprite.LookAt(targetPos);
				tridentProjectile.parent = ActiveEnemy;

				tridentProjectile.Velocity = direction * tridentProjectile.data.speed;
			}

			if(ActiveEnemy.Shielded)
			{
				MermaidShield shieldProjectile = (MermaidShield)ActiveEnemy.shield.Instantiate();
				Owner.AddChild(shieldProjectile);

				shieldProjectile.GlobalPosition = ActiveEnemy.projectileSource.GlobalPosition;
				shieldProjectile.sprite.LookAt(targetPos);
				shieldProjectile.parent = ActiveEnemy;

				shieldProjectile.Velocity = direction * shieldProjectile.data.speed;
			}
        }
    }

	public override MermaidState Process(double delta)
    {
        return MermaidDash;
    }
}
