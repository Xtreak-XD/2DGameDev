using Godot;
using System;

public partial class GreenLibraryBoss : Node2D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
    {
        var dayNight = GetNode<DayNightCycle>("/root/DayNightCycle");
        dayNight.Visible = false;
    }

    public override void _ExitTree()
    {
        var dayNight = GetNode<DayNightCycle>("/root/DayNightCycle");
        dayNight.Visible = true;
    }
}
