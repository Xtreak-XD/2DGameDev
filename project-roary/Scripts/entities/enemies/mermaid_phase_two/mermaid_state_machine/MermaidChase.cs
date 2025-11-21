using System;
using Godot;

public partial class MermaidChase : MermaidState
{
    public Timer chaseTimer;
    bool EndChase = false;

    public MermaidThrow MermaidThrow;
    public MermaidFullThrow MermaidFullThrow;

    public override void _Ready()
    {
        MermaidThrow = GetParent().GetNode<MermaidThrow>("MermaidThrow");
        MermaidFullThrow = GetParent().GetNode<MermaidFullThrow>("MermaidFulLThrow");

        chaseTimer = GetParent().GetNode<Timer>("ChaseTimer");
        chaseTimer.Timeout += SetChaseOver;
    }

    public override void EnterState()
    {   
        chaseTimer.Start();

        GD.Print("Mermaid is now chasing player");
        EndChase = false;
    }

    public override void ExitState()
    {
        chaseTimer.Stop();
    }

    public override MermaidState Process(double delta)
    {
        if(EndChase)
        {
            if(ActiveEnemy.HasTrident && ActiveEnemy.Shielded)
            {
                if(new Random().Next(3) == 2)
                {
                    return MermaidFullThrow;
                }
            }

            return MermaidThrow;
        }

        Vector2 currentPos = ActiveEnemy.GlobalPosition;
		Vector2 playerPos = ActiveEnemy.target.GlobalPosition;

		Vector2 direction = (playerPos - currentPos).Normalized();

		//ActiveEnemy.animation(direction); COMMENTED OUT BECAUSE WE DO NOT HAVE ANIMATIONS
		ActiveEnemy.Velocity = direction * (float)(ActiveEnemy.data.Speed *
         delta * ActiveEnemy.data.Accel);
			
		if(!ActiveEnemy.Shielded)
        {
            ActiveEnemy.Velocity *= 2f;
        }

		ActiveEnemy.MoveAndSlide();
        return null;
    }

    public void SetChaseOver()
    {
        EndChase = true;
    }
}