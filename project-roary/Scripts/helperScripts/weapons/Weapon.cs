using Godot;

public partial class Weapon : Node
{
	[Export]
	public WeaponData data;
	public Eventbus eventbus;
	public Timer attackTimer;
	public bool canAttack;

	public override void _Ready()
	{
		eventbus = GetNode<Eventbus>("/root/Eventbus");
		attackTimer = GetNode<Timer>("AttackTimer");

		eventbus.triggerAttack += () => Attack(GetViewport().GetMousePosition());

		attackTimer.WaitTime = data.attackRate;
		attackTimer.Timeout += SetCanAttack;

		canAttack = true;
	}

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
	public virtual void Attack(Vector2 pos)
	{
		GD.Print("A weapon has attacked");
		GD.Print($"Attack position: {pos}");
	}
	
	public void SetCanAttack()
	{
		if(canAttack == false)
        {
            GD.Print("Weapon can attack.");
			canAttack = true;
        }
    }
}
