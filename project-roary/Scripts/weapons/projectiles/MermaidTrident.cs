using Godot;

public partial class MermaidTrident : EnemyProjectile
{
    public override void Travel(double delta)
    {
		if(!IsQueuedForDeletion())
        {
            if(parent is Mermaid mermaid)
        	{
				mermaid.HasTrident = false;

				if(projectileTimer.TimeLeft <= projectileTimer.WaitTime / 2)
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
				else
				{
					if(mermaid.target != null) 
					{
						Vector2 currentPos = GlobalPosition;
						Vector2 playerPos = mermaid.target.GlobalPosition;

						Vector2 direction = (playerPos - currentPos).Normalized();
						Velocity = direction * data.speed;

						sprite.LookAt(playerPos);
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
            mermaid.recoverTridentTimer.Start();
        }
    }

	public override void Kill()
    {
		if(parent is Mermaid mermaid)
        {
			GD.Print("Mermaid's trident has returned to her.");
			mermaid.SetHasTrident();
        }
		
        base.Kill();
    }
}
