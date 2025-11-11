using Godot;
using System;
using System.Collections.Generic;

/**
This class manages the Inventory UI in the game.
It handles opening and closing the inventory, as well as updating
the display of items within the inventory slots.
*/
public partial class InventoryUI : Control
{
	public Boolean isOpen = false;
	public Inventory inv; // The Inventory resource that holds the player's items
	public List<ItemUISlot> slots; // Holds each individual slot in the inventory UI
	private ItemUISlot draggedItemSlot; // The slot currently being dragged

	public override void _Ready()
	{
		close();
		inv = GetNode<Inventory>("/root/Inventory"); // Loads the player's inventory resource
		slots = new List<ItemUISlot>(); // Initializes the list to hold inventory slots
		GridContainer inventoryGrid = GetNode<GridContainer>("NinePatchRect/InventoryGrid"); // Gets the GridContainer node that holds the inventory slots

		foreach (Node slot in inventoryGrid.GetChildren()) // Loops through each child node in the GridContainer
		{
			if (slot is ItemUISlot s)
			{
				slots.Add(s); // Adds each slot of the inventory to the slots list
			}
		}

		GridContainer hotBarGrid = GetNode<GridContainer>("NinePatchRect/HotBarGrid"); // Gets the GridContainer node that holds the inventory slots

		foreach (Node slot in hotBarGrid.GetChildren()) // Loops through each child node in the GridContainer
		{
			if (slot is ItemUISlot s)
			{
				slots.Add(s); // Adds each slot of the inventory to the slots list
			}
		}

		for (int i = 0; i < slots.Count; i++)
		{
			slots[i].slotIndex = i; // Assign index to each slot
		}

		Eventbus eventbus = GetNode<Eventbus>("/root/Eventbus");
		eventbus.inventoryUpdated += updateSlots; // Subscribes to the inventoryChanged signal to update the UI when the inventory changes

		updateSlots();
	}

	public override void _Process(double delta)
	{
		if (Input.IsActionJustPressed("I"))
		{
			if (isOpen)
			{
				close();
			}
			else
			{
				open();
			}
		}
	}

	public override void _Input(InputEvent @event)
	{
		if (draggedItemSlot != null && @event is InputEventMouseButton mouseEvent && mouseEvent.ButtonIndex == MouseButton.Left && mouseEvent.Pressed == false)
		{
			if (GetGlobalRect().HasPoint(GetGlobalMousePosition()))
			{
				draggedItemSlot = null;
			}
			else
			{
				// Dropped outside inventory UI
				InventoryItem draggedItem = inv.RemoveItem(draggedItemSlot.slotIndex);
				if (draggedItem != null)
				{
					spawnItemInWorld(draggedItem, 1); // Spawns the item in the game world
				}
				draggedItemSlot = null;
			}
		}
	}
	
	public void spawnItemInWorld(InventoryItem item, int quantity)
	{
		PackedScene itemScene = GD.Load<PackedScene>("res://Scenes/ui/inventory/Items.tscn");
		Items itemInstance = itemScene.Instantiate() as Items;
		itemInstance.itemResource = item;
		itemInstance.GlobalPosition = GetTree().GetCurrentScene().GetNode<Node2D>("Player").GlobalPosition; // Spawns the item above the player
		GetTree().GetCurrentScene().AddChild(itemInstance);
    }

	public void updateSlots()
	{
		for (int i = 0; i < Math.Min(inv.slots.Count, slots.Count); i++) // Loops through each slot and updates its display based on the inventory items							
		{
			slots[i].updateDisplay(inv.slots[i]); // Change the specific slot to display the corresponding item from the inventory
		}
	}

	public void checkDraggedItem(ItemUISlot slot)
	{
		if (slot == null)
		{
			return;
		}

		draggedItemSlot = slot;
	}

	public void open()
	{
		Visible = true; // Built int function to display Nodes
		isOpen = true;
	}

	public void close()
	{
		Visible = false;
		isOpen = false;
	}
}
