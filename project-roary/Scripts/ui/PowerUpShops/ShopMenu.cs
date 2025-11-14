using Godot;
using System;

public partial class ShopMenu : Control
{
	// Called when the node enters the scene tree for the first time.
	private Eventbus eventbus;
	public override void _Ready()
    {
		Hide();
		ProcessMode = ProcessModeEnum.Always;
		eventbus = GetNode<Eventbus>("/root/Eventbus");
        eventbus.openShopMenu += openShop;
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
    {
        
    }

    public override void _Input(InputEvent @event)
    {
        if (@event.IsActionPressed("ui_cancel") && Visible)
		{
			eventbus.EmitSignal("openShopMenu", false);
		}
    }


	public void openShop(bool shouldOpen)
    {
        if (shouldOpen && !Visible) {
			Show();
			GetTree().Paused = true;
		}
		else if (!shouldOpen && Visible) {
			Hide();
			GetTree().Paused = false;
		}
    }
}
