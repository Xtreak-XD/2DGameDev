using Godot;
using System;

public partial class ShopMenuSlot : TextureButton
{
	private TextureRect itemIcon;
	private Label name;
	private Label price;
	private IndividualItem item;
	private bool isSelected = false;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		itemIcon = GetNode<TextureRect>("ItemIcon");
		name = GetNode<Label>("Name");
		price = GetNode<Label>("Price");

		this.Pressed += onSlotPressed;
	}

	public override void _ExitTree()
    {
       this.Pressed -= onSlotPressed;
    }

	public void SetItem(IndividualItem newItem)
    {
		item = newItem;
        if (item == null)
        {
            Hide();
			return;
        }

		Show();
		itemIcon.Texture = item.texture;
		name.Text = item.itemName;
		price.Text = "$" + item.shopPrice;
    }

	private void onSlotPressed()
    {
		GD.Print("Slot Pressed");
        
    }
}
