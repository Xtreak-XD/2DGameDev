using Godot;

public partial class Starfish : Enemy
{
	//SceneTree Variables
	public Player target;
	public Area2D hurtBox;
	public Marker2D projectileSource;
	public Timer setTargetTimer;

    [Export] public Vector2 AreaSize = new Vector2(500, 300); // Width & height of spawn area

	[Export]
	public PackedScene bubbleProjectile;

	public Timer projectileTimer;

	public override void _Ready()
    {
		hurtBox = GetNode<Area2D>("HurtBox");
		projectileSource = GetNode<Marker2D>("ProjectileSource");
		setTargetTimer = GetNode<Timer>("SetTargetTimer");
		projectileTimer = GetNode<Timer>("ProjectileTimer");

		projectileTimer.Timeout += ShootBubble;
		setTargetTimer.Timeout += SetTarget;
		setTargetTimer.Start();
    }
    
    public override void _EnterTree()
    {
        //AddToGroup("enemy");
    }

    public override void _ExitTree()
    {
        projectileTimer.Timeout -= ShootBubble;
		setTargetTimer.Timeout -= SetTarget;
    }

	public void SetTarget()
    {
        target = (Player)GetTree().GetFirstNodeInGroup("player");
    }

    public override void _Process(double delta)
    {
		if(Velocity.Length() > 0)
        {
            Velocity *= 0.9f;
        }

		if(Velocity.Length() <= 0.08)
        {
            Velocity = Vector2.Zero;
        }
    }

	// Aim projectile
	private void ShootBubble()
    {
		if(target != null)
        {
            Vector2 currentPos = projectileSource.GlobalPosition;
			Vector2 targetPos = target.GlobalPosition;

			StarfishBubble bubble = (StarfishBubble)bubbleProjectile.Instantiate();
			GetNode("ProjectileContainer").AddChild(bubble);

			bubble.GlobalPosition = currentPos;
			bubble.target = target;

			Vector2 direction = (targetPos - currentPos).Normalized();
			bubble.Velocity = direction * bubble.data.speed;
        }
    }
}