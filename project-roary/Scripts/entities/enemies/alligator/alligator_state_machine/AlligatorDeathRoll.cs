using Godot;

public partial class AlligatorDeathRoll : AlligatorState
{
    public AlligatorChase AlligatorChase;
    public Timer deathRollTimer;

    public override void _Ready()
    {
        AlligatorChase = GetParent().GetNode<AlligatorChase>("AlligatorChase");
        deathRollTimer = GetParent().GetNode<Timer>("DeathRollTimer");

        deathRollTimer.Timeout += Attack;
    }

    public override void EnterState()
    {
        GD.Print("Alligator has entered Death Roll state.");
        deathRollTimer.Start();
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

        ActiveEnemy.target.Velocity = Vector2.Zero;

        return null;
    }

    public void Attack()
    {
        GD.Print("Alligator Death Roll attack occurred.");
        
        deathRollTimer.Start();
    }
}