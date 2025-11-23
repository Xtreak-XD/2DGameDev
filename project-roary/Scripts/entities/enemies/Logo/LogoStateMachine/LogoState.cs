using Godot;



public abstract partial class LogoState : Node, ILogoState
{
    protected Logo logo;

    public virtual void Enter(Logo logo)
    {
        this.logo = logo;
    }

    public virtual void Update(double delta) { }

    public virtual void Exit() { }
}
