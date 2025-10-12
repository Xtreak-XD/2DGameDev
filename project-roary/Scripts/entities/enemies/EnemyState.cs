using Godot;

public partial class EnemyState : Node2D
{
	public int MaxHealth { get; set; }
	public int Health { get; set; }
	public int Damage { get; set; }
	public int Speed { get; set; }

	public override void _Ready()
    {
		MaxHealth = 10;
		Health = MaxHealth;
		Damage = 1;
		Speed = 100;
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
