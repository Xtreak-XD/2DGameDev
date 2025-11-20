using Godot;
using System;
using Godot.Collections;


[GlobalClass]
public partial class MetaData : Resource
{
    //player main metaData
    [Export] public int Money;
    public Vector2 savePos = Vector2.Zero;
    public static Array<InventorySlot> savedInventory { get; set; }
    public void SetSavePos(Vector2 pos)
    {
        savePos = pos;
    }

    public void updateInventory(Array<InventorySlot> x)
    {
        savedInventory = x;
    }

    //dialogue flags pls make these booleans (pls add as needed)
    public bool TalkedToWiseTurtleAboutBrother = false;
    public bool SavedYoungerTurtleBrother = false;
    public bool DefeatedMermaid = false;
    public bool CanEnterStadium = false;
}
