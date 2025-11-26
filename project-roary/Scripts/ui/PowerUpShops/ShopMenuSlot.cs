using Godot;
using System;

public partial class ShopMenuSlot : TextureButton
{
	private TextureRect itemIcon;
	private Label price;
	private Label quantityLabel;
	private IndividualItem item;
	private Eventbus eventbus;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		itemIcon = GetNode<TextureRect>("ItemIcon");
		price = GetNode<Label>("ShopPrice");
		quantityLabel = GetNode<Label>("ShopQuantity");
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
			itemIcon.Visible = false;
			price.Visible = false;
			quantityLabel.Visible = false;
			return;
        }

		Show();
		itemIcon.Texture = item.texture;
		price.Text = "$" + item.shopPrice;
		quantityLabel.Text = "x" + item.shopQuantity;
    }

	public void ClearItem()
	{
		item = null;
	}

	private void OnSlotPressed()
    {
		GD.Print(item);
		if (item == null) return;
		eventbus.EmitSignal(Eventbus.SignalName.shopItemSelected, item);        
    }
}
