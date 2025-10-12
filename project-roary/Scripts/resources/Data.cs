using Godot;

public partial class Data : Resource
{
	[Export]
	public int Health { get; set; } = 10;
	[Export]
	public int MaxHealth { get; set; } = 10;
	[Export]
	public int Damage { get; set; } = 1;
	[Export]
	public int Speed { get; set; } = 200;
}
