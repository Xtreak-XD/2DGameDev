using Godot;

public partial class ProjectileData : Resource
{
	[Export]
	public float maxDistance;

	[Export]
	public float speed;
	
	[Export]
	public float lifeSpan;
}
