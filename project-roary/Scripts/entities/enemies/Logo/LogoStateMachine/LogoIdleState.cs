using Godot;

public partial class LogoIdleState : LogoState
{
    [Export] public float IdleDuration = 1f;

    private float timer = 0f;

    public override void EnterState()
    {
        base.EnterState();
        timer = 0f;
        
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
    }

    public override LogoState Process(double delta)
    {
        timer += (float)delta;

        if (timer >= IdleDuration)
        {
            timer = 0f;
            return Logo.RollState;  
        }

        return null; 
    }
}

