using Godot;
using System;

public partial class ForegroundArea : Area2D
{
	private Sprite2D foreground;
	public override void _Ready()
    {
        foreground = GetNode<Sprite2D>("%foreground");
        BodyEntered += OnBodyEntered;
        BodyExited += OnBodyExited;
    }

    public override void _ExitTree()
    {
        BodyEntered -= OnBodyEntered;
        BodyExited -= OnBodyExited;
    }

    private void OnBodyEntered(Node2D body)
    {
        if (body is Player player)
        {
            foreground.SelfModulate = new Color(1, 1, 1, 0.5f);
        }

        if (body is Roary roary)
        {
            foreground.SelfModulate = new Color(1, 1, 1, 0.5f);
        }
    }

    private void OnBodyExited(Node2D body)
    {
        if (body is Player player)
        {
            foreground.SelfModulate = new Color(1, 1, 1, 1f);
        }

        if (body is Roary roary)
        {
            foreground.SelfModulate = new Color(1, 1, 1, 1f);
        }
    }
}
