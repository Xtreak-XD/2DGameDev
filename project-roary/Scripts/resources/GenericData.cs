using Godot;

[GlobalClass]
public partial class GenericData : Resource
{
	[Export]
	public int Health { get; set; }
	[Export]
	public int MaxHealth { get; set; }
	[Export]
	public int Damage { get; set; }
	[Export]
	public int Speed { get; set; }
}
