using Godot;
using System;
using System.Collections.Generic;

/**
Manages the Inventory UI in the game.
It handles opening and closing the inventory, as well as updating
the display of items within the inventory slots.
*/
public partial class InventoryUI : Control
{
	public Boolean isOpen = false;
	public Inventory inv; // The Inventory resource that holds the player's items
	public List<ItemUISlot> slots; // Holds each individual slot in the inventory UI
	private ItemUISlot draggedItemSlot; // The slot currently being dragged
	private Eventbus eventbus;

	public override void _Ready()
	{
		close();
		inv = GetNode<Inventory>("/root/Inventory"); // Loads the player's inventory resource
		slots = new List<ItemUISlot>(); // Initializes the list to hold inventory slots
		GridContainer inventoryGrid = GetNode<GridContainer>("NinePatchRect/InventoryGrid"); // Gets the GridContainer node that holds the inventory slots
		eventbus = GetNode<Eventbus>("/root/Eventbus");

		GridContainer hotBarGrid = GetNode<GridContainer>("NinePatchRect/HotBarGrid"); // Gets the GridContainer node that holds the inventory slots

		foreach (Node slot in hotBarGrid.GetChildren()) // Loops through each child node in the GridContainer
		{
			if (slot is ItemUISlot s)
			{
				slots.Add(s); // Adds each slot of the inventory to the slots list
			}
		}

		foreach (Node slot in inventoryGrid.GetChildren()) // Loops through each child node in the GridContainer
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
				IndividualItem draggedItem = inv.RemoveItem(draggedItemSlot.slotIndex);
				if (draggedItem != null)
				{
					eventbus.EmitSignal(Eventbus.SignalName.itemDropped, draggedItem, 1);
				}
				draggedItemSlot = null;
			}
		}
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
