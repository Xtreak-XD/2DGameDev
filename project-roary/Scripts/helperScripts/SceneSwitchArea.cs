using Godot;
using System;

public partial class SceneSwitchArea : Area2D
{

    public SceneManager sceneManager;

    [Export] public string path = "";

    public override void _EnterTree()
    {
        BodyEntered += _onEntered;
    }
    public override void _Ready()
    {
        sceneManager = GetNode<SceneManager>("/root/SceneManager");
    }

    
    public void _onEntered(Node2D body)
    {
        if (body is Player)
        {
            sceneManager.goToScene(GetParent(),path);
        }
    }

    public override void _ExitTree()
    {
        BodyEntered -= _onEntered;
    }

}
