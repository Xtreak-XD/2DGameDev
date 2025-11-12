using System;
using Godot;
using System.Threading.Tasks;

public partial class StarbucksNPC : CharacterBody2D
{
    
    [Export] public Vector2[] PatrolPoints;        // The positions making up the patrol route
    [Export] public float MoveSpeed = 80f;         // Movement speed
    [Export] public float PauseTime = 1.5f;        // Pause duration at the end of the route
    [Export] public String Npc;
    public string[][] DialogueLines = new string[][]
    {
        new string[] { "Hello there!", "Nice weather today." },
        new string[] { "Did you see that?", "Quite exciting, isn't it?" },
        new string[] { "Goodbye for now!" }
    };  // The dialogue this NPC will say
    public Eventbus eventbus;

    private int _currentPoint = 0;
    private bool _isPaused = false;
    private float _pauseTimer = 0f;
    public interactionArea interactionArea;
    public dialogueManager dialogueManager;
    public bool _isInteracting = false;
    

    public override void _Ready()
    {
        if (PatrolPoints.Length == 0)
        {
            GD.PrintErr("NPC PatrolPoints not set!");
            SetProcess(false);
        }
        else
        {
            GlobalPosition = PatrolPoints[0];
        }
        eventbus = GetNode<Eventbus>("/root/Eventbus");
        dialogueManager = GetNode<dialogueManager>("/root/DialogueManager");
        interactionArea = GetNode<interactionArea>("InteractableArea");
        interactionArea.interact = Callable.From(ShowDialogue);
    }

    public override void _PhysicsProcess(double deltaDouble)
    {
        float delta = (float)deltaDouble;

        if (_isInteracting)
        {
            Velocity = Vector2.Zero;
            MoveAndSlide();
            return;
        }
        if (_isPaused)
        {
            _pauseTimer -= delta;
            if (_pauseTimer <= 0f)
            {
                _isPaused = false;
                _currentPoint = (_currentPoint + 1) % PatrolPoints.Length;
            }
            else
            {
                Velocity = Vector2.Zero;
                MoveAndSlide();
                return;
            }
        }

        Vector2 target = PatrolPoints[_currentPoint];
        Vector2 toTarget = target - GlobalPosition;

        if (toTarget.Length() < 5f)
        {
            _isPaused = true;
            _pauseTimer = PauseTime;
            Velocity = Vector2.Zero;
            MoveAndSlide();
        }
        else
        {
            Velocity = toTarget.Normalized() * MoveSpeed;
            MoveAndSlide();
        }
    }

    // Call this to display the dialogue (e.g., on player interact)
    public async Task ShowDialogue()
    {
        _isInteracting = true;
        Random random = new Random();
        int randomNumber = random.Next(DialogueLines.Length);
        GD.Print("onInteractTest");
        dialogueManager.startDialog(GlobalPosition, DialogueLines[randomNumber]);
        await ToSignal(eventbus, "finishedDisplaying");
        await ToSignal(GetTree().CreateTimer(3), Timer.SignalName.Timeout);
        _isInteracting = false;
    }
}
