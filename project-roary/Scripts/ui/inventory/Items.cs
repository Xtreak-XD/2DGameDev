using Godot;
using System;

/**
Base class for  all pickable items in the game.
Handles the interaction logic and adding items to the player's inventory.
*/
public partial class Items : Sprite2D
{
	[Export] public int itemID = 0;
	[Export] public int itemQuantity = 1;
	[Export] public InventoryItem itemResource;
	private interactionArea interactionArea;


	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		interactionArea = GetNode<interactionArea>("InteractionArea");

		if (interactionArea == null)
		{
			GD.PrintErr("InteractionArea node not found!");
			return;
		}
		if (itemResource == null)
		{
			GD.PrintErr("Item resource not assigned!");
			return;
		}

		interactionArea.actionName = "pick up " + itemResource.itemName;
		interactionArea.interact = new Callable(this, "pickUpItem");

		if (itemResource.texture != null)
		{
			Texture = itemResource.texture;
		}
	}

	public void pickUpItem()
	{
		Player player = GetTree().GetFirstNodeInGroup("player") as Player;
		if (player == null)
		{
			GD.PrintErr("Player node not found!");
			return;
		}
		
		player.inventory.addItem(itemResource, itemQuantity);
		QueueFree(); // Remove the item from the scene after picking it up
		
	}
}
