using Godot;

public partial class IguanaRoam : IguanaState
{
    public Timer timer;
    public IguanaAttack IguanaAttack;
    public IguanaChase IguanaChase;
    private Vector2 targetPos;

    public override void _Ready()
    {
        timer = GetParent().GetNode<Timer>("RoamTimer");
        IguanaAttack = GetParent().GetNode<IguanaAttack>("IguanaAttack");
        IguanaChase = GetParent().GetNode<IguanaChase>("IguanaChase");
        timer.Timeout += OnRoamTimeout;
        timer.WaitTime = 2.0;
        timer.OneShot = true;
    }

    public override void _ExitTree()
    {
        timer.Timeout -= OnRoamTimeout;
    }

    public override void EnterState()
    {
        PickNewTargetPosition();
    }

    // Called when the state is exited
    public override void ExitState()
    {
        timer.Stop();
    }

    public override IguanaState Process(double delta)
    {
        if (Enemy.IsPlayerInChaseRange())
        {
            return IguanaChase;
        }
        if (Enemy.IsPlayerInAttackRange())
        {
            return IguanaAttack;
        }
        return null;
    }

    public override IguanaState Physics(double delta)
    {
        if (Enemy.GlobalPosition.DistanceTo(targetPos) > 10)
        {
            Vector2 direction = (targetPos - Enemy.GlobalPosition).Normalized();
            Enemy.Velocity = direction * Enemy.data.Speed;
            Enemy.MoveAndSlide();
            Enemy.animation(direction);
        }
        else
        {
            Enemy.Velocity = Vector2.Zero;
            Enemy.MoveAndSlide();
            Enemy.animation(Vector2.Zero);
            if (timer.IsStopped())
            {
                timer.Start();
            }
        }
        return null;
    }

    private void PickNewTargetPosition()
    {
        targetPos = Enemy.GetRandomPositionInRoamRange();
    }

    private void OnRoamTimeout()
    {
        PickNewTargetPosition();
    }
}