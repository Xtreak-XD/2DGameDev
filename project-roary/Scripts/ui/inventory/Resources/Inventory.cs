using Godot;
using System;
using Godot.Collections;

/**
This resource represents the player's inventory.
It holds an array of InventoryItem resources that the player possesses.
*/
[GlobalClass]
public partial class Inventory : Resource
{

    [Export]
    public Array<InventoryItem> items {get; set;}

}
