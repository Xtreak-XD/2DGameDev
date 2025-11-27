using Godot;

public partial class MermaidShield : EnemyProjectile
{
    public override void Travel(double delta)
    {	
		if(!IsQueuedForDeletion())
        {
			if(parent is Mermaid mermaid)
			{
				mermaid.Shielded = false;
			
				if(projectileTimer.TimeLeft <= (projectileTimer.WaitTime / 2) + 1)
				{
					Vector2 currentPos = GlobalPosition;
					Vector2 parentPos = parent.GlobalPosition;

					Vector2 direction = (parentPos - currentPos).Normalized();

					Velocity = direction * data.speed;

					if(currentPos.DistanceTo(parentPos) <= 80)
					{
						Kill();
						return;
					}
				}
			}
		}

        base.Travel(delta);
    }

    public override void HitEntity(Area2D area)
    {
        base.HitEntity(area);

		if(parent is Mermaid mermaid)
        {
			mermaid.recoverShieldTimer.Start();
        }
    }

	
    public override void Kill()
    {
		if(parent is Mermaid mermaid)
        {
			GD.Print("Mermaid's shield has returned to her.");
			mermaid.SetHasShield();
        }

        base.Kill();
    }
}
