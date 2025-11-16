using Godot;

public partial class ProjectileData : Resource
{
	public int Damage { get; set; } = 1;
	public float knockback {get; set; } = 0;

	[Export]
	public float maxDistance;

	[Export]
	public float speed;
	
	[Export]
	public float lifeSpan;
}
