using Godot;

public partial class Enemy : CharacterBody2D
{
	[Export]
	public GenericData data;
	[Export]
	public float OffScreenDespawnTime = 3f;
	public EnemyStateMachine stateMachine;
	private float offScreenTimer = 0f;
	private bool offScreen = false;

	private Vector2 knockBackVelocity = Vector2.Zero;
	private const float KnockBackDecay = 750.0f;
	public override void _EnterTree()
	{
		AddToGroup("enemy");
	}

	public override void _Ready()
	{
		stateMachine = GetNode<EnemyStateMachine>("EnemyStateMachine");
		stateMachine.Initialize(this);

		var visibilityNotifier = GetNode<VisibleOnScreenNotifier2D>("VisibleOnScreenNotifier2D");
        visibilityNotifier.ScreenExited += OnScreenExit;
        visibilityNotifier.ScreenEntered += OnScreenEnter;
	}
	public virtual void Die()
	{
		GD.Print($"{Name} died!");
		Eventbus.EnemyDied(this);
		QueueFree();  // K.O.'s the enemy
	}
	private void OnScreenExit()
	{
		offScreen = true;
		offScreenTimer = 0f;
	}
	private void OnScreenEnter()
	{
		offScreen = false;
		offScreenTimer = 0f;
	}
	
	public void ApplyKnockBack(Vector2 dir, float strength)
    {
		knockBackVelocity = dir * strength;
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (offScreen)
		{
			offScreenTimer += (float)delta;
			if (offScreenTimer >= OffScreenDespawnTime)
			{
				Die(); // Automatically despawns the enemy
			}
		}
	}

    public override void _PhysicsProcess(double delta)
    {
		if (knockBackVelocity.LengthSquared() > 0.1f)
		{
			Velocity += knockBackVelocity;
			knockBackVelocity = knockBackVelocity.MoveToward(Vector2.Zero, KnockBackDecay * (float)delta);
		}
		MoveAndSlide();
    }

}
