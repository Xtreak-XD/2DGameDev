using Godot;

public partial class LogoIdleState : LogoState
{
    [Export] public float IdleDuration = 1f;

    private float timer = 0f;

    public override void Enter(Logo logo)
    {
        base.Enter(logo);
        timer = 0f;



    float roll = GD.Randf();

    if (roll < 0.5f)
    {
        logo.SBAttackInstance.Fire(logo);
    }
    else
    {
        logo.CoffeeWaveAttackInstance.Fire(logo);
    }


        
        logo.Velocity = Vector2.Zero;
    }

    public override void Update(double delta)
    {
        timer += (float)delta;

        

        if (timer >= IdleDuration)
        {
            logo.ChangeState(logo.RollState);
            timer = 0f;
        }
    }
}



