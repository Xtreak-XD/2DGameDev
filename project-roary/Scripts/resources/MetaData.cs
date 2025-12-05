using Godot;
using System;
using Godot.Collections;


[GlobalClass]
public partial class MetaData : Resource
{
    //player main metaData
    [Export] public int Money { get; set; }
    [Export] public int Ammo { get; set;}
    [Export] public Vector2 savePos { get; set; }

    [Export] public string curScenepath { get; set; }
    [Export] public Array<InventorySlot> savedInventory { get; set; }

    public void SetCurScenePath(string path)
    {
        curScenepath = path;
    }
    public void SetSavePos(Vector2 pos)
    {
        savePos = pos;
    }

    public void updateInventory(Inventory x)
    {
        savedInventory = new Array<InventorySlot>();

        foreach (var slot in x.slots)
        {
            var newSlot = new InventorySlot();
            newSlot.item = slot.item;
            newSlot.quantity = slot.quantity;
            savedInventory.Add(newSlot);
        }
    }

    public void updateMoney(int amount)
    {
        Money += amount;
    }
    public void updaateAmmo(int amount)
    {
        Ammo+= amount;
    }

    //dialogue flags pls make these booleans (pls add as needed)
    [Export] public bool TalkedToWiseTurtleAboutBrother { get; set; } = false;
    [Export] public bool SavedYoungerTurtleBrother { get; set; } = false;
    [Export] public bool DefeatedMermaid { get; set; } = false;

    //event flags
    [Export] public bool playerBeatPG { get; set; } = false;
    [Export] public bool justLeftPG { get; set; } = false;
    [Export] public bool CanEnterStadium { get; set; } = false;
    [Export] public bool openingWorldPlayed { get; set; } = false;
    [Export] public bool introPlayed { get; set; } = false;
}
