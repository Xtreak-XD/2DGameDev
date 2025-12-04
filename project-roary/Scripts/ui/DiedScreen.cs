using Godot;
using System;

public partial class DiedScreen : Control
{
    public Button restart;
    public Button mainMenu;
    public SceneManager sceneManager;
    public Eventbus eventbus;

    public override void _Ready()
    {
        eventbus = GetNode<Eventbus>("/root/Eventbus");
        sceneManager = GetNode<SceneManager>("/root/SceneManager");
        restart = GetNode<Button>("buttons/restart");
        mainMenu = GetNode<Button>("buttons/mainMenu");
        

        restart.Pressed += onRestart;
        mainMenu.Pressed += onMainMenu;
    }

    public override void _ExitTree()
    {
        restart.Pressed -= onRestart;
        mainMenu.Pressed -= onMainMenu;
    }


    void onRestart()
    {
        eventbus.EmitSignal("load");
    }

    void onMainMenu()
    {
        string path = "res://Scenes/ui/menus/main_menu.tscn";
		if (!ResourceLoader.Exists(path))
		{
			GD.PrintErr($"First scene not found: {path}");
			return;
		}
		sceneManager.goToScene(this, path, false);
    }

}
