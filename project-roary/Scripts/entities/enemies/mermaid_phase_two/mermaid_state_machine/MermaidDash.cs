using Godot;
using System;

public partial class MermaidDash : MermaidState
{
	public Timer dashTimer;
    bool EndDash = false;

	public MermaidChase MermaidChase;

    public override void _Ready()
    {
		MermaidChase = GetParent().GetNode<MermaidChase>("MermaidChase");

        dashTimer = GetParent().GetNode<Timer>("DashTimer");
        dashTimer.Timeout += SetDashOver;
    }

    public override void EnterState()
    {   
        dashTimer.Start();
		ActiveEnemy.Shielded = false;

        GD.Print("Mermaid is now dashing at the player");
        EndDash = false;
    }

    public override void ExitState()
    {
        dashTimer.Stop();
    }

    public override MermaidState Process(double delta)
    {
        if(ActiveEnemy.target != null)
        {
            if(EndDash)
            {
                return MermaidChase;
            }

            Vector2 currentPos = ActiveEnemy.GlobalPosition;
            Vector2 playerPos = ActiveEnemy.target.GlobalPosition;

            if(currentPos.DistanceTo(playerPos) <= 70)
            {
                SetDashOver();
            }

            Vector2 direction = (playerPos - currentPos).Normalized();

            //ActiveEnemy.animation(direction); COMMENTED OUT BECAUSE WE DO NOT HAVE ANIMATIONS
            ActiveEnemy.Velocity = direction * (float)(ActiveEnemy.data.Speed * 2.5);

            ActiveEnemy.MoveAndSlide();
        }
        
        return null;
    }

    public void SetDashOver()
    {
        EndDash = true;
    }
}
