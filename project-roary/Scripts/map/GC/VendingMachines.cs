using Godot;
using System;

public partial class VendingMachines : Sprite2D
{
    public Eventbus eventbus;
    public interactionArea interactionArea;

    public override void _Ready()
    {
        eventbus = GetNode<Eventbus>("/root/Eventbus");
        interactionArea = GetNode<interactionArea>("InteractableArea");
        interactionArea.actionName = "OOps Broken!!";

        interactionArea.interact = Callable.From(onInteract);
    }


    void onInteract()
    {
        eventbus.EmitSignal(Eventbus.SignalName.interactionComplete);
    }

}
