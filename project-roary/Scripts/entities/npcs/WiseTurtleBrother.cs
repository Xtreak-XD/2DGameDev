using Godot;
using System;
using System.Threading.Tasks;

public partial class WiseTurtleBrother : StaticBody2D
{
    [Export] public string NpcName;

    // Dialogue lines
    public string[][] DialogueLines = new string[][]
    {
        new string[]
        {
            "Hello there!",
            "Boy, can I use your help.",
            "My brother has been kidnapped.",
            "Can you help him?"
        },
    };

    private interactionArea _interactionArea;
    private dialogueManager _dialogueManager;
    private Eventbus _eventbus;
    private bool _isInteracting = false;

    private AnimatedSprite2D _anim;

    public override void _Ready()
    {
        _eventbus = GetNode<Eventbus>("/root/Eventbus");
        _dialogueManager = GetNode<dialogueManager>("/root/DialogueManager");
        _interactionArea = GetNode<interactionArea>("InteractableArea");
        _anim = GetNode<AnimatedSprite2D>("AnimatedSprite2D");

        // Start in idle animation
        _anim.Play("idle");

        // Set up interaction callback
        _interactionArea.interact = Callable.From(() => { _ = OnInteractAsync(); });
    }

    private async Task OnInteractAsync()
    {
        if (_isInteracting)
            return;

        _isInteracting = true;

        // Play interaction animation
        _anim.Play("talk");

        foreach (var lines in DialogueLines)
        {
            // Position the text box above the NPC
            Vector2 textBoxOffset = new Vector2(0, -20);
            Vector2 textBoxPosition = GlobalPosition + textBoxOffset;

            _dialogueManager.startDialog(textBoxPosition, lines);

            // Wait for dialogue to finish
            await ToSignal(_eventbus, "finishedDisplaying");

            // Small delay between each dialogue set
            await ToSignal(GetTree().CreateTimer(0.25f), Timer.SignalName.Timeout);
        }

        // Return to idle animation
        _anim.Play("idle");

        _isInteracting = false;
    }
}