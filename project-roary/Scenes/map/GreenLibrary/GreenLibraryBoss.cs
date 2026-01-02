using Godot;
using System;

public partial class GreenLibraryBoss : Node2D
{
	// Called when the node enters the scene tree for the first time.
    Eventbus eventbus;
	public override void _Ready()
    {
        var dayNight = GetNode<DayNightCycle>("/root/DayNightCycle");
        dayNight.Visible = false;
        eventbus = GetNode<Eventbus>("/root/Eventbus");
        eventbus.DefeatedMermaid += defeatedBoss;
    }

    async void defeatedBoss()
    {
        await ToSignal(GetTree().CreateTimer(3.0f), SceneTreeTimer.SignalName.Timeout);
        eventbus.EmitSignal("load");
    }

    public override void _ExitTree()
    {
        var dayNight = GetNode<DayNightCycle>("/root/DayNightCycle");
        dayNight.Visible = true;
        eventbus.DefeatedMermaid -= defeatedBoss;
    }
}
