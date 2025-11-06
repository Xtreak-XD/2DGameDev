using Godot;
using System;

public partial class InventoryUI : Control
{
	public Boolean isOpen = false;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		close();
	}

	public override void _Process(double delta)
	{
		if (Input.IsActionJustPressed("I"))
		{
			GD.Print("Toggling Inventory");
			if (isOpen)
			{
				close();
			}
			else
			{
				open();
			}
		}
	}
	
	public void open()
	{
		Visible = true; // Built int function to display Nodes
		isOpen = true;
	}

	public void close()
    {
		Visible = false;
		isOpen = false;
    }
}
