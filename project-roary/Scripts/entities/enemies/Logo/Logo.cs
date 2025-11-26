using Godot;

public partial class Logo : Enemy
{
    public new LogoStateMachine stateMachine;

	public LogoIdleState IdleState;

	public RollState RollState;
    
    public Hitbox hitbox;
    public HurtBox hurtBox;

    public Sprite2D sprite;

    [Export] public PackedScene SBAttack;
    public SBAttack SBAttackInstance;

    [Export] public PackedScene CoffeeShot;
    public CoffeeWaveAttack CoffeeWaveAttackInstance;

    [Export]
    public PackedScene starfish;
    
    [Export] public LogoData Data { get; set; }

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
        hurtBox = GetNode<HurtBox>("HurtBox");
        hitbox  = GetNode<Hitbox>("Hitbox");

        sprite = GetNode<Sprite2D>("Sprite2D");

		CoffeeWaveAttackInstance = new CoffeeWaveAttack();
        CoffeeWaveAttackInstance.CoffeeShotScene = CoffeeShot;
        AddChild(CoffeeWaveAttackInstance);

        SBAttackInstance = new SBAttack();
        SBAttackInstance.SpiralBubble = SBAttack;
        AddChild(SBAttackInstance);

        stateMachine = GetNode<LogoStateMachine>("LogoStateMachine");
        stateMachine.Initialize(this);

		IdleState = stateMachine.GetNode<LogoIdleState>("LogoIdleState");
    	RollState = stateMachine.GetNode<RollState>("RollState");
    }

    public override void _PhysicsProcess(double delta)
    {
        MoveAndSlide();
    }
}