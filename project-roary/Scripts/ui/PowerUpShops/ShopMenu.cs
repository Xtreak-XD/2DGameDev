using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class ShopMenu : Control
{
	public ShopResource shopConfig; // The shop configuration resource

	private Eventbus eventbus;

	private Label receiptLabel;
	private Label coinLabel;
	private Label totalLabel;
	private MetaData playerMetaData;
	private Inventory playerInv;
	private Button buyButton;
	private Player player;

	private List<ShopMenuSlot> shopItems; // Holds the UI containers
	private System.Collections.Generic.Dictionary<IndividualItem, CartItem> cart; // Tracks item → quantity

	private int totalCost = 0;

	public override void _Ready()
    {
		Hide();
		ProcessMode = ProcessModeEnum.Always;

		cart = new System.Collections.Generic.Dictionary<IndividualItem, CartItem>();
		receiptLabel = GetNode<Label>("%Receipt");
		coinLabel = GetNode<Label>("%Coins");
		totalLabel = GetNode<Label>("%Total");
		playerMetaData = ResourceLoader.Load<MetaData>("res://Resources/entities/player/playerMetaData.tres");
		playerInv = GetNode<Inventory>("/root/Inventory");
		buyButton = GetNode<Button>("%BuyButton");
		player = (Player)GetTree().GetFirstNodeInGroup("player");
		
		eventbus = GetNode<Eventbus>("/root/Eventbus");
        eventbus.openShopMenu += OpenShop;
		eventbus.shopItemSelected += AddItemToShoppingCart;
		buyButton.Pressed += OnBuyPress;

		shopItems = new List<ShopMenuSlot>();
		GetShopSlots(this);
	}

    public override void _ExitTree()
    {
		eventbus.openShopMenu -= OpenShop;
    }

    public override void _Input(InputEvent @event)
    {
        if (@event.IsActionPressed("Close_Shop") && Visible)
		{
			eventbus.EmitSignal(Eventbus.SignalName.openShopMenu, false, (ShopResource)null);
		}
    }

	private void OpenShop(bool shouldOpen, ShopResource config = null)
    {
        if (shouldOpen && !Visible && config != null) {
			shopConfig = config;
			Show();
			PopulateShop();
			GetTree().Paused = true;
			UpdateCoinDisplay();
		}
		else if (!shouldOpen && Visible) {
			Hide();
			GetTree().Paused = false;
			
			ClearCart();

			eventbus.EmitSignal(Eventbus.SignalName.interactionComplete);
		}
    }

	private void AddItemToShoppingCart(IndividualItem item)
	{
		if (cart.ContainsKey(item))
		{
			cart.Remove(item);
		}
		else
		{
			cart[item] = new CartItem(item, item.shopQuantity, item.shopQuantity); 
		}
		UpdateReceipt();
	}

	private void UpdateReceipt()
    {		
		if (cart.Count == 0)
		{
			receiptLabel.Text = "No items selected";
			totalLabel.Text = "Total: $0";
			return;
		}

		String receipt = "";
		totalCost = 0;
        foreach (var entry in cart.Values)
        {
			string itemLine = $"{entry.Item.itemName} x{entry.Quantity}".PadRight(15);
			string priceLine = $"${entry.TotalPrice}".PadLeft(5);
			receipt += itemLine + priceLine + "\n"; 
			totalCost += entry.TotalPrice;
        }

		receiptLabel.Text = receipt;
		totalLabel.Text = "Total: $" + totalCost;

		buyButton.Disabled = (totalCost > playerMetaData.Money || cart.Count == 0);

		// Maybe
		// if (totalCost > playerMetaData.Money)
		// {
		// totalLabel.AddThemeColorOverride("font_color", new Color(1, 0, 0)); // Red
		// }
		// else
		// {
		// 	totalLabel.AddThemeColorOverride("font_color", new Color(1, 1, 1)); // White (or your default color)
		// }
	}

	private void OnBuyPress()
	{
		if (cart.Count == 0) return;

		playerMetaData.Money -= totalCost;
		bool allItemsAdded = PurchaseItems();
		
		if (allItemsAdded)
		{
			GD.Print("Purchase successful!");
		}
		else
		{
			GD.Print("Purchase partially completed - some items couldn't fit in inventory!");
		}
		
		removeItemFromShop();
		ClearCart();
	}

	private void removeItemFromShop()
    {
        foreach (var cartItem in cart.Values)
        {
            shopConfig.Items.Remove(cartItem.Item);
        }
		PopulateShop();
	}

	private void UpdateCoinDisplay()
	{
		if (coinLabel != null && playerMetaData != null)
		{
			coinLabel.Text = playerMetaData.Money.ToString();
		}
	}

	private void PopulateShop()
    {
        for (int i = 0; i < shopItems.Count; i++)
        {
			if (i < shopConfig.Items.Count)
            {
				shopItems[i].SetItem(shopConfig.Items[i]);
            }
            else
            {
                shopItems[i].SetItem(null);
            }
        }
	}

	private void GetShopSlots(Node node)
	{
		foreach (Node child in node.GetChildren())
		{
			if (child is ShopMenuSlot slot && slot.Name.ToString().Contains("ItemSlot"))
			{
				shopItems.Add(slot);
			}
			GetShopSlots(child);
		}
	}

	private bool PurchaseItems()
	{
		bool allSucceeded = true;
		List<(IndividualItem, int)> failedItems = new List<(IndividualItem, int)>();
		
		foreach (var cartItem in cart.Values)
		{
			bool added = playerInv.AddItem(cartItem.Item, cartItem.Quantity);
			
			if (!added)
			{
				allSucceeded = false;
				failedItems.Add((cartItem.Item,cartItem.Quantity));
			}
		}
		
		// Print detailed feedback if any items failed
		if (!allSucceeded)
		{
			if (player == null)
			{
				GD.Print("Error: Player node not found. Cannot drop items on ground.");
				return allSucceeded;
			}

			GD.Print("Inventory full! The following items were dropped on the ground:");
			foreach (var item in failedItems)
			{
				GD.Print($"  - {item.Item1.itemName} x{item.Item2}");
				player.spawnItemInWorld(item.Item1, item.Item2);
			}
		}
		
		return allSucceeded;
	}

	private void ClearCart()
    {
        cart.Clear();
		foreach (var slot in shopItems) {
			slot.ButtonPressed = false;
		}
		UpdateReceipt();
		UpdateCoinDisplay();
    }
}
