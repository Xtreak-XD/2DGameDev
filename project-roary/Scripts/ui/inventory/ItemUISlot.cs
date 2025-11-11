using Godot;
using System;

/**
Represents a single slot in the inventory UI.
It is responsible for displaying the item icon and handling
the visual representation of the item in that slot.
*/
public partial class ItemUISlot : Panel
{
	public TextureRect icon; // The TextureRect node that displays the item's icon
	public Label quantityLabel; // The Label node that displays the quantity of the item
	public int slotIndex; // The index of this slot in the inventory
	private Inventory inv;
	private InventoryUI inventoryUI;

	public override void _Ready()
	{
		icon = GetNode<TextureRect>("ItemDisplay"); //Gets the ItemDisplay Sprite Node from InventoryUISlot
		quantityLabel = GetNode<Label>("ItemDisplay/QuantityLabel"); //Gets the QuantityLabel Node from InventoryUISlot
		inv = GetNode<Inventory>("/root/Inventory");
		inventoryUI = this.Owner as InventoryUI;
	}

/**
	This function is called when the user starts dragging an item from this slot.
	@param position The position where the drag started.
*/
	public override Variant _GetDragData(Vector2 position)
	{
		if (inv.slots[slotIndex] == null)
		{
			return default;
		}
		TextureRect dragPreview = icon.Duplicate() as TextureRect; // Create a duplicate of the icon for the drag preview

		Control c = new Control();
		c.AddChild(dragPreview);
		dragPreview.Scale = new Vector2(0.75f, 0.75f); // Scale down the preview for better visibility during drag
		dragPreview.Position -= new Vector2(25, 25); // Center the preview under the cursor
		c.Modulate = new Color(c.Modulate, .5f); // Make the preview slightly transparent

		SetDragPreview(c); // Set the drag preview to the duplicated icon
		
		inventoryUI.checkDraggedItem(this);

		return this;
	}

	/**
	Determines if the dropped data can be accepted by this slot.
	@param position The position where the data is dropped.
	@param data The data being dropped.
	*/
	public override bool _CanDropData(Vector2 position, Variant data)
	{
		return true;
	}

/**
	This function is called when an item is dropped onto this slot.
	@param position The position where the item is dropped.
	@param data The data being dropped.
*/
	public override void _DropData(Vector2 position, Variant data)
	{
		ItemUISlot itemSlot = data.As<ItemUISlot>(); // Cast the dropped data back to ItemUISlot

		if (itemSlot == null) return;
		Input.SetMouseMode(Input.MouseModeEnum.Visible);
		DisplayServer.CursorSetShape(DisplayServer.CursorShape.Drag);
		Inventory inv = GetNode<Inventory>("/root/Inventory");
		inv.SwapSlots(itemSlot.slotIndex, this.slotIndex);  // Swap the items in the inventory based on the slot indices
	}

	/**
	Updates the display of the item in the slot.
	@param item The InventoryItem resource to display in this slot.
	*/
	public void updateDisplay(InventorySlot slot)
	{
		if (slot.item == null)
		{
			icon.Visible = false;
			quantityLabel.Visible = false;
		}
		else
		{
			icon.Visible = true;
			icon.Texture = slot.item.texture; // Updates texture of the slot to the texture of the item
			if (slot.quantity > 1)
			{
				quantityLabel.Visible = true;
			}
			quantityLabel.Text = slot.quantity.ToString(); // Updates the quantity label to show the
		}
	}

}
