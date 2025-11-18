using Godot;

public partial class RoaryRoarIndicator : EnemyProjectile
{
    public override void Travel(double delta)
    {   
        Scale += new Vector2(25f * (float) delta, 25f * (float) delta);
    }

    public override void HitEntity(Area2D area)
    {
        if (area.GetParent().IsInGroup("player"))
        {
            GD.Print("Player has been hit by Roary's roar");
        }
    }
}