using Godot;

public partial class WeaponData : Resource
{
	[Export]
	public int damage;
	
	// Delay between attacks (in seconds)
	[Export]
	public float attackRate;
	[Export]
	public float knockback;
}
