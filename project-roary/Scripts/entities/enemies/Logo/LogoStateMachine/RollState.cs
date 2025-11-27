using Godot;
using System;

public partial class RollState : LogoState
{
    public Timer timer;
    public bool EndRoll = false;
    public bool SpawnedStarfish = false;

    public override void _Ready()
    {
       timer = GetParent().GetNode<Timer>("RollTimer");
       timer.Timeout += SetRollEnded;
    }

    public override void EnterState()
    {
        if(Logo.target != null)
        {
            Vector2 dir = (Logo.target.GlobalPosition - Logo.GlobalPosition).Normalized();

            Logo.Velocity = dir * Logo.Data.Speed;
            EndRoll = false;
            timer.Start();
        }

        Logo.hurtBox.Monitoring = false;
        Logo.hitbox.Monitoring = true;
        SpawnedStarfish = false;
    }

    public override void ExitState()
    {
        timer.Stop();
    }

    public override LogoState Process(double delta)
    {
        if(timer.TimeLeft <= timer.WaitTime / 2 && !SpawnedStarfish)
        {
            Vector2 direction = Logo.Velocity.Rotated(Logo.sprite.Rotation).Normalized();

            Starfish starfish = (Starfish)Logo.starfish.Instantiate();
            Logo.Owner.AddChild(starfish);
            starfish.GlobalPosition = Logo.GlobalPosition;
            
            starfish.Velocity = direction * (Logo.Data.SpinSpeed * 60);

            GD.Print("The logo has flung out a starfish");
            SpawnedStarfish = true;
        }

        Logo.sprite.RotationDegrees += Logo.Data.SpinSpeed;

        if(EndRoll)
        {
            return Logo.IdleState;
        }

        return null;
    }

    public void SetRollEnded()
    {
        EndRoll = true;
    }
}
