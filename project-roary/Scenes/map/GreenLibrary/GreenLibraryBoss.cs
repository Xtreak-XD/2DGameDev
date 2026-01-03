using Godot;
using System;

public partial class GreenLibraryBoss : Node2D
{
	// Called when the node enters the scene tree for the first time.
    Eventbus eventbus;
    CanvasLayer bossHealth;
	public override void _Ready()
    {
        bossHealth = GetNode<CanvasLayer>("BossHealths");
        var dayNight = GetNode<DayNightCycle>("/root/DayNightCycle");
        dayNight.Visible = false;
        eventbus = GetNode<Eventbus>("/root/Eventbus");
        eventbus.DefeatedMermaid += defeatedBoss;
    }

    async void defeatedBoss()
    {
        bossHealth.Visible = false;
        await ToSignal(GetTree().CreateTimer(1.5f), SceneTreeTimer.SignalName.Timeout);
        eventbus.EmitSignal(Eventbus.SignalName.load);
    }

    public override void _ExitTree()
    {
        var dayNight = GetNode<DayNightCycle>("/root/DayNightCycle");
        dayNight.Visible = true;
        if (eventbus != null)
            eventbus.DefeatedMermaid -= defeatedBoss;
    }
}
