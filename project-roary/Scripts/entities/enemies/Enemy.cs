using Godot;

public partial class Enemy : CharacterBody2D
{
	const int speed = 100;
	const double attackRange = 10.0;
	const int damage = 1;
	public int Health { get; private set; } = 5;

	//public Player Target {get; set; }; USE THIS LATER

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
    {
		Vector2 targetPos = GetGlobalMousePosition();
		//Vector2 targetPos = Target.GlobalPosition; // USE THIS LATER INSTEAD OF MOUSE POS
		Vector2 direction = (targetPos - GlobalPosition).Normalized();

		Velocity = direction * speed;
		MoveAndSlide();

	

		if(Position.DistanceTo(targetPos) < attackRange)
		{
			GD.Print("Attack for " + damage + " damage.");
		}
	}

	public void TakeDamage(int amount)
	{
		Health -= amount;
		if(Health <= 0)
		{
			Die();
		}
	}
	
	public void Die()
	{
		QueueFree();
	}
}