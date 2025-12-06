using Godot;
using System;

public partial class WiseTurtleBrother : CharacterBody2D
{
    [Export] public string NpcName;
    public SaveManager saveManager;

    // Dialogue lines
    public string[][] DialogueLines = new string[][]
    {
        new string[]
        {
            "Hello there!",
            "Thank you for saving me!!!",
            "I am sure my brother will give you something!",
            "Go tell him I am okay!!"
        },
    };

    private interactionArea _interactionArea;
    private dialogueManager _dialogueManager;
    private Eventbus _eventbus;

    public override void _Ready()
    {
        _eventbus = GetNode<Eventbus>("/root/Eventbus");
        _dialogueManager = GetNode<dialogueManager>("/root/DialogueManager");
        _interactionArea = GetNode<interactionArea>("InteractableArea");
        saveManager = GetNode<SaveManager>("/root/SaveManager");

        // Set up interaction callback
        _interactionArea.interact = Callable.From(OnInteractAsync);
    }

    private async void OnInteractAsync()
    {
        var sm = saveManager;
        var md = sm.metaData;

        await _dialogueManager.startDialog(GlobalPosition, DialogueLines[0]);
        // Wait for dialogue to finish
        await ToSignal(_eventbus, "finishedDisplaying");

        if (!md.SavedYoungerTurtleBrother)
        {
            md.SavedYoungerTurtleBrother = true;
            sm.SaveNpcFlags();
        }
        _eventbus.EmitSignal(Eventbus.SignalName.interactionComplete);
    }
}