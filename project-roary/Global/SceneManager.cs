using System;
using Godot;


public partial class SceneManager : Node
{
    Node currentScene;
    Player player;
    string comingFromName;
    Marker2D spawnPosition;

    public override void _Ready()
    {
        var root = GetTree().Root;
        currentScene = root.GetChild(root.GetChildCount() - 1);
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

        player.Position = spawn;

        newScene.AddChild(player);
        currentScene = newScene;

    }

    public Vector2 extractCorrectSpawnpoint(Node sceneToSpawnIn, string comingFromName)
    {
        string spawnPointName = "from" + comingFromName;
        PlayerSpawn Spawns = sceneToSpawnIn.GetNode<PlayerSpawn>("PlayerSpawnPoints");

        foreach(Marker2D i in Spawns.spawnsAvailable)
        {
            if (i.Name.Equals(spawnPointName))
            {
                return i.Position;
            }
        }
        return new Vector2(0,0);
    }

}