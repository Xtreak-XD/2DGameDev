using Godot;
using System;

public partial class GreenLibraryFloor2 : Node2D
{
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
