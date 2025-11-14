using Godot;

public partial class ProjectileData : Resource
{
	public int damage { get; set; } = 1;
	public float knockback {get; set; } = 0;

	[Export]
	public float maxDistance;

	[Export]
	public float speed;
	
	[Export]
	public float lifeSpan;
}
