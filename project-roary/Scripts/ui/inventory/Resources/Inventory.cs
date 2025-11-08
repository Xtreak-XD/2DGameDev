using Godot;
using System;
using Godot.Collections;
using System.Linq;

/**
This resource represents the player's inventory.
It holds an array of InventoryItem resources that the player possesses.
*/
[GlobalClass]
public partial class Inventory : Resource
{
    [Signal]
    public delegate void inventoryChangedEventHandler();
    [Export]
    public Array<InventorySlot> slots { get; set; }
    
    public void addItem(InventoryItem itemToAdd, int quantity = 1)
    {
        var itemSlots = slots.Where(slot => slot.item == itemToAdd).ToList();
        // Check if the item already exists in the inventory
        if (itemSlots.Count > 0)
        {
            itemSlots[0].quantity += quantity; // Increase the quantity of the existing item
            EmitSignal(SignalName.inventoryChanged); // Notify that the inventory has been updated
        }
        // If the item does not exist, find an empty slot to add it 
        else
        {
            var emptySlots = slots.Where(slot => slot.item == null).ToList();
            if (emptySlots.Count > 0)
            {
                emptySlots[0].item = itemToAdd; // Add the new item to an empty slot
                emptySlots[0].quantity = quantity;
                EmitSignal(SignalName.inventoryChanged); // Notify that the inventory has been updated
            }
            else
            {
                GD.PrintErr("Inventory is full! Cannot add item: " + itemToAdd.itemName);
            }
        }
    }
}
