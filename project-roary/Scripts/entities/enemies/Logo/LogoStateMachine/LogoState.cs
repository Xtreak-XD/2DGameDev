using Godot;

public abstract partial class LogoState : Node
{
    public Logo Logo {get; private set;}

    public void Initialize(Logo logo)
    {
        Logo = logo;
    }
    public override void _Ready()
    {
        
    }

    public virtual void EnterState()
    {
        
    }

    public virtual void ExitState()
    {
        
    }

    
    public virtual LogoState Process(double delta)
    {
        return null;
    }

    
    public virtual LogoState Physics(double delta)
    {
        return null;
    }

}