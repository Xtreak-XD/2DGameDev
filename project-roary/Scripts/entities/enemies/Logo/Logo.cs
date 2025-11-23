using Godot;

public partial class Logo : Enemy
{
	public LogoState CurrentState;
	public LogoIdleState LogoIdleState;
	public RollState RollState;

	[Export] public LogoData Data { get; set; }

	[Export] public PackedScene SBAttack; 

	public SBAttack SBAttackInstance; 

	[Export] public PackedScene CoffeeShot; 
	public CoffeeWaveAttack CoffeeWaveAttackInstance;

	public Timer timer;

	public Area2D hurtBox;

	public Area2D hitbox;

	[Export] public RollDirection Direction = RollDirection.TopRight;

	public enum RollDirection
	{
		BottomLeft,
		BottomRight,
		TopLeft,
		TopRight,
	}

	public override void _Ready()
	{
		LogoIdleState = GetNode<LogoIdleState>("LogoStateMachine/LogoIdleState");
		RollState = GetNode<RollState>("LogoStateMachine/RollState");

		CoffeeWaveAttackInstance = new CoffeeWaveAttack();
		CoffeeWaveAttackInstance.CoffeeShotScene = CoffeeShot;
		AddChild(CoffeeWaveAttackInstance);

		SBAttackInstance = new SBAttack(); 
		SBAttackInstance.SpiralBubble = SBAttack; 
		AddChild(SBAttackInstance); 

		ChangeState(RollState);

		hurtBox = GetNode<Area2D>("HurtBox");
		hitbox = GetNode<Area2D>("Hitbox");
	}

	public void ChangeState(LogoState newState)
	{
		CurrentState?.Exit();
		CurrentState = newState;
		CurrentState.Enter(this);
	}

	//Movement Check
	public override void _PhysicsProcess(double delta)
	{
		CurrentState?.Update(delta);

		MoveAndSlide();
	}
}
