using Godot;

public partial class CoffeeWaveAttack : Node
{
    [Export] public PackedScene CoffeeShotScene;

    public void Fire(Logo owner)
    {
        if(CoffeeShotScene == null)
        {
            GD.PrintErr("CoffeeShotScene is NULL!");
            return;
        }

        Vector2[] dirs = [
            Vector2.Right,
            Vector2.Left,
            Vector2.Up,
            Vector2.Down
        ];

        foreach(Vector2 dir in dirs)
        {
            CoffeeShot shot = (CoffeeShot)CoffeeShotScene.Instantiate();
            owner.GetParent().CallDeferred("add_child", shot);
            shot.data.Damage = owner.data.Damage;

            shot.GlobalPosition = owner.GlobalPosition;

            shot.Velocity = dir * shot.data.speed;
        }
    }
}