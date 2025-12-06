using Godot;

public partial class AlligatorDeathRoll : AlligatorState
{
    public AlligatorChase AlligatorChase;
    public AlligatorRoam AlligatorRoam;
    public Timer deathRollTimer;
    public Eventbus eventbus;
    public int times = 0;

    public override void _Ready()
    {
        eventbus = GetNode<Eventbus>("/root/Eventbus");
        AlligatorChase = GetParent().GetNode<AlligatorChase>("AlligatorChase");
        AlligatorRoam = GetParent().GetNode<AlligatorRoam>("AlligatorRoam");
        deathRollTimer = GetParent().GetNode<Timer>("DeathRollTimer");

        deathRollTimer.Timeout += Attack;
    }

    public override void EnterState()
    {
        GD.Print("Alligator has entered Death Roll state.");
        deathRollTimer.Start();

        times = 0;
    }

    public override void ExitState()
	{
        deathRollTimer.Stop();
        deathRollTimer.WaitTime = 0.5;
    }

    public override AlligatorState Process(double delta)
    {
        // Player needs to be able to dodge out of this state
        // The alligator will return to chase if the player dodges successfully
        if(!(ActiveEnemy.target == null))
        {
            ActiveEnemy.target.Velocity = Vector2.Zero;
            ActiveEnemy.target.GlobalPosition = ActiveEnemy.hitbox.GlobalPosition;

            if(times >= 7)
            {
                ActiveEnemy.target.GlobalPosition += -ActiveEnemy.Velocity.Normalized() * 600;

                return AlligatorRoam;
            }
        }
       
        return null;
    }

    public void Attack()
    {
        eventbus.EmitSignal("applyDamage", ActiveEnemy.target, ActiveEnemy, ActiveEnemy.data.Damage / 2);
        times++;

        deathRollTimer.Start();
    }
}