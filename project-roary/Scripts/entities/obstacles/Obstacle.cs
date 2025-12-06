using Godot;
using System;

public partial class Obstacle : Enemy
{
    [Export] public Texture2D car {get; set;}

    public Sprite2D texture;

    public override void _Ready()
    {
        texture = GetNode<Sprite2D>("DummyCar");
        texture.Texture = car;
    }

}
