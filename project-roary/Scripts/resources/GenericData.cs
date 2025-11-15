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

	[Export] public bool dealKnockback;
	[Export(PropertyHint.Range, "0.0, 100.0, 15.0")] public float knockBackAmount;
	

}
