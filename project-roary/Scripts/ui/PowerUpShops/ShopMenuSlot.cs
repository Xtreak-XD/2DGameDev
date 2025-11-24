using Godot;
using System;

public partial class ShopMenuSlot : TextureButton
{
	private TextureRect itemIcon;
	private Label price;
	private IndividualItem item;
	private Eventbus eventbus;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		itemIcon = GetNode<TextureRect>("ItemIcon");
		price = GetNode<Label>("Price");
		eventbus = GetNode<Eventbus>("/root/Eventbus");

		Pressed += OnSlotPressed;
	}

	public override void _ExitTree()
    {
       Pressed -= OnSlotPressed;
    }

	public void SetItem(IndividualItem newItem)
    {
		item = newItem;
        if (item == null)
        {
			return;
        }

		Show();
		itemIcon.Texture = item.texture;
		price.Text = "$" + item.shopPrice;
    }

	private void OnSlotPressed()
    {
		if (item == null) return;
		eventbus.EmitSignal(Eventbus.SignalName.shopItemSelected, item);        
    }
}
