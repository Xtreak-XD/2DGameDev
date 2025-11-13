using Godot;

// Do not make any classes directly extending Weapon, extend MeleeWeapon or RangedWeapon
public partial class Weapon : Node2D
{
	// DO NOT OVERRIDE ANY OF THIS
	[Export]
	public WeaponData data;
	public Eventbus eventbus;
	public Timer attackTimer;
	public bool canAttack;
	public Sprite2D sprite;
	public Node2D parent;
	public Vector2 mousePosition;

	// IF YOU NEED TO OVERRIDE THIS, CALL base._Ready()
	// DO NOT OVERRIDE THIS UNLESS YOU ARE CREATNG A NEW
	// WEAPON SUBTYPE LIKE MeleeWeapon or RangedWeapon.
	public override void _Ready()
	{
		eventbus = GetNode<Eventbus>("/root/Eventbus");
		attackTimer = GetNode<Timer>("AttackTimer");
		sprite = GetNode<Sprite2D>("Sprite2D");

		eventbus.triggerAttack += () => Attack(mousePosition);

		attackTimer.WaitTime = data.attackRate;
		attackTimer.Timeout += SetCanAttack;

		if(GetParent() != null)
        {
            parent = GetParent<Node2D>();
        }

		canAttack = true;
	}

	// DO NOT OVERRIDE THIS
	public override void _Input(InputEvent @event)
	{
		if (@event is InputEventMouseButton mouseEvent)
		{
			if (mouseEvent.ButtonIndex == MouseButton.Left)
			{
				if (canAttack)
				{
					attackTimer.Start();
					eventbus.EmitSignal("triggerAttack");

					canAttack = false;
				}
			}
		}
	}

	// DO NOT OVERRIDE THIS
    public override void _Process(double delta)
    {
		mousePosition = GetGlobalMousePosition();

		LookAt(mousePosition);
    }

	// Note: We want position for the purposes of aiming projectiles.
	// Also for aiming the animations for weapons.
	// A melee weapon will likely have a very basic attack, while
	// a ranged weapon will fire a projectile.
	// DO NOT FORGET TO CALL base.Attack() at the start 
	// in overrides.
	// DO NOT OVERRIDE THIS UNLESS YOU ARE CREATNG A NEW
	// WEAPON SUBTYPE LIKE MeleeWeapon or RangedWeapon
	public virtual void Attack(Vector2 pos)
	{
		GD.Print("A weapon has attacked.");
		GD.Print($"Attack position: {pos}");
		GD.Print($"Attack damage: {data.damage}");
		GD.Print($"Attack delay: {data.attackRate} seconds");

		Vector2 direction = (pos - parent.Position).Normalized();
		float angle = direction.Angle();

		Vector2 offset = new Vector2(Mathf.Cos(angle),
		 Mathf.Sin(angle)).Normalized() * 200; // THIS WILL NEED ADJUSTING BASED
											   // ON FINAL DIMENSIONS
											   // SINCE PLAYER'S NODES ARE 
											   // NOT CENTERED

		Position = offset;
	}

	// DO NOT OVERRIDE THIS
	public void SetCanAttack()
	{
		if (canAttack == false)
		{
			Position = Vector2.Zero;
			GD.Print("Weapon can attack.");
			canAttack = true;
		}
	}
}
