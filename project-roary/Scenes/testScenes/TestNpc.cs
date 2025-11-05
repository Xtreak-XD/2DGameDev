using Godot;
using System;
using System.Threading.Tasks;

public partial class TestNpc : Sprite2D
{
    public interactionArea interactionArea;
    public dialogueManager dialogueManager;
    public Eventbus eventbus;

    string[] thisLine =
    {
        "Hey, what's up!",
        "My name is Lebron James"
    };

    public override void _Ready()
    {
        eventbus = GetNode<Eventbus>("/root/Eventbus");
        dialogueManager = GetNode<dialogueManager>("/root/DialogueManager");
        interactionArea = GetNode<interactionArea>("InteractableArea");
        interactionArea.interact = Callable.From(onInteract);
    }

    public async Task onInteract()
    {
        GD.Print("onInteractTest");
        dialogueManager.startDialog(GlobalPosition, thisLine);
        await ToSignal(eventbus, "finishedDisplaying");
    }

}
