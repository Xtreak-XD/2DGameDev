using Godot;
using System;
using System.Threading.Tasks;

public partial class dialogueManager : Node
{
    public Eventbus eventbus;
    public PackedScene textBoxScene;

    public string[] dialogLines = [];
    public int currentLineIndex = 0;

    public TextBox textBox;
    public Vector2 textBoxPosition;

    public bool isDialogActive = false;
    public bool canAdvanceLine = false;

    public override void _Ready()
    {
        eventbus = GetNode<Eventbus>("/root/Eventbus");
        textBoxScene = GD.Load<PackedScene>("res://Scenes/Dialogue/text_box.tscn");

        eventbus.finishedDisplaying += onTextBoxFinishedDisplaying;
    }

    public async Task startDialog(Vector2 position, string[] lines)
    {
        if (isDialogActive)
        {
            return;
        }

        dialogLines = lines;
        textBoxPosition = position;
        isDialogActive = true;

        canAdvanceLine = false;

        await showTextBox();
    }

    public async Task showTextBox()
    {
        textBox = (TextBox)textBoxScene.Instantiate();
        GetTree().Root.AddChild(textBox);
        textBox.GlobalPosition = textBoxPosition;

        canAdvanceLine = false;
        await textBox.displayText(dialogLines[currentLineIndex]);
    }

    public void onTextBoxFinishedDisplaying()
    {
        canAdvanceLine = true;
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        if (
            @event.IsActionPressed("advance_dialog") &&
            isDialogActive &&
            canAdvanceLine
            )
        {
            HandleDialogAdvance();
        }
    }

    public async void HandleDialogAdvance()
    {
        textBox.QueueFree();
        currentLineIndex++;

        if (currentLineIndex >= dialogLines.Length)
        {
            isDialogActive = false;
            currentLineIndex = 0;
            eventbus.EmitSignal("interactionComplete");
            return;
        }

        await showTextBox();
    }

    public void ForceEndDialog()
    {
        if (textBox != null && IsInstanceValid(textBox))
        {
            textBox.QueueFree();
            eventbus.EmitSignal(Eventbus.SignalName.finishedDisplaying);
        }
        
        isDialogActive = false;
        currentLineIndex = 0;
    }

    public override void _ExitTree()
    {
        eventbus.finishedDisplaying -= onTextBoxFinishedDisplaying;
    }

}
