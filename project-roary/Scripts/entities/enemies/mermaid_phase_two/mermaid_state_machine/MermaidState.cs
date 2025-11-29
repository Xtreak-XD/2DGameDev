using Godot;
using System;

public partial class MermaidState : Node
{
	public static Mermaid ActiveEnemy;

    public override void _Ready()
    {

    }

    // Called when the state is entered	
    public virtual void EnterState()
    {
    }

    // Called when the state is exited
    public virtual void ExitState()
    {
    }

    public virtual MermaidState Process(double delta)
    {
        return null;
    }

    public virtual MermaidState Physics(double delta)
    {
        return null;
    }
}
