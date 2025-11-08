using Godot;
using System;

/**
This class represents a single slot in the inventory UI.
It is responsible for displaying the item icon and handling
the visual representation of the item in that slot.
*/
public partial class ItemUISlot : Panel
{
	public TextureRect itemVisual;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		itemVisual = GetNode<TextureRect>("ItemDisplay"); //Gets the ItemDisplay Sprite Node from InventoryUISlot
	}

	/**
	This function updates the display of the item in the slot.
	@param item The InventoryItem resource to display in this slot.
	*/
	public void updateDisplay(InventorySlot slot)
	{
		if (slot.item == null)
		{
			itemVisual.Visible = false;
		}
		else
		{
			itemVisual.Visible = true;
			itemVisual.Texture = slot.item.texture; // Updates texture of the slot to the texture of the item
		}
	}

}
