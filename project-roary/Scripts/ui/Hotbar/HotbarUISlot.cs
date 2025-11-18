using Godot;
using System;

public partial class HotbarUISlot : TextureButton
{
	public int slotIndex;
	public bool isSelected;
	private Inventory inv;
	private TextureRect icon;
	private Label quantityLabel;
	
	
	public override void _Ready()
    {
		inv = GetNode<Inventory>("/root/Inventory");
		icon = GetNode<TextureRect>("ItemDisplay");
		quantityLabel = GetNode<Label>("ItemDisplay/QuantityLabel");
    }

	public void UpdateDisplay(InventorySlot slot)
	{
		if (slot.item == null)
		{
			icon.Visible = false;
			quantityLabel.Visible = false;
		}
		else
		{
			icon.Visible = true;
			icon.Texture = slot.item.texture;
			if (slot.quantity > 1)
			{
				quantityLabel.Visible = true;
            }
            else
            {
				quantityLabel.Visible = false;
            }
			quantityLabel.Text = slot.quantity.ToString();
		}
	}
}
