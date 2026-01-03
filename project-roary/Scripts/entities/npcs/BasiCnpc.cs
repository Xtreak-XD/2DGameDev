using Godot;
using System;
using System.Threading.Tasks;

public partial class BasiCnpc : CharacterBody2D
{

    public interactionArea interactionArea;
    public dialogueManager dialogueManager;
    public Eventbus eventbus;
    public AnimatedSprite2D animatedSprite2D;

    public int speed = 50;
    public Vector2 direction = Vector2.Zero;

    enum STATE
    {
        walking,
        talking,
    }

    STATE currentState = STATE.walking;
    enum NPC
    {
        one = 1,
        two,
        three,
        four,
        five,
    }

    NPC typeOfNPC;

    double timer = 6;

    readonly String[][] dialogues =
    [
        ["sorry I got class, move!"],
        ["Wassup!"],
        ["ew..."],
        ["I am so tired."],
        ["I got like 5 tests today!"],
        ["What a beautiful day, too bad there are so many lizards!"],
        ["Wow, those petitioners sure are annoying!"],
        ["Did you see? The alligators are loose in the Nature Preserve."],
        ["I wonder what's been going on with roary??"],
        ["I want to go homeeeee!"],
        ["damn, you ugly twin..."],
        ["HOLLOW PURPLE!...I swear someone said that."],
        ["That staple gun is so OP"],
        ["I swear I am going to get a least a 60 this next test!"],
        ["I guess I am not eating today..."],
        ["I ain't got any money left and it's still..."],
        ["My. birthday is coming up, what are you getting me stranger?"],
        ["You're a first year aren't you? hahaha"],
        ["Leave me alone weirdo!"],
        ["What's your problem talking to everyone?"],
        ["Have to go!"],
        ["6777777777"],
        ["get outtttt!!"],
        ["I am gonna be king of the pirates!"],
        ["One Piece is the greatest animae of all time!"],
    ];
    string[] chosenDialogue;
    RandomNumberGenerator rand;
    Vector2 cardinalDirection;
    Vector2 currentDestination;
    Timer wait;

    public override void _Ready()
    {
        eventbus = GetNode<Eventbus>("/root/Eventbus");
        dialogueManager = GetNode<dialogueManager>("/root/DialogueManager");
        interactionArea = GetNode<interactionArea>("InteractableArea");
        interactionArea.interact = Callable.From(onInteractWrapper);
        animatedSprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
        
        rand = new RandomNumberGenerator();
        typeOfNPC = (NPC)rand.RandiRange(1,5);
        GD.Print(typeOfNPC);

        wait = new()
        {
            WaitTime = 3,
            OneShot = true
        };
        AddChild(wait);
    }

    public override void _PhysicsProcess(double delta)
    {
        Velocity = direction.Normalized() * speed;
        MoveAndSlide();
    }

    public override void _Process(double delta)
    {
        if (currentState == STATE.talking)
        {
            direction = Vector2.Zero;
            updateAnimation();
            return;
        }
        if(currentDestination == Vector2.Zero || GlobalPosition == currentDestination)
        {
            currentDestination = ChoosePoint();
        }
        else
        {
            direction = GlobalPosition - currentDestination;
        }

        if (timer <= 0)
        {
            currentDestination = ChoosePoint();
            timer = 6;
        }

        timer -= delta;

        if (SetDirection())
        {
            updateAnimation();
        }
    }

    Vector2 ChoosePoint()
    {
        Vector2 currentPos = GlobalPosition;
        int randX = rand.RandiRange((int)currentPos.X-25,(int)currentPos.X+25);
        int randY = rand.RandiRange((int)currentPos.Y-25,(int)currentPos.Y+25);

        Vector2 newPos = new(randX, randY);
        return newPos;
    }

    public void onInteractWrapper()
    {
        _ = onInteract();
    }

    public async Task onInteract()
    {
        chosenDialogue = dialogues[rand.RandiRange(0,dialogues.Length - 1)];
        wait.Start();
        currentState = STATE.talking;

        await dialogueManager.startDialog(GlobalPosition, chosenDialogue);
        await ToSignal(eventbus, "finishedDisplaying");
        eventbus.EmitSignal(Eventbus.SignalName.interactionComplete);

        await ToSignal(wait, "timeout");
        currentState = STATE.walking;
        updateAnimation();
    }

    async void updateAnimation()
    {
        if(currentState == STATE.walking)
        {
            animatedSprite2D.Play("npc" + (int)typeOfNPC + "_" + updateDirection());
        }
        else
        {
            animatedSprite2D.Play("npc" + (int)typeOfNPC + "_" + "down");
            animatedSprite2D.Stop();
            animatedSprite2D.Play();
        }
    }

    string updateDirection()
    {
        if (Mathf.Abs(cardinalDirection.X) > Mathf.Abs(cardinalDirection.Y))
        {
            return cardinalDirection.X > 0 ? "right" : "left";
        }
        else
        {
            return cardinalDirection.Y > 0 ? "down" : "up";
        }
    }
    public bool SetDirection()
	{
		if (direction == Vector2.Zero){ return false;}
		Vector2 new_dir;

		if (Mathf.Abs(direction.X) > Mathf.Abs(direction.Y))
		{
			new_dir = direction.X > 0 ? Vector2.Right : Vector2.Left;
		}
		else
		{
			new_dir = direction.Y > 0 ? Vector2.Down : Vector2.Up;
		}

		if (new_dir == cardinalDirection){ return false; }

		cardinalDirection = new_dir;

		return true;
    }
}
