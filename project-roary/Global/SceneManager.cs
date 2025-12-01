using System;
using Godot;


public partial class SceneManager : Node
{
    Node currentScene;
    Player player;
    public string comingFromName;
    Marker2D spawnPosition;
    PackedScene newPlayerInstance;
    Vector2 loadSpawnPosition = Vector2.Zero;
    private string[] scenesWithoutPlayer = {
        "MainMenu",
        "ParkingGarage",
        "Setting Menu",
    };

    public override void _Ready()
    {
        newPlayerInstance = GD.Load<PackedScene>("res://Scenes/entities/player/player.tscn");
        var root = GetTree().Root;
        currentScene = root.GetChild(root.GetChildCount() - 1);
        
        if (!ShouldSceneHavePlayer(currentScene))
        {
            GD.Print($"Scene {currentScene.Name} should not have a player");
            return;
        }

        if (currentScene.GetNodeOrNull<Player>("Player") == null)
        {
            Player newInstance = (Player)newPlayerInstance.Instantiate();
            currentScene.AddChild(newInstance);
            newInstance.CallDeferred("setSpawnPosition",extractCorrectSpawnpoint(currentScene,"Nothing"));
            player = newInstance;
        }
    }

    private bool ShouldSceneHavePlayer(Node scene)
    {
        foreach (string sceneName in scenesWithoutPlayer)
        {
            if (scene.Name.ToString().Equals(sceneName, StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }
        }
        return true;
    }

    public void goToScene(Node from, string scene, bool loading = false, Vector2 spawnPos = default)
    {
        comingFromName = currentScene.Name;
        
        Player existingPlayer = from.GetNodeOrNull<Player>("Player");
        if (existingPlayer != null)
        {
            player = existingPlayer; // saving player
            player.GetParent().RemoveChild(player);
        }

        if (loading)
        {
            loadSpawnPosition = spawnPos;
        }
        
        CallDeferred("_deferred_scene_switch", scene, loading);
        
    }

    public void _deferred_scene_switch(string path, bool loading)
    {
        PackedScene packedScene = GD.Load<PackedScene>(path);
        Node newScene = packedScene.Instantiate();

        currentScene.Free();

        GetTree().Root.AddChild(newScene);
        GetTree().CurrentScene = newScene;

        if (!ShouldSceneHavePlayer(newScene))
        {
            GD.Print($"Scene {newScene.Name} should not have a player");
            currentScene = newScene;
            return;
        }

        if (player == null)
        {
            Player newInstance = (Player)newPlayerInstance.Instantiate();
            player = newInstance;
        }

        Vector2 spawn;
        if (!loading)
        {
            spawn = extractCorrectSpawnpoint(newScene, comingFromName);
        }
        else
        {
            spawn = loadSpawnPosition;
            loadSpawnPosition = Vector2.Zero;
        }
        
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

        if (Spawns == null)
        {
            GD.PrintErr($"No PlayerSpawnPoints found in {sceneToSpawnIn.Name}");
            return Vector2.Zero;
        }

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