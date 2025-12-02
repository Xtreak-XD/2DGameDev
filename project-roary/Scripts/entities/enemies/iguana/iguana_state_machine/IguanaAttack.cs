using Godot;

public partial class IguanaAttack : IguanaState
{
    public Timer timer;
    private bool change;
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
        timer.Start();
        change = false;
    }

    // Called when the state is exited
    public override void ExitState()
    {
        timer.Stop();
    }

    public override IguanaState Process(double delta)
    {
        if (!Enemy.IsPlayerInAttackRange())
        {
            return IguanaChase;
        }
        if (change)
        {
            return IguanaChase;
        }
        return null;
    }

    public override IguanaState Physics(double delta)
    {
        Enemy.Velocity = Vector2.Zero;
        Enemy.MoveAndSlide();
        Enemy.animation(Vector2.Zero);
        return null;
    }

    private void OnAttackTimeout()
    {
        if (Enemy.IsPlayerInAttackRange())
        {
            Enemy.AttackPlayer();
        }
        change = true;
    }
}