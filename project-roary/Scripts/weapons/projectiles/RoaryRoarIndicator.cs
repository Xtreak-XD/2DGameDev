public partial class RoaryRoarIndicator : EnemyProjectile
{
    public override void Travel(double delta)
    {   
        sprite.Scale *= 1.05f;
        hitbox.Scale *= 1.05f;

        base.Travel(delta);
    }
}