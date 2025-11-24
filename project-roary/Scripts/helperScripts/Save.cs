using Godot;
using System;

public partial class Save : Sprite2D
{
    public interactionArea interactionArea;
    public Eventbus eventbus;

    public override void _Ready()
    {
        eventbus = GetNode<Eventbus>("/root/Eventbus");
        interactionArea = GetNode<interactionArea>("InteractableArea");
        interactionArea.interact = Callable.From(onInteract);
    }

    void onInteract()
    {
        eventbus.EmitSignal("save");
        eventbus.EmitSignal("interactionComplete");
    }
}
