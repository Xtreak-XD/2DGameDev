using Godot;
using System;

[GlobalClass]
public partial class InventoryItem : Resource
{
	[Export]
	public string ItemName = "New Item";
	[Export]
	public Texture2D Icon;
	[Export]
	public string Description = "Item Description";
}
