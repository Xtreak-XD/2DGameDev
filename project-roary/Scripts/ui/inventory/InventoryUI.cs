using Godot;
using System;
using System.Collections.Generic;

/**
This class manages the Inventory UIin the game.
It handles opening and closing the inventory, as well as updating
the display of items within the inventory slots.
*/
public partial class InventoryUI : Control
{
	public Boolean isOpen = false;
	public Inventory inv; // The Inventory resource that holds the player's items
	public List<ItemUISlot> slots; // Holds each individual slot in the inventory UI

	public override void _Ready()
	{
		close();
		inv = GetParent<Player>().inventory; // Loads the player's inventory resource
		slots = new List<ItemUISlot>();
		GridContainer grid = GetNode<GridContainer>("NinePatchRect/GridContainer");

		foreach (Node slot in grid.GetChildren()) // Loops through each child node in the GridContainer
		{
			if (slot is ItemUISlot s)
			{
				slots.Add(s); // Adds each slot of the inventory to the slots list
			}
		}

		updateSlots();
	}

	public void updateSlots()
	{
		for (int i = 0; i < Math.Min(inv.slots.Count, slots.Count); i++) // Loops through each slot and updates its display based on the inventory items							
		{
			slots[i].updateDisplay(inv.slots[i]); // Change the specific slot to display the corresponding item from the inventory
		}
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
