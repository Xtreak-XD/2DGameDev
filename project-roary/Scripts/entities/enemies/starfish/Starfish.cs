using Godot;
using System;

public partial class Starfish : Node
{

	{Export} public StarfishData 
	public float bubble

	// Detect player
	public override void _Ready()
    {
		
    }


	public override void _Process(double delta)
	{
	}

	// Aim projectile
	private void Bubble()
    {
        GetTree().Root.GetChild(0).AddChild(bubble);
    }
}
