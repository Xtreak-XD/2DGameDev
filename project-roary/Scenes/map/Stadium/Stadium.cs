using Godot;
using System;

public partial class Stadium : Node2D
{
    Eventbus eventbus;
    SceneManager sceneManager;
    CanvasLayer bossHealth;
    public override void _Ready()
    {
        eventbus = GetNode<Eventbus>("/root/Eventbus");
        sceneManager = GetNode<SceneManager>("/root/SceneManager");
        bossHealth = GetNode<CanvasLayer>("BossHealths");
        eventbus.beatRoary += beatGame;
        var dayNight = GetNode<DayNightCycle>("/root/DayNightCycle");
        dayNight.Visible = false;
    }

    public override void _ExitTree()
    {
        var dayNight = GetNode<DayNightCycle>("/root/DayNightCycle");
        dayNight.Visible = true;
        eventbus.beatRoary -= beatGame;
    }

    async void beatGame()
    {
        GD.Print("beat game");
        bossHealth.Visible = false;
        await ToSignal(GetTree().CreateTimer(1.5f), SceneTreeTimer.SignalName.Timeout);
        sceneManager.goToScene(this, "res://Scenes/ui/menus/main_menu.tscn");
    }
}
