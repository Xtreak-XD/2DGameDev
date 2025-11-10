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
    public const int HOTBAR_SIZE = 4; // Number of slots in the hotbar
    public const int INVENTORY_SIZE = 12; // Maximum number of slots in the inventory
    public const int TOTAL_SIZE = HOTBAR_SIZE + INVENTORY_SIZE; // Maximum number of slots in the inventory

    public Eventbus eventbus;

    public override void _Ready()
    {
        eventbus = GetNode<Eventbus>("/root/Eventbus");
        slots = new Array<InventorySlot>();
        // Initialize inventory with empty slots
        for (int i = 0; i < TOTAL_SIZE; i++)
        {
            slots.Add(new InventorySlot());
        }      
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

    public void SwapSlots(int indexA, int indexB)
    {
        if (indexA < 0 || indexA >= slots.Count || indexB < 0 || indexB >= slots.Count)
        {
            GD.PrintErr("Invalid slot indices for swapping.");
            return;
        }

        InventorySlot temp = slots[indexA];
        slots[indexA] = slots[indexB];
        slots[indexB] = temp;

        eventbus.EmitSignal(Eventbus.SignalName.inventoryUpdated);
    }

    /**
    Removes a specified quantity of an item from a given slot.
    @param slotIndex The index of the slot to remove the item from.
    @param quantity The quantity of the item to remove. Default is 1.
    @return The InventoryItem that was removed, or null if removal was unsuccessful.
    */
    public InventoryItem RemoveItem(int slotIndex, int quantity = 1)
    {
        if (slotIndex < 0 || slotIndex >= slots.Count)
        {
            GD.PrintErr("Invalid slot index for removing item.");
            return null;
        }

        InventorySlot slot = slots[slotIndex]; // Get the slot at the specified index

        if (slot.item == null || slot.quantity < quantity)
        {
            GD.PrintErr("Not enough items in the slot to remove.");
            return null;
        }
        InventoryItem itemToReturn = slot.item; // Store the item to return
        slot.quantity -= quantity; // Decrease the quantity in the slot

        if (slot.quantity <= 0)
        {
            slot.item = null; // Clear the slot if quantity reaches zero
            slot.quantity = 0;
        }

        eventbus.EmitSignal(Eventbus.SignalName.inventoryUpdated);

        return itemToReturn;
    }
}
