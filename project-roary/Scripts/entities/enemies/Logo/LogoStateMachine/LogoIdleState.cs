using Godot;

public partial class LogoIdleState : LogoState
{
    public Timer timer;
    public bool IdleOver = false;

    public override void _Ready()
    {
       timer = GetParent().GetNode<Timer>("IdleTimer");
       timer.Timeout += SetIdleEnded;
    }

    public override void ExitState()
    {
        timer.Stop();
    }

    public override void EnterState()
    {
        base.EnterState();
        
        float roll = GD.Randf();

        if (roll < 0.5f)
        {
            Logo.SBAttackInstance.Fire(Logo);
        }
        else
        {
            Logo.CoffeeWaveAttackInstance.Fire(Logo);
        }

        Logo.Velocity = Vector2.Zero;
        IdleOver = false;
        timer.Start();

        Logo.hurtBox.Monitoring = true;
        Logo.hitbox.Monitoring = false;
    }

    public override LogoState Process(double delta)
    {
        if (IdleOver)
        {
            return Logo.RollState;  
        }

        return null; 
    }

    public void SetIdleEnded()
    {
        IdleOver = true;
    }
}

