using Godot;
using System;

public partial class ShopInteractable : Node2D
{
	[Export] public ShopResource shopConfig;
	private interactionArea interactable;
	private Callable interact;
	private Eventbus eventbus;
	
	public override void _Ready()
    {
		eventbus = GetNode<Eventbus>("/root/Eventbus");
        interactable = GetNode<interactionArea>("InteractableArea");

		interactable.actionName = "Shop at " + shopConfig.ShopName;
		interactable.interact = new Callable(this, "openShop");
		ProcessMode = ProcessModeEnum.Always;

    }

	public void openShop()
    {
		eventbus.EmitSignal("openShopMenu", true, shopConfig);
    }
}
