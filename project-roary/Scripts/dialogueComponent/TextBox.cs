using Godot;
using System;
using System.Threading.Tasks;

public partial class TextBox : MarginContainer
{
    Timer timer;
    Label textLabel;

    const int MAX_WIDTH = 256*4;
    string textToShow = "";
    int letterIndex = 0;

    double letterTime = 0.03;
    double spaceTime = 0.06;
    double punctuationTime = 0.2;


    Eventbus eventbus;

    public override void _Ready()
    {
        eventbus = GetNode<Eventbus>("/root/Eventbus");
        timer = GetNode<Timer>("letterDisplayTimer");
        textLabel = GetNode<Label>("MarginContainer/Label");

        timer.Timeout += onTimeOut;
    }


    public async Task displayText(string textToDisplay)
    {
        textToShow = textToDisplay;
        textLabel.Text = textToDisplay;

        await ToSignal(this, Control.SignalName.Resized);
        CustomMinimumSize = new Vector2(Mathf.Min(Size.X, MAX_WIDTH), CustomMinimumSize.Y);

        if (Size.X > MAX_WIDTH)
        {
            textLabel.AutowrapMode = TextServer.AutowrapMode.Word;
            await ToSignal(this, Control.SignalName.Resized);
            await ToSignal(this, Control.SignalName.Resized);
            CustomMinimumSize = new Vector2(CustomMinimumSize.X, Size.Y);
        }
        GlobalPosition = GlobalPosition + new Vector2(-(Size.X / 2), -600);

        textLabel.Text = "";
        displayLetter();
    }

    public void displayLetter()
    {
        textLabel.Text += textToShow[letterIndex];

        letterIndex++;
        if (letterIndex >= textToShow.Length)
        {
            eventbus.EmitSignal("finishedDisplaying");
            return;
        }

        switch (textToShow[letterIndex])
        {
            case '!' or '.' or ',' or '?':
                timer.Start(punctuationTime);
                break;
            case ' ':
                timer.Start(spaceTime);
                break;
            default:
                timer.Start(letterTime);
                break;
        }
    }

    public void onTimeOut()
    {
        displayLetter();
    }
}
