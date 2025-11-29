using System;
using Godot;


public partial class Player : CharacterBody2D
{

	public SceneManager sceneManager;
	public string save_file_path = "user://LocalSave/";
	public string save_file_name = "playerMetaData.tres";
	public MetaData metaData = new();
	[Export] public GenericData data;
	public AnimationPlayer animationPlayer;
	public PlayerStateMachine stateMachine;

	public Vector2 cardinalDirection = Vector2.Down;
	public Vector2 direction = Vector2.Zero;
	public Vector2 lastDirection = Vector2.Zero;
	public Vector2[] DIRECTIONS = {Vector2.Right, Vector2.Left, Vector2.Up, Vector2.Down};

	private Eventbus eventbus;
	private int equippedSlot = -1;
	public Inventory inv;

	public bool usingStamina = false;
	public bool recoveringStamina = false;

	[Export] public float rateOfStaminaRecovery;
	[Export] public int amountOfStaminaRecovered;

	public Vector2 mousePosition;
	private Vector2 knockBackVelocity = Vector2.Zero;
	private const float KnockBackDecay = 750.0f;

    public override void _EnterTree()
    {
		AddToGroup("player");
		eventbus = GetNode<Eventbus>("/root/Eventbus");
		eventbus.itemDropped += spawnItemInWorld;
		eventbus.itemEquipped += equipItem;
		eventbus.inventoryUpdated += checkIfEquipped;
		eventbus.save += save;
		eventbus.load += loadSave;
    }
	public override void _Ready()
	{
		sceneManager = GetNode<SceneManager>("/root/SceneManager");
		VerifySaveDirectory(save_file_path);
		stateMachine = GetNode<PlayerStateMachine>("PlayerStateMachine");
		animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
		stateMachine.Initialize(this);

		inv = GetNode<Inventory>("/root/Inventory");
	}

	void loadSave()
    {
		string fullPath = save_file_path + save_file_name;
        if (!ResourceLoader.Exists(fullPath))
        {
			GD.Print("No save file found at " + fullPath);
			return;
        }

        try
        {
            metaData = ResourceLoader.Load<MetaData>(fullPath);

			if (metaData == null)
            {
                GD.PrintErr("Failed to load save from " + fullPath);
				return;
            }

			if (metaData.savedInventory != null && metaData.savedInventory.Count > 0)
            {
				if(inv.slots == metaData.savedInventory) //not loading the correct inventory
                {
                    GD.Print("saved inventory and current inventory are the same");
                }
                else
                {
                    GD.Print("saved inventory and current inventory are different");
					inv.slots = metaData.savedInventory;
					eventbus.EmitSignal(Eventbus.SignalName.inventoryUpdated);
                }
            }

			if (metaData.Money > 0)
            {
                eventbus.EmitSignal("updateMoney", metaData.Money);
            }

			GD.Print("loaded save from " + fullPath);

			string curScene = GetTree().CurrentScene.SceneFilePath;
			if (curScene != metaData.curScenepath)
            {
                onStartLoad();
            }
            else
            {
                GlobalPosition = metaData.savePos;
            }
        }
		catch(Exception e)
        {
            GD.PrintErr($"Error loading save: {e.Message}");
        }
    }

	void onStartLoad()
    {
		if (string.IsNullOrEmpty(metaData.curScenepath))
        {
            GD.PrintErr("Can't load scene path");
			return;
        }
		if (!ResourceLoader.Exists(metaData.curScenepath))
		{
			GD.PrintErr($"Scene doesn't exist: {metaData.curScenepath}");
			return;
		}
		sceneManager.goToScene(GetParent(), metaData.curScenepath, true);
    }

	void save()
    {
		metaData.SetSavePos(Position);
		metaData.SetCurScenePath(GetTree().CurrentScene.SceneFilePath);
		metaData.updateInventory(inv);

		VerifySaveDirectory(save_file_path);

		string fullPath = save_file_path+save_file_name;
        Error result = ResourceSaver.Save(metaData, fullPath);
		if (result == Error.Ok)
        {
            GD.Print("saved at " + save_file_path + save_file_name);
			//emit signal to say we saved it later
        }
        else
        {
            GD.PrintErr($"Failed to save game: {result}");
        }
		
    }

	void VerifySaveDirectory(string saveFilePath)
    {
        DirAccess.MakeDirAbsolute(saveFilePath);
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

    public override void _ExitTree()
    {
        eventbus.itemDropped -= spawnItemInWorld;
		eventbus.itemEquipped -= equipItem;
		eventbus.inventoryUpdated -= checkIfEquipped;
		eventbus.save -= save;
		eventbus.load -= loadSave;
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
		if (direction == Vector2.Zero){ return false;}
		Vector2 new_dir;

		if (Mathf.Abs(direction.X) > Mathf.Abs(direction.Y))
		{
			new_dir = direction.X > 0 ? Vector2.Right : Vector2.Left;
		}
		else
		{
			new_dir = direction.Y > 0 ? Vector2.Down : Vector2.Up;
		}

		if (new_dir == cardinalDirection){ return false; }

		cardinalDirection = new_dir;

		return true;
    }

	public string SetAnimDir()
    {
		if (Mathf.Abs(cardinalDirection.X) > Mathf.Abs(cardinalDirection.Y))
		{
			return cardinalDirection.X > 0 ? "right" : "left";
		}
		else
		{
			return cardinalDirection.Y > 0 ? "down" : "up";
		}
    }
	public void UpdateAnimation(string state)
	{
		if( state != "idle")
		{
			animationPlayer.Play(state + "_" + SetAnimDir());
		}
        else
        {
            animationPlayer.Play(state + "_" + SetAnimDir());
        }
	}

	public void setSpawnPosition(Vector2 pos)
    {
        GlobalPosition = pos;
    }

	public void spawnItemInWorld(IndividualItem item, int quantity)
	{
		PackedScene itemScene = GD.Load<PackedScene>("res://Scenes/ui/inventory/Items.tscn");
		Items itemInstance = itemScene.Instantiate() as Items;
		itemInstance.itemResource = item;
		itemInstance.itemQuantity = quantity;
		itemInstance.GlobalPosition = this.GlobalPosition + (cardinalDirection * 30); // Spawns in front of player
		GetTree().GetCurrentScene().AddChild(itemInstance);
	}

	public void equipItem(int slotIndex)
	{
		if (slotIndex < -1 || slotIndex >= inv.slots.Count)
		{
			GD.PrintErr($"Invalid slot index: {slotIndex}");
			return;
		}

		if (slotIndex == equippedSlot) return;	

		equippedSlot = slotIndex;
		if (slotIndex < 0)
		{
			GD.Print("Unequipped hotbar slot");
			return;
		}

		InventorySlot equipItem = inv.slots[slotIndex];

		if (equipItem.item != null && equipItem.quantity > 0)
		{
			GD.Print($"Equipped: {equipItem.item.itemName}");
		}
		else
		{
			GD.Print("Empty slot equipped");
		}
	}

	public void checkIfEquipped()
	{
		if (equippedSlot < 0) return;

		if (equippedSlot >= inv.slots.Count)
		{
			GD.PrintErr("Invalid equipped slot index");
			return;
		}

		InventorySlot equipItem = inv.slots[equippedSlot];
		if (equipItem.item == null || equipItem.quantity == 0)
		{
			GD.Print("Equipped slot is now empty");
		}
	}

    public override void _Input(InputEvent @event)
    {
        if (@event.IsActionPressed("load"))
        {
            loadSave();
        }
    }

}
