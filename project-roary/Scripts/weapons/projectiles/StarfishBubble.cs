using Godot;

public partial class StarfishBubble : EnemyProjectile
{
    public Player target;

    public override void Travel(double delta)
    {
        Vector2 currentPos = GlobalPosition;
        Vector2 playerPos = target.GlobalPosition;

        Vector2 direction = (playerPos - currentPos).Normalized();

        Velocity = direction * data.speed;

        base.Travel(delta);
    }
}
