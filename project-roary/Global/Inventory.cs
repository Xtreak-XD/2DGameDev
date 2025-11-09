using Godot;
using System;
using Godot.Collections;
using System.Linq;

/**
This Node is the inventory manager for the player.
It holds the list of items the player has collected
and provides methods to manipulate the inventory.
*/
public partial class Inventory : Node
{
    [Export]
    public Array<InventorySlot> slots { get; set; }
    public int size = 12; // Maximum number of slots in the inventory
    public Eventbus eventbus;

    public override void _Ready()
    {
        slots = new Array<InventorySlot>();
        // Initialize inventory with empty slots
        for (int i = 0; i < size; i++)
        {
            slots.Add(new InventorySlot());
        }
        eventbus = GetNode<Eventbus>("/root/Eventbus");
    }

    public bool AddItem(InventoryItem itemToAdd, int quantity = 1)
    {
        int remaining = quantity;

        // Fill existing stacks that aren't full
        foreach (var slot in slots.Where(s => s.item == itemToAdd && s.quantity < itemToAdd.maxStackSize)) // Only consider slots with the same item that aren't full
        {

            int spaceInSlot = itemToAdd.maxStackSize - slot.quantity; // Calculate how much space is left in this slot
            int amountToAdd = Math.Min(remaining, spaceInSlot); // Determine how much we can add to this slot
            slot.quantity += amountToAdd;
            remaining -= amountToAdd;

            if (remaining == 0) break; // Stop if we've added all items
        }

        //Create new stacks in empty slots
        foreach (var slot in slots.Where(s => s.item == null))
        {
            if (remaining == 0) break;

            int amountToAdd = Math.Min(remaining, itemToAdd.maxStackSize); // Determine how much we can add to this new slot
            slot.item = itemToAdd;
            slot.quantity = amountToAdd;
            remaining -= amountToAdd;
        }

        // Emit signal and return result
        if (remaining < quantity)
        {
            eventbus.EmitSignal(Eventbus.SignalName.inventoryUpdated);
        }

        if (remaining > 0)
        {
            GD.PrintErr($"Could only add {quantity - remaining}/{quantity} of {itemToAdd.itemName}");
            return false;
        }

        return true;
    }

    // ToDo : Implement RemoveItem
    // ToDo : Consider saving/loading inventory state 
    // ToDo : Implement item drag and drop between slots
}
