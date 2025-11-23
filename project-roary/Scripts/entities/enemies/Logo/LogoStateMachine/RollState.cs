using Godot;
using System;


public partial class RollState : LogoState
{


    [Export] public float RollDuration = 3f;

    private float timer = 0f;

   public override void Enter(Logo logo)
    {
        base.Enter(logo);


        RollDuration = (float)GD.RandRange(1.0, 2.5);


        Array values = Enum.GetValues(typeof(Logo.RollDirection));
    	logo.Direction = (Logo.RollDirection)values.GetValue(GD.Randi() % values.Length);

        
        Vector2 dir = logo.Direction switch
        {
            Logo.RollDirection.TopRight => new Vector2(-1, 1),
            Logo.RollDirection.TopLeft  => new Vector2(1, 1),
            Logo.RollDirection.BottomRight => new Vector2(-1, -1),
            Logo.RollDirection.BottomLeft => new Vector2(1, -1),
            _ => Vector2.Zero
        };

        
        logo.Velocity = dir * logo.Data.Speed;
    }

    public override void Update(double delta)
    {
        timer += (float)delta;

        if (timer >= RollDuration)
        {
            logo.ChangeState(logo.LogoIdleState);
            timer = 0f;
        }
    }
}

