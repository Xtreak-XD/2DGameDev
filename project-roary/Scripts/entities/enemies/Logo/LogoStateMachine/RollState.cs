using Godot;
using System;

public partial class RollState : LogoState
{
    [Export] public float RollDuration = 3f;

    private float timer = 0f;

    public override void EnterState()
    {
        
        timer = 0f;

        
        RollDuration = (float)GD.RandRange(1.0, 2.5);

        
        Array values = Enum.GetValues(typeof(Logo.RollDirection));
        Logo.Direction = 
            (Logo.RollDirection) values.GetValue(GD.Randi() % values.Length);

        
        Vector2 dir = Logo.Direction switch
        {
            Logo.RollDirection.TopRight    => new Vector2(-1,  1),
            Logo.RollDirection.TopLeft     => new Vector2( 1,  1),
            Logo.RollDirection.BottomRight => new Vector2(-1, -1),
            Logo.RollDirection.BottomLeft  => new Vector2( 1, -1),
            _ => Vector2.Zero
        };

        
        Logo.Velocity = dir * Logo.Data.Speed;
    }

    public override void ExitState()
    {
        
    }

    public override LogoState Process(double delta)
    {
        timer += (float)delta;

        if (timer >= RollDuration)
            return Logo.IdleState;

        return null;
    }
}
