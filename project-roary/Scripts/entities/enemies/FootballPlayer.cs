using Godot;
using System;

public partial class FootballPlayer : CharacterBody2D
{
    private Sprite2D footballSprite;


    public override void _Ready()
    {
        footballSprite = GetNode<Sprite2D>("Sprite2D");
        float footballSpriteWidth = footballSprite.Texture.GetWidth();
        float footballSpriteHeight = footballSprite.Texture.GetHeight();
    }
}
