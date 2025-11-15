using Godot;
using System;


public partial class Player : CharacterBody2D
{
	[Export] Resource metaData;
	[Export]
	public GenericData data;
	public AnimationPlayer animationPlayer;
	public AnimatedSprite2D anim;
	public PlayerStateMachine stateMachine;

	public Vector2 cardinalDirection = Vector2.Down;
	public Vector2 direction = Vector2.Zero;

	private Eventbus eventbus;
	private int equippedSlot = 0;
	private Inventory inv;

	public Vector2 lastDirection = Vector2.Zero;

	public bool usingStamina = false;
	public bool recoveringStamina = false;

	[Export] public float rateOfStaminaRecovery;
	[Export] public int amountOfStaminaRecovered;

	public Vector2 mousePosition;
	private Vector2 knockBackVelocity = Vector2.Zero;
	private const float KnockBackDecay = 750.0f;
	public override void _Ready()
	{
		AddToGroup("player");
		animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
		anim = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		stateMachine = GetNode<PlayerStateMachine>("PlayerStateMachine");
		inv = GetNode<Inventory>("/root/Inventory");
		stateMachine.Initialize(this);
		eventbus = GetNode<Eventbus>("/root/Eventbus");
		eventbus.itemDropped += spawnItemInWorld;
		eventbus.itemEquipped += equipItem;
		eventbus.inventoryUpdated += checkIfEquipped;
	}

	public override void _Process(double delta)
	{
		direction.X = Input.GetActionStrength("Right") - Input.GetActionStrength("Left");
		direction.Y = Input.GetActionStrength("Down") - Input.GetActionStrength("Up");

		mousePosition = GetLocalMousePosition().Normalized();

		if (!usingStamina && data.Stamina < data.MaxStamina && !recoveringStamina)
		{
			recoveringStamina = true;
			RecoverStamina();
		}
	}

	private async void RecoverStamina()
	{
		await ToSignal(GetTree().CreateTimer(rateOfStaminaRecovery), Timer.SignalName.Timeout);
		data.Stamina += amountOfStaminaRecovered;
		eventbus.EmitSignal("updateStamina", data.Stamina);
		recoveringStamina = false;
	}
	
	public void ApplyKnockBack(Vector2 dir, float strength)
    {
		knockBackVelocity = dir * strength;
    }

	public override void _PhysicsProcess(double delta)
	{
		if (knockBackVelocity.LengthSquared() > 0.1f)
		{
			Velocity += knockBackVelocity;
			knockBackVelocity = knockBackVelocity.MoveToward(Vector2.Zero, KnockBackDecay * (float)delta);
		}
		MoveAndSlide();
	}

	public bool SetDirection()
	{
		Vector2 sprite = Scale;

		Vector2 newDir = cardinalDirection;

		if (direction == Vector2.Zero)
		{
			return false;
		}

		if (direction.Y == 0)
		{
			newDir = direction.X < 0 ? Vector2.Left : Vector2.Right;
		}
		else if (direction.X == 0)
		{
			newDir = direction.Y < 0 ? Vector2.Up : Vector2.Down;
		}

		if (newDir == cardinalDirection)
		{
			return false;
		}

		cardinalDirection = newDir;
		sprite.X = cardinalDirection == Vector2.Left ? -1 : 1;
		return true;
	}

	public void equipItem(int slotIndex)
	{
		if (slotIndex == equippedSlot) return;

		equippedSlot = slotIndex;
		InventorySlot equipItem = inv.slots[slotIndex];

		if (equipItem.item != null && equipItem.quantity > 0)
		{
			GD.Print($"Equipped: {equipItem.item.itemName}");
		}
		else
		{
			GD.Print($"Slot {slotIndex + 1} selected (empty)");
		}
	}

	public void checkIfEquipped()
    {
        InventorySlot currentSlot = inv.slots[equippedSlot];
	
		if (currentSlot.item != null && currentSlot.quantity > 0)
		{
			GD.Print($"Now equipped: {currentSlot.item.itemName}");
		}
    }
	
	public void spawnItemInWorld(InventoryItem item, int quantity)
	{
		PackedScene itemScene = GD.Load<PackedScene>("res://Scenes/ui/inventory/Items.tscn");
		Items itemInstance = itemScene.Instantiate() as Items;
		itemInstance.itemResource = item;
		itemInstance.itemQuantity = quantity;
		itemInstance.GlobalPosition = this.GlobalPosition + (cardinalDirection * 30); // Spawns the infront of player
		GetTree().GetCurrentScene().AddChild(itemInstance);
    }

	public void UpdateAnimation(String state)
	{
		if (animationPlayer.HasAnimation(state) &&
			(animationPlayer.CurrentAnimation != state || !animationPlayer.IsPlaying()))
		{
			animationPlayer.Play(state);
		}
	}

	public void setSpawnPosition(Vector2 pos)
    {
        GlobalPosition = pos;
    }
	
	public string AnimDirection()
    {
		if (cardinalDirection == Vector2.Down) return "down";
		else if (cardinalDirection == Vector2.Up) return "up";
		else return "side";
    }

}
