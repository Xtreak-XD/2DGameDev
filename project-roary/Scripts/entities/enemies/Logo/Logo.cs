using Godot;

public partial class Logo : Enemy
{
    public new LogoStateMachine stateMachine;

    public Player target;

    private Timer TargetTimer;

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

    [Export]
    public PackedScene mermaid;
    
    [Export] public LogoData Data { get; set; }

    [Export]
    public Marker2D[] starfishSpawns;
    
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

        TargetTimer = GetNode<Timer>("SetTargetTimer");
        TargetTimer.Timeout += SetTarget;
        TargetTimer.Start();
    }

    public override void _PhysicsProcess(double delta)
    {
        MoveAndSlide();
    }

    public void SetTarget()
    {
        target = (Player)GetTree().GetFirstNodeInGroup("player");

        if(target == null)
        {
            GD.PrintErr("The player could not be found");
        }
    }

    public override void Die()
    {
        Mermaid phaseTwo = (Mermaid) mermaid.Instantiate();
        phaseTwo.GlobalPosition = GlobalPosition;
        phaseTwo.starfishSpawns = starfishSpawns;
        GetParent().AddChild(phaseTwo);
        
        base.Die();
    }
}