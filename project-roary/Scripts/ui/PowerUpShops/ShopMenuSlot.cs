using Godot;
using System;

public partial class ShopMenuSlot : TextureButton
{
	private TextureRect itemIcon;
	private Label price;
	private Label quantityLabel;
	private IndividualItem item;
	private bool isSold = false;
	private Eventbus eventbus;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		itemIcon = GetNode<TextureRect>("ItemIcon");
		price = GetNode<Label>("ShopPrice");
		quantityLabel = GetNode<Label>("ShopQuantity");
		eventbus = GetNode<Eventbus>("/root/Eventbus");

		Pressed += OnSlotPressed;
		Toggled += OnSlotToggled;

		MouseEntered += OnMouseEntered;
		MouseExited += OnMouseExited;

	}

	public override void _ExitTree()
    {
    	Pressed -= OnSlotPressed;
		Toggled -= OnSlotToggled;

		MouseEntered -= OnMouseEntered;
		MouseExited -= OnMouseExited;
    }

	public void SetItem(IndividualItem newItem)
    {
		if (item != newItem)
		{
			isSold = false;
		}

		item = newItem;

        if (item == null || isSold)
        {
			itemIcon.Visible = false;
			price.Visible = false;
			quantityLabel.Visible = false;
			return;
        }

		itemIcon.Visible = true;
		price.Visible = true;
		quantityLabel.Visible = true;

		Show();
		itemIcon.Texture = item.texture;
		price.Text = "$" + item.shopPrice;
		quantityLabel.Text = "x" + item.shopQuantity;
    }

	public void SetSlotTexture(Texture2D texture)
	{
		if (texture != null)
		{
			TextureNormal = texture;
			TexturePressed = texture;
			TextureHover = texture;
		}
	}

	public void markAsSold()
	{
		isSold = true;
		SetItem(item);
	}

	public IndividualItem GetItem() => item;

	private void OnSlotPressed()
    {
		if (item == null || isSold) return;
		eventbus.EmitSignal(Eventbus.SignalName.shopItemSelected, item);        
    }

	private void OnSlotToggled(bool pressed)
	{
		UpdateVisualState();
	}

	private void OnMouseEntered()
	{
		if (item != null && !isSold && !ButtonPressed)
		{
			Modulate = new Color(1.2f, 1.2f, 1.2f);
		}
	}

	private void OnMouseExited()
	{
		UpdateVisualState();
	}

	private void UpdateVisualState()
	{
		if (item == null || isSold)
		{
			Modulate = Colors.White;
			return;
		}
		
		if (ButtonPressed)
		{
			Modulate = new Color(0.6f, 0.6f, 0.6f);
		}
		else // Not selected
		{
			Modulate = Colors.White;
		}
	}
}
