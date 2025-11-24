// using Godot;
// using System;




// public partial class Logo : Enemy
// {

// public LogoStateMachine StateMachine;
// public LogoState CurrentState;
// public LogoIdleState LogoIdleState;
// public RollState RollState;

// [Export] public LogoData Data { get; set; }

// [Export] public PackedScene SBAttack; 

// public SBAttack SBAttackInstance; 

// [Export] public PackedScene CoffeeShot; 
// public CoffeeWaveAttack CoffeeWaveAttackInstance;

// public Timer timer;

// public Area2D hurtBox;

// public Area2D hitbox;

// [Export] public RollDirection Direction = RollDirection.TopRight;

// public enum RollDirection
// 	{
// 		BottomLeft,
// 		BottomRight,
// 		TopLeft,
// 		TopRight,
// 	}





	
// 	public override void _Ready()
//     {

// 		LogoIdleState = GetNode<LogoIdleState>("LogoStateMachine/LogoIdleState");
// 		RollState = GetNode<RollState>("LogoStateMachine/RollState");

		
// 		CoffeeWaveAttackInstance = new CoffeeWaveAttack();
// 		CoffeeWaveAttackInstance.CoffeeShotScene = CoffeeShot;
// 		AddChild(CoffeeWaveAttackInstance);

// 		SBAttackInstance = new SBAttack(); 
//     	SBAttackInstance.SpiralBubble = SBAttack; 
//     	AddChild(SBAttackInstance); 

// 		StateMachine = GetNode<LogoStateMachine>("LogoStateMachine");
//     	StateMachine.Initialize(this);


// 		ChangeState(RollState);


// 		hurtBox = GetNode<Area2D>("HurtBox");
// 		hitbox = GetNode<Area2D>("Hitbox");
//     }

// 	public void ChangeState(LogoState newState)
// {
//     CurrentState?.Exit();
//     CurrentState = newState;
//     CurrentState.Enter(this);
// }

	



	


// 	//Movement Check
// 	public override void _PhysicsProcess(double delta)
// {
//     CurrentState?.Update(delta);

//     MoveAndSlide();
// }

	
// }


using Godot;
using System;

public partial class Logo : Enemy
{
    
    public LogoStateMachine stateMachine;

	public LogoIdleState IdleState;

	public RollState RollState;

    
    public Hitbox hitbox;
    public HurtBox hurtBox;

    
    [Export] public PackedScene SBAttack;
    public SBAttack SBAttackInstance;

    [Export] public PackedScene CoffeeShot;
    public CoffeeWaveAttack CoffeeWaveAttackInstance;

    
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