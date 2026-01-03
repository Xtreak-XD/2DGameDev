using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class ShopMenu : Control
{
	public ShopResource shopConfig; // The shop configuration resource

	private Eventbus eventbus;

	public SaveManager saveManager;

	private Label receiptLabel;
	private Label coinLabel;
	private Label totalLabel;
	private MetaData playerMetaData;
	private Inventory playerInv;
	private Button buyButton;
	private Player player;

	private List<ShopMenuSlot> shopItems; // Holds the UI containers
	private System.Collections.Generic.Dictionary<IndividualItem, CartItem> cart; // Tracks item → quantity
	private static System.Collections.Generic.Dictionary<string, HashSet<string>> soldItemsByShop = new System.Collections.Generic.Dictionary<string, HashSet<string>>(); // Tracks sold items per shop

	private int totalCost = 0;

	public override void _Ready()
    {
		Hide();
		ProcessMode = ProcessModeEnum.Always;
		saveManager = GetNode<SaveManager>("/root/SaveManager");

		cart = new System.Collections.Generic.Dictionary<IndividualItem, CartItem>();
		receiptLabel = GetNode<Label>("%Receipt");
		coinLabel = GetNode<Label>("%Coins");
		totalLabel = GetNode<Label>("%Total");
		playerInv = GetNode<Inventory>("/root/Inventory");
		buyButton = GetNode<Button>("%BuyButton");
		player = (Player)GetTree().GetFirstNodeInGroup("player");
		playerMetaData = saveManager.GetMetaData();
		
		eventbus = GetNode<Eventbus>("/root/Eventbus");
        eventbus.openShopMenu += OpenShop;
		eventbus.shopItemSelected += AddItemToShoppingCart;
		buyButton.Pressed += OnBuyPress;

		shopItems = new List<ShopMenuSlot>();
		GetShopSlots(this);

		eventbus.resetShops += ResetAllShops;
	}

    public override void _ExitTree()
    {
		eventbus.openShopMenu -= OpenShop;
		eventbus.shopItemSelected -= AddItemToShoppingCart;
		buyButton.Pressed -= OnBuyPress;
		eventbus.resetShops -= ResetAllShops;
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

			ApplyShopTheme();

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

	private void ApplyShopTheme()
	{
		if (shopConfig.TableTexture != null)
		{
			var tableContainer = GetNode<PanelContainer>("%TableContainer");
			var styleBox = new StyleBoxTexture();
			styleBox.Texture = shopConfig.TableTexture;
			tableContainer.AddThemeStyleboxOverride("panel", styleBox);
		}
		
		// Apply banner/receipt background texture
		if (shopConfig.BannerTexture != null)
		{
			var bannerContainer = GetNode<PanelContainer>("%CoinsContainer"); // Or whatever you named it
			var styleBox = new StyleBoxTexture();
			styleBox.Texture = shopConfig.BannerTexture;
			bannerContainer.AddThemeStyleboxOverride("panel", styleBox);
		}
		
		// Apply slot textures to all 4 slots
		if (shopConfig.SlotTexture != null)
		{
			foreach (var slot in shopItems)
			{
				slot.SetSlotTexture(shopConfig.SlotTexture);
			}
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

		string receipt = "";
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

		buyButton.Disabled = totalCost > playerMetaData.Money || cart.Count == 0;

		// Change total label color
		if (totalCost > playerMetaData.Money)
		{
		totalLabel.AddThemeColorOverride("font_color", new Color(1, 0, 0)); // Red
		}
		else
		{
			totalLabel.AddThemeColorOverride("font_color", new Color(1, 1, 1)); // White (or your default color)
		}
	}

	private void OnBuyPress()
	{
		if (cart.Count == 0) return;

		playerMetaData.updateMoney(-totalCost);
		bool allItemsAdded = PurchaseItems();
		
		removeItemFromShop();
		ClearCart();
	}

	private void removeItemFromShop()
    {
		if (!soldItemsByShop.ContainsKey(shopConfig.ShopName))
		{
			soldItemsByShop[shopConfig.ShopName] = new HashSet<string>();
		}

		foreach (var cartItem in cart.Values)
		{
			soldItemsByShop[shopConfig.ShopName].Add(cartItem.Item.itemName);
			
			foreach (var slot in shopItems)
			{
				if (slot.GetItem() == cartItem.Item)
				{
					slot.markAsSold();
				}
			}
		}
	}

	private void UpdateCoinDisplay()
	{
		if (coinLabel != null && playerMetaData != null)
		{
			coinLabel.Text = playerMetaData.Money.ToString();
		}
		eventbus.EmitSignal(Eventbus.SignalName.updateMoney, playerMetaData.Money);
	}

	private void PopulateShop()
    {
        for (int i = 0; i < shopItems.Count; i++)
        {
			if (i < shopConfig.Items.Count)
            {
				shopItems[i].SetItem(shopConfig.Items[i]);

				if (soldItemsByShop.ContainsKey(shopConfig.ShopName) && 
					soldItemsByShop[shopConfig.ShopName].Contains(shopConfig.Items[i]?.itemName))
				{
					shopItems[i].markAsSold();
				}
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
			int remainingAmt = playerInv.AddItem(cartItem.Item, cartItem.Quantity); // Try to add items to inventory and return amount that couldn't be added
			
			if (remainingAmt > 0)
			{
				allSucceeded = false;
				failedItems.Add((cartItem.Item, remainingAmt));
			}
		}
		
		// Print detailed feedback if any items failed
		if (!allSucceeded)
		{
			if (player == null)
			{
				GD.PrintErr("Error: Player node not found. Cannot drop items on ground.");
				return allSucceeded;
			}

			GD.Print("Inventory full! The following items were dropped on the ground:");
			foreach (var item in failedItems)
			{
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

	public void ResetAllShops()
	{
		soldItemsByShop.Clear();
		GD.Print("All shop stock has been replenished!");
	}
}
