using Godot;
using System;

/**
This resource represents a single item that can be stored in the inventory.
It includes properties such as the item's name, texture, and description.
*/
[GlobalClass]
public partial class InventoryItem : Resource
{
	[Export]
	public string itemName { get; set; } = "";
	[Export]
	public Texture2D texture { get; set; }
	[Export]
	public string description { get; set; } = "";
	[Export]
	public bool isStackable { get; set; } = false;
	[Export]
	public int maxStackSize { get; set; } = 20;

// Shop Specific Resources
	[Export] public int shopPrice { get; set; } = -1;
	[Export] public bool isSellableInShop { get; set; } = false;
}
