using System;
using Godot;


public partial class SceneManager : Node
{
    Node currentScene;
    public Eventbus eventbus;
    Player player;
    public string comingFromName;
    Marker2D spawnPosition;
    PackedScene newPlayerInstance;
    Vector2 loadSpawnPosition = Vector2.Zero;
    private string[] scenesWithoutPlayer = {
        "MainMenu",
        "ParkingGarage",
        "Setting Menu",
        "DiedScreen",
    };

    public override void _Ready()
    {
        eventbus = GetNode<Eventbus>("/root/Eventbus");
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

        eventbus = GetNode<Eventbus>("/root/Eventbus");
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
        comingFromName = ShouldSceneHavePlayer(currentScene) ? currentScene.Name : "";
        if(from.Name == "ParkingGarage"){ comingFromName = "ParkingGarage";} //look at these 2 lines later
        
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

        if (newScene.Name == "MainMenu")
        {
            player = null;
        }

        currentScene.Free();

        GetTree().Root.AddChild(newScene);
        GetTree().CurrentScene = newScene;

        eventbus.EmitSignal(Eventbus.SignalName.sceneChanged, newScene.Name);

        if (!ShouldSceneHavePlayer(newScene))
        {
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
        if (loading)
        {
            player.data.Health = player.data.MaxHealth;
            player.data.Stamina = player.data.MaxStamina;
            eventbus.EmitSignal(Eventbus.SignalName.updateHealth, player.data.Health);
            eventbus.EmitSignal(Eventbus.SignalName.updateStamina, player.data.Stamina);
        }
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

        GD.Print("Spawning in " + $"{spawnPointName}");

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
                GD.Print("" + i.Name);
                return i.GlobalPosition;
            }
        }

        GD.Print("No matching spawn point found, defaulting to (0,0)");
        return Vector2.Zero;
    }

}