using System;
using Godot;


public partial class SceneManager : Node
{
    Node currentScene;
    Player player;
    string comingFromName;
    Marker2D spawnPosition;

    PackedScene newPlayerInstance;
    public override void _Ready()
    {
        newPlayerInstance = GD.Load<PackedScene>("res://Scenes/entities/player/player.tscn");
        Player newInstance = (Player)newPlayerInstance.Instantiate();
        var root = GetTree().Root;
        currentScene = root.GetChild(root.GetChildCount() - 1);

        if (currentScene.GetNodeOrNull<Player>("Player") == null)
        {
            currentScene.AddChild(newInstance);
            newInstance.CallDeferred("setSpawnPosition",extractCorrectSpawnpoint(currentScene,"Nothing"));

            player = newInstance;
        }
    }

    public void goToScene(Node from, string scene)
    {
        comingFromName = currentScene.Name;
        player = from.GetNode<Player>("Player");//saving player
        player.GetParent().RemoveChild(player);

        CallDeferred("_deferred_scene_switch", scene);
    }

    public void _deferred_scene_switch(string path)
    {
        PackedScene packedScene = GD.Load<PackedScene>(path);
        Node newScene = packedScene.Instantiate();

        currentScene.Free();

        GetTree().Root.AddChild(newScene);
        GetTree().CurrentScene = newScene;

        Vector2 spawn = extractCorrectSpawnpoint(newScene, comingFromName);
        
        
        newScene.AddChild(player);
        player.CallDeferred(nameof(Player.setSpawnPosition), spawn);

        currentScene = newScene;
    }

    public Vector2 extractCorrectSpawnpoint(Node sceneToSpawnIn, string comingFromName)
    {
        string spawnPointName = "";
        if (string.IsNullOrEmpty(comingFromName))
        {
            spawnPointName = "fromNothing";
        }
        else
        {
            spawnPointName = "from" + comingFromName;
        }

        PlayerSpawn Spawns = sceneToSpawnIn.GetNode<PlayerSpawn>("PlayerSpawnPoints");

        foreach(Marker2D i in Spawns.spawnsAvailable)
        {
            if (i.Name.Equals(spawnPointName))
            {
                return i.GlobalPosition;
            }
        }
        return Vector2.Zero;
    }

}