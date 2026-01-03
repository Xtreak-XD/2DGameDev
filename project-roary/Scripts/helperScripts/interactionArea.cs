using Godot;
using System;

public partial class interactionArea : Area2D
{
    [Export] public string actionName = "interact";
    public InteractionManager interactionManager;
    public Callable interact; //can later be overriden if anyone needs custom interact logic

    public override void _Ready()
    {
        interactionManager = GetNode<InteractionManager>("/root/InteractionManager");
        interact = new Callable(this, "x");
        BodyEntered += onBodyEntered;
        BodyExited += onBodyExited;
    }

    public void x()
    {
        GD.Print("Default callable!");
        GetNode<Eventbus>("/root/Eventbus").EmitSignal(Eventbus.SignalName.interactionComplete);
    }

    public void onBodyEntered(Node2D body)
    {
        if (body.IsInGroup("player"))
        {
            GD.Print("registered to manager");
            interactionManager.registerArea(this);
        }
        return;
        
    }

    public void onBodyExited(Node2D body)
    {
        if (body.IsInGroup("player"))
        {
            GD.Print("unregistered to manager");
            interactionManager.unregisterArea(this);
        }
        return;
    }

    public override void _ExitTree()
    {
        BodyEntered -= onBodyEntered;
        BodyExited -= onBodyExited;
    }


}
