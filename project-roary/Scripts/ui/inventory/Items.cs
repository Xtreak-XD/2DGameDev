using Godot;
using System;

/**
Base class for  all pickable items in the game.
Handles the interaction logic and adding items to the player's inventory.
*/
public partial class Items : Sprite2D
{
	[Export] public int itemID = 0;
	[Export] public int itemQuantity = 1;
	[Export] public IndividualItem itemResource;
	private interactionArea interactionArea;
	private Inventory inv;
	private Eventbus eventbus;

	private SaveManager saveManager;


	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		interactionArea = GetNode<interactionArea>("InteractionArea");
		eventbus = GetNode<Eventbus>("/root/Eventbus");
		saveManager = GetNode<SaveManager>("/root/SaveManager");
		inv = GetNode<Inventory>("/root/Inventory");

		if (interactionArea == null)
		{
			GD.PrintErr("InteractionArea node not found!");
			return;
		}
		if (itemResource == null)
		{
			GD.PrintErr("IndividualItem resource not assigned!");
			return;
		}

		interactionArea.actionName = "pick up " + itemResource.itemName;
		interactionArea.interact = new Callable(this, "pickUpItem");

		if (itemResource.texture != null)
		{
			Texture = itemResource.texture;
		}
	}

	public void pickUpItem()
	{
		if(itemResource.itemName == "Coin")
        {
			saveManager.metaData.updateMoney(itemQuantity);
			eventbus.EmitSignal(Eventbus.SignalName.updateMoney, saveManager.metaData.Money);
			eventbus.EmitSignal(Eventbus.SignalName.interactionComplete);
			QueueFree();
            return;
        }

		if (inv == null)
		{
			GD.PrintErr("Inventory node not found!");
			return;
		}

		int remainingAmt = inv.AddItem(itemResource, itemQuantity);

		if (remainingAmt == 0)
        {
			eventbus.EmitSignal(Eventbus.SignalName.interactionComplete);
            QueueFree(); // Remove the item from the scene after picking it up
        } else if(remainingAmt < itemQuantity)
        {
         	itemQuantity = remainingAmt;
			interactionArea.actionName = $"pick up {itemResource.itemName} x{itemQuantity}";
			eventbus.EmitSignal(Eventbus.SignalName.interactionComplete);   
        }else
		{
			eventbus.EmitSignal(Eventbus.SignalName.interactionComplete);
		}
	}
}
