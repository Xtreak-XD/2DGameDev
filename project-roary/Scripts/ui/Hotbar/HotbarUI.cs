using Godot;
using System;
using System.Collections.Generic;

public partial class HotbarUI : Control
{
	private Inventory inv;
	private Eventbus eventbus;
	private List<HotbarUISlot> hotBarSlots { get; set; }

	public override void _Ready()
	{
		eventbus = GetNode<Eventbus>("/root/Eventbus");
		inv = GetNode<Inventory>("/root/Inventory");
		HBoxContainer container = GetNode<HBoxContainer>("HBoxContainer");
		hotBarSlots = new List<HotbarUISlot>();

		foreach (Node slot in container.GetChildren())
		{
			if (slot is HotbarUISlot t)
			{
				hotBarSlots.Add(t); // Adds each slot of the inventory to the slots list
			}
		}

		for (int i = 0; i < Inventory.HOTBAR_SIZE; i++)
		{
			hotBarSlots[i].slotIndex = i;
		}

		eventbus.inventoryUpdated += UpdateHotBar;
	}

    public override void _ExitTree()
    {
        eventbus.inventoryUpdated -= UpdateHotBar;
    }


	public override void _Input(InputEvent @event)
	{
		int selectedSlot = -1;
		if (@event is InputEventKey keyPress && keyPress.Pressed)
		{
			if (Input.IsActionJustPressed("hotbar_1"))
			{
				hotBarSlots[0].ButtonPressed = true;
				selectedSlot = 0;
			}
			else if (Input.IsActionJustPressed("hotbar_2"))
			{
				hotBarSlots[1].ButtonPressed = true;
				selectedSlot = 1;
			}
			else if (Input.IsActionJustPressed("hotbar_3"))
			{
				hotBarSlots[2].ButtonPressed = true;
				selectedSlot = 2;
			}
			else if (Input.IsActionJustPressed("hotbar_4"))
			{
				hotBarSlots[3].ButtonPressed = true;
				selectedSlot = 3;
			}
			if (selectedSlot >= 0)
            {
                eventbus.EmitSignal(Eventbus.SignalName.itemEquipped, selectedSlot);
            }	
		}
	}
	public void UpdateHotBar()
	{
		for (int i = 0; i < Inventory.HOTBAR_SIZE; i++)
		{
			hotBarSlots[i].UpdateDisplay(inv.slots[i]);
		}
	}
}
