using Godot;

public partial class IguanaAttack : IguanaState
{
    public Timer timer;
    private bool change;
	private bool isAttacking = false;
	private bool isAnimationFinishedConnected = false;
	private Vector2 attackDirection;
    public IguanaChase IguanaChase;
    public IguanaRoam IguanaRoam;

    public override void _Ready()
    {
        timer = GetParent().GetNode<Timer>("AttackTimer");
        IguanaChase = GetParent().GetNode<IguanaChase>("IguanaChase");
        IguanaRoam = GetParent().GetNode<IguanaRoam>("IguanaRoam");
        timer.Timeout += OnAttackTimeout;
        timer.WaitTime = 0.5;
        timer.OneShot = true;
    }

    public override void _ExitTree()
    {
        timer.Timeout -= OnAttackTimeout;
    }

    // Called when the state is entered
    public override void EnterState()
    {
		if (Enemy.IsPlayerInAttackRange())
        {
            Vector2 targetPos = Enemy.target.GlobalPosition;
            attackDirection = (targetPos - Enemy.GlobalPosition).Normalized();
        }
        else
        {
            attackDirection = Vector2.Zero;
        }

        timer.Start();
        change = false;

		if (attackDirection != Vector2.Zero)
		{
			Enemy.AttackPlayer(attackDirection);
			isAttacking = true;

			if (!isAnimationFinishedConnected)
			{
				Enemy.anim.AnimationFinished += OnAttackAnimationFinished;
				isAnimationFinishedConnected = true;
			}
		}
	}

	private void OnAttackAnimationFinished(StringName animName)
	{
		if (!isAttacking) return;
		if (((string)animName).Contains("_attack"))
		{
			isAttacking = false;
			change = true;
		}
	}

	// Called when the state is exited
	public override void ExitState()
    {
        timer.Stop();
    }

    public override IguanaState Process(double delta)
    {
        if (!Enemy.IsPlayerInAttackRange() || change)
        {
            return IguanaChase;
        }
        return null;
    }

    public override IguanaState Physics(double delta)
    {
        Enemy.Velocity = Vector2.Zero;
        Enemy.MoveAndSlide();
		if (!isAttacking)
		{
			Enemy.animation(Vector2.Zero);
		}

		return null;
    }

    private void OnAttackTimeout()
    {
        change = true;
    }
}