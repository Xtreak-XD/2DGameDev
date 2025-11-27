using System;
using Godot;

public partial class MermaidThrow : MermaidState
{
	public MermaidSummon MermaidSummon;
	public MermaidChase MermaidChase;
	public MermaidDash MermaidDash;

    public override void _Ready()
    {
		MermaidSummon = GetParent().GetNode<MermaidSummon>("MermaidSummon");
		MermaidChase = GetParent().GetNode<MermaidChase>("MermaidChase");
		MermaidDash = GetParent().GetNode<MermaidDash>("MermaidDash");
    }

    public override void EnterState()
    {
		GD.Print("The mermaid is considering throwing a projectile");

		if(ActiveEnemy.target != null)
        {
            Vector2 currentPos = ActiveEnemy.projectileSource.GlobalPosition;
			Vector2 targetPos = ActiveEnemy.target.GlobalPosition;

			Vector2 direction = (targetPos - currentPos).Normalized();

			if(ActiveEnemy.HasTrident)
			{
				if(new Random().Next(2) == 1)
				{
					GD.Print("The mermaid is throwing its trident");

					MermaidTrident tridentProjectile = (MermaidTrident)ActiveEnemy.trident.Instantiate();
					ActiveEnemy.Owner.AddChild(tridentProjectile);

					tridentProjectile.GlobalPosition = ActiveEnemy.projectileSource.GlobalPosition;
					tridentProjectile.sprite.LookAt(targetPos);
					tridentProjectile.parent = ActiveEnemy;

					tridentProjectile.Velocity = direction * tridentProjectile.data.speed;
				}
			} 
			else if(ActiveEnemy.Shielded)
			{
				if(new Random().Next(2) == 1)
				{
					GD.Print("The mermaid is throwing its shield");

					MermaidShield shieldProjectile = (MermaidShield)ActiveEnemy.shield.Instantiate();
					ActiveEnemy.Owner.AddChild(shieldProjectile);

					shieldProjectile.GlobalPosition = ActiveEnemy.projectileSource.GlobalPosition;
					shieldProjectile.sprite.LookAt(targetPos);
					shieldProjectile.parent = ActiveEnemy;

					shieldProjectile.Velocity = direction * shieldProjectile.data.speed;
				}
			}
    	}
	}

    public override MermaidState Process(double delta)
    {
		if(ActiveEnemy.HasTrident)
        {
            return MermaidSummon;
        }

        return MermaidDash;
    }
}
