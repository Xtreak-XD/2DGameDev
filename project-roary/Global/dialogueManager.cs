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
    }

    public async Task startDialog(Vector2 position, string[] lines)
    {
        if (isDialogActive)
        {
            return;
        }

        dialogLines = lines;
        textBoxPosition = position;
        await showTextBox();

        isDialogActive = true;
    }

    public async Task showTextBox()
    {
        textBox = (TextBox)textBoxScene.Instantiate();
        eventbus.finishedDisplaying += onTextBoxFinishedDisplaying;
        GetTree().Root.AddChild(textBox);
        textBox.GlobalPosition = textBoxPosition;
        await textBox.displayText(dialogLines[currentLineIndex]);
        canAdvanceLine = false;
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

    private async void HandleDialogAdvance()
    {
        textBox.QueueFree();
        currentLineIndex++;

        if (currentLineIndex >= dialogLines.Length)
        {
            isDialogActive = false;
            currentLineIndex = 0;
            eventbus.EmitSignal("finishedDisplaying");
            return;
        }

        await showTextBox();
    }

    public override void _ExitTree()
    {
        eventbus.finishedDisplaying -= onTextBoxFinishedDisplaying;
    }

}
