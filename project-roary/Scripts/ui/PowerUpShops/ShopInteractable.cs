using Godot;
using System;

public partial class ShopInteractable : Node2D
{
	[Export] public ShopResource shopConfig;
	private interactionArea interactable;
	private Sprite2D shopSprite;
	private Callable interact;
	private Eventbus eventbus;
	
	public override void _Ready()
    {
		eventbus = GetNode<Eventbus>("/root/Eventbus");
        interactable = GetNode<interactionArea>("%InteractableArea");
		shopSprite = GetNode<Sprite2D>("%ShopSprite");

		if (shopConfig.WorldShopSprite != null)
		{
			shopSprite.Texture = shopConfig.WorldShopSprite;
			if (shopSprite.Texture.ResourcePath.Contains("LeftFacingWeaponTable") || shopSprite.Texture.ResourcePath.Contains("RightFacingWeaponTable"))
            {
				shopSprite.Rotation = 0;
            }
		}

		interactable.actionName = "Shop at " + shopConfig.ShopName;
		interactable.interact = new Callable(this, "openShop");
		ProcessMode = ProcessModeEnum.Always;

    }

	public void openShop()
    {
		eventbus.EmitSignal(Eventbus.SignalName.openShopMenu, true, shopConfig);
    }
}
