using Godot;
using System;

public partial class InventorySlot : Resource
{
    [Export]
    public InventoryItem item; // The item stored in this inventory slot
    [Export]
    public int quantity; // The quantity of the item in this slot

    public void InputEventShortcut(InventoryItem item)
    {
        
    }
}
