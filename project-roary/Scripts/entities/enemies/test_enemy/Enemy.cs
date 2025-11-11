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
	public void Die()
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
}
