using System;
using Godot;

public partial class Starfish : Enemy
{
	[Export] 
	public Resource GenericData;
	public Player target;
	public Area2D hurtBox;
	public Area2D hitbox;
	public Marker2D projectileSource;
	public Timer setTargetTimer;

	[Export] public PackedScene EnemyScene; // Assign your enemy scene in the Inspector
    [Export] public Vector2 AreaSize = new Vector2(500, 300); // Width & height of spawn area
    [Export] public Vector2 AreaCenter = Vector2.Zero; // Center of spawn area



	[Export]

	public PackedScene bubbleProjectile;

	[Export]
	public Timer projectileTimer;

	public override void _Ready()
    {
		hurtBox = GetNode<Area2D>("HurtBox");
		hitbox = GetNode<Area2D>("Hitbox");
		projectileSource = GetNode<Marker2D>("ProjectileSource");
		setTargetTimer = GetNode<Timer>("SetTargetTimer");

		projectileTimer.Timeout += ShootBubble;
		setTargetTimer.Timeout += SetTarget;
		setTargetTimer.Start();
    }
    

    public override void _EnterTree()
    {
        AddToGroup("enemy");
    }

    public override void _ExitTree()
    {
        base._ExitTree();
    }


        

	public void SetTarget()
    {
        target = (Player)GetTree().GetFirstNodeInGroup("player");
    }

	// Aim projectile
	private void ShootBubble()
    {
		Vector2 currentPos = projectileSource.GlobalPosition;
		Vector2 targetPos = target.GlobalPosition;

		StarfishBubble bubble = (StarfishBubble)bubbleProjectile.Instantiate();
		AddChild(bubble);

		bubble.GlobalPosition = currentPos;
		bubble.target = target;

		Vector2 direction = (targetPos - currentPos).Normalized();
		bubble.Velocity = direction * bubble.data.speed;
    }
}