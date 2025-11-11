using Godot;

// Do not make any classes directly overriding Weapon, override MeleeWeapon or RangedWeapon
public partial class Weapon : Node2D
{
	[Export]
	public WeaponData data;
	public Eventbus eventbus;
	public Timer attackTimer;
	public bool canAttack;

	// IF YOU NEED TO OVERRIDE THIS, CALL base._Ready()
	public override void _Ready()
	{
		eventbus = GetNode<Eventbus>("/root/Eventbus");
		attackTimer = GetNode<Timer>("AttackTimer");

		eventbus.triggerAttack += () => Attack(GetViewport().GetMousePosition());

		attackTimer.WaitTime = data.attackRate;
		attackTimer.Timeout += SetCanAttack;

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

	// Note: We want position for the purposes of aiming projectiles.
	// Also for aiming the animations for weapons.
	// A melee weapon will likely have a very basic attack, while
	// a ranged weapon will fire a projectile.
	// DO NOT FORGET TO CALL base.Attack() in overrides
	public virtual void Attack(Vector2 pos)
	{
		GD.Print("A weapon has attacked.");
		GD.Print($"Attack position: {pos}");
		GD.Print($"Attack damage: {data.damage}");
		GD.Print($"Attack delay: {data.attackRate} seconds");

		// May have to change this later.
		Rotation = pos.Angle();
	}

	// DO NOT OVERRIDE THIS
	public void SetCanAttack()
	{
		if (canAttack == false)
		{
			GD.Print("Weapon can attack.");
			canAttack = true;
		}
	}
}
