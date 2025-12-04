using Godot;

public partial class IguanaChase : IguanaState
{
    public IguanaRoam IguanaRoam;
    public IguanaAttack IguanaAttack;

    public override void _Ready()
    {
        IguanaRoam = GetParent().GetNode<IguanaRoam>("IguanaRoam");
        IguanaAttack = GetParent().GetNode<IguanaAttack>("IguanaAttack");
    }

    // Called when the state is entered
    public override void EnterState()
    {
    }

    // Called when the state is exited
    public override void ExitState()
    {
    }

    public override IguanaState Process(double delta)
    {
        if (!Enemy.IsPlayerInChaseRange())
        {
            return IguanaRoam;
        }
        if (Enemy.IsPlayerInAttackRange())
        {
            return IguanaAttack;
        }
        return null;
    }

    public override IguanaState Physics(double delta)
    {
        Vector2 targetPos = Enemy.target.GlobalPosition;
        Vector2 direction = (targetPos - Enemy.GlobalPosition).Normalized();
        Enemy.Velocity = direction * Enemy.data.Speed;
        Enemy.MoveAndSlide();
        Enemy.animation(direction);
        return null;
    }
}