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
}
