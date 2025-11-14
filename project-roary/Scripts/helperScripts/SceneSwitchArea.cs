using Godot;
using System;

public partial class SceneSwitchArea : Area2D
{

    public SceneManager sceneManager;

    [Export] public PackedScene path_to_scene {get; set;}


    public override void _Ready()
    {
        sceneManager = GetNode<SceneManager>("/root/SceneManager");
        BodyEntered += _onEntered;
    }

    
    public void _onEntered(Node2D body)
    {
        if (body is Player)
        {
            sceneManager.goToScene(GetParent(),path_to_scene.ResourcePath);
        }
    }

    public override void _ExitTree()
    {
        BodyEntered -= _onEntered;
    }

}
