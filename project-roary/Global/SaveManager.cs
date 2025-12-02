using Godot;
using System;

public partial class SaveManager : Node
{
    private string save_file_path = "user://LocalSave/";
    private string save_file_name = "playerMetaData.tres";
    private Eventbus eventbus;
    private Inventory inv;
    private MetaData metaData = new();

    public override void _Ready()
    {
        eventbus = GetNode<Eventbus>("/root/Eventbus");
        inv = GetNode<Inventory>("/root/Inventory");

        //metaData.Money = 5000;
        //metaData.Ammo = 500;
        //change metadata info here to test!
        //metadata.... = ...;

        eventbus.save += Save;
        eventbus.load += Load;

        VerifySaveDirectory(save_file_path);
    }

    public override void _ExitTree()
    {
        eventbus.save -= Save;
        eventbus.load -= Load;
    }

    public bool SaveFileExists()
    {
        string fullPath = save_file_path + save_file_name;
        return ResourceLoader.Exists(fullPath);
    }

    public void Save(bool firstLoad = false)
    {
        // Get player
        Player player = GetTree().Root.GetNodeOrNull<Player>("Player");
        if (player == null)
        {
            var currentScene = GetTree().CurrentScene;
            player = currentScene?.GetNodeOrNull<Player>("Player");
        }

        if (player != null)
        {
            metaData.SetSavePos(player.Position);
        }
        if (!firstLoad)
        {
            metaData.SetCurScenePath(GetTree().CurrentScene.SceneFilePath);
        }
        
        metaData.updateInventory(inv);

        VerifySaveDirectory(save_file_path);

        string fullPath = save_file_path + save_file_name;
        Error result = ResourceSaver.Save(metaData, fullPath);

        if (result == Error.Ok)
        {
            GD.Print("Game saved at " + fullPath);
        }
        else
        {
            GD.PrintErr($"Failed to save game: {result}");
        }
    }

    public void Load()
    {
        string fullPath = save_file_path + save_file_name;
        if (!ResourceLoader.Exists(fullPath))
        {
            GD.Print("No save file found at " + fullPath);
            return;
        }

        try
        {
            metaData = ResourceLoader.Load<MetaData>(fullPath, null, ResourceLoader.CacheMode.ReplaceDeep);

            if (metaData == null)
            {
                GD.PrintErr("Failed to load save from " + fullPath);
                return;
            }

            // Load inventory
            loadInventory();

            // Load money
            if (metaData.Money > 0)
            {
                eventbus.EmitSignal("updateMoney", metaData.Money);
            }
            if(metaData.Ammo > 0)
            {
                eventbus.EmitSignal("updateAmmo", metaData.Ammo);
            }

            GD.Print("Save loaded from " + fullPath);

            // Load the scene
            string sceneToLoad = metaData.curScenepath;
            if (!string.IsNullOrEmpty(sceneToLoad) && ResourceLoader.Exists(sceneToLoad))
            {
                var sceneManager = GetNode<SceneManager>("/root/SceneManager");
                sceneManager.goToScene(GetTree().CurrentScene, sceneToLoad, true, metaData.savePos);

                CallDeferred(nameof(EmitInventoryUpdate));
            }
            else
            {
                GD.PrintErr("Invalid scene path in save file");
            }
        }
        catch (Exception e)
        {
            GD.PrintErr($"Error loading save: {e.Message}");
        }
    }

    private void EmitInventoryUpdate()
    {
        GD.Print("Emitting deferred inventory update");
        eventbus.EmitSignal(Eventbus.SignalName.inventoryUpdated);
    }

    void loadInventory()
    {
        if (metaData.savedInventory == null || metaData.savedInventory.Count == 0)
        {
            GD.Print("No inventory data to load");
            return;
        }

        var loadedInventory = new Godot.Collections.Array<InventorySlot>();
        foreach (var savedSlot in metaData.savedInventory)
        {
            var newSlot = new InventorySlot();
            newSlot.item = savedSlot.item;
            newSlot.quantity = savedSlot.quantity;
            loadedInventory.Add(newSlot);
        }
        inv.slots = loadedInventory;
    }

    public void CreateNewSave(string startingScenePath)
    {
        metaData = new MetaData();
        metaData.Money = 0;
        metaData.savePos = Vector2.Zero; //imma change this to the starting location later
        metaData.SetCurScenePath(startingScenePath);

        var inv = GetNode<Inventory>("/root/Inventory");
        inv.slots = new Godot.Collections.Array<InventorySlot>();
        for (int i = 0; i < Inventory.TOTAL_SIZE; i++)
        {
            inv.slots.Add(new InventorySlot());
        }

        metaData.updateInventory(inv);

        Save(true);
    }

    private void VerifySaveDirectory(string saveFilePath)
    {
        DirAccess.MakeDirAbsolute(saveFilePath);
    }

    public MetaData GetMetaData()
    {
        return metaData;
    }
}