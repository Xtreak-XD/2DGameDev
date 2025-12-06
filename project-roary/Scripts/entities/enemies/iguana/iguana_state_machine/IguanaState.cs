using System;
using Godot;

public partial class IguanaState : Node
{
	protected Iguana Enemy { get; private set; }
    public void SetEnemy(Iguana enemy) => Enemy = enemy;

    // Called when the state is entered
    public virtual void EnterState()
    {
    }

    // Called when the state is exited
    public virtual void ExitState()
    {
    }

    public virtual IguanaState Process(double delta)
    {
        return null;
    }

    public virtual IguanaState Physics(double delta)
    {
        return null;
    }
}