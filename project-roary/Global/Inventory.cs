using Godot; // For Godot engine functionalities
using System; // For basic C# functionalities
using Godot.Collections; // For Godot's Array type
using System.Linq; // For LINQ methods like Where

/**
This Node is the inventory manager for the player.
It holds the list of items the player has collected
and provides methods to manipulate the inventory.
*/
public partial class Inventory : Node
{
    [Export]
    public Array<InventorySlot> slots { get; set; } // The list of inventory slots
    public const int HOTBAR_SIZE = 4; // Number of slots in the hotbar
    public const int INVENTORY_SIZE = 12; // Maximum number of slots in the inventory
    public const int TOTAL_SIZE = HOTBAR_SIZE + INVENTORY_SIZE; // Maximum number of slots in the inventory

    public Eventbus eventbus; // Reference to the Eventbus for emitting signals

    public override void _Ready()
    {
        eventbus = GetNode<Eventbus>("/root/Eventbus"); // Get the Eventbus node
        slots = new Array<InventorySlot>(); // Initialize the slots array
        // Initialize inventory with empty slots
        for (int i = 0; i < TOTAL_SIZE; i++)
        {
            slots.Add(new InventorySlot());// Add an empty InventorySlot to the slots array
        }
    }


    /**
    Adds an item to the inventory, stacking it if possible.
    @param itemToAdd The InventoryItem to add.
    @param quantity The quantity of the item to add. Default is 1.
    @return True if the item was added successfully, false otherwise.
    */
    public bool AddItem(InventoryItem itemToAdd, int quantity = 1) // Adds an item to the inventory
    {
 
        int remaining = quantity; // Track how many items are left to add

        // Fill existing stacks that aren't full
        foreach (var slot in slots.Where(s => s.item == itemToAdd && s.quantity < itemToAdd.maxStackSize)) // Only consider slots with the same item that aren't full
        {

            int spaceInSlot = itemToAdd.maxStackSize - slot.quantity; // Calculate how much space is left in this slot
            int amountToAdd = Math.Min(remaining, spaceInSlot); // Determine how much we can add to this slot
            slot.quantity += amountToAdd; // Add the items to the slot
            remaining -= amountToAdd; // Decrease the remaining count

            if (remaining == 0) break; // Stop if we've added all items
        }

        //Create new stacks in empty slots
        foreach (var slot in slots.Where(s => s.item == null))
        {
            if (remaining == 0) break; // Stop if we've added all items

            int amountToAdd = Math.Min(remaining, itemToAdd.maxStackSize); // Determine how much we can add to this new slot
            slot.item = itemToAdd; // Assign the item to the slot
            slot.quantity = amountToAdd; // Set the quantity in the slot
            remaining -= amountToAdd; // Decrease the remaining count
        }

        // Emit signal and return result
        if (remaining < quantity)
        {
            eventbus.EmitSignal(Eventbus.SignalName.inventoryUpdated); // Notify that the inventory has been updated
        }

        if (remaining > 0) // If there are still items left to add
        {
            GD.PrintErr($"Could only add {quantity - remaining}/{quantity} of {itemToAdd.itemName}"); // Log if not all items could be added
            return false; // Not all items were added
        }

        return true; //    All items were added successfully
    }

    /**
    Swaps the items between two inventory slots.
    @param indexA The index of the first slot.
    @param indexB The index of the second slot.
    */
    public void SwapSlots(int indexA, int indexB) // Swaps the items between two slots
    {
        if (indexA < 0 || indexA >= slots.Count || indexB < 0 || indexB >= slots.Count) // Validate indices
        {
            GD.PrintErr("Invalid slot indices for swapping."); // Validate indices
            return;
        }

        InventorySlot temp = slots[indexA]; // Temporarily store one slot
        slots[indexA] = slots[indexB]; // Swap the slots
        slots[indexB] = temp; // Complete the swap

        eventbus.EmitSignal(Eventbus.SignalName.inventoryUpdated); // Notify that the inventory has been updated
    }

    /**
    Removes a specified quantity of an item from a given slot.
    @param slotIndex The index of the slot to remove the item from.
    @param quantity The quantity of the item to remove. Default is 1.
    @return The InventoryItem that was removed, or null if removal was unsuccessful.
    */
    public InventoryItem RemoveItem(int slotIndex, int quantity = 1) // Removes a specified quantity of an item from a given slot
    {
        if (slotIndex < 0 || slotIndex >= slots.Count) // Validate index
        {
            GD.PrintErr("Invalid slot index for removing item."); // Log error for invalid index
            return null;
        }

        InventorySlot slot = slots[slotIndex]; // Get the slot at the specified index

        if (slot.item == null || slot.quantity < quantity) // Check if there are enough items to remove
        {
            GD.PrintErr("Not enough items in the slot to remove."); // Log error for insufficient items
            return null;
        }
        InventoryItem itemToReturn = slot.item; // Store the item to return
        slot.quantity -= quantity; // Decrease the quantity in the slot

        if (slot.quantity <= 0) // If the quantity reaches zero
        {
            slot.item = null; // Clear the slot if quantity reaches zero
            slot.quantity = 0; // Ensure quantity is zero
        }

        eventbus.EmitSignal(Eventbus.SignalName.inventoryUpdated); // Notify that the inventory has been updated

        return itemToReturn; // Return the removed item
    }
}
