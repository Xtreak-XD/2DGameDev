using Godot;
using System;

public partial class CoffeeWaveAttack : Node
{
    [Export] public PackedScene CoffeeShotScene;

    public void Fire(Node2D owner)
    {
        if (CoffeeShotScene == null)
        {
            GD.PrintErr("CoffeeShotScene is NULL!");
            return;
        }

        Vector2[] dirs =
        {
            Vector2.Right,
            Vector2.Left,
            Vector2.Up,
            Vector2.Down
        };

        foreach (var dir in dirs)
        {
            var shot = CoffeeShotScene.Instantiate<Node2D>();
            owner.GetParent().AddChild(shot);

            shot.GlobalPosition = owner.GlobalPosition;

            if (shot.HasMethod("SetDirection"))
                shot.Call("SetDirection", dir);
        }
    }
}