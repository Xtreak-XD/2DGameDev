using Godot;
using System;
using System.Threading.Tasks;

public partial class Guide : CharacterBody2D
{
    public interactionArea interactionArea;
    public dialogueManager dialogueManager;
    public Eventbus eventbus;

    public SaveManager saveManager;

    public Godot.Collections.Dictionary<string,string[]> Lines;

    public override void _Ready()
    {
        eventbus = GetNode<Eventbus>("/root/Eventbus");
        dialogueManager = GetNode<dialogueManager>("/root/DialogueManager");
        interactionArea = GetNode<interactionArea>("InteractableArea");
        saveManager = GetNode<SaveManager>("/root/SaveManager");
        interactionArea.interact = Callable.From(onInteractWrapper);

        BuildLinesDictionary();
    }

    void BuildLinesDictionary() //flag, string lines
    {
        Lines = new Godot.Collections.Dictionary<string,string[]>
        {
            {
                "default",
                new string[]
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
                }
            },
            {
                "TalkedToWiseTurtleAboutBrother",
                new string[]
                {
                    "Ah, we meet again.",
                    "I trust your search for my brother is still ongoing.",
                    "Listen carefully, something strange is happening around the nature preserve..."
                }
            },
            {
                "SavedYoungerTurtleBrother",
                new string[]
                {
                    "You saved my brother!",
                    "Words cannot express my gratitude.",
                    "You must continue forward — the mission is not over."
                }
            }
        };
    }

    public void onInteractWrapper()
    {
        _ = onInteract(); // call async function without returning Task
    }

    public async Task onInteract()
    {
        var sm = saveManager;
        var md = sm.metaData;

        string[] chosenLines;

        if (md.SavedYoungerTurtleBrother)
        {
            chosenLines = Lines["SavedYoungerTurtleBrother"];
        }
        else if (md.TalkedToWiseTurtleAboutBrother)
        {
            chosenLines = Lines["TalkedToWiseTurtleAboutBrother"];
        }
        else
        {
            chosenLines = Lines["default"];
        }

        await dialogueManager.startDialog(GlobalPosition, chosenLines);
        await ToSignal(eventbus, "finishedDisplaying");

        if (!md.TalkedToWiseTurtleAboutBrother)
        {
            md.TalkedToWiseTurtleAboutBrother = true;
            sm.Save();
        }

        eventbus.EmitSignal(Eventbus.SignalName.interactionComplete);
    }
}