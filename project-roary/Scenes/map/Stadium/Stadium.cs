using Godot;
using System;

public partial class Stadium : Node2D
{
    Eventbus eventbus;
    SceneManager sceneManager;
    public override void _Ready()
    {
        eventbus = GetNode<Eventbus>("/root/Eventbus");
        sceneManager = GetNode<SceneManager>("/root/SceneManager");

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

    void beatGame()
    {
        sceneManager.goToScene(this, "res://Scenes/ui/menus/main_menu.tscn");
    }
}
