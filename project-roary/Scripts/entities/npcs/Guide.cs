using Godot;
using System;
using System.Threading.Tasks;

public partial class Guide : Sprite2D
{
    public interactionArea interactionArea;
    public dialogueManager dialogueManager;
    public Eventbus eventbus;

    string[] thisLine =
    {
        "Good noon!",
        "Yes, it is that time of day where the sun shines just right!",
        "It is I, the Wise Turtle.",
        "Do not fear my appearance.",
        "I am merely a humble passerby in these parts.",
        "There's been lots of trouble stirring around here recently.",
        "So something tells me you're here for a reason.",
        "If that's the case, there may still be time in our current situation.",
        "You must go across campus in order to put a stop to a certain panther.",
        "Do not fret anytime on your way for our paths will surely cross again."
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